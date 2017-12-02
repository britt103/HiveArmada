//Name: Chad Johnson
//Student ID: 1763718
//Email: johns428@mail.chapman.edu
//Course: CPSC 340-01, CPSC-344-01
//Assignment: Group Project
//Purpose: Visuals and stats for game over condition; Controls buttons and navigation

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Hive.Armada.Player;
using Hive.Armada.Game;

namespace Hive.Armada
{
    public class GameOver : MonoBehaviour
    {
        /// <summary>
        /// Reference to ReferenceManager.
        /// </summary>
        private ReferenceManager reference;

        /// <summary>
        /// Reference to Text GameObject for wave stat.
        /// </summary>
        public GameObject wavesTextGO;

        /// <summary>
        /// Reference to Text GameObject for time stat.
        /// </summary>
        public GameObject timeTextGO;

        /// <summary>
        /// Reference to Text GameObject for kills stat.
        /// </summary>
        public GameObject killsTextGO;

        /// <summary>
        /// Triggers gameover/reload process. Find references. Set text values.
        /// </summary>
        public void OnEnable()
        {
            reference = FindObjectOfType<ReferenceManager>();
            StartCoroutine(Reload());

            PlayerStats stats = FindObjectOfType<PlayerStats>();

            stats.PrintStats();
            wavesTextGO.GetComponent<Text>().text = "Waves: " + stats.waves;
            timeTextGO.GetComponent<Text>().text = "Time: " + stats.totalAliveTime;
            killsTextGO.GetComponent<Text>().text = "Kills: " + stats.totalEnemiesKilled;
        }

        /// <summary>
        /// Activate game over text, reloads scene
        /// </summary>
        private IEnumerator Reload()
        {
            yield return new WaitForSeconds(10.0f);
            reference.sceneTransitionManager.TransitionOut("Menu Room");
        }

        /// <summary>
        /// Restart button pressed; navigates to start menu
        /// </summary>
        public void OnRestartButton()
        {
            //GameObject.Find("Main Canvas").transform.Find("Start Menu").gameObject.SetActive(true);
            //gameObject.SetActive(false);
            reference.sceneTransitionManager.TransitionOut("Wave Room");
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Quit to main menu button pressed; navigated to main manu
        /// </summary>
        public void OnQuitMainMenuButton()
        {
            //GameObject.Find("Main Canvas").transform.Find("Main Menu").gameObject.SetActive(true);
            //gameObject.SetActive(false);
            reference.sceneTransitionManager.TransitionOut("Menu Room");
            gameObject.SetActive(false);
        }
    }
}
