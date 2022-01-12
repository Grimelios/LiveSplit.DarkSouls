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

			// A split is considered valid if all dropdowns have valid values (or the split is manual/quitout). Note
			// that disabled dropdowns have their index stored as int.MaxValue rather than -1 (in order to keep the
			// simple rule that -1 always means unfinished).
			IsFinished = type == SplitTypes.Manual || type == SplitTypes.Quitout ||
				(data != null && data.All(d => d >= 0));
		}

		public SplitTypes Type { get; set; }

		public int[] Data { get; set; }

		public bool IsFinished{ get; }
	}
}