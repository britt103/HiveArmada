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

using System.Collections.Generic;
using UnityEngine;

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
            Laser,
            Minigun,
            RocketPods,
            Plasma
        }

        /// <summary>
        /// Weapon enum.
        /// </summary>
        public Weapon selectedWeapon;

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

        /// <summary>
        /// Type of color blind mode.
        /// </summary>
        public ColorBlindMode.Mode colorBlindMode;

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
