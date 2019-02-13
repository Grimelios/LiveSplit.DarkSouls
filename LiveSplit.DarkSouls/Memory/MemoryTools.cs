using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.DarkSouls.Memory
{
	/**
	 * Adapted from CapitaineToinon's repositories.
	 */
	public static class MemoryTools
	{
		public static bool ReadBoolean(IntPtr handle, IntPtr address)
		{
			int bytesRead = 0;
			byte[] bytes = new byte[1];

			Kernel.ReadProcessMemory(handle, address, bytes, bytes.Length, ref bytesRead);

			return BitConverter.ToBoolean(bytes, 0);
		}

		public static byte ReadByte(IntPtr handle, IntPtr address)
		{
			int bytesRead = 0;
			byte[] bytes = new byte[1];

			Kernel.ReadProcessMemory(handle, address, bytes, bytes.Length, ref bytesRead);

			return bytes[0];
		}

		public static int ReadInt(IntPtr handle, IntPtr address)
		{
			int bytesRead = 0;
			byte[] bytes = new byte[4];

			Kernel.ReadProcessMemory(handle, address, bytes, bytes.Length, ref bytesRead);

			return BitConverter.ToInt32(bytes, 0);
		}

		public static float ReadFloat(IntPtr handle, IntPtr address)
		{
			int bytesRead = 0;
			byte[] bytes = new byte[4];

			Kernel.ReadProcessMemory(handle, address, bytes, bytes.Length, ref bytesRead);

			return BitConverter.ToSingle(bytes, 0);
		}
	}
}
