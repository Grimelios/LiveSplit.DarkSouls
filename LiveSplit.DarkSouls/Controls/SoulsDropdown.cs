using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LiveSplit.DarkSouls.Controls
{
	public class SoulsDropdown : ComboBox
	{
		private static readonly Color DefaultColor = Color.White;
		private static readonly Color UnfinishedColor = Color.PaleVioletRed;

		private SoulsSplitControl parent;

		private int previousIndex = -1;

		// The parent split is null for split type (which is added directly from the forms editor).
		public SoulsDropdown(SoulsSplitControl parent = null)
		{
			this.parent = parent;

			DropDownStyle = ComboBoxStyle.DropDownList;
			DrawMode = DrawMode.OwnerDrawFixed;

			// When a dropdown is created, it's unfinished by default (because no value is selected).
			BackColor = UnfinishedColor;
			ForeColor = SystemColors.ControlText;
		}

		public string Prompt { get; set; }

		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			const int SeparatorPadding = 2;
			const int UnfinishedTextColor = 80;

			string value;

			Graphics g = e.Graphics;
			Color textColor = Color.Black;
			Rectangle bounds = e.Bounds;

			int offsetX = -1;
			int offsetY = 1;

			if (e.Index == -1)
			{
				value = Prompt;

				// Enabled splits with a -1 index have a red-tinted background. On that background, grey text is hard
				// to read.
				textColor = !Enabled
					? SystemColors.ButtonShadow
					: Color.FromArgb(255, UnfinishedTextColor, UnfinishedTextColor, UnfinishedTextColor);
			}
			else
			{
				value = Items[e.Index].ToString();

				if (value.Length == 0)
				{
					g.FillRectangle(new SolidBrush(Color.White), bounds);

					return;
				}
				
				bool isCategoryLine = value[0] == '-';

				if ((e.State & DrawItemState.Selected) > 0)
				{
					// Category lines (starting with a dash) aren't filled with a visible color in order to emphasize
					// that they're not selectable.
					Color highlightColor = isCategoryLine ? Color.White : Color.LimeGreen;

					g.FillRectangle(new SolidBrush(highlightColor), bounds);
				}
				else
				{
					g.FillRectangle(new SolidBrush(Color.White), bounds);
				}

				if (isCategoryLine)
				{
					textColor = SystemColors.ButtonShadow;
					value = value.Substring(2, value.Length - 4);
					offsetX = (bounds.Width - (int)g.MeasureString(value, Font).Width) / 2;

					int bottom = bounds.Bottom - 1;

					g.DrawLine(SystemPens.ButtonShadow, bounds.Left + SeparatorPadding, bottom,
						bounds.Right - SeparatorPadding - 1, bottom);
				}
			}

			// See http://blog.stevex.net/rendering-text-using-the-net-framework/.
			TextRenderer.DrawText(g, value, Font, new Point(bounds.X + offsetX, bounds.Y + offsetY), textColor);
		}

		protected override void OnEnabledChanged(EventArgs e)
		{
			const int DisabledLightness = 240;

			BackColor = Enabled
				? UnfinishedColor
				: Color.FromArgb(255, DisabledLightness, DisabledLightness, DisabledLightness);

			base.OnEnabledChanged(e);
		}

		protected override void OnSelectedIndexChanged(EventArgs e)
		{
			if (SelectedIndex == previousIndex)
			{
				return;
			}

			if (SelectedIndex == -1)
			{
				previousIndex = -1;

				return;
			}

			string value = Items[SelectedIndex].ToString();

			if (value.Length == 0 || value[0] == '-')
			{
				SelectedIndex = previousIndex;

				return;
			}

			previousIndex = SelectedIndex;

			base.OnSelectedIndexChanged(e);

			// This function is intentionally called after other callbacks (to ensure other split controls are updated
			// before the split's finished state is updated).
			if (SelectedIndex >= 0)
			{
				parent?.RefreshFinished();
				BackColor = DefaultColor;
			}
		}

		public void RefreshPrompt(string prompt, bool enabled = false)
		{
			Prompt = prompt;
			Enabled = enabled;
			SelectedIndex = -1;

			if (enabled)
			{
				parent?.RefreshFinished(false);
				BackColor = UnfinishedColor;
			}

			// Clearing items also invalidates the control.
			Items.Clear();
		}
	}
}