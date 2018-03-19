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
using Hive.Armada.Game;
using Hive.Armada.Player;

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
        /// Reference to Menu Audio source.
        /// </summary>
		public AudioSource source;

        /// <summary>
        /// Clips to use with source.
        /// </summary>
    	public AudioClip[] clips;

        /// <summary>
        /// Names of weapons.
        /// </summary>
        public string[] weaponNames;

        /// <summary>
        /// Enumerators of weapons.
        /// </summary>
        public int[] weaponEnums;

        /// <summary>
        /// Buttons for weapons.
        /// </summary>
        public GameObject[] weaponButtons;

        /// <summary>
        /// Enum of initially selected weapon. Starts as first weapon. Overriden by PlayerPrefs
        /// .defaultWeapon.
        /// </summary>
        public int initialWeapon = 0;

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
        private ShipLoadout shipLoadout;

        /// <summary>
        /// Reference to Iridium System.
        /// </summary>
        private IridiumSystem iridiumSystem;

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
            shipLoadout = FindObjectOfType<ShipLoadout>();
            iridiumSystem = FindObjectOfType<IridiumSystem>();

            selectedWeapon = PlayerPrefs.GetInt("defaultWeapon", initialWeapon);

            if (!iridiumSystem.CheckAnyWeaponsUnlocked())
            {
                PressPlay();
            }
            else
            {
                for (int i = 0; i < weaponButtons.Length; i++)
                {
                    if (iridiumSystem.CheckWeaponIsPresent(weaponNames[i]))
                    {
                        weaponButtons[i].SetActive(iridiumSystem.CheckWeaponUnlocked(weaponNames[i]));
                    }
                    else
                    {
                        weaponButtons[i].SetActive(true);
                    }
                }

                PressWeapon(selectedWeapon);
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
                weaponButtons[selectedWeapon].GetComponent<UIHover>().EndSelect();
                selectedWeapon = weaponNum;
            }
            weaponButtons[selectedWeapon].GetComponent<UIHover>().Select();
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
        /// Play button pressed. Trigger scene transition to Wave Room. Set defaultWeapon.
        /// </summary>
        public void PressPlay()
        {
            shipLoadout.weapon = selectedWeapon;
            PlayerPrefs.SetInt("defaultWeapon", selectedWeapon);
            source.PlayOneShot(clips[0]);
            //StartCoroutine(pressPlaySound());
            //playCount += 1;
            //if (playCount > 1)
            //{
            //    source.Stop();
            //    source.PlayOneShot(clips[0]);
            //}
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