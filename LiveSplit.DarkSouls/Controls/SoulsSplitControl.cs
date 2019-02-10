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
		private const int ControlSpacing = 4;
		private const int ItemSplitCorrection = 2;

		private SplitLists lists;
		private Dictionary<SplitTypes, Func<Control[]>> functionMap;
		private Dictionary<string, string[]> itemMap;
		private SoulsSplitCollectionControl parent;

		// Equipment lists need to track upgrade data per item in order to properly update secondary item lines. This
		// array is updated whenever item type changes.
		private UpgradeData[] upgrades;

		private bool isArmorTypeSelected;

		// Tracking this value is required to properly shrink item splits that swap to a different type.
		private SplitTypes previousSplitType;

		public SoulsSplitControl(SoulsSplitCollectionControl parent, int index)
		{
			this.parent = parent;

			Index = index;

			InitializeComponent();

			lists = JsonConvert.DeserializeObject<SplitLists>(Resources.Splits);
			functionMap = new Dictionary<SplitTypes, Func<Control[]>>()
			{
				{ SplitTypes.Bonfire, GetBonfireControls },
				{ SplitTypes.Boss, GetBossControls },
				{ SplitTypes.Covenant, GetCovenantControls },
				{ SplitTypes.Events, GetEventControls },
				{ SplitTypes.Item, GetItemControls },
				{ SplitTypes.Zone, GetZoneControls }
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

		// Indices can be updated if earlier splits are removed.
		public int Index { get; set; }

		public Split ExtractSplit()
		{
			var index = splitTypeComboBox.SelectedIndex;
			var type = index != -1 ? (SplitTypes)index : SplitTypes.Unassigned;
			var controls = splitDetailsPanel.Controls;

			int[] data = null;

			if (controls.Count > 0)
			{
				data = new int[controls.Count];

				for (int i = 0; i < controls.Count; i++)
				{
					// Item splits have a numeric textbox (representing item count). All other split types use
					// exclusively dropdowns.
					if (type == SplitTypes.Item && i == 2)
					{
						string text = ((TextBox)controls[2]).Text;

						data[2] = text.Length > 0 ? int.Parse(text) : -1;

						continue;
					}

					data[i] = ((ComboBox)controls[i]).SelectedIndex;
				}
			}

			return new Split(type, data);
		}

		public void Refresh(Split split)
		{
			SplitTypes type = split.Type;

			if (type == SplitTypes.Unassigned)
			{
				return;
			}

			splitTypeComboBox.SelectedIndex = (int)type;

			var controls = splitDetailsPanel.Controls;

			// If a valid split type was selected, the data array is guaranteed to exist (although it might still have
			// -1 values).
			int[] data = split.Data;

			for (int i = 0; i < controls.Count; i++)
			{
				if (type == SplitTypes.Item && i == 2)
				{
					int value = data[2];

					((TextBox)controls[2]).Text = value != -1 ? data[2].ToString() : "";

					continue;
				}

				((ComboBox)controls[i]).SelectedIndex = data[i];
			}
		}

		private void splitTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			OnSplitTypeChange((SplitTypes)splitTypeComboBox.SelectedIndex);
		}

		private void OnSplitTypeChange(SplitTypes splitType)
		{
			Control[] controls = splitType != SplitTypes.Manual ? functionMap[splitType]() : null;
			ControlCollection panelControls = splitDetailsPanel.Controls;
			panelControls.Clear();

			if (controls != null)
			{
				for (int i = 0; i < controls.Length; i++)
				{
					Control control = controls[i];

					if (i > 0)
					{
						control.Location = new Point(controls[i - 1].Bounds.Right + ControlSpacing, 0);
					}

					panelControls.Add(control);
				}
			}

			// Item splits require two lines.
			if (splitType == SplitTypes.Item)
			{
				Height = Height * 2 - ItemSplitCorrection;
				splitDetailsPanel.Height = splitDetailsPanel.Height * 2 + ControlSpacing;

				Control[] secondaryControls = GetItemSecondaryControls();

				int y = controls[0].Bounds.Bottom + ControlSpacing;

				for (int i = 0; i < secondaryControls.Length; i++)
				{
					Control control = secondaryControls[i];
					Point point = new Point(0, y);

					if (i > 0)
					{
						point.X = secondaryControls[i - 1].Bounds.Right + ControlSpacing;
					}

					control.Location = point;
					panelControls.Add(control);

					LinkItemLines(controls, secondaryControls);
				}

				// Changing an existing split to an item split can cause later splits to be shifted down.
				if (Index < parent.SplitCount - 1)
				{
					parent.ShiftSplits(Index + 1);
				}
			}
			else if (previousSplitType == SplitTypes.Item)
			{
				Height = (Height + ItemSplitCorrection) / 2;
				splitDetailsPanel.Height = (splitDetailsPanel.Height - ControlSpacing) / 2;
				parent.ShiftSplits(Index + 1);
			}

			previousSplitType = splitType;
		}

		private void deleteButton_Click(object sender, EventArgs e)
		{
			parent.RemoveSplit(Index);
		}

		private Control[] GetBonfireControls()
		{
			const int BonfireListWidth = 169;
			const int BonfireCriteriaWidth = 119;

			var bonfireCriteria = GetDropdown(new []
			{
				"On light",
				"On rest",
				"On kindle (first)",
				"On kindle (second)",
				"On kindle (third)"
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
			const int CovenantListWidth = 139;
			const int CovenantCriteriaWidth = 87;

			var covenantCriteria = GetDropdown(new []
			{
				"On discover",
				"On join",
				"On warp"
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

		private Control[] GetEventControls()
		{
			const int EventListWidth = 88;
			const int EventCriteriaWidth = 80;

			var eventCriteria = GetDropdown(new []
			{
				"On ring",
				"On warp"
			}, "Criteria", EventCriteriaWidth, false);

			var eventList = GetDropdown(new []
			{
				"- Bells -",
				"First Bell",
				"Second Bell",
				"",
				"- Endings -",
				"Dark Lord",
				"Link the Fire"
			}, "Ending", EventListWidth);
			
			eventList.SelectedIndexChanged += (sender, args) =>
			{
				eventCriteria.Enabled = eventList.SelectedIndex <= 2;
			};

			return new Control[]
			{
				eventList,
				eventCriteria
			};
		}

		private Control[] GetItemControls()
		{
			const int ItemTypeWidth = 95;
			const int ItemListWidth = 184;
			const int ItemCountWidth = 30;

			var itemCount = new TextBox
			{
				Enabled = false,
				Width = ItemCountWidth,

				// This height results in the text box exactly lining up with the adjacent item list dropdown.
				AutoSize = false,
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

			// Additional callbacks are added when item lines are linked.
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

			return new Control[]
			{
				itemTypes,
				itemList,
				itemCount
			};
		}

		private Control[] GetItemSecondaryControls()
		{
			const int ItemModificationWidth = 96;
			const int ItemReinforcementWidth = 104;
			const int ItemCriteriaWidth = 97;

			// Mods and reinforcements are updated based on item type.
			var itemModifications = GetDropdown(null, "Modifications", ItemModificationWidth, false);
			var itemReinforcements = GetDropdown(null, "Reinforcement", ItemReinforcementWidth, false);
			var itemCriteria = GetDropdown(new []
			{
				"On acquisition",
				"On warp"
			}, "Criteria", ItemCriteriaWidth);

			return new Control[]
			{
				itemModifications,
				itemReinforcements,
				itemCriteria
			};
		}

		private void LinkItemLines(Control[] line1, Control[] line2)
		{
			SoulsDropdown itemTypes = (SoulsDropdown)line1[0];
			SoulsDropdown itemList = (SoulsDropdown)line1[1];
			SoulsDropdown mods = (SoulsDropdown)line2[0];
			SoulsDropdown reinforcements = (SoulsDropdown)line2[1];

			itemTypes.SelectedIndexChanged += (sender, args) =>
			{
				itemList.Enabled = true;

				var rawList = itemMap[itemTypes.Text];
				var items = itemList.Items;
				items.Clear();

				int typeIndex = itemTypes.SelectedIndex;

				bool isArmorType = typeIndex >= 1 && typeIndex <= 4;
				bool isWeaponType = typeIndex >= 34;

				if (isArmorType || isWeaponType)
				{
					upgrades = new UpgradeData[rawList.Length];

					for (int i = 0; i < rawList.Length; i++)
					{
						string value = rawList[i];

						if (value.Length == 0 || value[0] == '-')
						{
							items.Add(value);

							continue;
						}

						string[] tokens = value.Split('|');
						string name = tokens[0];
						string reinforcementString = tokens[1];

						int maxReinforcement = reinforcementString[0] == '+'
							? int.Parse(reinforcementString.Substring(1))
							: 0;

						ModificationTypes availableMods = isArmorType ? ModificationTypes.None : ModificationTypes.Standard;

						items.Add(name);
						upgrades[i] = new UpgradeData(maxReinforcement, availableMods);
					}

					mods.RefreshPrompt(isArmorType ? "N/A" : "Modifications");
					reinforcements.RefreshPrompt("Reinforcement");
				}
				else
				{
					items.AddRange(rawList);
					mods.RefreshPrompt("N/A");
					reinforcements.RefreshPrompt("N/A");
					upgrades = null;
				}

				isArmorTypeSelected = isArmorType;
			};

			itemList.SelectedIndexChanged += (sender, args) =>
			{
				// This means the current item type is not equipment.
				if (upgrades == null)
				{
					return;
				}

				UpgradeData data = upgrades[itemList.SelectedIndex];

				ModificationTypes availableMods = data.AvailableMods;

				if (availableMods != ModificationTypes.None)
				{
					mods.RefreshPrompt("Modifications", true);
					//mods.Items.AddRange();
				}
				else if (!isArmorTypeSelected)
				{
					mods.RefreshPrompt("Unmodifiable");
				}

				int maxReinforcement = data.MaxReinforcement;

				if (maxReinforcement > 0)
				{
					reinforcements.RefreshPrompt("Reinforcements", true);
					reinforcements.Items.AddRange(Enumerable.Range(1, maxReinforcement).Select(r => "+" + r).ToArray());
				}
				else
				{
					reinforcements.RefreshPrompt("Unreinforceable");
				}
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
