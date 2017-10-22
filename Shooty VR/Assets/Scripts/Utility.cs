//=============================================================================
// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
// 
// This static class has utility functions and fields that are used in
// multiple scripts throughout the project.
// 
//=============================================================================

using UnityEngine;

namespace Hive.Armada
{
    public static class Utility
    {
        /// <summary>
        /// Whether or not to output debug information to the console.
        /// </summary>
        public static bool isDebug = true;

        /// <summary>
        /// Layer mask for the Enemy layer
        /// </summary>
        public static LayerMask enemyMask = LayerMask.GetMask("Enemy");

        /// <summary>
        /// Layer mask for the Room layer
        /// </summary>
        public static LayerMask roomMask = LayerMask.GetMask("Room");

        /// <summary>
        /// Layer mask for the UI layer
        /// </summary>
        public static LayerMask uiMask = LayerMask.GetMask("UI");

        /// <summary>
        /// Layer mask for the UI and Room layer. Used for the laser when in Menu mode.
        /// </summary>
        public static LayerMask laserMenuMask = LayerMask.GetMask("UI", "Room");
    }
}
