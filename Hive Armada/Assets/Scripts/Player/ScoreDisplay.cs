//=============================================================================
// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-440-01
// Group Project
// 
// This class displays the player's score on a canvas below the ship.
// 
//=============================================================================

using UnityEngine;
using UnityEngine.UI;
using Hive.Armada.Game;
using System;

namespace Hive.Armada.Player
{
    /// <summary>
    /// The player score display for under the ship.
    /// </summary>
    public class ScoreDisplay : MonoBehaviour
    {
        /// <summary>
        /// Reference manager that holds all needed references
        /// (e.g. spawner, game manager, etc.)
        /// </summary>
        private ReferenceManager reference;

        /// <summary>
        /// Where the canvas should face.
        /// </summary>
        private GameObject playerLookTarget;

        /// <summary>
        /// The canvas with all of the score display UI on it.
        /// </summary>
        public Canvas displayCanvas;

        /// <summary>
        /// The game object for the display canvas.
        /// </summary>
        private GameObject displayCanvasObject;

        /// <summary>
        /// The 
        /// </summary>
        public Text scoreText;

        /// <summary>
        /// 
        /// </summary>
        public Text multiplierText;

        /// <summary>
        /// The color for the x1 multiplier text.
        /// </summary>
        [Tooltip("The color for the x1 multiplier text." +
            "\nDefault: #FFFFFFFF")]
        public Color x1Color;

        /// <summary>
        /// The color for the x5 multiplier text.
        /// </summary>
        [Tooltip("The color for the x5 multiplier text." +
            "\nDefault: #A700FFFF")]
        public Color x5Color;

        /// <summary>
        /// The color for the x10 multiplier text.
        /// </summary>
        [Tooltip("The color for the x10 multiplier text." +
            "\nDefault: #3A21F8FF")]
        public Color x10Color;

        /// <summary>
        /// The internal score value.
        /// </summary>
        private int score = 0;

        /// <summary>
        /// The internal multiplier value.
        /// </summary>
        private int multiplier = 1;

        /// <summary>
        /// The maximum score that will 
        /// </summary>
        private int maxDisplayableScore = 99999999;

        /// <summary>
        /// Initializes the score display.
        /// </summary>
        private void Awake()
        {
            reference = FindObjectOfType<ReferenceManager>();

            if (reference != null)
            {
                if (displayCanvas != null)
                {
                    if (reference.gameSettings.scoreDisplay)
                    {
                        displayCanvasObject = displayCanvas.gameObject;
                        playerLookTarget = reference.playerLookTarget;

                        if (scoreText != null)
                        {
                            SetScore(0);
                        }
                        else
                        {
                            Debug.LogError(GetType().Name + " - scoreText is null.");
                        }

                        if (multiplierText != null)
                        {
                            SetMultiplier(1);
                        }
                        else
                        {
                            Debug.LogError(GetType().Name + " - multiplierText is null.");
                        }

                        reference.scoringSystem.SetScoreDisplay(this);
                    }
                    else
                    {
                        displayCanvas.enabled = false;
                        this.enabled = false;
                    }
                }
                else
                {
                    Debug.LogError(GetType().Name + " - scoreCanvas is null.");
                }
            }
        }

        /// <summary>
        /// Makes the display canvas face the HMD.
        /// </summary>
        private void Update()
        {
            displayCanvasObject.transform.rotation =
                Quaternion.LookRotation(displayCanvasObject.transform.position -
                                        playerLookTarget.transform.position);
        }

        /// <summary>
        /// Adds to the score.
        /// </summary>
        /// <param name="newScore"> How much score to add </param>
        public void AddScore(int newScore)
        {
            score = Math.Min(score + newScore, 99999999);
            UpdateScoreText();
        }

        /// <summary>
        /// Sets the score to a value.
        /// </summary>
        /// <param name="newScore"> The new score value </param>
        public void SetScore(int newScore)
        {
            score = Math.Min(newScore, 99999999);
            UpdateScoreText();
        }

        /// <summary>
        /// Sets the multiplier value.
        /// </summary>
        /// <param name="newMultiplier">  </param>
        public void SetMultiplier(int newMultiplier)
        {
            multiplier = newMultiplier;

            Color color;
            switch (multiplier)
            {
                case 1:
                    color = x1Color;
                    break;
                case 5:
                    color = x5Color;
                    break;
                case 10:
                    color = x10Color;
                    break;
                default:
                    Debug.LogWarning(GetType().Name + " - New multiplier is not 1, 5 or 10.");
                    color = x1Color;
                    break;
            }

            UpdateMultiplierText(color);
        }

        /// <summary>
        /// Updates the score text display.
        /// </summary>
        private void UpdateScoreText()
        {
            scoreText.text = string.Format("{0:n0}", score);
            //score.ToString("{0:n0}")
        }

        private void UpdateMultiplierText(Color color)
        {
            multiplierText.text = "X" + multiplier.ToString();
            multiplierText.color = color;
        }
    }
}