using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveSplit.DarkSouls.Data;
using IntPtr = System.IntPtr;

namespace LiveSplit.DarkSouls.Memory
{
	public class SoulsMemory
	{
		private Process process;
		private IntPtr handle;
		private SoulsPointers pointers;

		// Tracking items is a bit complex. It makes more sense for the memory class to manage that complexity rather
		// than the main component class.
		private ItemTracker keyTracker;
		private ItemTracker itemTracker;
		private BottomlessBoxTracker bottomlessBoxTracker;

		public bool ProcessHooked { get; private set; }

		public bool Hook()
		{
			if (ProcessHooked)
			{
				if (process.HasExited)
				{
					ProcessHooked = false;
					process = null;

					return false;
				}

				pointers.Refresh(process);

				return true;
			}

			Process[] processes = Process.GetProcessesByName("DARKSOULS");

			if (processes.Length > 0)
			{
				process = processes[0];

				if (process.HasExited)
				{
					return false;
				}

				// Item trackers are created below (or left as null if not needed).
				handle = process.Handle;
				pointers = new SoulsPointers(process);
				ProcessHooked = true;
			}

			return ProcessHooked;
		}

		public void SetItems(List<ItemId> itemIds, int[] keyItems)
		{
			if (itemIds == null)
			{
				keyTracker = null;
				itemTracker = null;
				bottomlessBoxTracker = null;

				return;
			}

			List<ItemId> keys = new List<ItemId>();
			List<ItemId> items = new List<ItemId>();

			foreach (ItemId id in itemIds)
			{
				if (keyItems.Contains(id.BaseId))
				{
					keys.Add(id);
				}
				else
				{
					items.Add(id);
				}
			}

			IntPtr inventory = pointers.Inventory;

			// Both trackers are nullified if no splits would require using them.
			keyTracker = keys.Count > 0
				? new ItemTracker(inventory, handle, (int)InventoryFlags.KeyStart, (int)InventoryFlags.KeyCount)
				: null;

			itemTracker = items.Count > 0
				? new ItemTracker(inventory, handle, (int)InventoryFlags.ItemStart, (int)InventoryFlags.ItemCount)
				: null;

			// Key items can't be put in the box, meaning that the bottomless box only needs to be tracked if items
			// themselves are tracked.
			bottomlessBoxTracker = items.Count > 0
				? new BottomlessBoxTracker(inventory, handle)
				: null;

			keyTracker?.SetItems(keys);
			itemTracker?.SetItems(items);
			bottomlessBoxTracker?.SetItems(items);
		}

		// This function is called once per update tick if item splits are in use (regardless of whether an item split
		// is active). In contrast, the function above effectively resets the tracker at the start of a new run.
		public void RefreshItems()
		{
			keyTracker?.Refresh();
			itemTracker?.Refresh();
			bottomlessBoxTracker?.Refresh();
		}

		public ItemState[] GetItemStates(int baseId, int category, bool isKey)
		{
			// Key items don't share IDs with any other items (except for a few mystery items, but those aren't
			// included in the autosplitter UI).
			if (isKey)
			{
				return keyTracker.GetItemStates(baseId, category);
			}

			List<ItemState> states = new List<ItemState>();
			states.AddRange(itemTracker.GetItemStates(baseId, category));
			states.AddRange(bottomlessBoxTracker.GetItemStates(baseId, category));

			return states.ToArray();
		}

		public void ResetEquipmentIndexes()
		{
			int[] slots =
			{
				0x0, // Slot 7
				0x4, // Slot 0
				0x8, // Slot 8
				0xC, // Slot 1
				0x10, // Slot 9
				0x10 + 0x8, // Slot 10
				0x14, // Slot 11
				0x14 + 0x8, // Slot 12
				0x20, // Slot 14
				0x20 + 0x4, // Slot 15
				0x20 + 0x8, // Slot 16
				0x20 + 0xC, // Slot 17
				0x34, // Slot 18
				0x34 + 0x4, // Slot 19
				0x3C, // Slot 2
				0x3C + 0x4, // Slot 3
				0x3C + 0x8, // Slot 4
				0x3C + 0xC, // Slot 5
				0x3C + 0x10, // Slot 6
			};

			foreach (int slot in slots)
			{
				MemoryTools.Write(handle, pointers.Equipment + slot, uint.MaxValue);
			}
		}

		public int GetGameTimeInMilliseconds()
		{
            return MemoryTools.ReadInt32(handle, pointers.InGameTime);

            //IntPtr pointer = (IntPtr)MemoryTools.ReadInt32(handle, (IntPtr)0x1378700);
			//
			//return MemoryTools.ReadInt32(handle, IntPtr.Add(pointer, 0x68));
		}

