//=============================================================================
//
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// This file defines all logic related to subwaves within the spawn system.
// Subwaves use spawn zones and spawn groups to define what type, how many, and
// where to spawn enemies. There are 6 spawn zones. The Introduction zone is a
// single point where a new enemy is spawned when it is first introduced to the
// player. This is so new enemies are spawned front and center so the player
// won't be able to miss them. The other 5 zones are defined by a lower and
// upper bound. Enemies are spawned at a random point within these bounds. The
// idea behind subwaves is to give us more control over what enemies spawn when
// and where. This way we can have a subwave that only introduces a new enemy.
// 
// The spawn groups allow us to define a set of enemy types and a number for
// how many of each should spawn. It converts that into a list of type
// identifiers (for performance purposes) from the object pool manager. That
// list is then randomly shuffled to make each spawn group more unique and less
// hard-coded.
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hive.Armada.Enemies;
using SubjectNerd.Utilities;
using Random = System.Random;

namespace Hive.Armada.Game
{
    /// <summary>
    /// All spawn zones in the scene.
    /// </summary>
    public enum SpawnZone
    {
        /// <summary>
        /// The introduction spawn point that is right in front of the player's view.
        /// </summary>
        Introduction = 0,

        /// <summary>
        /// The main spawn region in front of the player.
        /// </summary>
        Center = 1,

        /// <summary>
        /// The spawn region that is in the front left.
        /// </summary>
        FrontLeft = 2,

        /// <summary>
        /// The spawn region that is in the front right.
        /// </summary>
        FrontRight = 3,

        /// <summary>
        /// The spawn region that is up in the back left.
        /// </summary>
        BackLeft = 4,

        /// <summary>
        /// The spawn region that is up in the back right.
        /// </summary>
        BackRight = 5
    }

    /// <summary>
    /// Contains all logic for spawning within a subwave.
    /// </summary>
    [DisallowMultipleComponent]
    public class Subwave : MonoBehaviour
    {
        /// <summary>
        /// Defines a type identifier for an enemy to spawn and which zone to spawn that enemy.
        /// </summary>
        public struct Spawn
        {
            /// <summary>
            /// The type identifier for the enemy prefab to spawn.
            /// </summary>
            public int typeIdentifier;

            /// <summary>
            /// The zone to spawn the enemy in.
            /// </summary>
            public SpawnZone spawnZone;

            /// <summary>
            /// Spawn constructor.
            /// </summary>
            /// <param name="typeIdentifier"> The type identifier for the enemy prefab to spawn </param>
            /// <param name="spawnZone"> The zone to spawn the enemy in </param>
            public Spawn(int typeIdentifier, SpawnZone spawnZone)
            {
                this.typeIdentifier = typeIdentifier;
                this.spawnZone = spawnZone;
            }
        }

        /// <summary>
        /// Merged SpawnGroup based on SpawnGroup's boolean spawnWithPrevious.
        /// </summary>
        public struct MergedSpawnGroup
        {
            /// <summary>
            /// Time delay before this group is spawned.
            /// </summary>
            public float spawnGroupDelay;

            /// <summary>
            /// Time delay between individual spawns in this group.
            /// </summary>
            public float spawnDelay;

            /// <summary>
            /// List of individual spawns for this group.
            /// </summary>
            public List<Spawn> spawns;

            /// <summary>
            /// MergedSpawnGroup constructor.
            /// </summary>
            /// <param name="spawnGroupDelay"> Time delay before this group is spawned </param>
            /// <param name="spawnDelay"> Time delay between individual spawns </param>
            /// <param name="spawns"> List of individual spawns </param>
            public MergedSpawnGroup(float spawnGroupDelay, float spawnDelay, List<Spawn> spawns)
            {
                this.spawnGroupDelay = spawnGroupDelay;
                this.spawnDelay = spawnDelay;
                this.spawns = spawns;
            }
        }

