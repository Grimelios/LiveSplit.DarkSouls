using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.DarkSouls.Data
{
	public class ItemState
	{
		// Modification and reinforcement are only applicable to a subset of item types (weapons, armor, shields, and
		// pyromancy flames). For all other items (or for equipment with mods/reinforcement unavailable), the values
		// are set to -1.
		public ItemState() : this(-1, -1, 0)
		{
		}

		public ItemState(int modification, int reinforcement, int count)
		{
			Modification = modification;
			Reinforcement = reinforcement;
			Count = count;
		}

		public int Modification { get; set; }
		public int Reinforcement { get; set; }
		public int Count { get; set; }
	}
}
