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

namespace ShootyVR
{
    public class PlayerStats : MonoBehaviour
    {
        //combat
        public int enemiesKilled = 0;
        public float firingTime = 0.0F;
        public int score = 0;

        //powerups
        public int shieldCount = 0;
        public int areaBombCount = 0;
        public int clearCount = 0;
        public int allyCount = 0;

        //currency
        public int currencyCollected = 0;

        public ShipController controller;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (controller.isTriggerPressed)
            {
                firingTime += Time.deltaTime;
            }
        }

        private void OnApplicationQuit()
        {
            printStats();
        }

        void printStats()
        {
            Debug.Log("Current Score: " + score + "\n" +
                      "Enemies Killed: " + enemiesKilled + "\n" +
                      "Time Holding Down Trigger: " + Math.Round(firingTime, 2) + "\n" +
                      "Currency Collected: " + currencyCollected + "\n" +
                      "Shields Used: " + shieldCount + "\n" +
                      "Area Bombs Used: " + areaBombCount + "\n" +
                      "Clears Used: " + clearCount + "\n" +
                      "Allies Used: " + allyCount);
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
    }

}