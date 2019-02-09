using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
		
		public SoulsComponent()
		{
			splitCollection = new SplitCollection();
			memory = new SoulsMemory();
			masterControl = new SoulsMasterControl();
			run = new RunState();

			splitFunctions = new Dictionary<SplitTypes, Func<int[], bool>>
			{
				{ SplitTypes.Bonfire, ProcessBonfire },
				{ SplitTypes.Boss, ProcessBoss }
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
			XmlElement igtElement = document.CreateElementWithInnerText("UseGameTime", masterControl.UseGameTime.ToString());
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
			if (timer == null)
			{
				timer = new TimerModel();
				timer.CurrentState = state;

				state.OnSplit += (sender, args) => { OnSplit(); };
				state.OnUndoSplit += (sender, args) => { splitCollection.OnUndoSplit(); };
				state.OnSkipSplit += (sender, args) => { splitCollection.OnSkipSplit(); };
				state.OnReset += (sender, value) => { splitCollection.OnReset(); };
			}

			var phase = state.CurrentPhase;

			if (phase == TimerPhase.NotRunning || phase == TimerPhase.Ended)
			{
				return;
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

			if (phase == TimerPhase.Running)
			{
				Refresh();
			}
		}

		public void Refresh()
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

			Split split = splitCollection.CurrentSplit;

			if (splitFunctions[split.Type](split.Data))
			{
				// Timer is null when testing the program from the testing class.
				timer?.Split();
			}
		}

		private void OnSplit()
		{
			splitCollection.OnSplit();

			Split split = splitCollection.CurrentSplit;

			int[] data = split.Data;

			switch (split.Type)
			{
				case SplitTypes.Bonfire:
					run.BonfireFlag = GetEnumValue<BonfireFlags>(data[0]);
					run.BonfireState = memory.GetBonfireState(run.BonfireFlag);

					int bonfireCriteria = data[1];

					if (bonfireCriteria != 1)
					{
						run.TargetBonfireState = GetEnumValue<BonfireStates>(bonfireCriteria);
					}

					break;

				case SplitTypes.Boss:
					run.BossFlag = GetEnumValue<BossFlags>(data[0]);
					run.BossDefeated = memory.IsBossDefeated(run.BossFlag);

					break;

				case SplitTypes.Covenant:
					break;

				case SplitTypes.Events:
					break;

				case SplitTypes.Item:
					break;

				case SplitTypes.Zone:
					break;
			}
		}

		private T GetEnumValue<T>(int index)
		{
			return (T)Enum.GetValues(typeof(T)).GetValue(index);
		}

		private bool ProcessBonfire(int[] data)
		{
			bool onRest = data[1] == 1;

			if (onRest)
			{
			}

			BonfireStates previousState = run.BonfireState;
			BonfireStates state = memory.GetBonfireState(run.BonfireFlag);

			run.BonfireState = state;

			return state != previousState && state == run.TargetBonfireState;
		}

		private bool ProcessBoss(int[] data)
		{
			bool previouslyDefeated = run.BossDefeated;
			bool onVictory = data[1] == 0;
			bool defeated = memory.IsBossDefeated(run.BossFlag);

			run.BossDefeated = defeated;

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

		public void Dispose()
		{
		}
	}
}
