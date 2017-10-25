//Name: Chad Johnson
//Student ID: 1763718
//Email: johns428@mail.chapman.edu
//Course: CPSC 340-01, CPSC-344-01
//Assignment: Group Project
//Purpose: Tracks player statistics

//Reference required in: PowerUpStatus, Enemy, LaserGun, ...

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Valve.VR.InteractionSystem;

namespace Hive.Armada
{
    public class PlayerStats : MonoBehaviour
    {
        //combat
        public int enemiesKilled = 0;
        public int totalEnemiesKilled = 0;
        public float firingTime = 0.0F;
        public float totalFiringTime = 0.0f;
        public int score = 0;
        public int totalScore = 0;
        public bool isFiring = false;
        public int shotsFired = 0;
        public int totalShotsFired = 0;

        //powerups
        public int shieldCount = 0;
        public int totalShieldCount = 0;
        public int areaBombCount = 0;
        public int totalAreaBombCount = 0;
        public int clearCount = 0;
        public int totalClearCount = 0;
        public int allyCount = 0;
        public int totalAllyCount = 0;
        public int damageBoostCount = 0;
        public int totalDamageBoostCount = 0;

        //currency
        public int currencyCollected = 0;
        public int totalCurrencyCollected = 0;

        //time alive
        public float aliveTime = 0.0f;
        public float totalAliveTime = 0.0f;
        public bool isAlive = false;
        public int waves = 0;

        // Use this for initialization
        void Start()
        {

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

        private void OnApplicationQuit()
        {
            PrintStats();
        }

        public void PrintStats()
        {
            String output = DateTime.Now.ToString() + "\n" +
                      "Waves Finished: " + waves + "\n" +
                      "Total Score: " + totalScore + "\n" +
                      "Total Enemies Killed: " + totalEnemiesKilled + "\n" +
                      "Total Time Holding Down Trigger: " + Math.Round(totalFiringTime, 2) + "\n" +
                      "Total Shots Fired: " + totalShotsFired + "\n" +
                      "Total Time Alive: " + Math.Round(totalAliveTime, 2) + "\n" +
                      "Total Currency Collected: " + totalCurrencyCollected + "\n" +
                      "Total Shields Used: " + totalShieldCount + "\n" +
                      "Total Area Bombs Used: " + totalAreaBombCount + "\n" +
                      "Total Clears Used: " + totalClearCount + "\n" +
                      "Total Allies Used: " + totalAllyCount + "\n" +
                      "Total Damage Boosts Used: " + totalDamageBoostCount;

            Debug.Log(output);

            File.AppendAllText(@"PlayerStats.txt", output + "\n\n");
        }

        public void PrintWaveStats()
        {
            String output = DateTime.Now.ToString() + "\n" +
                      "Wave: " + waves + "\n" +
                      "Current Score: " + score + "\n" +
                      "Enemies Killed: " + enemiesKilled + "\n" +
                      "Time Holding Down Trigger: " + Math.Round(firingTime, 2) + "\n" +
                      "Shots Fired: " + shotsFired + "\n" +
                      "Time Alive: " + Math.Round(aliveTime, 2) + "\n" +
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
            firingTime = 0.0f;
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
            aliveTime = 0.0f;
            isAlive = false;
        }

        public void EnemyKilled()
        {
            enemiesKilled++;
        }

        public void CurrencyCollected(int amount)
        {
            currencyCollected += amount;
        }

        public void Score(int points)
        {
            score += points;
        }

        public void ShieldCount()
        {
            shieldCount++;
        }

        public void AreaBombCount()
        {
            areaBombCount++;
        }

        public void ClearCount()
        {
            clearCount++;
        }

        public void AllyCount()
        {
            allyCount++;
        }
        public void DamageBoostCount()
        {
            damageBoostCount++;
        }

        public void ShotsFired(int shots)
        {
            shotsFired += shots;
        }

        public void WaveComplete()
        {
            waves++;
            PrintWaveStats();
        }
    }

}