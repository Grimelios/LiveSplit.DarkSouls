using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.DarkSouls.Data
{
	public class RunData
	{
		public bool BossDefeated { get; set; }

		public int PreviousGameTime { get; set; }
		public int MaxGameTime { get; set; }
	}
}
