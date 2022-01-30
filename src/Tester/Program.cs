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

            var items = darkSouls.GetCurrentInventoryItems();


            //darkSouls.BonfireWarp(WarpType.DarkrootBasinBonfire);
            //darkSouls.SetCheat(CheatType.PlayerExterminate, true);
            //darkSouls.SetCheat(CheatType.AllNoStaminaConsume, true);

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
