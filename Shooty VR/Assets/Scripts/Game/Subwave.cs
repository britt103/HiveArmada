using System;
using System.Collections;
using System.Collections.Generic;
using SubjectNerd.Utilities;
using UnityEngine;
using Valve.VR;
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
    /// </summary>
    [DisallowMultipleComponent]
    public class Subwave : MonoBehaviour
    {
        /// <summary>
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
            /// Delay before this group is spawned.
            /// </summary>
            [Tooltip("Time delay between this spawn group and the previous.")]
            public float spawnGroupDelay;

            /// <summary>
            /// </summary>
            [Tooltip("Time delay between individual spawns in this group.")]
            public float spawnDelay;

            /// <summary>
            /// Array of enemy types that will be spawned.
            /// </summary>
            [Tooltip("The enemy types will be spawned.")]
            public GameObject[] enemyTypes;

            /// <summary>
            /// Number of each enemy type to spawn.
            /// </summary>
            [Tooltip("How many of each enemy type to spawn.")]
            public int[] enemyCounts;
        }

        /// <summary>
        /// Reference manager that holds all needed references
        /// (e.g. spawner, game manager, etc.)
        /// </summary>
        private ReferenceManager reference;

        /// <summary>
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
        /// Array of all spawn groups for this subwave.
        /// </summary>
        [Tooltip("Array of all spawn groups for this subwave, in order.")]
        [Reorderable("Spawn Group", false)]
        public SpawnGroup[] spawnGroups;

        /// <summary>
        /// Index of the current spawn group that needs to be spawned.
        /// </summary>
        private int currentSpawnGroup;

        /// <summary>
        /// Each list in this array is every enemy that will be spawned in a spawn group.
        /// </summary>
        private List<GameObject>[] spawns;

        /// <summary>
        /// Count of how many of the enemies from this subwave are still alive.
        /// </summary>
        private int enemiesRemaining;

        /// <summary>
        /// How many enemies are queued up to spawn. This increases as spawned enemies die or are hit by the player.
        /// </summary>
        private int enemiesToSpawn;

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
            reference = transform.parent.GetComponent<ReferenceManager>();

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
                random = new Random((int) Time.time);
                WaveNumber = wave;
                SubwaveNumber = subwave;

                SetupSubwave();
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
                spawns = new List<GameObject>[spawnGroups.Length];

                for (int group = 0; group < spawnGroups.Length; ++group)
                {
                    spawns[group] = new List<GameObject>();

                    for (int type = 0; type < spawnGroups[group].enemyTypes.Length; ++type)
                    {
                        for (int count = 0; count < spawnGroups[group].enemyCounts[type]; ++count)
                        {
                            spawns[group].Add(spawnGroups[group].enemyTypes[type]);
                        }
                    }

                    Shuffle(spawns[group]);
                }
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
            if (Math.Abs(spawnGroups[currentSpawnGroup].spawnGroupDelay) > 0.001f)
            {
                // wait the group delay time
                yield return new WaitForSeconds(spawnGroups[currentSpawnGroup].spawnGroupDelay);
            }

            for (int i = 0; i < spawns[currentSpawnGroup].Count; ++i)
            {
                if (Math.Abs(spawnGroups[currentSpawnGroup].spawnDelay) > 0.001f)
                {
                    // wait the delay time
                    yield return new WaitForSeconds(spawnGroups[currentSpawnGroup].spawnDelay);
                }
                else
                {
                    // wait until the next frame
                    yield return null;
                }

                Vector3 position;

                if (spawnGroups[currentSpawnGroup].spawnZone != SpawnZone.Introduction)
                {
                    Vector3 lower = spawnZones[(int) spawnGroups[currentSpawnGroup].spawnZone]
                        .lowerBound.transform.position;
                    Vector3 upper = spawnZones[(int) spawnGroups[currentSpawnGroup].spawnZone]
                        .upperBound.transform.position;
                }
            }
        }

        /// <summary>
        /// Shuffles a list of game objects using the Fisher-Yates shuffle algorithm
        /// </summary>
        /// <param name="list"> The list to shuffle </param>
        private void Shuffle(List<GameObject> list)
        {
            for (int i = 0; i < list.Count; ++i)
            {
                int j = random.Next(i, list.Count);
                GameObject value = list[i];
                list[i] = list[j];
                list[j] = value;
            }
        }
    }
}