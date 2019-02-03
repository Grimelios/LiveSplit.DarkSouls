using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.DarkSouls.Data
{
	public class SplitCollection
	{
		private Split currentSplit;

		private int splitIndex;

		public Split[] Splits { get; set; }

		public void OnSplit()
		{
			if (Splits.Length == 0)
			{
				return;
			}

			AdvanceSplit();
		}

		public void OnUndoSplit()
		{
			if (Splits.Length == 0)
			{
				return;
			}

			currentSplit = Splits[--splitIndex];
		}

		public void OnSkipSplit()
		{
			if (Splits.Length == 0)
			{
				return;
			}

			AdvanceSplit();
		}

		private void AdvanceSplit()
		{
			currentSplit = ++splitIndex < Splits.Length ? Splits[splitIndex] : null;
		}

		public void OnReset()
		{
			if (Splits.Length == 0)
			{
				return;
			}

			currentSplit = null;
			splitIndex = 0;
		}
	}
}
