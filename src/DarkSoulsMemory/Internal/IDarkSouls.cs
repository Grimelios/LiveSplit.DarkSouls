using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DarkSoulsMemory.Internal
{
    internal interface IDarkSouls
    {
        /// <summary>
        /// Hooks the game's process. Should be called every so often to ensure the game is still running.
        /// </summary>
        /// <returns></returns>
        bool Attach();


        /// <summary>
        /// Returns the in game time in millisecond format. Returns 0 when in a menu, returns the clock value during a loading screen and starts running when fully loaded into the game.
        /// </summary>
        /// <returns></returns>
        int GetGameTimeInMilliseconds();


        /// <summary>
        /// Returns true if the given boss is still alive.
        /// </summary>
        /// <param name="boss"></param>
        /// <returns></returns>
        bool IsBossDefeated(BossType boss);


        /// <summary>
        /// Returns which menu is currently open (homewardBone/darksign/firekeepersoul, etc.)
        /// </summary>
        /// <returns></returns>
        MenuPrompt GetMenuPrompt();


        /// <summary>
        /// Returns the current forced animation
        /// </summary>
        /// <returns></returns>
        ForcedAnimation GetForcedAnimation();


        /// <summary>
        /// Returns the current item prompt
        /// </summary>
        /// <returns></returns>
        ItemPrompt GetItemPrompt();

        /// <summary>
        /// Returns a list of the current inventory items
        /// </summary>
        /// <returns></returns>
        List<Item> GetCurrentInventoryItems();


        /// <summary>
        /// Returns the player's position
        /// </summary>
        /// <returns></returns>
        Vector3 GetPlayerPosition();

        /// <summary>
        /// For testing
        /// </summary>
        /// <returns></returns>
        int GetCurrentTestValue();
    }
}
