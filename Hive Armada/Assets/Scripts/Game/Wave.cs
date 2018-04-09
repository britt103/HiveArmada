//=============================================================================
// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-440-01, CPSC-340-01 & CPSC-344-01
// Group Project
// 
// 
//=============================================================================

using Hive.Armada.Enemies;
using SubjectNerd.Utilities;
using System;
using System.Collections;
using UnityEngine;

namespace Hive.Armada.Game
{
    /// <summary>
    /// 
    /// </summary>
    public enum SpawnPaths
    {
        /// <summary>
        /// 
        /// </summary>
        Zero,

        /// <summary>
        /// 
        /// </summary>
        One,

        /// <summary>
        /// 
        /// </summary>
        Two,

        /// <summary>
        /// 
        /// </summary>
        Three,

        /// <summary>
        /// 
        /// </summary>
        Four,

        /// <summary>
        /// 
        /// </summary>
        Five,

        /// <summary>
        /// 
        /// </summary>
        Six,

        /// <summary>
        /// 
        /// </summary>
        Seven,

        /// <summary>
        /// 
        /// </summary>
        Eight,

        /// <summary>
        /// 
        /// </summary>
        Nine,

        /// <summary>
        /// 
        /// </summary>
        Ten,

        /// <summary>
        /// 
        /// </summary>
        Eleven,

        /// <summary>
        /// 
        /// </summary>
        Twelve
    }

