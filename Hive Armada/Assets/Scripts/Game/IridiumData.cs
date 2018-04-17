//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-440-1
// Group Project
//
// IridiumData stores data about Iridium unlockables and what items have
// been unlocked.
//
//=============================================================================

namespace Hive.Armada.Game
{
    public class IridiumData
    {
        /// <summary>
        /// Amount of Iridium currently possesed by player.
        /// </summary>
        public int amount;

        /// <summary>
        /// Names of weapon items.
        /// </summary>
        public string[] weaponNames;

        /// <summary>
        /// Display names of weapon items.
        /// </summary>
        public string[] weaponDisplayNames;

        /// <summary>
        /// Descriptions of weapon items.
        /// </summary>
        public string[] weaponTexts;

        /// <summary>
        /// Costs of weapon items.
        /// </summary>
        public int[] weaponCosts;

        /// <summary>
        /// States of whether weapon item is locked.
        /// </summary>
        public bool[] weaponsLocked;
    }
}
