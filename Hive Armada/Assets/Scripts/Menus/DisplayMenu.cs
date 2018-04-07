//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// DisplayMenu controls interactions with the Display Menu.
//
//=============================================================================

using System;
using UnityEngine;
using UnityEngine.UI;
using Hive.Armada.Game;

namespace Hive.Armada.Menus
{
    /// <summary>
    /// Controls interactions with Display Menu.
    /// </summary>
    public class DisplayMenu : MonoBehaviour
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
        /// Reference to Bloom Toggle.
        /// </summary>
        public Toggle bloomToggle;

        /// <summary>
        /// Reference to Color Blind Mode Toggle.
        /// </summary>
        public Toggle colorBlindModeToggle;

        /// <summary>
        /// Represents a color blind mode button GO and its associated ColorBlindMode.Mode.
        /// </summary>
        [Serializable]
        public struct ColorBlindModeButton
        {
            public ColorBlindMode.Mode mode;
            public GameObject buttonGO;
        }

        /// <summary>
        /// References to color blind mode buttons.
        /// </summary>
        public ColorBlindModeButton[] colorBlindModeButtons;

        /// <summary>
        /// Reference to Reference Manager.
        /// </summary>
        private ReferenceManager reference;

        /// <summary>
        /// Variables used to make sure that audio
        /// doesn't play over itself
        /// </summary>
        private int backCounter = 0;

        private int bloomCounter = 0;

        private int colorBlindModeCounter = 0;

        /// <summary>
        /// Find references. 
        /// </summary>
        private void Awake()
        {
            reference = FindObjectOfType<ReferenceManager>();
            bloomToggle.isOn = reference.optionsValues.bloom;

            if (reference.optionsValues.colorBlindMode == ColorBlindMode.Mode.Standard)
            {
                colorBlindModeToggle.isOn = false;
                foreach (ColorBlindModeButton button in colorBlindModeButtons)
                {
                    button.buttonGO.SetActive(false);
                }
            }
            else
            {
                colorBlindModeToggle.isOn = true;
                foreach (ColorBlindModeButton button in colorBlindModeButtons)
                {
                    button.buttonGO.SetActive(true);
                    if (button.mode == reference.optionsValues.colorBlindMode)
                    {
                        button.buttonGO.GetComponent<UIHover>().Select();
                    }
                }
            }
        }

        /// <summary>
        /// Back button pressed. Navigate to Options Menu.
        /// </summary>
        public void PressBack()
        {
            source.PlayOneShot(reference.menuSounds.menuButtonSelectSound);
            reference.optionsValues.SetDisplayPlayerPrefs();
            transitionManager.Transition(backMenuGO);
        }

        /// <summary>
        /// Change bloom setting based on bloomToggle value.
        /// </summary>
        public void SetBloom(bool isOn)
        {
            source.PlayOneShot(reference.menuSounds.menuButtonSelectSound);
            reference.optionsValues.SetBloom(isOn);
        }

        /// <summary>
        /// Toggle whether color blind mode is active (using a non-standard mode).
        /// </summary>
        /// <param name="isOn"></param>
        public void ToggleColorBlindMode(bool isOn)
        {
            if (isOn)
            {
                foreach (ColorBlindModeButton button in colorBlindModeButtons)
                {
                    button.buttonGO.SetActive(true);
                    button.buttonGO.GetComponent<UIHover>().EndSelect();
                }

                SetColorBlindMode(ColorBlindMode.Mode.Standard);
            }
            else
            {
                foreach (ColorBlindModeButton button in colorBlindModeButtons)
                {
                    button.buttonGO.SetActive(false);
                    colorBlindModeButtons[0].buttonGO.GetComponent<UIHover>().Select();
                }

                SetColorBlindMode(colorBlindModeButtons[0].mode);
            }
        }

        /// <summary>
        /// Change color blind mode setting.
        /// </summary>
        public void SetColorBlindMode(ColorBlindMode.Mode mode)
        {
            source.PlayOneShot(reference.menuSounds.menuButtonSelectSound);
            reference.optionsValues.SetColorBlindMode(mode);
        }
    }
}
