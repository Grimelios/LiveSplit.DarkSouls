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
            //var items = darkSouls.GetCurrentInventoryItems();
            //
            //foreach (var item in items)
            //{
            //    Console.WriteLine(item.Type + " " + item.Quantity);
            //}
            //
            //Console.ReadKey();
            //return;

            

            while (true)
            {
                darkSouls.Refresh();

                var stuff = darkSouls.GetCurrentInventoryItems();

                Console.Clear();
                var test = darkSouls.GetCurrentTestValue();
                Console.WriteLine($"{darkSouls.GetBonfireState(Bonfire.UndeadAsylumInterior)} {darkSouls.GetZone()}");
                Thread.Sleep(50);
            }
        }
    }
}
