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
	public partial class SoulsSplitCollectionControl : UserControl
	{
		private int splitHeight = -1;

		public SoulsSplitCollectionControl()
		{
			InitializeComponent();
		}

		private void addSplitButton_Click(object sender, EventArgs e)
		{
			AddSplit();
		}

		private void clearSplitsButton_Click(object sender, EventArgs e)
		{
			splitsPanel.Controls.Clear();
			UpdateSplitCount();
		}

		private void UpdateSplitCount()
		{
			int count = splitsPanel.Controls.Count;

			splitCountLabel.Text = count + " split" + (count != 1 ? "s" : "");
		}

		public void AddSplit(Split split = null)
		{
			var controls = splitsPanel.Controls;

			SoulsSplitControl control = new SoulsSplitControl(this, controls.Count);

			if (split != null)
			{
				control.Refresh(split);
			}

			if (splitHeight == -1)
			{
				splitHeight = control.Bounds.Height;
			}

			control.Location = new Point(0, splitHeight * controls.Count);
			controls.Add(control);

			UpdateSplitCount();
		}

		public void RemoveSplit(int index)
		{
			// This function is only called from split controls, which means the index is guaranteed to be valid.
			var controls = splitsPanel.Controls;
			controls.RemoveAt(index);

			for (int i = index; i < controls.Count; i++)
			{
				SoulsSplitControl control = (SoulsSplitControl)controls[i];
				Point point = control.Location;

				point.Y -= splitHeight;
				control.Location = point;
				control.Index--;
			}

			UpdateSplitCount();
		}

		public Split[] ExtractSplits()
		{
			var controls = splitsPanel.Controls;

			if (controls.Count == 0)
			{
				return null;
			}

			Split[] splits = new Split[controls.Count];

			for (int i = 0; i < controls.Count; i++)
			{
				splits[i] = ((SoulsSplitControl)controls[i]).ExtractSplit();
			}

			return splits;
		}
	}
}
