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
        /// Layer mask for the room and pathing enemies.
        /// </summary>
        public static LayerMask roomPathingMask = LayerMask.GetMask("Room", "PathingEnemy");

        /// <summary>
        /// Layer mask for the UI layer
        /// </summary>
        public static LayerMask uiMask = LayerMask.GetMask("UI");

        /// <summary>
        /// Layer mask for UICover layer
        /// </summary>
        public static LayerMask uiCoverMask = LayerMask.GetMask("UICover");

        /// <summary>
        /// Layer mask for the Shootable layer
        /// </summary>
        public static LayerMask shootableMask = LayerMask.GetMask("Shootable");

        /// <summary>
        /// Layer mask for the UI and Room layer. Used for the laser when in Menu mode.
        /// </summary>
        public static LayerMask laserMenuMask = LayerMask.GetMask("UI", "Room");

        /// <summary>
        /// The layer ID number for Room.
        /// </summary>
        public static int roomLayerId = 8;

        /// <summary>
        /// The layer ID number for Enemy.
        /// </summary>
        public static int enemyLayerId = 9;

        /// <summary>
        /// The layer ID number for Shootable.
        /// </summary>
        public static int shootableLayerId = 10;

        /// <summary>
        /// The layer ID number for UICover.
        /// </summary>
        public static int uiCoverLayerId = 11;

        /// <summary>
        /// The layer ID number for PathingEnemy.
        /// </summary>
        public static int pathingEnemyLayerId = 12;
    }
}
