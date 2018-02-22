//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// OptionsValues saves changes made to options through options menu and 
// implements game adjustments using values for sounds, display, etc. Values
// are stored as variables and as PlayerPrefs. Note: PlayerPrefs do not 
// support booleans, so conversion is used between boolean and stored int
// values (0 = false, 1 = true).
//
//=============================================================================

using System;
using UnityEngine;
using UnityEngine.PostProcessing;

namespace Hive.Armada.Game
{
    /// <summary>
    /// Store options values.
    /// </summary>
    public class OptionsValues : MonoBehaviour
    {
        [Header("Sound")]
        /// <summary>
        /// Master volume level.
        /// </summary>
        public float masterVolume;

        /// <summary>
        /// Default master volume level.
        /// </summary>
        public float defaultMasterVolume;

        /// <summary>
        /// Music volume level.
        /// </summary>
        public float musicVolume;

        /// <summary>
        /// Default music volume level.
        /// </summary>
        public float defaultMusicVolume;

        /// <summary>
        /// Music volume level.
        /// </summary>
        public float fxVolume;

        /// <summary>
        /// Default master volume level.
        /// </summary>
        public float defaultFXVolume;

        [Header("Display")]
        /// <summary>
        /// State of whether bloom is on.
        /// </summary>
        public bool bloom;

        /// <summary>
        /// Default bloom.
        /// </summary>
        public bool defaultBloom;

        [Header("References")]
        /// <summary>
        /// Reference to audio source for scene music.
        /// </summary>
        public AudioSource musicAudioSource;

        /// <summary>
        /// References to audio sources for scene fx.
        /// </summary>
        public AudioSource[] fxAudioSources;

        /// <summary>
        /// Reference to player camera gameObject.
        /// </summary>
        public GameObject cameraGO;

        /// <summary>
        /// Find references. Get PlayerPrefs value and set game settings.
        /// </summary>
        private void Awake()
        {
            GetPlayerPrefs();
            SetGameValues();
        }

        /// <summary>
        /// Change and store value for masterVolume.
        /// </summary>
        /// <param name="masterVolume">New masterVolume value.</param>
        public void SetMasterVolume(float masterVolume)
        {
            this.masterVolume = masterVolume;
            AudioListener.volume = masterVolume;
        }

        /// <summary>
        /// Change and store value for musicVolume.
        /// </summary>
        /// <param name="musicVolume">New masterVolume value.</param>
        public void SetMusicVolume(float musicVolume)
        {
            this.musicVolume = musicVolume;
            if (musicAudioSource != null)
            {
                musicAudioSource.volume = musicVolume;
            }
        }

        /// <summary>
        /// Change and store value for FXVolume.
        /// </summary>
        /// <param name="fxVolume">New fxVolume value.</param>
        public void SetFXVolume(float fxVolume)
        {
            this.fxVolume = fxVolume;
            foreach (AudioSource fxAudioSource in fxAudioSources)
            {
                fxAudioSource.volume = fxVolume;
            }
        }

        /// <summary>
        /// Change and store value for bloom.
        /// </summary>
        /// <param name="bloom">New bloom value.</param>
        public void SetBloom(bool bloom)
        {
            this.bloom = bloom;
            cameraGO.GetComponent<PostProcessingBehaviour>().profile.bloom.enabled = bloom;
        }

        /// <summary>
        /// Set player prefs for sound values.
        /// </summary>
        public void SetSoundPlayerPrefs()
        {
            PlayerPrefs.SetFloat("masterVolume", masterVolume);
            PlayerPrefs.SetFloat("musicVolume", musicVolume);
            PlayerPrefs.SetFloat("fxVolume", fxVolume);
        }

        /// <summary>
        /// Set player prefs for display values.
        /// </summary>
        public void SetDisplayPlayerPrefs()
        {
            PlayerPrefs.SetInt("bloom", Convert.ToInt32(bloom));
        }

        /// <summary>
        /// Set all player prefs.
        /// </summary>
        public void SetPlayerPrefs()
        {
            SetSoundPlayerPrefs();
            SetDisplayPlayerPrefs();
        }

        /// <summary>
        /// Get all player prefs, or default values if prefs not yet set.
        /// </summary>
        private void GetPlayerPrefs()
        {
            //Sound
            masterVolume = PlayerPrefs.GetFloat("masterVolume", defaultMasterVolume);
            musicVolume = PlayerPrefs.GetFloat("musicVolume", defaultMusicVolume);
            fxVolume = PlayerPrefs.GetFloat("fxVolume", defaultFXVolume);

            //Display
            bloom = Convert.ToBoolean(PlayerPrefs.GetInt("bloom", Convert.ToInt32(defaultBloom)));
        }

        /// <summary>
        /// Set in-game values.
        /// </summary>
        private void SetGameValues()
        {
            //Sound
            SetMasterVolume(masterVolume);
            SetMusicVolume(musicVolume);
            SetFXVolume(fxVolume);

            //Display
            SetBloom(bloom);
        }
    }
}