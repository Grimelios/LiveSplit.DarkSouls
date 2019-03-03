using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.DarkSouls.Memory
{
	public enum AnimationFlags
	{
		// These three flags are matched with their respective sitting animations below.
		BonfireLeave1 = 7702,
		BonfireLeave2 = 7712,
		BonfireLeave3 = 7722,

		// These three flags correspond to all possible sitting styles the player can take when resting at a bonfire.
		BonfireSit1 = 7700,
		BonfireSit2 = 7710,
		BonfireSit3 = 7720,
		BonfireWarp = 7725
	}

	public enum BellFlags
	{
		FirstBell = 11010700,
		SecondBell = 11400200
	}

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
		// Note that "inactive" bonfires (bonfires that are disabled because their fire keeper is dead) aren't given
		// their own state (at least not at the same memory location).
		Lit = 10,
		KindledOnce = 20,
		KindledTwice = 30,
		KindledThrice = 40,
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
		Chaos = 9,
		Darkmoon = 8,
		Darkwraith = 4,
		Dragon = 5,
		Forest = 7,
		Gravelord = 6,
		None = 0,
		Princess = 2,
		Sunlight = 3,
		WayOfWhite = 1
	}

	public enum InventoryFlags
	{
		ItemCount = 0x128,
		ItemStart = 0xA24,
		KeyCount = 0x12C,
		KeyStart = 0x324
	}

	public static class Flags
	{
		public static readonly int[] OrderedBonfires =
		{
			(int)BonfireFlags.AnorLondoEntrance,
			(int)BonfireFlags.AnorLondoInterior,
			(int)BonfireFlags.AnorLondoPrincess,
			(int)BonfireFlags.AnorLondoTomb,
			(int)BonfireFlags.AshLakeEntrance,
			(int)BonfireFlags.AshLakeDragon,
			(int)BonfireFlags.BlighttownBridge,
			(int)BonfireFlags.BlighttownSwamp,
			(int)BonfireFlags.CatacombsEntrance,
			(int)BonfireFlags.CatacombsIllusion,
			(int)BonfireFlags.ChasmOfTheAbyss,
			(int)BonfireFlags.CrystalCaves,
			(int)BonfireFlags.DarkrootBasin,
			(int)BonfireFlags.DarkrootGarden,
			(int)BonfireFlags.DaughterOfChaos,
			(int)BonfireFlags.DemonRuinsCentral,
			(int)BonfireFlags.DemonRuinsFiresage,
			(int)BonfireFlags.DemonRuinsEntrance,
			(int)BonfireFlags.DukesArchivesBalcony,
			(int)BonfireFlags.DukesArchivesEntrance,
			(int)BonfireFlags.DukesArchivesPrison,
			(int)BonfireFlags.FirelinkAltar,
			(int)BonfireFlags.FirelinkShrine,
			(int)BonfireFlags.GreatHollow,
			(int)BonfireFlags.LostIzalithBedOfChaos,
			(int)BonfireFlags.LostIzalithIllusion,
			(int)BonfireFlags.LostIzalithLavaField,
			(int)BonfireFlags.OolacileSanctuary,
			(int)BonfireFlags.OolacileTownshipDungeon,
			(int)BonfireFlags.OolacileTownshipEntrance,
			(int)BonfireFlags.PaintedWorld,
			(int)BonfireFlags.SanctuaryGarden,
			(int)BonfireFlags.SensFortress,
			(int)BonfireFlags.TheAbyss,
			(int)BonfireFlags.TheDepths,
			(int)BonfireFlags.TombOfTheGiantsAlcove,
			(int)BonfireFlags.TombOfTheGiantsNito,
			(int)BonfireFlags.TombOfTheGiantsPatches,
			(int)BonfireFlags.UndeadAsylumCourtyard,
			(int)BonfireFlags.UndeadAsylumInterior,
			(int)BonfireFlags.UndeadBurgEntrance,
			(int)BonfireFlags.UndeadBurgSunlight,
			(int)BonfireFlags.UndeadParish
		};

		public static readonly int[] OrderedBonfireStates =
		{
			// The placeholder -1 is required to line up with the bonfire state dropdown.
			(int)BonfireStates.Lit,
			-1,
			(int)BonfireStates.KindledOnce,
			(int)BonfireStates.KindledTwice,
			(int)BonfireStates.KindledThrice
		};

		public static readonly int[] OrderedBosses =
		{
			(int)BossFlags.Artorias,
			(int)BossFlags.AsylumDemon,
			(int)BossFlags.BedOfChaos,
			(int)BossFlags.CapraDemon,
			(int)BossFlags.CeaselessDischarge,
			(int)BossFlags.CentipedeDemon,
			(int)BossFlags.Firesage,
			(int)BossFlags.FourKings,
			(int)BossFlags.GapingDragon,
			(int)BossFlags.Gargoyles,
			(int)BossFlags.Gwyn,
			(int)BossFlags.Gwyndolin,
			(int)BossFlags.IronGolem,
			(int)BossFlags.Kalameet,
			(int)BossFlags.Manus,
			(int)BossFlags.MoonlightButterfly,
			(int)BossFlags.Nito,
			(int)BossFlags.OrnsteinAndSmough,
			(int)BossFlags.Pinwheel,
			(int)BossFlags.Priscilla,
			(int)BossFlags.Quelaag,
			(int)BossFlags.SanctuaryGuardian,
			(int)BossFlags.Seath,
			(int)BossFlags.Sif,
			(int)BossFlags.StrayDemon,
			(int)BossFlags.TaurusDemon
		};

		public static readonly int[] OrderedCovenants =
		{
			// Note that the Darkmoon covenant is actually "Blade of the Darkmoon" and the Dragon covenant is "Path of
			// the Dragon" (which changes the alphabetical ordering).
			(int)CovenantFlags.Darkmoon,
			(int)CovenantFlags.Chaos,
			(int)CovenantFlags.Darkwraith,
			(int)CovenantFlags.Forest,
			(int)CovenantFlags.Gravelord,
			(int)CovenantFlags.Dragon,
			(int)CovenantFlags.Princess,
			(int)CovenantFlags.Sunlight,
			(int)CovenantFlags.WayOfWhite
		};
	}
}