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
using SubjectNerd.Utilities;

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
        public GameObject previousMenu;

        /// <summary>
        /// Reference to play button game object.
        /// </summary>
        [Space]
        public GameObject playButton;

        public GameObject selectWeaponText;

        public GameObject statsText;

        public GameObject currentText;

        public GameObject currentWeaponText;

        public GameObject gameMode;

        public GameObject currentMode;

        [Reorderable("Weapon", false)]
        public GameObject[] weaponStatsTexts;

        private string[] weaponDisplayNames;

        /// <summary>
        /// Reference to icon image game object.
        /// </summary>
        public GameObject weaponIcon;

        /// <summary>
        /// Names of weapons.
        /// </summary>
        [Space]
        [Reorderable("Weapon", false)]
        public string[] weaponNames;

        /// <summary>
        /// Icons of weapons, in order.
        /// </summary>
        [Reorderable("Weapon", false)]
        public Sprite[] weaponIcons;

        /// <summary>
        /// Cells for weapons.
        /// </summary>
        [Reorderable("Weapon", false)]
        public GameObject[] weaponCells;

        /// <summary>
        /// UIHover scripts of weapon buttons.
        /// </summary>
        [Reorderable("Weapon", false)]
        public UIHover[] weaponUiHoverScripts;

        /// <summary>
        /// Cells for weapons that are unlocked by default.
        /// </summary>
        [Reorderable("Weapon", false)]
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
        [Header("Audio")]
        public AudioSource source;

        public AudioSource zenaSource;

        /// <summary>
        /// Clips to use with source.
        /// </summary>
        [Reorderable("Clip", false)]
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

            weaponDisplayNames = new string[weaponCells.Length];

            for (int i = 0; i < weaponCells.Length; ++i)
            {
                weaponDisplayNames[i] = weaponCells[i].GetComponentInChildren<Text>().text;
            }
        }

        /// <summary>
        /// Set display for initial waepon. If no Iridium weapons have been 
        /// unlocked, skip menu.
        /// </summary>
        private void OnEnable()
        {
            gameMode.SetActive(true);

            if (gameSettings.selectedGameMode == GameSettings.GameMode.SoloNormal)
            {
                currentMode.GetComponent<Text>().text = "BOSS";
            }
            else
            {
                currentMode.GetComponent<Text>().text = "INFINITE";
            }
            
            currentMode.SetActive(true);

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
                }

                selectedWeapon = PlayerPrefs.GetInt("defaultWeapon", weaponCells.Length);

                if (selectedWeapon == weaponCells.Length)
                {
                    HidePlayButton();
                    selectionMade = false;
                    weaponIcon.SetActive(false);
                    selectWeaponText.SetActive(true);
                    SetActiveStats(-1);
                    statsText.SetActive(false);
                    currentText.SetActive(false);
                    currentWeaponText.SetActive(false);

                }
                else
                {
                    ShowPlayButton();
                    weaponUiHoverScripts[selectedWeapon].Select();
                    selectionMade = true;
                    selectWeaponText.SetActive(false);
                    statsText.SetActive(true);
                    SetActiveStats(selectedWeapon);
                    currentText.SetActive(true);
                    currentWeaponText.SetActive(true);
                    currentWeaponText.GetComponent<Text>().text = weaponDisplayNames[selectedWeapon];
                    weaponIcon.SetActive(true);
                    weaponIcon.GetComponent<Image>().sprite = weaponIcons[selectedWeapon];
                }
            }
        }

        private void SetActiveStats(int weapon)
        {
            for (int i = 0; i < weaponStatsTexts.Length; ++i)
            {
                weaponStatsTexts[i].SetActive(weapon == i);
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
                    weaponUiHoverScripts[selectedWeapon].EndSelect();
                }
                else
                {
                    ShowPlayButton();
                }

                PlayerPrefs.SetInt("defaultWeapon", weaponNum);
                selectedWeapon = weaponNum;
                selectionMade = true;
                selectWeaponText.SetActive(false);
                statsText.SetActive(true);
                SetActiveStats(selectedWeapon);
                currentText.SetActive(true);
                currentWeaponText.SetActive(true);
                currentWeaponText.GetComponent<Text>().text = weaponDisplayNames[selectedWeapon];
                weaponIcon.SetActive(true);
                weaponIcon.GetComponent<Image>().sprite = weaponIcons[selectedWeapon];

            }
            weaponUiHoverScripts[selectedWeapon].Select();
        }

        /// <summary>
        /// Back button pressed. Navigate to Start Menu.
        /// </summary>
        public void PressBack()
        {
            source.PlayOneShot(reference.menuSounds.menuButtonSelectSound);
            if (selectedWeapon != weaponCells.Length)
            {
                weaponUiHoverScripts[selectedWeapon].EndSelect();
            }
            transitionManager.Transition(previousMenu);
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
            
            reference.talkingParticle.Speak(reference.menuSounds.gameStart[Random.Range(0, reference.menuSounds.gameStart.Length)], true);
            // zenaSource.PlayOneShot(reference.menuSounds.gameStart[Random.Range(0, reference.menuSounds.gameStart.Length)]);

        }

        //private IEnumerator pressPlaySounds()
        //{
        //    yield return new WaitForSeconds(0.3f);
        //    int playSoundIndex = Random.Range(2, clips.Length);
        //    if (zenaSource.isPlaying)
        //    {
        //        new WaitWhile(() => source.isPlaying);
        //    }
        //    else
        //    {
        //        zenaSource.PlayOneShot(clips[playSoundIndex]);
        //    }
        //}
    }
}