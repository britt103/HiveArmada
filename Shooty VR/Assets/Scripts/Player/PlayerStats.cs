//Name: Chad Johnson
//Student ID: 1763718
//Email: johns428@mail.chapman.edu
//Course: CPSC 340-01, CPSC-344-01
//Assignment: Group Project
//Purpose: Tracks player statistics

//PlayerStats is referenced in multiple classes for tracking player data for individual waves and whole runs. Data is output both 
//to the editor console and to a file in our project folder, currently PlayerStats.txt.

using UnityEngine;
using System;
using System.IO;

namespace Hive.Armada
{
    public class PlayerStats : MonoBehaviour
    {
        //tracking
        private string dateTime;

        //time alive
        [NonSerialized]
        private double aliveTime = 0;
        private double totalAliveTime = 0;
        [NonSerialized]
        private bool isAlive = false;
        private int waves = 0;

        //combat
        [NonSerialized]
        private int enemiesKilled = 0;
        private int totalEnemiesKilled = 0;
        [NonSerialized]
        private double firingTime = 0;
        private double totalFiringTime = 0;
        [NonSerialized]
        private int score = 0;
        private int totalScore = 0;
        [NonSerialized]
        private bool isFiring = false;

        //weapons
        [NonSerialized]
        public string w1Name = "";
        [NonSerialized]
        private int w1ShotsFired = 0;
        private int w1TotalShotsFired = 0;
        [NonSerialized]
        public string w2Name = "";
        [NonSerialized]
        private int w2ShotsFired = 0;
        private int w2TotalShotsFired = 0;

        //powerups
        [NonSerialized]
        public string p1Name = "";
        [NonSerialized]
        private int p1Count = 0;
        private int p1TotalCount = 0;
        [NonSerialized]
        public string p2Name = "";
        [NonSerialized]
        private int p2Count = 0;
        private int p2TotalCount = 0;
        [NonSerialized]
        public string p3Name = "";
        [NonSerialized]
        private int p3Count = 0;
        private int p3TotalCount = 0;
        [NonSerialized]
        public string p4Name = "";
        [NonSerialized]
        private int p4Count = 0;
        private int p4TotalCount = 0;
        [NonSerialized]
        public string p5Name = "";
        [NonSerialized]
        private int p5Count = 0;
        private int p5TotalCount = 0;

        //currency
        [NonSerialized]
        private int currencyCollected = 0;
        private int totalCurrencyCollected = 0;

        // Use this for initialization
        private void Start()
        {
            dateTime = DateTime.Now.ToString();
        }

        // Update is called once per frame
        void Update()
        {
            if (isFiring)
            {
                firingTime += Time.deltaTime;
            }

            if (isAlive)
            {
                aliveTime += Time.deltaTime;
            }
        }

        /// <summary>
        /// Append data to file when application it closed; only does so when waves have started
        /// </summary>
        private void OnApplicationQuit()
        {
            if(waves > 0)
            {
                PrintStats();
            }
        }

        /// <summary>
        /// Output player run data to console and to file
        /// </summary>
        public void PrintStats()
        {
            //totalFiringTime = Math.Round(totalFiringTime, 2);
            //totalAliveTime = Math.Round(totalAliveTime, 2);

            String output = dateTime + "\n" +
                      "Waves Finished: " + waves + "\n" +
                      "Total Score: " + totalScore + "\n" +
                      "Total Enemies Killed: " + totalEnemiesKilled + "\n" +
                      "Total Time Holding Down Trigger: " + totalFiringTime + "\n" +
                      "Total Shots Fired w/" + w1Name + ": " + w1TotalShotsFired + "\n" +
                      "Total Shots Fired w/" + w2Name + ": " + w2TotalShotsFired + "\n" +
                      "Total Time Alive: " + totalAliveTime + "\n" +
                      "Total Currency Collected: " + totalCurrencyCollected + "\n" +
                      "Total " + p1Name + " Use: " + p1TotalCount + "\n" +
                      "Total " + p2Name + " Use: " + p2TotalCount + "\n" +
                      "Total " + p3Name + " Use: " + p3TotalCount + "\n" +
                      "Total " + p4Name + " Use: " + p4TotalCount + "\n" +
                      "Total " + p5Name + " Use: " + p5TotalCount;
   
            Debug.Log(output);

            File.AppendAllText(@"PlayerStatsJson.txt", JsonUtility.ToJson(this, true) + "\n\n");
        }

