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
        /// Reference to volume slider
        /// </summary>
        public Slider volumeSlider;

        /// <summary>
        /// Reference to Reference Manager.
        /// </summary>
        private ReferenceManager reference;

	    public AudioSource source;
    	public AudioClip[] clips;

        /// <summary>
        /// Set default volume slider level.
        /// </summary>
        private void Awake()
        {
            reference = FindObjectOfType<ReferenceManager>();
            volumeSlider.value = reference.optionsValues.masterVolume;
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

        /// <summary>
        /// Change AudioListener volume based on volumeSlider value.
        /// </summary>
        public void AdjustVolume(float value)
        {
            reference.optionsValues.SetMasterVolume(value);
        }
    }
}
