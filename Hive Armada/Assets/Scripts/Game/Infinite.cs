//=============================================================================
//
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-440-01, CPSC-340-01 & CPSC-344-01
// Group Project
//
// This is the manager for the infinite game mode.
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using Hive.Armada.Enemies;
using SubjectNerd.Utilities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Hive.Armada.Game
{
    /// <summary>
    /// </summary>
    public class Infinite : MonoBehaviour
    {
        [Serializable]
        public struct SpawnChance
        {
            public EnemyType enemyType;

            [Range(0.0f, 1.0f)]
            public float percentChance;

            [Range(1, 100)]
            public int firstWave;
        }

        [Serializable]
        public struct PowerupSpawnChance
        {
            public Powerups powerup;

            [Range(0.0f, 1.0f)]
            public float percentChance;

            [Range(1, 100)]
            public int firstWave;
        }

        [Serializable]
        private struct Chances
        {
            public int LowerLimit { get; private set; }

            public int UpperLimit { get; private set; }

            public int FirstWave { get; private set; }

            public Chances(int lowerLimit, int upperLimit, int firstWave)
                : this()
            {
                LowerLimit = lowerLimit;
                UpperLimit = upperLimit;
                FirstWave = firstWave;
            }
        }

        [Serializable]
        private struct CurrentChances
        {
            public int LowerLimit { get; private set; }

            public int UpperLimit { get; private set; }

            public CurrentChances(int lowerLimit, int upperLimit)
                : this()
            {
                LowerLimit = lowerLimit;
                UpperLimit = upperLimit;
            }
        }

        /// <summary>
        /// Reference manager that holds all needed references
        /// (e.g. spawner, game manager, etc.)
        /// </summary>
        private ReferenceManager reference;

        private WaveManager waveManager;

        [Header("Enemy Setup")]
        [Reorderable("Enemy", false)]
        public SpawnChance[] enemySpawnChanceSetup;

        private Chances[] enemySpawnChances;

        private int[] typeIds;

        private Chances[] attackPatternChances =
        {
            new Chances(0, 49, 1),
            new Chances(50, 69, 5),
            new Chances(70, 84, 7),
            new Chances(85, 99, 9)
        };

        [Header("Powerup Setup")]
        [Reorderable("Powerup", false)]
        public PowerupSpawnChance[] powerupSpawnChanceSetup;

        private Chances[] powerupSpawnChances;

        private GameObject[] powerupPrefabs;

        [Header("Wave Setup")]
        public float waveLength = 60.0f;

        [Range(0.0f, 100.0f)]
        public float spawnChance = 0.8f;

        private int intSpawnChance;

        public int minSpawns = 10;

        public int maxSpawns = 25;

        public int maxEnemies;

        public float preemptTime;

        [Range(0.0f, 100.0f)]
        public float powerupSpawnChance = 0.8f;

        private int intPowerupSpawnChance;

        private readonly float powerupLowerDelay = 10.0f;

        private readonly float powerupUpperDelay = 30.0f;

        private float spawnDelay = 2.0f;

        private readonly float minSpawnDelay = 0.25f;

        private float currentTime;

        private int currentWave;

        private Hashtable paths;

        private bool canSpawnWave = true;

        private Coroutine infiniteCoroutine;

        private Coroutine waveCoroutine;

        private bool preemptWave;

        private int spawns;

        private int enemiesRemaining;

        private bool spawned;

        private bool lowLevel;

        private float maxDelay;

        private float endTime;

        private float currentDelay;

        public bool IsRunning { get; private set; }

        public void Initialize(ReferenceManager referenceManager)
        {
            reference = referenceManager;
            waveManager = reference.waveManager;
            Random.InitState((int) DateTime.Now.Ticks);
            intSpawnChance = (int) (spawnChance * 100);
            intPowerupSpawnChance = (int) (powerupSpawnChance * 100);

            enemySpawnChances = new Chances[enemySpawnChanceSetup.Length];
            typeIds = new int[enemySpawnChanceSetup.Length];
            int high = 0;

            for (int i = 0; i < enemySpawnChances.Length; ++i)
            {
                int low = high;
                high += (int) (enemySpawnChanceSetup[i].percentChance * 100);
                typeIds[i] = waveManager.EnemyIDs[(int) enemySpawnChanceSetup[i].enemyType];

                enemySpawnChances[i] = new Chances(low, high, enemySpawnChanceSetup[i].firstWave);
            }

            powerupSpawnChances = new Chances[powerupSpawnChanceSetup.Length];
            powerupPrefabs = new GameObject[powerupSpawnChanceSetup.Length];
            high = 0;

            for (int i = 0; i < powerupSpawnChanceSetup.Length; ++i)
            {
                int low = high;
                high += (int) (powerupSpawnChanceSetup[i].percentChance * 100);
                powerupPrefabs[i] =
                    waveManager.powerupPrefabs[(int) powerupSpawnChanceSetup[i].powerup];

                powerupSpawnChances[i] =
                    new Chances(low, high, powerupSpawnChanceSetup[i].firstWave);
            }

            paths = new Hashtable {{"child", false}};

            foreach (string zone in waveManager.PathNames)
            {
                for (int num = 0; num < 13; ++num)
                {
                    string path = zone + num;
                    paths.Add(path, false);
                }
            }
        }

        public void Run()
        {
            if (!IsRunning)
            {
                IsRunning = true;

                infiniteCoroutine = StartCoroutine(InfiniteSpawn());
            }
            else
            {
                Debug.Log(GetType().Name + " - Already running.");
            }
        }

        private IEnumerator InfiniteSpawn()
        {
            while (IsRunning)
            {
                if (canSpawnWave)
                {
                    waveCoroutine = StartCoroutine(Wave());
                    canSpawnWave = false;
                }

                do
                {
                    yield return new WaitForSeconds(0.1f);

                    currentTime += 0.1f;

                    if (preemptWave)
                    {
                        preemptWave = false;
                        currentTime = waveLength + Time.deltaTime;
                    }

                    if (enemiesRemaining >= maxEnemies && spawns < maxSpawns)
                    {
                        currentTime -= 0.1f;
                    }

                    if (currentTime >= waveLength)
                    {
                        if (waveCoroutine != null)
                        {
                            StopCoroutine(waveCoroutine);
                            canSpawnWave = true;
                        }

                        currentTime -= waveLength;
                    }
                }
                while (!canSpawnWave);
            }

            yield return null;

            Debug.LogWarning(GetType().Name + " - InfiniteSpawn() ended. IsRunning is false.");
            infiniteCoroutine = null;
        }

        private IEnumerator Wave()
        {
            maxDelay = minSpawns / waveLength;
            spawns = 0;
            spawnDelay = ReduceDelay(spawnDelay, ++currentWave);

            if (currentWave % 3 == 0)
            {
                if (maxEnemies < maxSpawns)
                {
                    maxEnemies++;
                }
            }

            Debug.Log("Beginning wave #" + currentWave);

            endTime = Time.time + waveLength;

            int roll = Random.Range(0, 100);

            if (roll >= 100 - intPowerupSpawnChance)
            {
                StartCoroutine(SpawnPowerup());
            }

            currentDelay = 0.0f;

            while (Time.time < endTime)
            {
                if (enemiesRemaining >= maxEnemies && spawns < maxSpawns)
                {
                    yield return null;
                    endTime += 0.1f + Time.deltaTime;
                    continue;
                }

                roll = Random.Range(0, 100);
                spawned = false;

                if (roll >= 100 - intSpawnChance)
                {
                    StartCoroutine(SpawnEnemy());

                    while (!spawned)
                    {
                        yield return new WaitForSeconds(0.01f);
                    }

                    spawned = false;

                    if (spawns >= maxSpawns)
                    {
                        maxSpawns = IncreaseSpawns(maxSpawns, currentWave);
                        yield return new WaitForSeconds(preemptTime);

                        PreemptWave();
                        //StopCoroutine(waveCoroutine);
                    }

                    if (currentDelay >= maxDelay)
                    {
                        StartCoroutine(SpawnEnemy());

                        while (!spawned)
                        {
                            yield return new WaitForSeconds(0.01f);
                        }

                        spawned = false;
                    }
                }
                else
                {
                    float delay = spawnDelay * Random.Range(0.6f, 0.85f);
                    yield return new WaitForSeconds(delay);

                    currentDelay += delay;
                }
            }

            yield return null;

            canSpawnWave = true;
            waveCoroutine = null;
        }

        private IEnumerator SpawnEnemy()
        {
            int roll = Random.Range(0, 100);

            CurrentChances[] spawnableEnemies = GetCurrentChances(true);

            bool locSpawned = false;

            for (int i = 0; i < spawnableEnemies.Length; ++i)
            {
                if (roll >= spawnableEnemies[i].LowerLimit &&
                    roll < spawnableEnemies[i].UpperLimit)
                {
                    while (!locSpawned)
                    {
                        string zone =
                            waveManager.PathNames
                                [Random.Range(0, waveManager.PathNames.Length)];

                        int num = Random.Range(0, 13);

                        string path = zone + num;

                        if (paths[path].Equals(false))
                        {
                            Vector3 position;

                            if (zone.Equals("CenterPath") ||
                                zone.Equals("LeftPath") ||
                                zone.Equals("RightPath"))
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
                                    Quaternion.LookRotation(
                                        new Vector3(0.0f, 2.0f, 0.0f) - position);
                            }

                            GameObject spawnedEnemy =
                                reference.objectPoolManager.Spawn(
                                    gameObject, (short)typeIds[i], position, rotation);

                            Enemy spawnedEnemyScript = spawnedEnemy.GetComponent<Enemy>();
                            spawnedEnemyScript.SetPath(path);

                            bool patternChosen = false;

                            do
                            {
                                roll = Random.Range(0, 100);
                                CurrentChances[] patternChances = GetCurrentChances(false);

                                for (int p = 0; p < patternChances.Length; ++p)
                                {
                                    if (roll >= patternChances[p].LowerLimit &&
                                        roll < patternChances[p].UpperLimit)
                                    {
                                        spawnedEnemyScript.SetAttackPattern((AttackPattern)p);
                                        patternChosen = true;
                                    }
                                }

                                if (patternChosen)
                                {
                                    continue;
                                }

                                yield return null;

                                endTime += Time.deltaTime;
                            }
                            while (!patternChosen);

                            //spawnedEnemyScript.SetAttackPattern(AttackPattern.One);

                            Hashtable moveHash = new Hashtable
                                                 {
                                                     {"easetype", iTween.EaseType.easeInOutSine},
                                                     {"time", 3.0f},
                                                     {
                                                         "looktarget",
                                                         reference.shipLookTarget.transform
                                                     },
                                                     {"onComplete", "OnPathingComplete"},
                                                     {"onCompleteTarget", spawnedEnemy},
                                                     {"path", iTweenPath.GetPath(path)}
                                                 };

                            iTween.MoveTo(spawnedEnemy, moveHash);
                            paths[path] = true;
                            locSpawned = true;
                            ++spawns;
                            ++enemiesRemaining;
                            currentDelay -= maxDelay;

                            if (spawns >= maxSpawns)
                            {
                                maxSpawns = IncreaseSpawns(maxSpawns, currentWave);
                                yield return new WaitForSeconds(preemptTime);

                                PreemptWave();
                            }
                        }
                        else
                        {
                            yield return null;

                            currentDelay += Time.deltaTime;
                        }
                    }

                    break;
                }
            }

            if (locSpawned)
            {
                yield return new WaitForSeconds(spawnDelay);

                currentDelay += spawnDelay;
            }
            else
            {
                Debug.LogError(GetType().Name + " - Did not spawn enemy.");
                spawned = true;
            }

            yield return null;

            spawned = true;
        }

        private IEnumerator SpawnPowerup()
        {
            float delay = Random.Range(powerupLowerDelay, powerupUpperDelay);

            yield return new WaitForSeconds(delay);

            bool locSpawned = false;

            while (!locSpawned)
            {
                int roll = Random.Range(0, 100);
                bool lowLevel = false;

                for (int i = 0; i < powerupSpawnChances.Length; ++i)
                {
                    if (roll >= powerupSpawnChances[i].LowerLimit &&
                        roll < powerupSpawnChances[i].UpperLimit)
                    {
                        if (currentWave < powerupSpawnChances[i].FirstWave)
                        {
                            lowLevel = true;
                            break;
                        }

                        int spawnPoint = Random.Range(0, waveManager.powerupSpawnPoints.Length);

                        Transform spawn = waveManager.powerupSpawnPoints[spawnPoint];

                        Instantiate(powerupPrefabs[i], spawn.position, Quaternion.identity);

                        if (!waveManager.spawnedPowerupOnce)
                        {
                            waveManager.spawnedPowerupOnce = true;
                            reference.tooltips.SpawnGrabPowerup(spawnPoint);
                        }

                        locSpawned = true;
                        break;
                    }
                }

                if (lowLevel && !locSpawned)
                {
                    continue;
                }

                yield return null;
            }
        }

        public void EnemyDead(string path)
        {
            if (--enemiesRemaining < 0)
            {
                enemiesRemaining = 0;
            }

            paths[path] = false;
        }

        private void PreemptWave()
        {
        }

        private float ReduceDelay(float delay, int wave)
        {
            if (Math.Abs(delay - minSpawnDelay) < 0.01f)
            {
                return minSpawnDelay;
            }

            float newDelay;

            if (wave % 5 == 0)
            {
                newDelay = delay * 0.65f;
            }
            else
            {
                newDelay = delay * 0.9f;
            }

            return Mathf.Clamp(newDelay, minSpawnDelay, 100.0f);
        }

        /// <summary>
        /// Gets the currently available chances for either enemies or attack patterns.
        /// </summary>
        /// <param name="isEnemy"> If the chances are for enemies </param>
        /// <returns> Array of the currently available chances </returns>
        private CurrentChances[] GetCurrentChances(bool isEnemy)
        {
            List<CurrentChances> chances = new List<CurrentChances>();
            int high = 0;

            Chances[] currentChances = isEnemy ? enemySpawnChances : attackPatternChances;

            foreach (Chances chance in currentChances)
            {
                if (chance.FirstWave <= currentWave)
                {
                    if (chance.UpperLimit > high)
                    {
                        high = chance.UpperLimit;
                    }

                    chances.Add(new CurrentChances(chance.LowerLimit, chance.UpperLimit));
                }
                else
                {
                    break;
                }
            }

            for (int i = 0; i < chances.Count; ++i)
            {
                int lower = Remap(chances[i].LowerLimit, 0, high, 0, 99);
                int upper = Remap(chances[i].UpperLimit, 0, high, 0, 99);
                chances[i] = new CurrentChances(lower, upper);
            }

            return chances.ToArray();
        }

        private static int Remap(int x, int a, int b, int c, int d)
        {
            float map = ((float) x - a) * (((float) d - c) / ((float) b - a)) + c;
            return Mathf.RoundToInt(map);
        }

        private static int IncreaseSpawns(int maxSpawns, int wave)
        {
            int spawns = Mathf.Clamp(maxSpawns + wave % 5, 25, 50);
            return spawns;
        }
    }
}