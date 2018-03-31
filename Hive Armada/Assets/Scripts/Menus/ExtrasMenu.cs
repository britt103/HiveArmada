//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// ExtrasMenu controls interactions with the Extras Menu.
//
//=============================================================================

using UnityEngine;

namespace Hive.Armada.Menus
{
    /// <summary>
    /// Contains navigation functions for the Shop and Bestiary menus.
    /// </summary>
    public class ExtrasMenu : MonoBehaviour
    {
        /// <summary>
        /// Reference to Menu Transition Manager.
        /// </summary>
        public MenuTransitionManager transitionManager;

        /// <summary>
        /// Reference to Shop Menu.
        /// </summary>
        public GameObject shopMenuGO;

        /// <summary>
        /// Reference to player transform for Shop Menu.
        /// </summary>
        public Transform shopTransform;

        /// <summary>
        /// Reference to Bestiary Menu.
        /// </summary>
        public GameObject bestiaryMenuGO;

        /// <summary>
        /// Reference to player transform for Bestiary Menu.
        /// </summary>
        public Transform bestiaryTransform;

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
        /// Start button pressed. Navigate to Shop Menu.
        /// </summary>
        public void PressShop()
        {
            source.PlayOneShot(clips[0]);
            FindObjectOfType<RoomTransport>().Transport(shopTransform, gameObject, shopMenuGO);
        }

        /// <summary>
        /// Bestiary button pressed. Navigate to Bestiary Menu.
        /// </summary>
        public void PressBestiary()
        {
            source.PlayOneShot(clips[0]);
            FindObjectOfType<RoomTransport>().Transport(bestiaryTransform, gameObject, bestiaryMenuGO);
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
