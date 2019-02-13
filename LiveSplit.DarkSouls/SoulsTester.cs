using System;
using System.IO;
using System.Threading;

namespace LiveSplit.DarkSouls
{
	public class SoulsTester
	{
		private const int Tick = 60;
		
		public static void Main(string[] args)
		{
			bool formTesting = true;

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
