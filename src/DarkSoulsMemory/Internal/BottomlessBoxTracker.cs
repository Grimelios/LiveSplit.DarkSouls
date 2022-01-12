using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkSoulsMemory.Internal;

namespace DarkSoulsMemory.Internal
{
    internal enum InventoryFlags
    {
        BottomlessBox = 0xEA40,
        ItemCount = 0x128,
        ItemStart = 0xA24,
        KeyCount = 0x12C,
        KeyStart = 0x324
    }


	internal class BottomlessBoxTracker : TrackerBase
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
			base(handle, inventory + (int)InventoryFlags.BottomlessBox, Step, 0x8)
		{
		}

		public override void SetItems(List<ItemId> itemIds)
		{
			SetItems(itemIds, DefaultSlots, DefaultSlots);

			IntPtr address = Start + (DefaultSlots - 1) * Step;

			if (OpenSlots.Count == 0)
			{
				return;
			}

			// Since the item count above isn't accurate (just a default value large enough to reasonably accommodate
			// most scenarios), fake slots at the end of the list must be removed in order for the initial state to be
			// correct.
			for (int i = 0; i < DefaultSlots; i++)
			{
				int rawId = ReadInt32(Handle, address);

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
		}

		protected override ItemId ComputeItemId(IntPtr address)
		{
			// Data is stored in the bottomless box a little differently than the main inventory. In the main
			// inventory, raw ID is stored as an integer (four bytes), with category as a single byte at a different
			// address. In contrast, the bottomless box stores both raw ID and category in a single integer, with three
			// bytes devoted to the ID and the fourth representing category.
			byte[] bytes = ReadBytes(Handle, address, 4);

			int category = bytes[3];

			// This means that the slot is empty.
			if (category == byte.MaxValue)
			{
				return null;
			}

			int rawId = TrimLastByte(bytes);
			
			return ComputeItemId(rawId, category.ToHex());
		}

		protected override int ComputeRawId(IntPtr address)
		{
			// Data is stored in the bottomless box a little differently than the main inventory. In the main
			// inventory, raw ID is stored as an integer (four bytes), with category as a single byte at a different
			// address. In contrast, the bottomless box stores both raw ID and category in a single integer, with three
			// bytes devoted to the ID and the fourth representing category.
			byte[] bytes = ReadBytes(Handle, address, 4);

			return TrimLastByte(bytes);
		}

		private int TrimLastByte(byte[] bytes)
		{
			byte[] idBytes = new byte[4];

			for (int i = 0; i < 3; i++)
			{
				idBytes[i] = bytes[i];
			}

			return BitConverter.ToInt32(idBytes, 0);
		}

		public override void Refresh()
		{
			IntPtr nextSlot = Start + (OpenSlots.Count > 0 ? OpenSlots.First.Value : TotalSlots) * Step;

			int value = ReadInt32(Handle, nextSlot);

			// Unlike the main inventory, there's no memory location that directly stores the number of items in the
			// bottomless box (or at least I wasn't able to find one). As such, item addition/removal must be detected
			// by checking slots directly. Also note that only one item can be put into the bottomless box at a time
			// (as opposed to picking up multiple items at once off the ground).
			if (value != -1)
			{
				AddItems(1);

				return;
			}

			LinkedListNode<int> slot = OpenSlots.First;

			// Since the bottomless box can't track count directly, all non-empty slots must be scanned each tick in
			// order to detect when one of them becomes empty.
			for (int i = 0; i < TotalSlots; i++)
			{
				if (slot != null && i == slot.Value)
				{
					slot = slot.Next;

					continue;
				}

				IntPtr address = Start + i * Step;

				value = ReadInt32(Handle, address);

				if (value == -1)
				{
					RemoveItems(1, address);

					return;
				}
			}
		}
	}
}
