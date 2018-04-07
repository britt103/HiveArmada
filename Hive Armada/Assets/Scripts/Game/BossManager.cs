//=============================================================================
// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-440-01
// Group Project
// 
// 
// 
//=============================================================================

using Hive.Armada.Enemies;
using Hive.Armada.Player;
using SubjectNerd.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hive.Armada.Game
{
    /// <summary>
    /// The manager for all boss behavior.
    /// </summary>
    public class BossManager : MonoBehaviour
    {
        /// <summary>
        /// Reference manager that holds all needed references
        /// (e.g. spawner, game manager, etc.)
        /// </summary>
        private ReferenceManager reference;

        public GameObject bossPrefab;

        private GameObject bossObject;

        private NewBoss bossScript;

        [Header("Points")]
        public Transform bossSpawn;

        public Transform combatPoint;

        public Transform patrolCenterPoint;

        public Transform idleLeftPoint;

        public Transform idleRightPoint;

        public Dictionary<string, Transform> bossPoints;

        [Header("Paths")]
        public iTweenPath spawnToPatrolPath;

        [Reorderable("Path", false)]
        public iTweenPath[] patrolCLPaths;

        [Reorderable("Path", false)]
        public iTweenPath[] patrolLCPaths;

        [Reorderable("Path", false)]
        public iTweenPath[] patrolCRPaths;

        [Reorderable("Path", false)]
        public iTweenPath[] patrolRCPaths;

        [Reorderable("Path", false)]
        public iTweenPath[] combatToLPaths;

        [Reorderable("Path", false)]
        public iTweenPath[] combatFromLPaths;

        [Reorderable("Path", false)]
        public iTweenPath[] combatToCPaths;

        [Reorderable("Path", false)]
        public iTweenPath[] combatFromCPaths;

        [Reorderable("Path", false)]
        public iTweenPath[] combatToRPaths;

        [Reorderable("Path", false)]
        public iTweenPath[] combatFromRPaths;

        public Dictionary<string, iTweenPath[]> bossPaths;

        [Header("Audio")]
        public AudioSource source;

        public AudioClip introClip;

        public bool IsInitialized { get; private set; }

        private void Awake()
        {
            bossPaths = new Dictionary<string, iTweenPath[]>
                        {
                            {"patrolCL", patrolCLPaths},
                            {"patrolLC", patrolLCPaths},
                            {"patrolCR", patrolCRPaths},
                            {"patrolRC", patrolRCPaths},
                            //{"combatToL", combatToLPaths},
                            {"combatFromL", combatFromLPaths},
                            {"combatToC", combatToCPaths},
                            {"combatFromC", combatFromCPaths},
                            //{"combatToR", combatToRPaths},
                            {"combatFromR", combatFromRPaths}
                        };

            bossPoints = new Dictionary<string, Transform>
                         {
                             {"bossSpawn", bossSpawn},
                             {"combat", combatPoint},
                             {"patrol", patrolCenterPoint},
                             {"idleLeft", idleLeftPoint},
                             {"idleRight", idleRightPoint}
                         };
        }

        /// <summary>
        /// Initializes
        /// </summary>
        /// <param name="referenceManager"> The reference manager </param>
        public void Initialize(ReferenceManager referenceManager)
        {
            if (!IsInitialized)
            {
                reference = referenceManager;

                if (bossPrefab != null)
                {
                    IsInitialized = true;
                    ObjectPoolManager opm = reference.objectPoolManager;

                    bossObject = opm.Spawn(gameObject, opm.GetTypeIdentifier(bossPrefab),
                                           bossSpawn.position, bossSpawn.rotation, transform);

                    //bossObject = Instantiate(bossPrefab, bossSpawn.position, bossSpawn.rotation,
                    //                         transform);
                    bossScript = bossObject.GetComponent<NewBoss>();
                    TransitionState(BossStates.Intro);
                }
                else
                {
                    Debug.LogError(GetType().Name + " - bossPrefab is not set.");
                }
            }
        }

        /// <summary>
        /// Tells the boss to transition state.
        /// </summary>
        /// <param name="newState"> The new state to transition to </param>
        private void TransitionState(BossStates newState)
        {
            bossScript.TransitionState(newState);
        }

        /// <summary>
        /// Tells the boss to enter combat.
        /// </summary>
        /// <param name="currentWave"> Index of the current wave </param>
        public void EnterBoss(int currentWave)
        {
            bossScript.SetWave(currentWave);
            TransitionState(BossStates.TransitionToCombat);
        }

        /// <summary>
        /// Tells the wave manager that the boss is done.
        /// </summary>
        /// <param name="currentWave"> Index of the current wave </param>
        public void BossDead(int currentWave)
        {
            if (currentWave < reference.waveManager.waves.Length - 1)
            {
                if (reference.playerShip != null)
                {
                    reference.playerShip.GetComponent<PlayerHealth>().HealFull();
                }
            }

            reference.waveManager.BossWaveComplete(currentWave);
            Debug.Log(GetType().Name + " - Boss finished for Wave " + (currentWave + 1));
        }
    }
}