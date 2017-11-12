//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// ControlsMenu controls interactions with the Controls Menu.
//
//=============================================================================

using UnityEngine;
using Hive.Armada.Player;

namespace Hive.Armada.Menu
{
    /// <summary>
    /// Controls interactions with Controls Menu and ControlsHighlighter activation. 
    /// </summary>
    public class ControlsMenu : MonoBehaviour
    {
        /// <summary>
        /// ControlsHighlighter reference on active hand.
        /// </summary>
        private ControlsHighlighter ch;

        /// <summary>
        /// Activate controller highlighting.
        /// </summary>
        private void OnEnable()
        {
            ch = FindObjectOfType<ShipController>().transform.parent.GetComponentInChildren<ControlsHighlighter>();
            ch.FireOn();
            ch.PowerupOn();
            ch.PauseOn();
        }

        /// <summary>
        /// Back button pressed. Navigate to Options Menu.
        /// </summary>
        public void PressBack()
        {
            GameObject.Find("Main Canvas").transform.Find("Options Menu").gameObject
                    .SetActive(true);
            ch.AllOff();
            gameObject.SetActive(false);
        }
    }
}