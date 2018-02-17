﻿//=============================================================================
// 
// Perry Sidler & Miguel Gotao
// 1831784 - 2264941
// sidle104@mail.chapman.edu - gotao100@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
// 
// This class contains the basic scoring and combo system. When enemies are killed,
// a combo begins that, upon ending, calculates a point value that is added to the 
// player's score.
// 
//=============================================================================

using System.Collections;
using UnityEngine;

namespace Hive.Armada.Game
{
    /// <summary>
    /// Handles the scoring system in the game.
    /// </summary>
    public class ScoringSystem : MonoBehaviour
    {
        /// <summary>
        /// Reference Manager
        /// </summary>
        public ReferenceManager reference;

        /// <summary>
        /// The player's score.
        /// </summary>
        private int score;

        /// <summary>
        /// The score bank of the currect combo.
        /// </summary>
        private float comboBank;

        /// <summary>
        /// The timer of the current combo.
        /// </summary>
        private int comboTimer;

        /// <summary>
        /// Multiplier value of the current combo.
        /// </summary>
        private int comboMultiplier;

        /// <summary>
        /// Current kill counter, resets combo at 5.
        /// </summary>
        private int comboSequence;

        /// <summary>
        /// Spawn location of point emitter.
        /// </summary>
        private Transform pointLocation;

        /// <summary>
        /// Structure holding all point emitters
        /// </summary>
        public GameObject[] pointEmitters;

        /// <summary>
        /// Emitter to be spawned at enemy death
        /// </summary>
        GameObject mEmitter;

        /// <summary>
        /// Bool dictating whether a combo is active or not.
        /// </summary>
        private bool comboActive;

        public void Start()
        {
            mEmitter = null;
            comboTimer = 0;
            comboSequence = 0;
            comboMultiplier = 1;
            comboActive = false;
            pointLocation = reference.player.transform;
        }

        /// <summary>
        /// Test function for when Vive is unavailable
        /// </summary>
        public void Update()
        {
            if (Input.GetKeyDown("space"))
            {
                ComboIn(10);
                Debug.Log("Sequence: " + comboSequence);
                Debug.Log("Multiplier: " + comboMultiplier);
            }
        }

        /// <summary>
        /// Main combo function. When the combo timer reacher zero, the bank
        /// is calculated with multiplier and sent to PlayerStats.
        /// </summary>
        public IEnumerator StartCombo()
        {
            do
            {
                comboTimer--;
                if (comboTimer <= 0)
                {
                    comboActive = false;
                    break;
                }
                yield return new WaitForSeconds(1);
            }
            while (comboActive);
            //Debug.Log("Combo Death");
            //comboActive = false;
            //comboBank *= comboMultiplier;
            int comboOut = (int)comboBank;
            AddScore(comboOut);
            comboMultiplier = 1;
            comboSequence = 0;
            comboBank = 0;
        }

        /// <summary>
        /// Adds points to the player's score.
        /// </summary>
        /// <param name="points"> Number of points to add </param>
        public void AddScore(int points)
        {
            score += points;
            reference.statistics.AddScore(points);
        }

        /// <summary>
        /// Get's the player's current score.
        /// </summary>
        /// <returns> The player's score integer </returns
        public int GetScore()
        {
            return score;
        }

        /// <summary>
        /// Sets the location for which the emitter will spawn from.
        /// </summary>
        /// <param name="location"></param>
        public void setLocation(Transform location)
        {
            this.pointLocation = location;
        }

        /// <summary>
        /// Starts a new combo. If a combo is currently ongoing,
        /// then extend the current combo and add to the multiplier.
        /// </summary>
        public void ComboIn(int points)
        {
            if(comboActive == false)
            {
                comboActive = true;
                comboSequence = 1;
                comboMultiplier = 1;
                comboTimer = 3;
                points *= comboMultiplier;
                comboBank += points;
                StartCoroutine(StartCombo());
            }
            else if (comboActive == true)
            {
                comboTimer = 3;
                ++comboSequence;
                switch (comboSequence)
                {
                    case 3:
                        comboMultiplier = 5;
                        break;
                    case 5:
                        comboMultiplier = 10;
                        break;
                }
                points *= comboMultiplier;
                comboBank += points;
            }
            spawnPointEmitter(points);
        }

        /// <summary>
        /// Function that chooses the emitter to be spawned, dependent on input points.
        /// TODO: work with pooling so no need to instantiate.
        /// </summary>
        /// <param name="points"></param>
        private void spawnPointEmitter(int points)
        {
            switch (points)
            {
                case 5:
                    mEmitter = pointEmitters[0];
                    break;
                case 10:
                    mEmitter = pointEmitters[1];
                    break;
                case 20:
                    mEmitter = pointEmitters[2];
                    break;
                case 25:
                    mEmitter = pointEmitters[3];
                    break;
                case 50:
                    mEmitter = pointEmitters[4];
                    break;
                case 100:
                    mEmitter = pointEmitters[5];
                    break;
                case 200:
                    mEmitter = pointEmitters[6];
                    break;
                case 250:
                    mEmitter = pointEmitters[7];
                    break;
                case 500:
                    mEmitter = pointEmitters[8];
                    break;
            }
            Instantiate(mEmitter, pointLocation.position, pointLocation.rotation);
        }
    }
}
