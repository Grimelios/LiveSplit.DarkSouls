using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.DarkSouls.Data
{
	public class RunState
	{
		// Item splits are more complex than other splits, so they use special data objects for easier tracking.
		public ItemState ItemData { get; set; }
		public ItemState ItemTarget { get; set; }

		public int GameTime { get; set; }
		public int MaxGameTime { get; set; }
		public int Id { get; set; }
		public int Data { get; set; }
		public int Target { get; set; }

		public bool Flag { get; set; }
	}
}