using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hive.Armada.Game;
using Hive.Armada.Player;
using UnityEngine.UI;

namespace Hive.Armada.Menu
{
    public class StartMenu : MonoBehaviour
    {
        public Spawner spawner;
        public GameObject[] countdownTimers;
        private bool isStarting;

        void Start()
        {
            foreach (GameObject countdownTimer in countdownTimers)
            {
                countdownTimer.SetActive(false);
            }
        }

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
                    StartCoroutine(Countdown());
                }
                else
                {
                    Debug.Log("CRITICAL - MENU'S REFERENCE TO SPAWNER IS NULL");
                }  
            }
        }

        private IEnumerator Countdown()
        {
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
            gameObject.SetActive(false);
        }
    }
}