        /// <summary>
        /// Defines an enemy prefab and how many of it to spawn.
        /// </summary>
        [Serializable]
        public struct EnemySpawn
        {
            /// <summary>
            /// The prefab of the enemy to spawn.
            /// </summary>
            public GameObject enemyPrefab;

            /// <summary>
            /// How many of enemyPrefab to spawn.
            /// </summary>
            public int spawnCount;
        }

        /// <summary>
        /// Includes all variables needed to define a group of enemies including how many, where to, and what type to spawn.
        /// </summary>
        [Serializable]
        public struct SpawnGroup
        {
            /// <summary>
            /// The zone that this group will spawn in.
            /// </summary>
            [Tooltip("Which spawn zone will this group spawn in?")]
            public SpawnZone spawnZone;

            /// <summary>
            /// If true, this group spawns with the previous and ignores spawnGroupDelay.
            /// </summary>
            [Tooltip(
                "If true, this spawn group will spawn with the previous group and ignore Spawn Group Delay.")]
            public bool spawnWithPrevious;

            /// <summary>
            /// Delay before this group is spawned.
            /// </summary>
            [Tooltip("Time delay between this spawn group and the previous.")]
            [Range(0.0f, 100.0f)]
            public float spawnGroupDelay;

            /// <summary>
            /// Time delay between individual spawns in this group.
            /// </summary>
            [Tooltip("Time delay between individual spawns in this group.")]
            [Range(0.0f, 20.0f)]
            public float spawnDelay;

            /// <summary>
            /// </summary>
            [Tooltip("Array of enemy prefabs and number to spawn.")]
            [Reorderable("Enemy Spawn", false)]
            public EnemySpawn[] enemySpawns;
        }

        /// <summary>
        /// Reference manager that holds all needed references
        /// (e.g. spawner, game manager, etc.)
        /// </summary>
        private ReferenceManager reference;

        /// <summary>
        /// The ObjectPoolManager for Spawning and Despawning enemies.
        /// </summary>
        private ObjectPoolManager objectPoolManager;

        /// <summary>
        /// Used to randomly shuffle the spawn order inside of spawn groups.
        /// </summary>
        private Random random;

        /// <summary>
        /// Index position of the current wave in the game.
        /// </summary>
        public int WaveNumber { get; private set; }

