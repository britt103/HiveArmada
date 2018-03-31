//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// LoadoutMenu controls interactions with the Load Menu. The player selects
// which weapon they want to use in this menu.
//
//=============================================================================

using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Hive.Armada.Game;

namespace Hive.Armada.Menus
{
    /// <summary>
    /// Controls interactions with Loadout Menu. 
    /// </summary>
    public class LoadoutMenu : MonoBehaviour
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
        /// Reference to play button game object.
        /// </summary>
        public GameObject playButton;

        /// <summary>
        /// Reference to menu ScrollView game object.
        /// </summary>
        public GameObject scrollView;

        /// <summary>
        /// Names of weapons.
        /// </summary>
        public string[] weaponNames;

        /// <summary>
        /// Enumerators of weapons.
        /// </summary>
        public int[] weaponEnums;

        /// <summary>
        /// Cells for weapons.
        /// </summary>
        public GameObject[] weaponCells;

        /// <summary>
        /// UIHover scripts of weapon buttons.
        /// </summary>
        public UIHover[] weaponUIHoverScripts;

        /// <summary>
        /// Cells for weapons that are unlocked by default.
        /// </summary>
        public GameObject[] fixedWeaponCells;

        /// <summary>
        /// Currently selected weapon.
        /// </summary>
        private int selectedWeapon;

        /// <summary>
        /// Reference to ReferenceManager.
        /// </summary>
        private ReferenceManager reference;

        /// <summary>
        /// Reference to ShipLoadout.
        /// </summary>
        private GameSettings gameSettings;

        /// <summary>
        /// Reference to Iridium System.
        /// </summary>
        private IridiumSystem iridiumSystem;

        /// <summary>
        /// Reference to Menu Audio source.
        /// </summary>
        public AudioSource source;

        /// <summary>
        /// Clips to use with source.
        /// </summary>
    	public AudioClip[] clips;

        /// <summary>
        /// State of whether a weapon has been selected.
        /// </summary>
        private bool selectionMade;

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
        /// Set display for initial waepon. If no Iridium weapons have been 
        /// unlocked, skip menu.
        /// </summary>
        private void OnEnable()
        {
            int activeCells = 0;
            if (!iridiumSystem.CheckAnyWeaponsUnlocked())
            {
                selectedWeapon = (int)GameSettings.Weapon.Laser;
                PressPlay();
            }
            else
            {
                for (int i = 0; i < weaponCells.Length; i++)
                {
                    if (!fixedWeaponCells.Contains(weaponCells[i]))
                    {
                        weaponCells[i].SetActive(iridiumSystem.CheckWeaponUnlocked(weaponNames[i]));
                        if (weaponCells[i].activeSelf)
                        {
                            activeCells++;
                        }
                    }
                    else
                    {
                        weaponCells[i].SetActive(true);
                        activeCells++;
                    }
                }

                selectedWeapon = PlayerPrefs.GetInt("defaultWeapon", weaponCells.Length);

                if (selectedWeapon == weaponCells.Length)
                {
                    HidePlayButton();
                    selectionMade = false;
                    ScrollToCell(0, activeCells);
                }
                else
                {
                    ShowPlayButton();
                    weaponUIHoverScripts[selectedWeapon].Select();
                    selectionMade = true;
                    ScrollToCell(selectedWeapon, activeCells);
                }
            }
        }

        /// <summary>
        /// Weapon button pressed. Set shipLoadout weapon to weaponNum.
        /// </summary>
        public void PressWeapon(int weaponNum)
        {
            if (weaponNum != selectedWeapon)
            {
                source.PlayOneShot(clips[0]);
                if (selectionMade)
                {
                    weaponUIHoverScripts[selectedWeapon].EndSelect();
                }
                else
                {
                    ShowPlayButton();
                }
                selectedWeapon = weaponNum;
                selectionMade = true;
            }
            weaponUIHoverScripts[selectedWeapon].Select();
        }

        /// <summary>
        /// Back button pressed. Navigate to Start Menu.
        /// </summary>
        public void PressBack()
        {
            source.PlayOneShot(clips[1]);
            transitionManager.Transition(backMenuGO);
        }

        /// <summary>
        /// Move scroll view to position of specified cell.
        /// </summary>
        /// <param name="weaponNum">Number of cell to move to.</param>
        /// <param name="activeCells">Number of active cells in content.</param>
        private void ScrollToCell(int weaponNum, int activeCells)
        {
            float scrollStep = 1.0f / (activeCells - 1.0f);
            float scrollValue = 1.0f - scrollStep * weaponNum;
            scrollView.gameObject.GetComponent<ScrollRect>().verticalNormalizedPosition = scrollValue;
        }

        /// <summary>
        /// Make continue button unusable.
        /// </summary>
        private void HidePlayButton()
        {
            playButton.GetComponent<BoxCollider>().enabled = false;
            Color tempColor = playButton.GetComponent<Image>().color;
            tempColor.a = 0.2f;
            playButton.GetComponent<Image>().color = tempColor;
        }

        /// <summary>
        /// Make continue button usable.
        /// </summary>
        private void ShowPlayButton()
        {
            playButton.GetComponent<BoxCollider>().enabled = true;
            Color tempColor = playButton.GetComponent<Image>().color;
            tempColor.a = 1.0f;
            playButton.GetComponent<Image>().color = tempColor;
        }

        /// <summary>
        /// Play button pressed. Trigger scene transition to Wave Room.
        /// </summary>
        public void PressPlay()
        {
            source.PlayOneShot(clips[0]);
            gameSettings.selectedWeapon = (GameSettings.Weapon)selectedWeapon;
            PlayerPrefs.SetInt("defaultWeapon", selectedWeapon);
            reference.sceneTransitionManager.TransitionOut("Wave Room");
        }
    }
}