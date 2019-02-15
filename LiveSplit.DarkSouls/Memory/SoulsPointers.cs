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

			IntPtr characterStats = (IntPtr)MemoryTools.ReadInt(handle, (IntPtr)0x1378700);
			characterStats = (IntPtr)MemoryTools.ReadInt(handle, characterStats + 0x8);
			CharacterStats = characterStats;

			CharacterMap = (IntPtr)MemoryTools.ReadInt(handle, character + 0x28);
			CharacterPosition = (IntPtr)MemoryTools.ReadInt(handle, CharacterMap + 0x1C);
			WorldState = (IntPtr)MemoryTools.ReadInt(handle, (IntPtr)0x13784A0);
		}

		public IntPtr ClearCount => (IntPtr)0x1378700;
		public IntPtr Character { get; }
		public IntPtr CharacterStats { get; }
		public IntPtr CharacterMap { get; }
		public IntPtr CharacterPosition { get; }
		public IntPtr GameTime => (IntPtr)0x1378700;
		public IntPtr WorldState { get; }
	}
}