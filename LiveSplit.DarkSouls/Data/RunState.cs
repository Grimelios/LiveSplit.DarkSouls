using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.DarkSouls.Data
{
	public class RunState
	{
		public bool BossDefeated { get; set; }

		public BonfireFlags BonfireFlag { get; set; }
		public BonfireStates BonfireState { get; set; }
		public BonfireStates TargetBonfireState { get; set; }
		public BossFlags BossFlag { get; set; }

		public int GameTime { get; set; }
		public int MaxGameTime { get; set; }
	}
}
