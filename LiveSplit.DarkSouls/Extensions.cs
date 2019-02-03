using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace LiveSplit.DarkSouls
{
	public static class Extensions
	{
		public static XmlElement CreateElement(this XmlDocument document, string tag, string value)
		{
			XmlElement element = document.CreateElement(tag);
			element.InnerText = value;

			return element;
		}
	}
}
