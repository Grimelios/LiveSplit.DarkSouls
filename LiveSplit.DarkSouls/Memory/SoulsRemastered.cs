using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LiveSplit.DarkSouls.Memory
{
    public class SoulsRemastered
    {
        private Process _process;
        private IntPtr _worldProgression;
        private IntPtr _player;
        private IntPtr _misc;
        private bool _hooked;
        
        public SoulsRemastered()
        {
            Hook();
        }

        public bool Hook()
        {
            if (_hooked)
            {
                if (_process.HasExited)
                {
                    _hooked = false;
                    _process = null;

                    return false;
                }
                
                return true;
            }

            Process[] processes = Process.GetProcessesByName("DarkSoulsRemastered");

            if (processes.Length > 0)
            {
                _process = processes[0];

                if (_process.HasExited)
                {
                    return false;
                }
                _hooked = true;
                _worldProgression = (IntPtr)MemoryTools.ReadInt32(_process.Handle, GetBasePtr(_process, new byte?[] { 0x48, 0x8B, 0x0D, null, null, null, null, 0x41, 0xB8, 0x01, 0x00, 0x00, 0x00, 0x44 }));// GetWorldProgressBasePtr(_process);
                _player = GetBasePtr(_process, new byte?[] { 0x48, 0x8B, 0x05, null, null, null, null, 0x45, 0x33, 0xED, 0x48, 0x8B, 0xF1, 0x48, 0x85, 0xC0});// GetWorldProgressBasePtr(_process);
                _misc = GetBasePtr(_process, new byte?[] { 0x48, 0x8B, 0x05, null, null, null, null, 0x48, 0x39, 0x48, 0x68, 0x0F, 0x94, 0xC0, 0xC3 });// GetWorldProgressBasePtr(_process);
                //48 8B 05 xx xx xx xx 45 33 ED 48 8B F1 48 85 C0
                
            }

            return _hooked;
        }

        private IntPtr Scan(Process process, byte?[] scanBytes)
        {
            IntPtr basePtr = IntPtr.Zero;

            //Save as the scan function, except I don't want to immediately read the result
            var regions = MemoryScanner.GetRegions(process);
            foreach (var region in regions)
            {
                var bytes = region.Value;
                for (int i = 0; i < bytes.Length - scanBytes.Length; i++)
                {
                    bool found = true;
                    for (int j = 0; j < scanBytes.Length; j++)
                    {
                        if (scanBytes[j] != null)
                        {
                            if (scanBytes[j] != bytes[i + j])
                            {
                                found = false;
                                break;
                            }
                        }
                    }

                    if (found)
                    {
                        basePtr = region.Key + i;
                    }
                }
            }

            return basePtr;
        }

        private IntPtr ReadPtr(IntPtr ptr)
        {
            return (IntPtr)MemoryTools.ReadInt32(_process.Handle, ptr);
        }


        private IntPtr GetBasePtr(Process process, byte?[] scanBytes)
        {
            IntPtr basePtr = IntPtr.Zero;

            //Save as the scan function, except I don't want to immediately read the result
            var regions = MemoryScanner.GetRegions(process);
            foreach (var region in regions)
            {
                var bytes = region.Value;
                for (int i = 0; i < bytes.Length - scanBytes.Length; i++)
                {
                    bool found = true;
                    for (int j = 0; j < scanBytes.Length; j++)
                    {
                        if (scanBytes[j] != null)
                        {
                            if (scanBytes[j] != bytes[i + j])
                            {
                                found = false;
                                break;
                            }
                        }
                    }

                    if (found)
                    {
                        basePtr = region.Key + i;
                    }
                }
            }

            //Console.WriteLine($"0x{basePtr.ToInt64():X}");
            //var temp = MemoryTools.ReadInt32(process.Handle, basePtr + 3);
            //Console.WriteLine($"0x{temp:X}");
            basePtr = (IntPtr)(basePtr.ToInt64() + MemoryTools.ReadInt32(process.Handle, basePtr + 3) + 7);
            //Console.WriteLine($"0x{basePtr.ToInt64():X}");
            basePtr = (IntPtr)MemoryTools.ReadInt32(process.Handle, basePtr);
            //Console.WriteLine($"0x{basePtr.ToInt32():X}");
            //basePtr = (IntPtr)MemoryTools.ReadInt32(process.Handle, basePtr);
            //Console.WriteLine($"0x{basePtr.ToInt32():X}");

            return basePtr;
        }

        public bool IsBossAlive(BossFlags bossFlags)
        {
            var boss = _bosses.First(i => i.BossFlags == bossFlags);
            
            var memVal = MemoryTools.ReadByte(_process.Handle, _worldProgression + boss.Offset);
            return !IsBitSet(memVal, boss.Bit);
        }

        public int GetGameTimeInMilliseconds()
        {
            return MemoryTools.ReadInt32(_process.Handle, _player + 0xA4);
        }

        public int NewGameType()
        {
            return MemoryTools.ReadByte(_process.Handle, _player + 0x78);
        }

        public void ResetInventoryIndices()
        {
            var basePtr = Scan(_process, new byte?[] { 0x48, 0x8D, 0x15, null, null, null, null, 0xC1, 0xE1, 0x10, 0x49, 0x8B, 0xC6, 0x41, 0x0B, 0x8F, 0x14, 0x02, 0x00, 0x00, 0x44, 0x8B, 0xC6, 0x42, 0x89, 0x0C, 0xB2, 0x41, 0x8B, 0xD6, 0x49, 0x8B, 0xCF });
            basePtr = ReadPtr(basePtr + 3) + 7;
            
            for (int i = 0; i < 20; i++)
            {
                MemoryTools.Write(_process.Handle, basePtr + (0x4 * i), uint.MaxValue);
            }
        }


        private int _initialMillis = 0;
        public bool IsPlayerLoaded()
        {
            //Can't find an address that has this flag, but I did notice that the timer only starts running when the player is loaded.
            var temp = GetGameTimeInMilliseconds();
            
            //Millis is 0 in main menu, when no save is loaded
            if (temp == 0)
            {
                _initialMillis = 0;
                return false;
            }

            //Detect a non 0 value of the clock - a save has just been loaded but the clock might not be running yet
            if(_initialMillis == 0)
            {
                _initialMillis = temp;
                return false;
            }

            //Clock is running since it has been initially loaded. 
            if (_initialMillis != temp)
            {
                return true;
            }

            return false;
        }
        


        private bool IsBitSet(byte b, int pos)
        {
            return (b & (1 << pos)) != 0;
        }


        private struct Boss
        {
            public Boss(BossFlags bossFlags, int offset, int bit)
            {
                BossFlags = bossFlags;
                Offset = offset;
                Bit = bit;
            }

            public BossFlags BossFlags;
            public int Offset;
            public int Bit;
        }

        private readonly List<Boss> _bosses = new List<Boss>()
        {
            new Boss(BossFlags.AsylumDemon       , 0x1   , 7),
            new Boss(BossFlags.Kalameet          , 0x2303, 3),
            new Boss(BossFlags.Gargoyles         , 0x3   , 4),
            new Boss(BossFlags.CapraDemon        , 0xF73 , 1),
            new Boss(BossFlags.CeaselessDischarge, 0x1   , 3),
            new Boss(BossFlags.CentipedeDemon    , 0x3C73, 2),
            new Boss(BossFlags.Quelaag           , 0x2   , 6),
            new Boss(BossFlags.Priscilla         , 0x3   , 3),
            new Boss(BossFlags.Gwyndolin         , 0x4673, 3),
            new Boss(BossFlags.Firesage          , 0x3F30, 5),
            new Boss(BossFlags.OrnsteinAndSmough , 0x2   , 3),
            new Boss(BossFlags.FourKings         , 0x2   , 2),
            new Boss(BossFlags.GapingDragon      , 0x3   , 5),
            new Boss(BossFlags.Nito              , 0x3   , 0),
            new Boss(BossFlags.Sif               , 0x3   , 2),
            new Boss(BossFlags.Gwyn              , 0x2   , 0),
            new Boss(BossFlags.IronGolem         , 0x2   , 4),
            new Boss(BossFlags.MoonlightButterfly, 0x2173, 3),
            new Boss(BossFlags.Pinwheel          , 0x3   , 1),
            new Boss(BossFlags.Seath             , 0x2   , 1),
            new Boss(BossFlags.TaurusDemon       , 0xF73 , 2),
            new Boss(BossFlags.BedOfChaos        , 0x2   , 5),
            new Boss(BossFlags.Manus             , 0x1   , 6),
            new Boss(BossFlags.Artorias          , 0x2303, 6),
        };
    }
}
