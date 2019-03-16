using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveSplit.DarkSouls.Memory;

namespace LiveSplit.DarkSouls.Data
{
	using AddressTracker = Dictionary<ItemId, List<IntPtr>>;

	public abstract class TrackerBase
	{
		// For any given run, usually only a few items will be used for splits (far fewer than the total size of the
		// inventory). Tracking the addresses of items known to be relevant speeds things up while querying for item
		// states. Note that some items can't stack, which is why a list of pointers is needed.
		private AddressTracker tracker;

		// The total slot count needs to be tracked separately from item count. If items are dropped, the item count
		// decreases even though the total length of the array remains unchanged (unless the last item in the array was
		// lost).
		private int step;
		private int countOffset;

		private string name;

		protected TrackerBase(IntPtr handle, IntPtr start, int step, int countOffset, string name)
		{
			this.step = step;
			this.countOffset = countOffset;
			this.name = name;

			Handle = handle;
			Start = start;
			OpenSlots = new LinkedList<int>();
			tracker = new Dictionary<ItemId, List<IntPtr>>();
		}

		protected IntPtr Handle { get; }
		protected IntPtr Start { get; }

		protected int TotalSlots { get; set; }

		// When new items are picked up, they're usually appended to the end of the item list. If items had previously
		// been dropped (or otherwise removed from the inventory) however, new items are inserted into those open slots
		// instead. Tracking open slots as total item count changes allows new items to be checked more efficiently
		// (rather than looping through the full item list).
		protected LinkedList<int> OpenSlots { get; }

		// This is used by the bottomless box to determine when a new item is put into the box (since there's no direct
		// item count for the bottomless box).
		protected IntPtr NextSlot => Start + (OpenSlots.Count > 0 ? OpenSlots.First.Value : TotalSlots) * step;

		// This function is called only once per tracker at the start of a run (or when the process is hooked if the
		// timer already running). The list of item IDs is pulled from the UI.
		public abstract void SetItems(List<ItemId> itemIds);

		protected void SetItems(List<ItemId> itemIds, int totalItems, int maxSlots = int.MaxValue)
		{
			// It's simpler to add each list at the start (since each one will become relevant at some point anyway,
			// assuming the splits are correctly configured).
			itemIds.ForEach(id =>
			{
				// It's possible to have multiple splits for the same item (unlikely, but totally allowable). In those
				// cases, the same collection of item states should be returned when queried.
				if (!tracker.ContainsKey(id))
				{
					tracker.Add(id, new List<IntPtr>());
				}
			});

			IntPtr address = Start;

			int slot = 0;

			for (int i = 0; i < totalItems; i++)
			{
				ItemId id = ComputeItemId(address);

				int index = itemIds.IndexOf(id);

				// If this function is called, it's assumed that at least one ID is given (rather than an empty list).
				if (index >= 0)
				{
					Console.WriteLine($"[{name}] Slot {i}: [baseId: {id.BaseId}, category: {id.Category}] (tracked)");

					tracker[id].Add(address);
					itemIds.RemoveAt(index);
				}
				else if (id.BaseId == -1)
				{
					Console.WriteLine($"[{name}] Slot {i}: [empty]");

					OpenSlots.AddLast(slot);

					// This trick ensures that all slots are reached.
					i--;
				}
				else
				{
					Console.WriteLine($"[{name}] Slot {i}: [baseId: {id.BaseId}, category: {id.Category}]");
				}

				address += step;
				slot++;

				// Maximum slot is useful for limiting range when initializing the bottomless box.
				if (slot == maxSlots)
				{
					break;
				}
			}

			TotalSlots = slot;

			Console.WriteLine($"[{name}] Slots: {slot}");
			Console.WriteLine();
		}

		protected void AddItems(int count)
		{
			// When a new item is acquired, it goes into the first available slot (or appended to the end of the list
			// if no slots are available).
			for (int i = 0; i < count; i++)
			{
				int itemIndex;

				// The open slot list is maintained in sorted order (with closest slot first).
				if (OpenSlots.Count > 0)
				{
					itemIndex = OpenSlots.First.Value;
					OpenSlots.RemoveFirst();
				}
				else
				{
					itemIndex = TotalSlots;
					TotalSlots++;
				}

				IntPtr address = Start + itemIndex * step;
				ItemId id = ComputeItemId(address);

				Console.Write($"[{name}] Item added [baseId: {id.BaseId}, category: {id.Category}]");

				// Items not in the current set of splits are irrelevant for tracking purposes.
				if (!tracker.TryGetValue(id, out List<IntPtr> list))
				{
					Console.WriteLine();

					continue;
				}

				Console.WriteLine(" (tracked)");

				list.Add(address);
			}
		}

