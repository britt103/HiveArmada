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
    /// Contains navigation functions for the Shop and Lexicon menus.
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
        /// Reference to Lexicon Menu.
        /// </summary>
        public GameObject lexiconMenuGO;

        /// <summary>
        /// Reference to player transform for Lexicon Menu.
        /// </summary>
        public Transform lexiconTransform;

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
            //startCounter += 1;
            //if (startCounter > 1)
            //{
            //    source.Stop();
            //    source.PlayOneShot(clips[0]);
            //}
            FindObjectOfType<RoomTransport>().Transport(shopTransform, gameObject, shopMenuGO);
        }

        /// <summary>
        /// Lexicon button pressed. Navigate to Lexicon Menu.
        /// </summary>
        public void PressLexicon()
        {
            source.PlayOneShot(clips[0]);
            //startCounter += 1;
            //if (startCounter > 1)
            //{
            //    source.Stop();
            //    source.PlayOneShot(clips[0]);
            //}
            FindObjectOfType<RoomTransport>().Transport(lexiconTransform, gameObject, lexiconMenuGO);
        }

        /// <summary>
        /// Back button pressed; navigates to Main Menu.
        /// </summary>
        public void PressBack()
        {
            source.PlayOneShot(clips[1]);
            //backCounter += 1;
            //if (backCounter > 1)
            //{
            //    source.Stop();
            //    source.PlayOneShot(clips[1]);
            //}
            transitionManager.Transition(backMenuGO);
        }
    }
}
