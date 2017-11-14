//Name: Chad Johnson
//Student ID: 1763718
//Email: johns428@mail.chapman.edu
//Course: CPSC 340-01, CPSC-344-01
//Assignment: Group Project
//Purpose: Visuals and stats for game over condition; Controls buttons and navigation

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hive.Armada.Player;

namespace Hive.Armada
{
    public class GameOver : MonoBehaviour
    {
        /// <summary>
        /// Triggers gameover/reload process
        /// </summary>
        public void OnEnable()
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
            yield return new WaitForSeconds(10.0f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        /// <summary>
        /// Restart button pressed; navigates to start menu
        /// </summary>
        public void OnRestartButton()
        {
            GameObject.Find("Main Canvas").transform.Find("Start Menu").gameObject.SetActive(true);
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Quit to main menu button pressed; navigated to main manu
        /// </summary>
        public void OnQuitMainMenuButton()
        {
            GameObject.Find("Main Canvas").transform.Find("Main Menu").gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
