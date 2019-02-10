using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveSplit.DarkSouls.Data;

namespace LiveSplit.DarkSouls.Controls
{
	public partial class SoulsSplitCollectionControl : UserControl
	{
		public SoulsSplitCollectionControl()
		{
			InitializeComponent();
		}

		public int SplitCount => splitsPanel.Controls.Count;

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
			clearSplitsButton.Enabled = count > 0;
		}

		public void AddSplit(Split split = null)
		{
			var controls = splitsPanel.Controls;

			SoulsSplitControl control = new SoulsSplitControl(this, controls.Count);

			if (split != null)
			{
				control.Refresh(split);
			}

			int y = controls.Count == 0 ? 0 : controls[controls.Count - 1].Bottom;

			control.Location = new Point(0, y);
			controls.Add(control);

			UpdateSplitCount();
		}

		public void RemoveSplit(int index)
		{
			// This function is only called from split controls, which means the index is guaranteed to be valid.
			var controls = splitsPanel.Controls;
			int height = controls[index].Height;

			controls.RemoveAt(index);

			for (int i = index; i < controls.Count; i++)
			{
				SoulsSplitControl control = (SoulsSplitControl)controls[i];
				Point point = control.Location;

				point.Y -= height;
				control.Location = point;
				control.Index--;
			}

			UpdateSplitCount();
		}

		public void ShiftSplits(int fromIndex)
		{
			var controls = splitsPanel.Controls;

			for (int i = fromIndex; i < controls.Count; i++)
			{
				Control control = controls[i];
				Point point = control.Location;
				point.Y = controls[i - 1].Bottom;
				control.Location = point;
			}
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
