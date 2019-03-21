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
		private static Version[] versions;
		private static Action<Split>[] functions;

		static VersionHelper()
		{
			versions = new []
			{
				new Version(1, 0, 1),
				new Version(1, 1, 3), 
			};

			functions = new Action<Split>[]
			{
				To101,
				To103
			};
		}

		public static void Convert(Split[] splits, Version fileVersion)
		{
			// As the autosplitter version grows, the list of required conversions grows as well. Rather than
			// converting all file versions directly to current (e.g. 1.0.0 to 1.0.3), conversions are applied in order
			// as needed (e.g. 1.0.0 is converted to 1.0.1, then 1.0.1 is updated to 1.0.3). Using this approach, each
			// new conversion function only needs to account for exactly what that version changed.
			for (int i = 0; i < versions.Length; i++)
			{
				if (fileVersion >= versions[i])
				{
					continue;
				}

				var function = functions[i];

				foreach (Split split in splits)
				{
					function(split);
				}
			}
		}

		private static void To101(Split split)
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
					// This is another correction due to the addition of quitout splits.
					data[5] = Convert(data[5], 1, 2);

					break;
			}

			split.Data = data;
		}

		private static void To103(Split split)
		{
			const int ConsumablesIndex = 16;
			const int LloydsTalismanIndex = 11;

			int[] data = split.Data;

			switch (split.Type)
			{
				case SplitTypes.Item:
					// In 1.0.3, Lloyd's Talismans were added (they had been accidentally missed before).
					if (data[0] == ConsumablesIndex && data[1] >= LloydsTalismanIndex)
					{
						data[1]++;
					}

					break;

				case SplitTypes.Quitout:
					// A numeric textbox for quitouts splits (representing quitout count) was also added in this
					// version. Previously, quitout splits had no data.
					data = new int[1];
					data[0] = 1;

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
