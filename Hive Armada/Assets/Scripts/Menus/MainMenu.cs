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

        /// <summary>
        /// Variables used as a check to make sure audio
        /// doesn't play over itself
        /// </summary>
        private int startCounter;

        private int optionsCounter;

        //private void Awake()
        //{
        //    Random.InitState((int) Time.time);
        //    int randNum = Random.Range(0, startClips.Length);
        //    source.PlayOneShot(startClips[randNum]);
        //}

        /// <summary>
        /// Start button pressed. Navigate to Start Menu.
        /// </summary>
        public void PressStart()
        {
            source.PlayOneShot(clips[0]);
            startCounter += 1;
            if (startCounter > 1)
            {
                source.Stop();
                source.PlayOneShot(clips[0]);
            }
            transitionManager.Transition(startMenuGO);
        }

        /// <summary>
        /// Options button pressed. Navigate to Options Menu.
        /// </summary>
        public void PressOptions()
        {
            source.PlayOneShot(clips[0]);
            optionsCounter += 1;
            if (optionsCounter > 1)
            {
                source.Stop();
                source.PlayOneShot(clips[0]);
            }
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