using DarkSoulsMemory;
using LiveSplit.DarkSouls.Controls;
using LiveSplit.DarkSouls.Data;
using LiveSplit.DarkSouls.Memory;
using LiveSplit.Model;
using LiveSplit.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using IComponent = LiveSplit.UI.Components.IComponent;

namespace LiveSplit.DarkSouls
{
    public class SoulsComponent : IComponent
	{
		public const string DisplayName = "Dark Souls Autosplitter";
		
        private readonly DarkSoulsMemory.DarkSouls darkSouls;

		private TimerModel timer;
		private SplitCollection splitCollection;

		private SoulsMasterControl masterControl;
		private Dictionary<SplitTypes, Func<int[], bool>> splitFunctions;
		private Vector3f[] covenantLocations;
		private Vector3f[] bonfireLocations;
		private RunState run;
		
		private bool preparedForWarp;
		private bool isLoadScreenVisible;
		private bool waitingOnQuitout;
		private bool waitingOnUnload;
		private bool waitingOnReload;
		private bool waitingOnCredits;
		private bool waitingOnFirstLoad;

		// This variable tracks whether the player confirmed a warp from a bonfire prompt. The data used to detect this
		// state (beginning a bonfire warp) doesn't persist up to the loading screen's appearance, so it needs to be
		// tracked separately.
		private bool isBonfireWarpConfirmed;

		// Most warp splits detect an event, then wait for a warp to occur. Bonfire and item warps are unique in that
		// they can be undone once activated (by leaving the target bonfire or losing the target item).
		private bool isBonfireWarpSplitActive;
		private bool isItemWarpSplitActive;

		
		public SoulsComponent()
        {
            darkSouls = new DarkSoulsMemory.DarkSouls();

			splitCollection = new SplitCollection();
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
				{ SplitTypes.Zone, ProcessZone },
				{ SplitTypes.Box, ProcessBox },
			};

