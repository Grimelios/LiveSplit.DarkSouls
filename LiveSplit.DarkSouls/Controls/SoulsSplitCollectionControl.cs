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
			var controls = splitsPanel.Controls;

			SoulsSplitControl control = new SoulsSplitControl();

			if (controls.Count > 0)
			{
				control.Location = new Point(0, control.Bounds.Height * controls.Count);
			}

			controls.Add(control);
		}

		private void clearSplitsButton_Click(object sender, EventArgs e)
		{
			splitsPanel.Controls.Clear();
		}
	}
}
