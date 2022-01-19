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
                Console.WriteLine($"{darkSouls.CheckFlag(11010700)}");
                Thread.Sleep(50);
            }
        }
    }
}
