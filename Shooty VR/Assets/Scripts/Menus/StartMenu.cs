//Name: Chad Johnson
//Student ID: 1763718
//Email: johns428@mail.chapman.edu
//Course: CPSC 340-01, CPSC-344-01
//Assignment: Group Project
//Purpose: Control interactions and navigation with start game menu

using UnityEngine;
using Hive.Armada.Game;

namespace Hive.Armada.Menu
{
    public class StartMenu : MonoBehaviour
    {
        /// <summary>
        /// Reference to Reference Manager.
        /// </summary>
        private ReferenceManager reference;

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
        public void ButtonSoloNormalClicked()
        {
            reference.sceneTransitionManager.TransitionOut("Wave Room");
            gameObject.SetActive(false);
            
        }

        /// <summary>
        /// Back button pressed; navigates to main menu.
        /// </summary>
        public void OnBackButton()
        {
            GameObject.Find("Main Canvas").transform.Find("Main Menu").gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
