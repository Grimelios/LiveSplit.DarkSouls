using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.DarkSouls.Memory
{
	public class MemoryScanner
	{
		[DllImport("kernel32.dll")]
		private static extern uint VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MemoryRegion lpBuffer,
			uint dwLength);

		public static IntPtr Scan(Process process, byte?[] bytes, int offset)
		{
			var regions = GetRegions(process);
			var results = new List<IntPtr>();

			foreach (IntPtr baseAddress in regions.Keys)
			{
				byte[] bytesRead = regions[baseAddress];

				for (int i = 0; i < bytesRead.Length - bytes.Length; i++)
				{
					bool found = true;

					for (int j = 0; j < bytes.Length; j++)
					{
						if (bytes[j] != null && bytes[j] != bytesRead[i + j])
						{
							found = false;

							break;
						}
					}

					if (found)
					{
						results.Add(baseAddress + i);
					}
				}
			}

			return (IntPtr)MemoryTools.ReadInt(process.Handle, results[0] + offset);
		}

		private static Dictionary<IntPtr, byte[]> GetRegions(Process process)
		{
			const uint MEM_COMMIT = 0x1000;
			const uint PAGE_GUARD = 0x100;
			const uint PAGE_EXECUTE_ANY = 0xF0;

			List<MemoryRegion> regions = new List<MemoryRegion>();

			var mainModule = process.MainModule;

			IntPtr baseAddress = mainModule.BaseAddress;
			IntPtr regionAddress = baseAddress;
			IntPtr mainModuleEnd = baseAddress + mainModule.ModuleMemorySize;

			uint queryResult;

			do
			{
				MemoryRegion data = new MemoryRegion();
				queryResult = VirtualQueryEx(process.Handle, regionAddress, out data, (uint)Marshal.SizeOf(data));

				if (queryResult != 0)
				{
					if ((data.State & MEM_COMMIT) != 0 &&
					    (data.Protect & PAGE_GUARD) == 0 &&
					    (data.Protect & PAGE_EXECUTE_ANY) != 0)
					{
						regions.Add(data);
					}

					regionAddress = (IntPtr)((ulong)data.BaseAddress.ToInt64() + data.RegionSize);
				}
			}
			while (queryResult != 0 && regionAddress.ToInt64() < mainModuleEnd.ToInt64());

			var memory = new Dictionary<IntPtr, byte[]>();

			foreach (MemoryRegion memRegion in regions)
			{
				memory[memRegion.BaseAddress] = ReadBytes(process.Handle, memRegion.BaseAddress, (int)memRegion.RegionSize);
			}

			return memory;
		}
		
		private static byte[] ReadBytes(IntPtr handle, IntPtr address, int size)
		{
			int bytesRead = 0;
			byte[] result = new byte[size];

			MemoryTools.ReadProcessMemory(handle, address, result, size, ref bytesRead);

			return result;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct MemoryRegion
		{
			public IntPtr BaseAddress;
			public IntPtr AllocationBase;

			public uint AllocationProtect;
			public ulong RegionSize;
			public uint State;
			public uint Protect;
			public uint Type;
		}
	}
}
