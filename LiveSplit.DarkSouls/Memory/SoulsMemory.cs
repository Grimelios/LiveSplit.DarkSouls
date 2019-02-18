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
	/**
	 * Adapted from CapitaineToinon's repositories.
	 */
	public class SoulsMemory
	{
		private Process process;
		private IntPtr handle;
		private SoulsPointers pointers;

		// Tracking items is more complex than other split types. For efficiency (given that inventory items are stored
		// in a list), addresses of split items are tracked as they are detected.
		private Dictionary<int, IntPtr> itemTracker;

		// When items are picked up in-game, they're appended to a list in memory (in order). When items are dropped,
		// however, earlier slots in that list can open up. Picking up new items fills those slots first (before
		// appending). As such, tracking those open slots allows the code to immediately check new items as they're
		// acquired (rather than having to loop through the full list from the beginning).
		private List<int> openSlots;

		public SoulsMemory()
		{
			itemTracker = new Dictionary<int, IntPtr>();
		}

		public bool ProcessHooked { get; private set; }

		public bool Hook()
		{
			if (ProcessHooked && process.HasExited)
			{
				ProcessHooked = false;
				process = null;

				return false;
			}

			Process[] processes = Process.GetProcessesByName("DARKSOULS");

			if (processes.Length > 0)
			{
				process = processes[0];
				handle = process.Handle;
				pointers = new SoulsPointers(handle);
				ProcessHooked = true;
			}

			return ProcessHooked;
		}

		// This function is called once when the timer starts (or when the process is hooked if the timer was already
		// running).
		public void RefreshItems(int[] itemIds)
		{
			itemTracker.Clear();

			if (itemIds == null)
			{
				return;
			}

			IntPtr keyStart = pointers.CharacterStats + 0x342;
			IntPtr itemStart = pointers.CharacterStats + 0xA40;
			IntPtr address = itemStart;

			int size = GetInventorySize();

			for (int i = 0; i < size; i++)
			{
				int id = MemoryTools.ReadInt(handle, address);

				if (itemIds.Contains(id))
				{
					itemTracker.Add(id, address);
				}
			}
		}

		public int GetGameTimeInMilliseconds()
		{
			IntPtr pointer = (IntPtr)MemoryTools.ReadInt(handle, (IntPtr)0x1378700);

			return MemoryTools.ReadInt(handle, IntPtr.Add(pointer, 0x68));
		}

		public byte GetActiveAnimation()
		{
			return MemoryTools.ReadByte(handle, pointers.Character + 0xC5C);
		}

		public int GetForcedAnimation()
		{
			return MemoryTools.ReadInt(handle, pointers.Character + 0xFC);
		}

		// Every time the player uses an item that requires a yes/no confirmation box, the ID of the item can be
		// retreived. That ID remains in place until the item's animation is complete.
		public int GetPromptedItem()
		{
			return MemoryTools.ReadInt(handle, pointers.Character + 0x62C);
		}

		// "Prompted menu" here refers to the small menu near the bottom of the screen (such as yes/no confirmation
		// boxes). Each box has a unique ID (based on the text displayed).
		public int GetPromptedMenu()
		{
			// The prompted menu ID is stored using a static address.
			return MemoryTools.ReadInt(handle, (IntPtr)0xEE33E0);
		}

		public int GetClearCount()
		{
			IntPtr pointer = (IntPtr)MemoryTools.ReadInt(handle, (IntPtr)0x1378700);

			if (pointer == IntPtr.Zero)
			{
				return -1;
			}

			return MemoryTools.ReadInt(handle, pointer + 0x3C);
		}

		public float GetPlayerX()
		{
			return MemoryTools.ReadFloat(handle, pointers.CharacterPosition + 0x10);
		}

		public int GetInventorySize()
		{
			// 0x128 = Count
			// 0x324 = Keys
			// 0xA40 = Items

			return MemoryTools.ReadInt(handle, pointers.CharacterStats + 0x128);
		}

		public ItemState GetItemState(int itemId)
		{
			// It's assumed that an item ID will only be queried if it was present in the list of saved splits.
			IntPtr address = itemTracker[itemId];

			// For upgradeable items, mods and reinforcement are represented through the ID directly. The hundreds
			// digit (third from the right) represents mods, while the ones digit (far right) represents reinforcement.
			// All such IDs are at least five digits long.
			int actualId = MemoryTools.ReadInt(handle, address);
			int mods = (actualId % 1000) / 100;
			int reinforcement = actualId % 10;
			int count = MemoryTools.ReadInt(handle, address + 0x4);

			return new ItemState(mods, reinforcement, count);
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
			return MemoryTools.ReadInt(handle, pointers.WorldState + 0xB04) - 1000;
		}

		public BonfireStates GetBonfireState(BonfireFlags bonfire)
		{
			IntPtr pointer = (IntPtr)0x137E204;
			pointer = (IntPtr)MemoryTools.ReadInt(handle, pointer);
			pointer = (IntPtr)MemoryTools.ReadInt(handle, IntPtr.Add(pointer, 0xB48));
			pointer = (IntPtr)MemoryTools.ReadInt(handle, IntPtr.Add(pointer, 0x24));
			pointer = (IntPtr)MemoryTools.ReadInt(handle, pointer);

			IntPtr bonfirePointer = (IntPtr)MemoryTools.ReadInt(handle, IntPtr.Add(pointer, 8));

			while (bonfirePointer != IntPtr.Zero)
			{
				int bonfireId = MemoryTools.ReadInt(handle, IntPtr.Add(bonfirePointer, 4));

				if (bonfireId == (int)bonfire)
				{
					int bonfireState = MemoryTools.ReadInt(handle, IntPtr.Add(bonfirePointer, 8));

					return (BonfireStates)bonfireState;
				}

				pointer = (IntPtr)MemoryTools.ReadInt(handle, pointer);
				bonfirePointer = (IntPtr)MemoryTools.ReadInt(handle, IntPtr.Add(pointer, 8));
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
				uint flags = (uint)MemoryTools.ReadInt(handle, (IntPtr)address);

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

					address = MemoryTools.ReadInt(handle, (IntPtr)0x137D7D4);
					address = MemoryTools.ReadInt(handle, (IntPtr)address);
					address += offset;

					mask = 0x80000000 >> (number % 32);

					return true;
				}
			}

			address = 0;
			mask = 0;

			return false;
		}
	}
}