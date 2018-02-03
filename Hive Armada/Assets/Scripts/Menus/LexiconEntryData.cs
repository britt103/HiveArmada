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
        /// Name of item on each entry.
        /// </summary>
        public string[] entryNames;

        /// <summary>
        /// Text description for item on each entry.
        /// </summary>
        public string[] entryTexts;

        /// <summary>
        /// States of whether each entry is locked.
        /// </summary>
        public bool[] entriesLocked;

        /// <summary>
        /// Name used for locked entries.
        /// </summary>
        public string lockedName;

        /// <summary>
        /// Text used for locked entries.
        /// </summary>
        public string lockedText;

        /// <summary>
        /// Number of entrys in entries array.
        /// </summary>
        public int numEntries;
    }
}
