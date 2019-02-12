using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.DarkSouls.Memory
{
	public class SoulsPointers
	{
		public SoulsPointers(IntPtr handle)
		{
			IntPtr character = (IntPtr)MemoryTools.ReadInt(handle, (IntPtr)0x137DC70);
			character = (IntPtr)MemoryTools.ReadInt(handle, character + 0x4);
			character = (IntPtr)MemoryTools.ReadInt(handle, character);
			Character = character;

			WorldState = (IntPtr)MemoryTools.ReadInt(handle, (IntPtr)0x13784A0);
		}

		public IntPtr Character { get; }
		public IntPtr GameTime => (IntPtr)0x1378700;
		public IntPtr WorldState { get; }
	}
}
