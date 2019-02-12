using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.DarkSouls.Memory
{
	public enum BonfireFlags
	{
		AnorLondoEntrance = 1511960,
		AnorLondoInterior = 1511961,
		AnorLondoPrincess = 1511950,
		AnorLondoTomb = 1511962,
		AshLakeEntrance = 1321961,
		AshLakeDragon = 1321960,
		BlighttownBridge = 1401962,
		BlighttownSwamp = 1401961,
		CatacombsEntrance = 1301960,
		CatacombsIllusion = 1301961,
		ChasmOfTheAbyss = 1211950,
		CrystalCaves = 1701950,
		DarkrootBasin = 1601961,
		DarkrootGarden = 1201961,
		DaughterOfChaos = 1401960,
		DemonRuinsCentral = 1411962,
		DemonRuinsFiresage = 1411963,
		DemonRuinsEntrance = 1411961,
		DukesArchivesBalcony = 1701960,
		DukesArchivesEntrance = 1701962,
		DukesArchivesPrison = 1701961,
		FirelinkAltar = 1801960,
		FirelinkShrine = 1021960,
		GreatHollow = 1321962,
		LostIzalithBedOfChaos = 1411950,
		LostIzalithIllusion = 1411960,
		LostIzalithLavaField = 1411964,
		OolacileSanctuary = 1211961,
		OolacileTownshipDungeon = 1211964,
		OolacileTownshipEntrance = 1211962,
		PaintedWorld = 1101960,
		SanctuaryGarden = 1211963,
		SensFortress = 1501961,
		TheAbyss = 1601950,
		TheDepths = 1001960,
		TombOfTheGiantsAlcove = 1311960,
		TombOfTheGiantsNito = 1311950,
		TombOfTheGiantsPatches = 1311961,
		UndeadAsylumCourtyard = 1811960,
		UndeadAsylumInterior = 1811961,
		UndeadBurgEntrance = 1011962,
		UndeadBurgSunlight = 1011961,
		UndeadParish = 1011964
	}

	public enum BonfireStates
	{
		// Enum values are intentionally listed to match the bonfire criteria dropdown (Inactive takes the place of
		// "On rest").
		Lit = 10,

		// "Inactive" means that the bonfire's fire keeper is not present.
		Inactive,
		KindledOnce = 20,
		KindledTwice = 40,
		KindledThrice = 30,
		Undiscovered,
		Unlit = 0
	}

	public enum BossFlags
	{
		Artorias = 11210001,
		AsylumDemon = 16,
		BedOfChaos = 10,
		CapraDemon = 11010902,
		CeaselessDischarge = 11410900,
		CentipedeDemon = 11410901,
		Firesage = 11410410,
		FourKings = 13,
		GapingDragon = 2,
		Gargoyles = 3,
		Gwyn = 15,
		Gwyndolin = 11510900,
		IronGolem = 11,
		Kalameet = 11210004,
		Manus = 11210002,
		MoonlightButterfly = 11200900,
		Nito = 7,
		OrnsteinAndSmough = 12,
		Pinwheel = 6,
		Priscilla = 4,
		Quelaag = 9,
		SanctuaryGuardian = 11210000,
		Seath = 14,
		Sif = 5,
		StrayDemon = 11810900,
		TaurusDemon = 11010901
	}

	public enum CovenantFlags
	{
	}

	public static class Flags
	{
		public static readonly int[] BossFlags =
		{
			11210001, // Artorias
			16, // Asylum Demon
			10, // Bed of Chaos
			11010902, // Capra Demon
			11410900, // Ceaseless Discharge
			11410901, // Centipede Demon
			11410410, // Firesage
			13, // Four Kings
			2, // Gaping Dragon
			3, // Gargoyles
			15, // Gwyn
			11510900, // Gwyndolin
			11, // Iron Golem
			11210004, // Kalemeet
			11210002, // Manus
			11200900, // Moonlight Butterfly
			7, // Nito
			12, // Ornstein and Smough
			6, // Pinwheel
			4, // Priscilla
			9, // Quelaag
			11210000, // Sanctuary Guardian
			14, // Seath
			5, // Sif
			11810900, // Stray Demon
			11010901 // Taurus Demons
		};
	}
}
