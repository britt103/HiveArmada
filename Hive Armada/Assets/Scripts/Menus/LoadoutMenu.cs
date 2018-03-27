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
        /// Variables used as a check to make sure audio
        /// doesn't play over itself
        /// </summary>
        private int weaponCount = 0;

        private int backCount = 0;

        private int playCount = 0;

        /// <summary>
        /// Find references. Set display for initial waepon. If no Iridium weapons have been 
        /// unlocked, skip menu.
        /// </summary>
        void Awake()
        {
            reference = FindObjectOfType<ReferenceManager>();
            gameSettings = reference.gameSettings;
            iridiumSystem = FindObjectOfType<IridiumSystem>();

            if (!iridiumSystem.CheckAnyWeaponsUnlocked())
            {
                selectedWeapon = (int)GameSettings.Weapon.Laser;
                PressPlay();
            }
            else
            {
                for (int i = 0; i < weaponCells.Length; i++)
                {
                    if (iridiumSystem.CheckWeaponIsPresent(weaponNames[i]))
                    {
                        weaponCells[i].SetActive(iridiumSystem.CheckWeaponUnlocked(weaponNames[i]));
                    }
                    else
                    {
                        weaponCells[i].SetActive(true);
                    }
                }

                selectedWeapon = PlayerPrefs.GetInt("defaultWeapon", weaponCells.Length);

                if (selectedWeapon == weaponCells.Length)
                {
                    HidePlayButton();
                }
                else
                {
                    ScrollToCell(selectedWeapon);
                    weaponUIHoverScripts[selectedWeapon].Select();
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
                //weaponCount += 1;
                //if (weaponCount > 1)
                //{
                //    source.Stop();
                //    source.PlayOneShot(clips[0]);
                //}
                weaponUIHoverScripts[selectedWeapon].EndSelect();
                selectedWeapon = weaponNum;
            }
            weaponUIHoverScripts[selectedWeapon].Select();
        }

        /// <summary>
        /// Back button pressed. Navigate to Start Menu.
        /// </summary>
        public void PressBack()
        {
            source.PlayOneShot(clips[1]);
            backCount += 1;
            //if (backCount > 1)
            //{
            //    source.Stop();
            //    source.PlayOneShot(clips[1]);
            //}
            transitionManager.Transition(backMenuGO);
        }

        /// <summary>
        /// Move scroll view to position of specified cell.
        /// </summary>
        /// <param name="weaponNum">Number of cell to move to.</param>
        private void ScrollToCell(int weaponNum)
        {
            float totalHeight = (weaponCells.Length * scrollViewCellHeight) +
                (Mathf.Max((weaponCells.Length - 1), 0) * scrollViewCellVerticalSpacing);
            float buttonHeight = (weaponNum * scrollViewCellHeight) +
                (weaponNum * scrollViewCellVerticalSpacing);
            float value = buttonHeight / totalHeight;
            scrollBar.value = 1 - value;
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
            tempColor = playButton.GetComponentInChildren<Text>().color;
            tempColor.a = 0.2f;
            playButton.GetComponentInChildren<Text>().color = tempColor;
        }

        /// <summary>
        /// Play button pressed. Trigger scene transition to Wave Room.
        /// </summary>
        public void PressPlay()
        {
            source.PlayOneShot(clips[0]);
            //StartCoroutine(pressPlaySound());
            //playCount += 1;
            //if (playCount > 1)
            //{
            //    source.Stop();
            //    source.PlayOneShot(clips[0]);
            //}
            gameSettings.selectedWeapon = (GameSettings.Weapon)selectedWeapon;
            PlayerPrefs.SetInt("defaultWeapon", selectedWeapon);
            reference.sceneTransitionManager.TransitionOut("Wave Room");
            gameObject.SetActive(false);
        }

        //private IEnumerator pressPlaySound()
        //{
        //    source.PlayOneShot(clips[0]);
        //    yield return new WaitForSeconds(0.5f);
        //    source.PlayOneShot(clips[2]);
        //}
    }
}