			// This array is used for covenant discovery splits. Discovery occurs when the player is prompted to join a
			// covenant via a yes/no confirmation box. That prompt's appearance can be detected through memory, but
			// it's shared among all covenants. As such, position is used to narrow down the covenant.
			covenantLocations = new []
			{
				new Vector3f(-28, -53, 87), // Way of White (in front of Petrus)
				new Vector3f(9, 29, 121), // Way of White (beside Rhea) 
				new Vector3f(622, 164, 255), // Princess (in front of Gwynevere)
				new Vector3f(36, 12, -32), // Sunlight (in front of the sunlight altar) 
				new Vector3f(93, -311, 4), // Darkwraith (in front of Kaathe)
				new Vector3f(-702, -412, -333), // Dragon (in front of the everlasting dragon)
				new Vector3f(-161, -265, -32), // Gravelord (below Nito)
				new Vector3f(285, -3, -105), // Forest (below Alvina) 
				new Vector3f(430, 60, 255), // Darkmoon (just outside Gwyndolin's boss arena)
				new Vector3f(138, -252, 94) // Chaos (in front of the fair lady)
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
				new Vector3f(171, 173, 255), // Anor Londo - Entrance
				new Vector3f(504, 135, 175), // Anor Londo - Interior
				new Vector3f(593, 161, 254), // Anor Londo - Princess
				new Vector3f(391, 70, 255), // Anor Londo - Tomb
				new Vector3f(-388, -408, 156), // Ash Lake - Entrance
				new Vector3f(-700, -414, -323), // Ash Lake - Dragon
				new Vector3f(-277, -137, 73), // Blighttown - Bridge
				new Vector3f(-198, -215, 100), // Blighttown - Swamp
				new Vector3f(44, -119, 204), // Catacombs - Entrance
				new Vector3f(48, -112, 301), // Catacombs - Illusion
				new Vector3f(854, -577, 849), // Chasm of the Abyss
				new Vector3f(96, 134, 864), // Crystal Caves
				new Vector3f(165, -77, -55), // Darkroot Basin
				new Vector3f(257, -3, -12), // Darkroot Garden
				new Vector3f(141, -253, 94), // Daughter of Chaos
				new Vector3f(253, -334, 22), // Demon Ruins - Central
				new Vector3f(194, -267, 130), // Demon Ruins - Entrance
				new Vector3f(118, -357, 139), // Demon Ruins - Firesage
				new Vector3f(349, 278, 594), // Duke's Archives - Balcony
				new Vector3f(230, 200, 481), // Duke's Archives - Entrance
				new Vector3f(378, 270, 552), // Duke's Archives - Prison
				new Vector3f(52, -64, 106), // Firelink Altar
				new Vector3f(-51, -61, 55), // Firelink Shrine
				new Vector3f(-318, -236, 123), // Great Hollow
				new Vector3f(581, -444, 444), // Lost Izalith - Bed of Chaos
				new Vector3f(456, -380, 170), // Lost Izalith - Illusion
				new Vector3f(229, -384, 91), // Lost Izalith - Lava Field
				new Vector3f(972, -314, 583), // Oolacile Sanctuary
				new Vector3f(863, -448, 912), // Oolacile Township - Dungeon
				new Vector3f(1041, -332, 875), // Oolacile Township - Entrance
				new Vector3f(-24, 52, 944), // Painted World
				new Vector3f(897, -329, 452), // Sanctuary Garden
				new Vector3f(73, 60, 301), // Sen's Fortress
				new Vector3f(85, -311, 3), // The Abyss
				new Vector3f(-121, -74, 13), // The Depths
				new Vector3f(77, -214, 45), // Tomb of the Giants - Alcove
				new Vector3f(-159, -265, -34), // Tomb of the Giants - Nito
				new Vector3f(97, -200, 104), // Tomb of the Giants - Patches
				new Vector3f(3, 196, 7), // Undead Asylum - Courtyard
				new Vector3f(34, 193, -26), // Undead Asylum - Interior
				new Vector3f(3, -10, -61), // Undead Burg
				new Vector3f(88, 15, 107), // Undead Parish - Andre
				new Vector3f(24, 10, -23) // Undead Parish - Sunlight
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
			if (settings == null)
            {
                return;
            }

			bool useGameTime = bool.Parse(settings["UseGameTime"].InnerText);
			bool resetEquipment = bool.Parse(settings["ResetEquipment"].InnerText);
			bool autostart = bool.Parse(settings["TimerAutostart"].InnerText);

			masterControl.UseGameTime = useGameTime;
			masterControl.ResetEquipmentIndexes = resetEquipment;
			masterControl.StartTimerAutomatically = autostart;

			var versionElement = settings["Version"];

			// The version element itself wasn't present in the original 1.0.0 release.
			string rawFileVersion = versionElement == null ? "1.0.0" : settings["Version"].InnerText;

			Version currentVersion = Utilities.GetVersion();
			Version fileVersion = Version.Parse(rawFileVersion);

			// For the time being, downgrading layout files (e.g. trying to load a 1.0.2 layout file on the 1.0.1
			// autosplitter) isn't supported. To ensure no weird errors crop up, the splits are simply emptied instead.
			if (fileVersion > currentVersion)
			{
				splitCollection.Splits = null;

				return;
			}

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

			if (currentVersion > fileVersion)
			{
				VersionHelper.Convert(splits, fileVersion);
			}

			splitCollection.Splits = splits;
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
			splitCollection.OnStart();

			if (!darkSouls.IsPlayerLoaded())
			{
				waitingOnFirstLoad = true;

				return;
			}
			
			UpdateRunState();
		}

