using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.DarkSouls.Data
{
	public class SplitLists
	{
		public ItemLists Items { get; set; }

		public string[] Bonfires { get; set; }
		public string[] Bosses { get; set; }
		public string[] Covenants { get; set; }
		public string[] Npcs { get; set; }
	}
}
