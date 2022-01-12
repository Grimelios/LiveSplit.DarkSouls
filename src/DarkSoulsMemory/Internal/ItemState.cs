using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkSoulsMemory.Internal
{
    internal class ItemState
	{
		// Modification and reinforcement are only applicable to a subset of item types. For all other items (or for
		// equipment with mods/reinforcement unavailable), the values are set to -1.
		public ItemState() : this(-1, -1, 0)
		{
		}

		public ItemState(int mods, int reinforcement, int count)
		{
			Mods = mods;
			Reinforcement = reinforcement;
			Count = count;
		}

		public int Mods { get; set; }
		public int Reinforcement { get; set; }
		public int Count { get; set; }
	}
}
