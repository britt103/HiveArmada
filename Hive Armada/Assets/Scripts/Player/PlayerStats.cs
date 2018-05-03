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
// to a JSON text file. Stats are tracked for individual waves and whole runs.
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
        public double aliveTime = 0;

        /// <summary>
        /// Time player has been alive in the current run.
        /// </summary>
        public double totalAliveTime = 0;

        /// <summary>
        /// State of whether player is alive.
        /// </summary>
        [NonSerialized]
        private bool isAlive = false;

        /// <summary>
        /// Number of completed waves.
        /// </summary>
        public int waves = 0;

        /// <summary>
        /// Number of enemies killed in the current wave.
        /// </summary>
        [NonSerialized]
        public int enemiesKilled = 0;

        /// <summary>
        /// Number of enemies killed in the current run.
        /// </summary>
        public int totalEnemiesKilled = 0;

        /// <summary>
        /// Best combo achieved by player in the current run.
        /// </summary>
        public int bestCombo = 0;

        /// <summary>
        /// Time player has been firing in the current wave.
        /// </summary>
        [NonSerialized]
        public double firingTime = 0;

        /// <summary>
        /// Time player has been firing in the current run.
        /// </summary>
        public double totalFiringTime = 0;

        /// <summary>
        /// Score player has earned in the current wave.
        /// </summary>
        [NonSerialized]
        public int score = 0;

        /// <summary>
        /// Score player has earned in the current run.
        /// </summary>
        public int totalScore = 0;

        /// <summary>
        /// State of whether player won the most recent run.
        /// </summary>
        public bool won = false;

        /// <summary>
        /// State of whether player is currently firing.
        /// </summary>
        [NonSerialized]
        private bool isFiring = false;

        /// <summary>
        /// Array of the names of the weapons.
        /// </summary>
        public string[] weaponNames;

        /// <summary>
        /// Array of the number of shots fired by each weapon in the current wave.
        /// </summary>
        [NonSerialized]
        public int[] weaponShotsFired;

        /// <summary>
        /// Array of the number of shots fired by each weapon in the current run.
        /// </summary>
        public int[] weaponTotalShotsFired;

        /// <summary>
        /// Array of the names of the powerups.
        /// </summary>
        public string[] powerupNames;

        /// <summary>
        /// Array of the number of times each powerup has been used in the current wave.
        /// </summary>
        [NonSerialized]
        public int[] powerupCount;

        /// <summary>
        /// Array of the number of times each powerup has been used in the current run.
        /// </summary>
        public int[] powerupTotalCount;

        /// <summary>
        /// Amount of currency the player has collected in the current wave.
        /// </summary>
        [NonSerialized]
        public int currencyCollected = 0;

        /// <summary>
        /// Amount of currency the player has collected in the current run.
        /// </summary>
        public int totalCurrencyCollected = 0;

        /// <summary>
        /// Set initial values for weapons and powerups. Set dateTime.
        /// </summary>
        private void Awake()
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
            UpdateTotals();

            totalFiringTime = Math.Round(totalFiringTime, 2);
            totalAliveTime = Math.Round(totalAliveTime, 2);

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
                      "Best Combo: " + bestCombo + "\n" +
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
                      "Best Combo: " + bestCombo + "\n" +
                      "Time Holding Down Trigger: " + firingTime + "\n" +
                      weaponsOutput + powerupsOutput +
                      "Time Alive: " + aliveTime + "\n" +
                      "Currency Collected: " + currencyCollected + "\n";

            Debug.Log(output);

            UpdateTotals();
            ResetValues();
        }

        /// <summary>
        /// Update totals variables using current wave values.
        /// </summary>
        private void UpdateTotals()
        {
            totalEnemiesKilled += enemiesKilled;
            enemiesKilled = 0;
            totalFiringTime += firingTime;
            firingTime = 0;
            totalScore += score;
            score = 0;
            totalCurrencyCollected += currencyCollected;
            currencyCollected = 0;
            totalAliveTime += aliveTime;
            aliveTime = 0;

            for (int i = 0; i < weaponNames.Length; ++i)
            {
                weaponTotalShotsFired[i] += weaponShotsFired[i];
                weaponShotsFired[i] = 0;
            }

            for (int i = 0; i < powerupNames.Length; ++i)
            {
                powerupTotalCount[i] += powerupCount[i];
                powerupCount[i] = 0;
            }
        }

        /// <summary>
        /// Reset wave values.
        /// </summary>
        public void ResetValues()
        {
            dateTime = DateTime.Now.ToString();
            aliveTime = 0;
            isAlive = false;
            enemiesKilled = 0;
            bestCombo = 0;
            firingTime = 0;
            score = 0;
            isFiring = false;

            for (int i = 0; i < weaponShotsFired.Length; ++i)
            {
                weaponShotsFired[i] = 0;
            }

            for (int i = 0; i < powerupCount.Length; ++i)
            {
                powerupCount[i] = 0;
            }

            currencyCollected = 0;
        }

        /// <summary>
        /// Reset totals.
        /// </summary>
        public void ResetTotals()
        {
            waves = 0;
            totalAliveTime = 0;
            totalEnemiesKilled = 0;
            totalFiringTime = 0;
            totalScore = 0;

            for (int i = 0; i < weaponShotsFired.Length; ++i)
            {
                weaponTotalShotsFired[i] = 0;
            }

            for (int i = 0; i < powerupCount.Length; ++i)
            {
                powerupTotalCount[i] = 0;
            }

            totalCurrencyCollected = 0;
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
            won = false;
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
        /// Set current best combo of run
        /// </summary>
        /// <param name="combo">latest combo that ended</param>
        public void Combo(int combo)
        {
            if(combo > bestCombo)
            {
                bestCombo = combo;
            }
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
        public void AddScore(int points)
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