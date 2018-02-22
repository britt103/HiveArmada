//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-440-01
// Group Project
//
// LexiconEntryButton stores an identifier for the corresponding Lexicon entry
// and calls LexiconMenu when its button has been clicked.
//
//=============================================================================

using UnityEngine;

namespace Hive.Armada.Menus
{
    /// <summary>
    /// Stores Lexicon entry identifier and calls LexiconMenu.
    /// </summary>
    public class LexiconEntryButton : MonoBehaviour
    {
        /// <summary>
        /// Index of corresponding object within Lexicon entries.
        /// </summary>
        public int id;

        /// <summary>
        /// Reference to Lexicon Menu.
        /// </summary>
        public LexiconMenu lexiconMenu;

        /// <summary>
        /// Open corresponding Lexicon entry through Lexicon Menu.
        /// </summary>
        public void PressButton()
        {
            lexiconMenu.OpenEntry(id);
        }
    }
}
