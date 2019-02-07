using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using LiveSplit.DarkSouls.Data;
using Newtonsoft.Json;

namespace LiveSplit.DarkSouls.Controls
{
	public partial class SoulsSplitControl : UserControl
	{
		private const int DefaultControlWidth = 89;

		private SplitLists lists;
		private Dictionary<string, Func<Control[]>> functionMap;
		private Dictionary<string, string[]> itemMap;

		public SoulsSplitControl()
		{
			InitializeComponent();

			lists = JsonConvert.DeserializeObject<SplitLists>(Resources.Splits);
			functionMap = new Dictionary<string, Func<Control[]>>()
			{
				{ "Bonfire", GetBonfireControls },
				{ "Boss", GetBossControls },
				{ "Covenant", GetCovenantControls },
				{ "Ending", GetEndingControls },
				{ "Item", GetItemControls },
				{ "Zone", GetZoneControls }
			};

			var items = lists.Items;

			itemMap = new Dictionary<string, string[]>
			{
				{ "Ammunition", items.Ammunition },
				{ "Axes", items.Axes },
				{ "Bonfire", items.Bonfire },
				{ "Bows", items.Bows },
				{ "Catalysts", items.Catalysts },
				{ "Chest Pieces", items.ChestPieces },
				{ "Consumables", items.Consumables },
				{ "Covenant", items.Covenant },
				{ "Crossbows", items.Crossbows },
				{ "Daggers", items.Daggers },
				{ "Embers", items.Embers },
				{ "Fist", items.Fist },
				{ "Flames", items.Flames },
				{ "Gauntlets", items.Gauntlets },
				{ "Greatswords", items.Greatswords },
				{ "Halberds", items.Halberds },
				{ "Hammers", items.Hammers },
				{ "Helmets", items.Helmets },
				{ "Keys", items.Keys },
				{ "Leggings", items.Leggings },
				{ "Miracles", items.Miracles },
				{ "Multiplayer", items.Multiplayer },
				{ "Ore", items.Ore },
				{ "Other", items.Other },
				{ "Projectiles", items.Projectiles },
				{ "Pyromancies", items.Pyromancies },
				{ "Rings", items.Rings },
				{ "Shields", items.Shields },
				{ "Sorceries", items.Sorceries },
				{ "Souls", items.Souls },
				{ "Spears", items.Spears },
				{ "Swords", items.Swords },
				{ "Talismans", items.Talismans },
				{ "Whips", items.Whips },
			};
		}

		public Split ExtractSplit()
		{
			SplitTypes type = (SplitTypes)splitTypeComboBox.SelectedIndex;

			var controls = splitDetailsPanel.Controls;

			int[] data = new int[controls.Count];
			int dropdownCount = controls.Count - (type == SplitTypes.Item ? 1 : 0);

			for (int i = 0; i < dropdownCount; i++)
			{
				data[i] = ((ComboBox)controls[i]).SelectedIndex;
			}
			
			// Item splits have a numeric textbox (representing item count). All other split types use exclusively
			// dropdowns.
			if (type == SplitTypes.Item)
			{
				data[2] = int.Parse(((TextBox)controls[2]).Text);
			}

			return new Split(type, null);
		}

		public void Refresh(Split split)
		{
		}

		private void splitTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!functionMap.ContainsKey(splitTypeComboBox.Text))
			{
				return;
			}

			Control[] controls = functionMap[splitTypeComboBox.Text]();
			ControlCollection panelControls = splitDetailsPanel.Controls;
			panelControls.Clear();

