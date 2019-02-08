using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.DarkSouls
{
	public enum BonfireStates
	{
		// "Inactive" means that the bonfire's fire keeper is not present.
		Inactive,
		KindledOnce = 20,
		KindledThrice = 30,
		KindledTwice = 40,
		Lit = 10,
		Undiscovered,
		Unlit = 0
	}

	public enum CovenantStates
	{
		Discovered,
		Joined
	}
}
