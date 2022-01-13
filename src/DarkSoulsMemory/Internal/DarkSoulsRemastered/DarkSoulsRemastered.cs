using System;
using System.Collections.Generic;
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
            InitWorldProgressionPtr();
            InitMenuPrompt();
        }

        public bool Attach()
        {
            if (AttachByName("DarkSoulsRemastered"))
            {
                //These ptrs must refresh every frame
                InitGameDataManPtr();
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
                _gameDataMan = (IntPtr)ReadInt32(_gameDataMan + ReadInt32(_gameDataMan + 3) + 7);
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
            return ReadInt32(_gameDataMan + 0xA4);
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
            //Path: GameDataMan->hostPlayerGameData->equipItemData->equipGameData->equipInventoryData->equipInventoryDataSub

            var items = new List<Item>();

            var hostPlayerGameData = (IntPtr)ReadInt32(_gameDataMan + 16);
            var equipData = hostPlayerGameData + 0x280;
            var equipItemData = equipData + 416;
            var equipGameData = (IntPtr)ReadInt32(equipItemData + 16);
            var equipInventoryData = equipGameData + 288;
            var equipInventoryDataSub = equipInventoryData + 16;

            //Item count
            var itemCount = ReadInt32(equipInventoryDataSub + 48);
            var keyCount = ReadInt32(equipInventoryDataSub + 52);

            //Struct has 2 lists, list 1 seems to be a subset of list 2, the lists start at the same address..

            var itemList2Len = ReadInt32(equipInventoryDataSub);
            var itemList1Len = ReadInt32(equipInventoryDataSub + 16);

            var itemList1 = (IntPtr)ReadInt32(equipInventoryDataSub + 32);
            var itemList2 = (IntPtr)ReadInt32(equipInventoryDataSub + 40);

            for (var i = 0; i < itemList2Len; i++)
            {
                //size of NS::FRPG_EquipInventoryDataItem is 28 or 0x1c
                var addr = itemList2 + (i * 0x1c);
                
                var cat = ReadByte(addr + 3);
                var item = ReadInt32(addr + 4);

                if (item != -1)
                {
                    //Console.WriteLine($"0x{addr.ToInt32():X}: {cat} {item}");

                    var categories = new List<ItemCategory>();
                    switch (cat.ToHex())
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
                    if (categories.Contains(ItemCategory.Consumables) && item >= 200 && item <= 215 && !items.Any(j => j.Type == ItemType.EstusFlask))
                    {
                        var estus = Item.AllItems.First(j => j.Type == ItemType.EstusFlask);
                        var instance = new Item(estus.Name, estus.Id, estus.Type, estus.Category, estus.StackLimit, estus.Upgrade);
                        instance.Infusion = infusion;
                        instance.UpgradeLevel = level;
                        items.Add(instance);
                        continue;
                    }
                    else if (item < 10000)
                    {
                        id = item;
                    }
                    else
                    {
                        //Separate digits
                        int one = item % 10;
                        int ten = (item / 10) % 10;
                        int hundred = (item / 100) % 10;

                        id = item - (one + (10 * ten) + (100 * hundred));
                        infusion = (ItemInfusion)hundred;
                        level = one + (10 * ten);
                    }

                    var lookupItem = Item.AllItems.FirstOrDefault(j => categories.Contains(j.Category) && j.Id == id);
                    if (lookupItem != null)
                    {
                        var instance = new Item(lookupItem.Name, lookupItem.Id, lookupItem.Type, lookupItem.Category, lookupItem.StackLimit, lookupItem.Upgrade);
                        instance.Infusion = infusion;
                        instance.UpgradeLevel = level;
                        items.Add(instance);
                    }
                }
            }

            //For debugging
            //foreach (var item in items)
            //{
            //    Console.WriteLine(item.Type);
            //}

            return items;
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
