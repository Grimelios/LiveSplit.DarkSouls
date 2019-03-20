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

		private int unfinishedCount;
		private int previousUnfinishedCount;

		// This value is used on load to make sure splits don't erroneously decrement the unfinished count. Bit of a
		// sloppy solution, but it works.
		private bool ignoreOnRefresh;

		public SoulsSplitCollectionControl()
		{
			InitializeComponent();
			UnfinishedCount = 0;
		}

		public int SplitCount => splitsPanel.Controls.Count;

		// This value is updated by child splits as their details change.
		public int UnfinishedCount
		{
			get => unfinishedCount;
			set
			{
				if (ignoreOnRefresh)
				{
					return;
				}

				previousUnfinishedCount = unfinishedCount;

				// When loading splits from a file, split controls have their data refreshed from a split object. This
				// can cause the unfinished count to dip into the negatives due to the (wrong) assumption that the
				// split was previously invalid (when in reality, it just wasn't loaded yet). This check fixes that
				// problem.
				unfinishedCount = Math.Max(value, 0);
				UpdateUnfinishedCount();
			}
		}

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
				ClearSplits();
			}
		}

		private void UpdateSplitCount()
		{
			int count = splitsPanel.Controls.Count;

			splitCountLabel.Text = count + " split" + (count != 1 ? "s" : "");
			clearSplitsButton.Enabled = count > 0;
		}

		private void UpdateUnfinishedCount()
		{
			// I found pure red and green (255) to be too bright.
			const int Red = 225;

			if (unfinishedCount > 0)
			{
				string s = "split" + (unfinishedCount != 1 ? "s" : "");

				unfinishedSplitsLabel.Text = $"{unfinishedCount} {s} unfinished";
				unfinishedSplitsLabel.ForeColor = Color.FromArgb(255, Red, 0, 0);

				if (previousUnfinishedCount == 0)
				{
					unfinishedSplitsLabel.Visible = true;
				}
			}
			else if (splitsPanel.Controls.Count > 0)
			{
				unfinishedSplitsLabel.Text = "All splits finished";
				unfinishedSplitsLabel.ForeColor = Color.LimeGreen;
			}
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

			bool nullSplit = split == null;

			if (!nullSplit)
			{
				ignoreOnRefresh = true;
				control.Refresh(split);
				ignoreOnRefresh = false;
			}
			
			if (nullSplit || !split.IsFinished)
			{
				UnfinishedCount++;
			}

			int y = controls.Count == 0 ? 0 : controls[controls.Count - 1].Bottom;

			control.Location = new Point(0, y);
			controls.Add(control);

			if (!unfinishedSplitsLabel.Visible)
			{
				unfinishedSplitsLabel.Visible = true;

				// If the split was invalid, the label's text and color would have already been updated above (when
				// incrementing the unfinished count).
				if (!nullSplit && split.IsFinished)
				{
					UpdateUnfinishedCount();
				}
			}

			UpdateSplitCount();
			splitsPanel.ScrollControlIntoView(control);
		}

		public void RemoveSplit(int index)
		{
			// This function is only called from split controls, which means the index is guaranteed to be valid.
			var controls = splitsPanel.Controls;
			var split = (SoulsSplitControl)controls[index];

			int height = split.Height;

			controls.RemoveAt(index);

			if (!split.IsFinished)
			{
				UnfinishedCount--;
			}

			if (controls.Count == 0)
			{
				unfinishedSplitsLabel.Visible = false;
			}

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

			unfinishedCount = 0;
			unfinishedSplitsLabel.Visible = false;

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