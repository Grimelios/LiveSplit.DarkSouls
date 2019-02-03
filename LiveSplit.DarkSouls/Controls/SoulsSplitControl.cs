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
		private SplitLists lists;
		private Dictionary<string, Func<Control[]>> functionMap;

		private string[][] itemLists;

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

			itemLists = new string[15][];
			itemLists[0] = items.Ammunition;
			itemLists[1] = items.Armor;
			itemLists[2] = items.Bonfire;
			itemLists[3] = items.Consumables;
			itemLists[4] = items.Covenant;
			itemLists[5] = items.Embers;
			itemLists[6] = items.Keys;
			itemLists[7] = items.Miracles;
			itemLists[8] = items.Multiplayer;
			itemLists[9] = items.Ore;
			itemLists[10] = items.Other;
			itemLists[11] = items.Projectiles;
			itemLists[12] = items.Rings;
			itemLists[13] = items.Sorceries;
			itemLists[14] = items.Souls;
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
			}, false);

			var bossList = GetDropdown(lists.Bosses);
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
			}, false);

			var covenantList = GetDropdown(lists.Covenants);
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
			var itemCriteria = GetDropdown(new[]
			{
				"Acquired",
				"Sold"
			}, false);

			var itemList = GetDropdown(null, false);
			itemList.SelectedIndexChanged += (sender, args) =>
			{
				itemCriteria.Enabled = true;
				itemCriteria.SelectedIndex = 0;
			};

			var itemTypes = GetDropdown(new []
			{
				"Ammunition",
				"Armor",
				"Bonfire",
				"Consumables",
				"Covenant",
				"Embers",
				"Keys",
				"Miracles",
				"Multiplayer",
				"Ore",
				"Other",
				"Projectiles",
				"Rings",
				"Sorceries",
				"Souls"
			});

			itemTypes.SelectedIndexChanged += (sender, args) =>
			{
				itemList.Enabled = true;

				var items = itemList.Items;
				items.Clear();
				items.AddRange(itemLists[itemTypes.SelectedIndex]);
			};

			var itemCount = GetNumericTextbox();

			return new Control[]
			{
				itemTypes,
				itemList,
				itemCriteria,
				itemCount
			};
		}

		private Control[] GetNpcControls()
		{
			return null;
		}

		private SoulsDropdown GetDropdown(string[] items, bool enabled = true)
		{
			SoulsDropdown box = new SoulsDropdown();
			box.Enabled = enabled;

			if (items != null)
			{
				box.Items.AddRange(items);
			}

			return box;
		}

		private TextBox GetNumericTextbox()
		{
			TextBox textbox = new TextBox
			{
				Text = "1",
				Visible = false,
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