        /// <summary>
        /// Index position of this subwave in the current wave.
        /// </summary>
        public int SubwaveNumber { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        [Tooltip("The most enemies that will be spawned if the player isn't shooting them." +
                 " Once the player shoots them, more will be spawned.")]
        public int enemyCap;

        /// <summary>
        /// Array of all spawn groups for this subwave.
        /// </summary>
        [Tooltip("Array of all spawn groups for this subwave, in order.")]
        [Reorderable("Spawn Group", false)]
        public SpawnGroup[] spawnGroups;

        /// <summary>
        /// 
        /// </summary>
        private List<MergedSpawnGroup> mergedSpawnGroups;

        /// <summary>
        /// Count of how many of the enemies from this subwave are still alive.
        /// Includes unspawned enemies.
        /// </summary>
        private int enemiesRemaining;

        /// <summary>
        /// How many enemies are queued up to spawn.
        /// This increases as spawned enemies die or are hit by the player.
        /// </summary>
        private int enemiesToSpawn;

        /// <summary>
        /// How many enemies we can spawn until we hit the cap.
        /// </summary>
        private int enemyCapCount;

        /// <summary>
        /// Used to limit new enemy spawning to require the player to actually shoot enemies.
        /// </summary>
        private bool canSpawn;

        /// <summary>
        /// Array of the spawn zone bounds from WaveManager.
        /// </summary>
        private WaveManager.SpawnZone[] spawnZones;

        /// <summary>
        /// If this subwave is currently running and still has spawn groups
        /// to spawn or enemies or there are still enemies alive.
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// If this subwave has run and all of its spawn groups have been killed.
        /// </summary>
        public bool IsComplete { get; private set; }

        /// <summary>
        /// Initializes the reference to the Reference Manager
        /// </summary>
        private void Awake()
        {
            reference = GameObject.Find("Reference Manager").GetComponent<ReferenceManager>();
            objectPoolManager = reference.objectPoolManager;

            spawnZones = reference.waveManager.spawnZones;
        }

        /// <summary>
        /// Prepares this subwave and then runs it by spawning all of the spawn groups in it.
        /// </summary>
        /// <param name="wave"> Current wave number </param>
        /// <param name="subwave"> This subwave's index in the current wave </param>
        public void Run(int wave, int subwave)
        {
            if (!IsRunning)
            {
                IsRunning = true;

                random = new Random((int) Time.time);
                WaveNumber = wave;
                SubwaveNumber = subwave;

                SetupSubwave();

                canSpawn = true;

                StartCoroutine(SpawnSubwave());
            }
            else
            {
                Debug.LogWarning("Wave " + WaveNumber + " Subwave " + SubwaveNumber +
                                 " is already running!");
            }
        }

        /// <summary>
        /// Builds an array of lists of all enemies that should be spawned in each subwave.
        /// </summary>
        private void SetupSubwave()
        {
            if (spawnGroups.Length > 0)
            {
                enemyCapCount = enemyCap;
                mergedSpawnGroups = new List<MergedSpawnGroup>();

                List<Spawn> spawns = new List<Spawn>();
                float groupDelay = -10.0f;
                float spawnDelay = -10.0f;

                for (int group = 0; group < spawnGroups.Length; ++group)
                {
                    /**
                     * If this group doesn't spawn with the previous and it isn't the first
                     *  group, we shuffle the spawns and add a new MergedSpawnGroup
                     *  to the list. Then, reset our intermediate variables.
                     */
                    if (!spawnGroups[group].spawnWithPrevious && group > 0)
                    {
                        Shuffle(spawns);
                        mergedSpawnGroups.Add(
                            new MergedSpawnGroup(groupDelay, spawnDelay, new List<Spawn>(spawns)));

                        // reset these, -10.0f just to be safe
                        spawns.Clear();
                        groupDelay = -10.0f;
                        spawnDelay = -10.0f;
                    }

                    // these are set to negative when they are "uninitialized"
                    // so we know when to get the current spawnGroup's values
                    if (groupDelay <= -1.0f || spawnDelay <= -1.0f)
                    {
                        groupDelay = spawnGroups[group].spawnGroupDelay;
                        spawnDelay = spawnGroups[group].spawnDelay;
                    }

                    for (int type = 0; type < spawnGroups[group].enemySpawns.Length; ++type)
                    {
                        for (int count = 0;
                             count < spawnGroups[group].enemySpawns[type].spawnCount;
                             ++count)
                        {
                            int typeIdentifier =
                                objectPoolManager.GetTypeIdentifier(
                                    spawnGroups[group].enemySpawns[type].enemyPrefab);

                            spawns.Add(new Spawn(typeIdentifier, spawnGroups[group].spawnZone));

                            ++enemiesToSpawn;
                        }
                    }
                }

                // The last group wasn't added because the for logic for
                // that is at the top of the for loop, so we add it here.
                Shuffle(spawns);
                mergedSpawnGroups.Add(
                    new MergedSpawnGroup(groupDelay, spawnDelay, new List<Spawn>(spawns)));
            }
            else
            {
                Debug.LogError("Wave " + WaveNumber + " Subwave " + SubwaveNumber +
                               " does not have any spawn groups!");
            }
        }

        /// <summary>
        /// Runs the spawning logic for the subwave.
        /// </summary>
        private IEnumerator SpawnSubwave()
        {
            for (int group = 0; group < mergedSpawnGroups.Count; ++group)
            {
                if (Math.Abs(mergedSpawnGroups[group].spawnGroupDelay) > 0.001f)
                {
                    // wait the group delay time
                    yield return new WaitForSeconds(mergedSpawnGroups[group].spawnGroupDelay);
                }

                // spawn all enemies in the spawn group
                for (int i = 0; i < mergedSpawnGroups[group].spawns.Count; ++i)
                {
                    // wait until we can spawn a new enemy
                    while (!canSpawn)
                    {
                        yield return new WaitForSeconds(0.1f);
                    }

                    if (Math.Abs(mergedSpawnGroups[group].spawnDelay) > 0.001f)
                    {
                        // wait the delay time
                        yield return new WaitForSeconds(mergedSpawnGroups[group].spawnDelay);
                    }
                    else
                    {
                        // wait until the next frame
                        yield return null;
                    }

                    Vector3 position;

                    if (mergedSpawnGroups[group].spawns[i].spawnZone != SpawnZone.Introduction)
                    {
                        SpawnZone zone = mergedSpawnGroups[group].spawns[i].spawnZone;

                        // spawn position is random point in its zone
                        Vector3 lower = spawnZones[(int) zone].lowerBound.transform.position;
                        Vector3 upper = spawnZones[(int) zone].upperBound.transform.position;

                        position = new Vector3(UnityEngine.Random.Range(lower.x, upper.x),
                                               UnityEngine.Random.Range(lower.y, upper.y),
                                               UnityEngine.Random.Range(lower.z, upper.z));
                    }
                    else
                    {
                        // spawn position is the introduction point
                        position = spawnZones[0].lowerBound.transform.position;
                    }

                    int typeIdentifier = mergedSpawnGroups[group].spawns[i].typeIdentifier;

                    // wait until we can spawn a new enemy
                    while (!canSpawn)
                    {
                        yield return new WaitForSeconds(0.1f);
                    }

                    GameObject spawned = objectPoolManager.Spawn(typeIdentifier, position);
                    spawned.GetComponent<Enemy>().SetSubwave(this);

                    --enemyCapCount;

                    if (enemyCapCount <= 0)
                    {
                        canSpawn = false;
                    }

                    ++enemiesRemaining;
                    --enemiesToSpawn;
                }
            }

            if (enemiesToSpawn > 0)
            {
                Debug.LogWarning(GetType().Name + " #" + SubwaveNumber + " - enemiesToSpawn = " +
                                 enemiesToSpawn);
            }

            StartCoroutine(WaitForSubwaveEnd());
        }

        /// <summary>
        /// Waits until all enemies for this subwave has been killed,
        /// then informs the Wave that this subwave has completed.
        /// </summary>
        private IEnumerator WaitForSubwaveEnd()
        {
            while (enemiesRemaining > 0)
            {
                yield return new WaitForSeconds(0.1f);
            }

            IsRunning = false;
            IsComplete = true;

            reference.waveManager.waves[WaveNumber].SubwaveComplete(SubwaveNumber);
        }

        /// <summary>
        /// This is called by enemies when they are first hit. Allows for more enemies to spawn.
        /// </summary>
        public void EnemyHit()
        {
            ++enemyCapCount;
            canSpawn = true;
        }

        /// <summary>
        /// This is called by enemies when they are killed. Allows for more enemies
        /// to spawn and tracks how many enemies are still alive this subwave.
        /// </summary>
        public void EnemyDead()
        {
            --enemiesRemaining;
            canSpawn = true;
        }

        /// <summary>
        /// Shuffles a list of game objects using the Fisher-Yates shuffle algorithm
        /// </summary>
        /// <param name="list"> The list to shuffle </param>
        private void Shuffle(IList<Spawn> list)
        {
            for (int i = 0; i < list.Count; ++i)
            {
                int j = random.Next(i, list.Count);
                Spawn value = list[i];
                list[i] = list[j];
                list[j] = value;
            }
        }
    }
}