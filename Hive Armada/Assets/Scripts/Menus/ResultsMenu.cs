//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// Results Menu displays the results of the the just completed run to the 
// player.
//
//=============================================================================

using System;
using UnityEngine;
using UnityEngine.UI;
using Hive.Armada.Game;
using Hive.Armada.Player;

namespace Hive.Armada.Menus
{
    /// <summary>
    /// Display run results to player.
    /// </summary>
    public class ResultsMenu : MonoBehaviour
    {
        /// <summary>
        /// Reference to Menu Transition Manager.
        /// </summary>
        public MenuTransitionManager transitionManager;

        /// <summary>
        /// Reference to menu to go to when back is pressed.
        /// </summary>
        public GameObject nextMenu;

        /// <summary>
        /// Reference to scroll view content.
        /// </summary>
        public GameObject scrollViewContent;

        /// <summary>
        /// Reference to vertical scrollbar.
        /// </summary>
        public Scrollbar scrollbar;

        /// <summary>
        /// Reference to vertical slider.
        /// </summary>
        public Slider verticalSlider;

        /// <summary>
        /// Reference to Menu Audio source.
        /// </summary>
        public AudioSource source;

        /// <summary>
        /// reference to zena audio.
        /// </summary>
        public AudioSource zenaSource;

        /// <summary>
        /// Refernece to IridiumSystem.
        /// </summary>
        private IridiumSystem iridiumSystem;

        /// <summary>
        /// Percent of score used to calculate Iridium gain.
        /// </summary>
        public float iridiumScoreMultiplier = 0.2f;

        /// <summary>
        /// Reference to PlayerStats.
        /// </summary>
        private PlayerStats stats;

        /// <summary>
        /// Reference to Text GameObjecft for victory/defeat.
        /// </summary>
        public Text victoryDefeatText;

        /// <summary>
        /// Message to display if player wins.
        /// </summary>
        public string victoryMessage;

        /// <summary>
        /// Color to apply to victory text.
        /// </summary>
        public Color victoryColor;

        /// <summary>
        /// Message to display if player loses.
        /// </summary>
        public string defeatMessage;

        /// <summary>
        /// Color to apply to defeat text.
        /// </summary>
        public Color defeatColor;

        /// <summary>
        /// Reference to Text GameObject for score stat.
        /// </summary>
        public Text scoreText;

        public Text scoreValue;

        /// <summary>
        /// Reference to Iridium text GO.
        /// </summary>
        public Text iridiumText;

        public Text iridiumValue;

        public Text iridiumSpawnedText;

        /// <summary>
        /// Reference to Text GameObject for time stat.
        /// </summary>
        public Text timeText;

        public Text timeValue;

        public GameObject statShift;

        /// <summary>
        /// Number of cells that can fit in scroll view content without scrolling.
        /// </summary>
        public int numFittableCells;

        /// <summary>
        /// Reference to ReferenceManager.
        /// </summary>
        private ReferenceManager reference;

        /// <summary>
        /// Reference to GameSettings.
        /// </summary>
        private GameSettings gameSettings;

        /// <summary>
        /// Variable used to make sure that audio
        /// doesn't play over itself
        /// </summary>
        private int continueCounter = 0;

        [Header("Scrollview Texts and Values")]
        public Text killsText;

        public Text killsValue;

        public Text allyText;

        public Text allyValue;

        public Text areaBombText;

        public Text areaBombValue;

        public Text damageBoostText;

        public Text damageBoostValue;

        public Text shieldText;

        public Text shieldValue;

        public Text timeWarpText;

        public Text timeWarpValue;

        public Text comboScoreText;

        public Text comboScoreValue;

        public Text comboEnemiesText;

        public Text comboEnemiesValue;

        public Text damageDealtText;

        public Text damageDealtValue;

        public Text standardEnemyText;

        public Text standardEnemyValue;

        public Text buckshotEnemyText;

        public Text buckshotEnemyValue;

        public Text movingEnemyText;

        public Text movingEnemyValue;

        public Text splitterEnemyText;

        public Text splitterEnemyValue;

        public Text kamikazeEnemyText;

        public Text kamikazeEnemyValue;

        public Text bossEnemyText;

        public Text bossEnemyValue;

        public Text weaponText;

        public Text weaponValue;

        public Text enemyShotsText;

        public Text enemyShotsValue;

