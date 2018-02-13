//=============================================================================
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
        public ReferenceManager reference;

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
        /// Current kill counter, resets combo at 5
        /// </summary>
        private int comboSequence;

        /// <summary>
        /// location to spawn the point emitter, fed in from enemy death
        /// </summary>
        private Transform deathLocation;

        public GameObject[] pointEmitters;

        /// <summary>
        /// Bool dictating whether a combo is active or not.
        /// </summary>
        private bool comboActive;

        public void Start()
        {
            comboTimer = 0;
            comboSequence = 0;
            comboMultiplier = 1;
            comboActive = false;
        }

        /// <summary>
        /// Main combo function. When the combo timer reacher zero, the bank
        /// is calculated with multiplier and sent to PlayerStats.
        /// </summary>
        public IEnumerator StartCombo()
        {
            while (comboTimer <= 0)
            {
                if (comboMultiplier >= 5) break;
                yield return new WaitForSeconds(1);
                comboTimer--;
            }
            comboActive = false;
            //comboBank *= comboMultiplier;
            int comboOut = (int)comboBank;
            AddScore(comboOut);
            comboMultiplier = 1;
            comboSequence = 0;
            comboBank = 0;
            //do point emitter stuff
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

        public void setLocation(Transform location)
        {
            this.deathLocation = location;
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
                comboMultiplier = 1;
                comboTimer = 2;
                points *= comboMultiplier;
                comboBank += points;
                comboSequence += 1;
                StartCoroutine(StartCombo());
            }
            else if (comboActive == true)
            {
                comboTimer = 2;
                comboSequence += 1;
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

        private void spawnPointEmitter(int points)
        {
            GameObject mEmitter = null;
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
            Instantiate(mEmitter, deathLocation.position, deathLocation.rotation);
        }
    }
}
