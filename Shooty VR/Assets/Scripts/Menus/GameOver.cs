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

namespace Hive.Armada
{
    public class GameOver : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// Triggers gameover/reload process
        /// </summary>
        public void Awake()
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
            yield return new WaitForSeconds(3.0f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void OnRestartButton()
        {
            GameObject.Find("Main Canvas").transform.Find("Start Menu").gameObject.SetActive(true);
            gameObject.SetActive(false);
        }

        public void OnQuitMainMenuButton()
        {
            GameObject.Find("Main Canvas").transform.Find("Main Menu").gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
