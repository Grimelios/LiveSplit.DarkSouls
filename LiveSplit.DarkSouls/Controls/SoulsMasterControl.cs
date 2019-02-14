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

		public SoulsSplitCollectionControl CollectionControl { get; private set; }

		public void Refresh(Split[] splits, bool useGameTime)
		{
			igtCheckbox.Checked = useGameTime;

			foreach (var split in splits)
			{
				CollectionControl.AddSplit(split);
			}
		}
	}
}