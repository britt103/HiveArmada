//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// GameOver controls interactions with the Game Over Menu.
//
//=============================================================================

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hive.Armada.Menu
{
    /// <summary>
    /// Controls interactions with Game Over Menu.
    /// </summary>
    public class GameOver : MonoBehaviour
    {
        /// <summary>
        /// Start Reload coroutine and trigger PrintStats() in PlayerStats.
        /// </summary>
        public void OnEnable()
        {
            StartCoroutine(Reload());
            FindObjectOfType<PlayerStats>().PrintStats();
        }

        /// <summary>
        /// Activate game over text and reload scene.
        /// </summary>
        private IEnumerator Reload()
        {
            yield return new WaitForSeconds(10.0f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        /// <summary>
        /// Restart button pressed. Navigate to Start Menu.
        /// </summary>
        public void PressRestart()
        {
            GameObject.Find("Main Canvas").transform.Find("Start Menu").gameObject.SetActive(true);
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Quit to Main Menu button pressed. Navigate to Main Menu.
        /// </summary>
        public void PressQuitMainMenu()
        {
            GameObject.Find("Main Canvas").transform.Find("Main Menu").gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
