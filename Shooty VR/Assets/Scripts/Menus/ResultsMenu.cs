//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// Results Menu display the results of the the just completed run to the 
// player.
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
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

        /// <summary>
        /// Get and set results values.
        /// </summary>
        void Awake()
        {
            reference = FindObjectOfType<ReferenceManager>();
            stats = FindObjectOfType<PlayerStats>();

            wavesTextGO.GetComponent<Text>().text = "Waves: " + stats.waves;
            TimeSpan time = TimeSpan.FromSeconds(stats.totalAliveTime);
            string timeOutput = string.Format(time.Minutes + ":" + time.Seconds);
            timeTextGO.GetComponent<Text>().text = "Time: " + timeOutput;
            killsTextGO.GetComponent<Text>().text = "Kills: " + stats.totalEnemiesKilled;
            scoreTextGO.GetComponent<Text>().text = "Score: " + stats.totalScore;
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

