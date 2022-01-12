using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.DarkSouls.Data
{
	public class SplitLists
	{
		// Previously, splits were stored using Json (then using Newtonsoft.Json to parse the file). The problem is
		// that I don't know how to bundle the Newtonsoft.Json DLL into the component DLL, meaning that, while
		// developing, I had to manually copy over the DLL. I didn't want to force existing Souls runners to worry
		// about two DLLs, so I decided to kill the Json file in favor of a simpler text file.
		public static SplitLists Load()
		{
			string[] lines = Resources.Splits.Split(new [] {Environment.NewLine}, StringSplitOptions.None);

			var groups = new Dictionary<string, List<string>>();

			List<string> list = null;

			for (int i = 0; i < lines.Length; i++)
			{
				string line = lines[i];

				// Each split group is separated by a single empty line. The first line is the group's key, while the
				// remaining lines are values.
				if (i == 0 || lines[i - 1].Length == 0)
				{
					list = new List<string>();
					groups.Add(line, list);

					continue;
				}

				if (line.Length == 0)
				{
					continue;
				}

				// Empty lines within lists are represented with a plus.
				list.Add(line == "+" ? "" : line);
			}

			ItemLists items = new ItemLists();
			items.Ammunition = groups["Ammunition"].ToArray();
			items.Axes = groups["Axes"].ToArray();
			items.Bonfire = groups["Bonfire"].ToArray();
			items.Bows = groups["Bows"].ToArray();
			items.Catalysts = groups["Catalysts"].ToArray();
			items.ChestPieces = groups["ChestPieces"].ToArray();
			items.Consumables = groups["Consumables"].ToArray();
			items.Covenant = groups["Covenant"].ToArray();
			items.Crossbows = groups["Crossbows"].ToArray();
			items.Daggers = groups["Daggers"].ToArray();
			items.Embers = groups["Embers"].ToArray();
			items.Fist = groups["Fist"].ToArray();
			items.Flames = groups["Flames"].ToArray();
			items.Gauntlets = groups["Gauntlets"].ToArray();
			items.Greatswords = groups["Greatswords"].ToArray();
			items.Halberds = groups["Halberds"].ToArray();
			items.Hammers = groups["Hammers"].ToArray();
			items.Helmets = groups["Helmets"].ToArray();
			items.Keys = groups["Keys"].ToArray();
			items.Leggings = groups["Leggings"].ToArray();
			items.Miracles = groups["Miracles"].ToArray();
			items.Multiplayer = groups["Multiplayer"].ToArray();
			items.Ore = groups["Ore"].ToArray();
			items.Projectiles = groups["Projectiles"].ToArray();
			items.Pyromancies = groups["Pyromancies"].ToArray();
			items.Rings = groups["Rings"].ToArray();
			items.Shields = groups["Shields"].ToArray();
			items.Sorceries = groups["Sorceries"].ToArray();
			items.Souls = groups["Souls"].ToArray();
			items.Spears = groups["Spears"].ToArray();
			items.Swords = groups["Swords"].ToArray();
			items.Talismans = groups["Talismans"].ToArray();
			items.Tools = groups["Tools"].ToArray();
			items.Trinkets = groups["Trinkets"].ToArray();
			items.Whips = groups["Whips"].ToArray();

			SplitLists splits = new SplitLists();
			splits.Items = items;
			splits.Bonfires = groups["Bonfires"].ToArray();
			splits.Bosses = groups["Bosses"].ToArray();
			splits.Covenants = groups["Covenants"].ToArray();
			splits.Zones = groups["Zones"].ToArray();
            splits.RemasteredBosses = groups["Remastered"].ToArray();

			return splits;
		}

		public ItemLists Items { get; set; }

		public string[] Bonfires { get; set; }
		public string[] Bosses { get; set; }
		public string[] Covenants { get; set; }
		public string[] Zones { get; set; }
        public string[] RemasteredBosses { get; set; }
	}
}