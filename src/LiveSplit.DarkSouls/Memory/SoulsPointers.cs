using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.DarkSouls.Memory
{
	public class SoulsPointers
	{

		private IntPtr handle;

		public SoulsPointers(Process process)
		{
			handle = process.Handle;
            Refresh(process);
		}
		
		public IntPtr CharacterStats { get; private set; }
		//public IntPtr CharacterMap { get; private set; }
		public IntPtr Inventory { get; private set; }
		public IntPtr WorldState { get; private set; }
		public IntPtr Zone { get; private set; }

     
		public void Refresh(Process process)
		{
		
			//6d8a1f0

			if (MemoryScanner.TryScan(process, new byte?[] { 0x8B, 0x0D, null, null, null, null, 0x8B, 0x7E, 0x1C, 0x8B, 0x49, 0x08, 0x8B, 0x46, 0x20, 0x81, 0xC1, 0xB8, 0x01, 0x00, 0x00, 0x57, 0x51, 0x32, 0xDB }, out IntPtr characterBasePtr))
            {
                characterBasePtr = characterBasePtr + 2;
                characterBasePtr = (IntPtr)MemoryTools.ReadInt32(process.Handle, characterBasePtr);


                IntPtr characterStats = (IntPtr)MemoryTools.ReadInt32(handle, characterBasePtr);
                characterStats = (IntPtr)MemoryTools.ReadInt32(handle, characterStats + 0x8);
                CharacterStats = characterStats;

                IntPtr inventory = (IntPtr)MemoryTools.ReadInt32(handle, (IntPtr)characterBasePtr);
                inventory = (IntPtr)MemoryTools.ReadInt32(handle, inventory + 0x8);
                Inventory = inventory + 0x1B8;
            }

            //IntPtr characterStats = (IntPtr)MemoryTools.ReadInt32(handle, (IntPtr)0x1378700);
			//characterStats = (IntPtr)MemoryTools.ReadInt32(handle, characterStats + 0x8);
			//CharacterStats = characterStats;
			

			WorldState = (IntPtr)MemoryTools.ReadInt32(handle, (IntPtr)0x13784A0);
			Zone = (IntPtr)MemoryTools.ReadInt32(handle, (IntPtr)0x137E204);
		}
	}
}