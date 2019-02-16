using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.DarkSouls.Data
{
	public class ItemData
	{
		private int id;
		private int modification;
		private int reinforcement;

		public ItemData(int id, int modification, int reinforcement)
		{
			this.id = id;
			this.modification = modification;
			this.reinforcement = reinforcement;
		}
	}
}
