//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-440-01
// Group Project
//
// ShopItemButton stores an identifier for the corresponding Shop itemand calls 
// ShopMenu when its button has been clicked.
//
//=============================================================================

using UnityEngine;

namespace Hive.Armada.Menus
{
    /// <summary>
    /// Stores Shop item identifier and calls ShopMenu.
    /// </summary>
    public class ShopItemButton : MonoBehaviour
    {
        /// <summary>
        /// Index of corresponding object within Shop items.
        /// </summary>
        public int id;

        /// <summary>
        /// Reference to Lexicon Menu.
        /// </summary>
        public ShopMenu shopMenu;

        /// <summary>
        /// 
        /// </summary>
        public void PressButton()
        {
            shopMenu.OpenItem(id);
        }
    }
}
