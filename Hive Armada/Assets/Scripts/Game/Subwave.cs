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
using Hive.Armada.Menus;
using SubjectNerd.Utilities;
using Random = System.Random;

namespace Hive.Armada.Game
{
    /// <summary>
    /// Defines an enemy prefab, how many to spawn, and what attack pattern to use.
    /// </summary>
    [Serializable]
    public struct SetupEnemySpawn
    {
        /// <summary>
        /// The prefab of the enemy to spawn.
        /// </summary>
        public GameObject enemyPrefab;

        /// <summary>
        /// How many of enemyPrefab to spawn.
        /// </summary>
        public int spawnCount;

        /// <summary>
        /// What attack pattern this enemy should use.
        /// </summary>
        [Tooltip("Each enemy has different patterns of how they either shoot at the player" +
                 " or how they move throughout the scene.")]
        public AttackPattern attackPattern;
    }

    /// <summary>
    /// Defines a powerup to spawn after a random time defined by a base and range value.
    /// </summary>
    [Serializable]
    public struct SetupPowerupSpawn
    {
        /// <summary>
        /// The prefab of the powerup to spawn.
        /// </summary>
        public GameObject powerupPrefab;

        /// <summary>
        /// Base time for the powerup to spawn.
        /// </summary>
        [Tooltip("Base time for the powerup to spawn.")]
        [Range(0.0f, 100.0f)]
        public float spawnTimeDelayBase;

        /// <summary>
        /// Range that the powerup can spawn within. A random number is generated between 0 and
        /// this value. That time is then added to the base to give the final spawn time for the
        /// powerup.
        /// </summary>
        [Tooltip("A random number is generated between 0 and Spawn Time Delay Range. That value" +
                 " is then added to Spawn Time Delay Base to give the final spawn time for the" +
                 " powerup.")]
        [Range(0.0f, 100.0f)]
        public float spawnTimeDelayRange;
    }

    /// <summary>
    /// Includes all variables needed to define a group of enemies including how many to spawn,
    /// what type to spawn, where to spawn, when to spawn, and what attack pattern they should use.
    /// </summary>
    [Serializable]
    public struct SetupSpawnGroup
    {
        /// <summary>
        /// The zone that this group will spawn in.
        /// </summary>
        [Tooltip("Which spawn zone will this group spawn in?")]
        public SpawnZone spawnZone;

        /// <summary>
        /// If true, this group spawns with the previous and ignores spawnGroupDelay.
        /// </summary>
        [Tooltip("If true, this spawn group will spawn with the previous" +
                 " group and ignore Spawn Group Delay.")]
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
        /// Array of enemy types to spawn, with options.
        /// </summary>
        [Tooltip("Array of enemy types to spawn, with options. Duplicate types are valid.")]
        public SetupEnemySpawn[] setupEnemySpawns;

        /// <summary>
        /// Array of powerups to spawn using base and range times.
        /// </summary>
        [Tooltip("Array of powerups to spawn using base and range times." +
                 " Duplicate types are valid.")]
        public SetupPowerupSpawn[] setupPowerupSpawns;
    }

    /// <summary>
    /// Defines a type identifier for an enemy, which zone
    /// to spawn it in, and what attack pattern to use.
    /// </summary>
    [Serializable]
    public struct EnemySpawn
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
        /// The attack pattern this enemy will use.
        /// </summary>
        public AttackPattern attackPattern;

