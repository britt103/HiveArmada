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
using System.Xml.Serialization;

namespace Hive.Armada
{
    public class PlayerStats : MonoBehaviour
    {
        //tracking
        public string dateTime;

        //time alive
        [NonSerialized]
        public double aliveTime = 0;
        public double totalAliveTime = 0;
        [NonSerialized]
        public bool isAlive = false;
        public int waves = 0;

        //combat
        [NonSerialized]
        public int enemiesKilled = 0;
        public int totalEnemiesKilled = 0;
        [NonSerialized]
        public double firingTime = 0;
        public double totalFiringTime = 0;
        [NonSerialized]
        public int score = 0;
        public int totalScore = 0;
        [NonSerialized]
        public bool isFiring = false;
        [NonSerialized]
        public int shotsFired = 0;
        public int totalShotsFired = 0;

        //powerups
        [NonSerialized]
        public int shieldCount = 0;
        public int totalShieldCount = 0;
        [NonSerialized]
        public int areaBombCount = 0;
        public int totalAreaBombCount = 0;
        [NonSerialized]
        public int clearCount = 0;
        public int totalClearCount = 0;
        [NonSerialized]
        public int allyCount = 0;
        public int totalAllyCount = 0;
        [NonSerialized]
        public int damageBoostCount = 0;
        public int totalDamageBoostCount = 0;

        //currency
        [NonSerialized]
        public int currencyCollected = 0;
        public int totalCurrencyCollected = 0;

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
            String output = dateTime + "\n" +
                      "Waves Finished: " + waves + "\n" +
                      "Total Score: " + totalScore + "\n" +
                      "Total Enemies Killed: " + totalEnemiesKilled + "\n" +
                      "Total Time Holding Down Trigger: " + totalFiringTime + "\n" +
                      "Total Shots Fired: " + totalShotsFired + "\n" +
                      "Total Time Alive: " + totalAliveTime + "\n" +
                      "Total Currency Collected: " + totalCurrencyCollected + "\n" +
                      "Total Shields Used: " + totalShieldCount + "\n" +
                      "Total Area Bombs Used: " + totalAreaBombCount + "\n" +
                      "Total Clears Used: " + totalClearCount + "\n" +
                      "Total Allies Used: " + totalAllyCount + "\n" +
                      "Total Damage Boosts Used: " + totalDamageBoostCount;

            Debug.Log(output);

            File.AppendAllText(@"PlayerStatsJson.txt", JsonUtility.ToJson(this, true) + "\n\n");
        }

        /// <summary>
        /// Output player wave data to console, reset wave values and add to run values.
        /// </summary>
        public void PrintWaveStats()
        {
            firingTime = (float)Math.Round(firingTime, 2);
            aliveTime = (float)Math.Round(aliveTime, 2);

            String output = DateTime.Now.ToString() + "\n" +
                      "Wave: " + waves + "\n" +
                      "Current Score: " + score + "\n" +
                      "Enemies Killed: " + enemiesKilled + "\n" +
                      "Time Holding Down Trigger: " + firingTime + "\n" +
                      "Shots Fired: " + shotsFired + "\n" +
                      "Time Alive: " + aliveTime + "\n" +
                      "Currency Collected: " + currencyCollected + "\n" +
                      "Shields Used: " + shieldCount + "\n" +
                      "Area Bombs Used: " + areaBombCount + "\n" +
                      "Clears Used: " + clearCount + "\n" +
                      "Allies Used: " + allyCount + "\n" +
                      "Damage Boosts Used: " + damageBoostCount;

            Debug.Log(output);

            totalEnemiesKilled += enemiesKilled;
            enemiesKilled = 0;
            totalFiringTime += firingTime;
            firingTime = 0;
            totalScore += score;
            score = 0;
            totalShotsFired += shotsFired;
            shotsFired = 0;
            totalShieldCount += shieldCount;
            shieldCount = 0;
            totalAreaBombCount += areaBombCount;
            areaBombCount = 0;
            totalClearCount += clearCount;
            clearCount = 0;
            totalAllyCount += allyCount;
            allyCount = 0;
            totalDamageBoostCount += damageBoostCount;
            damageBoostCount = 0;
            totalCurrencyCollected += currencyCollected;
            currencyCollected = 0;
            totalAliveTime += aliveTime;
            aliveTime = 0;
            isAlive = false;
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
        /// Add to shieldCount
        /// </summary>
        public void ShieldCount()
        {
            shieldCount++;
        }

        /// <summary>
        /// Add to areaBombCount
        /// </summary>
        public void AreaBombCount()
        {
            areaBombCount++;
        }

        /// <summary>
        /// Add to clearCount
        /// </summary>
        public void ClearCount()
        {
            clearCount++;
        }

        /// <summary>
        /// Add to allyCount
        /// </summary>
        public void AllyCount()
        {
            allyCount++;
        }

        /// <summary>
        /// Add to damageBoostCount
        /// </summary>
        public void DamageBoostCount()
        {
            damageBoostCount++;
        }

        /// <summary>
        /// Add to shotsFired
        /// </summary>
        /// <param name="shots"></param>
        public void ShotsFired(int shots)
        {
            shotsFired += shots;
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