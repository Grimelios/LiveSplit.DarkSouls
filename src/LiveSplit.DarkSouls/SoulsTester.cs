using System;
using System.IO;
using System.Threading;
using LiveSplit.DarkSouls.Data;

namespace LiveSplit.DarkSouls
{
	public class SoulsTester
	{
		private const int Tick = 30;

		public static void Main(string[] args)
        {
            //new SoulsForm().ShowDialog();
            //return;

			bool formTesting = args.Length > 0;

			if (formTesting)
			{
				new SoulsForm().ShowDialog();
			}
			else
			{
				SoulsComponent component = new SoulsComponent();

				while (true)
				{
					component.Refresh();
                    Thread.Sleep((int)(1000f / Tick));
				}
			}
		}
	}
}