        /// <summary>
        /// Enemy spawn constructor.
        /// </summary>
        /// <param name="typeIdentifier"> The type identifier for the enemy prefab to spawn </param>
        /// <param name="spawnZone"> The zone to spawn the enemy in </param>
        /// <param name="attackPattern"> The attack pattern for this enemy to use </param>
        public EnemySpawn(int typeIdentifier, SpawnZone spawnZone, AttackPattern attackPattern)
        {
            this.typeIdentifier = typeIdentifier;
            this.spawnZone = spawnZone;
            this.attackPattern = attackPattern;
        }
    }

    /// <summary>
    /// Defines a type identifier for a powerup and how long into the wave it will start.
    /// </summary>
    [Serializable]
    public struct PowerupSpawn
    {
        /// <summary>
        /// The prefab for the powerup to spawn.
        /// </summary>
        public GameObject powerupPrefab;

        /// <summary>
        /// How long into the wave this powerup will be spawned, in seconds.
        /// </summary>
        public float spawnTime;

        /// <summary>
        /// Powerup spawn constructor.
        /// </summary>
        /// <param name="powerupPrefab"> The prefab for the powerup to spawn </param>
        /// <param name="spawnTime"> How long into the wave this powerup will spawn, in seconds </param>
        public PowerupSpawn(GameObject powerupPrefab, float spawnTime)
        {
            this.powerupPrefab = powerupPrefab;
            this.spawnTime = spawnTime;
        }
    }

    /// <summary>
    /// Converted version of SetupSpawnGroup that merges groups that spawn together
    /// and swaps GameObject references to enemies with type identifiers.
    /// </summary>
    [Serializable]
    public struct SpawnGroup
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
        public List<EnemySpawn> enemySpawns;

        /// <summary>
        /// List of powerups to spawn and when.
        /// </summary>
        public List<PowerupSpawn> powerupSpawns;

        /// <summary>
        /// SpawnGroup constructor.
        /// </summary>
        /// <param name="spawnGroupDelay"> Time delay before this group is spawned </param>
        /// <param name="spawnDelay"> Time delay between individual spawns </param>
        /// <param name="enemySpawns"> List of individual spawns </param>
        /// <param name="powerupSpawns"> List of powerup spawns </param>
        public SpawnGroup(float spawnGroupDelay, float spawnDelay, List<EnemySpawn> enemySpawns,
                          List<PowerupSpawn> powerupSpawns)
        {
            this.spawnGroupDelay = spawnGroupDelay;
            this.spawnDelay = spawnDelay;
            this.enemySpawns = enemySpawns;
            this.powerupSpawns = powerupSpawns;
        }
    }

    /// <summary>
    /// Contains all logic for spawning within a subwave.
    /// </summary>
    [DisallowMultipleComponent]
    public class Subwave : MonoBehaviour
    {
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
        /// Maximum enemies to spawn until the player begins shooting them.
        /// </summary>
        [Tooltip("The most enemies that will be spawned if the player isn't shooting them." +
                 " Once the player shoots them, more will be spawned.")]
        public int enemyCap;

        /// <summary>
        /// Array of all setup spawn groups for this subwave.
        /// </summary>
        [Tooltip("Array of all spawn groups for this subwave, in order.")]
        [Reorderable("Spawn Group", false)]
        public SetupSpawnGroup[] setupSpawnGroups;

        /// <summary>
        /// List of the finalized spawn groups for this subwave.
        /// </summary>
        private List<SpawnGroup> spawnGroups;

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
        private SpawnZoneBounds[] spawnZones;

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
        /// Reference to the coroutine that spawns powerups.
        /// </summary>
        private Coroutine powerupCoroutine;

        /// <summary>
        /// Wave timer used to spawn powerups.
        /// </summary>
        private float powerupTimer;

        /// <summary>
        /// List of enemies that need respawned.
        /// </summary>
        private List<EnemySpawn> respawns;

        /// <summary>
        /// Time delay between respawning enemies.
        /// </summary>
        private float respawnDelay;

        /// <summary>
        /// Reference to LexiconUnlockData.
        /// </summary>
        private LexiconUnlockData unlockData;

        /// <summary>
        /// Initializes the reference to the Reference Manager
        /// </summary>
        private void Awake()
        {
            reference = GameObject.Find("Reference Manager").GetComponent<ReferenceManager>();
            objectPoolManager = reference.objectPoolManager;
            respawns = new List<EnemySpawn>();

            spawnZones = reference.waveManager.spawnZonesBounds;

            unlockData = FindObjectOfType<LexiconUnlockData>();
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
                Debug.LogWarning("Wave " + WaveNumber + " Subwave " +
                                 SubwaveNumber + " is already running!");
            }
        }

        /// <summary>
        /// Builds an array of lists of all enemies that should be spawned in each subwave.
        /// </summary>
        private void SetupSubwave()
        {
            if (setupSpawnGroups.Length > 0)
            {
                UnityEngine.Random.InitState((int) Time.time);
                enemyCapCount = enemyCap;
                spawnGroups = new List<SpawnGroup>();

                List<EnemySpawn> enemySpawns = new List<EnemySpawn>();
                float groupDelay = -10.0f;
                float spawnDelay = -10.0f;

                List<PowerupSpawn> powerupSpawns = new List<PowerupSpawn>();

                for (int group = 0; group < setupSpawnGroups.Length; ++group)
                {
                    /**
                     * If this group doesn't spawn with the previous and it isn't the first
                     *  group, we shuffle the spawns and add a new SpawnGroup
                     *  to the list. Then, reset our intermediate variables.
                     */
                    if (!setupSpawnGroups[group].spawnWithPrevious && group > 0)
                    {
                        Shuffle(enemySpawns);
                        spawnGroups.Add(new SpawnGroup(groupDelay,
                                                       spawnDelay,
                                                       new List<EnemySpawn>(enemySpawns),
                                                       new List<PowerupSpawn>(powerupSpawns)));

                        // reset these, -10.0f just to be safe
                        enemySpawns.Clear();
                        groupDelay = -10.0f;
                        spawnDelay = -10.0f;
                        powerupSpawns.Clear();
                    }

                    // these are set to negative when they are "uninitialized"
                    // so we know when to get the current spawnGroup's values
                    if (groupDelay <= -1.0f || spawnDelay <= -1.0f)
                    {
                        groupDelay = setupSpawnGroups[group].spawnGroupDelay;
                        spawnDelay = setupSpawnGroups[group].spawnDelay;
                    }

                    for (int type = 0;
                         type < setupSpawnGroups[group].setupEnemySpawns.Length;
                         ++type)
                    {
                        for (int count = 0;
                             count < setupSpawnGroups[group].setupEnemySpawns[type].spawnCount;
                             ++count)
                        {
                            int typeIdentifier =
                                objectPoolManager.GetTypeIdentifier(
                                    setupSpawnGroups[group].setupEnemySpawns[type].enemyPrefab);

                            SpawnZone spawnZone = setupSpawnGroups[group].spawnZone;

                            AttackPattern attackPattern = setupSpawnGroups[group]
                                .setupEnemySpawns[type].attackPattern;

                            enemySpawns.Add(
                                new EnemySpawn(typeIdentifier, spawnZone, attackPattern));

                            //signal unlock for spawned enemy
                            unlockData.AddUnlock(setupSpawnGroups[group].setupEnemySpawns[type]
                                .enemyPrefab.name);

                            ++enemiesToSpawn;
                        }
                    }

                    for (int powerup = 0;
                         powerup < setupSpawnGroups[group].setupPowerupSpawns.Length; ++powerup)
                    {
                        GameObject powerupPrefab = setupSpawnGroups[group]
                            .setupPowerupSpawns[powerup].powerupPrefab;
                        float spawnTimeDelayBase = setupSpawnGroups[group]
                            .setupPowerupSpawns[powerup].spawnTimeDelayBase;
                        float spawnTimeDelayRange = setupSpawnGroups[group]
                            .setupPowerupSpawns[powerup].spawnTimeDelayRange;

                        float spawnTime = spawnTimeDelayBase +
                                          UnityEngine.Random.Range(0.0f, spawnTimeDelayRange);

                        powerupSpawns.Add(new PowerupSpawn(powerupPrefab, spawnTime));
                    }
                }

                // The last group wasn't added because the for logic for
                // that is at the top of the for loop, so we add it here.
                Shuffle(enemySpawns);
                spawnGroups.Add(
                    new SpawnGroup(groupDelay, spawnDelay, new List<EnemySpawn>(enemySpawns),
                                   new List<PowerupSpawn>(powerupSpawns)));
            }
            else
            {
                Debug.LogError("Wave " + WaveNumber + " Subwave " +
                               SubwaveNumber + " does not have any spawn groups!");
            }
        }

        /// <summary>
        /// Runs the spawning logic for the subwave.
        /// </summary>
        private IEnumerator SpawnSubwave()
        {
            for (int group = 0; group < spawnGroups.Count; ++group)
            {
                if (spawnGroups[group].powerupSpawns.Count > 0)
                {
                    // stop the powerup coroutine if the previous ran over.
                    if (powerupCoroutine != null)
                    {
                        StopCoroutine(powerupCoroutine);
                    }

                    powerupCoroutine =
                        StartCoroutine(PowerupSpawn(spawnGroups[group].powerupSpawns));
                }

                if (Math.Abs(spawnGroups[group].spawnGroupDelay) > 0.001f)
                {
                    // wait the group delay time
                    yield return new WaitForSeconds(spawnGroups[group].spawnGroupDelay);
                }

                // spawn all enemies in the spawn group
                for (int i = 0; i < spawnGroups[group].enemySpawns.Count; ++i)
                {
                    respawnDelay = spawnGroups[group].spawnDelay;

                    // wait until we can spawn a new enemy
                    while (!canSpawn)
                    {
                        yield return new WaitForSeconds(0.1f);
                    }

                    if (Math.Abs(spawnGroups[group].spawnDelay) > 0.001f)
                    {
                        // wait the delay time
                        yield return new WaitForSeconds(spawnGroups[group].spawnDelay);
                    }
                    else
                    {
                        // wait until the next frame
                        yield return null;
                    }

                    Vector3 position;
                    SpawnZone zone = spawnGroups[group].enemySpawns[i].spawnZone;
                    if (zone != SpawnZone.Introduction)
                    {
                        // spawn position is random point in its zone
                        Vector3 lower = spawnZones[(int) zone].lowerBound.transform.position;
                        Vector3 upper = spawnZones[(int) zone].upperBound.transform.position;

                        float radius;
                        bool isColliding;

                        // set the collision radius for the enemy we're about to spawn
                        switch (spawnGroups[group].enemySpawns[i].typeIdentifier)
                        {
                            case 0:
                                radius = 0.4f;
                                break;
                            case 1:
                                radius = 0.4f;
                                break;
                            case 2:
                                radius = 0.24f;
                                break;
                            case 3:
                                radius = 0.9f;
                                break;
                            default:
                                radius = 0.4f;
                                break;
                        }

                        do
                        {
                            // find a position that isn't colliding with any enemies
                            position = new Vector3(UnityEngine.Random.Range(lower.x, upper.x),
                                                   UnityEngine.Random.Range(lower.y, upper.y),
                                                   UnityEngine.Random.Range(lower.z, upper.z));

                            Collider[] colliders =
                                Physics.OverlapSphere(position, radius, Utility.enemyMask);

                            isColliding = colliders.Length > 0;

                            // wait one frame to throttle this do-while
                            yield return null;
                        }
                        while (isColliding);
                    }
                    else
                    {
                        // spawn position is the introduction point
                        position = spawnZones[0].lowerBound.transform.position;
                    }

                    int typeIdentifier = spawnGroups[group].enemySpawns[i].typeIdentifier;

                    // wait until we can spawn a new enemy
                    while (!canSpawn)
                    {
                        yield return new WaitForSeconds(0.1f);
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

                    GameObject spawned =
                        objectPoolManager.Spawn(typeIdentifier, position, rotation);

                    // initialize enemy subwave reference, EnemySpawn
                    // for respawning, and attack pattern.
                    Enemy spawnedEnemyScript = spawned.GetComponent<Enemy>();
                    spawnedEnemyScript.SetSubwave(this);
                    spawnedEnemyScript.SetEnemySpawn(spawnGroups[group].enemySpawns[i]);
                    spawnedEnemyScript.SetAttackPattern(
                        spawnGroups[group].enemySpawns[i].attackPattern);

                    --enemyCapCount;

                    if (enemyCapCount <= 0)
                    {
                        canSpawn = false;
                    }

                    ++enemiesRemaining;

                    if (spawned.name.Contains("Enemy_Splitter"))
                    {
                        // accounting for splitter children that the splitter spawns
                        enemiesRemaining += 4;
                    }

                    --enemiesToSpawn;
                }
            }

            if (enemiesToSpawn > 0)
            {
                Debug.LogWarning(GetType().Name + " #" + SubwaveNumber +
                                 " - enemiesToSpawn = " + enemiesToSpawn);
            }

            StartCoroutine(WaitForSubwaveEnd());
        }

        /// <summary>
        /// Spawns all powerups for the subwave.
        /// </summary>
        private IEnumerator PowerupSpawn(List<PowerupSpawn> powerupSpawns)
        {
            powerupTimer = 0.0f;

            float spawnTime = powerupSpawns[0].spawnTime;

            while (powerupSpawns.Count > 0)
            {
                powerupTimer += 0.1f;
                yield return new WaitForSeconds(0.1f);

                if (spawnTime > powerupTimer)
                {
                    // spawn position is random point in the powerup zone
                    Vector3 lower = reference
                        .waveManager.powerupSpawnZone.lowerBound.transform.position;
                    Vector3 upper = reference
                        .waveManager.powerupSpawnZone.upperBound.transform.position;

                    Vector3 position = new Vector3(UnityEngine.Random.Range(lower.x, upper.x),
                                                   UnityEngine.Random.Range(lower.y, upper.y),
                                                   UnityEngine.Random.Range(lower.z, upper.z));

                    Instantiate(powerupSpawns[0].powerupPrefab, position, Quaternion.identity);

                    powerupSpawns.RemoveAt(0);

                    if (powerupSpawns.Count > 0)
                    {
                        spawnTime = powerupSpawns[0].spawnTime;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            powerupCoroutine = null;
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

                if (respawns.Count > 0)
                {
                    yield return new WaitForSeconds(respawnDelay);

                    Vector3 position;

                    if (respawns[0].spawnZone != SpawnZone.Introduction)
                    {
                        SpawnZone zone = respawns[0].spawnZone;

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

                    GameObject spawned =
                        objectPoolManager.Spawn(respawns[0].typeIdentifier, position);

                    // set info for the enemy
                    Enemy spawnedEnemyScript = spawned.GetComponent<Enemy>();
                    spawnedEnemyScript.SetSubwave(this);
                    spawnedEnemyScript.SetEnemySpawn(
                        new EnemySpawn(respawns[0].typeIdentifier, respawns[0].spawnZone,
                                       respawns[0].attackPattern));
                    spawnedEnemyScript.SetAttackPattern(respawns[0].attackPattern);

                    respawns.RemoveAt(0);
                }
            }

            if (powerupCoroutine != null)
            {
                StopCoroutine(powerupCoroutine);
            }

            IsRunning = false;
            IsComplete = true;

            Debug.Log("Subwave #" + WaveNumber + "-" + SubwaveNumber + " is complete");

            reference.waveManager.waves[WaveNumber].SubwaveComplete(SubwaveNumber);
        }

        /// <summary>
        /// Adds an enemy to be respawned.
        /// </summary>
        /// <param name="enemySpawn"> The info for the enemy to respawn </param>
        public void AddRespawn(EnemySpawn enemySpawn)
        {
            respawns.Add(enemySpawn);
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
        private void Shuffle(IList<EnemySpawn> list)
        {
            for (int i = 0; i < list.Count; ++i)
            {
                int j = random.Next(i, list.Count);
                EnemySpawn value = list[i];
                list[i] = list[j];
                list[j] = value;
            }
        }
    }
}