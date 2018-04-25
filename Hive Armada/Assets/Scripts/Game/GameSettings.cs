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

        public int selectedSkin;

        /// <summary>
        /// State of whether aim assist is on.
        /// </summary>
        public bool aimAssist;

        /// <summary>
        /// State of whether score display is on.
        /// </summary>
        public bool scoreDisplay;

        /// <summary>
        /// Type of color blind mode.
        /// </summary>
        public ColorBlindMode.Mode colorBlindMode;

        [Tooltip("Which # wave to start on? Not 0-based." +
                 "\n0 - Wave 1" +
                 "\n1 - Wave 1" +
                 "\n2 - Wave 2")]
        public int startingWave;
    }
}
