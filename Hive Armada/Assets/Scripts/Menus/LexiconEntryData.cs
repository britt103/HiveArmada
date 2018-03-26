//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// LexiconEntryData stores entry data for use in LexiconMenu. Saved using a 
// Json file.
//
//=============================================================================

namespace Hive.Armada.Menus
{
    public class LexiconEntryData
    {
        /// <summary>
        /// Name of powerup on each entry.
        /// </summary>
        public string[] powerupNames;

        /// <summary>
        /// Display name of powerup on each entry.
        /// </summary>
        public string[] powerupDisplayNames;

        /// <summary>
        /// Text description for powerup on each entry.
        /// </summary>
        public string[] powerupTexts;

        /// <summary>
        /// States of whether powerup entry is locked.
        /// </summary>
        public bool[] powerupsLocked;

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
        /// Name of weapon on each entry.
        /// </summary>
        public string[] weaponNames;

        /// <summary>
        /// Display name of weapon on each entry.
        /// </summary>
        public string[] weaponDisplayNames;

        /// <summary>
        /// Text description for weapon on each entry.
        /// </summary>
        public string[] weaponTexts;

        /// <summary>
        /// States of whether each weapon is locked.
        /// </summary>
        public bool[] weaponsLocked;

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
