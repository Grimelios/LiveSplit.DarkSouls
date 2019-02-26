using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.DarkSouls.Memory
{
	public enum AmmunitionFlags
	{
		DragonslayerArrow = 2007000,
		FeatherArrow = 2002000,
		FireArrow = 2003000,
		GoughsGreatArrow = 2008000,
		HeavyBolt = 2101000,
		LargeArrow = 2001000,
		LightningBolt = 2104000,
		MoonlightArrow = 2005000,
		PoisonArrow = 2004000,
		SniperBolt = 2102000,
		StandardArrow = 2000000,
		StandardBolt = 2100000,
		WoodBold = 2103000,
		WoodenArrow = 2006000
	}

	public enum AxeFlags
	{
		BattleAxe = 701000,
		BlackKnightGreataxe = 753000,
		ButcherKnife = 703000,
		CrescentAxe = 702000,
		DemonsGreataxe = 751000,
		DragonKingGreataxe = 752000,
		GargoyleTailAxe = 705000,
		GolemAxe = 704000,
		Greataxe = 750000,
		HandAxe = 700000,
		StoneGreataxe = 9015000
	}

	public enum BonfireItemFlags
	{
		ArmorSmithbox = 2601,
		BottomlessBox = 2608,
		Lordvessel = 2510,
		Repairbox = 2602,
		RiteOfKindling = 2607,
		WeaponSmithbox = 2600
	}

	public enum BowFlags
	{
		BlackBowOfPharis = 1202000,
		CompositeBow = 1204000,
		DarkmoonBow = 1205000,
		DragonslayerGreatbow = 1203000,
		GoughsGreatbow = 9021000,
		Longbow = 1201000,
		ShortBow = 1200000
	}

	public enum CatalystFlags
	{
		BeatricesCatalyst = 1301000,
		DemonsCatalyst = 1307000,
		IzalithCatalyst = 1308000,
		LogansCatalyst = 1303000,
		ManusCatalyst = 9017000,
		OolacileCatalyst = 9018000,
		OolacileIvoryCatalyst = 1305000,
		SorcerersCatalyst = 1300000,
		TinBanishmentCatalyst = 1302000,
		TinCrystallizationCatalyst = 1306000,
		TinDarkmoonCatalyst = 1304000
	}

	public enum ChestPieceFlags
	{
	}

	public enum ConsumableFlags
	{
		BloodredMossClump = 270,
		BloomingPurpleMossClump = 272,
		CharcoalPineResin = 310,
		DivineBlessing = 240,
		EggVermifuge = 275,
		ElizabethsMushroom = 230,
		EstusFlask = 201,
		GreenBlossom = 260,
		GoldPineResin = 311,
		HelloCarving = 510,
		HelpMeCarving = 514,
		HomewardBone = 330,
		Humanity = 500,
		ImSorryCarving = 513,
		PrismStone = 370,
		PurgingStone = 274,
		PurpleMossClump = 271,
		RepairPowder = 280,
		RottenPineResin = 313,
		ThankYouCarving = 511,
		TransientCurse = 312,
		TwinHumanities = 501,
		VeryGoodCarving = 512
	}

	public enum CovenantItemFlags
	{
		DriedFinger = 385,
		EyeOfDeath = 109,
		SunlightMedal = 375,
		SouvenirOfReprisal = 374
	}

	public enum CrossbowFlags
	{
		Avelyn = 1252000,
		HeavyCrossbow = 1251000,
		LightCrossbow = 1250000,
		SniperCrossbow = 1253000
	}

	public enum DaggerFlags
	{
		BanditsKnife = 103000,
		Dagger = 100000,
		DarkSilverTracer = 9011000,
		GhostBlade = 102000,
		ParryingDagger = 101000,
		PriscillasDagger = 104000
	}

	public enum EmberFlags
	{
		ChaosFlameEmber = 813,
		CrystalEmber = 802,
		DarkEmber = 810,
		DivineEmber = 808,
		EnchantedEmber = 807,
		LargeDivineEmber = 809,
		LargeEmber = 800,
		LargeFlameEmber = 812,
		LargeMagicEmber = 806,
		VeryLargeEmber = 801
	}

	public enum FistFlags
	{
		Caestus = 901000,
		Claws = 902000,
		DarkHand = 904000,
		DragonBoneFist = 903000
	}

	public enum FlameFlags
	{
		PyromancyFlame = 1330000,
		PyromancyFlameAscended = 1332000
	}

	public enum GauntletFlags
	{
	}

	public enum GreatswordFlags
	{
		AbyssGreatsword = 9012000,
		BastardSword = 300000,
		BlackKnightGreatsword = 354000,
		BlackKnightSword = 310000,
		Claymore = 301000,
		CrystalGreatsword = 304000,
		DemonGreatMachete = 352000,
		DragonGreatsword = 354000,
		Flamberge = 303000,
		GravelordSword = 453000,
		GreatLordGreatsword = 314000,
		Greatsword = 351000,
		GreatswordOfArtorias = 307000,
		GreatswordOfArtoriasCursed = 311000,
		ManSerpentGreatsword = 302000,
		MoonlightGreatsword = 309000,
		Murakumo = 451000,
		ObsidianGreatsword = 9020000,
		StoneGreatsword = 306000,
		Server = 450000,
		Zweihander = 350000
	}

	public enum HalberdFlags
	{
		BlackKnightHalberd = 1105000,
		GargoylesHalberd = 1103000,
		GiantsHalberd = 1101000,
		GreatScythe = 1150000,
		Halberd = 1100000,
		LifehuntScythe = 1151000,
		Lucerne = 1106000,
		Scythe = 1107000,
		TitaniteCatchPole = 1102000
	}

	public enum HammerFlags
	{
		BlacksmithGiantHammer = 811000,
		BlacksmithHammer = 810000,
		Club = 800000,
		DemonsGreatHammer = 852000,
		DragonTooth = 854000,
		Grant = 851000,
		GreatClub = 850000,
		HammerOfVamos = 812000,
		LargeClub = 855000,
		Mace = 801000,
		MorningStar = 802000,
		Pickaxe = 804000,
		ReinforcedClub = 809000,
		SmoughsHammer = 856000,
		Warpick = 803000
	}

	public enum HelmetFlags
	{
	}

	public enum KeyFlags
	{
		AnnexKey = 2009,
		ArchivePrisonExtraKey = 2020,
		ArchiveTowerCellKey = 2004,
		ArchiveTowerGiantCellKey = 2006,
		ArchiveTowerGiantDoorKey = 2005,
		BasementKey = 2001,
		BigPilgrimsKey = 2011,
		BlightttownKey = 2007,
		BrokenPendant = 2520,
		CageKey = 2003,
		CrestKey = 2022,
		CrestOfArtorias = 2002,
		DungeonCellKey = 2010,
		KeyToTheDepths = 2014,
		KeyToNewLondoRuins = 2008,
		KeyToTheSeal = 2013,
		MasterKey = 2100,
		MysteryKey = 2017,
		PeculiarDoll = 384,
		ResidenceKey = 2021,
		SewerChamberKey = 2018,
		UndeadAsylumF2EastKey = 2012,
		UndeadAsylumF2WestKey = 2012,
		WatchtowerBasementKey = 2019
	}

	public enum LeggingFlags
	{
	}

	public enum MiracleFlags
	{
		BountifulSunlight = 5050,
		DarkmoonBlade = 5910,
		EmitForce = 5320,
		Force = 5300,
		GravelordGreatswordDance = 5110,
		GravelordSwordDance = 5100,
		GreatHeal = 5010,
		GreatHealExcerpt = 5020,
		GreatLightningSpear = 5510,
		GreatMagicBarrier = 5610,
		Heal = 5000,
		Homeward = 5210,
		KarmicJustice = 5700,
		LightningSpear = 5500,
		MagicBarrier = 5600,
		Replenishment = 5040,
		SeekGuidance = 5400,
		SoothingSunlight = 5030,
		SunlightBlade = 5900,
		SunlightSpear = 5520,
		TranquilWalkOfPeace = 5800,
		VowOfSilence = 5810,
		WrathOfTheGods = 5310
	}

	public enum MultiplayerItemFlags
	{
		BlackSeparationCrystal = 103,
		BlueEyeOrb = 113,
		BookOfTheGuilty = 108,
		CrackedRedEyeOrb,
		DragonEye = 114,
		Indictment = 373,
		OrangeGuidanceSoapstone = 106,
		PurpleCowardsCrystal = 118,
		RedEyeOrb = 102,
		RedSignSoapstone = 101,
		ServantRoster = 112,
		WhiteSignSoapstone = 100
	}

	public enum OreFlags
	{
		BlueTitaniteChunk = 1040,
		BlueTitaniteSlab = 1080,
		DemonTitanite = 1120,
		DragonScale = 1110,
		GreenTitaniteShard = 1020,
		LargeTitaniteShard = 1010,
		RedTitaniteChunk = 1060,
		RedTitaniteSlab = 1100,
		TitaniteChunk = 1030,
		TitaniteSlab = 1070,
		TitaniteShard = 1000,
		TwinklingTitanite = 1130,
		WhiteTitaniteChunk = 1050,
		WhiteTitaniteSlab = 1090
	}

	public enum ProjectileFlags
	{
		AlluringSkull = 294,
		BlackFirebomb = 297,
		DungPie = 293,
		Firebomb = 292,
		LloydsTalisman = 296,
		PoisonThrowingKnife = 291,
		ThrowingKnife = 290
	}

	public enum PyromancyFlags
	{
		AcidSurge = 4220,
		BlackFlame = 4530,
		ChaosFireWhip = 4520,
		ChaosStorm = 4510,
		Combustion = 4100,
		Fireball = 4000,
		FireOrb = 4010,
		Firestorm = 4030,
		FireSurge = 4050,
		FireTempest = 4040,
		FireWhip = 4060,
		FlashSweat = 4310,
		GreatChaosFireball = 4500,
		GreatCombustion = 4110,
		GreatFireball = 4020,
		IronFlesh = 4300,
		PoisonMist = 4200,
		PowerWithin = 4400,
		ToxicMist = 4210,
		UndeadRapport = 4360
	};

	public enum RingFlags
	{
		BellowingDragoncrestRing = 115,
		BloodbiteRing = 109,
		BlueTearstoneRing = 147,
		CalamityRing = 150,
		CatCovenantRing = 103,
		CloranthyRing = 104,
		CovenantofArtorias = 138,
		CovetousGoldSerpentRing = 121,
		CovetousSilverSerpentRing = 122,
		CursebiteRing = 113,
		DarkmoonBladeCovenantRing = 102,
		DarkmoonSeanceRing = 149,
		DarkWoodGrainRing = 128,
		DuskCrownRing = 116,
		EastWoodGrainRing = 145,
		FlameStoneplateRing = 105,
		HavelsRing = 100,
		HawkRing = 119,
		HornetRing = 117,
		LeoRing = 144,
		LingeringDragoncrestRing = 141,
		OldWitchsRing = 137,
		OrangeCharredRing = 139,
		PoisonbiteRing = 110,
		RedTearstoneRing = 101,
		RareRingofSacrifice = 127,
		RingOfFavorAndProtection = 143,
		RingOfFog = 124,
		RingOfSacrifice = 126,
		RingOfSteelProtection = 120,
		RingOfTheEvilEye = 142,
		RingOfTheSunPrincess = 130,
		RingOfTheSunsFirstborn = 148,
		RustedIronRing = 125,
		SlumberingDragoncrestRing = 123,
		SpeckledStoneplateRing = 108,
		SpellStoneplateRing = 107,
		TinyBeingsRing = 111,
		ThunderStoneplateRing = 106,
		WhiteSeanceRing = 114,
		WolfRing = 146
	}

	public enum ShieldFlags
	{
	}

	public enum SorceryFlags
	{
		AuralDecoy = 3520,
		CastLight = 3500,
		Chameleon = 3550,
		CrystalMagicWeapon = 3120,
		CrystalSoulSpear = 3070,
		DarkBead = 3720,
		DarkFog = 3730,
		DarkOrb = 3710,
		FallControl = 3540,
		GreatHeavySoulArrow = 3030,
		GreatMagicWeapon = 3110,
		GreatSoulArrow = 3010,
		HeavySoulArrow = 3020,
		HiddenBody = 3410,
		HiddenWeapon = 3400,
		HomingCrystalSoulmass = 3050,
		HomingSoulmass = 3040,
		Hush = 3510,
		MagicShield = 3300,
		MagicWeapon = 3100,
		Pursuers = 3740,
		Remedy = 3610,
		Repair = 3530,
		ResistCurse = 6600,
		SoulArrow = 3000,
		SoulSpear = 3060,
		StrongMagicShield = 3310,
		WhiteDragonBreath = 3700
	}

	public enum SoulFlags
	{
		CoreOfAnIronGolem = 703,
		GuardianSoul = 709,
		LargeSoulOfABraveWarrior = 407,
		LargeSoulsOfALostUndead = 401,
		LargeSoulOfANamelessSoldier = 403,
		LargeSoulOfAProudKnight = 405,
		SoulOfABraveWarrior = 406,
		SoulOfAGreatHero = 409,
		SoulOfAHero = 408,
		SoulOfALostUndead = 400,
		SoulOfANamelessSoldier = 402,
		SoulOfAProudKnight = 404,
		SoulOfArtorias = 710,
		SoulOfGwyn = 702,
		SoulOfGwyndolin = 708,
		SoulOfManus = 711,
		SoulOfOrnstein = 7004,
		SoulOfPriscilla = 707,
		SoulOfSif = 701,
		SoulOfSmough = 706,
		SoulOfTheMoonlightButterfly = 705,
		SoulOfQuelaag = 700
	}

	public enum SpearFlags
	{
		ChannelersTrident = 1004000,
		DemonsSpear = 1003000,
		DragonslayerSpear = 1051000,
		FourProngedPlow = 9016000,
		MoonlightButterflyHorn = 1052000,
		Partizan = 1002000,
		Pike = 1050000,
		SilverKnightSpear = 1006000,
		Spear = 1000000,
		WingedSpear = 1001000
	}

	public enum SwordFlags
	{
	}

	public enum TalismanFlags
	{
		CanvasTalisman = 1361000,
		DarkmoonTalisman = 1366000,
		IvoryTalisman = 1363000,
		SunlightTalisman = 1365000,
		Talisman = 1360000,
		ThorolundTalisman = 1362000,
		VelkasTalisman = 1367000
	}

	public enum WhipFlags
	{
		GuardianTail = 9019000,
		NotchedWhip = 1601000,
		Whip = 1600000
	}

	public static class ItemFlags
	{
		public static readonly int[] OrderedAmmunition =
		{
			-1,
			(int)AmmunitionFlags.DragonslayerArrow,
			(int)AmmunitionFlags.FeatherArrow,
			(int)AmmunitionFlags.FireArrow,
			(int)AmmunitionFlags.GoughsGreatArrow,
			(int)AmmunitionFlags.LargeArrow,
			(int)AmmunitionFlags.MoonlightArrow,
			(int)AmmunitionFlags.PoisonArrow,
			(int)AmmunitionFlags.StandardArrow,
			(int)AmmunitionFlags.WoodenArrow,
			-1,
			-1,
			(int)AmmunitionFlags.HeavyBolt,
			(int)AmmunitionFlags.LightningBolt,
			(int)AmmunitionFlags.SniperBolt,
			(int)AmmunitionFlags.StandardBolt,
			(int)AmmunitionFlags.WoodBold
		};

		public static readonly int[] OrderedAxes =
		{
			-1,
			(int)AxeFlags.BattleAxe,
			(int)AxeFlags.ButcherKnife,
			(int)AxeFlags.CrescentAxe,
			(int)AxeFlags.GargoyleTailAxe,
			(int)AxeFlags.GolemAxe,
			(int)AxeFlags.HandAxe,
			-1,
			-1,
			(int)AxeFlags.BlackKnightGreataxe,
			(int)AxeFlags.DemonsGreataxe,
			(int)AxeFlags.DragonKingGreataxe,
			(int)AxeFlags.Greataxe,
			(int)AxeFlags.StoneGreataxe
		};

		public static readonly int[] OrderedBonfireItems =
		{
			(int)BonfireItemFlags.ArmorSmithbox,
			(int)BonfireItemFlags.BottomlessBox,
			(int)BonfireItemFlags.Lordvessel,
			(int)BonfireItemFlags.Repairbox,
			(int)BonfireItemFlags.RiteOfKindling,
			(int)BonfireItemFlags.WeaponSmithbox
		};

		public static readonly int[] OrderedBows =
		{
			-1,
			(int)BowFlags.BlackBowOfPharis,
			(int)BowFlags.CompositeBow,
			(int)BowFlags.DarkmoonBow,
			(int)BowFlags.Longbow,
			(int)BowFlags.ShortBow,
			-1,
			-1,
			(int)BowFlags.DragonslayerGreatbow,
			(int)BowFlags.GoughsGreatbow
		};

		public static readonly int[] OrderedCatalysts =
		{
			(int)CatalystFlags.BeatricesCatalyst,
			(int)CatalystFlags.DemonsCatalyst,
			(int)CatalystFlags.IzalithCatalyst,
			(int)CatalystFlags.LogansCatalyst,
			(int)CatalystFlags.ManusCatalyst,
			(int)CatalystFlags.OolacileCatalyst,
			(int)CatalystFlags.OolacileIvoryCatalyst,
			(int)CatalystFlags.SorcerersCatalyst,
			(int)CatalystFlags.TinBanishmentCatalyst,
			(int)CatalystFlags.TinCrystallizationCatalyst,
			(int)CatalystFlags.TinDarkmoonCatalyst
		};

		public static readonly int[] OrderedConsumables =
		{
			-1,
			(int)ConsumableFlags.BloodredMossClump,
			(int)ConsumableFlags.BloomingPurpleMossClump,
			(int)ConsumableFlags.CharcoalPineResin,
			(int)ConsumableFlags.DivineBlessing,
			(int)ConsumableFlags.EggVermifuge,
			(int)ConsumableFlags.ElizabethsMushroom,
			(int)ConsumableFlags.EstusFlask,
			(int)ConsumableFlags.GreenBlossom,
			(int)ConsumableFlags.GoldPineResin,
			(int)ConsumableFlags.HomewardBone,
			(int)ConsumableFlags.Humanity,
			(int)ConsumableFlags.PrismStone,
			(int)ConsumableFlags.PurgingStone,
			(int)ConsumableFlags.PurpleMossClump,
			(int)ConsumableFlags.RepairPowder,
			(int)ConsumableFlags.RottenPineResin,
			(int)ConsumableFlags.TransientCurse,
			(int)ConsumableFlags.TwinHumanities
			-1,
			-1,
			(int)ConsumableFlags.HelloCarving,
			(int)ConsumableFlags.HelpMeCarving,
			(int)ConsumableFlags.ImSorryCarving,
			(int)ConsumableFlags.ThankYouCarving,
			(int)ConsumableFlags.VeryGoodCarving
		};

		public static readonly int[] OrderedCovenantItems =
		{
			(int)CovenantItemFlags.DriedFinger,
			(int)CovenantItemFlags.EyeOfDeath,
			(int)CovenantItemFlags.SunlightMedal,
			(int)CovenantItemFlags.SouvenirOfReprisal
		};

		public static readonly int[] OrderedCrossbows =
		{
			(int)CrossbowFlags.Avelyn,
			(int)CrossbowFlags.HeavyCrossbow,
			(int)CrossbowFlags.LightCrossbow,
			(int)CrossbowFlags.SniperCrossbow
		};

		public static readonly int[] OrderedDaggers =
		{
			(int)DaggerFlags.BanditsKnife,
			(int)DaggerFlags.Dagger,
			(int)DaggerFlags.DarkSilverTracer,
			(int)DaggerFlags.GhostBlade,
			(int)DaggerFlags.ParryingDagger,
			(int)DaggerFlags.PriscillasDagger
		};

		public static readonly int[] OrderedEmbers =
		{
			(int)EmberFlags.ChaosFlameEmber,
			(int)EmberFlags.CrystalEmber,
			(int)EmberFlags.DarkEmber,
			(int)EmberFlags.DivineEmber,
			(int)EmberFlags.EnchantedEmber,
			(int)EmberFlags.LargeDivineEmber,
			(int)EmberFlags.LargeEmber,
			(int)EmberFlags.LargeFlameEmber,
			(int)EmberFlags.LargeMagicEmber,
			(int)EmberFlags.VeryLargeEmber
		};

		public static readonly int[] OrderedFistItems =
		{
			(int)FistFlags.Caestus,
			(int)FistFlags.Claws,
			(int)FistFlags.DarkHand,
			(int)FistFlags.DragonBoneFist
		};

		public static readonly int[] OrderedFlames =
		{
			(int)FlameFlags.PyromancyFlame,
			(int)FlameFlags.PyromancyFlameAscended
		};

		public static readonly int[] OrderedGreatswords =
		{
			-1,
			(int)GreatswordFlags.GravelordSword,
			(int)GreatswordFlags.Murakumo,
			(int)GreatswordFlags.Server,
			-1,
			-1,
			(int)GreatswordFlags.AbyssGreatsword,
			(int)GreatswordFlags.BastardSword,
			(int)GreatswordFlags.BlackKnightSword,
			(int)GreatswordFlags.Claymore,
			(int)GreatswordFlags.CrystalGreatsword,
			(int)GreatswordFlags.Flamberge,
			(int)GreatswordFlags.GreatLordGreatsword,
			(int)GreatswordFlags.GreatswordOfArtorias,
			(int)GreatswordFlags.GreatswordOfArtoriasCursed,
			(int)GreatswordFlags.ManSerpentGreatsword,
			(int)GreatswordFlags.MoonlightGreatsword,
			(int)GreatswordFlags.ObsidianGreatsword,
			(int)GreatswordFlags.StoneGreatsword,
			-1,
			-1,
			(int)GreatswordFlags.BlackKnightGreatsword,
			(int)GreatswordFlags.DemonGreatMachete,
			(int)GreatswordFlags.DragonGreatsword,
			(int)GreatswordFlags.Greatsword,
			(int)GreatswordFlags.Zweihander
		};

		public static readonly int[] OrderedHalberds =
		{
			(int)HalberdFlags.BlackKnightHalberd,
			(int)HalberdFlags.GargoylesHalberd,
			(int)HalberdFlags.GiantsHalberd,
			(int)HalberdFlags.GreatScythe,
			(int)HalberdFlags.Halberd,
			(int)HalberdFlags.LifehuntScythe,
			(int)HalberdFlags.Lucerne,
			(int)HalberdFlags.Scythe,
			(int)HalberdFlags.TitaniteCatchPole
		};

		public static readonly int[] OrderedHammers =
		{
			-1,
			(int)HammerFlags.BlacksmithGiantHammer,
			(int)HammerFlags.BlacksmithHammer,
			(int)HammerFlags.Club,
			(int)HammerFlags.HammerOfVamos,
			(int)HammerFlags.Mace,
			(int)HammerFlags.MorningStar,
			(int)HammerFlags.Pickaxe,
			(int)HammerFlags.ReinforcedClub,
			(int)HammerFlags.Warpick,
			-1,
			-1,
			(int)HammerFlags.DemonsGreatHammer,
			(int)HammerFlags.DragonTooth,
			(int)HammerFlags.Grant,
			(int)HammerFlags.GreatClub,
			(int)HammerFlags.LargeClub,
			(int)HammerFlags.SmoughsHammer
		};

		public static readonly int[] OrderedKeys =
		{
			(int)KeyFlags.AnnexKey,
			(int)KeyFlags.ArchivePrisonExtraKey,
			(int)KeyFlags.ArchiveTowerCellKey,
			(int)KeyFlags.ArchiveTowerGiantCellKey,
			(int)KeyFlags.ArchiveTowerGiantDoorKey,
			(int)KeyFlags.BasementKey,
			(int)KeyFlags.BigPilgrimsKey,
			(int)KeyFlags.BlightttownKey,
			(int)KeyFlags.BrokenPendant,
			(int)KeyFlags.CageKey,
			(int)KeyFlags.CrestKey,
			(int)KeyFlags.CrestOfArtorias,
			(int)KeyFlags.DungeonCellKey,
			(int)KeyFlags.KeyToTheDepths,
			(int)KeyFlags.KeyToNewLondoRuins,
			(int)KeyFlags.KeyToTheSeal,
			(int)KeyFlags.MasterKey,
			(int)KeyFlags.MysteryKey,
			(int)KeyFlags.PeculiarDoll,
			(int)KeyFlags.ResidenceKey,
			(int)KeyFlags.SewerChamberKey,
			(int)KeyFlags.UndeadAsylumF2EastKey,
			(int)KeyFlags.UndeadAsylumF2WestKey,
			(int)KeyFlags.WatchtowerBasementKey
		};

		public static readonly int[] OrderedMiracles =
		{
			(int)MiracleFlags.BountifulSunlight,
			(int)MiracleFlags.DarkmoonBlade,
			(int)MiracleFlags.EmitForce,
			(int)MiracleFlags.Force,
			(int)MiracleFlags.GravelordGreatswordDance,
			(int)MiracleFlags.GravelordSwordDance,
			(int)MiracleFlags.GreatHeal,
			(int)MiracleFlags.GreatHealExcerpt,
			(int)MiracleFlags.GreatLightningSpear,
			(int)MiracleFlags.GreatMagicBarrier,
			(int)MiracleFlags.Heal,
			(int)MiracleFlags.Homeward,
			(int)MiracleFlags.KarmicJustice,
			(int)MiracleFlags.LightningSpear,
			(int)MiracleFlags.MagicBarrier,
			(int)MiracleFlags.Replenishment,
			(int)MiracleFlags.SeekGuidance,
			(int)MiracleFlags.SoothingSunlight,
			(int)MiracleFlags.SunlightBlade,
			(int)MiracleFlags.SunlightSpear,
			(int)MiracleFlags.TranquilWalkOfPeace,
			(int)MiracleFlags.VowOfSilence,
			(int)MiracleFlags.WrathOfTheGods
		};

		public static readonly int[] OrderedMultiplayerItems =
		{
			(int)MultiplayerItemFlags.BlackSeparationCrystal,
			(int)MultiplayerItemFlags.BlueEyeOrb,
			(int)MultiplayerItemFlags.BookOfTheGuilty,
			(int)MultiplayerItemFlags.CrackedRedEyeOrb,
			(int)MultiplayerItemFlags.DragonEye,
			(int)MultiplayerItemFlags.Indictment,
			(int)MultiplayerItemFlags.OrangeGuidanceSoapstone,
			(int)MultiplayerItemFlags.PurpleCowardsCrystal,
			(int)MultiplayerItemFlags.RedEyeOrb,
			(int)MultiplayerItemFlags.RedSignSoapstone,
			(int)MultiplayerItemFlags.ServantRoster,
			(int)MultiplayerItemFlags.WhiteSignSoapstone
		};

		public static readonly int[] OrderedOres =
		{
			(int)OreFlags.BlueTitaniteChunk,
			(int)OreFlags.BlueTitaniteSlab,
			(int)OreFlags.DemonTitanite,
			(int)OreFlags.DragonScale,
			(int)OreFlags.GreenTitaniteShard,
			(int)OreFlags.LargeTitaniteShard,
			(int)OreFlags.RedTitaniteChunk,
			(int)OreFlags.RedTitaniteSlab,
			(int)OreFlags.TitaniteChunk,
			(int)OreFlags.TitaniteShard,
			(int)OreFlags.TitaniteSlab,
			(int)OreFlags.TwinklingTitanite,
			(int)OreFlags.WhiteTitaniteChunk,
			(int)OreFlags.WhiteTitaniteSlab
		};

		public static readonly int[] OrderedProjectiles =
		{
			(int)ProjectileFlags.AlluringSkull,
			(int)ProjectileFlags.BlackFirebomb,
			(int)ProjectileFlags.DungPie,
			(int)ProjectileFlags.Firebomb,
			(int)ProjectileFlags.LloydsTalisman,
			(int)ProjectileFlags.PoisonThrowingKnife,
			(int)ProjectileFlags.ThrowingKnife
		};

		public static readonly int[] OrderedPyromancies =
		{
			(int)PyromancyFlags.AcidSurge,
			(int)PyromancyFlags.BlackFlame,
			(int)PyromancyFlags.ChaosFireWhip,
			(int)PyromancyFlags.ChaosStorm,
			(int)PyromancyFlags.Combustion,
			(int)PyromancyFlags.FireOrb,
			(int)PyromancyFlags.FireSurge,
			(int)PyromancyFlags.FireTempest,
			(int)PyromancyFlags.FireWhip,
			(int)PyromancyFlags.Fireball,
			(int)PyromancyFlags.Firestorm,
			(int)PyromancyFlags.FlashSweat,
			(int)PyromancyFlags.GreatChaosFireball,
			(int)PyromancyFlags.GreatCombustion,
			(int)PyromancyFlags.GreatFireball,
			(int)PyromancyFlags.IronFlesh,
			(int)PyromancyFlags.PoisonMist,
			(int)PyromancyFlags.PowerWithin,
			(int)PyromancyFlags.ToxicMist,
			(int)PyromancyFlags.UndeadRapport
		};

		public static readonly int[] OrderedRings =
		{
			(int)RingFlags.BellowingDragoncrestRing,
			(int)RingFlags.BloodbiteRing,
			(int)RingFlags.BlueTearstoneRing,
			(int)RingFlags.CalamityRing,
			(int)RingFlags.CatCovenantRing,
			(int)RingFlags.CloranthyRing,
			(int)RingFlags.CovenantofArtorias,
			(int)RingFlags.CovetousGoldSerpentRing,
			(int)RingFlags.CovetousSilverSerpentRing,
			(int)RingFlags.CursebiteRing,
			(int)RingFlags.DarkmoonBladeCovenantRing,
			(int)RingFlags.DarkmoonSeanceRing,
			(int)RingFlags.DarkWoodGrainRing,
			(int)RingFlags.DuskCrownRing,
			(int)RingFlags.EastWoodGrainRing,
			(int)RingFlags.FlameStoneplateRing,
			(int)RingFlags.HavelsRing,
			(int)RingFlags.HawkRing,
			(int)RingFlags.HornetRing,
			(int)RingFlags.LeoRing,
			(int)RingFlags.LingeringDragoncrestRing,
			(int)RingFlags.OldWitchsRing,
			(int)RingFlags.OrangeCharredRing,
			(int)RingFlags.PoisonbiteRing,
			(int)RingFlags.RedTearstoneRing,
			(int)RingFlags.RareRingofSacrifice,
			(int)RingFlags.RingOfFavorAndProtection,
			(int)RingFlags.RingOfFog,
			(int)RingFlags.RingOfSacrifice,
			(int)RingFlags.RingOfSteelProtection,
			(int)RingFlags.RingOfTheEvilEye,
			(int)RingFlags.RingOfTheSunPrincess,
			(int)RingFlags.RingOfTheSunsFirstborn,
			(int)RingFlags.RustedIronRing,
			(int)RingFlags.SlumberingDragoncrestRing,
			(int)RingFlags.SpeckledStoneplateRing,
			(int)RingFlags.SpellStoneplateRing,
			(int)RingFlags.TinyBeingsRing,
			(int)RingFlags.ThunderStoneplateRing,
			(int)RingFlags.WhiteSeanceRing,
			(int)RingFlags.WolfRing
		};

		public static readonly int[] OrderedSorceries =
		{
			(int)SorceryFlags.AuralDecoy,
			(int)SorceryFlags.CastLight,
			(int)SorceryFlags.Chameleon,
			(int)SorceryFlags.CrystalMagicWeapon,
			(int)SorceryFlags.CrystalSoulSpear,
			(int)SorceryFlags.DarkBead,
			(int)SorceryFlags.DarkFog,
			(int)SorceryFlags.DarkOrb,
			(int)SorceryFlags.FallControl,
			(int)SorceryFlags.GreatHeavySoulArrow,
			(int)SorceryFlags.GreatMagicWeapon,
			(int)SorceryFlags.GreatSoulArrow,
			(int)SorceryFlags.HeavySoulArrow,
			(int)SorceryFlags.HiddenBody,
			(int)SorceryFlags.HiddenWeapon,
			(int)SorceryFlags.HomingCrystalSoulmass,
			(int)SorceryFlags.HomingSoulmass,
			(int)SorceryFlags.Hush,
			(int)SorceryFlags.MagicShield,
			(int)SorceryFlags.MagicWeapon,
			(int)SorceryFlags.Pursuers,
			(int)SorceryFlags.Remedy,
			(int)SorceryFlags.Repair,
			(int)SorceryFlags.ResistCurse,
			(int)SorceryFlags.SoulArrow,
			(int)SorceryFlags.SoulSpear,
			(int)SorceryFlags.StrongMagicShield,
			(int)SorceryFlags.WhiteDragonBreath
		};

		public static readonly int[] OrderedSouls =
		{
		};

		public static readonly int[] OrderedSpears =
		{
			(int)SpearFlags.ChannelersTrident,
			(int)SpearFlags.DemonsSpear,
			(int)SpearFlags.DragonslayerSpear,
			(int)SpearFlags.FourProngedPlow,
			(int)SpearFlags.MoonlightButterflyHorn,
			(int)SpearFlags.Partizan,
			(int)SpearFlags.Pike,
			(int)SpearFlags.SilverKnightSpear,
			(int)SpearFlags.Spear,
			(int)SpearFlags.WingedSpear
		};

		public static readonly int[] OrderedTalismans =
		{
			(int)TalismanFlags.CanvasTalisman,
			(int)TalismanFlags.DarkmoonTalisman,
			(int)TalismanFlags.IvoryTalisman,
			(int)TalismanFlags.SunlightTalisman,
			(int)TalismanFlags.Talisman,
			(int)TalismanFlags.ThorolundTalisman,
			(int)TalismanFlags.VelkasTalisman
		};

		public static readonly int[] OrderedWhips =
		{
			(int)WhipFlags.GuardianTail,
			(int)WhipFlags.NotchedWhip,
			(int)WhipFlags.Whip
		};
	}
}
