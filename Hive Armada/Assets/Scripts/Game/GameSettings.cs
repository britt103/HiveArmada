//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-440-01
// Group Project
//
// GameSettings stores information about the selected game mode 
// and ship loadout preserved between scenes and read in Wave Room.
//
//=============================================================================

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
using Valve.VR;

namespace Hive.Armada.Game
{
    /// <summary>
    /// Stores selected game mode and loadout information.
    /// </summary>
    public class GameSettings : MonoBehaviour
    {
        /// <summary>
        /// Enums for different game modes.
        /// </summary>
        public enum GameMode
        {
            SoloNormal,
            SoloInfinite
        }

        /// <summary>
        /// Game mode enum.
        /// </summary>
        public GameMode selectedGameMode;

        /// <summary>
        /// Enums for different weapons.
        /// </summary>
        public enum Weapon
        {
            Laser = 0,
            Minigun = 1,
            RocketPods = 2,
            Plasma = 3
        }

        /// <summary>
        /// Names of weapons; primarily meant for Results Menu.
        /// </summary>
        public string[] weaponNames;

        /// <summary>
        /// Enum for different powerups.
        /// </summary>
        public enum Powerups
        {
            Shield,
            DamageBoost,
            AreaBomb,
            Clear,
            Ally
        }

        /// <summary>
        /// Weapon enum.
        /// </summary>
        public Weapon selectedWeapon;

        /// <summary>
        /// Enums for different weapons.
        /// </summary>
        public enum Skin
        {
            Skin1 = 0,
            Skin2 = 1,
            Skin3 = 2,
        }

        /// <summary>
        /// Skin enum.
        /// </summary>
        public Skin selectedSkin;

        /// <summary>
        /// Enum of default skin; meant to initially unlocked skin.
        /// </summary>
        public Skin defaultSkin = Skin.Skin1;

        /// <summary>
        /// State of whether aim assist is on.
        /// </summary>
        public bool aimAssist;

        /// <summary>
        /// State of whether score display is on.
        /// </summary>
        public bool scoreDisplay;

        /// <summary>
        /// Reference to shield prefab.
        /// </summary>
        public GameObject shieldPrefab;

        /// <summary>
        /// Reference to areaBomb prefab.
        /// </summary>
        public GameObject areaBombPrefab;

        /// <summary>
        /// Reference to damageBoost prefab.
        /// </summary>
        public GameObject damageBoostPrefab;

        /// <summary>
        /// Reference to ally prefab.
        /// </summary>
        public GameObject allyPrefab;

        /// <summary>
        /// Reference to clearBomb prefab.
        /// </summary>
        public GameObject clearBombPrefab;

        /// <summary>
        /// List of selected powerups.
        /// </summary>
        public List<GameObject> selectedPowerups;

        /// <summary>
        /// Enum for different fire rates.
        /// </summary>
        public enum FireRate
        {
            Slow = 0,
            Normal = 1,
            Fast = 2
        }

        /// <summary>
        /// Fire rate enum
        /// </summary>
        public FireRate selectedFireRate;

        public float[] fireRatePercents;

        /// <summary>
        /// Type of color blind mode.
        /// </summary>
        public ColorBlindMode.Mode colorBlindMode;

        [Header("Wave")]
        public int startingWave;

        [Header("Loading Screen")]
        public Texture[] bossLoadingScreens;

        public Texture[] infiniteLoadingScreens;
        
        public bool IsVive { get; private set; }
        
        public EVRButtonId ToggleMenuButtonId { get; private set; }
        
        public EVRButtonId UsePowerupButtonId1 { get; private set; }
        
        public EVRButtonId UsePowerupButtonId2 { get; private set; }
        
        /// <summary>
        /// Get default skin from PlayerPrefs.
        /// </summary>
        private void Awake()
        {
            selectedSkin = (Skin)PlayerPrefs.GetInt("defaultSkin", (int)defaultSkin);

            IsVive = VRDevice.model.IndexOf("Vive", StringComparison.Ordinal) >= 0;

            if (IsVive)
            {
                ToggleMenuButtonId = EVRButtonId.k_EButton_ApplicationMenu;
                UsePowerupButtonId1 = EVRButtonId.k_EButton_SteamVR_Touchpad;
                UsePowerupButtonId2 = EVRButtonId.k_EButton_SteamVR_Touchpad;
            }
            else
            {
                ToggleMenuButtonId = EVRButtonId.k_EButton_Axis0;
                UsePowerupButtonId1 = EVRButtonId.k_EButton_A;
                UsePowerupButtonId2 = EVRButtonId.k_EButton_ApplicationMenu;
            }
        }

        /// <summary>
        /// Create new list of selected powerups.
        /// </summary>
        /// <param name="shield">State of whether shield is in powerups pool.</param>
        /// <param name="areaBomb">State of whether area bomb is in powerups pool.</param>
        /// <param name="damageBoost">State of whether damage boost is in powerups pool.</param>
        /// <param name="ally">State of whether ally is in powerups pool.</param>
        /// <param name="clearBomb">State of whether clear bomb is in powerups pool.</param>
        public void SetSelectedPowerups(bool shield, bool areaBomb, bool damageBoost, bool ally,
            bool clearBomb)
        {
            selectedPowerups = new List<GameObject>();

            if (shield)
            {
                selectedPowerups.Add(shieldPrefab);
            }
            if (areaBomb)
            {
                selectedPowerups.Add(areaBombPrefab);
            }
            if (damageBoost)
            {
                selectedPowerups.Add(damageBoostPrefab);
            }
            if (ally)
            {
                selectedPowerups.Add(allyPrefab);
            }
            if (clearBomb)
            {
                selectedPowerups.Add(clearBombPrefab);
            }
        }
    }
}
