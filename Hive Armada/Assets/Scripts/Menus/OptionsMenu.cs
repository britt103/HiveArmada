//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// OptionsMenu controls interactions with the Options Menu.
//
//=============================================================================

using UnityEngine;

namespace Hive.Armada.Menus
{
    /// <summary>
    /// Controls interations with Options Menu.
    /// </summary>
    public class OptionsMenu : MonoBehaviour
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
        /// Reference to Display Menu.
        /// </summary>
        public GameObject displayGO;

        /// <summary>
        /// Reference to Sound Menu.
        /// </summary>
        public GameObject soundGO;

        /// <summary>
        /// Reference to Controls Menu.
        /// </summary>
        public GameObject controlsGO;

        /// <summary>
        /// Reference to Lexicon Menu.
        /// </summary>
        public GameObject lexiconGO;

        /// <summary>
        /// Reference to player transform for Lexicon Menu.
        /// </summary>
        public Transform lexiconTransform;

        /// <summary>
        /// Reference to Intro Menu.
        /// </summary>
        public GameObject introGO;

        /// <summary>
        /// Reference to Credits Menu.
        /// </summary>
        public GameObject creditsGO;

        /// <summary>
        /// Reference to Menu Audio source.
        /// </summary>
		public AudioSource source;

        /// <summary>
        /// Clips to use with source.
        /// </summary>
    	public AudioClip[] clips;

        /// <summary>
        /// Controls button pressed. Navigate to Controls Menu.
        /// </summary>
        public void PressControls()
        {
			source.PlayOneShot(clips[0]);
            transitionManager.Transition(controlsGO);
        }

        /// <summary>
        /// Display button pressed. Navigate to Display Menu.
        /// </summary>
        public void PressDisplay()
        {
			source.PlayOneShot(clips[0]);
            transitionManager.Transition(displayGO);
        }

        /// <summary>
        /// Sound button pressed. Navigate to Sound Menu.
        /// </summary>
        public void PressSound()
        {
			source.PlayOneShot(clips[0]);
            transitionManager.Transition(soundGO);
        }

        /// <summary>
        /// Lexicon button pressed. Navigate to Lexicon Menu.
        /// </summary>
        public void PressLexicon()
        {
			source.PlayOneShot(clips[0]);
            FindObjectOfType<RoomTransport>().Transport(lexiconTransform, gameObject, lexiconGO);
        }

        /// <summary>
        /// Intro button pressed. Play intro cinematic.
        /// </summary>
        public void PressIntro()
        {
			source.PlayOneShot(clips[0]);
            //transitionManager.Transition(introGO);
            Debug.Log("Intro button pressed");
        }

        /// <summary>
        /// Credits button pressed. Roll credits.
        /// </summary>
        public void PressCredits()
        {
			source.PlayOneShot(clips[0]);
            //transitionManager.Transition(creditsGO);
            Debug.Log("Credits button pressed");
        }

        /// <summary>
        /// Back button pressed. Navigate to Main Menu.
        /// </summary>
        public void PressBack()
        {
			source.PlayOneShot(clips[1]);
            transitionManager.Transition(backMenuGO);  
        }
    }
}
