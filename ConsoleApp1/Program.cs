using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LiveSplit.DarkSouls;
using LiveSplit.DarkSouls.Memory;

namespace ConsoleApp1
{
    internal class Program
    {
        public struct Boss
        {
            public Boss(string name, int offset, int bit)
            {
                Name = name;
                Offset = offset;
                Bit = bit;
            }

            public string Name;
            public int Offset;
            public int Bit;
        }

        private static List<Boss> _bosses = new List<Boss>()
        {
            new Boss("Asylum Demon"                                 , 0x1   , 7),
            new Boss("Black Dragon Kalameet"                        , 0x2303, 3),
            new Boss("Bell Gargoyles"                               , 0x3   , 4),
            new Boss("Capra Demon"                                  , 0xF73 , 1),
            new Boss("Ceaseless Discharge"                          , 0x1   , 3),
            new Boss("Centipede Demon"                              , 0x3C73, 2),
            new Boss("Chaos Witch Quelaag"                          , 0x2   , 6),
            new Boss("Crossbreed Priscilla"                         , 0x3   , 3),   
            new Boss("Dark Sun Gwyndolin"                           , 0x4673, 3),
            new Boss("Demon Firesage"                               , 0x3F30, 5),
            new Boss("Dragon Slayer Ornstein & Executioner Smough"  , 0x2   , 3),
            new Boss("Four Kings"                                   , 0x2   , 2),
            new Boss("Gaping Dragon"                                , 0x3   , 5),
            new Boss("Gravelord Nito"                               , 0x3   , 0),
            new Boss("Great Grey Wolf Sif"                          , 0x3   , 2),
            new Boss("Gwyn, Lord of Cinder"                         , 0x2   , 0),
            new Boss("Iron Golem"                                   , 0x2   , 4),
            new Boss("Moonlight Butterfly"                          , 0x2173, 3),
            new Boss("Pinwheel "                                    , 0x3   , 1),
            new Boss("Seath the Scaleless"                          , 0x2   , 1),
            new Boss("Taurus Demon"                                 , 0xF73 , 2),
            new Boss("Bed of Chaos "                                , 0x2   , 5),
            new Boss("Manus, Father of the Abyss"                   , 0x1   , 6),
            new Boss("Knight Artorias"                              , 0x2303, 6),
        };

        private Dictionary<string, bool> BosAlive = new Dictionary<string, bool>();


        static void Main(string[] args)
        {
            SoulsRemastered s = new LiveSplit.DarkSouls.Memory.SoulsRemastered();
            while (true)
            {
                var d = s.IsBossAlive(BossFlags.AsylumDemon);
                var ad = s.IsBossAlive(BossFlags.Firesage);

                Console.Clear();
                Console.WriteLine(s.GetGameTimeInMilliseconds() + " " + s.IsPlayerLoaded());
                Thread.Sleep(16);
            }
            

            var process = Process.GetProcessesByName("DarkSoulsRemastered").FirstOrDefault();
            
            byte?[] scanBytes = { 0x48, 0x8B, 0x0D, null, null, null, null, 0x41, 0xB8, 0x01, 0x00, 0x00, 0x00, 0x44 };

            IntPtr worldProgression = IntPtr.Zero;

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
           

            Console.WriteLine($"0x{worldProgression.ToInt64():X}");
            var temp = MemoryTools.ReadInt32(process.Handle, worldProgression + 3);
            Console.WriteLine($"0x{temp:X}");
            worldProgression = (IntPtr)(worldProgression.ToInt64() + MemoryTools.ReadInt32(process.Handle, worldProgression + 3) + 7);
            Console.WriteLine($"0x{worldProgression.ToInt64():X}");
            worldProgression = (IntPtr)MemoryTools.ReadInt32(process.Handle, worldProgression);
            Console.WriteLine($"0x{worldProgression.ToInt32():X}");
            worldProgression = (IntPtr)MemoryTools.ReadInt32(process.Handle, worldProgression);
            Console.WriteLine($"0x{worldProgression.ToInt32():X}");


            var bosStates = GetBossState(process, worldProgression);

            
            while (true)
            {
                var newStates = GetBossState(process, worldProgression);
                foreach (var state in newStates)
                {
                    if (bosStates[state.Key] != state.Value)
                    {
                        Console.WriteLine($"Boss died: {state.Key}");
                    }
                }
                bosStates = newStates;
                Thread.Sleep(16);
            }
        }


        public static Dictionary<string, bool> GetBossState(Process process, IntPtr worldProgress)
        {
            var bosStates = new Dictionary<string, bool>();
            foreach (var boss in _bosses)
            {
                var memVal = MemoryTools.ReadByte(process.Handle, worldProgress + boss.Offset);
                bosStates.Add(boss.Name, !IsBitSet(memVal, boss.Bit));
            }

            return bosStates;
        }

        public static bool IsBitSet(byte b, int pos)
        {
            return (b & (1 << pos)) != 0;
        }
    }
}
