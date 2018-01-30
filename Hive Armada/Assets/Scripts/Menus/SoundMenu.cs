﻿//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// SoundMenu controls interactions with the Sound Menu.
//
//=============================================================================

using UnityEngine;
using UnityEngine.UI;
using Hive.Armada.Game;

namespace Hive.Armada.Menus
{
    /// <summary>
    /// Controls interactions with Sound Menu.
    /// </summary>
    public class SoundMenu : MonoBehaviour
    {
        /// <summary>
        /// Reference to Menu Audio source.
        /// </summary>
        public AudioSource source;

        /// <summary>
        /// Clips to use with source.
        /// </summary>
    	public AudioClip[] clips;

        /// <summary>
        /// Reference to master volume slider
        /// </summary>
        public Slider masterVolumeSlider;

        /// <summary>
        /// Reference to music volume slider
        /// </summary>
        public Slider musicVolumeSlider;

        /// <summary>
        /// Reference to fx volume slider
        /// </summary>
        public Slider fxVolumeSlider;

        /// <summary>
        /// Reference to Reference Manager.
        /// </summary>
        private ReferenceManager reference;

        /// <summary>
        /// Set default volume slider level.
        /// </summary>
        private void Awake()
        {
            reference = FindObjectOfType<ReferenceManager>();
            masterVolumeSlider.value = reference.optionsValues.masterVolume;
            musicVolumeSlider.value = reference.optionsValues.musicVolume;
            fxVolumeSlider.value = reference.optionsValues.fxVolume;
        }

        /// <summary>
        /// Back button pressed. Navigate to Options Menu.
        /// </summary>
        public void PressBack()
        {
			source.PlayOneShot(clips[0]);
            reference.optionsValues.SetSoundPlayerPrefs();
            GameObject.Find("Main Canvas").transform.Find("Options Menu").gameObject
                    .SetActive(true);
            gameObject.SetActive(false);
        }

        //hover sound is flash-element
        //select sound is menuselect

        /// <summary>
        /// Change AudioListener volume based on masterVolumeSlider value.
        /// </summary>
        public void AdjustMasterVolume(float value)
        {
            reference.optionsValues.SetMasterVolume(value);
        }

        /// <summary>
        /// Change music volume based on musicVolumeSlider value.
        /// </summary>
        public void AdjustMusicVolume(float value)
        {
            reference.optionsValues.SetMusicVolume(value);
        }

        /// <summary>
        /// Change fx volume based on fxVolumeSlider value.
        /// </summary>
        public void AdjustFXVolume(float value)
        {
            reference.optionsValues.SetFXVolume(value);
        }
    }
}
