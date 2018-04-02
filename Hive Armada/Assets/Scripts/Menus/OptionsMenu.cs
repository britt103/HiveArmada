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
        private int controlsCounter = 0;

        private int displayCounter = 0;

        private int soundCounter = 0;

        private int gameplayCounter = 0;

        private int introCounter = 0;

        private int creditsCounter = 0;

        private int backCounter = 0;

        /// <summary>
        /// Controls button pressed. Navigate to Controls Menu.
        /// </summary>
        public void PressControls()
        {
			source.PlayOneShot(clips[0]);
            controlsCounter += 1;
            if (controlsCounter > 1)
            {
                source.Stop();
                source.PlayOneShot(clips[0]);
            }
            transitionManager.Transition(controlsGO);
        }

        /// <summary>
        /// Display button pressed. Navigate to Display Menu.
        /// </summary>
        public void PressDisplay()
        {
			source.PlayOneShot(clips[0]);
            displayCounter += 1;
            if (displayCounter > 1)
            {
                source.Stop();
                source.PlayOneShot(clips[0]);
            }
            transitionManager.Transition(displayGO);
        }

        /// <summary>
        /// Sound button pressed. Navigate to Sound Menu.
        /// </summary>
        public void PressSound()
        {
			source.PlayOneShot(clips[0]);
            soundCounter += 1;
            if (soundCounter > 1)
            {
                source.Stop();
                source.PlayOneShot(clips[0]);
            }
            transitionManager.Transition(soundGO);
        }

        /// <summary>
        /// Bestiary button pressed. Navigate to Bestiary Menu.
        /// </summary>
        public void PressGameplay()
        {
			source.PlayOneShot(clips[0]);
            gameplayCounter += 1;
            if (gameplayCounter > 1)
            {
                source.Stop();
                source.PlayOneShot(clips[0]);
            }
            transitionManager.Transition(gameplayGO);
        }

        /// <summary>
        /// Intro button pressed. Play intro cinematic.
        /// </summary>
        public void PressIntro()
        {
			source.PlayOneShot(clips[0]);
            introCounter += 1;
            if (introCounter > 1)
            {
                source.Stop();
                source.PlayOneShot(clips[0]);
            }
            //transitionManager.Transition(introGO);
            Debug.Log("Intro button pressed");
        }

        /// <summary>
        /// Credits button pressed. Roll credits.
        /// </summary>
        public void PressCredits()
        {
			source.PlayOneShot(clips[0]);
            creditsCounter += 1;
            if (creditsCounter > 1)
            {
                source.Stop();
                source.PlayOneShot(clips[0]);
            }
            //transitionManager.Transition(creditsGO);
            Debug.Log("Credits button pressed");
        }

        /// <summary>
        /// Back button pressed. Navigate to Main Menu.
        /// </summary>
        public void PressBack()
        {
			source.PlayOneShot(clips[1]);
            backCounter += 1;
            if (backCounter > 1)
            {
                source.Stop();
                source.PlayOneShot(clips[1]);
            }
            transitionManager.Transition(backMenuGO);  
        }
    }
}
