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
        public float firingTime = 0.0F;
        public int score = 0;
        public bool isFiring = false;

        //powerups
        public int shieldCount = 0;
        public int areaBombCount = 0;
        public int clearCount = 0;
        public int allyCount = 0;

        //currency
        public int currencyCollected = 0;

        //hands
        //private Hand[] hands;

        //time alive

        // Use this for initialization
        void Start()
        {
            //Debug.Log(gameObject.GetComponentInChildren<Hand>().gameObject.name);
            //hands = gameObject.GetComponentsInChildren<Hand>();
            //Debug.Log(hands[1].gameObject.name);
            //Debug.Log(hands[0].gameObject.name);
            //Debug.Log(hands[1].controller.GetHairTriggerDown());
        }

        // Update is called once per frame
        void Update()
        {
            if (isFiring)
            {
                firingTime += Time.deltaTime;
            }
            isFiring = false;
        }

        private void OnApplicationQuit()
        {
            printStats();
        }

        void printStats()
        {
            String output = DateTime.Now.ToString() + "\n" + 
                      "Current Score: " + score + "\n" +
                      "Enemies Killed: " + enemiesKilled + "\n" +
                      "Time Holding Down Trigger: " + Math.Round(firingTime, 2) + "\n" +
                      "Currency Collected: " + currencyCollected + "\n" +
                      "Shields Used: " + shieldCount + "\n" +
                      "Area Bombs Used: " + areaBombCount + "\n" +
                      "Clears Used: " + clearCount + "\n" +
                      "Allies Used: " + allyCount;

            Debug.Log(output);

            File.AppendAllText(@"PlayerStats.txt", output + "\n\n");
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