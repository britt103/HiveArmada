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

using Hive.Armada.Game;
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
        /// Reference to Gameplay Menu.
        /// </summary>
        public GameObject gameplayGO;

        /// <summary>
        /// Reference to Intro Menu.
        /// </summary>
        public GameObject introGO;

        /// <summary>
        /// Reference to Credits Menu.
        /// </summary>
        public GameObject creditsGO;

        /// <summary>
        /// Reference to ReferenceManager.
        /// </summary>
        private ReferenceManager reference;

        /// <summary>
        /// Reference to Menu Audio source.
        /// </summary>
		public AudioSource source;

        /// <summary>
        /// Variables used as a check to make sure audio
        /// doesn't play over itself
        /// </summary>
        private int controlsCounter = 0;

        private int displayCounter = 0;

        private int soundCounter = 0;

        private int gameplayCounter = 0;

        private int introCounter = 0;

        private int creditsCounter = 0;

        private int backCounter = 0;

        private void Awake()
        {
            reference = FindObjectOfType<ReferenceManager>();
        }

        /// <summary>
        /// Controls button pressed. Navigate to Controls Menu.
        /// </summary>
        public void PressControls()
        {
            source.PlayOneShot(reference.menuSounds.menuButtonSelectSound);
            transitionManager.Transition(controlsGO);
        }

        /// <summary>
        /// Display button pressed. Navigate to Display Menu.
        /// </summary>
        public void PressDisplay()
        {
            source.PlayOneShot(reference.menuSounds.menuButtonSelectSound);
            transitionManager.Transition(displayGO);
        }

        /// <summary>
        /// Sound button pressed. Navigate to Sound Menu.
        /// </summary>
        public void PressSound()
        {
            source.PlayOneShot(reference.menuSounds.menuButtonSelectSound);
            transitionManager.Transition(soundGO);
        }

        /// <summary>
        /// Bestiary button pressed. Navigate to Bestiary Menu.
        /// </summary>
        public void PressGameplay()
        {
            source.PlayOneShot(reference.menuSounds.menuButtonSelectSound);
            transitionManager.Transition(gameplayGO);
        }

        /// <summary>
        /// Intro button pressed. Play intro cinematic.
        /// </summary>
        public void PressIntro()
        {
            source.PlayOneShot(reference.menuSounds.menuButtonSelectSound);
            //transitionManager.Transition(introGO);
            Debug.Log("Intro button pressed");
        }

        /// <summary>
        /// Credits button pressed. Roll credits.
        /// </summary>
        public void PressCredits()
        {
            source.PlayOneShot(reference.menuSounds.menuButtonSelectSound);
            //transitionManager.Transition(creditsGO);
            Debug.Log("Credits button pressed");
        }

        /// <summary>
        /// Back button pressed. Navigate to Main Menu.
        /// </summary>
        public void PressBack()
        {
            source.PlayOneShot(reference.menuSounds.menuButtonSelectSound);
            transitionManager.Transition(backMenuGO);  
        }
    }
}
