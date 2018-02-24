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
        public GameObject loadoutGO;

        /// <summary>
        /// Reference to Shop Menu.
        /// </summary>
        public GameObject shopGO;

        /// <summary>
        /// Reference to player transform for Shop Menu.
        /// </summary>
        public Transform shopTransform;

        /// <summary>
        /// Reference to Menu Audio source.
        /// </summary>
		public AudioSource source;

        /// <summary>
        /// Clips to use with source.
        /// </summary>
    	public AudioClip[] clips;

        /// <summary>
        /// Called by start button; navigates to Loadout Menu.
        /// </summary>
        public void PressSoloNormal()
        {
            source.PlayOneShot(clips[0]);
            transitionManager.Transition(loadoutGO);
        }
        /// <summary>
        /// Called by shop button; navigates to Shop Menu.
        /// </summary>
        public void PressShop()
        {
            source.PlayOneShot(clips[0]);
            FindObjectOfType<RoomTransport>().Transport(shopTransform, gameObject, shopGO);
        }

        /// <summary>
        /// Back button pressed; navigates to Main Menu.
        /// </summary>
        public void PressBack()
        {
            source.PlayOneShot(clips[1]);
            transitionManager.Transition(backMenuGO);
        }
    }
}
