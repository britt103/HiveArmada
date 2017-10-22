using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hive.Armada.Game;
using UnityEngine.UI;

namespace Hive.Armada.Menu
{
    public class StartMenu : MonoBehaviour
    {
        public Spawner spawner;
        public GameObject[] countdownTimers;

        // Use this for initialization
        void Start()
        {
            foreach(GameObject countdownTimer in countdownTimers)
            {
                countdownTimer.SetActive(false);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void buttonClicked()
        {
            if (spawner != null)
            {
                gameObject.GetComponentInChildren<Button>().enabled = false;
                StartCoroutine(Countdown());
            }

            else
                Debug.Log("CRITICAL - MENU'S REFERENCE TO SPAWNER IS NULL");
        }

        private IEnumerator Countdown()
        {
            for(int i = 0; i < countdownTimers.Length; ++i)
            {
                countdownTimers[i].SetActive(true);
            }

            for (int i = 5; i > 0; i--)
            {
                for (int j = 0; j < countdownTimers.Length; ++j)
                {
                    countdownTimers[j].GetComponentInChildren<Text>().text = i.ToString();
                }
                yield return new WaitForSeconds(1.0f);
            }

            for (int i = 0; i < countdownTimers.Length; ++i)
            {
                countdownTimers[i].SetActive(false);
            }

            spawner.Run();
            gameObject.SetActive(false);
        }
    }
}
