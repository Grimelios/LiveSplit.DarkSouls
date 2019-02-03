using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LiveSplit.DarkSouls.Controls
{
	public class SoulsLabel : Label
	{
		private const float TextPadding = 2;

		public SoulsLabel()
		{
			AutoSize = false;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			var bounds = e.ClipRectangle;

			int midWidth = bounds.Width / 2;
			int midHeight = bounds.Height / 2;

			float halfTextWidth = e.Graphics.MeasureString(Text, Font).Width / 2;

			PointF topLeft = new PointF(0, midHeight);
			PointF topRight = new PointF(bounds.Right - 1, midHeight);
			PointF bottomLeft = new PointF(0, bounds.Bottom);
			PointF bottomRight = new PointF(bounds.Right - 1, bounds.Bottom);
			PointF textLeft = new PointF(midWidth - halfTextWidth - TextPadding, midHeight);
			PointF textRight = new PointF(midWidth + halfTextWidth + TextPadding, midHeight);
			PointF textPosition = new PointF(midWidth - halfTextWidth, 0);

			Pen pen = SystemPens.ButtonShadow;

			e.Graphics.DrawLine(pen, topLeft, bottomLeft);
			e.Graphics.DrawLine(pen, topLeft, textLeft);
			e.Graphics.DrawLine(pen, topRight, textRight);
			e.Graphics.DrawLine(pen, topRight, bottomRight);
			e.Graphics.DrawString(Text, Font, new SolidBrush(ForeColor), textPosition);
		}
	}
}
