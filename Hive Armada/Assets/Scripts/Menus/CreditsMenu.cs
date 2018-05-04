//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// CreditsMenu controls interactions with the Credits Menu.
//
//=============================================================================

using System.Collections;
using Hive.Armada.Game;
using UnityEngine;

namespace Hive.Armada.Menus
{
    /// <summary>
    /// Contains navigation from the Credits Menu.
    /// </summary>
    public class CreditsMenu : MonoBehaviour
    {
        /// <summary>
        /// Reference to ReferenceManager.
        /// </summary>
        private ReferenceManager reference;

        /// <summary>
        /// Reference to Menu Transition Manager.
        /// </summary>
        public MenuTransitionManager transitionManager;

        /// <summary>
        /// Reference to menu to go to when back is pressed.
        /// </summary>
        public GameObject backMenuGO;

        private bool isInteractable = false;

        /// <summary>
        /// Reference to Menu Audio source.
        /// </summary>
        public AudioSource source;

        private void Awake()
        {
            reference = FindObjectOfType<ReferenceManager>();
        }

        private void OnEnable()
        {
            StartCoroutine(InteractDelay());
        }

        private void OnDisable()
        {
            isInteractable = false;
        }

        private IEnumerator InteractDelay()
        {
            yield return new WaitForSeconds(Utility.interactDelay);

            isInteractable = true;
        }

        /// <summary>
        /// Back button pressed; navigates to Main Menu.
        /// </summary>
        public void PressBack()
        {
            if (!isInteractable)
            {
                return;
            }

            source.PlayOneShot(reference.menuSounds.menuButtonSelectSound);
            transitionManager.Transition(backMenuGO);
        }
    }
}

