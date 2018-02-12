//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// Results Menu displays the results of the the just completed run to the 
// player.
//
//=============================================================================

using System;
using UnityEngine;
using UnityEngine.UI;
using Hive.Armada.Game;
using Hive.Armada.Player;

namespace Hive.Armada.Menus
{
    /// <summary>
    /// Display run results to player.
    /// </summary>
    public class ResultsMenu : MonoBehaviour
    {
        /// <summary>
        /// Reference to Menu Audio source.
        /// </summary>
        public AudioSource source;

        /// <summary>
        /// Clips to use with source.
        /// </summary>
    	public AudioClip[] clips;

        /// <summary>
        /// Reference to ReferenceManager.
        /// </summary>
        private ReferenceManager reference;

        private IridiumSystem iridiumSystem;

        public float iridiumScoreMultiplier = 0.2f;

        /// <summary>
        /// Reference to PlayerStats.
        /// </summary>
        private PlayerStats stats;

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
        /// Reference to Text GameObject for score stat.
        /// </summary>
        public GameObject scoreTextGO;

        public GameObject iridiumTextGO;

        public GameObject iridiumScoreTextGO;

        public GameObject iridiumShootablesTextGO;

        /// <summary>
        /// Get and set results values. Reset stats totals. Write to Iridium file.
        /// </summary>
        void Awake()
        {
            reference = FindObjectOfType<ReferenceManager>();
            iridiumSystem = FindObjectOfType<IridiumSystem>();
            stats = FindObjectOfType<PlayerStats>();

            wavesTextGO.GetComponent<Text>().text = "Waves: " + stats.waves;
            TimeSpan time = TimeSpan.FromSeconds(stats.totalAliveTime);
            string timeOutput = string.Format("{0:D2}:{1:D2}", time.Minutes, time.Seconds);
            timeTextGO.GetComponent<Text>().text = "Time: " + timeOutput;
            killsTextGO.GetComponent<Text>().text = "Kills: " + stats.totalEnemiesKilled;
            scoreTextGO.GetComponent<Text>().text = "Score: " + stats.totalScore;

            int iridiumScoreAmount = (int)(stats.totalScore * 0.2);
            int iridiumShootablesAmount = iridiumSystem.GetShootablesAmount();
            
            iridiumTextGO.GetComponent<Text>().text = "Iridium: " + 
                (iridiumScoreAmount + iridiumShootablesAmount);

            iridiumScoreTextGO.GetComponent<Text>().text = "From Score: " + iridiumScoreAmount;
            iridiumShootablesTextGO.GetComponent<Text>().text = "From Shootables: " 
                + iridiumShootablesAmount;

            stats.ResetTotals();
            iridiumSystem.AddIridium(iridiumScoreAmount);
            iridiumSystem.WriteIridiumFile();
        }

        /// <summary>
        /// Continue button pressed; navigates to Main Menu.
        /// Reset stat values.
        /// </summary>
        public void PressContinue()
        {
            source.PlayOneShot(clips[0]);
            stats.ResetValues();
            reference.menuMain.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}

