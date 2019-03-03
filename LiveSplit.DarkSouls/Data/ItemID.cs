using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.DarkSouls.Data
{
	[DebuggerDisplay("BaseId = {BaseId}, Category = {Category}")]
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
			return BaseId == other.BaseId && Category == other.Category;
		}

		public override int GetHashCode()
		{
			return BaseId.GetHashCode() + Category.GetHashCode();
		}
	}
}
