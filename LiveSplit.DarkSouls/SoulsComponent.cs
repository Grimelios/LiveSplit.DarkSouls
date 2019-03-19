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

namespace LiveSplit.DarkSouls
{
	public class SoulsComponent : IComponent
	{
		public const string DisplayName = "Dark Souls Autosplitter";

		private const int EstusId = 200;

		private TimerModel timer;
		private SplitCollection splitCollection;
		private SoulsMemory memory;
		private SoulsMasterControl masterControl;
		private Dictionary<SplitTypes, Func<int[], bool>> splitFunctions;
		private Dictionary<Zones, List<Zone>> zoneMap;
		private Vector3[] covenantLocations;
		private Vector3[] bonfireLocations;
		private RunState run;

		private bool preparedForWarp;
		private bool isLoadScreenVisible;
		private bool waitingOnQuitout;
		private bool waitingOnUnload;
		private bool waitingOnReload;

		// This variable tracks whether the player confirmed a warp from a bonfire prompt. The data used to detect this
		// state (beginning a bonfire warp) doesn't persist up to the loading screen's appearance, so it needs to be
		// tracked separately.
		private bool isBonfireWarpConfirmed;

		// Most warp splits detect an event, then wait for a warp to occur. Bonfire and item warps are unique in that
		// they can be undone once activated (by leaving the target bonfire or losing the target item).
		private bool isBonfireWarpSplitActive;
		private bool isItemWarpSplitActive;

		// The estus flask is more complex than other items. Conceptually, you have a single estus flask (either empty
		// or filled) that can be upgraded to a maximum of +7. Internally, though, each separate flask has its own ID
		// (16 total). Some special code is required to deal with that fact.
		private bool isEstusSplit;

		// This value is used to bump estus IDs up to their correct value (from the base, unfilled +0 ID). For estus
		// splits, reinforcement can't be stored in the item target directly because it would mess with state
		// comparison.
		private int estusReinforcement;

		// If a particular run doesn't ever split on items, it would be wasteful to track them.
		private bool itemsEnabled;

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
				{ SplitTypes.Event, ProcessEvent },
				{ SplitTypes.Flag, ProcessFlag },
				{ SplitTypes.Item, ProcessItem },
				{ SplitTypes.Quitout, ProcessQuitout },
				{ SplitTypes.Zone, ProcessZone }
			};

			// This array is used for covenant discovery splits. Discovery occurs when the player is prompted to join a
			// covenant via a yes/no confirmation box. That prompt's appearance can be detected through memory, but
			// it's shared among all covenants. As such, position is used to narrow down the covenant.
			covenantLocations = new []
			{
				new Vector3(-28, -53, 87), // Way of White (in front of Petrus)
				new Vector3(9, 29, 121), // Way of White (beside Rhea) 
				new Vector3(622, 164, 255), // Princess (in front of Gwynevere)
				new Vector3(36, 12, -32), // Sunlight (in front of the sunlight altar) 
				new Vector3(93, -311, 4), // Darkwraith (in front of Kaathe)
				new Vector3(-702, -412, -333), // Dragon (in front of the everlasting dragon)
				new Vector3(-161, -265, -32), // Gravelord (below Nito)
				new Vector3(285, -3, -105), // Forest (below Alvina) 
				new Vector3(430, 60, 255), // Darkmoon (just outside Gwyndolin's boss arena)
				new Vector3(138, -252, 94) // Chaos (in front of the fair lady)
			};

