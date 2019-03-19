using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveSplit.DarkSouls.Data;

namespace LiveSplit.DarkSouls
{
	// Autosplitter settings are saved in an XML file (part of the .lsl LiveSplit layout file). Split types are stored
	// using their name (e.g. "Bonfire", "Boss", "Event", etc.), but dropdown data values are stored by index. That
	// means that if the dropdown values change with an update (add, removal, or re-ordering), splits might be loaded
	// incorrectly (or the program might crash entirely). This class, then, deals with modifying split data based on
	// version differences.
	public static class VersionHelper
	{
		private static Dictionary<string, Action<Split>> functionMap;

		static VersionHelper()
		{
			functionMap = new Dictionary<string, Action<Split>>
			{
				{ "1.0.0", From100 }
			};
		}

		public static void Convert(Split[] splits, string fileVersion)
		{
			var function = functionMap[fileVersion];

			// The splits array is assumed non-null if this function is called.
			foreach (Split split in splits)
			{
				function(split);
			}
		}

		private static void From100(Split split)
		{
			int[] data = split.Data;

			switch (split.Type)
			{
				// In 1.0.0, Quitout splits (and "On quitout" options for other splits) didn't exist. As such, many
				// timing values need to be corrected.
				case SplitTypes.Boss:
				case SplitTypes.Event:
				case SplitTypes.Flag:
					data[1] = Convert(data[1], 1, 2);

					break;

				case SplitTypes.Covenant:
					// Previously, there were four criteria options (discover, join, then an "On warp" option for
					// each). Timing has now been moved to its own dropdown (which means the data array needs to be
					// resized, rather than simply updating indexes).
					data = Resize(data, 3);
					
					int criteria = data[1];

					if (criteria >= 2)
					{
						data[1] -= 2;
						data[2] = 2;
					}
					else
					{
						data[2] = 0;
					}

					break;

				case SplitTypes.Item:
					data[5] = Convert(data[5], 1, 2);

					break;
			}

			split.Data = data;
		}

		// This function is useful for when new data fields are added (rather than just updating indexes in the
		// existing array).
		private static int[] Resize(int[] data, int size)
		{
			int[] newData = new int[3];

			Array.Copy(data, newData, data.Length);

			return newData;
		}

		private static int Convert(int value, int old, int updated)
		{
			return value == old ? updated : value;
		}
	}
}
