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
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Hive.Armada.Player;
using Hive.Armada.Game;

namespace Hive.Armada.Menus
{
    /// <summary>
    /// Controls interactions with Game Over Menu.
    /// </summary>
    public class GameOver : MonoBehaviour
    {
        public AudioSource Roy;
        public AudioClip[] loseAudio;
        
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
        /// Activate game over text and reload scene.
        /// </summary>
        private IEnumerator Reload()
        {
            Roy.PlayOneShot(loseAudio[0]);
            yield return new WaitForSeconds(1);
            loseSound();
            yield return new WaitForSeconds(10.0f);
            reference.sceneTransitionManager.TransitionOut("Menu Room");
        }

        /// <summary>
        /// Restart button pressed. Navigate to Start Menu.
        /// </summary>
        public void PressRestart()
        {
            //GameObject.Find("Main Canvas").transform.Find("Start Menu").gameObject.SetActive(true);
            //gameObject.SetActive(false);
            reference.sceneTransitionManager.TransitionOut("Wave Room");
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Quit to Main Menu button pressed. Navigate to Main Menu.
        /// </summary>
        public void PressQuitMainMenu()
        {
            //GameObject.Find("Main Canvas").transform.Find("Main Menu").gameObject.SetActive(true);
            //gameObject.SetActive(false);
            reference.sceneTransitionManager.TransitionOut("Menu Room");
            gameObject.SetActive(false);
        }

        public void loseSound()
        {
            int loseNumber = Random.Range(1, loseAudio.Length);
            Roy.PlayOneShot(loseAudio[loseNumber]);
        }
    }
}
