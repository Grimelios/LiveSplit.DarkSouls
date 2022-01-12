using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkSoulsMemory.Internal.DarkSoulsRemastered
{
    internal class DarkSoulsRemastered : BaseMemoryReaderWriter, IDarkSouls
    {
        #region init/attaching =======================================================================================================================================
        public DarkSoulsRemastered()
        {
            Attach();

            InitPlayerPtr();
            InitWorldProgressionPtr();
            InitMenuPrompt();
        }

        public bool Attach()
        {
            if (AttachByName("DarkSoulsRemastered"))
            {
                //These ptrs must refresh every frame
                InitPlayerPtr();
                InitCharacter();
            }

            return _isHooked;
        }

        #endregion

        #region init base pointers =======================================================================================================================================

        private IntPtr _player;
        private void InitPlayerPtr()
        {
            if (TryScan(new byte?[] { 0x48, 0x8B, 0x05, null, null, null, null, 0x45, 0x33, 0xED, 0x48, 0x8B, 0xF1, 0x48, 0x85, 0xC0 }, out _player))
            {
                _player = (IntPtr)ReadInt32(_player + ReadInt32(_player + 3) + 7);
            }
        }

        private IntPtr _worldProgression;
        private void InitWorldProgressionPtr()
        {
            if (TryScan(new byte?[] { 0x48, 0x8B, 0x0D, null, null, null, null, 0x41, 0xB8, 0x01, 0x00, 0x00, 0x00, 0x44 }, out _worldProgression))
            {
                _worldProgression = (IntPtr)ReadInt32((IntPtr)ReadInt32(_worldProgression + ReadInt32(_worldProgression + 3) + 7));
            }
        }

        private IntPtr _menuPrompt;
        private void InitMenuPrompt()
        {
            if (TryScan(new byte?[] { 0x48, 0x8d, 0x0d, null, null, null, null, 0x48, 0x89, 0x8a, 0x38, 0x0a, 0x00, 0x00 }, out _menuPrompt))
            {
                _menuPrompt = _menuPrompt + ReadInt32(_menuPrompt + 3) + 7;
            }
        }

        private IntPtr _playerIns;
        private IntPtr _playerCtrl;
        private IntPtr _forcedAnimation;
        private IntPtr _itemPrompt;


        public void InitCharacter()
        {
            if (TryScan(new byte?[]{0x48, 0x8B, 0x05, null, null, null, null, 0x48, 0x39, 0x48, 0x68, 0x0F, 0x94, 0xC0, 0xC3}, out _playerIns))
            {
                _playerIns = (IntPtr)(_playerIns.ToInt64() + ReadInt32(_playerIns + 3) + 7);
                _playerCtrl = (IntPtr)(ReadInt32(_playerIns) + 0x68);
                _forcedAnimation = (IntPtr)ReadInt32(_playerCtrl) + 0x16C;
                _itemPrompt = (IntPtr)ReadInt32(_playerCtrl) + 0x814;
            }
        }

        #endregion

        #region Reading game vales =======================================================================================================================================

        public int GetGameTimeInMilliseconds()
        {
            return ReadInt32(_player + 0xA4);
        }


        public bool IsBossDefeated(BossType bossType)
        {
            var boss = _bosses.First(i => i.BossType == bossType);
            var memVal = ReadByte(_worldProgression + boss.Offset);
            return memVal.IsBitSet(boss.Bit);
        }

        public MenuPrompt GetMenuPrompt()
        {
            var mem = ReadByte(_menuPrompt);
            if (Enum.IsDefined(typeof(MenuPrompt), (int)mem))
            {
                return (MenuPrompt)mem;
            }

            return MenuPrompt.Unknown;
        }

        public ForcedAnimation GetForcedAnimation()
        {
            InitCharacter();
            var mem = ReadInt32(_forcedAnimation);
            if (Enum.IsDefined(typeof(ForcedAnimation), mem))
            {
                return (ForcedAnimation)mem;
            }
            return ForcedAnimation.Unknown;
        }

        public ItemPrompt GetItemPrompt()
        {
            InitCharacter();
            var mem = ReadInt32(_itemPrompt);
            if (mem.TryParseEnum(out ItemPrompt itenPrompt))
            {
                return itenPrompt;
            }
            return ItemPrompt.Unknown;
        }


        public int GetCurrentTestValue()
        {
            return 0;
        }


        //public int NewGameType()
        //{
        //    return ReadByte(_player + 0x78);
        //}
        #endregion

        #region data/lookup tables =======================================================================================================================================

        private struct Boss
        {
            public Boss(BossType bossType, int offset, int bit)
            {
                BossType = bossType;
                Offset = offset;
                Bit = bit;
            }

            public BossType BossType;
            public int Offset;
            public int Bit;
        }

        private readonly List<Boss> _bosses = new List<Boss>()
        {
            new Boss(BossType.AsylumDemon       , 0x1   , 7),
            new Boss(BossType.Kalameet          , 0x2303, 3),
            new Boss(BossType.Gargoyles         , 0x3   , 4),
            new Boss(BossType.CapraDemon        , 0xF73 , 1),
            new Boss(BossType.CeaselessDischarge, 0x1   , 3),
            new Boss(BossType.CentipedeDemon    , 0x3C73, 2),
            new Boss(BossType.Quelaag           , 0x2   , 6),
            new Boss(BossType.Priscilla         , 0x3   , 3),
            new Boss(BossType.Gwyndolin         , 0x4673, 3),
            new Boss(BossType.Firesage          , 0x3F30, 5),
            new Boss(BossType.OrnsteinAndSmough , 0x2   , 3),
            new Boss(BossType.FourKings         , 0x2   , 2),
            new Boss(BossType.GapingDragon      , 0x3   , 5),
            new Boss(BossType.Nito              , 0x3   , 0),
            new Boss(BossType.Sif               , 0x3   , 2),
            new Boss(BossType.Gwyn              , 0x2   , 0),
            new Boss(BossType.IronGolem         , 0x2   , 4),
            new Boss(BossType.MoonlightButterfly, 0x2173, 3),
            new Boss(BossType.Pinwheel          , 0x3   , 1),
            new Boss(BossType.Seath             , 0x2   , 1),
            new Boss(BossType.TaurusDemon       , 0xF73 , 2),
            new Boss(BossType.BedOfChaos        , 0x2   , 5),
            new Boss(BossType.Manus             , 0x1   , 6),
            new Boss(BossType.Artorias          , 0x2303, 6),
        };


        #endregion

    }
}
