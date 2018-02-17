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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hive.Armada.Enemies;
using Random = System.Random;
using SubjectNerd.Utilities;

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

                StartCoroutine(Spawn());
            }
            else
            {
                Debug.LogWarning("Wave " + WaveNumber + " is already running.");
            }
        }

        /// <summary>
        /// Runs the spawning logic for the wave.
        /// </summary>
        private IEnumerator Spawn()
        {
            for (int group = 0; group < setupSpawnGroups.Length; ++group)
            {
                SetupNormalSpawnGroup thisGroup = setupSpawnGroups[group];

                yield return new WaitForSeconds(Mathf.Abs(setupSpawnGroups[group].delay));

                for (int zone = 0; zone < thisGroup.setupSpawnZones.Length; ++zone)
                {
                    SetupNormalSpawnZone thisZone = thisGroup.setupSpawnZones[zone];

                    SpawnZone spawnZone = thisZone.zone;

                    Vector3 position;

                    if (thisZone.zone == SpawnZone.Center ||
                        thisZone.zone == SpawnZone.FrontLeft ||
                        thisZone.zone == SpawnZone.FrontRight)
                    {
                        position = GameObject.Find("FrontSpawn").transform.position;
                    }
                    else
                    {
                        position = GameObject.Find("BackSpawn").transform.position;
                    }

                    Quaternion rotation;

                    if (reference.playerShip)
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

                    for (int path = 0; path < thisZone.setupSpawnPaths.Length; ++path)
                    {
                        SetupNormalSpawnPath thisPath = thisZone.setupSpawnPaths[path];

                        string pathName = waveManager.PathNames[(int)thisZone.zone] + (int)thisPath.path;

                        GameObject spawned =
                            objectPoolManager.Spawn(
                                waveManager.EnemyIDs[(int)thisPath.enemy],
                                position,
                                rotation);

                        Enemy spawnedEnemyScript = spawned.GetComponent<Enemy>();
                        spawnedEnemyScript.SetAttackPattern(thisPath.attackPattern);

                        Hashtable moveHash = new Hashtable();

                        moveHash.Add("easetype", iTween.EaseType.easeInOutSine);
                        moveHash.Add("time", 3.0f);
                        moveHash.Add("looktarget", reference.player.transform);
                        moveHash.Add("onComplete", "AfterEnemyMove");
                        moveHash.Add("onCompleteTarget", spawned);
                        moveHash.Add("path", iTweenPath.GetPath(pathName));
                        iTween.MoveTo(spawned, moveHash);

                        ++enemiesRemaining;

                        if (spawned.name.Contains("Enemy_Splitter"))
                        {
                            // accounting for splitter children that the splitter spawns
                            enemiesRemaining += 4;
                        }
                    }
                }

                if (thisGroup.powerupSpawn.powerupSpawn != PowerupSpawnPoint.NoPowerup)
                {
                    Vector3 spawnPoint = waveManager.powerupSpawnPoints[(int)thisGroup.powerupSpawn.powerupSpawn - 1].position;

                    Instantiate(waveManager.powerupPrefabs[(int)thisGroup.powerupSpawn.powerup], spawnPoint, Quaternion.identity);
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

            Debug.Log("Wave #" + WaveNumber + " is complete");

            reference.waveManager.WaveComplete(WaveNumber);
        }
    }
}