using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.DarkSouls.Data
{
	public class Split
	{
		public Split() : this(SplitTypes.Unassigned, null)
		{
		}

		public Split(SplitTypes type, int[] data)
		{
			Type = type;
			Data = data;

			// A split is considered valid if all dropdowns have valid values. Note that for items with modification or
			// reinforcement enabled, empty dropdowns are saved as int.MaxValue rather than -1 (in order to keep the 
			// simple rule that -1 values are always invalid).
			IsValid = type != SplitTypes.Unassigned && data.All(d => d >= 0);
		}

		public SplitTypes Type { get; set; }

		public int[] Data { get; set; }

		public bool IsValid { get; }
	}
}