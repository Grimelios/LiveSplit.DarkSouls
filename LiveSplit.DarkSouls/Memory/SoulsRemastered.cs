using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.DarkSouls.Memory
{
    internal class SoulsRemastered
    {
        private Process _process;
        private IntPtr _worldProgression;
        public bool ProcessHooked { get; private set; }
        public SoulsRemastered()
        {
            Hook();
        }

        public bool Hook()
        {
            if (ProcessHooked)
            {
                if (_process.HasExited)
                {
                    ProcessHooked = false;
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
                ProcessHooked = true;
                _worldProgression = GetWorldProgressBasePtr(_process);
            }

            return ProcessHooked;
        }

        private IntPtr GetWorldProgressBasePtr(Process process)
        {
            byte?[] scanBytes = { 0x48, 0x8B, 0x0D, null, null, null, null, 0x41, 0xB8, 0x01, 0x00, 0x00, 0x00, 0x44 };

            IntPtr worldProgression = IntPtr.Zero;

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
                        worldProgression = region.Key + i;
                    }
                }
            }

            //Console.WriteLine($"0x{worldProgression.ToInt64():X}");
            var temp = MemoryTools.ReadInt32(process.Handle, worldProgression + 3);
            //Console.WriteLine($"0x{temp:X}");
            worldProgression = (IntPtr)(worldProgression.ToInt64() + MemoryTools.ReadInt32(process.Handle, worldProgression + 3) + 7);
            //Console.WriteLine($"0x{worldProgression.ToInt64():X}");
            worldProgression = (IntPtr)MemoryTools.ReadInt32(process.Handle, worldProgression);
            //Console.WriteLine($"0x{worldProgression.ToInt32():X}");
            worldProgression = (IntPtr)MemoryTools.ReadInt32(process.Handle, worldProgression);
            //Console.WriteLine($"0x{worldProgression.ToInt32():X}");

            return worldProgression;
        }

        public bool IsBossAlive(BossFlags bossFlags)
        {
            var boss = _bosses.First(i => i.BossFlags == bossFlags);
            
            var memVal = MemoryTools.ReadByte(_process.Handle, _worldProgression + boss.Offset);
            return !IsBitSet(memVal, boss.Bit);
        }

        //public Dictionary<string, bool> GetBossState()
        //{
        //    var bosStates = new Dictionary<string, bool>();
        //    foreach (var boss in _bosses)
        //    {
        //        var memVal = MemoryTools.ReadByte(_process.Handle, _worldProgression + boss.Offset);
        //        bosStates.Add(boss.Name, !IsBitSet(memVal, boss.Bit));
        //    }
        //    return bosStates;
        //}

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
