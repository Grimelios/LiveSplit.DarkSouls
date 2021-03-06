﻿using System;
using System.Reflection;
using LiveSplit.DarkSouls;
using LiveSplit.Model;
using LiveSplit.UI.Components;

[assembly: ComponentFactory(typeof(SoulsFactory))]

namespace LiveSplit.DarkSouls
{
	public class SoulsFactory : IComponentFactory
	{
		public string ComponentName => SoulsComponent.DisplayName + " v" + Version;

		public string Description =>
			"Configurable autosplitter and IGT tool for Dark Souls: Prepare To Die Edition. Does not work for the remaster.";

		public string UpdateName => ComponentName;
		public string XMLURL => UpdateURL + "Components/Updates.xml";
		public string UpdateURL => "https://raw.githubusercontent.com/Grimelios/LiveSplit.DarkSouls/master/";

		public ComponentCategory Category => ComponentCategory.Control;

		public Version Version => Utilities.GetVersion();

		public IComponent Create(LiveSplitState state)
		{
			return new SoulsComponent();
		}
	}
}