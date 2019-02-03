using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using LiveSplit.DarkSouls.Data;

namespace LiveSplit.DarkSouls
{
	public class SoulsSettings
	{
		private SplitCollection splitCollection;

		public SoulsSettings(SplitCollection splitCollection)
		{
			this.splitCollection = splitCollection;
		}

		public bool PauseIgt { get; private set; }

		public void Load(XmlNode node)
		{
			PauseIgt = bool.Parse(node["PauseIGT"].InnerText);

			XmlNodeList splitNodes = node["Splits"].GetElementsByTagName("Split");
			
			Split[] splits = new Split[splitNodes.Count];

			for (int i = 0; i < splits.Length; i++)
			{
			}

			splitCollection.Splits = splits;
		}

		public XmlNode Save(XmlDocument document)
		{
			XmlElement root = document.CreateElement("Settings");
			XmlElement igtElement = document.CreateElement("PauseIGT", PauseIgt.ToString());
			XmlElement splitsElement = document.CreateElement("Splits");

			foreach (var split in splitCollection.Splits)
			{
				string data = string.Join("|", split.Data);

				XmlElement splitElement = document.CreateElement("Split");
				splitElement.AppendChild(document.CreateElement("Type", split.Type.ToString()));
				splitElement.AppendChild(document.CreateElement("Data", data));
			}

			root.AppendChild(igtElement);
			root.AppendChild(splitsElement);

			return root;
		}
	}
}
