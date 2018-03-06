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
        /// Weapon number.
        /// </summary>
        public int weapon;
    }
}
