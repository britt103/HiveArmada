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
        /// List of names of entries unlocked.
        /// </summary>
        private List<string> unlocks = new List<string>();

        /// <summary>
        /// Add new entry name to unlocks list.
        /// </summary>
        /// <param name="name">Name of unlocked entry.</param>
        public void AddUnlock(string name)
        {
            if (!unlocks.Contains(name))
            {
                unlocks.Add(name);
            }
        }

        /// <summary>
        /// Return unlocks list.
        /// </summary>
        /// <returns>Unlocks list.</returns>
        public List<string> GetUnlocks()
        {
            return unlocks;
        }

        /// <summary>
        /// Clear unlocks list.
        /// </summary>
        public void ClearUnlocks()
        {
            unlocks.Clear();
        }
    }
}