			// Previously, bonfire resting was determined by reading the last bonfire from memory (i.e. the bonfire to
			// which the player will warp on death or when using an item). That approach works great for most cases,
			// but there's a catch in that the last bonfire seems to update a frame after the resting animation is
			// detected. As a result, it was possible for the autosplitter to split incorrectly if that last bonfire
			// value is already set to the target (in which case you'd split at any bonfire, not just the target).
			// There's unfortunately no foolproof solution to this problem using that last bonfire data alone, which is
			// why position data is used instead.
			//
			// Also note that each position is taken from the player standing right next to the bonfire (rounded to the
			// nearest integer). Positions don't need to be exact (just close enough that, on rest, the target bonfire
			// will be the closest).
			bonfireLocations = new []
			{
				new Vector3(171, 173, 255), // Anor Londo - Entrance
				new Vector3(504, 135, 175), // Anor Londo - Interior
				new Vector3(593, 161, 254), // Anor Londo - Princess
				new Vector3(391, 70, 255), // Anor Londo - Tomb
				new Vector3(-388, -408, 156), // Ash Lake - Entrance
				new Vector3(-700, -414, -323), // Ash Lake - Dragon
				new Vector3(-277, -137, 73), // Blighttown - Bridge
				new Vector3(-198, -215, 100), // Blighttown - Swamp
				new Vector3(44, -119, 204), // Catacombs - Entrance
				new Vector3(48, -112, 301), // Catacombs - Illusion
				new Vector3(854, -577, 849), // Chasm of the Abyss
				new Vector3(96, 134, 864), // Crystal Caves
				new Vector3(165, -77, -55), // Darkroot Basin
				new Vector3(257, -3, -12), // Darkroot Garden
				new Vector3(141, -253, 94), // Daughter of Chaos
				new Vector3(253, -334, 22), // Demon Ruins - Central
				new Vector3(194, -267, 130), // Demon Ruins - Entrance
				new Vector3(118, -357, 139), // Demon Ruins - Firesage
				new Vector3(349, 278, 594), // Duke's Archives - Balcony
				new Vector3(230, 200, 481), // Duke's Archives - Entrance
				new Vector3(378, 270, 552), // Duke's Archives - Prison
				new Vector3(52, -64, 106), // Firelink Altar
				new Vector3(-51, -61, 55), // Firelink Shrine
				new Vector3(-318, -236, 123), // Great Hollow
				new Vector3(581, -444, 444), // Lost Izalith - Bed of Chaos
				new Vector3(456, -380, 170), // Lost Izalith - Illusion
				new Vector3(229, -384, 91), // Lost Izalith - Lava Field
				new Vector3(972, -314, 583), // Oolacile Sanctuary
				new Vector3(863, -448, 912), // Oolacile Township - Dungeon
				new Vector3(1041, -332, 875), // Oolacile Township - Entrance
				new Vector3(-24, 52, 944), // Painted World
				new Vector3(897, -329, 452), // Sanctuary Garden
				new Vector3(73, 60, 301), // Sen's Fortress
				new Vector3(85, -311, 3), // The Abyss
				new Vector3(-121, -74, 13), // The Depths
				new Vector3(77, -214, 45), // Tomb of the Giants - Alcove
				new Vector3(-159, -265, -34), // Tomb of the Giants - Nito
				new Vector3(97, -200, 104), // Tomb of the Giants - Patches
				new Vector3(3, 196, 7), // Undead Asylum - Courtyard
				new Vector3(34, 193, -26), // Undead Asylum - Interior
				new Vector3(3, -10, -61), // Undead Burg
				new Vector3(88, 15, 107), // Undead Parish - Andre
				new Vector3(24, 10, -23) // Undead Parish - Sunlight
			};

			Zone abyss = new Zone(16, 0);
			Zone anorLondo = new Zone(15, 1);
			Zone asylum = new Zone(18, 1);
			Zone basin = new Zone(12, 0);
			Zone firelinkAltar = new Zone(18, 0);
			Zone firelinkShrine = new Zone(10, 2);
			Zone paintedWorld = new Zone(11, 0);
			Zone sanctuaryGarden = new Zone(12, 1);
			Zone sensRoof = new Zone(15, 0);

