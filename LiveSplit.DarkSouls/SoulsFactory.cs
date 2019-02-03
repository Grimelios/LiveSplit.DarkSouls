using System;
using System.Reflection;
using LiveSplit.Model;
using LiveSplit.UI.Components;

namespace LiveSplit.DarkSouls
{
	public class SoulsFactory : IComponentFactory
	{
		public string ComponentName => SoulsComponent.DisplayName;
		public string Description => "Configurable autosplitter and IGT tool for Dark Souls. Does not work for the remaster.";
		public string UpdateName => ComponentName;
		public string XMLURL => UpdateURL + "LiveSplit.DarkSouls/LiveSplit.DarkSouls.Updates.xml";
		public string UpdateURL => "https://raw.githubusercontent.com/Grimelios/LiveSplit.DarkSouls/master/";

		public ComponentCategory Category => ComponentCategory.Control;

		public Version Version
		{
			get
			{
				Version version = Assembly.GetExecutingAssembly().GetName().Version;

				return Version.Parse($"{version.Major}.{version.Minor}.{version.Build}");
			}
		}

		public IComponent Create(LiveSplitState state)
		{
			return new SoulsComponent();
		}
	}
}
