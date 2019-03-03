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
	// is 2109 (only the two matters). This category is extracted and stored as needed to allow the autosplitter to
	// distinguish between multiple items with a shared ID.
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
		DragonslayerArrow = 92007000,
		FeatherArrow = 92002000,
		FireArrow = 92003000,
		GoughsGreatArrow = 92008000,
		HeavyBolt = 92101000,
		LargeArrow = 92001000,
		LightningBolt = 92104000,
		MoonlightArrow = 92005000,
		PoisonArrow = 92004000,
		SniperBolt = 92102000,
		StandardArrow = 92000000,
		StandardBolt = 92100000,
		WoodBold = 92103000,
		WoodenArrow = 92006000
	}

	public enum AxeFlags
	{
		BattleAxe = 9701000,
		BlackKnightGreataxe = 9753000,
		ButcherKnife = 9703000,
		CrescentAxe = 9702000,
		DemonsGreataxe = 9751000,
		DragonKingGreataxe = 9752000,
		GargoyleTailAxe = 9705000,
		GolemAxe = 9704000,
		Greataxe = 9750000,
		HandAxe = 9700000,
		StoneGreataxe = 99015000
	}

	public enum BonfireItemFlags
	{
		ArmorSmithbox = 42601,
		BottomlessBox = 42608,
		Lordvessel = 42510,
		Repairbox = 42602,
		RiteOfKindling = 42607,
		WeaponSmithbox = 42600
	}

	public enum BowFlags
	{
		BlackBowOfPharis = 91202000,
		CompositeBow = 91204000,
		DarkmoonBow = 91205000,
		DragonslayerGreatbow = 91203000,
		GoughsGreatbow = 99021000,
		Longbow = 91201000,
		ShortBow = 91200000
	}

	public enum CatalystFlags
	{
		BeatricesCatalyst = 91301000,
		DemonsCatalyst = 91307000,
		IzalithCatalyst = 91308000,
		LogansCatalyst = 91303000,
		ManusCatalyst = 99017000,
		OolacileCatalyst = 99018000,
		OolacileIvoryCatalyst = 91305000,
		SorcerersCatalyst = 91300000,
		TinBanishmentCatalyst = 91302000,
		TinCrystallizationCatalyst = 91306000,
		TinDarkmoonCatalyst = 91304000
	}

	public enum ChestPieceFlags
	{
		AntiquatedDress = 1331000,
		ArmorOfArtorias = 1661000,
		ArmorOfTheGlorious = 1111000,
		ArmorOfTheSun = 1161000,
		ArmorOfThorns = 1201000,
		BalderArmor = 1511000,
		BlackClericRobe = 1151000,
		BlackIronArmor = 171000,
		BlackKnightArmor = 1321000,
		BlackLeatherArmor = 1301000,
		BlackSorcererCloak = 1641000,
		BrassArmor = 1451000,
		BrigandArmor = 151000,
		CatarinaArmor = 111000,
		ChainArmor = 1171000,
		ChestersLongCoat = 1701000,
		ClericArmor = 1181000,
		CrimsonRobe = 1141000,
		CrystallineArmor = 1131000,
		DarkArmor = 141000,
		DingyRobe = 1401000,
		EasternArmor = 1281000,
		EliteKnightArmor = 1351000,
		EmbracedArmorOfFavor = 1101000,
		GiantArmor = 1531000,
		GoldHemmedBlackCloak = 1461000,
		GolemArmor = 1471000,
		GoughsArmor = 1681000,
		GuardianArmor = 1691000,
		HardLeatherArmor = 1211000,
		HavelsArmor = 1441000,
		HollowSoldierArmor = 1481000,
		HollowThiefsLeatherArmor = 1501000,
		HollowWarriorArmor = 1521000,
		HolyRobe = 1311000,
		KnightArmor = 1391000,
		LeatherArmor = 1241000,
		LordsBladeRobe = 1671000,
		MaidenRobe = 1411000,
		MoonlightRobe = 1541000,
		OrnsteinsArmor = 1271000,
		PaintingGuardianRobe = 1251000,
		PaladinArmor = 121000,
		RobeOfTheChannelers = 191000,
		RobeOfTheGreatLord = 1551000,
		SageRobe = 1381000,
		ShadowGarb = 161000,
		SilverKnightArmor = 1421000,
		SmoughsArmor = 181000,
		SorcererCloak = 1221000,
		SteelArmor = 1491000,
		StoneArmor = 1121000,
		TatteredClothRobe = 1231000,
		WandererCoat = 1361000,
		WitchCloak = 1341000,
		XanthousOvercoat = 1291000
	}

	public enum ConsumableFlags
	{
		BloodredMossClump = 4270,
		BloomingPurpleMossClump = 4272,
		CharcoalPineResin = 4310,
		DivineBlessing = 4240,
		EggVermifuge = 4275,
		ElizabethsMushroom = 4230,
		
		// Note that item ID 200 is the base (un-upgraded) estus flask while empty. There's additional logic in the
		// main component class to account for all 14 possible estus flask IDs.
		EstusFlask = 4200,
		GreenBlossom = 4260,
		GoldPineResin = 4311,
		HomewardBone = 4330,
		Humanity = 4500,
		PrismStone = 4370,
		PurgingStone = 4274,
		PurpleMossClump = 4271,
		RepairPowder = 4280,
		RottenPineResin = 4313,
		TransientCurse = 4312,
		TwinHumanities = 4501
	}

	public enum CovenantItemFlags
	{
		DriedFinger = 4385,
		EyeOfDeath = 4109,
		SunlightMedal = 4375,
		SouvenirOfReprisal = 4374
	}

	public enum CrossbowFlags
	{
		Avelyn = 91252000,
		HeavyCrossbow = 91251000,
		LightCrossbow = 91250000,
		SniperCrossbow = 91253000
	}

	public enum DaggerFlags
	{
		BanditsKnife = 9103000,
		Dagger = 9100000,
		DarkSilverTracer = 99011000,
		GhostBlade = 9102000,
		ParryingDagger = 9101000,
		PriscillasDagger = 9104000
	}

	public enum EmberFlags
	{
		ChaosFlameEmber = 4813,
		CrystalEmber = 4802,
		DarkEmber = 4810,
		DivineEmber = 4808,
		EnchantedEmber = 4807,
		LargeDivineEmber = 4809,
		LargeEmber = 4800,
		LargeFlameEmber = 4812,
		LargeMagicEmber = 4806,
		VeryLargeEmber = 4801
	}

	public enum FistFlags
	{
		Caestus = 9901000,
		Claws = 9902000,
		DarkHand = 9904000,
		DragonBoneFist = 9903000
	}

	public enum FlameFlags
	{
		PyromancyFlame = 91330000,
		PyromancyFlameAscended = 91332000
	}

	public enum GauntletFlags
	{
		AntiquatedGloves = 1332000,
		BalderGauntlets = 1512000,
		BlackIronGauntlets = 172000,
		BlackKnightGauntlets = 1322000,
		BlackLeatherGloves = 1302000,
		BlackManchette = 1152000,
		BlackSorcererGauntlets = 1642000,
		BraceletOfTheGreatLord = 1552000,
		BrassGauntlets = 1452000,
		BrigandGauntlets = 152000,
		CatarinaGauntlets = 112000,
		ChestersGloves = 1702000,
		ClericGauntlets = 1182000,
		CrimsonGloves = 1142000,
		CrystallineGauntlets = 1132000,
		DarkGauntlets = 142000,
		DingyGloves = 1402000,
		EasternGauntlets = 1282000,
		EliteKnightGauntlets = 1352000,
		GauntletsOfArtorias = 1662000,
		GauntletsOfFavor = 1102000,
		GauntletsOfTheChanneler = 192000,
		GauntletsOfTheVanquisher = 1112000,
		GauntletsOfThorns = 1202000,
		GiantGauntlets = 1532000,
		GoldHemmedBlackGloves = 1462000,
		GolemGauntlets = 1472000,
		GoughsGauntlets = 1682000,
		GuardianGauntlets = 1692000,
		HardLeatherGauntlets = 1212000,
		HavelsGauntlets = 1442000,
		IronBracelet = 1162000,
		KnightGauntlets = 1392000,
		LeatherGauntlets = 1172000,
		LeatherGloves = 1242000,
		LordsBladeGloves = 1672000,
		MaidenGloves = 1412000,
		MoonlightGloves = 1542000,
		OrnsteinsGauntlets = 1272000,
		PaintingGuardianGloves = 1252000,
		PaladinGauntlets = 122000,
		ShadowGauntlets = 162000,
		SilverKnightGauntlets = 1422000,
		SmoughsGauntlets = 182000,
		SorcererGauntlets = 1222000,
		SteelGauntlets = 1492000,
		StoneGauntlets = 1122000,
		TatteredClothManchette = 1232000,
		TravelingGlovesHoly = 1312000,
		TravelingGlovesSage = 1382000,
		WandererManchette = 1362000,
		WitchGloves = 1342000,
		XanthousGloves = 1292000
	}

	public enum GreatswordFlags
	{
		AbyssGreatsword = 99012000,
		BastardSword = 9300000,
		BlackKnightGreatsword = 9355000,
		BlackKnightSword = 9310000,
		Claymore = 9301000,
		CrystalGreatsword = 9304000,
		DemonGreatMachete = 9352000,
		DragonGreatsword = 9354000,
		Flamberge = 9303000,
		GravelordSword = 9453000,
		GreatLordGreatsword = 9314000,
		Greatsword = 9351000,
		GreatswordOfArtorias = 9307000,
		GreatswordOfArtoriasCursed = 9311000,
		ManSerpentGreatsword = 9302000,
		MoonlightGreatsword = 9309000,
		Murakumo = 9451000,
		ObsidianGreatsword = 99020000,
		StoneGreatsword = 9306000,
		Server = 9450000,
		Zweihander = 9350000
	}

	public enum HalberdFlags
	{
		BlackKnightHalberd = 91105000,
		GargoylesHalberd = 91103000,
		GiantsHalberd = 91101000,
		GreatScythe = 91150000,
		Halberd = 91100000,
		LifehuntScythe = 91151000,
		Lucerne = 91106000,
		Scythe = 91107000,
		TitaniteCatchPole = 91102000
	}

	public enum HammerFlags
	{
		BlacksmithGiantHammer = 9811000,
		BlacksmithHammer = 9810000,
		Club = 9800000,
		DemonsGreatHammer = 9852000,
		DragonTooth = 9854000,
		Grant = 9851000,
		GreatClub = 9850000,
		HammerOfVamos = 9812000,
		LargeClub = 9855000,
		Mace = 9801000,
		MorningStar = 9802000,
		Pickaxe = 9804000,
		ReinforcedClub = 9809000,
		SmoughsHammer = 9856000,
		Warpick = 9803000
	}

	public enum HelmetFlags
	{
		BalderHelm = 1510000,
		BigHat = 1380000,
		BlackIronHelm = 170000,
		BlackKnightHelm = 1320000,
		BlackSorcererHat = 1640000,
		BloatedHead = 1710000,
		BloatedSorcererHead = 1720000,
		BrassHelm = 1450000,
		BrigandHood = 150000,
		CatarinaHelm = 110000,
		ChainHelm = 1170000,
		ClericHelm = 1180000,
		CrownOfDusk = 1330000,
		CrownOfTheDarkSun = 1540000,
		CrownOfTheGreatLord = 1550000,
		CrystallineHelm = 1130000,
		DarkMask = 140000,
		DingyHood = 1400000,
		EasternHelm = 1280000,
		EliteKnightHelm = 1350000,
		FangBoarHelm = 1620000,
		GargoyleHelm = 1630000,
		GiantHelm = 1530000,
		GoldHemmedBlackHood = 1460000,
		GolemHelm = 1470000,
		GoughsHelm = 1680000,
		GuardianHelm = 1690000,
		HavelsHelm = 1440000,
		HelmOfArtorias = 1660000,
		HelmOfFavor = 1100000,
		HelmOfTheWise = 1110000,
		HelmOfThorns = 1200000,
		HollowSoldierHelm = 1480000,
		HollowThiefsHood = 1500000,
		HollowWarriorHelm = 1520000,
		IronHelm = 1160000,
		KnightHelm = 1390000,
		MaidenHood = 1410000,
		MaskOfTheChild = 1610000,
		MaskOfTheFather = 1590000,
		MaskOfTheMother = 1600000,
		MaskOfTheSealer = 1140000,
		MaskOfVelka = 1150000,
		OrnsteinsHelm = 1270000,
		PaintingGuardianHood = 1250000,
		PaladinHelm = 120000,
		PharissHat = 1240000,
		PorcelainMask = 1670000,
		PriestsHat = 1310000,
		RoyalHelm = 1580000,
		Sack = 1560000,
		ShadowMask = 160000,
		SilverKnightHelm = 1420000,
		SixEyedHelmOfTheChannelers = 190000,
		SmoughsHelm = 180000,
		SnickeringTopHat = 1700000,
		SorcererHat = 1220000,
		StandardHelm = 1210000,
		SteelHelm = 1490000,
		StoneHelm = 1120000,
		SunlightMaggot = 1190000,
		SymbolOfAvarice = 1570000,
		TatteredClothHood = 1230000,
		ThiefMask = 1300000,
		WandererHood = 1360000,
		WitchHat = 1340000,
		XanthousCrown = 1290000
	}

	// Key flags don't actually need their category embedded (since keys are located in a separate array in memory),
	// but it's simpler to use them anyway.
	public enum KeyFlags
	{
		AnnexKey = 42009,
		ArchivePrisonExtraKey = 42020,
		ArchiveTowerCellKey = 42004,
		ArchiveTowerGiantCellKey = 42006,
		ArchiveTowerGiantDoorKey = 42005,
		BasementKey = 42001,
		BigPilgrimsKey = 42011,
		BlightttownKey = 42007,
		BrokenPendant = 42520,
		CageKey = 42003,
		CrestKey = 42022,
		CrestOfArtorias = 42002,
		DungeonCellKey = 42010,
		KeyToTheDepths = 42014,
		KeyToNewLondoRuins = 42008,
		KeyToTheSeal = 42013,
		MasterKey = 42100,
		MysteryKey = 42017,
		PeculiarDoll = 4384,
		ResidenceKey = 42021,
		SewerChamberKey = 42018,
		UndeadAsylumF2EastKey = 42012,
		UndeadAsylumF2WestKey = 42016,
		WatchtowerBasementKey = 42019
	}

	public enum LeggingFlags
	{		
		AnkletOfTheGreatLord = 1553000,
		AntiquatedSkirt = 1333000,
		BalderLeggings = 1513000,
		BlackIronLeggings = 173000,
		BlackKnightLeggings = 1323000,
		BlackLeatherBoots = 1303000,
		BlackSorcererBoots = 1643000,
		BlackTights = 1153000,
		BloodStainedSkirt = 1403000,
		BootsOfTheExplorer = 1113000,
		BrassLeggings = 1453000,
		BrigandTrousers = 153000,
		CatarinaLeggings = 113000,
		ChainLeggings = 1173000,
		ChestersTrousers = 1703000,
		ClericLeggings = 1183000,
		CrimsonWaistcloth = 1143000,
		CrystallineLeggings = 1133000,
		DarkLeggings = 143000,
		EasternLeggings = 1283000,
		EliteKnightLeggings = 1353000,
		GiantLeggings = 1533000,
		GoldHemmedBlackSkirt = 1463000,
		GolemLeggings = 1473000,
		GoughsLeggings = 1683000,
		GuardianLeggings = 1693000,
		HardLeatherBoots = 1213000,
		HavelsLeggings = 1443000,
		HeavyBoots = 1233000,
		HollowSoldierWaistcloth = 1483000,
		HollowThiefsTights = 1503000,
		HollowWarriorWaistcloth = 1523000,
		HolyTrousers = 1313000,
		IronLeggings = 1163000,
		KnightLeggings = 1393000,
		LeatherBoots = 1243000,
		LeggingsOfArtorias = 1663000,
		LeggingsOfFavor = 1103000,
		LeggingsOfThorns = 1203000,
		LordsBladeWaistcloth = 1673000,
		MaidenSkirt = 1413000,
		MoonlightWaistcloth = 1543000,
		OrnsteinsLeggings = 1273000,
		PaintingGuardianWaistcloth = 1253000,
		PaladinLeggings = 123000,
		ShadowLeggings = 163000,
		SilverKnightLeggings = 1423000,
		SmoughsLeggings = 183000,
		SorcererBoots = 1223000,
		SteelLeggings = 1493000,
		StoneLeggings = 1123000,
		TravelingBoots = 1383000,
		WaistclothOfTheChannelers = 193000,
		WandererBoots = 1363000,
		WitchSkirt = 1343000,
		XanthousWaistcloth = 1293000
	}

	public enum MiracleFlags
	{
		BountifulSunlight = 45050,
		DarkmoonBlade = 45910,
		EmitForce = 45320,
		Force = 45300,
		GravelordGreatswordDance = 45110,
		GravelordSwordDance = 45100,
		GreatHeal = 45010,
		GreatHealExcerpt = 45020,
		GreatLightningSpear = 45510,
		GreatMagicBarrier = 45610,
		Heal = 45000,
		Homeward = 45210,
		KarmicJustice = 45700,
		LightningSpear = 45500,
		MagicBarrier = 45600,
		Replenishment = 45040,
		SeekGuidance = 45400,
		SoothingSunlight = 45030,
		SunlightBlade = 45900,
		SunlightSpear = 45520,
		TranquilWalkOfPeace = 45800,
		VowOfSilence = 45810,
		WrathOfTheGods = 45310
	}

	public enum MultiplayerItemFlags
	{
		BlackSeparationCrystal = 4103,
		BlueEyeOrb = 4113,
		BookOfTheGuilty = 4108,
		CrackedRedEyeOrb = 4111,
		DragonEye = 4114,
		Indictment = 4373,
		OrangeGuidanceSoapstone = 4106,
		PurpleCowardsCrystal = 4118,
		RedEyeOrb = 4102,
		RedSignSoapstone = 4101,
		ServantRoster = 4112,
		WhiteSignSoapstone = 4100
	}

	public enum OreFlags
	{
		BlueTitaniteChunk = 41040,
		BlueTitaniteSlab = 41080,
		DemonTitanite = 41120,
		DragonScale = 41110,
		GreenTitaniteShard = 41020,
		LargeTitaniteShard = 41010,
		RedTitaniteChunk = 41060,
		RedTitaniteSlab = 41100,
		TitaniteChunk = 41030,
		TitaniteSlab = 41070,
		TitaniteShard = 41000,
		TwinklingTitanite = 41130,
		WhiteTitaniteChunk = 41050,
		WhiteTitaniteSlab = 41090
	}

	public enum ProjectileFlags
	{
		AlluringSkull = 4294,
		BlackFirebomb = 4297,
		DungPie = 4293,
		Firebomb = 4292,
		LloydsTalisman = 4296,
		PoisonThrowingKnife = 4291,
		ThrowingKnife = 4290
	}

	public enum PyromancyFlags
	{
		AcidSurge = 44220,
		BlackFlame = 44530,
		ChaosFireWhip = 44520,
		ChaosStorm = 44510,
		Combustion = 44100,
		Fireball = 44000,
		FireOrb = 44010,
		Firestorm = 44030,
		FireSurge = 44050,
		FireTempest = 44040,
		FireWhip = 44060,
		FlashSweat = 44310,
		GreatChaosFireball = 44500,
		GreatCombustion = 44110,
		GreatFireball = 44020,
		IronFlesh = 44300,
		PoisonMist = 44200,
		PowerWithin = 44400,
		ToxicMist = 44210,
		UndeadRapport = 44360
	};

	public enum RingFlags
	{
		BellowingDragoncrestRing = 2115,
		BloodbiteRing = 2109,
		BlueTearstoneRing = 2147,
		CalamityRing = 2150,
		CatCovenantRing = 2103,
		CloranthyRing = 2104,
		CovenantOfArtorias = 2138,
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
		RareRingOfSacrifice = 2127,
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
		BalderShield = 91455000,
		BlackIronGreatshield = 99003000,
		BlackKnightShield = 91474000,
		Bloodshield = 99002000,
		BonewheelShield = 91506000,
		Buckler = 91405000,
		CaduceusKiteShield = 91477000,
		CaduceusRoundShield = 91410000,
		CleansingGreatshield = 99014000,
		CrackedRoundShield = 91406000,
		CrestShield = 91456000,
		CrystalRingShield = 91411000,
		CrystalShield = 91471000,
		DragonCrestShield = 91457000,
		EagleShield = 91500000,
		EastWestShield = 91400000,
		EffigyShield = 99000000,
		GargoylesShield = 91478000,
		GiantShield = 91502000,
		GrassCrestShield = 91453000,
		GreatshieldOfArtorias = 91507000,
		HavelsGreatshield = 91505000,
		HeaterShield = 91450000,
		HollowSoldierShield = 91454000,
		IronRoundShield = 91461000,
		KnightShield = 91451000,
		LargeLeatherShield = 91402000,
		LeatherShield = 91408000,
		PierceShield = 91475000,
		PlankShield = 91409000,
		RedAndWhiteRoundShield = 91476000,
		SmallLeatherShield = 91403000,
		Sanctus = 99001000,
		SilverKnightShield = 91473000,
		SpiderShield = 91462000,
		SpikedShield = 91470000,
		StoneGreatshield = 91503000,
		SunlightShield = 91472000,
		TargetShield = 91404000,
		TowerKiteShield = 91452000,
		TowerShield = 91501000,
		WarriorsRoundShield = 91460000,
		WoodenShield = 91401000
	}

	public enum SorceryFlags
	{
		AuralDecoy = 43520,
		CastLight = 43500,
		Chameleon = 43550,
		CrystalMagicWeapon = 43120,
		CrystalSoulSpear = 43070,
		DarkBead = 43720,
		DarkFog = 43730,
		DarkOrb = 43710,
		FallControl = 43540,
		GreatHeavySoulArrow = 43030,
		GreatMagicWeapon = 43110,
		GreatSoulArrow = 43010,
		HeavySoulArrow = 43020,
		HiddenBody = 43410,
		HiddenWeapon = 43400,
		HomingCrystalSoulmass = 43050,
		HomingSoulmass = 43040,
		Hush = 43510,
		MagicShield = 43300,
		MagicWeapon = 43100,
		Pursuers = 43740,
		Remedy = 43610,
		Repair = 43530,
		ResistCurse = 46600,
		SoulArrow = 43000,
		SoulSpear = 43060,
		StrongMagicShield = 43310,
		WhiteDragonBreath = 43700
	}

	public enum SoulFlags
	{
		BequeathedLordSoulShardFourKings = 42502,
		BequeathedLordSoulShardSeath = 42503,
		CoreOfAnIronGolem = 4703,
		FireKeeperSoulAnastacia = 4390,
		FireKeeperSoulDarkmoonKnightess = 4391,
		FireKeeperSoulDaughterOfChaos = 4392,
		FireKeeperSoulNewLondoRuins = 4393,
		FireKeeperSoulBlighttown = 4394,
		FireKeeperSoulDukesArchives = 4395,
		FireKeeperSoulUndeadParish = 4396,
		GuardianSoul = 4709,
		LargeSoulOfABraveWarrior = 4407,
		LargeSoulOfALostUndead = 4401,
		LargeSoulOfANamelessSoldier = 4403,
		LargeSoulOfAProudKnight = 4405,
		LordSoulBedOfChaos = 42501,
		LordSoulNito = 42500,
		SoulOfABraveWarrior = 4406,
		SoulOfAGreatHero = 4409,
		SoulOfAHero = 4408,
		SoulOfALostUndead = 4400,
		SoulOfANamelessSoldier = 4402,
		SoulOfAProudKnight = 4404,
		SoulOfArtorias = 4710,
		SoulOfGwyn = 4702,
		SoulOfGwyndolin = 4708,
		SoulOfManus = 4711,
		SoulOfOrnstein = 4704,
		SoulOfPriscilla = 4707,
		SoulOfSif = 4701,
		SoulOfSmough = 4706,
		SoulOfTheMoonlightButterfly = 4705,
		SoulOfQuelaag = 4700
	}

	public enum SpearFlags
	{
		ChannelersTrident = 91004000,
		DemonsSpear = 91003000,
		DragonslayerSpear = 91051000,
		FourProngedPlow = 99016000,
		MoonlightButterflyHorn = 91052000,
		Partizan = 91002000,
		Pike = 91050000,
		SilverKnightSpear = 91006000,
		Spear = 91000000,
		WingedSpear = 91001000
	}

	public enum SwordFlags
	{
		AstorasStraightSword = 9209000,
		BalderSideSword = 9204000,
		BarbedStraightSword = 9207000,
		Broadsword = 9202000,
		BrokenStraightSword = 9203000,
		ChaosBlade = 9503000,
		CrystalStraightSword = 9205000,
		Darksword = 9210000,
		DrakeSword = 9211000,
		Estoc = 9602000,
		Falchion = 9401000,
		GoldTracer = 99010000,
		Iaito = 9502000,
		JaggedGhostBlade = 9403000,
		Longsword = 9201000,
		MailBreaker = 9600000,
		PaintingGurdianSword = 9405000,
		QuelaagsFurysword = 9406000,
		Rapier = 9601000,
		RicardsRapier = 9604000,
		Scimitar = 9400000,
		Shortsword = 9200000,
		Shotel = 9402000,
		SilverKnightStraightSword = 9208000,
		StraightSwordHilt = 9212000,
		SunlightStraightSword = 9206000,
		Uchigatana = 9500000,
		VelkasRapier = 9603000,
		WashingPole = 9501000
	}

	public enum TalismanFlags
	{
		CanvasTalisman = 91361000,
		DarkmoonTalisman = 91366000,
		IvoryTalisman = 91363000,
		SunlightTalisman = 91365000,
		Talisman = 91360000,
		ThorolundTalisman = 91362000,
		VelkasTalisman = 91367000
	}

	public enum ToolFlags
	{
		Binoculars = 4371,
		BlackEyeOrb = 4115,
		Darksign = 4117,
		DragonHeadStone = 4377,
		DragonTorsoStone = 4378,
		HelloCarving = 4510,
		HelpMeCarving = 4514,
		ImSorryCarving = 4513,
		SilverPendant = 4220,
		SkullLantern = 91396000,
		ThankYouCarving = 4511,
		VeryGoodCarving = 4512
	}

	public enum TrinketFlags
	{
		CopperCoin = 4381,
		GoldCoin = 4383,
		Pendant = 4376,
		Rubbish = 4380,
		SilverCoin = 4382
	}

	public enum WhipFlags
	{
		GuardianTail = 99019000,
		NotchedWhip = 91601000,
		Whip = 91600000
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
			(int)RingFlags.CovenantOfArtorias,
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
			(int)RingFlags.RareRingOfSacrifice,
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

		public static readonly int[] OrderedTools =
		{
			-1,
			(int)ToolFlags.Binoculars,
			(int)ToolFlags.BlackEyeOrb,
			(int)ToolFlags.Darksign,
			(int)ToolFlags.DragonHeadStone,
			(int)ToolFlags.DragonTorsoStone,
			(int)ToolFlags.SilverPendant,
			(int)ToolFlags.SkullLantern,
			-1,
			-1,
			(int)ToolFlags.HelloCarving,
			(int)ToolFlags.HelpMeCarving,
			(int)ToolFlags.ImSorryCarving,
			(int)ToolFlags.ThankYouCarving,
			(int)ToolFlags.VeryGoodCarving
		};

		public static readonly int[] OrderedTrinkets =
		{
			(int)TrinketFlags.CopperCoin,
			(int)TrinketFlags.GoldCoin,
			(int)TrinketFlags.Pendant,
			(int)TrinketFlags.Rubbish,
			(int)TrinketFlags.SilverCoin
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
			OrderedConsumables,
			OrderedCovenantItems,
			OrderedKeys,
			OrderedMultiplayerItems,
			OrderedSouls,
			OrderedTrinkets,
			null,
			null,
			OrderedAmmunition,
			OrderedProjectiles,
			OrderedRings,
			OrderedShields,
			OrderedTools,
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
