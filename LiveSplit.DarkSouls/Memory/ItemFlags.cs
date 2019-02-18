using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.DarkSouls.Memory
{
	public enum MiracleFlags
	{
	}

	public enum PyromancyFlags
	{
	};

	public enum SorceryFlags
	{
		SoulArrow = 3000,
		GreatSoulArrow = 3010,
		HeavySoulArrow = 3020,
		GreatHeavySoulArrow = 3030,
		HomingSoulmass = 3040,
		HomingCrystalSoulmass = 3050,
		SoulSpear = 3060,
		CrystalSoulSpear = 3070,
		MagicWeapon = 3100,
		GreatMagicWeapon = 3110,
		CrystalMagicWeapon = 3120,
		MagicShield = 3300,
		StrongMagicShield = 3310,
		HiddenWeapon = 3400,
		HiddenBody = 3410,
		CastLight = 3500,
		Hush = 3510,
		AuralDecoy = 3520,
		Repair = 3530,
		FallControl = 3540,
		Chameleon = 3550,
		ResistCurse = 6600,
		Remedy = 3610,
		WhiteDragonBreath = 3700,
		DarkOrb = 3710,
		DarkBead = 3720,
		DarkFog = 3730,
		Pursuers = 3740
	}

	public static class ItemFlags
	{
		public static readonly int[] OrderedMiracles =
		{
		};

		public static readonly int[] OrderedPyromancies =
		{
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
	}
}
