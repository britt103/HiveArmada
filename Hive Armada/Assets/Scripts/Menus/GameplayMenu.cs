//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// GameplayMenu controls interactions with the Gameplay Menu.
//
//=============================================================================

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hive.Armada.Game;

namespace Hive.Armada.Menus
{
    /// <summary>
    /// Controls interactions with Gameplay Menu.
    /// </summary>
    public class GameplayMenu : MonoBehaviour
    {
        /// <summary>
        /// Reference to Menu Transition Manager.
        /// </summary>
        public MenuTransitionManager transitionManager;

        /// <summary>
        /// Reference to menu to go to when back is pressed.
        /// </summary>
        public GameObject backMenuGO;

        /// <summary>
        /// Reference to Menu Audio source.
        /// </summary>
		public AudioSource source;

        /// <summary>
        /// Reference to Aim Assist Toggle.
        /// </summary>
        public Toggle aimAssistToggle;

        /// <summary>
        /// Reference to Score Display Toggle.
        /// </summary>
        public Toggle scoreDisplayToggle;

        /// <summary>
        /// Reference to none toggle.
        /// </summary>
        public Toggle noPowerupsToggle;

        /// <summary>
        /// Reference to shieldToggle.
        /// </summary>
        public Toggle shieldToggle;

        /// <summary>
        /// Reference to areaBOmbToggle.
        /// </summary>
        public Toggle areaBombToggle;

        /// <summary>
        /// Reference to damageBoostToggle.
        /// </summary>
        public Toggle damageBoostToggle;

        /// <summary>
        /// Reference to allyToggle.
        /// </summary>
        public Toggle allyToggle;

        /// <summary>
        /// Reference to clearBombToggle.
        /// </summary>
        public Toggle clearBombToggle;

        /// <summary>
        /// Reference to slowFireRateToggle.
        /// </summary>
        public Toggle slowFireRateToggle;

        /// <summary>
        /// Reference to normalFireRateToggle.
        /// </summary>
        public Toggle normalFireRateToggle;

        /// <summary>
        /// Reference to fastFireRateToggle.
        /// </summary>
        public Toggle fastFireRateToggle;

        /// <summary>
        /// Selected fire rate.
        /// </summary>
        private GameSettings.FireRate selectedFireRate;

        /// <summary>
        /// Reference to Reference Manager.
        /// </summary>
        private ReferenceManager reference;

        /// <summary>
        /// State of whether toggle assignments are currently being made.
        /// </summary>
        private bool assigning;

        /// <summary>
        /// Find references. 
        /// </summary>
        private void Awake()
        {
            reference = FindObjectOfType<ReferenceManager>();

            assigning = true;

            aimAssistToggle.isOn = reference.optionsValues.aimAssist;
            scoreDisplayToggle.isOn = reference.optionsValues.scoreDisplay;
            shieldToggle.isOn = reference.optionsValues.shield;
            areaBombToggle.isOn = reference.optionsValues.areaBomb;
            damageBoostToggle.isOn = reference.optionsValues.damageBoost;
            allyToggle.isOn = reference.optionsValues.ally;
            clearBombToggle.isOn = reference.optionsValues.clearBomb;

            if (!shieldToggle.isOn && !areaBombToggle.isOn && !damageBoostToggle.isOn 
                && !allyToggle.isOn && !clearBombToggle.isOn)
            {
                noPowerupsToggle.isOn = true;
            }
            
            switch (reference.optionsValues.fireRate)
            {
                case GameSettings.FireRate.Slow:
                    slowFireRateToggle.isOn = true;
                    normalFireRateToggle.isOn = false;
                    fastFireRateToggle.isOn = false;
                    selectedFireRate = GameSettings.FireRate.Slow;
                    break;
                case GameSettings.FireRate.Normal:
                    slowFireRateToggle.isOn = false;
                    normalFireRateToggle.isOn = true;
                    fastFireRateToggle.isOn = false;
                    selectedFireRate = GameSettings.FireRate.Normal;
                    break;
                case GameSettings.FireRate.Fast:
                    slowFireRateToggle.isOn = false;
                    normalFireRateToggle.isOn = false;
                    fastFireRateToggle.isOn = true;
                    selectedFireRate = GameSettings.FireRate.Fast;
                    break;
            }

            assigning = false;
        }

