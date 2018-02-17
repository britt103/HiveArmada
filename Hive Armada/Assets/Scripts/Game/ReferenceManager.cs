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

using UnityEngine;
using UnityEngine.UI;
using Hive.Armada.Player;
using Hive.Armada.PowerUps;

namespace Hive.Armada.Game
{
    /// <summary>
    /// Holds references to all prefabs and important objects.
    /// </summary>
    public class ReferenceManager : MonoBehaviour
    {
        //----------------------------------------
        // 
        // Systems & Managers
        // 
        //----------------------------------------
        [Header("Systems & Managers")]
        public GameManager gameManager;

        public EnemyAttributes enemyAttributes;

        public ScoringSystem scoringSystem;

        public PlayerStats statistics;

        public WaveManager waveManager;

        public ObjectPoolManager objectPoolManager;

		public SceneTransitionManager sceneTransitionManager;

        public OptionsValues optionsValues;

        public PlayerIdleTimer playerIdleTimer;

        //----------------------------------------
        // 
        // Player
        // 
        //----------------------------------------
        [Header("Player")]
        public GameObject player;

        public GameObject playerShip;

        public GameObject shipPickup;

        public PowerUpStatus powerUpStatus;

        //----------------------------------------
        // 
        // Menus & UI
        // 
        //----------------------------------------

        /// <summary>
        /// Main menu game object
        /// </summary>
        [Header("Menus")]
        public GameObject menuMain;

        /// <summary>
        /// Title screen game object
        /// </summary>
        public GameObject menuTitle;

        public GameObject countdown;

        /// <summary>
        /// Wave count Text. Shows "Wave #" where # is the current wave.
        /// </summary>
        public Text menuWaveNumberDisplay;

        /// <summary>
        /// Game win menu with score.
        /// </summary>
        public GameObject menuWin;

        /// <summary>
        /// Game over menu with score.
        /// </summary>
        public GameObject menuGameOver;

        //----------------------------------------
        // 
        // Audio
        // 
        //----------------------------------------
        [Header("Audio")]
        public AudioSource waveCountSource;

        public AudioSource gameMusicSource;

        public AudioSource playerShipSource;

        /// <summary>
        /// Sound for when a powerup is picked up by the player.
        /// </summary>
        public AudioClip powerupReadySound;

        //----------------------------------------
        // 
        // Methods
        // 
        //----------------------------------------

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

            if (!statistics)
            {
                statistics = FindObjectOfType<PlayerStats>();
            }

            if (!optionsValues)
            {
                optionsValues = FindObjectOfType<OptionsValues>();
            }
        }
    }
}
