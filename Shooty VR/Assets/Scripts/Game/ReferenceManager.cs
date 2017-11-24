//=============================================================================
// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
// 
// This class contains references for commonly referenced objects in the scene.
// Calling GameObject.Find() a lot causes overhead and can cause lag. The
// reference manager solves that by storing references so that each object only
// needs to call GameObject.Find() once to find the reference manager. Then,
// most references can be acquired through reference manager.
// 
//=============================================================================

using Hive.Armada.Player;
using UnityEngine;

namespace Hive.Armada.Game
{
    /// <summary>
    /// Holds references to all prefabs and important objects.
    /// </summary>
    public class ReferenceManager : MonoBehaviour
    {
        //--------------------
        // Systems & Managers
        //--------------------
        [Header("Systems & Managers")]
        public GameManager gameManager;

        public Spawner spawner;

        public EnemyAttributes enemyAttributes;

        public ScoringSystem scoringSystem;

        public PlayerStats statistics;
        public WaveManager waveManager;

        public ObjectPoolManager objectPoolManager;

        //--------------------
        // Player
        //--------------------
        [Header("Player")]
        public GameObject playerShip;

        public GameObject shipPickup;

        public PowerUpStatus powerUpStatus;

        //--------------------
        // Menus
        //--------------------
        [Header("Menus")]
        public GameObject menuMain;

        public GameObject menuTitle;

        /// <summary>
        /// Calls any functions needed to initialize any managers.
        /// </summary>
        private void Awake()
        {
            if (enemyAttributes)
            {
                enemyAttributes.Initialize();
            }

            if (objectPoolManager)
            {
                objectPoolManager.Initialize();
            }
        }
    }
}