using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.DarkSouls.Memory
{
	public static class MemoryTools
	{
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize,
			ref int lpNumberOfBytesRead);

		[DllImport("kernel32.dll")]
		public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize,
			uint lpNumberOfBytesWritten);

		public static bool ReadBoolean(IntPtr handle, IntPtr address)
		{
			int bytesRead = 0;
			byte[] bytes = new byte[1];

			ReadProcessMemory(handle, address, bytes, bytes.Length, ref bytesRead);

			return BitConverter.ToBoolean(bytes, 0);
		}

		public static byte ReadByte(IntPtr handle, IntPtr address)
		{
			int bytesRead = 0;
			byte[] bytes = new byte[1];

			ReadProcessMemory(handle, address, bytes, bytes.Length, ref bytesRead);

			return bytes[0];
		}

		public static int ReadInt(IntPtr handle, IntPtr address)
		{
			int bytesRead = 0;
			byte[] bytes = new byte[4];

			ReadProcessMemory(handle, address, bytes, bytes.Length, ref bytesRead);

			return BitConverter.ToInt32(bytes, 0);
		}

		public static float ReadFloat(IntPtr handle, IntPtr address)
		{
			int bytesRead = 0;
			byte[] bytes = new byte[4];

			ReadProcessMemory(handle, address, bytes, bytes.Length, ref bytesRead);

			return BitConverter.ToSingle(bytes, 0);
		}

		public static void Write(IntPtr handle, IntPtr address, uint value)
		{
			uint bytesWritten = 0;

			WriteProcessMemory(handle, address, BitConverter.GetBytes(value), 4, bytesWritten);
		}
	}
}