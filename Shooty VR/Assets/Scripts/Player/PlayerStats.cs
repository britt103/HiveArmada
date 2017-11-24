//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// PlayerStats tracks various player metrics including powerup use, time alive, 
// wave completion, and score. Data is output both to the editor console and
// to a JSON text file.
//
//=============================================================================

using System;
using System.IO;
using UnityEngine;

namespace Hive.Armada.Player
{
    /// <summary>
    /// Tracks player metrics.
    /// </summary>
    public class PlayerStats : MonoBehaviour
    {
        /// <summary>
        /// String of the datetime when PlayerStats starts.
        /// </summary>
        private string dateTime;

        /// <summary>
        /// Time player has been alive in the current wave.
        /// </summary>
        [NonSerialized]
        private double aliveTime = 0;

        /// <summary>
        /// Time player has been alive in the current run.
        /// </summary>
        private double totalAliveTime = 0;

        /// <summary>
        /// State of whether player is alive.
        /// </summary>
        [NonSerialized]
        private bool isAlive = false;

        /// <summary>
        /// Number of completed waves.
        /// </summary>
        private int waves = 0;

        /// <summary>
        /// Number of enemies killed in the current wave.
        /// </summary>
        [NonSerialized]
        private int enemiesKilled = 0;

        /// <summary>
        /// Number of enemies killed in the current run.
        /// </summary>
        private int totalEnemiesKilled = 0;

        /// <summary>
        /// Time player has been firing in the current wave.
        /// </summary>
        [NonSerialized]
        private double firingTime = 0;

        /// <summary>
        /// Time player has been firing in the current run.
        /// </summary>
        private double totalFiringTime = 0;

        /// <summary>
        /// Score player has earned in the current wave.
        /// </summary>
        [NonSerialized]
        private int score = 0;

        /// <summary>
        /// Score player has earned in the current run.
        /// </summary>
        private int totalScore = 0;

        /// <summary>
        /// State of whether player is currently firing.
        /// </summary>
        [NonSerialized]
        private bool isFiring = false;

        //weapons
        //[NonSerialized]
        //public string w1Name = "";
        //[NonSerialized]
        //private int w1ShotsFired = 0;
        //private int w1TotalShotsFired = 0;
        //[NonSerialized]
        //public string w2Name = "";
        //[NonSerialized]
        //private int w2ShotsFired = 0;
        //private int w2TotalShotsFired = 0;

        /// <summary>
        /// Array of the names of the weapons.
        /// </summary>
        public string[] weaponNames;

        /// <summary>
        /// Array of the number of shots fired by each weapon in the current wave.
        /// </summary>
        [NonSerialized]
        private int[] weaponShotsFired;

        /// <summary>
        /// Array of the number of shots fired by each weapon in the current run.
        /// </summary>
        private int[] weaponTotalShotsFired;

        //powerups
        //[NonSerialized]
        //public string p1Name = "";
        //[NonSerialized]
        //private int p1Count = 0;
        //private int p1TotalCount = 0;
        //[NonSerialized]
        //public string p2Name = "";
        //[NonSerialized]
        //private int p2Count = 0;
        //private int p2TotalCount = 0;
        //[NonSerialized]
        //public string p3Name = "";
        //[NonSerialized]
        //private int p3Count = 0;
        //private int p3TotalCount = 0;
        //[NonSerialized]
        //public string p4Name = "";
        //[NonSerialized]
        //private int p4Count = 0;
        //private int p4TotalCount = 0;
        //[NonSerialized]
        //public string p5Name = "";
        //[NonSerialized]
        //private int p5Count = 0;
        //private int p5TotalCount = 0;

        /// <summary>
        /// Array of the names of the powerups.
        /// </summary>
        public string[] powerupNames;

        /// <summary>
        /// Array of the number of times each powerup has been used in the current wave.
        /// </summary>
        [NonSerialized]
        private int[] powerupCount;

        /// <summary>
        /// Array of the number of times each powerup has been used in the current run.
        /// </summary>
        private int[] powerupTotalCount;

