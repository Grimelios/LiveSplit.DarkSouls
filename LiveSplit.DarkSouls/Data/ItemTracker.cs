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
		
		private int total;

		public ItemTracker(SoulsPointers pointers, IntPtr handle, int start, int count)
		{
			this.handle = handle;

			itemStart = pointers.Inventory + start;
			itemCount = pointers.Inventory + count;
			tracker = new Dictionary<ItemId, List<IntPtr>>();
			openSlots = new LinkedList<int>();
		}

		public void Clear()
		{
			tracker.Clear();
			openSlots.Clear();
		}

		// This function is called only once at the start of a run (or when the process is hooked if the timer was
		// already running). The list of item IDs is pulled from the UI.
		public void SetItems(List<ItemId> itemIds)
		{
			tracker.Clear();
			total = MemoryTools.ReadInt(handle, itemCount);

			Console.WriteLine("Total: " + total);

			IntPtr address = itemStart;

			for (int i = 0; i < total; i++)
			{
				Console.Write($"[{i}]: ");

				ItemId id = ComputeItemId(address);

				int index = itemIds.IndexOf(id);

				// If this function is called, it's assumed that at least one ID is given (rather than an empty list).
				if (index >= 0)
				{
					if (tracker.TryGetValue(id, out List<IntPtr> list))
					{
						list.Add(address);
					}
					else
					{
						tracker.Add(id, new List<IntPtr> { address });
					}

					itemIds.RemoveAt(index);
				}
				else if (id.BaseId == -1)
				{
					Console.WriteLine("Open");

					openSlots.AddLast(i);
				}

				address += Step;
			}
		}

		public void Refresh()
		{
			int newTotal = MemoryTools.ReadInt(handle, itemCount);

			if (newTotal != total)
			{
				// This variable is used for two different purposes. For new items, it tracks the index of all newly-
				// acquired items. For drops, it's used as a loop index to find new open slots.
				int itemIndex;

				IntPtr address;

				// When a new item is acquired, it goes into the first available slot (or it's appended to the end of
				// the list if no slots are available).
				if (newTotal > total)
				{
					// It's pretty common to acquire multiple items at once (such as an entire armor set).
					int pickupCount = newTotal - total;

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
							itemIndex = total;
						}

						address = itemStart + itemIndex * Step;

						int id = MemoryTools.ReadInt(handle, address);
						int category = MemoryTools.ReadInt(handle, address - 0x1);
						int count = MemoryTools.ReadInt(handle, address + 0x4);
					}
				}
				else
				{
					// I'm fairly certain that only one item can be removed from the inventory at one time (via drop or
					// giving items to an NPC), but I'm not completely sure. A loop guarantees that the autosplitter's
					// internal inventory state remains correct.
					int dropCount = total - newTotal;

					itemIndex = 0;
					address = itemStart;

					for (int i = 0; i < dropCount; i++)
					{
						while (itemIndex < newTotal)
						{
							int id = MemoryTools.ReadInt(handle, address);

							// Incrementing values here ensures the values are correct for the next outer loop iteration.
							itemIndex++;
							address += Step;

							// Empty inventory slots have an ID of -1 (equivalent to 4B+ as an unsigned value).
							if (id == -1)
							{
								openSlots.InsertSorted(itemIndex - 1);

								Console.WriteLine($"Open slot added ({itemIndex - 1})");

								break;
							}
						}
					}
				}

				total = newTotal;
			}
		}

		// Since some items don't stack, it's possible to have several of the target item in your inventory. The state
		// of each one is tracked and returned when the split becomes relevant.
		public ItemState[] GetItemStates(int baseId, int category)
		{
			ItemId id = new ItemId(baseId, category);

			// It's assumed that an item ID will only be queried if it was present in the list of saved splits
			// (although it may not have been acquired yet).
			if (!tracker.TryGetValue(id, out List<IntPtr> list))
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

				ComputeUpgrades(rawId, out int mods, out int reinforcement);

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

			if (rawId != -1)
			{
				Console.WriteLine($"Raw: {rawId}, Base: {baseId}, Category: {category}");
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
