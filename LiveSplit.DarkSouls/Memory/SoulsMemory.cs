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

		public void SetItems(List<ItemId> itemIds)
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

			// In terms of memory layout, "key items" refer to bonfire items, key boss souls, embers, and actual keys.
			List<int> keyIds = new List<int>();
			keyIds.AddRange(ItemFlags.OrderedKeys);
			keyIds.AddRange(ItemFlags.OrderedEmbers);
			keyIds.AddRange(ItemFlags.OrderedBonfireItems);
			keyIds.Add((int)SoulFlags.BequeathedLordSoulShardFourKings);
			keyIds.Add((int)SoulFlags.BequeathedLordSoulShardSeath);
			keyIds.Add((int)SoulFlags.LordSoulBedOfChaos);
			keyIds.Add((int)SoulFlags.LordSoulNito);

			for (int i = 0; i < keyIds.Count; i++)
			{
				keyIds[i] = Utilities.StripHighestDigit(keyIds[i], out int digit);
			}

			foreach (ItemId id in itemIds)
			{
				if (keyIds.Contains(id.BaseId))
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

			bottomlessBoxTracker = null;

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
		}

		public ItemState[] GetItemStates(int baseId, int category)
		{
			// Key items don't share IDs with any other items (except for a few mystery items, but those aren't
			// included in the autosplitter UI).
			bool isKey = Enum.IsDefined(typeof(KeyFlags), baseId);

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

		// This flag is set to true when the "YOU DIED" text appears, and remains true until the load screen appears.
		public bool IsDeathTextVisible()
		{
			return false;
		}

		public bool IsPlayerLoaded()
		{
			return MemoryTools.ReadInt(handle, (IntPtr)0x137DC70) != 0;
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

		public byte GetOverlay()
		{
			return MemoryTools.ReadByte(handle, pointers.Overlay);
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

			IntPtr bonfirePointer = (IntPtr)MemoryTools.ReadInt(handle, IntPtr.Add(pointer, 0x8));

			while (bonfirePointer != IntPtr.Zero)
			{
				int bonfireId = MemoryTools.ReadInt(handle, IntPtr.Add(bonfirePointer, 0x4));

				if (bonfireId == (int)bonfire)
				{
					int bonfireState = MemoryTools.ReadInt(handle, IntPtr.Add(bonfirePointer, 0x8));

					return (BonfireStates)bonfireState;
				}

				pointer = (IntPtr)MemoryTools.ReadInt(handle, pointer);
				bonfirePointer = (IntPtr)MemoryTools.ReadInt(handle, IntPtr.Add(pointer, 0x8));
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