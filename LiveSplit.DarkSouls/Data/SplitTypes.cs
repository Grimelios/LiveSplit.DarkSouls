using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.DarkSouls.Data
{
	public enum SplitTypes
	{
		Bonfire,
		Boss,
		Covenant,
		Events,
		Item,
		Zone,

		// This value allows splits to be created and always exist for a split line, even if no type/data has been
		// selected.
		Unassigned
	}
}