		protected void RemoveItems(int count)
		{
			// See the comment below (with regards to looping backwards).
			int itemIndex = TotalSlots - 1;

			IntPtr address = Start + itemIndex * step;

			for (int i = 0; i < count; i++)
			{
				LinkedListNode<int> openNode = OpenSlots.Last;

				// Looping backwards allows the total slots to be decreased correctly if items were dropped
				// from the end of the array.
				while (itemIndex >= 0)
				{
					int rawId = MemoryTools.ReadInt(Handle, address);

					foreach (var list in tracker.Values)
					{
						int index = list.IndexOf(address);

						if (index >= 0)
						{
							list.RemoveAt(index);

							break;
						}
					}

					itemIndex--;
					address -= step;

					if (openNode != null && itemIndex < openNode.Value - 1)
					{
						openNode = openNode.Previous;
					}

					// Empty inventory slots have an ID of -1 (equivalent to 4B+ as an unsigned integer).
					if (rawId == -1)
					{
						// This check ensures that the same open slot isn't accidentally added twice.
						if (openNode != null && itemIndex + 1 == openNode.Value)
						{
							continue;
						}

						// Note that the index was already decremented above.
						if (itemIndex == TotalSlots - 2)
						{
							TotalSlots--;

							// This loop allows the tracked inventory to shrink as much as possible if there
							// are a series of open slots at the end.
							while (OpenSlots.Count > 0 && OpenSlots.Last.Value == TotalSlots - 1)
							{
								OpenSlots.RemoveLast();
								TotalSlots--;
							}
						}
						else
						{
							OpenSlots.InsertSorted(itemIndex + 1);

							Console.WriteLine($"[{name}] Item removed [slot: {itemIndex - 2}]");
						}

						break;
					}
				}
			}
		}

		protected abstract ItemId ComputeItemId(IntPtr address);

		protected ItemId ComputeItemId(int rawId, int category)
		{
			int baseId;

			// All upgradeable items have an ID five digits or greater (but not all items with an ID that long are
			// necessarily upgradeable).
			if (rawId / 10000 > 0)
			{
				// For upgradeable items, mods and reinforcement are represented through the ID directly. The hundreds
				// digit (third from the right) represents mods, while the ones digit (far right) represents
				// reinforcement. All such IDs are at least five digits long.
				ComputeUpgrades(rawId, out int mods, out int reinforcement);

				baseId = rawId - mods * 100 - reinforcement;
			}
			else
			{
				baseId = rawId;
			}

			return new ItemId(baseId, category);
		}

		private void ComputeUpgrades(int rawId, out int mods, out int reinforcement)
		{
			// For upgradeable items, mods and reinforcement are represented through the ID directly. The hundreds
			// digit (third from the right) represents mods, while the tens digit (second from the right) represents
			// reinforcement. All such IDs are at least five digits long.
			mods = (rawId % 1000) / 100;
			reinforcement = rawId % 100;
		}

		public abstract void Refresh();

		// Since some items don't stack, it's possible to have several of the target item in your inventory. The state
		// of each one is tracked and returned when the split becomes relevant.
		public ItemState[] GetItemStates(int baseId, int category)
		{
			ItemId id = new ItemId(baseId, category);

			// It's assumed that an item ID will only be queried if it was present in the list of saved splits
			// (although it may not have been acquired yet).
			if (!tracker.TryGetValue(id, out List<IntPtr> list) || list.Count == 0)
			{
				// Returning null means that item isn't currently in the inventory.
				return null;
			}

			ItemState[] states = new ItemState[list.Count];

			for (int i = 0; i < list.Count; i++)
			{
				IntPtr address = list[i];

				int rawId = MemoryTools.ReadInt(Handle, address);
				int count = MemoryTools.ReadInt(Handle, address + countOffset);
				int mods = 0;
				int reinforcement = 0;

				if (rawId / 10000 > 0)
				{
					ComputeUpgrades(rawId, out mods, out reinforcement);
				}

				states[i] = new ItemState(mods, reinforcement, count);
			}

			return states;
		}
	}
}
