using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveSplit.DarkSouls.Memory;

namespace LiveSplit.DarkSouls.Data
{
	public class BottomlessBoxTracker : TrackerBase
	{
		private const int Step = 0x20;

		// When the timer starts, the initial state of the inventory is queried from memory. To do this, main inventory
		// trackers can simply read the total item count. The bottomless box can't, so a large default (i.e. assumed)
		// maximum total is used instead. This means that if someone starts their timer with more than 100 unique slots
		// stored in the bottomless box, item tracking will not function properly (with regards to the box). This
		// scenario should be extremely rare in practice (like, you'd have to examine the code and go out of your way
		// to break it, and even then, this value could easily be increased).
		private const int DefaultSlots = 50;

		public BottomlessBoxTracker(IntPtr inventory, IntPtr handle) :
			base(handle, inventory + (int)InventoryFlags.BottomlessBox, Step, 0x8, "Bottomless Box")
		{
		}

		public override void SetItems(List<ItemId> itemIds)
		{
			SetItems(itemIds, DefaultSlots, DefaultSlots);

			IntPtr address = Start + (DefaultSlots - 1) * Step;

			// Since the item count above isn't accurate (just a default value large enough to reasonably accommodate
			// most scenarios), fake slots at the end of the list must be removed in order for the initial state to be
			// correct.
			for (int i = 0; i < DefaultSlots; i++)
			{
				int rawId = MemoryTools.ReadInt(Handle, address);

				address -= Step;

				if (rawId == -1)
				{
					OpenSlots.RemoveLast();
					TotalSlots--;
				}
				else
				{
					break;
				}
			}

			Console.WriteLine($"[Bottomless Box] Slots (trimmed): {TotalSlots}");
			Console.WriteLine();
		}

		protected override ItemId ComputeItemId(IntPtr address)
		{
			return null;
		}

		public override void Refresh()
		{
			int value = MemoryTools.ReadInt(Handle, NextSlot);

			// Unlike the main inventory, there's no memory location that directly stores the number of items in the
			// bottomless box (or at least I wasn't able to find one). As such, item addition/removal must be detected
			// by checking slots directly. Also note that only one item can be put into the bottomless box at a time
			// (as opposed to picking up multiple items at once off the ground).
			if (value != -1)
			{
				AddItems(1);
			}
		}
	}
}
