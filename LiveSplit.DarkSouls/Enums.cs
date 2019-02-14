using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.DarkSouls
{
	public enum WorldEvents
	{
		// These indices correspond to values in the event dropdown.
		Bell1 = 1,
		Bell2 = 2,
		DarkLord = 5,
		LinkTheFire = 6
	}

	public enum ModificationTypes
	{
		None,
		Restricted,
		Special,
		Standard
	}
}