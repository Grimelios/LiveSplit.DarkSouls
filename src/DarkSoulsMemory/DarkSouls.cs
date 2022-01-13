using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DarkSoulsMemory.Internal;
using DarkSoulsMemory.Internal.DarkSoulsPtde;
using DarkSoulsMemory.Internal.DarkSoulsRemastered;

namespace DarkSoulsMemory
{
    public class DarkSouls
    {
        public DarkSouls()
        {
            Refresh();
        }

        private IDarkSouls _darkSouls;



        /// <summary>
        /// Returns the in game time in millisecond format. Returns 0 when in a menu, returns the clock value during a loading screen and starts running when fully loaded into the game.
        /// </summary>
        /// <returns></returns>
        public int GetGameTimeInMilliseconds()
        {
            if (_darkSouls == null)
            {
                return 0;
            }

            return _darkSouls.GetGameTimeInMilliseconds();
        }


        /// <summary>
        /// Returns true if the given boss is still alive.
        /// </summary>
        /// <param name="boss"></param>
        /// <returns></returns>
        public bool IsBossDefeated(BossType boss)
        {
            if (_darkSouls == null)
            {
                return false;
            }

            return _darkSouls.IsBossDefeated(boss);
        }


        /// <summary>
        /// Returns which menu is currently open (homewardBone/darksign/firekeepersoul, etc.)
        /// </summary>
        /// <returns></returns>
        public MenuPrompt GetMenuPrompt()
        {
            if (_darkSouls == null)
            {
                return MenuPrompt.Unknown;
            }

            return _darkSouls.GetMenuPrompt();
        }


        /// <summary>
        /// Returns the current forced animation
        /// </summary>
        /// <returns></returns>
        public ForcedAnimation GetForcedAnimation()
        {
            if (_darkSouls == null)
            {
                return ForcedAnimation.Unknown;
            }

            return _darkSouls.GetForcedAnimation();
        }


        /// <summary>
        /// Returns the current item prompt
        /// </summary>
        /// <returns></returns>
        public ItemPrompt GetItemPrompt()
        {
            if (_darkSouls == null)
            {
                return ItemPrompt.Unknown;
            }

            return _darkSouls.GetItemPrompt();
        }

        /// <summary>
        /// Returns the player's position
        /// </summary>
        /// <returns></returns>
        public Vector3 GetPlayerPosition()
        {
            if (_darkSouls == null)
            {
                return Vector3.Zero;
            }

            return _darkSouls.GetPlayerPosition();
        }


        /// <summary>
        /// Returns a list of the current inventory items
        /// </summary>
        /// <returns></returns>
        public List<Item> GetCurrentInventoryItems()
        {
            if (_darkSouls == null)
            {
                return new List<Item>();
            }

            return _darkSouls.GetCurrentInventoryItems();
        }


        /// <summary>
        /// Only for developers
        /// </summary>
        /// <returns></returns>
        public int GetCurrentTestValue()
        {
            if (_darkSouls == null)
            {
                return 0;
            }

            return _darkSouls.GetCurrentTestValue();
        }




        /// <summary>
        /// Refreshes the process attachment, should be called every frame. Returns true if the game is attached
        /// </summary>
        /// <returns></returns>
        public bool Refresh()
        {
            if (_darkSouls == null)
            {
                var processes = Process.GetProcesses().FirstOrDefault(i => i.ProcessName.ToLower().StartsWith("darksouls"));
                if (processes != null)
                {
                    if (processes.ProcessName == "DarkSoulsRemastered")
                    {
                        _darkSouls = new DarkSoulsRemastered();
                    }

                    if (processes.ProcessName == "DARKSOULS")
                    {
                        _darkSouls = new DarkSoulsPtde();
                    }
                }
            }
            else
            {
                if (!_darkSouls.Attach())
                {
                    _darkSouls = null;
                }
            }

            return _darkSouls != null;
        }
    }
}