        /// <summary>
        /// Amount of currency the player has collected in the current wave.
        /// </summary>
        [NonSerialized]
        private int currencyCollected = 0;

        /// <summary>
        /// Amount of currency the player has collected in the current run.
        /// </summary>
        private int totalCurrencyCollected = 0;

        /// <summary>
        /// Set initial values for weapons and powerups. Set dateTime.
        /// </summary>
        private void Start()
        {
            int numWeapons = weaponNames.Length;
            weaponShotsFired = new int[numWeapons];
            weaponTotalShotsFired = new int[numWeapons];
            for (int i = 0; i < weaponNames.Length; ++i)
            {
                weaponShotsFired[i] = 0;
                weaponTotalShotsFired[i] = 0;
            }

            int numPowerups = powerupNames.Length;
            powerupCount = new int[numPowerups];
            powerupTotalCount = new int[numPowerups];
            for (int i = 0; i < powerupNames.Length; ++i)
            {
                powerupCount[i] = 0;
                powerupTotalCount[i] = 0;
            }

            dateTime = DateTime.Now.ToString();
        }

        /// <summary>
        /// Add to firingTime and aliveTime when applicable.
        /// </summary>
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
        /// Append data to file when application it closed. Only does so when waves have started.
        /// </summary>
        private void OnApplicationQuit()
        {
            if(waves > 0)
            {
                PrintStats();
            }
        }

        /// <summary>
        /// Output player run data to console and to file.
        /// </summary>
        public void PrintStats()
        {
            //totalFiringTime = Math.Round(totalFiringTime, 2);
            //totalAliveTime = Math.Round(totalAliveTime, 2);

            string weaponsOutput = "";
            for(int i = 0; i < weaponNames.Length; ++i)
            {
                weaponsOutput += "Total Shots Fired w/" + weaponNames[i] + ": " + weaponTotalShotsFired[i] + "\n";
            }

            string powerupsOutput = "";
            for(int i = 0; i < powerupNames.Length; ++i)
            {
                powerupsOutput += "Total " + powerupNames[i] + " Use: " + powerupTotalCount[i] + "\n";
            }

            String output = dateTime + "\n" +
                      "Waves Finished: " + waves + "\n" +
                      "Total Score: " + totalScore + "\n" +
                      "Total Enemies Killed: " + totalEnemiesKilled + "\n" +
                      "Total Time Holding Down Trigger: " + totalFiringTime + "\n" +
                      weaponsOutput + powerupsOutput +
                      "Total Time Alive: " + totalAliveTime + "\n" +
                      "Total Currency Collected: " + totalCurrencyCollected + "\n";
   
            Debug.Log(output);

            File.AppendAllText(@"PlayerStatsJson.txt", JsonUtility.ToJson(this, true) + "\n\n");
        }

        /// <summary>
        /// Output player wave data to console. Reset wave values and add to run values.
        /// </summary>
        public void PrintWaveStats()
        {
            firingTime = Math.Round(firingTime, 2);
            aliveTime = Math.Round(aliveTime, 2);

            string weaponsOutput = "";
            for (int i = 0; i < weaponNames.Length; ++i)
            {
                weaponsOutput += "Shots Fired w/" + weaponNames[i] + ": " + weaponShotsFired[i] + "\n";
            }

            string powerupsOutput = "";
            for (int i = 0; i < powerupNames.Length; ++i)
            {
                powerupsOutput += powerupNames[i] + " Use: " + powerupCount[i] + "\n";
            }

            String output = DateTime.Now.ToString() + "\n" +
                      "Wave: " + waves + "\n" +
                      "Current Score: " + score + "\n" +
                      "Enemies Killed: " + enemiesKilled + "\n" +
                      "Time Holding Down Trigger: " + firingTime + "\n" +
                      weaponsOutput + powerupsOutput +
                      "Time Alive: " + aliveTime + "\n" +
                      "Currency Collected: " + currencyCollected + "\n";

            Debug.Log(output);

            totalEnemiesKilled += enemiesKilled;
            enemiesKilled = 0;
            totalFiringTime += firingTime;
            firingTime = 0;
            totalScore += score;
            score = 0;

            for(int i = 0; i < weaponNames.Length; ++i)
            {
                weaponTotalShotsFired[i] += weaponShotsFired[i];
                weaponShotsFired[i] = 0;
            }

            for (int i = 0; i < powerupNames.Length; ++i)
            {
                powerupTotalCount[i] += powerupCount[i];
                powerupCount[i] = 0;
            }

            //w1TotalShotsFired += w1ShotsFired;
            //w1ShotsFired = 0;
            //w2TotalShotsFired += w2ShotsFired;
            //w2ShotsFired = 0;

            //p1TotalCount += p1Count;
            //p1Count = 0;
            //p2TotalCount += p2Count;
            //p2Count = 0;
            //p3TotalCount += p3Count;
            //p3Count = 0;
            //p4TotalCount += p4Count;
            //p4Count = 0;
            //p5TotalCount += p5Count;
            //p5Count = 0;

            totalCurrencyCollected += currencyCollected;
            currencyCollected = 0;
            totalAliveTime += aliveTime;
            aliveTime = 0;
            isAlive = false;
        }

