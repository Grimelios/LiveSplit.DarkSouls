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

		// Tracking items is a bit complex. It makes more sense for the memory class to manage that complexity rather
		// than the main component class.
		private ItemTracker keyTracker;
		private ItemTracker itemTracker;

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

				pointers.Refresh();

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

				handle = process.Handle;
				pointers = new SoulsPointers(handle);

				keyTracker = new ItemTracker(pointers, handle, (int)InventoryFlags.KeyStart,
					(int)InventoryFlags.KeyCount);
				itemTracker = new ItemTracker(pointers, handle, (int)InventoryFlags.ItemStart,
					(int)InventoryFlags.ItemCount);

				ProcessHooked = true;
			}

			return ProcessHooked;
		}

		// This function is called once per update tick if item splits are in use (regardless of whether an item split
		// is active). In contrast, the function below effectively resets the tracker at the start of a new run.
		public void RefreshItems()
		{
			itemTracker.Refresh();
		}

		public void SetItems(List<ItemId> itemIds)
		{
			itemTracker.Clear();

			if (itemIds == null)
			{
				return;
			}

			itemTracker.SetItems(itemIds);

			// 0 = consumable
			// 140 = spear
			// 150 = fist
			// 196 = dagger (or maybe short sword?)
			// 200 = sword
			// 250 = armor (maybe light armor?)
			// 300 = shield
			// 450 = helm (maybe heavy armor?)

			/*
			for (int i = 0; i < size; i++)
			{
				IntPtr baseItem = address + i * 0x1C;

				int id = MemoryTools.ReadInt(handle, baseItem);
				int type = MemoryTools.ReadInt(handle, baseItem + 0x1B);
				int count = MemoryTools.ReadInt(handle, baseItem + 0x4);

				Console.WriteLine($"Id: {id}, Type: {type}, Count: {count}");

				if (itemIds.Contains(id))
				{
					itemTracker.Add(id, address);
				}
			}
			*/
		}

		public ItemState[] GetItemStates(int baseId, int category)
		{
			// Key items don't share IDs with any other items (except for a few mystery items, but those aren't
			// included in the autosplitter UI).
			ItemTracker tracker = Enum.IsDefined(typeof(KeyFlags), baseId) ? keyTracker : itemTracker;

			return tracker.GetItemStates(baseId, category);
		}

		public void ItemTest()
		{
			IntPtr address = pointers.Inventory + (int)InventoryFlags.ItemStart;

			int count = MemoryTools.ReadInt(handle, pointers.Inventory + (int)InventoryFlags.ItemCount);

			Console.WriteLine("Count: " + count);
			Console.WriteLine("-");

			for (int i = 0; i < count; i++)
			{
				int id = MemoryTools.ReadInt(handle, address);
				int category = MemoryTools.ReadByte(handle, address - 0x1);
				int itemCount = MemoryTools.ReadInt(handle, address + 0x4);

				Console.WriteLine($"Id: {id}, Category: {category.ToString("X")[0]}, Count: {itemCount}");

				address += 0x1C;
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
		public byte GetPromptedMenu()
		{
			// The prompted menu ID is stored using a static address.
			return MemoryTools.ReadByte(handle, (IntPtr)0x12E33E0);
		}

		public bool IsLoadScreenVisible()
		{
			return MemoryTools.ReadBoolean(handle, pointers.WorldState - 0x37EF4);
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