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