        /// <summary>
        /// Back button pressed. Set selected powerups. Navigate to Options Menu.
        /// </summary>
        public void PressBack()
        {
            source.PlayOneShot(reference.menuSounds.menuButtonSelectSound);
            reference.optionsValues.SetSelectedPowerups(shieldToggle.isOn, areaBombToggle.isOn,
                damageBoostToggle.isOn, allyToggle.isOn, clearBombToggle.isOn);
            reference.optionsValues.SetFireRate(selectedFireRate);
            reference.optionsValues.SetGameplayPlayerPrefs();
            transitionManager.Transition(backMenuGO);
        }

        /// <summary>
        /// Change aimAssist setting based on aimAssistToggle value.
        /// </summary>
        public void SetAimAssist(bool isOn)
        {
            source.PlayOneShot(reference.menuSounds.menuButtonSelectSound);
            reference.optionsValues.SetAimAssist(isOn);
        }

        /// <summary>
        /// Change score setting based on scoreDisplayToggle value.
        /// </summary>
        public void SetScoreDisplay(bool isOn)
        {
            source.PlayOneShot(reference.menuSounds.menuButtonSelectSound);
            reference.optionsValues.SetScoreDisplay(isOn);
        }

        /// <summary>
        /// Play sound effect for pressing toggle. Meant for toggle presses without additional
        /// code required.
        /// </summary>
        /// <param name="isOn"></param>
        public void SetPowerup(bool isOn)
        {
            if (!assigning)
            {
                assigning = true;

                source.PlayOneShot(reference.menuSounds.menuButtonSelectSound);

                if (isOn)
                {
                    noPowerupsToggle.isOn = false;
                }
                else if (!shieldToggle.isOn && !areaBombToggle.isOn && !damageBoostToggle.isOn
                    && !allyToggle.isOn && !clearBombToggle.isOn)
                {
                    noPowerupsToggle.isOn = true;
                }

                assigning = false;
            }
        }

        /// <summary>
        /// Change fireRate setting based on fire rate toggle values.
        /// </summary>
        public void SetFireRate(bool isOn)
        {
            if (!assigning)
            {
                assigning = true;

                source.PlayOneShot(reference.menuSounds.menuButtonSelectSound);

                if (!slowFireRateToggle.isOn && !normalFireRateToggle.isOn
                    && !fastFireRateToggle.isOn)
                {
                    switch (selectedFireRate)
                    {
                        case GameSettings.FireRate.Slow:
                            slowFireRateToggle.isOn = true;
                            break;
                        case GameSettings.FireRate.Normal:
                            normalFireRateToggle.isOn = true;
                            break;
                        case GameSettings.FireRate.Fast:
                            fastFireRateToggle.isOn = true;
                            break;
                    }
                }

                if (slowFireRateToggle.isOn && selectedFireRate != GameSettings.FireRate.Slow)
                {
                    slowFireRateToggle.isOn = true;
                    normalFireRateToggle.isOn = false;
                    fastFireRateToggle.isOn = false;
                    selectedFireRate = GameSettings.FireRate.Slow;
                }
                else if (normalFireRateToggle.isOn && selectedFireRate != GameSettings.FireRate.Normal)
                {
                    slowFireRateToggle.isOn = false;
                    normalFireRateToggle.isOn = true;
                    fastFireRateToggle.isOn = false;
                    selectedFireRate = GameSettings.FireRate.Normal;
                }
                else if (fastFireRateToggle.isOn && selectedFireRate != GameSettings.FireRate.Fast)
                {
                    slowFireRateToggle.isOn = false;
                    normalFireRateToggle.isOn = false;
                    fastFireRateToggle.isOn = true;
                    selectedFireRate = GameSettings.FireRate.Fast;
                }

                assigning = false;
            }
        }

        /// <summary>
        /// Remove all powerups from selected powerups pool.
        /// </summary>
        /// <param name="isOn">State of whether toggle isOn.</param>
        public void PressPowerupsNone(bool isOn)
        {
            if (!assigning)
            {
                assigning = true;

                source.PlayOneShot(reference.menuSounds.menuButtonSelectSound);

                if (isOn)
                {
                    shieldToggle.isOn = false;
                    areaBombToggle.isOn = false;
                    damageBoostToggle.isOn = false;
                    allyToggle.isOn = false;
                    clearBombToggle.isOn = false;
                }
                else
                {
                    shieldToggle.isOn = true;
                    areaBombToggle.isOn = true;
                    damageBoostToggle.isOn = true;
                    allyToggle.isOn = true;
                    clearBombToggle.isOn = true;
                }

                assigning = false;
            }
        }
    }
}
