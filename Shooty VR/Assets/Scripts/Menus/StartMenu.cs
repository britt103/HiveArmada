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
using Hive.Armada.Game;

namespace Hive.Armada.Menus
{
    /// <summary>
    /// Controls interactions with Start Menu;
    /// </summary>
    public class StartMenu : MonoBehaviour
    {
        /// <summary>
        /// Reference to Reference Manager.
        /// </summary>
        private ReferenceManager reference;

        public AudioSource source;
        public AudioClip[] clips;

        /// <summary>
        /// Find references.
        /// </summary>
        private void Awake()
        {
            reference = FindObjectOfType<ReferenceManager>();
        }

        /// <summary>
        /// Called by start button; transition to Wave Room.
        /// </summary>
        public void PressSoloNormal()
        {
            source.PlayOneShot(clips[0]);
            reference.sceneTransitionManager.TransitionOut("Wave Room");
            gameObject.SetActive(false);
            
        }

        /// <summary>
        /// Back button pressed; navigates to main menu.
        /// </summary>
        public void PressBack()
        {
            source.PlayOneShot(clips[1]);
            GameObject.Find("Main Canvas").transform.Find("Main Menu").gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
