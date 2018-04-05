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
using System.Collections.Generic;
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
        /// Reference to vertical scrollbar.
        /// </summary>
        public Scrollbar scrollbar;

        /// <summary>
        /// Reference to vertical slider.
        /// </summary>
        public Slider verticalSlider;

        /// <summary>
        /// Reference to menu to go to when back is pressed.
        /// </summary>
        public GameObject backMenuGO;

        /// <summary>
        /// Reference to play button game object.
        /// </summary>
        public GameObject playButton;

        /// <summary>
        /// Reference to description game object.
        /// </summary>
        public GameObject weaponDescription;

        /// <summary>
        /// Reference to icon image game object.
        /// </summary>
        public GameObject weaponIcon;

        /// <summary>
        /// Reference to menu ScrollView game object.
        /// </summary>
        public GameObject scrollView;

        /// <summary>
        /// Number of cells that are completely visible in view at a time.
        /// </summary>
        public int numFittableCells;

        /// <summary>
        /// Names of weapons.
        /// </summary>
        public string[] weaponNames;

        /// <summary>
        /// Icons of weapons, in order.
        /// </summary>
        public Sprite[] weaponIcons;

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
        /// List of weapon texts.
        /// </summary>
        private List<string> weaponTexts;

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

        public AudioSource zenaSource;

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
            weaponTexts = iridiumSystem.GetItemTexts("Weapons");

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
                    weaponCells[i].SetActive(iridiumSystem.CheckWeaponUnlocked(weaponNames[i]));
                    if (weaponCells[i].activeSelf)
                    {
                        activeCells++;
                    }
                }

                selectedWeapon = PlayerPrefs.GetInt("defaultWeapon", weaponCells.Length);

                if (selectedWeapon == weaponCells.Length)
                {
                    HidePlayButton();
                    selectionMade = false;
                    weaponDescription.GetComponent<Text>().text = "";
                    weaponIcon.SetActive(false);
                    ScrollToCell(0, activeCells);
                }
                else
                {
                    ShowPlayButton();
                    weaponUIHoverScripts[selectedWeapon].Select();
                    selectionMade = true;
                    weaponDescription.GetComponent<Text>().text = weaponTexts[selectedWeapon];
                    weaponIcon.SetActive(true);
                    weaponIcon.GetComponent<Image>().sprite = weaponIcons[selectedWeapon];
                    ScrollToCell(selectedWeapon, activeCells);
                }

                if (activeCells <= numFittableCells)
                {
                    scrollbar.gameObject.GetComponent<BoxCollider>().enabled = false;
                    verticalSlider.gameObject.SetActive(false);
                }
                else
                {
                    scrollbar.gameObject.GetComponent<BoxCollider>().enabled = true;
                    verticalSlider.gameObject.SetActive(true);
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
                source.PlayOneShot(reference.menuSounds.menuButtonSelectSound);
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
                weaponDescription.GetComponent<Text>().text = weaponTexts[selectedWeapon];
                weaponIcon.SetActive(true);
                weaponIcon.GetComponent<Image>().sprite = weaponIcons[selectedWeapon];

            }
            weaponUIHoverScripts[selectedWeapon].Select();
        }

        /// <summary>
        /// Back button pressed. Navigate to Start Menu.
        /// </summary>
        public void PressBack()
        {
            source.PlayOneShot(reference.menuSounds.menuButtonSelectSound);
            weaponUIHoverScripts[selectedWeapon].EndSelect();
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
            source.PlayOneShot(reference.menuSounds.menuButtonSelectSound);
            //StartCoroutine(pressPlaySounds());

            gameSettings.selectedWeapon = (GameSettings.Weapon)selectedWeapon;
            PlayerPrefs.SetInt("defaultWeapon", selectedWeapon);
            reference.sceneTransitionManager.TransitionOut("Wave Room");
            gameObject.SetActive(false);
        }

        IEnumerator pressPlaySounds()
        {
            yield return new WaitForSeconds(0.3f);
            int playSoundIndex = Random.Range(2, clips.Length);
            if (zenaSource.isPlaying)
            {
                new WaitWhile(() => source.isPlaying);
            }
            else
            {
                zenaSource.PlayOneShot(clips[playSoundIndex]);
            }
        }
    }
}