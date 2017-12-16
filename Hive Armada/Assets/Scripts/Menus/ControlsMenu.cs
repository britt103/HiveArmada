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

namespace Hive.Armada.Menus
{
    /// <summary>
    /// Controls interactions with Controls Menu and ControlsHighlighter activation. 
    /// </summary>
    public class ControlsMenu : MonoBehaviour
    {
        /// <summary>
        /// Reference to Menu Audio source.
        /// </summary>
		public AudioSource source;

        /// <summary>
        /// Clips to use with source.
        /// </summary>
    	public AudioClip[] clips;

        /// <summary>
        /// Back button pressed. Navigate to Options Menu.
        /// </summary>
        public void PressBack()
        {
			source.PlayOneShot(clips[0]);
            GameObject.Find("Main Canvas").transform.Find("Options Menu").gameObject
                    .SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
