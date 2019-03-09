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

		public bool UseGameTime => igtCheckbox.Checked;
		public bool ResetEquipmentIndexes => resetCheckbox.Checked;
		public bool StartTimerAutomatically => startCheckbox.Checked;

		public SoulsSplitCollectionControl CollectionControl => collectionControl;

		public void Refresh(Split[] splits, bool useGameTime)
		{
			igtCheckbox.Checked = useGameTime;
			CollectionControl.ClearSplits();

			foreach (var split in splits)
			{
				CollectionControl.AddSplit(split);
			}
		}
	}
}