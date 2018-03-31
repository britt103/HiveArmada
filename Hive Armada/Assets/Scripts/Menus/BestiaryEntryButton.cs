//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-440-01
// Group Project
//
// BestiaryEntryButton stores an identifier for the corresponding Bestiary entry
// and calls BestiaryMenu when its button has been clicked.
//
//=============================================================================

using UnityEngine;

namespace Hive.Armada.Menus
{
    /// <summary>
    /// Stores Bestiary entry identifier and calls BestiaryMenu.
    /// </summary>
    public class BestiaryEntryButton : MonoBehaviour
    {
        /// <summary>
        /// Index of corresponding object within Bestiary entries.
        /// </summary>
        public int id;

        /// <summary>
        /// Reference to Bestiary Menu.
        /// </summary>
        public BestiaryMenu BestiaryMenu;

        /// <summary>
        /// Open corresponding Bestiary entry through Bestiary Menu.
        /// </summary>
        public void PressButton()
        {
            BestiaryMenu.OpenEntry(id);
        }
    }
}
