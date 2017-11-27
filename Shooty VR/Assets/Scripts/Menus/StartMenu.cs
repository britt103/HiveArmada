//Name: Chad Johnson
//Student ID: 1763718
//Email: johns428@mail.chapman.edu
//Course: CPSC 340-01, CPSC-344-01
//Assignment: Group Project
//Purpose: Control interactions and navigation with start game menu

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hive.Armada.Game;
using Hive.Armada.Player;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Hive.Armada.Menu
{
    public class StartMenu : MonoBehaviour
    {
        public Spawner spawner;
        private bool isStarting;

        private ReferenceManager reference;

        /// <summary>
        /// Find references.
        /// </summary>
        private void Awake()
        {
            reference = FindObjectOfType<ReferenceManager>();
        }

        /// <summary>
        /// Called by start button; changes ship mode and starts countdown
        /// </summary>
        public void ButtonSoloNormalClicked()
        {
            if (!isStarting)
            {
                isStarting = true;

                //GameObject ship = GameObject.FindGameObjectWithTag("Player");

                //if (ship != null)
                //{
                //    if (ship.GetComponent<ShipController>() != null)
                //        ship.GetComponent<ShipController>().SetShipMode(ShipController.ShipMode.Game);
                //}

                //GameObject.Find("Main Canvas").transform.Find("Countdown").gameObject.SetActive(true);
                reference.sceneTransitionManager.TransitionTo("Test01");
                gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Back button pressed; navigates to main menu
        /// </summary>
        public void OnBackButton()
        {
            GameObject.Find("Main Canvas").transform.Find("Main Menu").gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
