using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveSplit.DarkSouls.Data;

namespace LiveSplit.DarkSouls.Controls
{
	public partial class SoulsMasterControl : UserControl
	{
		public SoulsMasterControl()
		{
			InitializeComponent();
		}

		public bool UseGameTime
		{
			get => igtCheckbox.Checked;
			set => igtCheckbox.Checked = value;
		}

		public bool ResetEquipmentIndexes
		{
			get => resetCheckbox.Checked;
			set => resetCheckbox.Checked = value;
		}

		public bool StartTimerAutomatically
		{
			get => startCheckbox.Checked;
			set => startCheckbox.Checked = value;
		}

		public SoulsSplitCollectionControl CollectionControl => collectionControl;

		public void Refresh(Split[] splits)
		{
			CollectionControl.ClearSplits();

			foreach (var split in splits)
			{
				CollectionControl.AddSplit(split);
			}
		}
	}
}