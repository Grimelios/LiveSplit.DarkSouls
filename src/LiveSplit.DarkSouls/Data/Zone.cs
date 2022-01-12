using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.DarkSouls.Data
{
	// Originally, zone splits were meant to trigger when named locations appear on screen (meaning they'd be pretty
	// fine-grained). Later on, zones were changed to refer only to very large-scale, interconnected areas of the world
	// (such as the entire Undead Asylum or all of Lordran). Finding data for individually-named zones would probably
	// be very difficult, on top of the fact that lines between those named zones are pretty fuzzy anyway.
	public class Zone
	{
		public Zone(int world, int area)
		{
			World = world;
			Area = area;
		}

		public int World { get; }
		public int Area { get; }

		public override bool Equals(object obj)
		{
			Zone other = (Zone)obj;

			return World == other.World && Area == other.Area;
		}

		public override int GetHashCode()
		{
			return new Tuple<int, int>(World, Area).GetHashCode();
		}
	}
}