    /// <summary>
    /// 
    /// </summary>
    public enum PowerupSpawnPoint
    {
        NoPowerup,
        Left,
        Center,
        Right
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public struct SetupNormalSpawnPath
    {
        /// <summary>
        /// 
        /// </summary>
        public SpawnPaths path;

        /// <summary>
        /// 
        /// </summary>
        public EnemyType enemy;

        /// <summary>
        /// 
        /// </summary>
        public AttackPattern attackPattern;
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public struct SetupNormalSpawnZone
    {
        /// <summary>
        /// 
        /// </summary>
        public SpawnZone zone;

        /// <summary>
        /// 
        /// </summary>
        public SetupNormalSpawnPath[] setupSpawnPaths;
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public struct SetupNormalPowerupSpawn
    {
        /// <summary>
        /// 
        /// </summary>
        public PowerupSpawnPoint powerupSpawn;

        /// <summary>
        /// 
        /// </summary>
        public Powerups powerup;
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public struct SetupNormalSpawnGroup
    {
        /// <summary>
        /// 
        /// </summary>
        public float delay;

        /// <summary>
        /// 
        /// </summary>
        public SetupNormalSpawnZone[] setupSpawnZones;

        /// <summary>
        /// 
        /// </summary>
        public SetupNormalPowerupSpawn powerupSpawn;
    }

    /// <summary>
    /// 
    /// </summary>
    [DisallowMultipleComponent]
    public class Wave : MonoBehaviour
    {
        /// <summary>
        /// Reference manager that holds all needed references
        /// (e.g. spawner, game manager, etc.)
        /// </summary>
        private ReferenceManager reference;

        /// <summary>
        /// The WaveManager
        /// </summary>
        private WaveManager waveManager;

        /// <summary>
        /// The ObjectPoolManager for Spawning and Despawning enemies.
        /// </summary>
        private ObjectPoolManager objectPoolManager;

        /// <summary>
        /// Index position of the current wave in the game.
        /// </summary>
        public int WaveNumber { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        [Header("Spawn Groups")]
        [Reorderable("Spawn Group", false)]
        public SetupNormalSpawnGroup[] setupSpawnGroups;

        /// <summary>
        /// Count of how many of the enemies from this wave are still alive.
        /// Includes unspawned enemies.
        /// </summary>
        private int enemiesRemaining;

        /// <summary>
        /// If this wave is currently running and still has spawn groups
        /// to spawn or enemies or there are still enemies alive.
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// If this wave has run and all of its spawn groups have been killed.
        /// </summary>
        public bool IsComplete { get; private set; }

        /// <summary>
        /// Initializes the Reference Manager and Object Pool Manager references
        /// </summary>
        private void Awake()
        {
            reference = GameObject.Find("Reference Manager").GetComponent<ReferenceManager>();
            waveManager = reference.waveManager;
            objectPoolManager = reference.objectPoolManager;
        }

        /// <summary>
        /// Runs this wave by spawning all of the spawn groups in it.
        /// </summary>
        /// <param name="wave"> This wave's index </param>
        public void Run(int wave)
        {
            if (!IsRunning)
            {
                IsRunning = true;

                WaveNumber = wave;

                Debug.Log("Wave #" + (WaveNumber + 1) + " is beginning.");

                StartCoroutine(Spawn());
            }
            else
            {
                Debug.LogWarning("Wave " + (WaveNumber + 1) + " is already running.");
            }
        }

        /// <summary>
        /// Runs the spawning logic for the wave.
        /// </summary>
        private IEnumerator Spawn()
        {
            foreach (SetupNormalSpawnGroup group in setupSpawnGroups)
            {
                yield return new WaitForSeconds(Mathf.Abs(group.delay));

                while (enemiesRemaining > 0)
                {
                    yield return new WaitForSeconds(0.1f);
                }

                foreach (SetupNormalSpawnZone zone in group.setupSpawnZones)
                {
                    Vector3 position;

                    if (zone.zone == SpawnZone.Center ||
                        zone.zone == SpawnZone.FrontLeft ||
                        zone.zone == SpawnZone.FrontRight)
                    {
                        position = waveManager.enemySpawnPoints[0].position;
                    }
                    else
                    {
                        position = waveManager.enemySpawnPoints[1].position;
                    }

                    Quaternion rotation;

                    if (reference.playerShip != null)
                    {
                        // spawn the enemy looking at the player
                        rotation =
                            Quaternion.LookRotation(
                                reference.playerShip.transform.position - position);
                    }
                    else
                    {
                        // no player in the scene, spawn looking about where they should be
                        rotation =
                            Quaternion.LookRotation(new Vector3(0.0f, 2.0f, 0.0f) - position);
                    }

                    foreach (SetupNormalSpawnPath thisPath in zone.setupSpawnPaths)
                    {
                        string pathName = waveManager.PathNames[(int)zone.zone] + (int)thisPath.path;

                        GameObject spawned =
                            objectPoolManager.Spawn(
                                gameObject, (short)waveManager.EnemyIDs[(int)thisPath.enemy],
                                position,
                                rotation);

                        Enemy spawnedEnemyScript = spawned.GetComponent<Enemy>();
                        spawnedEnemyScript.SetWave(WaveNumber);
                        spawnedEnemyScript.SetAttackPattern(thisPath.attackPattern);

                        Hashtable moveHash = new Hashtable
                                             {
                                                 {"easetype", iTween.EaseType.easeInOutSine},
                                                 {"time", 3.0f},
                                                 {"looktarget", reference.shipLookTarget.transform},
                                                 {"onComplete", "OnPathingComplete"},
                                                 {"onCompleteTarget", spawned},
                                                 {"path", iTweenPath.GetPath(pathName)}
                                             };

                        iTween.MoveTo(spawned, moveHash);
                        ++enemiesRemaining;

                        if (spawned.name.Contains("Enemy_Splitter"))
                        {
                            // accounting for splitter children that the splitter spawns
                            enemiesRemaining += 4;
                        }
                    }
                }

                if (group.powerupSpawn.powerupSpawn != PowerupSpawnPoint.NoPowerup)
                {
                    Vector3 spawnPoint = waveManager.powerupSpawnPoints[(int)group.powerupSpawn.powerupSpawn - 1].position;

                    Instantiate(waveManager.powerupPrefabs[(int)group.powerupSpawn.powerup], spawnPoint, Quaternion.identity);

                    if (!waveManager.spawnedPowerupOnce)
                    {
                        waveManager.spawnedPowerupOnce = true;
                        reference.tooltips.SpawnGrabPowerup((int)group.powerupSpawn.powerupSpawn - 1);
                    }
                }
            }

            StartCoroutine(WaitForWaveEnd());
        }

        /// <summary>
        /// Waits until all enemies for this wave has been killed,
        /// then informs the WaveManager that this wave has completed.
        /// </summary>
        private IEnumerator WaitForWaveEnd()
        {
            while (enemiesRemaining > 0)
            {
                yield return new WaitForSeconds(0.1f);
            }

            IsRunning = false;
            IsComplete = true;

            Debug.Log("Wave #" + (WaveNumber + 1) + " is complete.");

            reference.waveManager.WaveComplete(WaveNumber);
        }

        public void EnemyDead()
        {
            --enemiesRemaining;
        }
    }
}