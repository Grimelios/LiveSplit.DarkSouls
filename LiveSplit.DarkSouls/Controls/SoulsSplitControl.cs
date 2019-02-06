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
				{ "Boss", GetBossControls },
				{ "Covenant", GetCovenantControls },
				{ "Item", GetItemControls }
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
				{ "Sets", items.Sets },
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

			for (int i = 0; i < controls.Count; i++)
			{
				Control control = controls[i];
				ComboBox comboBox = control as ComboBox;

				data[i] = comboBox?.SelectedIndex ?? int.Parse(((TextBox)control).Text);
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
			return null;
		}

		private Control[] GetBossControls()
		{
			var bossCriteria = GetDropdown(new[]
			{
				"On final hit",
				"On victory message",
				"On warp"
			}, DefaultControlWidth, false);

			var bossList = GetDropdown(lists.Bosses, DefaultControlWidth);
			bossList.SelectedIndexChanged += (sender, args) =>
			{
				bossCriteria.Enabled = true;
				bossCriteria.SelectedIndex = 2;
			};

			return new Control[]
			{
				bossList,
				bossCriteria
			};
		}

		private Control[] GetCovenantControls()
		{
			var covenantCriteria = GetDropdown(new []
			{
				"On discovery",
				"On join"
			}, DefaultControlWidth, false);

			var covenantList = GetDropdown(lists.Covenants, DefaultControlWidth);
			covenantList.SelectedIndexChanged += (sender, args) =>
			{
				covenantCriteria.Enabled = true;
				covenantCriteria.SelectedIndex = 0;
			};

			return new Control[]
			{
				covenantList,
				covenantCriteria
			};
		}

		private Control[] GetItemControls()
		{
			const int ItemListWidth = 182;

			var itemCount = GetItemTextbox();
			var itemList = GetDropdown(null, ItemListWidth, false);
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
				"Sets",
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
				"- Other Equipment -",
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
			}, DefaultControlWidth);

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

		private Control[] GetNpcControls()
		{
			return null;
		}

		private SoulsDropdown GetDropdown(string[] items, int width, bool enabled = true)
		{
			SoulsDropdown box = new SoulsDropdown
			{
				Enabled = enabled,
				Width = width
			};

			if (items != null)
			{
				box.Items.AddRange(items);
			}

			return box;
		}

		private TextBox GetItemTextbox()
		{
			TextBox textbox = new TextBox
			{
				Text = "1",
				Enabled = false,
				Width = DefaultControlWidth,
				MaxLength = 3
			};

			// See https://stackoverflow.com/q/463299/7281613.
			textbox.KeyPress += (sender, args) =>
			{
				if (!char.IsDigit(args.KeyChar))
				{
					args.Handled = true;
				}
			};

			return textbox;
		}
	}
}
