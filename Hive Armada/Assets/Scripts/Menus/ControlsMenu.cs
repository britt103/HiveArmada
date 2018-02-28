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

namespace Hive.Armada.Menus
{
    /// <summary>
    /// Controls interactions with Controls Menu and ControlsHighlighter activation. 
    /// </summary>
    public class ControlsMenu : MonoBehaviour
    {
        /// <summary>
        /// Reference to Menu Transition Manager.
        /// </summary>
        public MenuTransitionManager transitionManager;

        /// <summary>
        /// Reference to menu to go to when back is pressed.
        /// </summary>
        public GameObject backMenuGO;

        /// <summary>
        /// Reference to Menu Audio source.
        /// </summary>
		public AudioSource source;

        /// <summary>
        /// Clips to use with source.
        /// </summary>
    	public AudioClip[] clips;

        /// <summary>
        /// Variable used as a check to make sure audio
        /// doesn't play over itself
        /// </summary>
        private int backCounter = 0;

        /// <summary>
        /// Back button pressed. Navigate to Options Menu.
        /// </summary>
        public void PressBack()
        {
			source.PlayOneShot(clips[0]);
            backCounter += 1;
            if (backCounter > 1)
            {
                source.Stop();
                source.PlayOneShot(clips[0]);
            }
            transitionManager.Transition(backMenuGO);
        }
    }
}
