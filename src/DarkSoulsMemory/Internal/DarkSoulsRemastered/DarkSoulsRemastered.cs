﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Pipes;
using System.Linq;
using System.Numerics;
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

            InitGameDataManPtr();
            InitNetManImp();
            InitWorldProgressionPtr();
            InitMenuPrompt();
            InitGameDataManPtr();
        }

        public bool Attach()
        {
            if (AttachByName("DarkSoulsRemastered"))
            {
                //These ptrs must refresh every frame
               
                InitCharacter();
            }

            return _isHooked;
        }

        #endregion

        #region init base pointers =======================================================================================================================================

        private IntPtr _gameDataMan;
        private void InitGameDataManPtr()
        {
            if (TryScan(new byte?[] { 0x48, 0x8B, 0x05, null, null, null, null, 0x45, 0x33, 0xED, 0x48, 0x8B, 0xF1, 0x48, 0x85, 0xC0 }, out _gameDataMan))
            {
                _gameDataMan = _gameDataMan + ReadInt32(_gameDataMan + 3) + 7;
            }
        }

        private IntPtr _netManImp;
        private void InitNetManImp()
        {
            if (TryScan(new byte?[] { 0x48, 0x8b, 0x05, null, null, null, null, 0x48, 0x05, 0x08, 0x0a, 0x00, 0x00, 0x48, 0x89, 0x44, 0x24, 0x50, 0xe8, 0x34, 0xfc, 0xfd, 0xff }, out _netManImp))
            {
                _netManImp = _netManImp + ReadInt32(_netManImp + 3) + 7;
            }
        }

        private IntPtr _worldProgression;
        private void InitWorldProgressionPtr()
        {
            //This pointer path doesn't change when quiting to main menu. Can just resolve the pointer and keep using it for as long as the game is running.
            if (TryScan(new byte?[] { 0x48, 0x8B, 0x0D, null, null, null, null, 0x41, 0xB8, 0x01, 0x00, 0x00, 0x00, 0x44 }, out _worldProgression))
            {
                _worldProgression = _worldProgression + ReadInt32(_worldProgression + 3) + 7;
                _worldProgression = (IntPtr)ReadInt32(_worldProgression);
                _worldProgression = (IntPtr)ReadInt32(_worldProgression);
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

        private IntPtr _playerIns = IntPtr.Zero;
        private IntPtr _playerCtrl;
        private IntPtr _forcedAnimation;
        private IntPtr _itemPrompt;
        public void InitCharacter()
        {
            //Only scan once during initialization
            if (_playerIns == IntPtr.Zero)
            {
                if (TryScan(new byte?[] { 0x48, 0x8B, 0x05, null, null, null, null, 0x48, 0x39, 0x48, 0x68, 0x0F, 0x94, 0xC0, 0xC3 }, out _playerIns))
                {
                    _playerIns = _playerIns + ReadInt32(_playerIns + 3) + 7;
                }
            }

            //Always update child pointers
            var instance = (IntPtr)ReadInt32(_playerIns);
            _playerCtrl = instance + 0x68;
            _forcedAnimation = (IntPtr)ReadInt32(_playerCtrl) + 0x16C;
            _itemPrompt = (IntPtr)ReadInt32(_playerCtrl) + 0x814;
        }

        #endregion

        #region Reading game vales =======================================================================================================================================

        public int GetGameTimeInMilliseconds()
        {
            var gameDataManIns = (IntPtr)ReadInt32(_gameDataMan);
            return ReadInt32(gameDataManIns + 0xA4);
        }

        private int _previousMillis = 0;
        public bool IsPlayerLoaded()
        {
            //Can't find an address that has this flag, but I did notice that the timer only starts running when the player is loaded.
            var millis = GetGameTimeInMilliseconds();

            //Millis is 0 in main menu, when no save is loaded
            if (millis == 0)
            {
                _previousMillis = 0;
                return false;
            }

            //Detect a non 0 value of the clock - a save has just been loaded but the clock might not be running yet
            if (_previousMillis == 0)
            {
                _previousMillis = millis;
                return false;
            }

            //Clock is running since it has been initially loaded. 
            if (_previousMillis != millis)
            {
                _previousMillis = millis;
                return true;
            }

            return false;
        }



        public bool IsBossDefeated(BossType bossType)
        {
            var boss = _bosses.First(i => i.BossType == bossType);
            var memVal = ReadByte(_worldProgression + boss.Offset);
            return memVal.IsBitSet(boss.Bit);
        }

        public MenuPrompt GetMenuPrompt()
        {
            var menu = ReadByte(_menuPrompt);
            if (((int)menu).TryParseEnum(out MenuPrompt menuPrompt))
            {
                return menuPrompt;
            }
            return MenuPrompt.Unknown;
        }

        public ForcedAnimation GetForcedAnimation()
        {
            var anim = ReadInt32(_forcedAnimation);
            if (anim.TryParseEnum(out ForcedAnimation forcedAnimation))
            {
                return forcedAnimation;
            }
            return ForcedAnimation.Unknown;
        }

        public ItemPrompt GetItemPrompt()
        {
            InitCharacter();
            var mem = ReadInt32(_itemPrompt);
            if (mem.TryParseEnum(out ItemPrompt itemPrompt))
            {
                return itemPrompt;
            }
            return ItemPrompt.Unknown;
        }

        public Vector3 GetPlayerPosition()
        {
            var position = (IntPtr)ReadInt32(_playerIns);
            position = (IntPtr)ReadInt32(position + 0x68);
            position = (IntPtr)ReadInt32(position + 0x18);
            position = (IntPtr)ReadInt32(position + 0x28);
            position = (IntPtr)ReadInt32(position + 0x50);
            position = (IntPtr)ReadInt32(position + 0x20);
            position = position + 0x120;
            
            var x = ReadFloat(position);
            var y = ReadFloat(position + 0x4);
            var z = ReadFloat(position + 0x8);
            return new Vector3(x, y, z);
        }



        public List<Item> GetCurrentInventoryItems()
        {
            //Path: GameDataMan->hostPlayerGameData->equipGameData->equipInventoryData->equipInventoryDataSub
            var gameDataManIns = (IntPtr)ReadInt32(_gameDataMan);
            var hostPlayerGameData = (IntPtr)ReadInt32(gameDataManIns + 16);
            var equipGameData = hostPlayerGameData + 0x280; //640
            var equipInventoryData = equipGameData + 288;
            var equipInventoryDataSub = equipInventoryData + 16;

            //Item count
            var itemCount = ReadInt32(equipInventoryDataSub + 48);
            var keyCount = ReadInt32(equipInventoryDataSub + 52);

            //Struct has 2 lists, list 1 seems to be a subset of list 2, the lists start at the same address..
            //I think the first list only contains keys. The "master" list contains both.
            var itemList2Len = ReadInt32(equipInventoryDataSub);
            //var itemList1Len = ReadInt32(equipInventoryDataSub + 16);

            //var itemList1 = (IntPtr)ReadInt32(equipInventoryDataSub + 32);
            var itemList2 = (IntPtr)ReadInt32(equipInventoryDataSub + 40);

            var bytes = ReadBytes(itemList2, itemList2Len * 0x1c);
            var items = ItemReader.GetCurrentInventoryItems(bytes, itemList2Len, itemCount, keyCount);
            
            return items;
        }



        public BonfireState GetBonfireState(Bonfire bonfire)
        {
            
            if (TryScan(new byte?[] { 0x48, 0x8b, 0x05, null, null, null, null, 0x48, 0x05, 0x08, 0x0a, 0x00, 0x00, 0x48, 0x89, 0x44, 0x24, 0x50, 0xe8, 0x34, 0xfc, 0xfd, 0xff }, out IntPtr netManImpIns))
            {
                netManImpIns = (IntPtr)ReadInt32(_netManImp);
                var frpgNetBonfireDb = (IntPtr)ReadInt32(netManImpIns + 2920);

                var unknownStruct1 = (IntPtr)ReadInt32(frpgNetBonfireDb + 0x28);
                var unknownStruct2 = (IntPtr)ReadInt32(unknownStruct1);
                var frpgNetBonfireDbItem = (IntPtr)ReadInt32(unknownStruct2 + 0x10);

                while (frpgNetBonfireDbItem != IntPtr.Zero)
                {
                    var bonfireId = ReadInt32(frpgNetBonfireDbItem + 0x8);
                    var bonfireStatus = ReadInt32(frpgNetBonfireDbItem + 0xc);

                    if (bonfireId.TryParseEnum(out Bonfire foundBonfire) && foundBonfire == bonfire)
                    {
                        if (!bonfireStatus.TryParseEnum(out BonfireState state))
                        {
                            state = BonfireState.Undiscovered;
                        }
                        return state;
                    }

                    //First pointer in this struct is a pointer to the next struct. Linked list?
                    unknownStruct2 = (IntPtr)ReadInt32(unknownStruct2);
                    //Also update the pointer to the next bonfire item
                    frpgNetBonfireDbItem = (IntPtr)ReadInt32(unknownStruct2 + 0x10);
                }
            }
            return BonfireState.Undiscovered;
        }

        public ZoneType GetZone()
        {
            var netManImpIns = (IntPtr)ReadInt32(_netManImp);
            var world = ReadByte(netManImpIns + 0xa23);
            var area = ReadByte(netManImpIns + 0xa22);

            var zone = _zones.FirstOrDefault(i => i.World == world && i.Area == area);
            if (zone != null)
            {
                return zone.ZoneType;
            }

            return ZoneType.Unknown;
        }


        public void ResetInventoryIndices()
        {
            if (TryScan(new byte?[] { 0x48, 0x8D, 0x15, null, null, null, null, 0xC1, 0xE1, 0x10, 0x49, 0x8B, 0xC6, 0x41, 0x0B, 0x8F, 0x14, 0x02, 0x00, 0x00, 0x44, 0x8B, 0xC6, 0x42, 0x89, 0x0C, 0xB2, 0x41, 0x8B, 0xD6, 0x49, 0x8B, 0xCF }, out IntPtr basePtr))
            {
                basePtr = ReadPtr(basePtr + 3) + 7;
                for (int i = 0; i < 20; i++)
                {
                    Write(basePtr + (0x4 * i), uint.MaxValue);
                }
            }
        }

        public Area GetArea()
        {
            var instance = (IntPtr)ReadInt32(_playerIns);
            var playerCtrl = (IntPtr)ReadInt32(instance + 104);

            //var multiplayerAreaId = ReadInt32(playerCtrl + 0x354);
            var areaId = ReadInt32(playerCtrl + 0x358);

            if (areaId.TryParseEnum(out Area area))
            {
                return area;
            }

            return Area.NonInvadeableArea;
        }

        public List<int> GetCurrentTestValue()
        {


            return new List<int>()
            {
            };

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

        private class Zone
        {
            public Zone(int world, int area, ZoneType zoneType)
            {
                World = world;
                Area = area;
                ZoneType = zoneType;
            }

            public int World;
            public int Area;
            public ZoneType ZoneType;
        }

        private readonly List<Zone> _zones = new List<Zone>()
        {
            new Zone(10, 0, ZoneType.Depths),
            new Zone(10, 1, ZoneType.UndeadBurgAndUndeadParish),
            new Zone(10, 2, ZoneType.Firelink),
            new Zone(11, 0, ZoneType.PaintedWorldOfAriamis),
            new Zone(12, 0, ZoneType.DarkrootGardenAndDarkrootBasin),
            new Zone(12, 1, ZoneType.EntireDlc),
            new Zone(13, 0, ZoneType.Catacombs),
            new Zone(13, 1, ZoneType.TombOfTheGiants),
            new Zone(13, 2, ZoneType.GreatHollowAndAshLake),
            new Zone(14, 0, ZoneType.BlightTownAndQuelaagsDomain),
            new Zone(14, 1, ZoneType.DemonRuinsAndLostIzalith),
            new Zone(15, 0, ZoneType.SensFortress),
            new Zone(15, 1, ZoneType.AnorLondo),
            new Zone(16, 0, ZoneType.NewLondoRuinsValleyofDrakesTheAbyss),
            new Zone(17, 0, ZoneType.DukesArchivesAndCrystalCave),
            new Zone(18, 0, ZoneType.FirelinkAltarAndKilnoftheFirstFlame),
            new Zone(18, 1, ZoneType.UndeadAsylum),
        };

        #endregion

    }
}
