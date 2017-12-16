//=============================================================================
// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
// 
// This class contains the basic scoring system. When enemies are killed, they
// add their score to the player's current score here.
// 
//=============================================================================

using UnityEngine;

namespace Hive.Armada.Game
{
    /// <summary>
    /// Handles the scoring system in the game.
    /// </summary>
    public class ScoringSystem : MonoBehaviour
    {
        /// <summary>
        /// The player's current score.
        /// </summary>
        private int score;

        /// <summary>
        /// Adds points to the player's score.
        /// </summary>
        /// <param name="points"> Number of points to add </param>
        public void AddScore(int points)
        {
            score += points;
        }

        /// <summary>
        /// Get's the player's current score.
        /// </summary>
        /// <returns> The player's score integer </returns>
        public int GetScore()
        {
            return score;
        }
    }
}
