//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// MainMenu controls interactions with the Main Menu.
//
//=============================================================================

using UnityEditor;
using UnityEngine;
using System.Collections;

namespace Hive.Armada.Menus
{
    /// <summary>
    /// Contains navigation functions for the Start and Options menu and prompting of applcation
    /// exit on Main Menu.
    /// </summary>
    public class MainMenu : MonoBehaviour
    {
        /// <summary>
        /// Reference to Menu Transition Manager.
        /// </summary>
        public MenuTransitionManager transitionManager;

        /// <summary>
        /// Reference to Start Menu.
        /// </summary>
        public GameObject startMenuGO;

        /// <summary>
        /// Reference to Options Menu.
        /// </summary>
        public GameObject optionsMenuGO;

        /// <summary>
        /// Reference to Menu Audio source.
        /// </summary>
        public AudioSource source;

        /// <summary>
        /// Clips to use with source.
        /// </summary>
        public AudioClip[] clips;

        public AudioClip[] startClips;

        private void Awake()
        {
            //StartCoroutine(playStartingSounds());
        }

        private IEnumerator playStartingSounds()
        {
            source.PlayOneShot(startClips[0]);
            yield return new WaitWhile(() => source.isPlaying);
            source.PlayOneShot(startClips[1]);
            yield return new WaitWhile(() => source.isPlaying);
            source.PlayOneShot(startClips[2]);
        }

        /// <summary>
        /// Start button pressed. Navigate to Start Menu.
        /// </summary>
        public void PressStart()
        {
            source.PlayOneShot(clips[0]);
            transitionManager.Transition(startMenuGO);
        }

        /// <summary>
        /// Options button pressed. Navigate to Options Menu.
        /// </summary>
        public void PressOptions()
        {
            source.PlayOneShot(clips[0]);
            transitionManager.Transition(optionsMenuGO);
        }

        /// <summary>
        /// Quit button pressed. Exit application.
        /// </summary>
        public void PressQuit()
        {
            if (Application.isEditor)
            {
                EditorApplication.isPlaying = false;
            }
            else
            {
                Application.Quit();
            }
        }
    }
}