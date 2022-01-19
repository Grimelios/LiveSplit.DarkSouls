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
	//public class SoulsMemory
	//{
	//	private Process process;
	//	private IntPtr handle;
	//	//private SoulsPointers pointers;
    //    
	//	public bool ProcessHooked { get; private set; }
	//
	//	public bool Hook()
	//	{
	//		if (ProcessHooked)
	//		{
	//			if (process.HasExited)
	//			{
	//				ProcessHooked = false;
	//				process = null;
	//
	//				return false;
	//			}
	//
	//			//pointers.Refresh(process);
	//
	//			return true;
	//		}
	//
	//		Process[] processes = Process.GetProcessesByName("DARKSOULS");
	//
	//		if (processes.Length > 0)
	//		{
	//			process = processes[0];
	//
	//			if (process.HasExited)
	//			{
	//				return false;
	//			}
	//
	//			// Item trackers are created below (or left as null if not needed).
	//			handle = process.Handle;
	//			//pointers = new SoulsPointers(process);
	//			ProcessHooked = true;
	//		}
	//
	//		return ProcessHooked;
	//	}
		
		//public byte GetWorld()
		//{
		//	return MemoryTools.ReadByte(handle, pointers.Zone + 0xA13);
		//}
		//
		//public byte GetArea()
		//{
		//	return MemoryTools.ReadByte(handle, pointers.Zone + 0xA12);
		//}

		//public bool IsPlayerLoaded()
		//{
		//	  return MemoryTools.ReadInt32(handle, (IntPtr)0x137DC70) != 0;
		//}

		//public int GetClearCount()
		//{
		//	IntPtr pointer = (IntPtr)MemoryTools.ReadInt32(handle, (IntPtr)0x1378700);
		//
		//	if (pointer == IntPtr.Zero)
		//	{
		//		return -1;
		//	}
		//
		//	return MemoryTools.ReadInt32(handle, pointer + 0x3C);
		//}

		//public int GetPlayerHP()
		//{
		//	return MemoryTools.ReadInt32(handle, pointers.CharacterStats + 0xC);
		//}
		//
		//public CovenantFlags GetCovenant()
		//{
		//	return (CovenantFlags)MemoryTools.ReadByte(handle, pointers.CharacterStats + 0x10B);
		//}
		

		//public BonfireStates GetBonfireState(BonfireFlags bonfire)
		//{
		//	IntPtr pointer = (IntPtr)0x137E204;
		//	pointer = (IntPtr)MemoryTools.ReadInt32(handle, pointer);
		//	pointer = (IntPtr)MemoryTools.ReadInt32(handle, IntPtr.Add(pointer, 0xB48));
		//	pointer = (IntPtr)MemoryTools.ReadInt32(handle, IntPtr.Add(pointer, 0x24));
		//	pointer = (IntPtr)MemoryTools.ReadInt32(handle, pointer);
		//
		//	IntPtr bonfirePointer = (IntPtr)MemoryTools.ReadInt32(handle, IntPtr.Add(pointer, 0x8));
		//
		//	while (bonfirePointer != IntPtr.Zero)
		//	{
		//		int bonfireId = MemoryTools.ReadInt32(handle, IntPtr.Add(bonfirePointer, 0x4));
		//
		//		if (bonfireId == (int)bonfire)
		//		{
		//			int bonfireState = MemoryTools.ReadInt32(handle, IntPtr.Add(bonfirePointer, 0x8));
		//
		//			return (BonfireStates)bonfireState;
		//		}
		//
		//		pointer = (IntPtr)MemoryTools.ReadInt32(handle, pointer);
		//		bonfirePointer = (IntPtr)MemoryTools.ReadInt32(handle, IntPtr.Add(pointer, 0x8));
		//	}
		//
		//	return BonfireStates.Undiscovered;
		//}

		//public bool CheckFlag(int flag)
		//{
		//	return GetEventFlagState(flag);
		//}
		//
		//private bool GetEventFlagState(int id)
		//{
		//	if (GetEventFlagAddress(id, out int address, out uint mask))
		//	{
		//		uint flags = (uint)MemoryTools.ReadInt32(handle, (IntPtr)address);
		//
		//		return (flags & mask) != 0;
		//	}
		//
		//	return false;
		//}
		//
		//private bool GetEventFlagAddress(int id, out int address, out uint mask)
		//{
		//	var eventFlagGroups = new Dictionary<string, int>
		//	{
		//		{"0", 0x00000},
		//		{"1", 0x00500},
		//		{"5", 0x05F00},
		//		{"6", 0x0B900},
		//		{"7", 0x11300},
		//	};
		//
		//	var eventFlagAreas = new Dictionary<string, int>
		//	{
		//		{"000", 00},
		//		{"100", 01},
		//		{"101", 02},
		//		{"102", 03},
		//		{"110", 04},
		//		{"120", 05},
		//		{"121", 06},
		//		{"130", 07},
		//		{"131", 08},
		//		{"132", 09},
		//		{"140", 10},
		//		{"141", 11},
		//		{"150", 12},
		//		{"151", 13},
		//		{"160", 14},
		//		{"170", 15},
		//		{"180", 16},
		//		{"181", 17},
		//	};
		//
		//	string idString = id.ToString("D8");
		//
		//	if (idString.Length == 8)
		//	{
		//		string group = idString.Substring(0, 1);
		//		string area = idString.Substring(1, 3);
		//		int section = int.Parse(idString.Substring(4, 1));
		//		int number = int.Parse(idString.Substring(5, 3));
		//
		//		if (eventFlagGroups.ContainsKey(group) && eventFlagAreas.ContainsKey(area))
		//		{
		//			int offset = eventFlagGroups[group];
		//			offset += eventFlagAreas[area] * 0x500;
		//			offset += section * 128;
		//			offset += (number - (number % 32)) / 8;
		//			
		//
		//			address = MemoryTools.ReadInt32(handle, (IntPtr)0x137D7D4);
		//			address = MemoryTools.ReadInt32(handle, (IntPtr)address);
		//			address += offset;
		//
		//			mask = 0x80000000 >> (number % 32);
		//
		//			return true;
		//		}
		//	}
		//
		//	address = 0;
		//	mask = 0;
		//
		//	return false;
		//}
    //}
}