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
using System;
using Hive.Armada.Ambient;
using Hive.Armada.Data;
using Hive.Armada.Menus;
using UnityEngine.SceneManagement;

namespace Hive.Armada.Game
{
    /// <summary>
    /// Holds references to all prefabs and important objects.
    /// </summary>
    public class ReferenceManager : MonoBehaviour
    {
        [Header("Systems & Managers")]
        public GameManager gameManager;

        public EnemyManager enemyAttributes;

        public ScoringSystem scoringSystem;

        public PlayerStats statistics;

        public WaveManager waveManager;

        public Infinite infinite;

        public ObjectPoolManager objectPoolManager;

        public BossManager bossManager;

		public SceneTransitionManager sceneTransitionManager;

        public OptionsValues optionsValues;

        public GameSettings gameSettings;

        public ColorBlindMode colorBlindMode;

        public PlayerIdleTimer playerIdleTimer;

        public IridiumSpawner iridiumSpawner;

        public MenuSounds menuSounds;

        public DialoguePlayer dialoguePlayer;

        public Tooltips tooltips;
        
        [Header("Player")]
        public GameObject player;

        public GameObject playerLookTarget;

        public GameObject playerShip;

        public GameObject shipLookTarget;

        public GameObject shipPickup;

        public PowerUpStatus powerUpStatus;

        public PlayerHitVignette playerHitVignette;

        public ScoreDisplay playerScoreDisplay;
        
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

        public TalkingParticle talkingParticle;
        
        [Header("Data Containers")]
        public PlayerData playerData;
        
        public ProjectileData projectileData;

        public Cheats cheats;
        
        [Header("Audio")]
        public AudioSource waveCountSource;

        public AudioSource gameMusicSource;

        public AudioSource playerShipSource;

        /// <summary>
        /// Sound for when a powerup is picked up by the player.
        /// </summary>
        public AudioClip powerupReadySound;
        
        /// <summary>
        /// Calls any functions needed to initialize any managers.
        /// </summary>
        private void Awake()
        {
            if (gameSettings == null)
            {
                gameSettings = FindObjectOfType<GameSettings>();
            }

            if (objectPoolManager != null)
            {
                objectPoolManager.Initialize(this);
            }

            if (colorBlindMode == null)
            {
                colorBlindMode = FindObjectOfType<ColorBlindMode>();
            }

            if (waveManager != null)
            {
                waveManager.Initialize(this);
            }

            if (infinite != null)
            {
                infinite.Initialize(this);
            }

            if (statistics == null)
            {
                statistics = FindObjectOfType<PlayerStats>();
            }

            if (optionsValues == null)
            {
                optionsValues = FindObjectOfType<OptionsValues>();
            }

            if (shipLookTarget == null)
            {
                shipLookTarget = GameObject.Find("Ship Look Target");
            }

            try
            {
                playerLookTarget.transform.parent = GameObject.Find("VRCamera").transform;
                playerLookTarget.transform.localPosition = Vector3.zero;
            }
            catch (Exception)
            {
                try
                {
                    playerLookTarget.transform.parent = GameObject.Find("FallbackObjects").transform;
                    playerLookTarget.transform.localPosition = Vector3.zero;
                }
                catch (Exception)
                {
                    Debug.LogError(GetType().Name + " - Cannot find VRCamera and FallbackObjects.");
                }
            }

            if (bossManager != null)
            {
                if (gameSettings.selectedGameMode == GameSettings.GameMode.SoloNormal)
                {
                    bossManager.Initialize(this);
                }
            }

            if (gameSettings.selectedGameMode == GameSettings.GameMode.SoloInfinite)
            {
                shipPickup.SetActive(true);
                tooltips.SpawnGrabShip();
            }

            if (tooltips != null)
            {
                tooltips.Initialize(this);
            }

            if (SceneManager.GetActiveScene().name.Equals("Menu Room"))
            {
                UIPointer[] pointers = FindObjectsOfType<UIPointer>();

                foreach (UIPointer pointer in pointers)
                {
                    pointer.Initialize(gameSettings);
                }
            }
        }
    }
}
