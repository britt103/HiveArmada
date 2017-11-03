//Name: Chad Johnson
//Student ID: 1763718
//Email: johns428@mail.chapman.edu
//Course: CPSC 340-01, CPSC-344-01
//Assignment: Group Project
//Purpose: Control interactions with start menu, primarily start button

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
        public GameObject[] countdownTimers;
        public GameObject gameOver;
        private bool isStarting;

        void Start()
        {
            foreach (GameObject countdownTimer in countdownTimers)
            {
                countdownTimer.SetActive(false);
            }
        }

        /// <summary>
        /// Called by start button; changes ship mode and starts countdown
        /// </summary>
        public void ButtonClicked()
        {
            if (!isStarting)
            {
                isStarting = true;

                GameObject ship = GameObject.FindGameObjectWithTag("Player");
                if (ship != null)
                {
                    if (ship.GetComponent<ShipController>() != null)
                        ship.GetComponent<ShipController>().SetShipMode(ShipController.ShipMode.Game);
                }

                if (spawner != null)
                {
                    gameObject.GetComponentInChildren<Button>().enabled = false;
                    FindObjectOfType<PowerUpStatus>().BeginTracking();
                    StartCoroutine(Countdown());
                }
                else
                {
                    Debug.Log("CRITICAL - MENU'S REFERENCE TO SPAWNER IS NULL");
                }  
            }
        }

        /// <summary>
        /// Changes countdown timer texts based on time, then starts spawner
        /// </summary>
        /// <returns></returns>
        private IEnumerator Countdown()
        {
            GameObject.Find("Title").SetActive(false);
            GameObject.Find("Start Button").SetActive(false);

            foreach (GameObject timer in countdownTimers)
            {
                timer.SetActive(true);
            }

            for (int i = 5; i > 0; i--)
            {
                foreach (GameObject timer in countdownTimers)
                {
                    timer.GetComponentInChildren<Text>().text = i.ToString();
                }
                yield return new WaitForSeconds(1.0f);
            }

            foreach (GameObject timer in countdownTimers)
            {
                timer.SetActive(false);
            }

            spawner.Run();
        }

        /// <summary>
        /// Triggers gameover/reload process
        /// </summary>
        public void GameOver()
        {
            StartCoroutine(Reload());
            FindObjectOfType<PlayerStats>().PrintStats();
        }

        /// <summary>
        /// Activate game over text, reloads scene
        /// </summary>
        /// <returns></returns>
        private IEnumerator Reload()
        {
            gameOver.SetActive(true);
            yield return new WaitForSeconds(3.0f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