		public byte GetActiveAnimation()
		{
			return MemoryTools.ReadByte(handle, pointers.Character + 0xC5C);
		}

		public int GetForcedAnimation()
		{
			return MemoryTools.ReadInt32(handle, pointers.Character + 0xFC);
		}

		// Every time the player uses an item that requires a yes/no confirmation box, the ID of the item can be
		// retreived. That ID remains in place until the item's animation is complete.
		public int GetPromptedItem()
		{
			return MemoryTools.ReadInt32(handle, pointers.Character + 0x62C);
		}

		// "Prompted menu" here refers to the small menu near the bottom of the screen (such as yes/no confirmation
		// boxes). Each box has a unique ID (based on the text displayed).
		public byte GetPromptedMenu()
		{
			// The prompted menu ID is stored using a static address.
			return MemoryTools.ReadByte(handle, (IntPtr)0x12E33E0);
		}

		// In this context, "World" (and "Area" below) refer to large geographic locations within the world. The two
		// values together can be used to roughly determine where you are (although not with the precision of
		// individual named zones).
		public byte GetWorld()
		{
			return MemoryTools.ReadByte(handle, pointers.Zone + 0xA13);
		}

		public byte GetArea()
		{
			return MemoryTools.ReadByte(handle, pointers.Zone + 0xA12);
		}

		public bool IsLoadScreenVisible()
		{
			return MemoryTools.ReadBoolean(handle, pointers.WorldState - 0x37EF4);
		}

		public bool IsPlayerLoaded()
		{
			return MemoryTools.ReadInt32(handle, (IntPtr)0x137DC70) != 0;
		}

		public int GetClearCount()
		{
			IntPtr pointer = (IntPtr)MemoryTools.ReadInt32(handle, (IntPtr)0x1378700);

			if (pointer == IntPtr.Zero)
			{
				return -1;
			}

			return MemoryTools.ReadInt32(handle, pointer + 0x3C);
		}

		public int GetPlayerHP()
		{
			return MemoryTools.ReadInt32(handle, pointers.CharacterStats + 0xC);
		}

		public Vector3 GetPlayerPosition()
		{
			float x = GetPlayerX();
			float y = GetPlayerY();
			float z = GetPlayerZ();

			return new Vector3(x, y, z);
		}

		public float GetPlayerX()
		{
			return MemoryTools.ReadFloat(handle, pointers.CharacterPosition + 0x10);
		}

		public float GetPlayerY()
		{
			return MemoryTools.ReadFloat(handle, pointers.CharacterPosition + 0x14);
		}

		public float GetPlayerZ()
		{
			return MemoryTools.ReadFloat(handle, pointers.CharacterPosition + 0x18);
		}

		public CovenantFlags GetCovenant()
		{
			return (CovenantFlags)MemoryTools.ReadByte(handle, pointers.CharacterStats + 0x10B);
		}

		// Note that the last bonfire value doesn't always correspond to a valid bonfire ID.
		public int GetLastBonfire()
		{
			// For whatever reason, bonfire IDs retrieved in this way need to be corrected by a thousand to match the
			// bonfire flags array.
			return MemoryTools.ReadInt32(handle, pointers.WorldState + 0xB04) - 1000;
		}

		public BonfireStates GetBonfireState(BonfireFlags bonfire)
		{
			IntPtr pointer = (IntPtr)0x137E204;
			pointer = (IntPtr)MemoryTools.ReadInt32(handle, pointer);
			pointer = (IntPtr)MemoryTools.ReadInt32(handle, IntPtr.Add(pointer, 0xB48));
			pointer = (IntPtr)MemoryTools.ReadInt32(handle, IntPtr.Add(pointer, 0x24));
			pointer = (IntPtr)MemoryTools.ReadInt32(handle, pointer);

			IntPtr bonfirePointer = (IntPtr)MemoryTools.ReadInt32(handle, IntPtr.Add(pointer, 0x8));

			while (bonfirePointer != IntPtr.Zero)
			{
				int bonfireId = MemoryTools.ReadInt32(handle, IntPtr.Add(bonfirePointer, 0x4));

				if (bonfireId == (int)bonfire)
				{
					int bonfireState = MemoryTools.ReadInt32(handle, IntPtr.Add(bonfirePointer, 0x8));

					return (BonfireStates)bonfireState;
				}

				pointer = (IntPtr)MemoryTools.ReadInt32(handle, pointer);
				bonfirePointer = (IntPtr)MemoryTools.ReadInt32(handle, IntPtr.Add(pointer, 0x8));
			}

			return BonfireStates.Undiscovered;
		}

		public bool CheckFlag(int flag)
		{
			return GetEventFlagState(flag);
		}

