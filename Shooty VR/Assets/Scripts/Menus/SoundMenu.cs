//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// SoundMenu controls interactions with the Sound Menu.
//
//=============================================================================

using UnityEngine;

namespace Hive.Armada.Menu
{
    /// <summary>
    /// Controls interactions with Sound Menu.
    /// </summary>
    public class SoundMenu : MonoBehaviour
    {
        /// <summary>
        /// Back button pressed. Navigate to Options Menu.
        /// </summary>
        public void PressBack()
        {
            GameObject.Find("Main Canvas").transform.Find("Options Menu").gameObject
                    .SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
