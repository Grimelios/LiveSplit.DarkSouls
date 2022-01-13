using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DarkSoulsMemory;
using LiveSplit.DarkSouls.Memory;

namespace Tester
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //var ptrs = new SoulsPointers(Process.GetProcessesByName("DARKSOULS").First());


            var darkSouls = new DarkSouls();

            while (true)
            {
                darkSouls.Refresh();

                Console.Clear();
                Console.WriteLine($"IGT {darkSouls.GetGameTimeInMilliseconds()} test value: {darkSouls.GetCurrentTestValue()} {darkSouls.GetPlayerPosition()}");
                Thread.Sleep(50);
            }
        }
    }
}
