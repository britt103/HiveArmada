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
            Laser = 0,
            Minigun = 1,
            RocketPods = 2,
            Plasma = 3
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
        /// Type of color blind mode.
        /// </summary>
        public ColorBlindMode.Mode colorBlindMode;

        /// <summary>
        /// Get default skin from PlayerPrefs.
        /// </summary>
        private void Awake()
        {
            selectedSkin = (Skin)PlayerPrefs.GetInt("defaultSkin", (int)defaultSkin);
        }
    }
}
