//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-440-1
// Group Project
//
// BestiaryUnlockData stores information about unlocked Bestiary entries. Meant 
// to be preserved between scenes.
//
//=============================================================================

using System.Collections.Generic;
using UnityEngine;

namespace Hive.Armada.Menus
{
    public class BestiaryUnlockData : MonoBehaviour
    {
        /// <summary>
        /// List of names of enemies unlocked.
        /// </summary>
        private List<string> enemyUnlocks = new List<string>();

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
        /// Return unlocks list.
        /// </summary>
        /// <returns>List of unlocked entries.</returns>
        public List<string> GetUnlocks()
        {
            return enemyUnlocks;
        }

        /// <summary>
        /// Clear unlocks list.
        /// </summary>
        public void ClearUnlocks()
        {
            enemyUnlocks.Clear();
        }
    }
}
