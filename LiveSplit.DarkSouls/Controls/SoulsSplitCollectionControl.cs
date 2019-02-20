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
		private SoulsSplitControl draggedSplit;

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
			var controls = splitsPanel.Controls;

			bool clear = true;

			// It seems annoying to prompt the player to clear splits when there's only one.
			if (controls.Count > 1)
			{
				DialogResult result = MessageBox.Show("Are you sure you want to clear your splits?", "Clear splits?",
					MessageBoxButtons.YesNo, MessageBoxIcon.Question);

				clear = result == DialogResult.Yes;
			}

			if (clear)
			{
				splitsPanel.Controls.Clear();
				UpdateSplitCount();
			}
		}

		private void UpdateSplitCount()
		{
			int count = splitsPanel.Controls.Count;

			splitCountLabel.Text = count + " split" + (count != 1 ? "s" : "");
			clearSplitsButton.Enabled = count > 0;
		}

		public void BeginDrag(SoulsSplitControl split)
		{
			Point localPoint = split.Location;

			draggedSplit = split;

			// Dragged splits are temporarily removed from the splits panel and added to the main control instead
			// (although the user shouldn't notice this).
			splitsPanel.Controls.Remove(split);
			Controls.Add(split);
			split.Location = splitsPanel.Location.Plus(localPoint);
			split.BringToFront();

			splitShadow.Bounds = split.Bounds;
			splitShadow.Visible = true;
		}

		public void UpdateDrag(int targetY)
		{
			int correctedY = targetY;
			correctedY = Math.Max(correctedY, splitsPanel.Top);
			correctedY = Math.Min(correctedY, splitsPanel.Bottom - draggedSplit.Height);

			var splits = splitsPanel.Controls;
			int dragIndex = draggedSplit.Index;
			int shadowY = splitShadow.Top;

			// This checks splits above the dragged split (meaning those splits should be shifted down if a valid
			// replacement index is found).
			if (CheckShift(0, dragIndex - 1, out int result))
			{
				SoulsSplitControl resultSplit = (SoulsSplitControl)splits[result];
				shadowY = resultSplit.Top + splitsPanel.Top;

				for (int i = result; i < dragIndex; i++)
				{
					SoulsSplitControl split = (SoulsSplitControl)splits[i];
					split.Top += draggedSplit.Height;
					split.Index++;
				}
			}
			// Note that since the dragged split is technically removed from the splits panel, some of the indices
			// involved this part are decreased by one.
			else if (CheckShift(dragIndex, splits.Count - 1, out result))
			{
				for (int i = dragIndex; i <= result; i++)
				{
					SoulsSplitControl split = (SoulsSplitControl)splits[i];
					split.Top -= draggedSplit.Height;
					split.Index--;
				}

				shadowY = splits[result].Bottom + splitsPanel.Top;
				result++;
			}

			if (result != -1)
			{
				draggedSplit.Index = result;
			}

			Point location = draggedSplit.Location;
			location.Y = correctedY;
			draggedSplit.Location = location;

			splitShadow.Top = shadowY;
		}

		// Note that if this function returns true, result is set to the index that the dragged split should replace
		// (meaning that in-between splits should shift).
		private bool CheckShift(int start, int end, out int result)
		{
			if (end < start)
			{
				result = -1;

				return false;
			}

			// A dragged split is considered to have changed position when its midpoint crosses into another split's
			// bounds.
			int halfHeight = draggedSplit.Height / 2;
			int draggedMidpoint = draggedSplit.Top + halfHeight - splitsPanel.Top;

			var splits = splitsPanel.Controls;

			for (int i = start; i <= end; i++)
			{
				SoulsSplitControl split = (SoulsSplitControl)splits[i];

				int midpoint = split.Top + split.Height / 2;
				int delta = Math.Abs(draggedMidpoint - midpoint);

				if (delta < halfHeight)
				{
					result = i;

					return true;
				}
			}

			result = -1;

			return false;
		}

		public void Drop()
		{
			Controls.Remove(draggedSplit);

			var controls = splitsPanel.Controls;
			controls.Add(draggedSplit);
			controls.SetChildIndex(draggedSplit, draggedSplit.Index);

			draggedSplit.Location = splitShadow.Location.Minus(splitsPanel.Location);
			draggedSplit = null;
			splitShadow.Visible = false;
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

		public void ClearSplits()
		{
			splitsPanel.Controls.Clear();
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