        /// <summary>
        /// Find references. Get and set results values. Reset stats totals.
        /// Calculate Iridium totals.
        /// </summary>
        private void Awake()
        {
            iridiumSystem = FindObjectOfType<IridiumSystem>();
            reference = FindObjectOfType<ReferenceManager>();
            gameSettings = reference.gameSettings;
            stats = reference.statistics;

            int iridiumScoreAmount = (int) (stats.totalScore * 0.2);

            if (gameSettings.selectedGameMode == GameSettings.GameMode.SoloNormal)
            {
                if (stats.won)
                {
                    victoryDefeatText.text = victoryMessage;
                    victoryDefeatText.color = victoryColor;
                    reference.talkingParticle.Speak(reference.menuSounds.victorySound[UnityEngine.Random.Range(0, reference.menuSounds.victorySound.Length)], true);
                    // zenaSource.PlayOneShot(reference.menuSounds.victorySound[UnityEngine.Random.Range(0, reference.menuSounds.victorySound.Length)]);
                }
                else
                {
                    victoryDefeatText.text = defeatMessage;
                    victoryDefeatText.color = defeatColor;
                    reference.talkingParticle.Speak(reference.menuSounds.defeatSound[UnityEngine.Random.Range(0, reference.menuSounds.defeatSound.Length)], true);
                    // zenaSource.PlayOneShot(reference.menuSounds.defeatSound[UnityEngine.Random.Range(0, reference.menuSounds.defeatSound.Length)]);
                }
            }
            else if (gameSettings.selectedGameMode == GameSettings.GameMode.SoloInfinite)
            {
                statShift.GetComponent<RectTransform>().anchoredPosition3D =
                    new Vector3(0.0f, -50.0f, 0.0f);

                victoryDefeatText.text = "Results";
                victoryDefeatText.color = victoryColor;

                TimeSpan time = TimeSpan.FromSeconds(stats.totalAliveTime);

                string format;
                if (time.Hours > 0)
                {
                    format = "H:mm:ss";
                }
                else if (time.Minutes > 9)
                {
                    format = "mm:ss";
                }
                else
                {
                    format = "m:ss";
                }

                timeText.text = "Time:";
                timeValue.text = new DateTime(time.Ticks).ToString(format);
            }

            scoreText.text = "Score:";
            scoreValue.text = string.Format("{0:n0}", stats.totalScore);

            int iridiumShootablesSpawnedAmount = iridiumSystem.GetSpawnedShootablesAmount();
            int iridiumShootablesObtainedAmount = iridiumSystem.GetObtainedShootablesAmount();

            iridiumText.text = "Iridium:";
            iridiumValue.text = string.Format("{0:n0}", iridiumScoreAmount);
            iridiumSpawnedText.text = string.Format("{0:n0} / {1:n0} total",
                                                    iridiumShootablesObtainedAmount,
                                                    iridiumShootablesSpawnedAmount);


            killsText.text = "Kills:";
            killsValue.text = stats.totalEnemiesKilled.ToString();
            allyText.text = "Allies Used:";
            allyValue.text = stats.powerupTotalCount[0].ToString();
            areaBombText.text = "Bombs Used:";
            areaBombValue.text = stats.powerupTotalCount[1].ToString();
            damageBoostText.text = "Damage Boosts Used:";
            damageBoostValue.text = stats.powerupTotalCount[2].ToString();
            shieldText.text = "Shields Used:";
            shieldValue.text = stats.powerupTotalCount[3].ToString();
            timeWarpText.text = "Time Warps Used:";
            timeWarpValue.text = stats.powerupTotalCount[4].ToString();
            comboScoreText.text = "Best Combo:";
            comboScoreValue.text = stats.bestScore.ToString();
            comboEnemiesText.text = "Enemies In Combo:";
            comboEnemiesValue.text = stats.bestCombo.ToString();
            damageDealtText.text = "Total Damage Dealt:";
            damageDealtValue.text = stats.totalDamageDealt.ToString();
            standardEnemyText.text = "Workers Killed:";
            standardEnemyValue.text = stats.enemyTotalCount[0].ToString();
            buckshotEnemyText.text = "Soldiers Killed:";
            buckshotEnemyValue.text = stats.enemyTotalCount[1].ToString();
            movingEnemyText.text = "Hunters Killed:";
            movingEnemyValue.text = stats.enemyTotalCount[2].ToString();
            splitterEnemyText.text = "Carriers Killed:";
            splitterEnemyValue.text = stats.enemyTotalCount[3].ToString();
            kamikazeEnemyText.text = "Predators Killed:";
            kamikazeEnemyValue.text = stats.enemyTotalCount[4].ToString();
            bossEnemyText.text = "Cortexes Killed:";
            bossEnemyValue.text = stats.enemyTotalCount[5].ToString();
            weaponText.text = "Weapon Used:";
            
            Debug.Log((int)reference.gameSettings.selectedWeapon);
            weaponValue.text = reference.gameSettings.weaponNames[PlayerPrefs.GetInt("defaultWeapon", 0)];
            // weaponValue.text = reference.gameSettings.weaponNames[(int)reference.gameSettings
            //     .selectedWeapon];
            enemyShotsText.text = "Enemy Shots Fired:";
            enemyShotsValue.text = stats.totalEnemyProjectiles.ToString();
            
            if (scrollViewContent.transform.childCount <= numFittableCells)
            {
                scrollbar.gameObject.GetComponent<BoxCollider>().enabled = false;
                verticalSlider.gameObject.SetActive(false);
            }
            else
            {
                scrollbar.gameObject.GetComponent<BoxCollider>().enabled = true;
                verticalSlider.gameObject.SetActive(true);
            }

            stats.ResetTotals();
            iridiumSystem.ResetShootablesAmounts();
            iridiumSystem.AddIridium(iridiumScoreAmount);
            iridiumSystem.WriteIridiumFile();
            transitionManager.currMenu = gameObject;
        }

        /// <summary>
        /// Continue button pressed; navigates to Main Menu.
        /// Reset stat values.
        /// </summary>
        public void PressContinue()
        {
            //// What is this code, Marc?
            //source.PlayOneShot(clips[0]);
            //continueCounter += 1;
            //if (continueCounter > 1)
            //{
            //    source.Stop();
            //    source.PlayOneShot(clips[0]);
            //}
            source.PlayOneShot(reference.menuSounds.menuButtonSelectSound);
            transitionManager.Transition(nextMenu);
        }
    }
}