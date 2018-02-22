//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-440-1
// Group Project
//
// LexiconUnlockData stores information about unlocked Lexicon entries. Meant 
// to be preserved between scenes.
//
//=============================================================================

using System.Collections.Generic;
using UnityEngine;

namespace Hive.Armada.Menus
{
    public class LexiconUnlockData : MonoBehaviour
    {
        /// <summary>
        /// List of names of powerups unlocked.
        /// </summary>
        private List<string> powerupUnlocks = new List<string>();

        /// <summary>
        /// List of names of enemies unlocked.
        /// </summary>
        private List<string> enemyUnlocks = new List<string>();

        /// <summary>
        /// List of names of weapons unlocked.
        /// </summary>
        private List<string> weaponUnlocks = new List<string>();

        /// <summary>
        /// Add new entry name to powerup unlocks list.
        /// </summary>
        /// <param name="name">Name of unlocked powerup entry.</param>
        public void AddPowerupUnlock(string name)
        {
            name = name.Replace("_", " ").Replace("-Powerup", "");
            if (!powerupUnlocks.Contains(name))
            {
                powerupUnlocks.Add(name);
            }
        }

        /// <summary>
        /// Add new entry name to enemy unlocks list.
        /// </summary>
        /// <param name="name">Name of unlocked enemy entry.</param>
        public void AddEnemyUnlock(string name)
        {
            name = name.Replace("_", " ").Replace("(Clone)", "");
            if (!enemyUnlocks.Contains(name))
            {
                enemyUnlocks.Add(name);
            }
        }

        /// <summary>
        /// Add new entry name to weapon unlocks list.
        /// </summary>
        /// <param name="name">Name of unlocked weapon entry.</param>
        public void AddWeaponUnlock(string name)
        {
            name = name.Replace("_", " ");
            if (!weaponUnlocks.Contains(name))
            {
                weaponUnlocks.Add(name);
            }
        }

        /// <summary>
        /// Return unlocks lists.
        /// </summary>
        /// <returns>All unlock lists in a single list.</returns>
        public List<List<string>> GetUnlocks()
        {
            List<List<string>> allUnlocks = new List<List<string>>();
            allUnlocks.Add(powerupUnlocks);
            allUnlocks.Add(enemyUnlocks);
            allUnlocks.Add(weaponUnlocks);
            return allUnlocks;
        }

        /// <summary>
        /// Clear unlocks lists.
        /// </summary>
        public void ClearUnlocks()
        {
            powerupUnlocks.Clear();
            enemyUnlocks.Clear();
            weaponUnlocks.Clear();
        }
    }
}
