//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-440-1
// Group Project
//
// MenuTransitionManager controls transitions between menus and tracks the
// currently active menu.
//
//=============================================================================

using UnityEngine;

namespace Hive.Armada.Menus
{
    /// <summary>
    /// Transition between menus.
    /// </summary>
    public class MenuTransitionManager : MonoBehaviour
    {
        /// <summary>
        /// Reference to currently active menu.
        /// </summary>
        public GameObject currMenu;

        /// <summary>
        /// Deactivate current menu and activate next menu. Save next menu as current menu.
        /// </summary>
        /// <param name="nextMenu"></param>
        public void Transition(GameObject nextMenu)
        {
            currMenu.SetActive(false);
            nextMenu.SetActive(true);
            currMenu = nextMenu;
        }
    }
}
