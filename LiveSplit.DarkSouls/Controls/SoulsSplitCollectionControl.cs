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

		// As a split is dragged, its index is continuously re-computed in order to properly shift surrounding splits.
		private int dragIndex;

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

			int newIndex = RecomputeDragIndex();

			if (newIndex != dragIndex)
			{
				var splits = splitsPanel.Controls;

				// This means that the held split was dragged down, meaning that in-between splits should be shifted
				// up.
				if (newIndex > dragIndex)
				{
					int shift = splits[dragIndex].Height;

					for (int i = dragIndex; i < newIndex; i++)
					{
						SoulsSplitControl split = (SoulsSplitControl)splits[i];
						Point point = split.Location;
						point.Y -= shift;
						split.Location = point;
					}
				}
				else
				{
				}

				dragIndex = newIndex;
			}

			Point location = draggedSplit.Location;
			location.Y = correctedY;
			draggedSplit.Location = location;
		}

		private int RecomputeDragIndex()
		{
			var splits = splitsPanel.Controls;

			// This indicates that exactly one split has been added (the one currently being dragged).
			if (splits.Count == 0)
			{
				return 0;
			}

			// A dragged split is considered to have changed position when its midpoint crosses into another split's
			// bounds.
			int draggedY = draggedSplit.Location.Y + draggedSplit.Height / 2;

			if (draggedY > splits[splits.Count - 1].Bottom + 1)
			{
				return splits.Count;
			}

			for (int i = 0; i < splits.Count; i++)
			{
				SoulsSplitControl split = (SoulsSplitControl)splits[i];

				if (draggedY >= split.Top && draggedY <= split.Bottom + 1)
				{
					return i;
				}
			}

			// This situation should never occur.
			return -1;
		}

		public void Drop()
		{
			draggedSplit.Location = splitShadow.Location;
			splitShadow.Visible = false;
			Controls.Remove(draggedSplit);
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