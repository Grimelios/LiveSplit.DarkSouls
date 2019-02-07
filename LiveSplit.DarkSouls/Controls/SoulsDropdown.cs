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
			FlatStyle = FlatStyle.Flat;
			BackColor = SystemColors.ControlLight;
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

				if (value[0] == '-')
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

			string value = Items[SelectedIndex].ToString();

			if (value.Length == 0 || value[0] == '-')
			{
				SelectedIndex = previousIndex;

				return;
			}

			previousIndex = SelectedIndex;

			base.OnSelectedIndexChanged(e);
		}
	}
}
