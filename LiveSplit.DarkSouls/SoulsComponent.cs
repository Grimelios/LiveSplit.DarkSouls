using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using LiveSplit.DarkSouls.Controls;
using LiveSplit.DarkSouls.Data;
using LiveSplit.DarkSouls.Memory;
using LiveSplit.Model;
using LiveSplit.UI;
using LiveSplit.UI.Components;
using Enum = System.Enum;

namespace LiveSplit.DarkSouls
{
	public class SoulsComponent : IComponent
	{
		public const string DisplayName = "Dark Souls Autosplitter";

		private TimerModel timer;
		private SplitCollection splitCollection;
		private SoulsMemory memory;
		private SoulsMasterControl masterControl;
		private Dictionary<SplitTypes, Func<int[], bool>> splitFunctions;
		private RunState run;

		private bool preparedForWarp;

		public SoulsComponent()
		{
			splitCollection = new SplitCollection();
			memory = new SoulsMemory();
			masterControl = new SoulsMasterControl();
			run = new RunState();

			splitFunctions = new Dictionary<SplitTypes, Func<int[], bool>>
			{
				{ SplitTypes.Bonfire, ProcessBonfire },
				{ SplitTypes.Boss, ProcessBoss },
				{ SplitTypes.Covenant, ProcessCovenant },
				{ SplitTypes.Events, ProcessEvent }
			};
		}

		public string ComponentName => DisplayName;

		public float HorizontalWidth => 0;
		public float MinimumHeight => 0;
		public float VerticalHeight => 0;
		public float MinimumWidth => 0;
		public float PaddingTop => 0;
		public float PaddingBottom => 0;
		public float PaddingLeft => 0;
		public float PaddingRight => 0;

		public IDictionary<string, Action> ContextMenuControls => null;

		public void DrawHorizontal(Graphics g, LiveSplitState state, float height, Region clipRegion)
		{
		}

		public void DrawVertical(Graphics g, LiveSplitState state, float width, Region clipRegion)
		{
		}

		public Control GetSettingsControl(LayoutMode mode)
		{
			return masterControl;
		}

		public XmlNode GetSettings(XmlDocument document)
		{
			XmlElement root = document.CreateElement("Settings");
			XmlElement igtElement =
				document.CreateElementWithInnerText("UseGameTime", masterControl.UseGameTime.ToString());
			XmlElement splitsElement = document.CreateElement("Splits");

			var splits = masterControl.CollectionControl.ExtractSplits();
			splitCollection.Splits = splits;

			// Splits can be null if the user hasn't added any splits through the LiveSplit UI.
			if (splits != null)
			{
				foreach (var split in splits)
				{
					var data = split.Data;
					var dataString = data != null ? string.Join("|", data) : "";

					XmlElement splitElement = document.CreateElement("Split");
					splitElement.AppendChild(document.CreateElementWithInnerText("Type", split.Type.ToString()));
					splitElement.AppendChild(document.CreateElementWithInnerText("Data", dataString));
					splitsElement.AppendChild(splitElement);
				}
			}

			root.AppendChild(igtElement);
			root.AppendChild(splitsElement);

			return root;
		}

		public void SetSettings(XmlNode settings)
		{
			bool useGameTime = bool.Parse(settings["UseGameTime"].InnerText);

			XmlNodeList splitNodes = settings["Splits"].GetElementsByTagName("Split");
			Split[] splits = new Split[splitNodes.Count];

			for (int i = 0; i < splitNodes.Count; i++)
			{
				var splitNode = splitNodes[i];
				var type = (SplitTypes)Enum.Parse(typeof(SplitTypes), splitNode["Type"].InnerText);

				string rawData = splitNode["Data"].InnerText;

				int[] data = null;

				if (rawData.Length > 0)
				{
					string[] dataTokens = splitNode["Data"].InnerText.Split('|');

					data = new int[dataTokens.Length];

					for (int j = 0; j < dataTokens.Length; j++)
					{
						data[j] = int.Parse(dataTokens[j]);
					}
				}

				splits[i] = new Split(type, data);
			}

			splitCollection.Splits = splits;
			masterControl.Refresh(splits, useGameTime);
		}

		public void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode)
		{
			if (!Hook())
			{
				return;
			}

			if (timer == null)
			{
				timer = new TimerModel();
				timer.CurrentState = state;

				state.OnStart += (sender, args) =>
				{
					splitCollection.OnStart();
					UpdateRunState();
				};

				state.OnSplit += (sender, args) =>
				{
					splitCollection.OnSplit();
					UpdateRunState();
				};

				state.OnUndoSplit += (sender, args) => { splitCollection.OnUndoSplit(); };
				state.OnSkipSplit += (sender, args) => { splitCollection.OnSkipSplit(); };
				state.OnReset += (sender, value) => { splitCollection.OnReset(); };
			}

			var phase = state.CurrentPhase;

			if (phase == TimerPhase.NotRunning || phase == TimerPhase.Ended)
			{
				return;
			}

			if (phase == TimerPhase.Running)
			{
				Refresh();
			}

			if (masterControl.UseGameTime)
			{
				int gameTime = memory.GetGameTimeInMilliseconds();
				int runTime = run.MaxGameTime;
				int previousTime = run.GameTime;

				// This condition is only possible during a run when game time isn't increasing (game time resets to
				// zero on the main menu).
				bool pause = gameTime == 0 && previousTime > 0;
				bool unpause = previousTime == 0 && gameTime > 0;

				if (pause && phase == TimerPhase.Running)
				{
					timer.Pause();
					state.IsGameTimePaused = true;
				}
				else if (unpause && phase == TimerPhase.Paused)
				{
					timer.UndoAllPauses();
					state.IsGameTimePaused = false;
				}

				int max = Math.Max(gameTime, runTime);

				state.SetGameTime(TimeSpan.FromMilliseconds(max));
				run.GameTime = gameTime;
				run.MaxGameTime = max;
			}
		}