			// Since zones are designed to capture transitions between distinct areas, each zone (from the enumeration)
			// maps to both its associated zone (the first item in the list) and a list of neighboring zones (the
			// remaining items).
			zoneMap = new Dictionary<Zones, List<Zone>>
			{
				{ Zones.AnorLondo, new List<Zone> { anorLondo, paintedWorld, sensRoof }},
				{ Zones.FirelinkAltar, new List<Zone> { firelinkAltar, abyss, firelinkShrine }},
				{ Zones.FirelinkShrine, new List<Zone> { firelinkShrine, asylum, firelinkAltar }},
				{ Zones.PaintedWorld, new List<Zone> { paintedWorld, anorLondo }},
				{ Zones.SanctuaryGarden, new List<Zone> { sanctuaryGarden, basin }},
				{ Zones.SensFortressRoof, new List<Zone> { sensRoof, anorLondo }},
				{ Zones.TheAbyss, new List<Zone> { abyss, firelinkAltar }},
				{ Zones.UndeadAsylum, new List<Zone> { asylum, firelinkShrine }}
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
			XmlElement versionElement =
				document.CreateElementWithInnerText("Version", Utilities.GetVersion().ToString());
			XmlElement igtElement =
				document.CreateElementWithInnerText("UseGameTime", masterControl.UseGameTime.ToString());
			XmlElement resetElement =
				document.CreateElementWithInnerText("ResetEquipment", masterControl.ResetEquipmentIndexes.ToString());
			XmlElement autostartElement =
				document.CreateElementWithInnerText("TimerAutostart", masterControl.StartTimerAutomatically.ToString());
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

			root.AppendChild(versionElement);
			root.AppendChild(igtElement);
			root.AppendChild(resetElement);
			root.AppendChild(autostartElement);
			root.AppendChild(splitsElement);

			return root;
		}

		public void SetSettings(XmlNode settings)
		{
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

			var versionElement = settings["Version"];

			// The version element itself wasn't present in the original 1.0.0 release.
			string fileVersion = versionElement == null ? "1.0.0" : settings["Version"].InnerText;

			if (Utilities.GetVersion() != Version.Parse(fileVersion))
			{
				VersionHelper.Convert(splits, fileVersion);
			}

			splitCollection.Splits = splits;

			bool useGameTime = bool.Parse(settings["UseGameTime"].InnerText);
			bool resetEquipment = bool.Parse(settings["ResetEquipment"].InnerText);
			bool autostart = bool.Parse(settings["TimerAutostart"].InnerText);

			masterControl.UseGameTime = useGameTime;
			masterControl.ResetEquipmentIndexes = resetEquipment;
			masterControl.StartTimerAutomatically = autostart;
			masterControl.Refresh(splits);
		}

