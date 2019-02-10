using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LiveSplit.DarkSouls.Data
{
	public class UpgradeData
	{
		public UpgradeData(int maxReinforcement, ModificationTypes availableMods)
		{
			MaxReinforcement = maxReinforcement;
			AvailableMods = availableMods;
		}

		public ModificationTypes AvailableMods { get; }
		
		public int MaxReinforcement { get; }
	}
}
