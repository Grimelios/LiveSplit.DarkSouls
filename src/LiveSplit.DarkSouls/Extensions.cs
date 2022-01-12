using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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

		// In practice, this function will only be used for linked lists of integers to aid with item tracking. Coding
		// it with generics feels more correct, though.
		public static void InsertSorted<T>(this LinkedList<T> list, T value) where T : IComparable<T>
		{
			if (list.Count == 0)
			{
				list.AddLast(value);

				return;
			}

			LinkedListNode<T> node = list.First;

			while (node != null && value.CompareTo(node.Value) > 0)
			{
				node = node.Next;
			}

			if (node == null)
			{
				list.AddLast(value);

				return;
			}

			list.AddBefore(node, value);
		}

		public static Point Plus(this Point point, Point p)
		{
			return new Point(point.X + p.X, point.Y + p.Y);
		}

		public static Point Minus(this Point point, Point p)
		{
			return new Point(point.X - p.X, point.Y - p.Y);
		}
	}
}