			for (int i = 0; i < controls.Length; i++)
			{
				Control control = controls[i];

				if (i > 0)
				{
					control.Location = new Point(controls[i - 1].Bounds.Right + 4, 0);
				}

				panelControls.Add(control);
			}
		}

		private Control[] GetBonfireControls()
		{
			const int BonfireListWidth = 169;
			const int BonfireCriteriaWidth = 67;

			var bonfireCriteria = GetDropdown(new []
			{
				"On light",
				"On rest"
			}, "Criteria", BonfireCriteriaWidth, false);

			var bonfireList = GetDropdown(lists.Bonfires, "Bonfire", BonfireListWidth);
			bonfireList.SelectedIndexChanged += (sender, args) =>
			{
				bonfireCriteria.Enabled = true;
			};

			return new Control[]
			{
				bonfireList,
				bonfireCriteria
			};
		}

		private Control[] GetBossControls()
		{
			const int BossListWidth = 131;
			const int BossCriteriaWidth = 122;

			var bossCriteria = GetDropdown(new []
			{
				"On victory message",
				"On warp"
			}, "Criteria", BossCriteriaWidth, false);

			var bossList = GetDropdown(lists.Bosses, "Bosses", BossListWidth);
			bossList.SelectedIndexChanged += (sender, args) =>
			{
				bossCriteria.Enabled = true;
			};

			return new Control[]
			{
				bossList,
				bossCriteria
			};
		}

		private Control[] GetCovenantControls()
		{
			const int CovenantListWidth = 137;
			const int CovenantCriteriaWidth = 87;

			var covenantCriteria = GetDropdown(new []
			{
				"On discover",
				"On join"
			}, "Criteria", CovenantCriteriaWidth, false);

			var covenantList = GetDropdown(lists.Covenants, "Covenants", CovenantListWidth);
			covenantList.SelectedIndexChanged += (sender, args) =>
			{
				covenantCriteria.Enabled = true;
			};

			return new Control[]
			{
				covenantList,
				covenantCriteria
			};
		}

		private Control[] GetEndingControls()
		{
			const int EndingListWidth = 88;

			var endingList = GetDropdown(new[]
			{
				"Dark Lord",
				"Link the Fire"
			}, "Ending", EndingListWidth);

			return new Control[] { endingList };
		}

		private Control[] GetItemControls()
		{
			const int ItemTypeWidth = 96;
			const int ItemListWidth = 183;
			const int ItemCountWidth = 40;

			var itemCount = new TextBox
			{
				Text = "1",
				Enabled = false,
				Width = ItemCountWidth,

				// By default, the textbox is one pixel shorter than the adjecent dropdown.
				Height = 21,
				MaxLength = 3
			};

			// See https://stackoverflow.com/q/463299/7281613.
			itemCount.KeyPress += (sender, args) =>
			{
				if (!char.IsDigit(args.KeyChar))
				{
					args.Handled = true;
				}
			};

			var itemList = GetDropdown(null, "Items", ItemListWidth, false);
			itemList.SelectedIndexChanged += (sender, args) =>
			{
				itemCount.Enabled = true;
				itemCount.Text = "1";
			};

			var itemTypes = GetDropdown(new []
			{
				"- Armor -",
				"Chest Pieces",
				"Helmets",
				"Gauntlets",
				"Leggings",
				"",
				"- Magic -",
				"Catalysts",
				"Flames",
				"Miracles",
				"Pyromancies",
				"Sorceries",
				"Talismans",
				"",
				"- Miscellaneous -",
				"Bonfire",
				"Covenant",
				"Keys",
				"Multiplayer",
				"Other",
				"Souls",
				"",

				// Ideally, this category would read "Other Equipment", but it's shortened to better fit into the 
				// LiveSplit window.
				"- Equipment -",
				"Ammunition",
				"Consumables",
				"Projectiles",
				"Rings",
				"Shields",
				"",
				"- Smithing -",
				"Embers",
				"Ore",
				"",
				"- Weapons -",
				"Bows",
				"Crossbows",
				"Daggers",
				"Fist",
				"Greatswords",
				"Halberds",
				"Hammers",
				"Spears",
				"Swords",
				"Whips"
			}, "Item Types", ItemTypeWidth);

			itemTypes.SelectedIndexChanged += (sender, args) =>
			{
				itemList.Enabled = true;

				var items = itemList.Items;
				items.Clear();
				items.AddRange(itemMap[itemTypes.Text]);
			};

			return new Control[]
			{
				itemTypes,
				itemList,
				itemCount
			};
		}

		private Control[] GetZoneControls()
		{
			const int ZoneListWidth = 152;

			var zoneList = GetDropdown(lists.Zones, "Zones", ZoneListWidth);

			return new Control[] { zoneList };
		}

		private SoulsDropdown GetDropdown(string[] items, string prompt, int width, bool enabled = true)
		{
			SoulsDropdown box = new SoulsDropdown
			{
				Enabled = enabled,
				Width = width,
				Prompt = prompt
			};

			if (items != null)
			{
				box.Items.AddRange(items);
			}

			return box;
		}
	}
}
