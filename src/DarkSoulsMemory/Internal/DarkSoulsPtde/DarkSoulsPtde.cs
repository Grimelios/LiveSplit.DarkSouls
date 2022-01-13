using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
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

        #endregion
        
        #region Reading game values

        public int GetGameTimeInMilliseconds()
        {
            return ReadInt32(_inGameTime);
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


        public Vector3 GetPosition()
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

                var memVal = ReadInt32(address);
                Console.WriteLine(memVal);

                //Decode item
                int id = 0;
                ItemInfusion infusion = ItemInfusion.Normal;
                int level = 0;

                //if 4 or less digits -> non-upgradable item.
                if (categories.Contains(ItemCategory.Consumables) && memVal >= 200 && memVal <= 215 && !items.Any(j => j.Type == ItemType.EstusFlask))
                {
                    var estus = Item.AllItems.First(j => j.Type == ItemType.EstusFlask);
                    var instance = new Item(estus.Name, estus.Id, estus.Type, estus.Category, estus.StackLimit, estus.Upgrade);
                    instance.Infusion = infusion;
                    instance.UpgradeLevel = level;
                    items.Add(instance);
                    continue;
                }
                else if (memVal < 10000)
                {
                    id = memVal;
                }
                else
                {
                    //Separate digits
                    int one = memVal % 10;
                    int ten = (memVal / 10) % 10;
                    int hundred = (memVal / 100) % 10;

                    id = memVal - (one + (10 * ten) + (100 * hundred));
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


        public int GetCurrentTestValue()
        {
            return 0;
        }



        protected ItemId ComputeItemId(IntPtr address)
        {
            int rawId = ComputeRawId(address);
            int category = rawId != -1 ? GetCategory(address) : -1;

            return ComputeItemId(rawId, category);
        }

        protected int ComputeRawId(IntPtr address)
        {
            return ReadInt32(address);
        }

        private int GetCategory(IntPtr address)
        {
            int value = ReadByte(address - 0x1);

            return value.ToHex();
        }



        protected ItemId ComputeItemId(int rawId, int category)
        {
            int baseId;
        
            // All upgradeable items have an ID five digits or greater (but not all items with an ID that long are
            // necessarily upgradeable).
            if (rawId / 10000 > 0)
            {
                // For upgradeable items, mods and reinforcement are represented through the ID directly. The hundreds
                // digit (third from the right) represents mods, while the ones digit (far right) represents
                // reinforcement. All such IDs are at least five digits long.
                ComputeUpgrades(rawId, out int mods, out int reinforcement);
        
                baseId = rawId - mods * 100 - reinforcement;
            }
            else
            {
                baseId = rawId;
            }
        
            return new ItemId(baseId, category);
        }

        private void ComputeUpgrades(int rawId, out int mods, out int reinforcement)
        {
            // For upgradeable items, mods and reinforcement are represented through the ID directly. The hundreds
            // digit (third from the right) represents mods, while the tens digit (second from the right) represents
            // reinforcement. All such IDs are at least five digits long.
            mods = (rawId % 1000) / 100;
            reinforcement = rawId % 100;
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

        #endregion

    }
}
