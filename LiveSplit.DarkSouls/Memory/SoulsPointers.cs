using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.DarkSouls.Memory
{
	public class SoulsPointers
	{
		private IntPtr handle;

		public SoulsPointers(IntPtr handle)
		{
			this.handle = handle;

			Refresh();
		}

		public IntPtr Character { get; private set; }
		public IntPtr CharacterStats { get; private set; }
		public IntPtr CharacterMap { get; private set; }
		public IntPtr CharacterPosition { get; private set; }
		public IntPtr Inventory { get; private set; }
		public IntPtr WorldState { get; private set; }
		public IntPtr Zone { get; private set; }

		public void Refresh()
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

			IntPtr inventory = (IntPtr)MemoryTools.ReadInt(handle, (IntPtr)0x1378700);
			inventory = (IntPtr)MemoryTools.ReadInt(handle, inventory + 0x8);
			Inventory = inventory + 0x1B8;

			WorldState = (IntPtr)MemoryTools.ReadInt(handle, (IntPtr)0x13784A0);
			Zone = (IntPtr)MemoryTools.ReadInt(handle, (IntPtr)0x137E204);
		}
	}
}