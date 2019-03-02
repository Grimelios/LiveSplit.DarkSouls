using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveSplit.DarkSouls.Data;

namespace LiveSplit.DarkSouls.Memory
{
	// Important note about all item flags: item category (see the list below) is embedded into each flag as the flag's
	// highest digit. For example, the Bloodbite Ring (ID 109) has a category of 20 (as a hex byte), so its enum value
	// is 2109. This category is extracted and stored as needed to allow the autosplitter to distinguish between
	// multiple items with a shared ID.
	//
	// Also note that most item categories share values. All that really matters is that those categories are distinct
	// among items that share an ID.
	//
	// Armor = 0x10,
	// Consumable = 0x40,
	// Key = 0x40,
	// Material = 0x40,
	// MeleeWeapon = 0x00,
	// RangedWeapon = 0x00,
	// Ring = 0x20,
	// Shield = 0x00,
	// Spell = 0x40,
	// SpellTool = 0x00,
	// Usable = 0x40
	//

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
		AntiquatedDress = 331000,
		ArmorOfArtorias = 661000,
		ArmorOfTheGlorious = 111000,
		ArmorOfTheSun = 161000,
		ArmorOfThorns = 201000,
		BalderArmor = 511000,
		BlackClericRobe = 151000,
		BlackIronArmor = 71000,
		BlackKnightArmor = 321000,
		BlackLeatherArmor = 301000,
		BlackSorcererCloak = 641000,
		BrassArmor = 451000,
		BrigandArmor = 51000,
		CatarinaArmor = 11000,
		ChainArmor = 171000,
		ChestersLongCoat = 701000,
		ClericArmor = 181000,
		CrimsonRobe = 141000,
		CrystallineArmor = 131000,
		DarkArmor = 41000,
		DingyRobe = 401000,
		EasternArmor = 281000,
		EliteKnightArmor = 351000,
		EmbracedArmorOfFavor = 101000,
		GiantArmor = 531000,
		GoldHemmedBlackCloak = 461000,
		GolemArmor = 471000,
		GoughsArmor = 681000,
		GuardianArmor = 691000,
		HardLeatherArmor = 211000,
		HavelsArmor = 441000,
		HollowSoldierArmor = 481000,
		HollowThiefsLeatherArmor = 501000,
		HollowWarriorArmor = 521000,
		HolyRobe = 311000,
		KnightArmor = 391000,
		LeatherArmor = 241000,
		LordsBladeRobe = 671000,
		MaidenRobe = 411000,
		MoonlightRobe = 541000,
		OrnsteinsArmor = 271000,
		PaintingGuardianRobe = 251000,
		PaladinArmor = 21000,
		RobeOfTheChannelers = 91000,
		RobeOfTheGreatLord = 551000,
		SageRobe = 381000,
		ShadowGarb = 61000,
		SilverKnightArmor = 421000,
		SmoughsArmor = 81000,
		SorcererCloak = 221000,
		SteelArmor = 491000,
		StoneArmor = 121000,
		TatteredClothRobe = 231000,
		WandererCoat = 361000,
		WitchCloak = 341000,
		XanthousOvercoat = 291000
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
		AntiquatedGloves = 332000,
		BalderGauntlets = 512000,
		BlackIronGauntlets = 72000,
		BlackKnightGauntlets = 322000,
		BlackLeatherGloves = 302000,
		BlackManchette = 152000,
		BlackSorcererGauntlets = 642000,
		BraceletOfTheGreatLord = 552000,
		BrassGauntlets = 452000,
		BrigandGauntlets = 52000,
		CatarinaGauntlets = 12000,
		ChestersGloves = 702000,
		ClericGauntlets = 182000,
		CrimsonGloves = 142000,
		CrystallineGauntlets = 132000,
		DarkGauntlets = 42000,
		DingyGloves = 402000,
		EasternGauntlets = 282000,
		EliteKnightGauntlets = 352000,
		GauntletsOfArtorias = 662000,
		GauntletsOfFavor = 102000,
		GauntletsOfTheChanneler = 92000,
		GauntletsOfTheVanquisher = 112000,
		GauntletsOfThorns = 202000,
		GiantGauntlets = 532000,
		GoldHemmedBlackGloves = 462000,
		GolemGauntlets = 472000,
		GoughsGauntlets = 682000,
		GuardianGauntlets = 692000,
		HardLeatherGauntlets = 212000,
		HavelsGauntlets = 442000,
		IronBracelet = 162000,
		KnightGauntlets = 392000,
		LeatherGauntlets = 172000,
		LeatherGloves = 242000,
		LordsBladeGloves = 672000,
		MaidenGloves = 412000,
		MoonlightGloves = 542000,
		OrnsteinsGauntlets = 272000,
		PaintingGuardianGloves = 252000,
		PaladinGauntlets = 22000,
		ShadowGauntlets = 62000,
		SilverKnightGauntlets = 422000,
		SmoughsGauntlets = 82000,
		SorcererGauntlets = 222000,
		SteelGauntlets = 492000,
		StoneGauntlets = 122000,
		TatteredClothManchette = 232000,
		TravelingGlovesHoly = 312000,
		TravelingGlovesSage = 382000,
		WandererManchette = 362000,
		WitchGloves = 342000,
		XanthousGloves = 292000
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
		BalderHelm = 510000,
		BigHat = 380000,
		BlackIronHelm = 70000,
		BlackKnightHelm = 320000,
		BlackSorcererHat = 640000,
		BloatedHead = 710000,
		BloatedSorcererHead = 720000,
		BrassHelm = 450000,
		BrigandHood = 50000,
		CatarinaHelm = 10000,
		ChainHelm = 170000,
		ClericHelm = 180000,
		CrownOfDusk = 330000,
		CrownOfTheDarkSun = 540000,
		CrownOfTheGreatLord = 550000,
		CrystallineHelm = 130000,
		DarkMask = 40000,
		DingyHood = 400000,
		EasternHelm = 280000,
		EliteKnightHelm = 350000,
		FangBoarHelm = 620000,
		GargoyleHelm = 630000,
		GiantHelm = 530000,
		GoldHemmedBlackHood = 460000,
		GolemHelm = 470000,
		GoughsHelm = 680000,
		GuardianHelm = 690000,
		HavelsHelm = 440000,
		HelmOfArtorias = 660000,
		HelmOfFavor = 100000,
		HelmOfTheWise = 110000,
		HelmOfThorns = 200000,
		HollowSoldierHelm = 480000,
		HollowThiefsHood = 500000,
		HollowWarriorHelm = 520000,
		IronHelm = 160000,
		KnightHelm = 390000,
		MaidenHood = 410000,
		MaskOfTheChild = 610000,
		MaskOfTheFather = 590000,
		MaskOfTheMother = 600000,
		MaskOfTheSealer = 140000,
		MaskOfVelka = 150000,
		OrnsteinsHelm = 270000,
		PaintingGuardianHood = 250000,
		PaladinHelm = 20000,
		PharissHat = 240000,
		PorcelainMask = 670000,
		PriestsHat = 310000,
		RoyalHelm = 580000,
		Sack = 560000,
		ShadowMask = 60000,
		SilverKnightHelm = 420000,
		SixEyedHelmOfTheChannelers = 90000,
		SmoughsHelm = 80000,
		SnickeringTopHat = 700000,
		SorcererHat = 220000,
		StandardHelm = 210000,
		SteelHelm = 490000,
		StoneHelm = 120000,
		SunlightMaggot = 190000,
		SymbolOfAvarice = 570000,
		TatteredClothHood = 230000,
		ThiefMask = 300000,
		WandererHood = 360000,
		WitchHat = 340000,
		XanthousCrown = 290000
	}

	// Key flags don't actually need their category embedded (since keys are located in a separate array in memory),
	// but it's simpler to use them anyway.
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
		AnkletOfTheGreatLord = 553000,
		AntiquatedSkirt = 333000,
		BalderLeggings = 513000,
		BlackIronLeggings = 73000,
		BlackKnightLeggings = 323000,
		BlackLeatherBoots = 303000,
		BlackSorcererBoots = 643000,
		BlackTights = 153000,
		BloodStainedSkirt = 403000,
		BootsOfTheExplorer = 113000,
		BrassLeggings = 453000,
		BrigandTrousers = 53000,
		CatarinaLeggings = 13000,
		ChainLeggings = 173000,
		ChestersTrousers = 703000,
		ClericLeggings = 183000,
		CrimsonWaistcloth = 143000,
		CrystallineLeggings = 133000,
		DarkLeggings = 43000,
		EasternLeggings = 283000,
		EliteKnightLeggings = 353000,
		GiantLeggings = 533000,
		GoldHemmedBlackSkirt = 463000,
		GolemLeggings = 473000,
		GoughsLeggings = 683000,
		GuardianLeggings = 693000,
		HardLeatherBoots = 213000,
		HavelsLeggings = 443000,
		HeavyBoots = 233000,
		HollowSoldierWaistcloth = 483000,
		HollowThiefsTights = 503000,
		HollowWarriorWaistcloth = 523000,
		HolyTrousers = 313000,
		IronLeggings = 163000,
		KnightLeggings = 393000,
		LeatherBoots = 243000,
		LeggingsOfArtorias = 663000,
		LeggingsOfFavor = 103000,
		LeggingsOfThorns = 203000,
		LordsBladeWaistcloth = 673000,
		MaidenSkirt = 413000,
		MoonlightWaistcloth = 543000,
		OrnsteinsLeggings = 273000,
		PaintingGuardianWaistcloth = 253000,
		PaladinLeggings = 23000,
		ShadowLeggings = 63000,
		SilverKnightLeggings = 423000,
		SmoughsLeggings = 83000,
		SorcererBoots = 223000,
		SteelLeggings = 493000,
		StoneLeggings = 123000,
		TravelingBoots = 383000,
		WaistclothOfTheChannelers = 93000,
		WandererBoots = 363000,
		WitchSkirt = 343000,
		XanthousWaistcloth = 293000
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
		CrackedRedEyeOrb = 111,
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
		BellowingDragoncrestRing = 2115,
		BloodbiteRing = 2109,
		BlueTearstoneRing = 2147,
		CalamityRing = 2150,
		CatCovenantRing = 2103,
		CloranthyRing = 2104,
		CovenantofArtorias = 2138,
		CovetousGoldSerpentRing = 2121,
		CovetousSilverSerpentRing = 2122,
		CursebiteRing = 2113,
		DarkmoonBladeCovenantRing = 2102,
		DarkmoonSeanceRing = 2149,
		DarkWoodGrainRing = 2128,
		DuskCrownRing = 2116,
		EastWoodGrainRing = 2145,
		FlameStoneplateRing = 2105,
		HavelsRing = 2100,
		HawkRing = 2119,
		HornetRing = 2117,
		LeoRing = 2144,
		LingeringDragoncrestRing = 2141,
		OldWitchsRing = 2137,
		OrangeCharredRing = 2139,
		PoisonbiteRing = 2110,
		RedTearstoneRing = 2101,
		RareRingofSacrifice = 2127,
		RingOfFavorAndProtection = 2143,
		RingOfFog = 2124,
		RingOfSacrifice = 2126,
		RingOfSteelProtection = 2120,
		RingOfTheEvilEye = 2142,
		RingOfTheSunPrincess = 2130,
		RingOfTheSunsFirstborn = 2148,
		RustedIronRing = 2125,
		SlumberingDragoncrestRing = 2123,
		SpeckledStoneplateRing = 2108,
		SpellStoneplateRing = 2107,
		TinyBeingsRing = 2111,
		ThunderStoneplateRing = 2106,
		WhiteSeanceRing = 2114,
		WolfRing = 2146
	}

	public enum ShieldFlags
	{
		BalderShield = 1455000,
		BlackIronGreatshield = 9003000,
		BlackKnightShield = 1474000,
		Bloodshield = 9002000,
		BonewheelShield = 1506000,
		Buckler = 1405000,
		CaduceusKiteShield = 1477000,
		CaduceusRoundShield = 1410000,
		CleansingGreatshield = 9014000,
		CrackedRoundShield = 1406000,
		CrestShield = 1456000,
		CrystalRingShield = 1411000,
		CrystalShield = 1471000,
		DragonCrestShield = 1457000,
		EagleShield = 1500000,
		EastWestShield = 1400000,
		EffigyShield = 9000000,
		GargoylesShield = 1478000,
		GiantShield = 1502000,
		GrassCrestShield = 1453000,
		GreatshieldOfArtorias = 1507000,
		HavelsGreatshield = 1505000,
		HeaterShield = 1450000,
		HollowSoldierShield = 1454000,
		IronRoundShield = 1461000,
		KnightShield = 1451000,
		LargeLeatherShield = 1402000,
		LeatherShield = 1408000,
		PierceShield = 1475000,
		PlankShield = 1409000,
		RedAndWhiteRoundShield = 1476000,
		SmallLeatherShield = 1403000,
		Sanctus = 9001000,
		SilverKnightShield = 1473000,
		SpiderShield = 1462000,
		SpikedShield = 1470000,
		StoneGreatshield = 1503000,
		SunlightShield = 1472000,
		TargetShield = 1404000,
		TowerKiteShield = 1452000,
		TowerShield = 1501000,
		WarriorsRoundShield = 1460000,
		WoodenShield = 1401000
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
		BequeathedLordSoulShardFourKings = 2502,
		BequeathedLordSoulShardSeath = 2503,
		CoreOfAnIronGolem = 703,
		FireKeeperSoulAnastacia = 390,
		FireKeeperSoulDarkmoonKnightess = 391,
		FireKeeperSoulDaughterOfChaos = 392,
		FireKeeperSoulNewLondoRuins = 393,
		FireKeeperSoulBlighttown = 394,
		FireKeeperSoulDukesArchives = 395,
		FireKeeperSoulUndeadParish = 396,
		GuardianSoul = 709,
		LargeSoulOfABraveWarrior = 407,
		LargeSoulOfALostUndead = 401,
		LargeSoulOfANamelessSoldier = 403,
		LargeSoulOfAProudKnight = 405,
		LordSoulBedOfChaos = 2501,
		LordSoulNito = 2500,
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
		AstorasStraightSword = 209000,
		BalderSideSword = 204000,
		BarbedStraightSword = 207000,
		Broadsword = 202000,
		BrokenStraightSword = 203000,
		ChaosBlade = 503000,
		CrystalStraightSword = 205000,
		Darksword = 210000,
		DrakeSword = 211000,
		Estoc = 602000,
		Falchion = 401000,
		GoldTracer = 9010000,
		Iaito = 502000,
		JaggedGhostBlade = 403000,
		Longsword = 201000,
		MailBreaker = 600000,
		PaintingGurdianSword = 405000,
		QuelaagsFurysword = 406000,
		Rapier = 601000,
		RicardsRapier = 604000,
		Scimitar = 400000,
		Shortsword = 200000,
		Shotel = 402000,
		SilverKnightStraightSword = 208000,
		StraightSwordHilt = 212000,
		SunlightStraightSword = 206000,
		Uchigatana = 500000,
		VelkasRapier = 603000,
		WashingPole = 501000
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

		public static readonly int[] OrderedChestPieces =
		{
			-1,
			(int)ChestPieceFlags.ArmorOfArtorias,
			(int)ChestPieceFlags.ArmorOfTheGlorious,
			(int)ChestPieceFlags.ArmorOfTheSun,
			(int)ChestPieceFlags.ArmorOfThorns,
			(int)ChestPieceFlags.BalderArmor,
			(int)ChestPieceFlags.BlackIronArmor,
			(int)ChestPieceFlags.BlackKnightArmor,
			(int)ChestPieceFlags.BlackLeatherArmor,
			(int)ChestPieceFlags.BrassArmor,
			(int)ChestPieceFlags.BrigandArmor,
			(int)ChestPieceFlags.CatarinaArmor,
			(int)ChestPieceFlags.ChainArmor,
			(int)ChestPieceFlags.ClericArmor,
			(int)ChestPieceFlags.CrystallineArmor,
			(int)ChestPieceFlags.DarkArmor,
			(int)ChestPieceFlags.EasternArmor,
			(int)ChestPieceFlags.EliteKnightArmor,
			(int)ChestPieceFlags.EmbracedArmorOfFavor,
			(int)ChestPieceFlags.GiantArmor,
			(int)ChestPieceFlags.GolemArmor,
			(int)ChestPieceFlags.GoughsArmor,
			(int)ChestPieceFlags.GuardianArmor,
			(int)ChestPieceFlags.HardLeatherArmor,
			(int)ChestPieceFlags.HavelsArmor,
			(int)ChestPieceFlags.HollowSoldierArmor,
			(int)ChestPieceFlags.HollowThiefsLeatherArmor,
			(int)ChestPieceFlags.HollowWarriorArmor,
			(int)ChestPieceFlags.KnightArmor,
			(int)ChestPieceFlags.LeatherArmor,
			(int)ChestPieceFlags.OrnsteinsArmor,
			(int)ChestPieceFlags.PaladinArmor,
			(int)ChestPieceFlags.SilverKnightArmor,
			(int)ChestPieceFlags.SmoughsArmor,
			(int)ChestPieceFlags.SteelArmor,
			(int)ChestPieceFlags.StoneArmor,
			-1,
			-1,
			(int)ChestPieceFlags.BlackSorcererCloak,
			(int)ChestPieceFlags.GoldHemmedBlackCloak,
			(int)ChestPieceFlags.SorcererCloak,
			(int)ChestPieceFlags.WitchCloak,
			-1,
			-1,
			(int)ChestPieceFlags.ChestersLongCoat,
			(int)ChestPieceFlags.WandererCoat,
			(int)ChestPieceFlags.XanthousOvercoat,
			-1,
			-1,
			(int)ChestPieceFlags.BlackClericRobe,
			(int)ChestPieceFlags.CrimsonRobe,
			(int)ChestPieceFlags.DingyRobe,
			(int)ChestPieceFlags.HolyRobe,
			(int)ChestPieceFlags.LordsBladeRobe,
			(int)ChestPieceFlags.MaidenRobe,
			(int)ChestPieceFlags.MoonlightRobe,
			(int)ChestPieceFlags.PaintingGuardianRobe,
			(int)ChestPieceFlags.RobeOfTheChannelers,
			(int)ChestPieceFlags.RobeOfTheGreatLord,
			(int)ChestPieceFlags.SageRobe,
			(int)ChestPieceFlags.TatteredClothRobe,
			-1,
			-1,
			(int)ChestPieceFlags.AntiquatedDress,
			(int)ChestPieceFlags.ShadowGarb
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

		public static readonly int[] OrderedGauntlets =
		{
			-1,
			(int)GauntletFlags.BraceletOfTheGreatLord,
			(int)GauntletFlags.IronBracelet,
			-1,
			-1,
			(int)GauntletFlags.BalderGauntlets,
			(int)GauntletFlags.BlackIronGauntlets,
			(int)GauntletFlags.BlackKnightGauntlets,
			(int)GauntletFlags.BlackSorcererGauntlets,
			(int)GauntletFlags.BrassGauntlets,
			(int)GauntletFlags.BrigandGauntlets,
			(int)GauntletFlags.CatarinaGauntlets,
			(int)GauntletFlags.ClericGauntlets,
			(int)GauntletFlags.CrystallineGauntlets,
			(int)GauntletFlags.DarkGauntlets,
			(int)GauntletFlags.EasternGauntlets,
			(int)GauntletFlags.EliteKnightGauntlets,
			(int)GauntletFlags.GauntletsOfArtorias,
			(int)GauntletFlags.GauntletsOfFavor,
			(int)GauntletFlags.GauntletsOfTheChanneler,
			(int)GauntletFlags.GauntletsOfTheVanquisher,
			(int)GauntletFlags.GauntletsOfThorns,
			(int)GauntletFlags.GiantGauntlets,
			(int)GauntletFlags.GolemGauntlets,
			(int)GauntletFlags.GoughsGauntlets,
			(int)GauntletFlags.GuardianGauntlets,
			(int)GauntletFlags.HardLeatherGauntlets,
			(int)GauntletFlags.HavelsGauntlets,
			(int)GauntletFlags.KnightGauntlets,
			(int)GauntletFlags.LeatherGauntlets,
			(int)GauntletFlags.OrnsteinsGauntlets,
			(int)GauntletFlags.PaladinGauntlets,
			(int)GauntletFlags.ShadowGauntlets,
			(int)GauntletFlags.SilverKnightGauntlets,
			(int)GauntletFlags.SmoughsGauntlets,
			(int)GauntletFlags.SorcererGauntlets,
			(int)GauntletFlags.SteelGauntlets,
			(int)GauntletFlags.StoneGauntlets,
			-1,
			-1,
			(int)GauntletFlags.AntiquatedGloves,
			(int)GauntletFlags.BlackLeatherGloves,
			(int)GauntletFlags.ChestersGloves,
			(int)GauntletFlags.CrimsonGloves,
			(int)GauntletFlags.DingyGloves,
			(int)GauntletFlags.GoldHemmedBlackGloves,
			(int)GauntletFlags.LeatherGloves,
			(int)GauntletFlags.LordsBladeGloves,
			(int)GauntletFlags.MaidenGloves,
			(int)GauntletFlags.MoonlightGloves,
			(int)GauntletFlags.PaintingGuardianGloves,
			(int)GauntletFlags.TravelingGlovesHoly,
			(int)GauntletFlags.TravelingGlovesSage,
			(int)GauntletFlags.WitchGloves,
			(int)GauntletFlags.XanthousGloves,
			-1,
			-1,
			(int)GauntletFlags.BlackManchette,
			(int)GauntletFlags.TatteredClothManchette,
			(int)GauntletFlags.WandererManchette
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

		public static readonly int[] OrderedHelmets =
		{
			-1,
			(int)HelmetFlags.CrownOfDusk,
			(int)HelmetFlags.CrownOfTheDarkSun,
			(int)HelmetFlags.CrownOfTheGreatLord,
			(int)HelmetFlags.XanthousCrown,
			-1,
			-1,
			(int)HelmetFlags.BigHat,
			(int)HelmetFlags.BlackSorcererHat,
			(int)HelmetFlags.PharissHat,
			(int)HelmetFlags.PriestsHat,
			(int)HelmetFlags.SnickeringTopHat,
			(int)HelmetFlags.SorcererHat,
			(int)HelmetFlags.WitchHat,
			-1,
			-1,
			(int)HelmetFlags.BalderHelm,
			(int)HelmetFlags.BlackIronHelm,
			(int)HelmetFlags.BlackKnightHelm,
			(int)HelmetFlags.BrassHelm,
			(int)HelmetFlags.CatarinaHelm,
			(int)HelmetFlags.ChainHelm,
			(int)HelmetFlags.ClericHelm,
			(int)HelmetFlags.CrystallineHelm,
			(int)HelmetFlags.EasternHelm,
			(int)HelmetFlags.EliteKnightHelm,
			(int)HelmetFlags.FangBoarHelm,
			(int)HelmetFlags.GargoyleHelm,
			(int)HelmetFlags.GiantHelm,
			(int)HelmetFlags.GolemHelm,
			(int)HelmetFlags.GoughsHelm,
			(int)HelmetFlags.GuardianHelm,
			(int)HelmetFlags.HavelsHelm,
			(int)HelmetFlags.HelmOfArtorias,
			(int)HelmetFlags.HelmOfFavor,
			(int)HelmetFlags.HelmOfTheWise,
			(int)HelmetFlags.HelmOfThorns,
			(int)HelmetFlags.HollowSoldierHelm,
			(int)HelmetFlags.HollowWarriorHelm,
			(int)HelmetFlags.IronHelm,
			(int)HelmetFlags.KnightHelm,
			(int)HelmetFlags.OrnsteinsHelm,
			(int)HelmetFlags.PaladinHelm,
			(int)HelmetFlags.RoyalHelm,
			(int)HelmetFlags.SilverKnightHelm,
			(int)HelmetFlags.SixEyedHelmOfTheChannelers,
			(int)HelmetFlags.SmoughsHelm,
			(int)HelmetFlags.StandardHelm,
			(int)HelmetFlags.SteelHelm,
			(int)HelmetFlags.StoneHelm,
			-1,
			-1,
			(int)HelmetFlags.BrigandHood,
			(int)HelmetFlags.DingyHood,
			(int)HelmetFlags.GoldHemmedBlackHood,
			(int)HelmetFlags.HollowThiefsHood,
			(int)HelmetFlags.MaidenHood,
			(int)HelmetFlags.PaintingGuardianHood,
			(int)HelmetFlags.TatteredClothHood,
			(int)HelmetFlags.WandererHood,
			-1,
			-1,
			(int)HelmetFlags.DarkMask,
			(int)HelmetFlags.MaskOfTheChild,
			(int)HelmetFlags.MaskOfTheFather,
			(int)HelmetFlags.MaskOfTheMother,
			(int)HelmetFlags.MaskOfTheSealer,
			(int)HelmetFlags.MaskOfVelka,
			(int)HelmetFlags.PorcelainMask,
			(int)HelmetFlags.ShadowMask,
			(int)HelmetFlags.ThiefMask,
			-1,
			-1,
			(int)HelmetFlags.BloatedHead,
			(int)HelmetFlags.BloatedSorcererHead,
			(int)HelmetFlags.Sack,
			(int)HelmetFlags.SunlightMaggot,
			(int)HelmetFlags.SymbolOfAvarice
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

		public static readonly int[] OrderedLeggings =
		{
			-1,
			(int)LeggingFlags.AnkletOfTheGreatLord,
			-1,
			-1,
			(int)LeggingFlags.BlackLeatherBoots,
			(int)LeggingFlags.BlackSorcererBoots,
			(int)LeggingFlags.BootsOfTheExplorer,
			(int)LeggingFlags.HardLeatherBoots,
			(int)LeggingFlags.HeavyBoots,
			(int)LeggingFlags.LeatherBoots,
			(int)LeggingFlags.SorcererBoots,
			(int)LeggingFlags.TravelingBoots,
			(int)LeggingFlags.WandererBoots,
			-1,
			-1,
			(int)LeggingFlags.BalderLeggings,
			(int)LeggingFlags.BlackIronLeggings,
			(int)LeggingFlags.BlackKnightLeggings,
			(int)LeggingFlags.BrassLeggings,
			(int)LeggingFlags.CatarinaLeggings,
			(int)LeggingFlags.ChainLeggings,
			(int)LeggingFlags.ClericLeggings,
			(int)LeggingFlags.CrystallineLeggings,
			(int)LeggingFlags.DarkLeggings,
			(int)LeggingFlags.EasternLeggings,
			(int)LeggingFlags.EliteKnightLeggings,
			(int)LeggingFlags.GiantLeggings,
			(int)LeggingFlags.GolemLeggings,
			(int)LeggingFlags.GoughsLeggings,
			(int)LeggingFlags.GuardianLeggings,
			(int)LeggingFlags.HavelsLeggings,
			(int)LeggingFlags.IronLeggings,
			(int)LeggingFlags.KnightLeggings,
			(int)LeggingFlags.LeggingsOfArtorias,
			(int)LeggingFlags.LeggingsOfFavor,
			(int)LeggingFlags.LeggingsOfThorns,
			(int)LeggingFlags.OrnsteinsLeggings,
			(int)LeggingFlags.PaladinLeggings,
			(int)LeggingFlags.ShadowLeggings,
			(int)LeggingFlags.SilverKnightLeggings,
			(int)LeggingFlags.SmoughsLeggings,
			(int)LeggingFlags.SteelLeggings,
			(int)LeggingFlags.StoneLeggings,
			-1,
			-1,
			(int)LeggingFlags.AntiquatedSkirt,
			(int)LeggingFlags.BloodStainedSkirt,
			(int)LeggingFlags.GoldHemmedBlackSkirt,
			(int)LeggingFlags.MaidenSkirt,
			(int)LeggingFlags.WitchSkirt,
			-1,
			-1,
			(int)LeggingFlags.BlackTights,
			(int)LeggingFlags.HollowThiefsTights,
			-1,
			-1,
			(int)LeggingFlags.BrigandTrousers,
			(int)LeggingFlags.ChestersTrousers,
			(int)LeggingFlags.HolyTrousers,
			-1,
			-1,
			(int)LeggingFlags.CrimsonWaistcloth,
			(int)LeggingFlags.HollowSoldierWaistcloth,
			(int)LeggingFlags.HollowWarriorWaistcloth,
			(int)LeggingFlags.LordsBladeWaistcloth,
			(int)LeggingFlags.MoonlightWaistcloth,
			(int)LeggingFlags.PaintingGuardianWaistcloth,
			(int)LeggingFlags.WaistclothOfTheChannelers,
			(int)LeggingFlags.XanthousWaistcloth
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

		public static readonly int[] OrderedShields =
		{
			-1,
			(int)ShieldFlags.BlackIronGreatshield,
			(int)ShieldFlags.BonewheelShield,
			(int)ShieldFlags.CleansingGreatshield,
			(int)ShieldFlags.EagleShield,
			(int)ShieldFlags.GiantShield,
			(int)ShieldFlags.GreatshieldOfArtorias,
			(int)ShieldFlags.HavelsGreatshield,
			(int)ShieldFlags.StoneGreatshield,
			(int)ShieldFlags.TowerShield,
			-1,
			-1,
			(int)ShieldFlags.Buckler,
			(int)ShieldFlags.CaduceusRoundShield,
			(int)ShieldFlags.CrackedRoundShield,
			(int)ShieldFlags.EffigyShield,
			(int)ShieldFlags.LeatherShield,
			(int)ShieldFlags.PlankShield,
			(int)ShieldFlags.RedAndWhiteRoundShield,
			(int)ShieldFlags.SmallLeatherShield,
			(int)ShieldFlags.TargetShield,
			(int)ShieldFlags.WarriorsRoundShield,
			-1,
			-1,
			(int)ShieldFlags.BalderShield,
			(int)ShieldFlags.BlackKnightShield,
			(int)ShieldFlags.Bloodshield,
			(int)ShieldFlags.CaduceusKiteShield,
			(int)ShieldFlags.CrestShield,
			(int)ShieldFlags.DragonCrestShield,
			(int)ShieldFlags.EastWestShield,
			(int)ShieldFlags.GargoylesShield,
			(int)ShieldFlags.GrassCrestShield,
			(int)ShieldFlags.HeaterShield,
			(int)ShieldFlags.HollowSoldierShield,
			(int)ShieldFlags.IronRoundShield,
			(int)ShieldFlags.KnightShield,
			(int)ShieldFlags.LargeLeatherShield,
			(int)ShieldFlags.Sanctus,
			(int)ShieldFlags.SilverKnightShield,
			(int)ShieldFlags.SpiderShield,
			(int)ShieldFlags.SunlightShield,
			(int)ShieldFlags.TowerKiteShield,
			(int)ShieldFlags.WoodenShield,
			-1,
			-1,
			(int)ShieldFlags.CrystalShield,
			(int)ShieldFlags.CrystalRingShield,
			(int)ShieldFlags.PierceShield,
			(int)ShieldFlags.SpikedShield,
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
			-1,
			(int)SoulFlags.CoreOfAnIronGolem,
			(int)SoulFlags.GuardianSoul,
			(int)SoulFlags.SoulOfArtorias,
			(int)SoulFlags.SoulOfGwyn,
			(int)SoulFlags.SoulOfGwyndolin,
			(int)SoulFlags.SoulOfManus,
			(int)SoulFlags.SoulOfTheMoonlightButterfly,
			(int)SoulFlags.SoulOfOrnstein,
			(int)SoulFlags.SoulOfPriscilla,
			(int)SoulFlags.SoulOfSif,
			(int)SoulFlags.SoulOfSmough,
			(int)SoulFlags.SoulOfQuelaag,
			-1,
			-1,
			(int)SoulFlags.FireKeeperSoulAnastacia,
			(int)SoulFlags.FireKeeperSoulBlighttown,
			(int)SoulFlags.FireKeeperSoulDarkmoonKnightess,
			(int)SoulFlags.FireKeeperSoulDaughterOfChaos,
			(int)SoulFlags.FireKeeperSoulDukesArchives,
			(int)SoulFlags.FireKeeperSoulNewLondoRuins,
			(int)SoulFlags.FireKeeperSoulUndeadParish,
			-1,
			-1,
			(int)SoulFlags.BequeathedLordSoulShardFourKings,
			(int)SoulFlags.BequeathedLordSoulShardSeath,
			(int)SoulFlags.LordSoulBedOfChaos,
			(int)SoulFlags.LordSoulNito,
			-1,
			-1,
			(int)SoulFlags.LargeSoulOfABraveWarrior,
			(int)SoulFlags.LargeSoulOfALostUndead,
			(int)SoulFlags.LargeSoulOfANamelessSoldier,
			(int)SoulFlags.LargeSoulOfAProudKnight,
			(int)SoulFlags.SoulOfABraveWarrior,
			(int)SoulFlags.SoulOfAGreatHero,
			(int)SoulFlags.SoulOfAHero,
			(int)SoulFlags.SoulOfALostUndead,
			(int)SoulFlags.SoulOfANamelessSoldier,
			(int)SoulFlags.SoulOfAProudKnight
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

		public static readonly int[] OrderedSwords =
		{
			-1,
			(int)SwordFlags.Falchion,
			(int)SwordFlags.GoldTracer,
			(int)SwordFlags.JaggedGhostBlade,
			(int)SwordFlags.PaintingGurdianSword,
			(int)SwordFlags.QuelaagsFurysword,
			(int)SwordFlags.Scimitar,
			(int)SwordFlags.Shotel,
			-1,
			-1,
			(int)SwordFlags.ChaosBlade,
			(int)SwordFlags.Iaito,
			(int)SwordFlags.Uchigatana,
			(int)SwordFlags.WashingPole,
			-1,
			-1,
			(int)SwordFlags.Estoc,
			(int)SwordFlags.MailBreaker,
			(int)SwordFlags.Rapier,
			(int)SwordFlags.RicardsRapier,
			(int)SwordFlags.VelkasRapier,
			-1,
			-1,
			(int)SwordFlags.AstorasStraightSword,
			(int)SwordFlags.BalderSideSword,
			(int)SwordFlags.BarbedStraightSword,
			(int)SwordFlags.Broadsword,
			(int)SwordFlags.BrokenStraightSword,
			(int)SwordFlags.CrystalStraightSword,
			(int)SwordFlags.Darksword,
			(int)SwordFlags.DrakeSword,
			(int)SwordFlags.Longsword,
			(int)SwordFlags.Shortsword,
			(int)SwordFlags.SilverKnightStraightSword,
			(int)SwordFlags.StraightSwordHilt,
			(int)SwordFlags.SunlightStraightSword
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

		public static readonly int[][] MasterList =
		{
			null,
			OrderedChestPieces,
			OrderedGauntlets,
			OrderedHelmets,
			OrderedLeggings,
			null,
			null,
			OrderedCatalysts,
			OrderedFlames,
			OrderedMiracles,
			OrderedPyromancies,
			OrderedSorceries,
			OrderedTalismans,
			null,
			null,
			OrderedBonfireItems,
			OrderedCovenantItems,
			OrderedKeys,
			OrderedMultiplayerItems,
			null,
			OrderedSouls,
			null,
			null,
			OrderedAmmunition,
			OrderedConsumables,
			OrderedProjectiles,
			OrderedRings,
			OrderedShields,
			null,
			null,
			OrderedEmbers,
			OrderedOres,
			null,
			null,
			OrderedAxes,
			OrderedBows,
			OrderedCrossbows,
			OrderedDaggers,
			OrderedFistItems,
			OrderedGreatswords,
			OrderedHalberds,
			OrderedHammers,
			OrderedSpears,
			OrderedSwords,
			OrderedWhips
		};
	}
}
