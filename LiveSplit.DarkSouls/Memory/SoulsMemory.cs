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

		public int GetGameTimeInMilliseconds()
		{
			IntPtr pointer = (IntPtr)MemoryTools.ReadInt(handle, pointers.GameTime);

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

		public int GetClearCount()
		{
			IntPtr pointer = (IntPtr)MemoryTools.ReadInt(handle, pointers.ClearCount);

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

		public void GetItem()
		{
			// InventoryStart = pointers.CharacterStats + 0x1B8
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