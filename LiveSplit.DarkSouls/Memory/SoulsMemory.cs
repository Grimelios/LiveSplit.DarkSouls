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

		public bool IsBossDefeated(BossFlags boss)
		{
			return GetEventFlagState((int)boss);
		}

		public int[] GetBossesKilled()
		{
			int[] totalBossFlags = {
				2, // Gaping Dragon
				3, // Gargoyles
				4, // Priscilla
				5, // Sif
				6, // Pinwheel
				7, // Nito
				9, // Quelaag
				10, // Bed of Chaos
				11, // Iron Golem
				12, // O&S
				13, // Four Kings
				14, // Seath
				15, // Gwyn
				16, // Asylum Demon
				11210000, // Sanctuary Guardian
				11210001, // Artorias
				11210002, // Manus
				11210004, // Kalameet
				11410410, // Firesage
				11410900, // Ceaseless Discharge
				11410901, // Centipede Demon
				11510900, // Gwyndolin
				11010901, // Taurus Demon
				11010902, // Capra Demon
				11200900, // Moonlight Butterfly
				11810900 // Stray Demon
			};

			int bossesKilled = 0;
			bool[] bossValues = new bool[totalBossFlags.Length];

			for (int i = 0; i < totalBossFlags.Length; i++)
			{
				bossValues[i] = GetEventFlagState(totalBossFlags[i]);

				if (bossValues[i])
				{
					bossesKilled++;
				}
			}

			string result = "Bosses: [";

			for (int i = 0; i < bossValues.Length; i++)
			{
				result += bossValues[i] ? "1" : "0";		
				result += i < bossValues.Length - 1 ? "," : "]";
			}

			Console.WriteLine(result);

			return new [] { bossesKilled, totalBossFlags.Length };
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
