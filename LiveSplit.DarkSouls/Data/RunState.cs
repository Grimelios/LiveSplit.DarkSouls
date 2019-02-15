using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveSplit.DarkSouls.Memory;

namespace LiveSplit.DarkSouls.Data
{
	public class RunState
	{
		public bool IsBossDefeated { get; set; }

		public BonfireFlags BonfireFlag { get; set; }
		public BonfireStates BonfireState { get; set; }
		public BonfireStates TargetBonfireState { get; set; }

		public int GameTime { get; set; }
		public int MaxGameTime { get; set; }
		public int BossFlag { get; set; }
		public int Data { get; set; }
		public int Target { get; set; }
		public int Id { get; set; }

		public bool Flag { get; set; }
	}
}