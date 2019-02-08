using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveSplit.DarkSouls.Data;

namespace LiveSplit.DarkSouls.Memory
{
	/**
	 * Adapted from CapitaineToinon's repositories.
	 */
	public class SoulsMemory
	{
		private Process process;
		private IntPtr handle;

		private Dictionary<int, int> bonfireMap = new Dictionary<int, int>();

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
				ProcessHooked = true;
			}

			return ProcessHooked;
		}

		public int GetGameTimeInMilliseconds()
		{
			IntPtr pointer = (IntPtr)MemoryTools.ReadInt(handle, Pointers.GameTime);

			return MemoryTools.ReadInt(handle, IntPtr.Add(pointer, 0x68));
		}

		public BonfireStates GetBonfireState(BonfireFlags bonfire)
		{
			/*
			1001960
			1011961
			1011962
			1011964
			1021960
			1101960
			1201961
			1211950
			1211961
			1211962
			1211963
			1211964
			1301960
			1301961
			1311950
			1311960
			1311961
			1321960
			1321961
			1321962
			1401960
			1401961
			1401962
			1411950
			1411960
			1411961
			1411962
			1411963
			1411964
			1501961
			1511950
			1511960
			1511961
			1511962
			1601950
			1601961
			1701950
			1701960
			1701961
			1701962
			1801960
			1811960
			1811961
			*/

			IntPtr pointer = (IntPtr)0x137E204;
			pointer = (IntPtr)MemoryTools.ReadInt(handle, pointer);
			pointer = (IntPtr)MemoryTools.ReadInt(handle, IntPtr.Add(pointer, 0xB48));
			pointer = (IntPtr)MemoryTools.ReadInt(handle, IntPtr.Add(pointer, 0x24));
			pointer = (IntPtr)MemoryTools.ReadInt(handle, pointer);

			IntPtr bonfirePointer = (IntPtr)MemoryTools.ReadInt(handle, IntPtr.Add(pointer, 8));

			while (bonfirePointer != IntPtr.Zero)
			{
				int bonfireId = MemoryTools.ReadInt(handle, IntPtr.Add(bonfirePointer, 4));

				//if (bonfireId == (int)bonfire)
				{
					int bonfireState = MemoryTools.ReadInt(handle, IntPtr.Add(bonfirePointer, 8));

					//return (BonfireStates)bonfireState;

					if (!bonfireMap.TryGetValue(bonfireId, out int existingState))
					{
						bonfireMap.Add(bonfireId, bonfireState);
					}
					else if (bonfireState != existingState)
					{
						bonfireMap[bonfireId] = bonfireState;

						Console.WriteLine($"Bonfire: {bonfireId}|{bonfireState}");
					}
				}

				pointer = (IntPtr)MemoryTools.ReadInt(handle, pointer);
				bonfirePointer = (IntPtr)MemoryTools.ReadInt(handle, IntPtr.Add(pointer, 8));
			}

			return BonfireStates.Undiscovered;
		}

		public bool IsBossDefeated(BossFlags boss)
		{
			return GetEventFlagState((int)boss);
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
