//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// BestiaryEntryData stores entry data for use in BestiaryMenu. Saved using a 
// Json file.
//
//=============================================================================

namespace Hive.Armada.Menus
{
    public class BestiaryEntryData
    {
        /// <summary>
        /// Name of enemy on each entry.
        /// </summary>
        public string[] enemyNames;

        /// <summary>
        /// Display name of enemy on each entry.
        /// </summary>
        public string[] enemyDisplayNames;

        /// <summary>
        /// Text description for enemy on each entry.
        /// </summary>
        public string[] enemyTexts;

        /// <summary>
        /// States of whether each enemy is locked.
        /// </summary>
        public bool[] enemiesLocked;

        /// <summary>
        /// Name used for locked entries.
        /// </summary>
        public string lockedName;

        /// <summary>
        /// Text used for locked entries.
        /// </summary>
        public string lockedText;
    }
}
