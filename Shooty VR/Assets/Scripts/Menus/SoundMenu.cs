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

namespace Hive.Armada.Menus { 
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
        /// Set default volume level.
        /// </summary>
        private void Awake()
        {
            volumeSlider.value = AudioListener.volume;
        }

        /// <summary>
        /// Back button pressed. Navigate to Options Menu.
        /// </summary>
        public void PressBack()
        {
            GameObject.Find("Main Canvas").transform.Find("Options Menu").gameObject
                    .SetActive(true);
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Change AudioListener volume based on volumeSlider value.
        /// </summary>
        public void AdjustVolume(float value)
        {
            AudioListener.volume = volumeSlider.value;
        }
    }
}
