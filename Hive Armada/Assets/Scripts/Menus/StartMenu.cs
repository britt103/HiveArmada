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
        /// Reference to continue button game object.
        /// </summary>
        public GameObject continueButton;

        /// <summary>
        /// Reference to Loadout Menu.
        /// </summary>
        public GameObject loadoutGO;

        /// <summary>
        /// Reference to player transform for Shop Menu.
        /// </summary>
        public Transform shopTransform;

        /// <summary>
        /// Cells for game modes.
        /// </summary>
        public GameObject[] gameModeCells;

        /// <summary>
        /// UIHover scripts of game mode buttons.
        /// </summary>
        public UIHover[] gameModeUIHoverScripts;

        /// <summary>
        /// Reference to menu ScrollView game object.
        /// </summary>
        public GameObject scrollView;

        /// <summary>
        /// Height of scroll view content cells.
        /// </summary>
        public float scrollViewCellHeight;

        /// <summary>
        /// Length of vertical space between scroll view content cells.
        /// </summary>
        public float scrollViewCellVerticalSpacing;

        /// <summary>
        /// Reference to scrollview vertical scrollbar.
        /// </summary>
        public Scrollbar scrollBar;

        /// <summary>
        /// Reference to vertical slider.
        /// </summary>
        public Slider verticalSlider;

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
        /// Variables used as a check to make sure audio
        /// doesn't play over itself
        /// </summary>
        private int soloNormalCounter = 0;
        private int soloInfiniteCounter = 0;
        private int backCounter = 0;

        /// <summary>
        /// Find references.
        /// </summary>
        private void Awake()
        {
            gameSettings = FindObjectOfType<GameSettings>();

            selectedGameMode = PlayerPrefs.GetInt("defaultGameMode", gameModeCells.Length);

            if (selectedGameMode == gameModeCells.Length)
            {
                HideContinueButton();
            }
            else
            {
                ScrollToCell(selectedGameMode);
                gameModeUIHoverScripts[selectedGameMode].Select();
            }
        }

        /// <summary>
        /// Called by start button; navigates to Loadout Menu. Set game mode to SoloNormal.
        /// </summary>
        public void PressGameMode(GameSettings.GameMode gameMode)
        {
            if ((GameSettings.GameMode)selectedGameMode != gameMode)
            {
                if (selectedGameMode == gameModeCells.Length)
                {
                    ShowContinueButton();
                }

                source.PlayOneShot(clips[0]);
                soloNormalCounter += 1;
                if (soloNormalCounter > 1)
                {
                    source.Stop();
                    source.PlayOneShot(clips[0]);
                }

                gameSettings.selectedGameMode = gameMode;
                gameModeUIHoverScripts[selectedGameMode].EndSelect();
                selectedGameMode = (int)gameMode;
                gameModeUIHoverScripts[selectedGameMode].Select();
            }
        }

        /// <summary>
        /// Move scroll view to position of specified button.
        /// </summary>
        /// <param name="gameModeEnum">Num of cell to move to.</param>
        private void ScrollToCell(int gameModeNum)
        {
            float totalHeight = (gameModeCells.Length * scrollViewCellHeight) +
                (Mathf.Max((gameModeCells.Length - 1), 0) * scrollViewCellVerticalSpacing);
            float buttonHeight = (gameModeNum * scrollViewCellHeight) +
                (gameModeNum * scrollViewCellVerticalSpacing);
            float value = buttonHeight / totalHeight;
            scrollBar.value = 1 - value;
        }

        /// <summary>
        /// Make continue button unusable.
        /// </summary>
        private void HideContinueButton()
        {
            continueButton.GetComponent<BoxCollider>().enabled = false;
            Color tempColor = continueButton.GetComponent<Image>().color;
            tempColor.a = 0.2f;
            continueButton.GetComponent<Image>().color = tempColor;
            tempColor = continueButton.GetComponentInChildren<Text>().color;
            tempColor.a = 0.2f;
            continueButton.GetComponentInChildren<Text>().color = tempColor;
        }

        /// <summary>
        /// Make continue button usable.
        /// </summary>
        private void ShowContinueButton()
        {
            continueButton.GetComponent<BoxCollider>().enabled = true;
            Color tempColor = continueButton.GetComponent<Image>().color;
            tempColor.a = 1.0f;
            continueButton.GetComponent<Image>().color = tempColor;
            tempColor = continueButton.GetComponentInChildren<Text>().color;
            tempColor.a = 1.0f;
            continueButton.GetComponentInChildren<Text>().color = tempColor;
        }

        /// <summary>
        /// Back button pressed; navigates to Main Menu.
        /// </summary>
        public void PressBack()
        {
            source.PlayOneShot(clips[1]);
            backCounter += 1;
            if(backCounter > 1)
            {
                source.Stop();
                source.PlayOneShot(clips[1]);
            }
            transitionManager.Transition(backMenuGO);
        }

        /// <summary>
        /// Continue button pressed; navigates to Loadout Menu.
        /// </summary>
        public void PressContinue()
        {
            source.PlayOneShot(clips[0]);

            PlayerPrefs.SetInt("defaultGameMode", selectedGameMode);
            transitionManager.Transition(loadoutGO);
        }
    }
}
