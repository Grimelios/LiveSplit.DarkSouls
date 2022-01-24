using System;
using System.Threading;
using DarkSoulsMemory;

namespace Tester
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var darkSouls = new DarkSouls();

            darkSouls.BonfireWarp(WarpType.DepthsGapingDragonsRoom);

            return;
            darkSouls.Teleport(new Vector3f(0,0,0), 0);
            darkSouls.SetCheat(CheatType.PlayerExterminate, true);
            darkSouls.SetCheat(CheatType.AllNoStaminaConsume, true);
            darkSouls.SetCheat(CheatType.AllNoUpdateAI, true);

            return;



            while (true)
            {
                darkSouls.Refresh();
                
                Console.Clear();
                //Console.WriteLine($"{darkSouls.GetCovenant()} {darkSouls.GetPlayerHealth()}");
                //Console.WriteLine($"{darkSouls.GetTestValue()}");
                Console.WriteLine($"{darkSouls.GetTestValue()} {darkSouls.IsBossDefeated(BossType.AsylumDemon)}");
                Thread.Sleep(50);
            }
        }
    }
}
