//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// StartMenu controls interactions with the Start Menu.
//
//=============================================================================

using UnityEngine;
using UnityEngine.UI;
using Hive.Armada.Game;

namespace Hive.Armada.Menus
{
    /// <summary>
    /// Controls interactions with Start Menu;
    /// </summary>
    public class StartMenu : MonoBehaviour
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
        /// Reference to description game object.
        /// </summary>
        public GameObject description;

        /// <summary>
        /// Reference to continue button game object.
        /// </summary>
        public GameObject continueButton;

        /// <summary>
        /// Reference to play button game object.
        /// </summary>
        public GameObject playButton;

        /// <summary>
        /// Reference to Loadout Menu.
        /// </summary>
        public GameObject loadoutGO;

        /// <summary>
        /// Reference to player transform for Shop Menu.
        /// </summary>
        public Transform shopTransform;

        /// <summary>
        /// Buttons for game modes.
        /// </summary>
        public GameObject[] gameModeButtons;

        /// <summary>
        /// Descriptions of game modes.
        /// </summary>
        public string[] gameModeDescriptions;

        /// <summary>
        /// UIHover scripts of game mode buttons.
        /// </summary>
        public UIHover[] gameModeUIHoverScripts;

        /// <summary>
        /// Reference to Menu Audio source.
        /// </summary>
		public AudioSource source;

        /// <summary>
        /// Clips to use with source.
        /// </summary>
    	public AudioClip[] clips;

        /// <summary>
        /// Reference to GameModeSelection.
        /// </summary>
        private GameSettings gameSettings;

        /// <summary>
        /// Currently selected game mode.
        /// </summary>
        private int selectedGameMode;

        /// <summary>
        /// State of whether a game mode has been selected.
        /// </summary>
        private bool selectionMade;

        /// <summary>
        /// Reference to refetrence manager.
        /// </summary>
        private ReferenceManager reference;

        /// <summary>
        /// Reference to iridium system.
        /// </summary>
        private IridiumSystem iridiumSystem;

        /// <summary>
        /// Find references.
        /// </summary>
        private void Awake()
        {
            reference = FindObjectOfType<ReferenceManager>();
            gameSettings = reference.gameSettings;
            iridiumSystem = FindObjectOfType<IridiumSystem>();
        }

        /// <summary>
        /// Get default selected game mode. Setup menu based on default selection.
        /// </summary>
        private void OnEnable()
        {
            selectedGameMode = PlayerPrefs.GetInt("defaultGameMode", gameModeButtons.Length);

            if (!iridiumSystem.CheckAnyWeaponsUnlocked())
            {
                continueButton.SetActive(false);
                playButton.SetActive(true);
            }
            else
            {
                continueButton.SetActive(true);
                playButton.SetActive(false);
            }

            if (selectedGameMode == gameModeButtons.Length)
            {
                HideButton();
                selectionMade = false;
            }
            else
            {
                ShowButton();
                gameModeUIHoverScripts[selectedGameMode].Select();
                description.GetComponent<Text>().text = gameModeDescriptions[selectedGameMode];
                selectionMade = true;
            }
        }

        /// <summary>
        /// Called by start button; navigates to Loadout Menu. Set game mode to SoloNormal.
        /// </summary>
        public void PressGameMode(int gameModeNum)
        {
            if (selectedGameMode != gameModeNum)
            {
                if (selectedGameMode == gameModeButtons.Length)
                {
                    ShowButton();
                }

                source.PlayOneShot(clips[0]);

                if (selectionMade)
                {
                    gameModeUIHoverScripts[selectedGameMode].EndSelect();
                    selectionMade = true;
                }
                selectedGameMode = gameModeNum;
                gameModeUIHoverScripts[selectedGameMode].Select();
                description.GetComponent<Text>().text = gameModeDescriptions[selectedGameMode];
            }
        }

        /// <summary>
        /// Make continue/play button unusable.
        /// </summary>
        private void HideButton()
        {
            if (continueButton.activeSelf)
            {
                continueButton.GetComponent<BoxCollider>().enabled = false;
                Color tempColor = continueButton.GetComponent<Image>().color;
                tempColor.a = 0.2f;
                continueButton.GetComponent<Image>().color = tempColor;
            }
            else
            {
                playButton.GetComponent<BoxCollider>().enabled = false;
                Color tempColor = playButton.GetComponent<Image>().color;
                tempColor.a = 0.2f;
                playButton.GetComponent<Image>().color = tempColor;
                tempColor = playButton.GetComponentInChildren<Text>().color;
                tempColor.a = 0.2f;
                playButton.GetComponentInChildren<Text>().color = tempColor;
            }
        }

        /// <summary>
        /// Make continue/play button usable.
        /// </summary>
        private void ShowButton()
        {
            if (continueButton.activeSelf)
            {
                continueButton.GetComponent<BoxCollider>().enabled = true;
                Color tempColor = continueButton.GetComponent<Image>().color;
                tempColor.a = 1.0f;
                continueButton.GetComponent<Image>().color = tempColor;
            }
            else
            {
                playButton.GetComponent<BoxCollider>().enabled = true;
                Color tempColor = playButton.GetComponent<Image>().color;
                tempColor.a = 1.0f;
                playButton.GetComponent<Image>().color = tempColor;
                tempColor = playButton.GetComponentInChildren<Text>().color;
                tempColor.a = 1.0f;
                playButton.GetComponentInChildren<Text>().color = tempColor;
            }
        }

        /// <summary>
        /// Back button pressed; navigates to Main Menu.
        /// </summary>
        public void PressBack()
        {
            source.PlayOneShot(clips[1]);
            if (selectionMade)
            {
                gameModeUIHoverScripts[selectedGameMode].EndSelect();
            }
            HideButton();
            selectionMade = false;
            transitionManager.Transition(backMenuGO);
        }

        /// <summary>
        /// Continue button pressed; navigates to Loadout Menu.
        /// </summary>
        public void PressContinue()
        {
            source.PlayOneShot(clips[0]);

            gameSettings.selectedGameMode = (GameSettings.GameMode)selectedGameMode;
            PlayerPrefs.SetInt("defaultGameMode", selectedGameMode);
            transitionManager.Transition(loadoutGO);
        }
    }
}
