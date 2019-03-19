using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveSplit.DarkSouls.Data;

namespace LiveSplit.DarkSouls
{
	// Autosplitter settings are saved in an XML file (part of the .lss LiveSplit layout file). Split types are stored
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
				case SplitTypes.Boss:
					break;
			}

			split.Data = data;
		}
	}
}
