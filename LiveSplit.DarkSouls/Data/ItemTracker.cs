using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveSplit.DarkSouls.Memory;

namespace LiveSplit.DarkSouls.Data
{
	public class ItemTracker
	{
		// Every item object is separated by this amount.
		private const int Step = 0x1C;
		
		private IntPtr handle;
		private IntPtr itemStart;
		private IntPtr itemCount;

		// For any given run, usually only a few items will be used for splits (far fewer than the total size of the
		// inventory). Tracking the addresses of items known to be relevant speeds things up while querying for item
		// states. Note that some items can't stack, which is why a list of pointers is needed.
		private Dictionary<ItemId, List<IntPtr>> tracker;

		// When new items are picked up, they're usually appended to the end of the item list. If items had previously
		// been dropped (or otherwise removed from the inventory) however, new items are inserted into those open slots
		// instead. Tracking open slots as total item count changes allows new items to be checked more efficiently
		// (rather than looping through the full item list).
		private LinkedList<int> openSlots;

		// The total slot count needs to be tracked separately from item count. If items are dropped, the item count
		// decreases even though the total length of the array remains unchanged (unless the last item in the array was
		// lost).
		private int totalItems;
		private int totalSlots;

		public ItemTracker(SoulsPointers pointers, IntPtr handle, int start, int count, List<ItemId> ids)
		{
			this.handle = handle;

			itemStart = pointers.Inventory + start;
			itemCount = pointers.Inventory + count;
			tracker = new Dictionary<ItemId, List<IntPtr>>();
			openSlots = new LinkedList<int>();

			SetItems(ids);
		}

		// This function is called only once at the start of a run (or when the process is hooked if the timer was
		// already running). The list of item IDs is pulled from the UI.
		private void SetItems(List<ItemId> itemIds)
		{
			totalItems = MemoryTools.ReadInt(handle, itemCount);

			// It's simpler to add each list at the start (since each one will become relevant at some point anyway,
			// assuming the splits are correctly configured).
			itemIds.ForEach(id => tracker.Add(id, new List<IntPtr>()));

			IntPtr address = itemStart;

			int slot = 0;

			for (int i = 0; i < totalItems; i++)
			{
				ItemId id = ComputeItemId(address);

				int index = itemIds.IndexOf(id);

				// If this function is called, it's assumed that at least one ID is given (rather than an empty list).
				if (index >= 0)
				{
					tracker[id].Add(address);
					itemIds.RemoveAt(index);
				}
				else if (id.BaseId == -1)
				{
					openSlots.AddLast(slot);

					// This trick ensures that all slots are reached.
					i--;
				}
				
				address += Step;
				slot++;
			}

			totalSlots = slot;
		}

		public void Refresh()
		{
			int newTotal = MemoryTools.ReadInt(handle, itemCount);

			if (newTotal != totalItems)
			{
				// This variable is used for two different purposes. For new items, it tracks the index of all newly-
				// acquired items. For drops, it's used as a loop index to find new open slots.
				int itemIndex;

				IntPtr address;

				// When a new item is acquired, it goes into the first available slot (or it's appended to the end of
				// the list if no slots are available).
				if (newTotal > totalItems)
				{
					// It's pretty common to acquire multiple items at once (such as an entire armor set).
					int pickupCount = newTotal - totalItems;

					for (int i = 0; i < pickupCount; i++)
					{
						if (openSlots.Count > 0)
						{
							// The open slot list is maintained in sorted order (with closest slot first).
							itemIndex = openSlots.First.Value;
							openSlots.RemoveFirst();
						}
						else
						{
							itemIndex = totalSlots;
							totalSlots++;
						}

						address = itemStart + itemIndex * Step;

						ItemId id = ComputeItemId(address);

						// Items not in the current set of splits are irrelevant for tracking purposes.
						if (!tracker.TryGetValue(id, out List<IntPtr> list))
						{
							continue;
						}
						
						list.Add(address);
					}
				}
				// This means that items were lost (dropped, sold, etc.).
				else
				{
					// I'm fairly certain that only one item can be removed from the inventory at one time (via drop or
					// giving items to an NPC), but I'm not completely sure. A loop guarantees that the autosplitter's
					// internal inventory state remains correct.
					int dropCount = totalItems - newTotal;

					// See the comment below (about looping backwards).
					itemIndex = totalSlots - 1;
					address = itemStart + itemIndex * Step;

					for (int i = 0; i < dropCount; i++)
					{
						LinkedListNode<int> openNode = openSlots.Last;

						// Looping backwards allows the total slots to be decreased correctly if items were dropped
						// from the end of the array.
						while (itemIndex >= 0)
						{
							int rawId = MemoryTools.ReadInt(handle, address);

							foreach (var list in tracker.Values)
							{
								int index = list.IndexOf(address);

								if (index >= 0)
								{
									list.RemoveAt(index);
									
									break;
								}
							}

							// Decrementing values here ensures the values are correct for the next outer loop
							// iteration.
							itemIndex--;
							address -= Step;

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
								if (itemIndex == totalSlots - 2)
								{
									totalSlots--;

									// This loop allows the tracked inventory to shrink as much as possible if there
									// are a series of open slots at the end.
									while (openSlots.Count > 0 && openSlots.Last.Value == totalSlots - 1)
									{
										openSlots.RemoveLast();
										totalSlots--;
									}
								}
								else
								{
									openSlots.InsertSorted(itemIndex + 1);
								}

								break;
							}
						}
					}
				}

				totalItems = newTotal;
			}
		}

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

				int rawId = MemoryTools.ReadInt(handle, address);
				int count = MemoryTools.ReadInt(handle, address + 0x4);
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

		private ItemId ComputeItemId(IntPtr address)
		{
			int rawId = MemoryTools.ReadInt(handle, address);
			int category = rawId != -1 ? GetCategory(address) : -1;
			int baseId;

			// Items with an ID five digits or greater are upgradeable equipment (weapons, armor, shields, and
			// pyromancy flames).
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

		private int GetCategory(IntPtr address)
		{
			int value = MemoryTools.ReadByte(handle, address - 0x1);
			string hex = value.ToString("X");

			return int.Parse(hex[0].ToString());
		}

		private void ComputeUpgrades(int rawId, out int mods, out int reinforcement)
		{
			// For upgradeable items, mods and reinforcement are represented through the ID directly. The hundreds
			// digit (third from the right) represents mods, while the tens digit (second from the right) represents
			// reinforcement. All such IDs are at least five digits long.
			mods = (rawId % 1000) / 100;
			reinforcement = rawId % 100;
		}
	}
}
