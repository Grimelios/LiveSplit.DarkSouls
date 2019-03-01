using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.DarkSouls.Data
{
	public class ItemId : IEquatable<ItemId>
	{
		public ItemId(int baseId, int category)
		{
			BaseId = baseId;
			Category = category;
		}

		public int BaseId { get; }
		public int Category { get; }

		public bool Equals(ItemId other)
		{
			// By design, null checks and self-equality checks aren't needed.
			return BaseId == other.BaseId && Category == other.Category;
		}
	}
}
