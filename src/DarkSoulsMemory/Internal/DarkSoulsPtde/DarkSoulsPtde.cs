using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DarkSoulsMemory.Internal.DarkSoulsPtde
{
    internal class DarkSoulsPtde : BaseMemoryReaderWriter, IDarkSouls
    {
        #region Init/attaching =======================================================================================================================================
        public DarkSoulsPtde()
        {
            Attach();

            InitInGameTimePtr();
            InitWorldProgressionPtr();
            InitMenuPrompt();
            InitFrgpNetManImpBasePtr();
        }

        public bool Attach()
        {
            if (AttachByName("DARKSOULS"))
            {
                //these ptrs must refresh every frame
                InitCharacter();
                InitInventory();
            }

            return _isHooked;
        }

        #endregion

        #region base pointers =======================================================================================================================================
        
        private IntPtr _inGameTime;
        private void InitInGameTimePtr()
        {
            if (TryScan(new byte?[] { 0x8B, 0x0D, null, null, null, null, 0x8B, 0x7E, 0x1C, 0x8B, 0x49, 0x08, 0x8B, 0x46, 0x20, 0x81, 0xC1, 0xB8, 0x01, 0x00, 0x00, 0x57, 0x51, 0x32, 0xDB }, out _inGameTime))
            {
                _inGameTime = _inGameTime + 2;
                _inGameTime = (IntPtr)ReadInt32(_inGameTime);
                _inGameTime = (IntPtr)ReadInt32(_inGameTime);
                _inGameTime = _inGameTime + 0x68;
            }
        }

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

        private IntPtr _inventory;

        private void InitInventory()
        {
            //Shared with inGameTime - could reduce the amount of scanning per frame if I share a base ptr.
            if (TryScan(new byte?[] { 0x8B, 0x0D, null, null, null, null, 0x8B, 0x7E, 0x1C, 0x8B, 0x49, 0x08, 0x8B, 0x46, 0x20, 0x81, 0xC1, 0xB8, 0x01, 0x00, 0x00, 0x57, 0x51, 0x32, 0xDB }, out _inventory))
            {
                _inventory = _inventory + 2;
                _inventory = (IntPtr)ReadInt32(_inventory);
                _inventory = (IntPtr)ReadInt32(_inventory);
                _inventory = (IntPtr)ReadInt32(_inventory + 0x8);
                _inventory = _inventory + 0x1B8;
            }
        }

        private IntPtr _frgpNetManImpBasePtr;
        private void InitFrgpNetManImpBasePtr()
        {
            if (TryScan(new byte?[] { 0x83, 0x3d, null, null, null, null, 0x00, 0x75, 0x4b, 0xa1, 0xc8, 0x87, 0x37, 0x01, 0x50, 0x6a, 0x08, 0x68, 0x78, 0x0b, 0x00, 0x00 }, out _frgpNetManImpBasePtr))
            {
                _frgpNetManImpBasePtr = (IntPtr)ReadInt32(_frgpNetManImpBasePtr + 2);
            }
        }

        #endregion
        
        #region Reading game values

        public int GetGameTimeInMilliseconds()
        {
            return ReadInt32(_inGameTime);
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


        public Vector3 GetPlayerPosition()
        {
            var map = (IntPtr)ReadInt32(_character + 0x28);
            var position = (IntPtr)ReadInt32(map + 0x1c);
            var x = ReadFloat(position + 0x10);
            var y = ReadFloat(position + 0x14);
            var z = ReadFloat(position + 0x18);
            return new Vector3(x, y, z);
        }


        private List<Item> GetInventoryItems(int countOffset, int startOffset)
        {
            var items = new List<Item>();
            
            var start = _inventory + startOffset;
            var count = ReadInt32(_inventory + countOffset);

            var address = start;
            for (int i = 0; i < count; i++)
            {
                var category = ReadByte(address - 0x1);
                var itemId = ReadInt32(address);
                var quantity = ReadInt32(address+0x4);
                
                var hexCategory = category.ToHex();


                var categories = new List<ItemCategory>();
                switch (hexCategory)
                {
                    case 0:
                        categories.Add(ItemCategory.MeleeWeapons);
                        categories.Add(ItemCategory.RangedWeapons);
                        categories.Add(ItemCategory.Ammo);
                        categories.Add(ItemCategory.Shields);
                        categories.Add(ItemCategory.SpellTools);
                        break;

                    case 1:
                        categories.Add(ItemCategory.Armor);
                        break;

                    case 2:
                        categories.Add(ItemCategory.Rings);
                        break;

                    case 4:
                        categories.Add(ItemCategory.Consumables);
                        categories.Add(ItemCategory.Key);
                        categories.Add(ItemCategory.Spells);
                        categories.Add(ItemCategory.UpgradeMaterials);
                        categories.Add(ItemCategory.UsableItems);
                        break;
                }

                //Decode item
                int id = 0;
                ItemInfusion infusion = ItemInfusion.Normal;
                int level = 0;

                //if 4 or less digits -> non-upgradable item.
                if (categories.Contains(ItemCategory.Consumables) && itemId >= 200 && itemId <= 215 && !items.Any(j => j.Type == ItemType.EstusFlask))
                {
                    var estus = Item.AllItems.First(j => j.Type == ItemType.EstusFlask);
                    var instance = new Item(estus.Name, estus.Id, estus.Type, estus.Category, estus.StackLimit, estus.Upgrade);

                    //Item ID is both the item + reinforcement. Level field does not change in the games memory for the estus flask.
                    //Goes like this:
                    //200 == empty level 0
                    //201 == full level 0
                    //202 == empty level 1
                    //203 == full level 1
                    //203 == empty level 2
                    //204 == full level 2
                    //etc

                    //If the flask is not empty, the amount of charges is stored in the quantity field.
                    //If the ID - 200 is an even number, the flask is empty. For this case we can even ignore the 200 and just check the ID

                    instance.Quantity = itemId % 2 == 0 ? 0 : quantity;

                    //Calculating the upgrade level
                    instance.UpgradeLevel = (itemId - 200) / 2;

                    instance.Infusion = infusion;
                    items.Add(instance);
                    continue;
                }
                else if (itemId < 10000)
                {
                    id = itemId;
                }
                else
                {
                    //Separate digits
                    int one = itemId % 10;
                    int ten = (itemId / 10) % 10;
                    int hundred = (itemId / 100) % 10;

                    id = itemId - (one + (10 * ten) + (100 * hundred));
                    infusion = (ItemInfusion)hundred;
                    level = one + (10 * ten);
                }
                
                var lookupItem = Item.AllItems.FirstOrDefault(j => categories.Contains(j.Category) && j.Id == id);
                if (lookupItem != null)
                {
                    var instance = new Item(lookupItem.Name, lookupItem.Id, lookupItem.Type, lookupItem.Category, lookupItem.StackLimit, lookupItem.Upgrade);
                    instance.Quantity = quantity;
                    instance.Infusion = infusion;
                    instance.UpgradeLevel = level;
                    items.Add(instance);
                }

                address += 0x1C; //Items are separated by this amount
            }
            return items;
        }

        public List<Item> GetCurrentInventoryItems()
        {
            var inventoryItems = GetInventoryItems(0x128, 0xA24);
            var keyItems = GetInventoryItems(0x12C, 0x324);
            inventoryItems.AddRange(keyItems);
            return inventoryItems;
        }

        public BonfireState GetBonfireState(Bonfire bonfire)
        {
            var pointer = (IntPtr)ReadInt32(_frgpNetManImpBasePtr);  //instance
            pointer = (IntPtr)ReadInt32(IntPtr.Add(pointer, 0xB48)); //frpgNetBonfireDb
            pointer = (IntPtr)ReadInt32(IntPtr.Add(pointer, 0x24));
            pointer = (IntPtr)ReadInt32(pointer);

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

        public int GetCurrentTestValue()
        {
            var state = GetBonfireState(Bonfire.UndeadAsylumInterior);

            return 0;
        }


        public ZoneType GetZone()
        {
            var zonePtr = (IntPtr)ReadInt32((IntPtr)0x137E204);//TODO: hardcoded address
            var world = ReadByte(zonePtr + 0xA13);
            var area = ReadByte(zonePtr + 0xA12);

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
            new Boss(BossType.GapingDragon      , 0x0   , 1),
            new Boss(BossType.Gargoyles         , 0x0   , 2),
            new Boss(BossType.Priscilla         , 0x0   , 3),
            new Boss(BossType.Sif               , 0x0   , 4),
            new Boss(BossType.Pinwheel          , 0x0   , 5),
            new Boss(BossType.Nito              , 0x0   , 6),
            new Boss(BossType.BedOfChaos        , 0x0   , 9),
            new Boss(BossType.Quelaag           , 0x0   , 8),
            new Boss(BossType.IronGolem         , 0x0   , 10),
            new Boss(BossType.OrnsteinAndSmough , 0x0   , 11),
            new Boss(BossType.FourKings         , 0x0   , 12),
            new Boss(BossType.Seath             , 0x0   , 13),
            new Boss(BossType.Gwyn              , 0x0   , 14),
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
            new Zone(15, 1, ZoneType.AnorLondo),
            new Zone(18, 0, ZoneType.FirelinkAltar),
            new Zone(10, 2, ZoneType.FirelinkShrine),
            new Zone(11, 0, ZoneType.PaintedWorld),
            new Zone(12, 1, ZoneType.SanctuaryGarden),
            new Zone(15, 0, ZoneType.SensFortressRoof),
            new Zone(16, 0, ZoneType.TheAbyss),
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
