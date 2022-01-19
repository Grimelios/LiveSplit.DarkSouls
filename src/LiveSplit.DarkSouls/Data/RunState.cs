using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSoulsMemory;

namespace LiveSplit.DarkSouls.Data
{
	public class RunState
	{
		// Item splits are more complex than other splits, so they use a special data object for easier tracking. Same
		// is true for zones (although zones are way simpler).
		public Item TargetItem { get; set; }
		public ZoneType Zone { get; set; }

		public int GameTime { get; set; }
		public int MaxGameTime { get; set; }
		public int Id { get; set; }
		public int Data { get; set; }
		public int Target { get; set; }

		public bool Flag { get; set; }
	}
}