//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// StartMenu controls interactions with the Start Menu.
//
//=============================================================================

using UnityEngine;

namespace Hive.Armada.Menus
{
    /// <summary>
    /// Controls interactions with Start Menu;
    /// </summary>
    public class StartMenu : MonoBehaviour
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
        /// Reference to Loadout Menu.
        /// </summary>
        public GameObject loadoutMenuGO;

        /// <summary>
        /// Reference to Menu Audio source.
        /// </summary>
		public AudioSource source;

        /// <summary>
        /// Clips to use with source.
        /// </summary>
    	public AudioClip[] clips;

        /// <summary>
        /// Variables used as a check to make sure audio
        /// doesn't play over itself
        /// </summary>
        private int soloNormalCounter = 0;
        private int backCounter = 0;

        /// <summary>
        /// Called by start button; transition to Wave Room.
        /// </summary>
        public void PressSoloNormal()
        {
            source.PlayOneShot(clips[0]);
            soloNormalCounter += 1;
            if(soloNormalCounter > 1)
            {
                source.Stop();
                source.PlayOneShot(clips[0]);
            }
            transitionManager.Transition(loadoutMenuGO);
        }

        /// <summary>
        /// Back button pressed; navigates to main menu.
        /// </summary>
        public void PressBack()
        {
            source.PlayOneShot(clips[1]);
            backCounter += 1;
            if(backCounter > 1)
            {
                source.Stop();
                source.PlayOneShot(clips[1]);
            }
            transitionManager.Transition(backMenuGO);
        }
    }
}
