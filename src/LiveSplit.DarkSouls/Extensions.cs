using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DarkSoulsMemory;

namespace LiveSplit.DarkSouls
{
	public static class Extensions
	{
		public static XmlElement CreateElementWithInnerText(this XmlDocument document, string tag, string value)
		{
			XmlElement element = document.CreateElement(tag);
			element.InnerText = value ?? "";

			return element;
		}

		public static Point Plus(this Point point, Point p)
		{
			return new Point(point.X + p.X, point.Y + p.Y);
		}

		public static Point Minus(this Point point, Point p)
		{
			return new Point(point.X - p.X, point.Y - p.Y);
		}

        public static float ComputeDistanceSquared(this Vector3f self, Vector3f other)
        {
            float dX = self.X - other.X;
            float dY = self.Y - other.Y;
            float dZ = self.Z - other.Z;

            return dX * dX + dY * dY + dZ * dZ;
        }

	}
}