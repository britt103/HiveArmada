//=============================================================================
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
        /// Reference to dialogue volume slider
        /// </summary>
        public Slider dialogueVolumeSlider;

        public Text masterVolume;

        public Text musicVolume;

        public Text effectsVolume;

        public Text dialogueVolume;

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
            dialogueVolumeSlider.value = reference.optionsValues.dialogueVolume;
            masterVolume.text = string.Format("{0:0%}", masterVolumeSlider.value);
            musicVolume.text = string.Format("{0:0%}", musicVolumeSlider.value);
            effectsVolume.text = string.Format("{0:0%}", fxVolumeSlider.value);
            dialogueVolume.text = string.Format("{0:0%}", dialogueVolumeSlider.value);
        }

        /// <summary>
        /// Back button pressed. Navigate to Options Menu.
        /// </summary>
        public void PressBack()
        {
            source.PlayOneShot(reference.menuSounds.menuButtonSelectSound);
            reference.optionsValues.SetSoundPlayerPrefs();
            transitionManager.Transition(backMenuGO);
        }


        /// <summary>
        /// Change AudioListener volume based on masterVolumeSlider value.
        /// </summary>
        public void AdjustMasterVolume(float value)
        {
            reference.optionsValues.SetMasterVolume(value);
            masterVolume.text = string.Format("{0:0%}", value);
        }

        /// <summary>
        /// Change music volume based on musicVolumeSlider value.
        /// </summary>
        public void AdjustMusicVolume(float value)
        {
            reference.optionsValues.SetMusicVolume(value);
            musicVolume.text = string.Format("{0:0%}", value);
        }

        /// <summary>
        /// Change fx volume based on fxVolumeSlider value.
        /// </summary>
        public void AdjustFXVolume(float value)
        {
            reference.optionsValues.SetFXVolume(value);
            effectsVolume.text = string.Format("{0:0%}", value);
        }

        /// <summary>
        /// Change fx volume based on dialogueVolumeSlider value.
        /// </summary>
        public void AdjustDialogueVolume(float value)
        {
            reference.optionsValues.SetDialogueVolume(value);
            dialogueVolume.text = string.Format("{0:0%}", value);
        }
    }
}