		private bool GetEventFlagState(int id)
		{
			if (GetEventFlagAddress(id, out int address, out uint mask))
			{
				uint flags = (uint)MemoryTools.ReadInt32(handle, (IntPtr)address);

				return (flags & mask) != 0;
			}

			return false;
		}

		private bool GetEventFlagAddress(int id, out int address, out uint mask)
		{
			var eventFlagGroups = new Dictionary<string, int>
			{
				{"0", 0x00000},
				{"1", 0x00500},
				{"5", 0x05F00},
				{"6", 0x0B900},
				{"7", 0x11300},
			};

			var eventFlagAreas = new Dictionary<string, int>
			{
				{"000", 00},
				{"100", 01},
				{"101", 02},
				{"102", 03},
				{"110", 04},
				{"120", 05},
				{"121", 06},
				{"130", 07},
				{"131", 08},
				{"132", 09},
				{"140", 10},
				{"141", 11},
				{"150", 12},
				{"151", 13},
				{"160", 14},
				{"170", 15},
				{"180", 16},
				{"181", 17},
			};

			string idString = id.ToString("D8");

			if (idString.Length == 8)
			{
				string group = idString.Substring(0, 1);
				string area = idString.Substring(1, 3);
				int section = int.Parse(idString.Substring(4, 1));
				int number = int.Parse(idString.Substring(5, 3));

				if (eventFlagGroups.ContainsKey(group) && eventFlagAreas.ContainsKey(area))
				{
					int offset = eventFlagGroups[group];
					offset += eventFlagAreas[area] * 0x500;
					offset += section * 128;
					offset += (number - (number % 32)) / 8;

					address = MemoryTools.ReadInt32(handle, (IntPtr)0x137D7D4);
					address = MemoryTools.ReadInt32(handle, (IntPtr)address);
					address += offset;

					mask = 0x80000000 >> (number % 32);

					return true;
				}
			}

			address = 0;
			mask = 0;

			return false;
		}


        public bool IsBossAlive(BossFlags bossType)
        {
            var boss = _bosses.First(i => i.BossType == bossType);
            var memVal = MemoryTools.ReadInt32(process.Handle, pointers.BossState + boss.Offset);
            return !IsBitSet(memVal, boss.Bit);
        }

        private static bool IsBitSet(int b, int pos)
        {
            return (b & (1 << pos)) != 0;
        }

		#region data/lookup tables =======================================================================================================================================

		private struct Boss
		{
			public Boss(BossFlags bossType, int offset, int bit)
			{
				BossType = bossType;
				Offset = offset;
				Bit = bit;
			}

			public BossFlags BossType;
			public int Offset;
			public int Bit;
		}

		private readonly List<Boss> _bosses = new List<Boss>()
		{
			new Boss(BossFlags.GapingDragon      , 0x0   , 1),
			new Boss(BossFlags.Gargoyles         , 0x0   , 2),
			new Boss(BossFlags.Priscilla         , 0x0   , 3),
			new Boss(BossFlags.Sif               , 0x0   , 4),
			new Boss(BossFlags.Pinwheel          , 0x0   , 5),
			new Boss(BossFlags.Nito              , 0x0   , 6),
			new Boss(BossFlags.BedOfChaos        , 0x0   , 9),
			new Boss(BossFlags.Quelaag           , 0x0   , 8),
			new Boss(BossFlags.IronGolem         , 0x0   , 10),
			new Boss(BossFlags.OrnsteinAndSmough , 0x0   , 11),
			new Boss(BossFlags.FourKings         , 0x0   , 12),
			new Boss(BossFlags.Seath             , 0x0   , 13),
			new Boss(BossFlags.Gwyn              , 0x0   , 14),
			new Boss(BossFlags.AsylumDemon       , 0x0   , 15),
			new Boss(BossFlags.TaurusDemon       , 0xF70 , 26),
			new Boss(BossFlags.CapraDemon        , 0xF70 , 25),
			new Boss(BossFlags.MoonlightButterfly, 0x1E70, 27),
			new Boss(BossFlags.SanctuaryGuardian , 0x2300, 31),
			new Boss(BossFlags.Artorias          , 0x2300, 30),
			new Boss(BossFlags.Manus             , 0x2300, 29),
			new Boss(BossFlags.Kalameet          , 0x2300, 27),
			new Boss(BossFlags.Firesage          , 0x3C30, 5),
			new Boss(BossFlags.CeaselessDischarge, 0x3C70, 27),
			new Boss(BossFlags.CentipedeDemon    , 0x3C70, 26),
			new Boss(BossFlags.Gwyndolin         , 0x4670, 27),
			new Boss(BossFlags.StrayDemon        , 0x5A70, 27),
		};

		#endregion
	}
}