		public void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode)
		{
			if (timer == null)
			{
				timer = new TimerModel();
				timer.CurrentState = state;

				state.OnStart += (sender, args) => { OnStart(); };

				state.OnSplit += (sender, args) =>
				{
					splitCollection.OnSplit();
					UpdateRunState();
				};

				state.OnUndoSplit += (sender, args) =>
				{
					splitCollection.OnUndoSplit();
					UpdateRunState();
				};

				state.OnSkipSplit += (sender, args) =>
				{
					splitCollection.OnSkipSplit();
					UpdateRunState();
				};

				state.OnReset += (sender, value) => { OnReset(); };
			}

			Refresh(state.CurrentPhase);
		}

		private void OnStart()
		{
			var splits = splitCollection.Splits;

			itemsEnabled = splits.Any(s => s.Type == SplitTypes.Item);

			if (itemsEnabled)
			{
				List<ItemId> items = new List<ItemId>();

				foreach (Split split in splits)
				{
					if (split.Type != SplitTypes.Item || !split.IsFinished)
					{
						continue;
					}

					ItemId id = ComputeItemId(split);

					// Given the nature of how estus IDs are stored, both the filled and unfilled versions of
					// the flask (at the target reinforcement) must be tracked.
					if (id.BaseId == EstusId)
					{
						int reinforcement = split.Data[3];

						id.BaseId += reinforcement * 2;

						ItemId filledId = new ItemId(id.BaseId + 1, id.Category);

						items.Add(filledId);
					}

					items.Add(id);
				}

				memory.SetItems(items);
			}

			splitCollection.OnStart();
			UpdateRunState();
		}

		private void OnReset()
		{
			if (memory.ProcessHooked && masterControl.ResetEquipmentIndexes)
			{
				memory.ResetEquipmentIndexes();
			}

			// Other run state values don't need to be reset (since they're always properly set before becoming
			// relevant).
			run.GameTime = 0;
			run.MaxGameTime = 0;

			splitCollection.OnReset();
		}

		private void UpdateRunState()
		{
			Split split = splitCollection.CurrentSplit;

			if (split == null || split.Type == SplitTypes.Manual || !split.IsFinished)
			{
				return;
			}

			preparedForWarp = false;
			waitingOnQuitout = false;
			waitingOnUnload = false;
			waitingOnReload = false;
			isBonfireWarpConfirmed = false;
			isBonfireWarpSplitActive = false;
			isItemWarpSplitActive = false;
			isEstusSplit = false;

			int[] data = split.Data;

			switch (split.Type)
			{
				case SplitTypes.Bonfire:
					int bonfireIndex = data[0];
					int criteria = data[1];

					bool onRest = criteria == 1;
					bool onLeave = criteria == 2;
					bool onWarp = criteria == 6;

					if (onRest || onLeave || onWarp)
					{
						run.Target = bonfireIndex;
						isBonfireWarpSplitActive = onWarp;
					}
					else
					{
						run.Id = Flags.OrderedBonfires[bonfireIndex];
						run.Data = (int)memory.GetBonfireState((BonfireFlags)run.Id);
						run.Target = Flags.OrderedBonfireStates[data[1]];
					}

					break;

				case SplitTypes.Boss:
					run.Id = Flags.OrderedBosses[data[0]];
					run.Flag = memory.CheckFlag(run.Id);

					break;

				case SplitTypes.Covenant:
					bool onDiscover = data[1] == 0;

					int target = Flags.OrderedCovenants[data[0]];

					if (onDiscover)
					{
						run.Data = memory.GetPromptedMenu();
						run.Target = target;
					}
					else
					{
						run.Data = (int)memory.GetCovenant();
						run.Target = target;
					}

					break;

				case SplitTypes.Event:
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

					break;

				case SplitTypes.Flag:
					int flag = data[0];

					run.Id = flag;
					run.Flag = memory.CheckFlag(flag);

					break;

				case SplitTypes.Item:
					ItemId id = ComputeItemId(split);

					int baseId = id.BaseId;
					int mods = data[2];
					int reinforcement = data[3];
					int count = data[4];

					isItemWarpSplitActive = data[5] == 2;
					isEstusSplit = baseId == EstusId;

					// In the layout file, mods and reinforcement are stored as int.MaxValue to simplify split
					// validation. 
					mods = mods == int.MaxValue ? 0 : Flags.OrderedInfusions[mods];
					reinforcement = reinforcement == int.MaxValue ? 0 : reinforcement;

					// Estus splits have their reinforcement stored as zero (since the reinforcement is implied through
					// ID directly).
					if (isEstusSplit)
					{
						estusReinforcement = reinforcement;
						reinforcement = 0;
					}

					// The data field of the run state isn't otherwise used for item splits, so it's used to store item
					// category (required to differentiate between items with the same ID).
					run.Id = id.BaseId;
					run.Data = id.Category;
					run.TargetItem = new ItemState(mods, reinforcement, count);

					break;

				case SplitTypes.Quitout:
					waitingOnQuitout = true;

					break;

				case SplitTypes.Zone:
					run.Zone = GetZone();
					run.Target = data[0];

					break;
			}
		}

		// Making the phase nullable makes testing easier.
		public void Refresh(TimerPhase? phase = null)
		{
			// In theory, the first IGT frame of a run should have a time value of about 32 milliseconds (i.e. one
			// frame at 30 fps). In practice, though, the first non-zero time value tends to be a bit bigger than that.
			// This threshold is arbitrary, but is meant to be big enough to cover that variation in start time, but
			// small enough that later loads into a file don't autostart the timer.
			const int TimerAutostartThreshold = 150;

			if (!Hook())
			{
				return;
			}

			if (phase != null)
			{
				switch (phase.Value)
				{
					case TimerPhase.Ended: return;
					case TimerPhase.NotRunning:
						// Someone might want to disable this feature if using real time and starting their timer from
						// the menu (or other reasons, probably).
						if (!masterControl.StartTimerAutomatically)
						{
							return;
						}

						int time = memory.GetGameTimeInMilliseconds();
						
						// The timer should autostart on a new game, but not every time you load into a file. Note that
						// if someone resets their timer manually immediately as the run begins, the timer may
						// autostart again if game time is still less than the threshold value. This is fixable, but
						// likely not worth the effort.
						if (run.GameTime == 0 && time > 0 && time < TimerAutostartThreshold)
						{
							timer.Start();
						}

						break;
				}
			}

			// Quitout splits (below) are detected using game time (specifically, when IGT is reset back to zero). As
			// such, the current IGT value needs to be stored here before it's reset while updating game time.
			int previousGameTime = run.GameTime;
			int newGameTime = memory.GetGameTimeInMilliseconds();

			// The timer is intentionally updated before an autosplit occurs (to ensure the split time is as accurate
			// as possible).
			if (masterControl.UseGameTime)
			{
				UpdateGameTime(newGameTime);
			}
			else
			{
				// Game time must be tracked even if "Use game time" is disabled (in order to accomodate quitout
				// splits.
				run.GameTime = newGameTime;
			}

			Split split = splitCollection.CurrentSplit;

			// It's possible for the current split to be null if no splits were configured at all.
			if (split == null || split.Type == SplitTypes.Manual || !split.IsFinished)
			{
				return;
			}

			// This is called each tick regardless of whether the current split is an item split (to ensure that the
			// inventory state is accurate by the time an item split crops up).
			if (itemsEnabled)
			{
				memory.RefreshItems();
			}

			// This condition covers all split types with warping as an option.
			if (preparedForWarp)
			{
				if (isBonfireWarpSplitActive)
				{
					int[] leaveValues =
					{
						(int)AnimationFlags.BonfireLeave1,
						(int)AnimationFlags.BonfireLeave2,
						(int)AnimationFlags.BonfireLeave3
					};

					int animation = memory.GetForcedAnimation();

					// Without this check, the player could rest at a target bonfire (without warping), then warp from
					// another bonfire and have the tool incorrectly split.
					if (leaveValues.Contains(animation))
					{
						preparedForWarp = false;

						return;
					}
				}
				else if (isItemWarpSplitActive && !IsTargetItemSatisfied())
				{
					preparedForWarp = false;

					return;
				}

				if (CheckWarp())
				{
					// Timer is null when testing the program from the testing class.
					timer?.Split();
				}

				return;
			}

			// Similar to warping, this covers all splits with quitouts as a timing option (and Quitout splits
			// themselves, of course).
			if (waitingOnQuitout)
			{
				// Game time is reset to zero on the title screen, but not on regular loading screens.
				if (newGameTime == 0 && previousGameTime > 0)
				{
					// On quitout splits, the actual split occurs on the loading screen following a reload
					// (specifically when the player is loaded). This is done to account for any potential IGT drift
					// between quitout and reload. In theory, this drift shouldn't exist, but it seems to happen anyway
					// in practice.
					waitingOnUnload = true;
					waitingOnQuitout = false;
				}

				return;
			}

			if (waitingOnUnload)
			{
				// When the load screen appears following a quitout, the player isn't unloaded instantly.
				if (!memory.IsPlayerLoaded())
				{
					waitingOnUnload = false;
					waitingOnReload = true;
				}

				return;
			}

			if (waitingOnReload)
			{
				// By the time this point in the code is reached, the player must already be unloaded (due to quitting
				// to the title screen).
				if (memory.IsPlayerLoaded())
				{
					waitingOnReload = false;
					timer.Split();
				}

				return;
			}

			if (splitFunctions[split.Type](split.Data))
			{
				timer?.Split();
			}
		}

		private bool Hook()
		{
			bool previouslyHooked = memory.ProcessHooked;

			// It's possible for the timer to be running before Dark Souls is launched. In this case, all splits are
			// treated as manual until the process is hooked, at which point the run state is updated appropriately.
			if (memory.Hook() && !previouslyHooked && timer?.CurrentState.CurrentPhase == TimerPhase.Running)
			{
				UpdateRunState();
			}

			return memory.ProcessHooked;
		}

		private void UpdateGameTime(int newGameTime)
		{
			// When the player quits the game, the IGT clock keeps ticking for 18 extra frames. Those frames are
			// removed from the timer on quitout. This is largely done to keep parity with the existing IGT tool.
			const int QuitoutCorrection = 594;

			LiveSplitState state = timer.CurrentState;

			// Setting this value to always be true prevents a weird timer creep from LiveSplit. I don't know why.
			state.IsGameTimePaused = true;

			TimerPhase phase = timer.CurrentState.CurrentPhase;
			
			int previousTime = run.GameTime;

			// This condition is only possible during a run when game time isn't increasing (game time resets to
			// zero on the main menu).
			bool quitout = newGameTime == 0 && previousTime > 0;

			// Previously, the timer was actually paused and unpaused here (rather than just putting game time in
			// stasis temporarily). I found that constant pausing and unpausing distracting, so I removed it.
			if (quitout && phase == TimerPhase.Running)
			{
				run.MaxGameTime -= QuitoutCorrection;
			}

			int max = Math.Max(newGameTime, run.MaxGameTime);

			if (phase != TimerPhase.Paused)
			{
				state.SetGameTime(TimeSpan.FromMilliseconds(max));
			}

			run.GameTime = newGameTime;
			run.MaxGameTime = max;
		}

		private ItemId ComputeItemId(Split split)
		{
			int[] data = split.Data;
			int rawId = ItemFlags.MasterList[data[0]][data[1]];
			int baseId = Utilities.StripHighestDigit(rawId, out int digit);

			// Many items have a category of zero, but the leading zero would be stripped from normal integers. As
			// such, nine is used instead (since there's no real category with ID nine).
			int category = digit == 9 ? 0 : digit;

			return new ItemId(baseId, category);
		}

		private void PrepareWarp()
		{
			preparedForWarp = true;
			isLoadScreenVisible = memory.IsLoadScreenVisible();
		}

		private bool CheckWarp()
		{
			const int Darksign = 117;
			const int HomewardBone = 330;
			const int BonfireWarpPrompt = 80;

			if (!isBonfireWarpConfirmed)
			{
				// This state becomes true for just a moment when the player confirms a bonfire warp.
				isBonfireWarpConfirmed = memory.GetPromptedMenu() == BonfireWarpPrompt &&
					memory.GetForcedAnimation() == (int)AnimationFlags.BonfireWarp;
			}

			bool visible = memory.IsLoadScreenVisible();
			bool previouslyVisible = isLoadScreenVisible;

			isLoadScreenVisible = visible;

			if (!visible || previouslyVisible)
			{
				return false;
			}

			// Note that for bonfire warp splits, this point will only be reached if the player is resting at the
			// correct bonfire.
			if (isBonfireWarpConfirmed)
			{
				isBonfireWarpConfirmed = false;

				return true;
			}

			// Dying counts as a warp (since you do actually warp on death). The exception here is fall control
			// quitouts, but thankfully a successful fall control quitout causes the player's HP to stay intact.
			if (memory.GetPlayerHP() == 0)
			{
				return true;
			}

			// This point in the code can only be reached via an on-warp bonfire split (meaning that warp items are
			// irrelevant).
			if (splitCollection.CurrentSplit.Type == SplitTypes.Bonfire)
			{
				return false;
			}

			int itemUsed = memory.GetPromptedItem();

			return itemUsed == Darksign || itemUsed == HomewardBone;
		}

		private bool ProcessBonfire(int[] data)
		{
			// The player must be very close to a bonfire in order to rest. The chosen value here is arbitrary and
			// could be smaller (but it makes no performance difference).
			const int Radius = 10;

			int criteria = data[1];

			bool onRest = criteria == 1;
			bool onLeave = criteria == 2;
			bool onWarp = criteria == 6;

			if (onRest || onLeave || onWarp)
			{
				int[] animationValues;

				if (onLeave)
				{
					animationValues = new[]
					{
						(int)AnimationFlags.BonfireLeave1,
						(int)AnimationFlags.BonfireLeave2,
						(int)AnimationFlags.BonfireLeave3
					};
				}
				// Both rest and warp splits need to check resting animation values.
				else
				{
					animationValues = new[]
					{
						(int)AnimationFlags.BonfireRest1,
						(int)AnimationFlags.BonfireRest2,
						(int)AnimationFlags.BonfireRest3
					};
				}

				int animation = memory.GetForcedAnimation();

				// This confirms that the player is the correct bonfire animation (either resting or leaving, as
				// appropriate), but not which bonfire.
				if (animationValues.Contains(animation))
				{
					int index = ComputeClosestTarget(bonfireLocations, Radius);
					
					bool correctBonfire = index == run.Target;

					if (correctBonfire && onWarp)
					{
						PrepareWarp();

						return false;
					}

					return correctBonfire;
				}

				return false;
			}

			int state = (int)memory.GetBonfireState((BonfireFlags)run.Id);

			// Increasing bonfires states (unlit, lit, then the different levels of kindling) always increase the state
			// value.
			if (run.Data > state)
			{
				run.Data = state;

				return state == run.Target;
			}

			return false;
		}

		private bool ProcessBoss(int[] data)
		{
			bool isDefeated = memory.CheckFlag(run.Id);

			if (isDefeated && !run.Flag)
			{
				run.Flag = true;

				return CheckDefaultTimingOptions(data[1]);
			}

			return false;
		}

		private bool ProcessCovenant(int[] data)
		{
			int criteria = data[1];

			bool onDiscover = criteria == 0;
			bool criteriaSatisfied = onDiscover ? CheckCovenantDiscovery() : CheckCovenantJoin();

			return criteriaSatisfied && CheckDefaultTimingOptions(data[2]);
		}

		private bool CheckCovenantDiscovery()
		{
			// This radius is arbitrary and could be smaller. All that matters is that the radius is large enough to
			// account for the maximum distance between any two points from which the player could join a single
			// covenant. For reference, the largest distance I could find is within the area surrounding the ancient
			// dragon in Ash Lake.
			const int Radius = 40;
			const int CovenantPromptId = 121;

			int menu = memory.GetPromptedMenu();

			if (menu != run.Data)
			{
				run.Data = menu;

				if (menu == CovenantPromptId)
				{
					// At this point, the covenant prompt has appeared, but it's unknown to which covenant the prompt
					// applies (since all covenants use the same prompt).
					int index = ComputeClosestTarget(covenantLocations, Radius);
					int target = run.Target;

					// Way of White is the only covenant that can be joined from two locations. Conveniently, the first
					// two locations in the array can both be used for Way of White (since there's no covenant zero).
					if (index <= 1)
					{
						return target == (int)CovenantFlags.WayOfWhite;
					}

					// Covenant locations are ordered the same as their corresponding covenant ID (ranging from 1
					// through 9 inclusive).
					return index == target;
				}
			}

			return false;
		}

		private bool CheckCovenantJoin()
		{
			int covenant = (int)memory.GetCovenant();

			if (covenant != run.Data)
			{
				run.Data = covenant;

				return covenant == run.Target;
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

				return CheckDefaultTimingOptions(data[1]);
			}

			return false;
		}

		private bool ProcessEnding(int[] data)
		{
			int clearCount = memory.GetClearCount();

			if (clearCount == run.Data + 1)
			{
				run.Data++;

				// The player's X coordinate increases as you approach the exit (the exit is at roughly 421).
				bool isDarkLord = memory.GetPlayerX() > 415;
				bool isDarkLordTarget = data[0] == 5;

				return isDarkLord == isDarkLordTarget;
			}

			return false;
		}

		private bool ProcessFlag(int[] data)
		{
			int flag = data[0];

			if (!run.Flag && memory.CheckFlag(flag))
			{
				run.Flag = true;

				return CheckDefaultTimingOptions(data[1]);
			}

			return false;
		}

		private bool ProcessItem(int[] data)
		{
			return IsTargetItemSatisfied() && CheckDefaultTimingOptions(data[5]);
		}

		// This check is done from two places (processing item splits and verifying that an item wasn't dropped while
		// waiting for a warp).
		private bool IsTargetItemSatisfied()
		{
			int targetId = run.Id;

			// Estus flasks are a bit unique as far as upgrades. Other reinforceable items (like weapons and pyromancy
			// flames) store their upgrades by directly modifying the ID in memory. In contrast, estus flasks span a
			// range of IDs, starting at 200 (for an unfilled +0 flask) up through 215 (a filled +7 flask).
			if (isEstusSplit)
			{
				targetId += estusReinforcement * 2;
			}

			// This double array felt like the easiest way to handle estus splits, even though literally every item
			// besides the estus flask will only use a single state array.
			ItemState[][] states = new ItemState[2][];
			states[0] = memory.GetItemStates(targetId, run.Data);

			if (isEstusSplit)
			{
				// For estus splits, the unfilled ID (at the target reinforement) is stored. Adding one brings you to
				// the filled ID for that same reinforcement level.
				states[1] = memory.GetItemStates(targetId + 1, run.Data);
			}

			ItemState target = run.TargetItem;

			int count = 0;

			foreach (ItemState[] array in states)
			{
				if (array == null)
				{
					continue;
				}
				
				// Computing the count in this way allows the target count to be honored even if the target item
				// doesn't stack (e.g. splitting on three stone greatswords rather than one).
				count += array
					.Where(state => state.Mods == target.Mods && state.Reinforcement >= target.Reinforcement)
					.Sum(state => state.Count);
			}

			return count >= target.Count;
		}

		private bool ProcessZone(int[] data)
		{
			Zone zone = GetZone();
			Zone previousZone = run.Zone;

			List<Zone> list = zoneMap[(Zones)run.Target];

			run.Zone = zone;

			// The first zone in the list is the target zone (the actual data object, not the enum value).
			if (!list[0].Equals(zone))
			{
				return false;
			}

			// The remaining items in the list are zones from which you can travel to the target zone.
			for (int i = 1; i < list.Count; i++)
			{
				if (list[i].Equals(previousZone))
				{
					return true;
				}
			}

			return false;
		}

		// Note that null is returned when no zone is active (i.e. you're on the title screen).
		private Zone GetZone()
		{
			int world = memory.GetWorld();
			int area = memory.GetArea();

			// Both world and area are set to 255 on load screens and the main menu.
			return world == byte.MaxValue ? null : new Zone(world, area);
		}

		private bool ProcessQuitout(int[] data)
		{
			bool loaded = memory.IsPlayerLoaded();

			if (loaded != run.Flag)
			{
				run.Flag = loaded;

				// This means that the player returned to the title screen (i.e. quit the game).
				if (!loaded)
				{
					return true;
				}
			}

			return false;
		}

		// Most splits with timing options available use the same set of options (on trigger, on quitout, and on warp).
		// The specific wording of "On trigger" can change based on split type, but the idea is to split immediately
		// regardless.
		private bool CheckDefaultTimingOptions(int index)
		{
			switch (index)
			{
				// Immediate
				case 0: return true;

				// On quitout
				case 1: waitingOnQuitout = true;
					return false;

				case 2: PrepareWarp();
					return false;
			}

			// This case should never occur.
			return false;
		}

		private int ComputeClosestTarget(Vector3[] targets, int radius)
		{
			Vector3 playerPosition = memory.GetPlayerPosition();

			int closestIndex = -1;

			float closestDistance = float.MaxValue;
			float radiusSquared = radius * radius;

			for (int i = 0; i < targets.Length; i++)
			{
				float d = playerPosition.ComputeDistanceSquared(targets[i]);

				// Using distance squared prevents having to do a square root operation.
				if (d <= radiusSquared && d < closestDistance)
				{
					closestDistance = d;
					closestIndex = i;
				}
			}

			return closestIndex;
		}

		public void Dispose()
		{
		}
	}
}