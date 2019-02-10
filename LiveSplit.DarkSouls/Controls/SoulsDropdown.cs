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
		private int previousIndex = -1;

		public SoulsDropdown()
		{
			DropDownStyle = ComboBoxStyle.DropDownList;
			DrawMode = DrawMode.OwnerDrawFixed;
		}

		public string Prompt { get; set; }

		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			const int SeparatorPadding = 2;

			string value;

			Graphics g = e.Graphics;
			Color textColor = Color.Black;
			Rectangle bounds = e.Bounds;

			int offsetX = -1;
			int offsetY = 1;

			if (e.Index == -1)
			{
				value = Prompt;
				textColor = SystemColors.ButtonShadow;
			}
			else
			{
				value = Items[e.Index].ToString();

				if (value.Length == 0)
				{
					return;
				}

				e.DrawBackground();

				bool isCategoryLine = value[0] == '-';

				if ((e.State & DrawItemState.Selected) > 0)
				{
					// Category lines (starting with a dash) aren't filled with a visible color in order to emphasize
					// that they're not selectable.
					Color highlightColor = isCategoryLine ? Color.White : Color.LimeGreen;

					g.FillRectangle(new SolidBrush(highlightColor), bounds);
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
		}

		public void RefreshPrompt(string prompt, bool enabled = false)
		{
			Prompt = prompt;
			Enabled = enabled;
			SelectedIndex = -1;

			// Clearing items also invalidates the control.
			Items.Clear();
		}
	}
}