        /// <summary>
        /// Set isAlive to true
        /// </summary>
        public void IsAlive()
        {
            isAlive = true;
        }

        /// <summary>
        /// Set isAlive to false
        /// </summary>
        public void IsNotAlive()
        {
            isAlive = false;
        }

        /// <summary>
        /// Set isFiring to true
        /// </summary>
        public void IsFiring()
        {
            isFiring = true;
        }

        /// <summary>
        /// Set isFiring to false
        /// </summary>
        public void IsNotFiring()
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
        /// <param name="amount">amount of currency to add</param>
        public void CurrencyCollected(int amount)
        {
            currencyCollected += amount;
        }

        /// <summary>
        /// Add to score
        /// </summary>
        /// <param name="points">number of points to add</param>
        public void Score(int points)
        {
            score += points;
        }

        /// <summary>
        /// Record powerup use.
        /// </summary>
        /// <param name="name">name of powerup used</param>
        public void PowerupUsed(string name)
        {
            for(int i = 0; i < powerupNames.Length; ++i)
            {
                if(name == powerupNames[i])
                {
                    powerupCount[i]++;
                }
            }
        }

        ///// <summary>
        ///// Add to 
        ///// </summary>
        //public void Powerup1Used()
        //{
        //    p1Count++;
        //}

        ///// <summary>
        ///// Add to 
        ///// </summary>
        //public void P2Used()
        //{
        //    p2Count++;
        //}

        ///// <summary>
        ///// Add to 
        ///// </summary>
        //public void P3Used()
        //{
        //    p3Count++;
        //}

        ///// <summary>
        ///// Add to 
        ///// </summary>
        //public void P4Used()
        //{
        //    p4Count++;
        //}

        ///// <summary>
        ///// Add to 
        ///// </summary>
        //public void P5Used()
        //{
        //    p5Count++;
        //}

        /// <summary>
        /// Record weapon shots.
        /// </summary>
        /// <param name="name">name of the weapon fired</param>
        /// <param name="shots">number of shots fired</param>
        public void WeaponFired(string name, int shots)
        {
            for (int i = 0; i < weaponNames.Length; ++i)
            {
                if (name == weaponNames[i])
                {
                    weaponShotsFired[i] += shots;
                }
            }
        }

        ///// <summary>
        ///// Add to 
        ///// </summary>
        ///// <param name="shots"></param>
        //public void W1Fired(int shots)
        //{
        //    w1ShotsFired += shots;
        //}

        ///// <summary>
        ///// Add to 
        ///// </summary>
        ///// <param name="shots"></param>
        //public void W2Fired(int shots)
        //{
        //    w2ShotsFired += shots;
        //}

        /// <summary>
        /// Increment waves. Print wave data to console.
        /// </summary>
        public void WaveComplete()
        {
            waves++;
            PrintWaveStats();
        }
    }
}