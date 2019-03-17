using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveSplit.DarkSouls.Memory;

namespace LiveSplit.DarkSouls.Data
{
	public class ItemTracker : TrackerBase
	{
		// Every item object is separated by this amount.
		private const int Step = 0x1C;

		private IntPtr itemCount;

		private int totalItems;

		public ItemTracker(IntPtr inventory, IntPtr handle, int start, int count) :
			base(handle, inventory + start, Step, 0x4)
		{
			itemCount = inventory + count;
		}

		public override void SetItems(List<ItemId> itemIds)
		{
			totalItems = MemoryTools.ReadInt(Handle, itemCount);

			SetItems(itemIds, totalItems);
		}

		public override void Refresh()
		{
			int newTotal = MemoryTools.ReadInt(Handle, itemCount);

			if (newTotal != totalItems)
			{
				if (newTotal > totalItems)
				{
					// It's pretty common to acquire multiple items at once (such as an entire armor set).
					int pickupCount = newTotal - totalItems;

					AddItems(pickupCount);
				}
				// This means that items were lost (dropped, sold, etc.).
				else
				{
					// I'm fairly certain that only one item can be removed from the inventory at one time (via drop or
					// giving items to an NPC), but I'm not completely sure. A loop guarantees that the autosplitter's
					// internal inventory state remains correct.
					int dropCount = totalItems - newTotal;

					RemoveItems(dropCount);
				}

				totalItems = newTotal;
			}
		}

		protected override ItemId ComputeItemId(IntPtr address)
		{
			int rawId = ComputeRawId(address);
			int category = rawId != -1 ? GetCategory(address) : -1;

			return ComputeItemId(rawId, category);
		}

		protected override int ComputeRawId(IntPtr address)
		{
			return MemoryTools.ReadInt(Handle, address);
		}

		private int GetCategory(IntPtr address)
		{
			int value = MemoryTools.ReadByte(Handle, address - 0x1);

			return Utilities.ToHex(value);
		}
	}
}