		private void OnReset()
		{
			if (masterControl.ResetEquipmentIndexes)
			{
				darkSouls.ResetInventoryIndices();
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
			waitingOnCredits = false;
			isBonfireWarpConfirmed = false;
			isBonfireWarpSplitActive = false;
			isItemWarpSplitActive = false;

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
						run.Data = (int)darkSouls.GetBonfireState((Bonfire)run.Id);
						run.Target = Flags.OrderedBonfireStates[criteria];
					}

					break;

				case SplitTypes.Boss:
					run.Id = Flags.OrderedBosses[data[0]];
					run.Flag = darkSouls.IsBossDefeated((BossType)run.Id);
					break;

				case SplitTypes.Covenant:
					var onDiscover = data[1] == 0;

					int target = Flags.OrderedCovenants[data[0]];

					if (onDiscover)
					{
						run.Data = (int)darkSouls.GetMenuPrompt();
						run.Target = target;
					}
					else
					{
						run.Data = (int)darkSouls.GetCovenant();
						run.Target = target;
					}

					break;

				case SplitTypes.Event:
					switch ((WorldEvents)data[0])
					{
						case WorldEvents.Bell1:
							run.Id = (int)BellFlags.FirstBell;
							run.Flag = darkSouls.CheckFlag(run.Id);

							break;

						case WorldEvents.Bell2:
							run.Id = (int)BellFlags.SecondBell;
							run.Flag = darkSouls.CheckFlag(run.Id);

							break;

						default: run.Data = darkSouls.GetClearCount(); break;
					}

					break;

				case SplitTypes.Flag:
					var flag = data[0];

					run.Id = flag;
					run.Flag = darkSouls.CheckFlag(flag);

					break;

				case SplitTypes.Item:
                    ComputeItemId(split, out int itemId, out int category);
					
					var infusion = ItemInfusion.Normal;
                    if (data[2] != int.MaxValue)
					{
                        infusion = (ItemInfusion)Flags.OrderedInfusions[data[2]];
                    }

                    var level = data[3];
                    var quantity = data[4];
					
					var categories = new List<ItemCategory>();
                    switch (Utilities.ToHex(category))
                    {
                        case 0:
                            categories.Add(ItemCategory.MeleeWeapons);
                            categories.Add(ItemCategory.RangedWeapons);
                            categories.Add(ItemCategory.Ammo);
                            categories.Add(ItemCategory.Shields);
                            categories.Add(ItemCategory.SpellTools);
                            break;

                        case 1:
                            categories.Add(ItemCategory.Armor);
                            break;

                        case 2:
                            categories.Add(ItemCategory.Rings);
                            break;

                        case 4:
                            categories.Add(ItemCategory.Consumables);
                            categories.Add(ItemCategory.Key);
                            categories.Add(ItemCategory.Spells);
                            categories.Add(ItemCategory.UpgradeMaterials);
                            categories.Add(ItemCategory.UsableItems);
                            break;
                    }
                    
                    var lookupItem = Item.AllItems.FirstOrDefault(j => categories.Contains(j.Category) && j.Id == itemId);
                    if (lookupItem != null)
                    {
                        var instance = new Item(lookupItem.Name, lookupItem.Id, lookupItem.Type, lookupItem.Category, lookupItem.StackLimit, lookupItem.Upgrade);
                        instance.Quantity = data[4];
						instance.Infusion = infusion;
                        instance.UpgradeLevel = level;
                        run.TargetItem = instance;
					}

					break;

				case SplitTypes.Quitout:
					// Quitout splits have an associated count (the number of quitouts required to split). The count is
					// stored in the data field.
					run.Data = 0;
					run.Target = data[0];

					waitingOnQuitout = true;

					break;

				case SplitTypes.Zone:
					run.Zone = darkSouls.GetZone();
					run.Target = Flags.OrderedZones[data[0]];

					break;

				case SplitTypes.Box:
                    var box = new Box(
                        new Vector3f(data[0], data[1], data[2]),
                        new Vector3f(data[3], data[4], data[5])
                    );
                    run.Box = box;

                    break;
			}
		}