		private bool Hook()
		{
			bool previouslyHooked = memory.ProcessHooked;

			if (memory.Hook())
			{
				if (!previouslyHooked)
				{
					Console.WriteLine("Process hooked.");
				}
			}
			else if (previouslyHooked)
			{
				Console.WriteLine("Process unhooked.");
			}

			return memory.ProcessHooked;
		}

		public void Refresh()
		{
			if (!Hook())
			{
				return;
			}

			Split split = splitCollection.CurrentSplit;

			var type = split.Type;

			if (type == SplitTypes.Manual)
			{
				return;
			}

			if (splitFunctions[split.Type](split.Data))
			{
				// Timer is null when testing the program from the testing class.
				timer?.Split();
			}
		}

		private void UpdateRunState()
		{
			Split split = splitCollection.CurrentSplit;

			int[] data = split.Data;

			switch (split.Type)
			{
				case SplitTypes.Bonfire:
					//run.BonfireFlag = GetEnumValue<BonfireFlags>(data[0]);
					run.BonfireState = memory.GetBonfireState(run.BonfireFlag);

					int bonfireCriteria = data[1];

					if (bonfireCriteria != 1)
					{
						//run.TargetBonfireState = GetEnumValue<BonfireStates>(bonfireCriteria);
					}

					break;

				case SplitTypes.Boss:
					run.BossFlag = Flags.OrderedBosses[data[0]];
					run.IsBossDefeated = memory.CheckFlag(run.BossFlag);

					break;

				case SplitTypes.Covenant:
					run.Data = (int)memory.GetCovenant();
					run.Target = Flags.OrderedCovenants[data[0]];

					break;

				case SplitTypes.Events:
					switch ((WorldEvents)data[0])
					{
						case WorldEvents.Bell1:
							run.Id = (int)BellFlags.FirstBell;
							run.Flag = memory.CheckFlag(run.Id);

							break;

						case WorldEvents.Bell2:
							run.Id = (int)BellFlags.SecondBell;
							run.Flag = memory.CheckFlag(run.Id);

							break;

						default: run.Data = memory.GetClearCount(); break;
					}

					//run.Target = 

					break;

				case SplitTypes.Item:
					break;

				case SplitTypes.Zone:
					break;
			}
		}

		private bool ProcessBonfire(int[] data)
		{
			bool onRest = data[1] == 1;

			int target = Flags.OrderedBonfires[data[0]];

			if (onRest)
			{
				int[] restValues =
				{
					(int)AnimationFlags.BonfireSit1,
					(int)AnimationFlags.BonfireSit2,
					(int)AnimationFlags.BonfireSit3
				};

				int animation = memory.GetForcedAnimation();

				// This confirms that the player is resting at a bonfire, but not which bonfire.
				if (restValues.Contains(animation))
				{
					int bonfire = memory.GetLastBonfire();

					if (Enum.IsDefined(typeof(BonfireFlags), bonfire))
					{
						return bonfire == target;
					}
				}

				return false;
			}

			BonfireStates previousState = run.BonfireState;
			BonfireStates state = memory.GetBonfireState(run.BonfireFlag);

			run.BonfireState = state;

			return state != previousState && state == run.TargetBonfireState;
		}

		private bool ProcessBoss(int[] data)
		{
			bool previouslyDefeated = run.IsBossDefeated;
			bool onVictory = data[1] == 0;
			bool defeated = memory.CheckFlag(run.BossFlag);

			run.IsBossDefeated = defeated;

			if (onVictory)
			{
				if (defeated && !previouslyDefeated)
				{
					return true;
				}
			}
			// The alternative to splitting on victory is splitting on the first warp after victory.
			else if (defeated)
			{
			}

			return false;
		}

		private bool ProcessCovenant(int[] data)
		{
			bool onJoin = data[1] == 1;

			if (onJoin)
			{
				int covenant = (int)memory.GetCovenant();

				if (covenant != run.Data)
				{
					run.Data = covenant;

					if (covenant == run.Target)
					{
						return true;
					}
				}
			}

			return false;
		}

		private bool ProcessEvent(int[] data)
		{
			bool isBell = data[0] <= 2;

			return isBell ? ProcessBell(data) : ProcessEnding(data);
		}

		private bool ProcessBell(int[] data)
		{
			bool rung = memory.CheckFlag(run.Id);

			if (rung && !run.Flag)
			{
				run.Flag = true;

				bool onRing = data[1] == 0;

				if (onRing)
				{
					return true;
				}

				preparedForWarp = true;
			}

			return false;
		}

		private bool ProcessEnding(int[] data)
		{
			int clearCount = memory.GetClearCount();

			if (clearCount > run.Data)
			{
				run.Data = clearCount;

				// The player's X coordinate increases as you approach the exit (the exit is at roughly 421).
				bool isDarkLord = memory.GetPlayerX() > 415;
				bool isDarkLordTarget = data[0] == 5;

				return isDarkLord == isDarkLordTarget;
			}

			return false;
		}

		public void Dispose()
		{
		}
	}
}