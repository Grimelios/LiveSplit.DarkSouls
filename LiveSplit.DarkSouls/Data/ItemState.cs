using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.DarkSouls.Data
{
	public class ItemState
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

		public bool Satisfies(ItemState target)
		{
			// An item split is considered satisfied if the item count and reinforcement are greater than or equal to
			// the target (rather than being exactly equal).
			return Mods == target.Mods && Reinforcement >= target.Reinforcement && Count >= target.Count;
		}
	}
}
