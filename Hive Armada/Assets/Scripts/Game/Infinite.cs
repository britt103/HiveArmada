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
        private struct Chances
        {
            public readonly int enemyTypeId;

            public int LowerLimit { get; private set; }

            public int UpperLimit { get; private set; }

            public int FirstWave { get; private set; }

            public Chances(int enemyTypeId, int lowerLimit, int upperLimit, int firstWave)
                : this()
            {
                this.enemyTypeId = enemyTypeId;
                LowerLimit = lowerLimit;
                UpperLimit = upperLimit;
                FirstWave = firstWave;
            }
        }

        /// <summary>
        /// Reference manager that holds all needed references
        /// (e.g. spawner, game manager, etc.)
        /// </summary>
        private ReferenceManager reference;

        private WaveManager waveManager;

        [Reorderable("Enemy", false)]
        public SpawnChance[] spawnChances;

        private Chances[] chances;

        public float waveLength = 60.0f;

        [Range(0.0f, 100.0f)]
        public float spawnChance = 0.8f;

        private int intSpawnChance;

        [Range(0.0f, 100.0f)]
        public float powerupSpawnChance = 0.8f;

        private int intPowerupSpawnChance;

        private float powerupLowerDelay = 10.0f;

        private float powerupUpperDelay = 30.0f;

        private float spawnDelay = 2.0f;

        private readonly float minSpawnDelay = 0.25f;

        private float currentTime;

        private int currentWave;

        private Hashtable paths;

        private bool canSpawnWave = true;

        private Coroutine infiniteCoroutine;

        private Coroutine waveCoroutine;

        public bool IsRunning { get; private set; }

        public void Initialize(ReferenceManager referenceManager)
        {
            reference = referenceManager;
            waveManager = reference.waveManager;
            Random.InitState((int) DateTime.Now.Ticks);
            intSpawnChance = (int) (spawnChance * 100);
            intPowerupSpawnChance = (int) (powerupSpawnChance * 100);

            chances = new Chances[spawnChances.Length];
            int high = 0;

            for (int i = 0; i < chances.Length; ++i)
            {
                int low = high;
                high += (int) (spawnChances[i].percentChance * 100);
                int typeId = waveManager.EnemyIDs[(int) spawnChances[i].enemyType];

                chances[i] = new Chances(typeId, low, high, spawnChances[i].firstWave);
            }

            paths = new Hashtable();

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

                    if (Math.Abs(currentTime % 30.0f) < 0.000001f)
                    {
                        if (waveCoroutine != null)
                        {
                            StopCoroutine(waveCoroutine);
                            canSpawnWave = true;
                        }
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
            spawnDelay = ReduceDelay(spawnDelay, ++currentWave);

            Debug.Log("Beginning wave #" + currentWave);

            float endTime = Time.time + waveLength;

            int roll = Random.Range(0, 100);

            if (roll >= 100 - intPowerupSpawnChance)
            {
                StartCoroutine(SpawnPowerup());
            }

            while (Time.time < endTime)
            {
                roll = Random.Range(0, 100);

                if (roll >= 100 - intSpawnChance)
                {
                    bool spawned = false;
                    bool lowLevel = false;
                    roll = Random.Range(0, 100);

                    for (int i = 0; i < chances.Length; ++i)
                    {
                        if (roll >= chances[i].LowerLimit && roll < chances[i].UpperLimit)
                        {
                            if (currentWave < chances[i].FirstWave)
                            {
                                lowLevel = true;
                                break;
                            }

                            while (!spawned)
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
                                        position = GameObject.Find("FrontSpawn").transform.position;
                                    }
                                    else
                                    {
                                        position = GameObject.Find("BackSpawn").transform.position;
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
                                            chances[i].enemyTypeId, position, rotation);

                                    Enemy spawnedEnemyScript = spawnedEnemy.GetComponent<Enemy>();
                                    spawnedEnemyScript.SetPath(path);
                                    spawnedEnemyScript.SetAttackPattern(AttackPattern.One);

                                    Hashtable moveHash = new Hashtable();

                                    moveHash.Add("easetype", iTween.EaseType.easeInOutSine);
                                    moveHash.Add("time", 3.0f);
                                    moveHash.Add("looktarget",
                                                 reference.playerShip != null
                                                     ? reference.playerShip.transform
                                                     : reference.player.transform);
                                    moveHash.Add("onComplete", "OnPathingComplete");
                                    moveHash.Add("onCompleteTarget", spawnedEnemy);
                                    moveHash.Add("path", iTweenPath.GetPath(path));
                                    iTween.MoveTo(spawnedEnemy, moveHash);

                                    paths[path] = true;
                                    spawned = true;
                                }
                                else
                                {
                                    yield return null;
                                }
                            }

                            break;
                        }
                    }

                    if (lowLevel)
                    {
                        yield return new WaitForSeconds(spawnDelay * Random.Range(0.6f, 0.85f));

                        continue;
                    }

                    if (!spawned)
                    {
                        Debug.LogError(GetType().Name + " - Did not spawn enemy.");
                    }
                    else
                    {
                        yield return new WaitForSeconds(spawnDelay);
                    }
                }
                else
                {
                    yield return new WaitForSeconds(spawnDelay * Random.Range(0.6f, 0.85f));
                }
            }

            yield return null;

            canSpawnWave = true;
            waveCoroutine = null;
        }

        private IEnumerator SpawnPowerup()
        {
            float delay = Random.Range(powerupLowerDelay, powerupUpperDelay);

            yield return new WaitForSeconds(delay);

            GameObject powerup =
                waveManager.powerupPrefabs[Random.Range(0, waveManager.powerupPrefabs.Length)];

            Transform spawn =
                waveManager.powerupSpawnPoints[
                    Random.Range(0, waveManager.powerupSpawnPoints.Length)];

            Instantiate(powerup, spawn.position, Quaternion.identity);
        }

        public void EnemyDead(string path)
        {
            paths[path] = false;
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
    }
}