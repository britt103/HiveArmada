//=============================================================================
//
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// This class simply shows the player their score when the game ends.
//
//=============================================================================

using Hive.Armada.Game;
using UnityEngine;
using UnityEngine.UI;

namespace Hive.Armada.Menus
{
    /// <summary>
    /// Shows the player their score.
    /// </summary>
    public class ScoreReadout : MonoBehaviour
    {
        /// <summary>
        /// Reference manager that holds all needed references
        /// (e.g. spawner, game manager, etc.)
        /// </summary>
        private ReferenceManager reference;

        /// <summary>
        /// Text component on this game object.
        /// </summary>
        private Text text;

        /// <summary>
        /// Initializes the reference to the Reference Manager
        /// </summary>
        private void Awake()
        {
            reference = GameObject.Find("Reference Manager").GetComponent<ReferenceManager>();
            text = gameObject.GetComponent<Text>();
        }

        /// <summary>
        /// Makes the Text component show the player's score.
        /// </summary>
        private void OnEnable()
        {
            if (text)
            {
                text.text = reference.scoringSystem.GetScore().ToString();
            }
        }
    }
}