        /// <summary>
        /// Output player wave data to console, reset wave values and add to run values.
        /// </summary>
        public void PrintWaveStats()
        {
            firingTime = Math.Round(firingTime, 2);
            aliveTime = Math.Round(aliveTime, 2);

            String output = DateTime.Now.ToString() + "\n" +
                      "Wave: " + waves + "\n" +
                      "Current Score: " + score + "\n" +
                      "Enemies Killed: " + enemiesKilled + "\n" +
                      "Time Holding Down Trigger: " + firingTime + "\n" +
                      "Shots Fired w/" + w1Name + ": " + w1ShotsFired + "\n" +
                      "Shots Fired w/" + w2Name + ": " + w2ShotsFired + "\n" +
                      "Time Alive: " + aliveTime + "\n" +
                      "Currency Collected: " + currencyCollected + "\n" +
                      p1Name + " Use: " + p1Count + "\n" +
                      p2Name + " Use: " + p2Count + "\n" +
                      p3Name + " Used: " + p3Count + "\n" +
                      p4Name + " Used: " + p4Count + "\n" +
                      p5Name + " Used: " + p5Count;
                      
            Debug.Log(output);

            totalEnemiesKilled += enemiesKilled;
            enemiesKilled = 0;
            totalFiringTime += firingTime;
            firingTime = 0;
            totalScore += score;
            score = 0;

            w1TotalShotsFired += w1ShotsFired;
            w1ShotsFired = 0;
            w2TotalShotsFired += w2ShotsFired;
            w2ShotsFired = 0;

            p1TotalCount += p1Count;
            p1Count = 0;
            p2TotalCount += p2Count;
            p2Count = 0;
            p3TotalCount += p3Count;
            p3Count = 0;
            p4TotalCount += p4Count;
            p4Count = 0;
            p5TotalCount += p5Count;
            p5Count = 0;

            totalCurrencyCollected += currencyCollected;
            currencyCollected = 0;
            totalAliveTime += aliveTime;
            aliveTime = 0;
            isAlive = false;
        }

        /// <summary>
        /// Set isAlive to true
        /// </summary>
        public void Alive()
        {
            isAlive = true;
        }

        /// <summary>
        /// Set isAlive to false
        /// </summary>
        public void NotAlive()
        {
            isAlive = false;
        }

        /// <summary>
        /// Set isFiring to true
        /// </summary>
        public void Firing()
        {
            isFiring = true;
        }

        /// <summary>
        /// Set isFiring to false
        /// </summary>
        public void NotFiring()
        {
            isFiring = false;
        }

        /// <summary>
        /// Add to enemiesKilled
        /// </summary>
        public void EnemyKilled()
        {
            enemiesKilled++;
        }

        /// <summary>
        /// Add to currencyCollected
        /// </summary>
        /// <param name="amount"></param>
        public void CurrencyCollected(int amount)
        {
            currencyCollected += amount;
        }

        /// <summary>
        /// Add to score
        /// </summary>
        /// <param name="points"></param>
        public void Score(int points)
        {
            score += points;
        }

        /// <summary>
        /// Add to 
        /// </summary>
        public void P1Used()
        {
            p1Count++;
        }

        /// <summary>
        /// Add to 
        /// </summary>
        public void P2Used()
        {
            p2Count++;
        }

        /// <summary>
        /// Add to 
        /// </summary>
        public void P3Used()
        {
            p3Count++;
        }

        /// <summary>
        /// Add to 
        /// </summary>
        public void P4Used()
        {
            p4Count++;
        }

        /// <summary>
        /// Add to 
        /// </summary>
        public void P5Used()
        {
            p5Count++;
        }

        /// <summary>
        /// Add to 
        /// </summary>
        /// <param name="shots"></param>
        public void W1Fired(int shots)
        {
            w1ShotsFired += shots;
        }

        /// <summary>
        /// Add to 
        /// </summary>
        /// <param name="shots"></param>
        public void W2Fired(int shots)
        {
            w2ShotsFired += shots;
        }

        /// <summary>
        /// Increment waves, print wave data to console
        /// </summary>
        public void WaveComplete()
        {
            waves++;
            PrintWaveStats();
        }
    }

}