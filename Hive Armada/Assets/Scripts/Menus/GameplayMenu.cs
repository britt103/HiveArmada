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
        /// Reference to Reference Manager.
        /// </summary>
        private ReferenceManager reference;

        /// <summary>
        /// Find references. 
        /// </summary>
        private void Awake()
        {
            reference = FindObjectOfType<ReferenceManager>();
            aimAssistToggle.isOn = reference.optionsValues.aimAssist;
            scoreDisplayToggle.isOn = reference.optionsValues.scoreDisplay;
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
    }
}
