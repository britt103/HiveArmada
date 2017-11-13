//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// LexiconMenu controls interactions with the Lexicon Menu.
//
//=============================================================================

using UnityEngine;

namespace Hive.Armada.Menus
{
    /// <summary>
    /// Controls interactions with Lexicon Menu.
    /// </summary>
    public class LexiconMenu : MonoBehaviour
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
