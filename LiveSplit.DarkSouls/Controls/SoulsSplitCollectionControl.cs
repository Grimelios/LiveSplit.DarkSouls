using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LiveSplit.DarkSouls.Controls
{
	public partial class SoulsSplitCollectionControl : UserControl
	{
		public SoulsSplitCollectionControl()
		{
			InitializeComponent();
		}

		private void addSplitButton_Click(object sender, EventArgs e)
		{
			splitsPanel.Controls.Add(new SoulsSplitControl());
		}

		private void clearSplitsButton_Click(object sender, EventArgs e)
		{
			splitsPanel.Controls.Clear();
		}
	}
}