		// Making the phase nullable makes testing easier.
		public void Refresh(TimerPhase? nullablePhase = null)
        {
            darkSouls.Refresh();


			// In theory, the first IGT frame of a run should have a time value of about 32 milliseconds (i.e. one
			// frame at 30 fps). In practice, though, the first non-zero time value tends to be a bit bigger than that.
			// This threshold is arbitrary, but is meant to be big enough to cover that variation in start time, but
			// small enough that later loads into a file don't autostart the timer.
			const int TimerAutostartThreshold = 150;

            //If neither game is connected
			if (!darkSouls.IsGameAttached)
			{
				return;
			}

			// This will only be true while testing (through a console program).
			if (nullablePhase == null)
			{
				return;
			}

			if (waitingOnFirstLoad)
			{
				if (!darkSouls.IsPlayerLoaded())
				{
					return;
				}

				//InitializeItems();
				UpdateRunState();

				waitingOnFirstLoad = false;
			}

			TimerPhase phase = nullablePhase.Value;

            int inGameTime = 0;
            if (darkSouls.IsGameAttached)
            {
                inGameTime = darkSouls.GetGameTimeInMilliseconds();
            }


			// Someone might want to disable timer autostart if using real time and starting their timer from the main
			// menu (or some other reason).
			if (phase == TimerPhase.NotRunning && masterControl.StartTimerAutomatically)
            {
                // The timer should autostart on a new game, but not every time you load into a file. Note that
				// if someone resets their timer manually immediately as the run begins, the timer may
				// autostart again if game time is still less than the threshold value. This is fixable, but
				// likely not worth the effort.
				if (run.GameTime == 0 && inGameTime > 0 && inGameTime < TimerAutostartThreshold)
				{
					timer.Start();
				}
			}

			// Quitout splits (below) are detected using game time (specifically, when IGT is reset back to zero). As
			// such, the current IGT value needs to be stored here before it's reset while updating game time.
			int previousGameTime = run.GameTime;


			// The timer is intentionally updated before an autosplit occurs (to ensure the split time is as accurate
			// as possible).
			if (masterControl.UseGameTime)
			{
				UpdateGameTime(inGameTime);
			}
			else
			{
				// Game time must be tracked even if "Use game time" is disabled (in order to accomodate quitout
				// splits).
				run.GameTime = inGameTime;
			}

			// This is called each tick regardless of whether the current split is an item split (to ensure that the
			// inventory state is accurate by the time an item split crops up).
			//if (itemsEnabled)
			//{
			//	memory.RefreshItems();
			//}

			// This check is intentionally done relatively far down in the function. The reason is that an ended timer
			// can be undone, and if that happens, I'd like all splits to continue working properly. Specifically, that
			// means that items and IGT (if applicable) are tracked even when the timer has ended.
			if (phase == TimerPhase.Ended)
			{
				return;
			}

			Split split = splitCollection.CurrentSplit;

			// It's possible for the current split to be null if no splits were configured at all.
			if (split == null || split.Type == SplitTypes.Manual || !split.IsFinished)
			{
				return;
			}

			// This condition covers all split types with warping as an option.
			if (preparedForWarp)
			{
				if (isBonfireWarpSplitActive)
				{
					ForcedAnimation[] leaveValues =
					{
						ForcedAnimation.BonfireLeave1,
						ForcedAnimation.BonfireLeave2,
						ForcedAnimation.BonfireLeave3
					};

                    var animation = darkSouls.GetForcedAnimation();

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
					timer.Split();
				}

				return;
			}

			// Similar to warping, this covers all splits with quitouts as a timing option (and Quitout splits
			// themselves, of course).
			if (waitingOnQuitout)
			{
				// Game time is reset to zero on the title screen, but not on regular loading screens.
				if (inGameTime == 0 && previousGameTime > 0)
				{
					if (splitCollection.CurrentSplit.Type == SplitTypes.Quitout)
					{
						run.Data++;

						// Quitout splits track quitout count. If that target hasn't yet been satisfied, the function
						// can just return immediately (which leaves the waitingOnQuitout variable intact).
						if (run.Data < run.Target)
						{
							return;
						}
					}

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
				if (!darkSouls.IsPlayerLoaded())
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
				if (darkSouls.IsPlayerLoaded())
				{
					waitingOnReload = false;
					timer.Split();
				}

				return;
			}

			if (splitFunctions[split.Type](split.Data))
			{
				timer.Split();
			}
		}


		private void UpdateGameTime(int newGameTime)
		{
			// When the player quits the game, the IGT clock keeps ticking for 18 extra frames. Those frames are
			// removed from the timer on quitout. This is largely done to keep parity with the existing IGT tool.
			const int quitoutCorrection = 594;
            
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
			if (quitout)
			{
				switch (phase)
				{
					case TimerPhase.Running: run.MaxGameTime -= quitoutCorrection;
						break;

					case TimerPhase.NotRunning: run.MaxGameTime = 0;
						break;
				}
			}

			int max = Math.Max(newGameTime, run.MaxGameTime);

			if (phase != TimerPhase.Paused)
            {
				timer.CurrentState.SetGameTime(TimeSpan.FromMilliseconds(max));
			}

			run.GameTime = newGameTime;
			run.MaxGameTime = max;
		}

		private void ComputeItemId(Split split, out int baseId, out int category)
		{
			int[] data = split.Data;
			int rawId = ItemFlags.MasterList[data[0]][data[1]];
			baseId = Utilities.StripHighestDigit(rawId, out int digit);

			// Many items have a category of zero, but the leading zero would be stripped from normal integers. As
			// such, nine is used instead (since there's no real category with ID nine).
			category = digit == 9 ? 0 : digit;

			//return new ItemId(baseId, category);
		}

		private void PrepareWarp()
		{
			preparedForWarp = true;
			isLoadScreenVisible = !darkSouls.IsPlayerLoaded();
		}

		private bool CheckWarp()
		{
			if (!isBonfireWarpConfirmed)
			{
				// This state becomes true for just a moment when the player confirms a bonfire warp.
				isBonfireWarpConfirmed = darkSouls.GetMenuPrompt() == MenuPrompt.BonfireWarp  && darkSouls.GetForcedAnimation() == ForcedAnimation.BonfireWarp;
			}

			bool visible = !darkSouls.IsPlayerLoaded();
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
			if (darkSouls.GetPlayerHealth() == 0)
			{
				return true;
			}

			// This point in the code can only be reached via an on-warp bonfire split (meaning that warp items are
			// irrelevant).
			if (splitCollection.CurrentSplit.Type == SplitTypes.Bonfire)
			{
				return false;
			}

			var itemUsed = darkSouls.GetItemPrompt();

			return itemUsed == ItemPrompt.Darksign || itemUsed == ItemPrompt.HomewardBone;
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
				ForcedAnimation[] animationValues;

				if (onLeave)
				{
					animationValues = new[]
					{
						ForcedAnimation.BonfireLeave1,
						ForcedAnimation.BonfireLeave2,
						ForcedAnimation.BonfireLeave3
					};
				}
				// Both rest and warp splits need to check resting animation values.
				else
				{
					animationValues = new[]
					{
						ForcedAnimation.BonfireStartRest1,
						ForcedAnimation.BonfireStartRest2,
						ForcedAnimation.BonfireStartRest3
					};
				}

				var animation = darkSouls.GetForcedAnimation();

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

            var test = (Bonfire)run.Id;
			int state = (int)darkSouls.GetBonfireState((Bonfire)run.Id);

			// Increasing bonfires states (unlit, lit, then the different levels of kindling) always increase the state
			// value.
			if (state > run.Data)
			{
				run.Data = state;

				return state == run.Target;
			}

			return false;
		}

		private bool ProcessBoss(int[] data)
        {
            var boss = (BossType)run.Id;
            var isDefeated = darkSouls.IsBossDefeated(boss);
			//IsBossAlive
			//bool isDefeated = memory.CheckFlag(run.Id);

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

            var menu = darkSouls.GetMenuPrompt();

			if (menu != (MenuPrompt)run.Data)
			{
				run.Data = (int)menu;

				if (menu == MenuPrompt.Covenant)
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
			int covenant = (int)darkSouls.GetCovenant();

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
			bool rung = darkSouls.CheckFlag(run.Id);

			if (rung && !run.Flag)
			{
				run.Flag = true;

				return CheckDefaultTimingOptions(data[1]);
			}

			return false;
		}

		private bool ProcessEnding(int[] data)
		{
			int clearCount = darkSouls.GetClearCount();

			if (clearCount == run.Data + 1)
			{
				run.Data++;

				// The player's X coordinate increases as you approach the exit (the exit is at roughly 421).
				bool isDarkLord = darkSouls.GetPlayerPosition().X > 415;
				bool isDarkLordTarget = data[0] == 5;

				if (isDarkLord == isDarkLordTarget)
				{
					// Ending splits split when the credits appear, not when the ending itself occurs.
					waitingOnCredits = true;

					return false;
				}
			}

			// The player is unloaded just as the credits start.
			return waitingOnCredits && !darkSouls.IsPlayerLoaded();
		}

		private bool ProcessFlag(int[] data)
		{
			int flag = data[0];

			if (!run.Flag && darkSouls.CheckFlag(flag))
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
            //This function is called before target item is set..
            if (run.TargetItem != null)
            {
                var items = darkSouls.GetCurrentInventoryItems();
                return items.Any(i => i.Type == run.TargetItem.Type && i.Infusion == run.TargetItem.Infusion && i.UpgradeLevel == run.TargetItem.UpgradeLevel && i.Quantity >= run.TargetItem.Quantity);
			}

            return false;
        }

		private bool ProcessZone(int[] data)
        {
            var zoneType = darkSouls.GetZone();
            var previousZone = run.Zone;

			run.Zone = zoneType;
			
			if ((ZoneType)run.Target != zoneType)
			{
				return false;
			}
            
            if (zoneType != previousZone && zoneType == (ZoneType)run.Target)
            {
				return true;
			}
			

			return false;
		}

        private bool ProcessBox(int[] data)
        {
            var playerPosition = darkSouls.GetPlayerPosition();

			if(run.Box.LowerBound.X < playerPosition.X && run.Box.LowerBound.Y < playerPosition.Y && run.Box.LowerBound.Z < playerPosition.Z &&
			   run.Box.UpperBound.X > playerPosition.X && run.Box.UpperBound.Y > playerPosition.Y && run.Box.UpperBound.Z > playerPosition.Z)
            {
                return true;
            }
            return false;
        }
		
		private bool ProcessQuitout(int[] data)
        {
            bool loaded = darkSouls.IsPlayerLoaded();
			
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

		private int ComputeClosestTarget(Vector3f[] targets, int radius)
		{
			Vector3f playerPosition = darkSouls.GetPlayerPosition();

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