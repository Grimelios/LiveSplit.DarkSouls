﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace DarkSoulsMemory.Internal
{
    internal class DarkSoulsPtde : BaseMemoryReaderWriter, IDarkSouls
    {
        #region Init/attaching =======================================================================================================================================
        public DarkSoulsPtde()
        {
            Attach();
            
            InitWorldProgressionPtr();
            InitMenuPrompt();
            InitNetManImp();
            InitGameDataMan();
            InitFlags();
        }

        public bool Attach()
        {
            if (AttachByName("DARKSOULS"))
            {
                //these ptrs must refresh every frame
                InitCharacter();
            }

            return _isHooked;
        }

        #endregion

        #region base pointers =======================================================================================================================================
        
        private IntPtr _worldProgression;
        private void InitWorldProgressionPtr()
        {
            if (TryScan(new byte?[] { 0x56, 0x8B, 0xF1, 0x8B, 0x46, 0x1C, 0x50, 0xA1, null, null, null, null, 0x32, 0xC9}, out _worldProgression))
            {
                _worldProgression = (IntPtr)ReadInt32(_worldProgression + 8); 
                _worldProgression = (IntPtr)ReadInt32(_worldProgression);
                _worldProgression = (IntPtr)ReadInt32(_worldProgression);
            }
        }

        private IntPtr _menuPrompt;
        private void InitMenuPrompt()
        {
            if (TryScan(new byte?[]{0xc7, 0x44, 0x24, 0x18, 0x00, 0x00, 0x00, 0x00, 0xa1, null, null, null, null, 0x8b, 0xb0}, out _menuPrompt))
            {
                _menuPrompt = (IntPtr)ReadInt32(_menuPrompt + 9);
                _menuPrompt = (IntPtr)ReadInt32(_menuPrompt);
                _menuPrompt = (IntPtr)ReadInt32(_menuPrompt + 0x9E8);
            }
        }
        
        private IntPtr _character;
        private void InitCharacter()
        {
            if (TryScan(new byte?[]{0x8B, 0x15, null, null, null, null, 0xF3, 0x0F, 0x10, 0x44, 0x24, 0x30, 0x52}, out _character))
            {
                _character = (IntPtr)ReadInt32(_character + 2);
                _character = (IntPtr)ReadInt32(_character);
                _character = (IntPtr)ReadInt32(_character + 0x4);
                _character = (IntPtr)ReadInt32(_character);
            }
        }

        private IntPtr _gameDataMan;
        private void InitGameDataMan()
        {
            if (TryScan(new byte?[] { 0x8B, 0x0D, null, null, null, null, 0x8B, 0x7E, 0x1C, 0x8B, 0x49, 0x08, 0x8B, 0x46, 0x20, 0x81, 0xC1, 0xB8, 0x01, 0x00, 0x00, 0x57, 0x51, 0x32, 0xDB }, out _gameDataMan))
            {
                _gameDataMan = _gameDataMan + 2;
                _gameDataMan = (IntPtr)ReadInt32(_gameDataMan);
            }
        }

        private IntPtr _netManImp;
        private void InitNetManImp()
        {
            if (TryScan(new byte?[] { 0x83, 0x3d, null, null, null, null, 0x00, 0x75, 0x4b, 0xa1 }, out _netManImp))
            {
                _netManImp = (IntPtr)ReadInt32(_netManImp + 2);
            }
        }

        private IntPtr _flags;

        private void InitFlags()
        {
            if (TryScan(new byte?[] { 0x33, 0xc4, 0x50, 0x8d, 0x44, 0x24, 0x0c, 0x64, 0xa3, 0x00, 0x00, 0x00, 0x00, 0x83, 0x3d, null, null, null, null, 0x00, 0x75, 0x4a }, out _flags))
            {
                _flags = _flags + 15;
                _flags = (IntPtr)ReadInt32(_flags);
            }
        }

        #endregion
        
        #region Reading game values

        public int GetGameTimeInMilliseconds()
        {
            var gameDataManIns = (IntPtr)ReadInt32(_gameDataMan);
            return ReadInt32(gameDataManIns + 0x68);
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
            if (_worldProgression == IntPtr.Zero)
            {
                InitWorldProgressionPtr();
            }

            var boss = _bosses.First(i => i.BossType == bossType);
            var memVal = ReadInt32(_worldProgression + boss.Offset);
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
            var mem = ReadInt32(_character + 0xFC);

            if (Enum.IsDefined(typeof(ForcedAnimation), mem))
            {
                return (ForcedAnimation)mem;
            }

            return ForcedAnimation.Unknown;
        }


        public ItemPrompt GetItemPrompt()
        {
            var mem = ReadInt32(_character + 0x62C);
            if (mem.TryParseEnum(out ItemPrompt i))
            {
                return i;
            }

            return ItemPrompt.Unknown;
        }


        public Vector3f GetPlayerPosition()
        {
            var map = (IntPtr)ReadInt32(_character + 0x28);
            var position = (IntPtr)ReadInt32(map + 0x1c);
            var x = ReadFloat(position + 0x10);
            var y = ReadFloat(position + 0x14);
            var z = ReadFloat(position + 0x18);
            return new Vector3f(x, y, z);
        }

        public List<Item> GetCurrentInventoryItems()
        {
            var gameDataManIns = (IntPtr)ReadInt32(_gameDataMan);//GameDataMan instance
            var hostPlayerGameData = (IntPtr)ReadInt32(gameDataManIns + 0x8);//Host player game data

            var itemCount = ReadInt32(hostPlayerGameData + 0x2e0);
            var keyCount = ReadInt32(hostPlayerGameData + 0x2e4);

            var listLength = ReadInt32(hostPlayerGameData + 0x4d4);
            var listStart = hostPlayerGameData + 0x4d8;

            var bytes = ReadBytes(listStart, listLength * 0x1c);
            var items = ItemReader.GetCurrentInventoryItems(bytes, listLength, itemCount, keyCount);

            return items;
        }

        public BonfireState GetBonfireState(Bonfire bonfire)
        {
            var pointer = (IntPtr)ReadInt32(_netManImp);  //instance
            pointer     = (IntPtr)ReadInt32(IntPtr.Add(pointer, 0xB48)); //frpgNetBonfireDb
            pointer     = (IntPtr)ReadInt32(IntPtr.Add(pointer, 0x24));
            pointer     = (IntPtr)ReadInt32(pointer);

            IntPtr bonfirePointer = (IntPtr)ReadInt32(IntPtr.Add(pointer, 0x8));

            while (bonfirePointer != IntPtr.Zero)
            {
                int bonfireId = ReadInt32(IntPtr.Add(bonfirePointer, 0x4));
                if (bonfireId == (int)bonfire)
                {
                    int bonfireState = ReadInt32(IntPtr.Add(bonfirePointer, 0x8));
                    var state = (BonfireState)bonfireState;
                    return (BonfireState)bonfireState;
                }

                pointer = (IntPtr)ReadInt32(pointer);
                bonfirePointer = (IntPtr)ReadInt32(IntPtr.Add(pointer, 0x8));
            }
            return BonfireState.Undiscovered;
        }


        //In remastered, an area id can be found in the player instance. It gets updated on the fly while walking around.
        //A similar behaving id does not seem to exist in PTDE.
        public Area GetArea()
        {
            throw new NotImplementedException();
        }

        public int GetPlayerHealth()
        {
            var gameDataManIns = (IntPtr)ReadInt32(_gameDataMan);//GameDataMan instance
            var hostPlayerGameData = (IntPtr)ReadInt32(gameDataManIns + 0x8);//Host player game data
            return ReadInt32(hostPlayerGameData + 0xc);
        }

        public CovenantType GetCovenant()
        {
            var gameDataManIns = (IntPtr)ReadInt32(_gameDataMan);//GameDataMan instance
            var hostPlayerGameData = (IntPtr)ReadInt32(gameDataManIns + 0x8);//Host player game data
            var covenant = ReadInt32(hostPlayerGameData + 0x10b);

            if (covenant.TryParseEnum(out CovenantType covenantType))
            {
                return covenantType;
            }
            return CovenantType.None;
        }

        public int GetClearCount()
        {
            var gameDataManIns = (IntPtr)ReadInt32(_gameDataMan);
            return ReadInt32(gameDataManIns + 0x3c);
        }

        public ZoneType GetZone()
        {
            var netManImpIns = (IntPtr)ReadInt32(_netManImp);
            var world = ReadByte(netManImpIns + 0xA13);
            var area = ReadByte(netManImpIns + 0xA12);

            var zone = _zones.FirstOrDefault(i => i.World == world && i.Area == area);
            if (zone != null)
            {
                return zone.ZoneType;
            }

            return ZoneType.Unknown;
        }

        public void ResetInventoryIndices()
        {
            if (TryScan(new byte?[] { 0x8B, 0x4C, 0x24, 0x34, 0x8B, 0x44, 0x24, 0x2C, 0x89, 0x8A, 0x38, 0x01, 0x00, 0x00, 0x8B, 0x90, 0x08, 0x01, 0x00, 0x00, 0xC1, 0xE2, 0x10, 0x0B, 0x90, 0x00, 0x01, 0x00, 0x00, 0x8B, 0xC1, 0x8B, 0xCD, 0x89, 0x14, 0xAD, null, null, null, null }, out IntPtr basePtr))
            {
                basePtr = (IntPtr)ReadInt32(basePtr + 0x24);
                foreach (int slot in _equipmentSlots)//Bit strange - the slots seem a single connected memory area. Why not write from start to end, increment baseptr by 4 bytes each loop
                {
                    Write(basePtr + slot, uint.MaxValue);
                }
            }
        }

        #endregion

        #region Flags

        public bool CheckFlag(int flag)
        {
            return GetEventFlagState(flag);
        }

        private bool GetEventFlagState(int id)
        {
            if (GetEventFlagAddress(id, out int address, out uint mask))
            {
                uint flags = (uint)ReadInt32((IntPtr)address);

                return (flags & mask) != 0;
            }
            return false;
        }

        private bool GetEventFlagAddress(int id, out int address, out uint mask)
        {
            string idString = id.ToString("D8");

            if (idString.Length == 8)
            {
                string group = idString.Substring(0, 1);
                string area = idString.Substring(1, 3);
                int section = int.Parse(idString.Substring(4, 1));
                int number = int.Parse(idString.Substring(5, 3));

                if (_eventFlagGroups.ContainsKey(group) && _eventFlagAreas.ContainsKey(area))
                {
                    int offset = _eventFlagGroups[group];
                    offset += _eventFlagAreas[area] * 0x500;
                    offset += section * 128;
                    offset += (number - (number % 32)) / 8;
                    
                    address = ReadInt32((IntPtr)_flags);
                    address = ReadInt32((IntPtr)address);
                    address += offset;

                    mask = 0x80000000 >> (number % 32);

                    return true;
                }
            }

            address = 0;
            mask = 0;

            return false;
        }

        private readonly Dictionary<string, int> _eventFlagGroups = new Dictionary<string, int>
        {
            {"0", 0x00000},
            {"1", 0x00500},
            {"5", 0x05F00},
            {"6", 0x0B900},
            {"7", 0x11300},
        };

        private readonly Dictionary<string, int> _eventFlagAreas = new Dictionary<string, int>
        {
            {"000", 00},
            {"100", 01},
            {"101", 02},
            {"102", 03},
            {"110", 04},
            {"120", 05},
            {"121", 06},
            {"130", 07},
            {"131", 08},
            {"132", 09},
            {"140", 10},
            {"141", 11},
            {"150", 12},
            {"151", 13},
            {"160", 14},
            {"170", 15},
            {"180", 16},
            {"181", 17},
        };

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
            new Boss(BossType.GapingDragon      , 0x0   , 29),
            new Boss(BossType.Gargoyles         , 0x0   , 28),
            new Boss(BossType.Priscilla         , 0x0   , 27),
            new Boss(BossType.Sif               , 0x0   , 26),
            new Boss(BossType.Pinwheel          , 0x0   , 25),
            new Boss(BossType.Nito              , 0x0   , 24),
            new Boss(BossType.BedOfChaos        , 0x0   , 21),
            new Boss(BossType.Quelaag           , 0x0   , 22),
            new Boss(BossType.IronGolem         , 0x0   , 20),
            new Boss(BossType.OrnsteinAndSmough , 0x0   , 19),
            new Boss(BossType.FourKings         , 0x0   , 18),
            new Boss(BossType.Seath             , 0x0   , 17),
            new Boss(BossType.Gwyn              , 0x0   , 16),
            new Boss(BossType.AsylumDemon       , 0x0   , 15),
            new Boss(BossType.TaurusDemon       , 0xF70 , 26),
            new Boss(BossType.CapraDemon        , 0xF70 , 25),
            new Boss(BossType.MoonlightButterfly, 0x1E70, 27),
            new Boss(BossType.SanctuaryGuardian , 0x2300, 31),
            new Boss(BossType.Artorias          , 0x2300, 30),
            new Boss(BossType.Manus             , 0x2300, 29),
            new Boss(BossType.Kalameet          , 0x2300, 27),
            new Boss(BossType.Firesage          , 0x3C30, 5),
            new Boss(BossType.CeaselessDischarge, 0x3C70, 27),
            new Boss(BossType.CentipedeDemon    , 0x3C70, 26),
            new Boss(BossType.Gwyndolin         , 0x4670, 27),
            new Boss(BossType.StrayDemon        , 0x5A70, 27),
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

        private int[] _equipmentSlots =
        {
            0x0, // Slot 7
            0x4, // Slot 0
            0x8, // Slot 8
            0xC, // Slot 1
            0x10, // Slot 9
            0x10 + 0x8, // Slot 10
            0x14, // Slot 11
            0x14 + 0x8, // Slot 12
            0x20, // Slot 14
            0x20 + 0x4, // Slot 15
            0x20 + 0x8, // Slot 16
            0x20 + 0xC, // Slot 17
            0x34, // Slot 18
            0x34 + 0x4, // Slot 19
            0x3C, // Slot 2
            0x3C + 0x4, // Slot 3
            0x3C + 0x8, // Slot 4
            0x3C + 0xC, // Slot 5
            0x3C + 0x10, // Slot 6
        };

        #endregion

    }
}