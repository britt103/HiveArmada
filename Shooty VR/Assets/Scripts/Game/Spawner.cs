//=============================================================================
// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
// 
// [DESCRIPTION]
// 
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Hive.Armada.Game;
using Hive.Armada.Player;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Hive.Armada.Player;

namespace Hive.Armada.Game
{
    public class Spawner : MonoBehaviour
    {
        private ReferenceManager reference;
        public int multiplier;
        public Waves waves;
        [Tooltip("Powerup prefabs in order: Ally, Area Bomb, Clear, Damage Boost, Shield")]
        public GameObject[] powerups;
        public GameObject[] enemyBounds;
        public GameObject[] powerupBounds;
        //public GameObject[] enemyPrefabs;
        public int wave { get; private set; }
        public int kills { get; private set; }
        private int enemySpawnCount;
        private float enemySpawnTime;
        public float powerupSpawnTime;
        private int alive;
        private int enemyCap;
        //private bool 
        private bool canSpawnEnemy = true;
        private bool canSpawnPowerup = true;
        private Coroutine waveSpawn;

        public GameObject waveCountGO;
        public GameObject winScreenGO;

        public AudioSource music;

        public int startWave;

        private PlayerStats stats;
        private PlayerIdleTimer playerIdleTimer;

        private List<GameObject> objectPoolTestObjects;

        void Start()
        {
            //StartCoroutine(PoolSpawnTest());
        }

        private IEnumerator PoolSpawnTest()
        {
            //yield return new WaitForSeconds(2.0f);

            for (int i = 0; i < 50; ++i)
            {
                Vector3 pos = new Vector3(Random.Range(reference.spawner.enemyBounds[0].transform.position.x, reference.spawner.enemyBounds[1].transform.position.x),
                    Random.Range(reference.spawner.enemyBounds[0].transform.position.y, reference.spawner.enemyBounds[1].transform.position.y),
                    Random.Range(reference.spawner.enemyBounds[0].transform.position.z, reference.spawner.enemyBounds[1].transform.position.z));

                objectPoolTestObjects.Add(reference.objectPoolManager.Spawn(0, pos));
                yield return new WaitForSeconds(0.1f);
            }

            StartCoroutine(PoolDespawnTest());
        }

        private IEnumerator PoolDespawnTest()
        {
            yield return new WaitForSeconds(1.0f);

            for (int i = 0; i < 50; ++i)
            {
                reference.objectPoolManager.Despawn(objectPoolTestObjects.First());
                objectPoolTestObjects.RemoveAt(0);
                yield return new WaitForSeconds(0.1f);
            }

            StartCoroutine(PoolSpawnTest());
        }

        void Awake()
        {
            reference = GameObject.Find("Reference Manager").GetComponent<ReferenceManager>();
            objectPoolTestObjects = new List<GameObject>();
            wave = startWave - 2;

            waveCountGO.SetActive(false);
            winScreenGO.SetActive(false);

            stats = FindObjectOfType<PlayerStats>();
            playerIdleTimer = FindObjectOfType<PlayerIdleTimer>();
        }

        public void Run()
        {
            StartCoroutine(WaveTimer());
            GameObject.Find("Game Music");
            music.Play();
        }

        private IEnumerator WaveTimer()
        {
            ////remind player to pickup ship before wave starts
            //if (GameObject.Find("Player").GetComponentInChildren<Player.ShipController>() == null)
            //{
            //    shipReminderGO.SetActive(true);
            //    yield return new WaitWhile(() => (GameObject.Find("Player").GetComponentInChildren<Player.ShipController>() == null));
            //    shipReminderGO.SetActive(false);
            //}

            while (wave <= waves.waveSpawns.Length - 2)
            {
                if (waveSpawn == null)
                {
                    Debug.Log("Kills: " + kills + ", Enemy Spawn Count: " + enemySpawnCount);
                    List<GameObject> spawns = SetupWave();

                    int waveNum = wave + 1;

                    Debug.Log("BEGINNING WAVE " + waveNum);
                    waveCountGO.SetActive(true);
                    waveCountGO.GetComponent<Text>().text = "Wave: " + waveNum;
                    StartCoroutine(waves.playWave(waveNum));

                    yield return new WaitForSeconds(2);

                    waveCountGO.SetActive(false);
                    waveSpawn = StartCoroutine(SpawnWave(spawns));

                    stats.IsAlive();
					playerIdleTimer.SetIsTracking(true);
                }
                else
                {
                    yield return new WaitForSeconds(0.1f);
                }
            }
            Debug.Log("Kills: " + kills + ", Enemy Spawn Count: " + enemySpawnCount);
        }

        private List<GameObject> SetupWave()
        {
            ++wave;
            kills = 0;
            alive = 0;
            enemyCap = waves.GetEnemyCap(wave) * multiplier;
            enemySpawnTime = waves.GetSpawnTime(wave);

            List<GameObject> spawns = GetSpawns();
            //enemySpawnCount = spawns.Count;

            //foreach (GameObject enemy in spawns)
            //{
            //    if (enemy.GetType().Equals(waves.enemies[3]))
            //        enemySpawnTime += 4;
            //}

            //StartCoroutine(SpawnWave(spawns));
            return spawns;
        }

        private List<GameObject> GetSpawns()
        {
            enemySpawnCount = 0;
            List<GameObject> spawns = new List<GameObject>();
            int[] waveSpawns = waves.GetSpawns(wave);

            for (int i = 0; i < waveSpawns.Length; ++i)
            {
                waveSpawns[i] *= multiplier;
            }

            System.Random random = new System.Random();

            for (int i = 0; i < waveSpawns.Length; ++i)
            {
                int count = waveSpawns[i];
                for (int j = 0; j < count; ++j)
                {
                    switch (i)
                    {
                        case 0:
                            if (waves.enemies[0] != null)
                            {
                                ++enemySpawnCount;
                                spawns.Add(waves.enemies[0]);
                            }
                            else
                                if (Utility.isDebug)
                                Debug.Log("CRITICAL - Waves enemies[0] is null");
                            break;
                        case 1:
                            if (waves.enemies[1] != null)
                            {
                                ++enemySpawnCount;
                                spawns.Add(waves.enemies[1]);
                            }
                            else
                                if (Utility.isDebug)
                                Debug.Log("CRITICAL - Waves enemies[1] is null");
                            break;
                        case 2:
                            if (waves.enemies[2] != null)
                            {
                                ++enemySpawnCount;
                                spawns.Add(waves.enemies[2]);
                            }
                            else
                                if (Utility.isDebug)
                                Debug.Log("CRITICAL - Waves enemies[2] is null");
                            break;
                        case 3:
                            //increase for splitter
                            if (waves.enemies[3] != null)
                            {
                                enemySpawnCount += 5;
                                spawns.Add(waves.enemies[3]);
                            }
                            else
                                if (Utility.isDebug)
                                Debug.Log("CRITICAL - Waves enemies[3] is null");
                            break;
                        default:
                            Debug.Log("ERROR - ENEMY DEFINITION DOES NOT EXIST");
                            break;
                    }
                }
            }

            return spawns.OrderBy(item => random.Next()).ToList();
        }

        private IEnumerator SpawnWave(List<GameObject> spawns)
        {
            Debug.Log("Wave: " + wave + " - spawns: " + enemySpawnCount);

            while (kills < enemySpawnCount)
            {
                if (canSpawnEnemy)
                {
                    if (spawns.Count > 0)
                    {
                        GameObject prefab = spawns.First();
                        spawns.RemoveAt(0);
                        StartCoroutine(SpawnEnemy(prefab));
                    }
                    else
                        yield return new WaitForSeconds(0.1f);
                }
                else
                    yield return new WaitForSeconds(0.1f);

                if (canSpawnPowerup)
                {
                    StartCoroutine(SpawnPowerup());
                }
            }

            stats.WaveComplete();
            waveSpawn = null;
            playerIdleTimer.SetIsTracking(false);

            if (wave == waves.waveSpawns.Length - 1)
            {
                //StartCoroutine(Win());
                reference.sceneTransitionManager.TransitionOut("Wave Room");
            }
        }

        public void EnemyHit()
        {
            canSpawnEnemy = true;
        }

        private IEnumerator SpawnEnemy(GameObject prefab)
        {
            canSpawnEnemy = false;
            yield return new WaitForSeconds(enemySpawnTime);

            Vector3 lower = enemyBounds[0].transform.position;
            Vector3 upper = enemyBounds[1].transform.position;
            Vector3 position = new Vector3(
                Random.Range(lower.x, upper.x),
                Random.Range(lower.y, upper.y),
                Random.Range(lower.z, upper.z));

            if (prefab.name.Contains("Splitter"))
            {
                if (position.y < 1)
                    position.y += Random.Range(1.0f, 2.0f);
            }

            Instantiate(prefab, position, Quaternion.identity);

            ++alive;

            if (alive < enemyCap)
                canSpawnEnemy = true;
        }

        public void AddKill()
        {
            ++kills;
            --alive;

            if (alive < enemyCap)
                canSpawnEnemy = true;

            if (alive < 0)
                alive = 0;
        }

        private IEnumerator SpawnPowerup()
        {
            canSpawnPowerup = false;
            yield return new WaitForSeconds(powerupSpawnTime);

            Vector3 lower = powerupBounds[0].transform.position;
            Vector3 upper = powerupBounds[1].transform.position;

            Vector3 position = new Vector3(
                Random.Range(lower.x, upper.x),
                Random.Range(lower.y, upper.y),
                Random.Range(lower.z, upper.z));

            //GameObject prefab = powerups[Random.Range(0, powerups.Length)];
            float chance = Random.Range(0.0f, 1.0f);

            float[] chances = waves.GetPowerupChances(wave);

            for (int i = 0; i < chances.Length; ++i)
            {
                if (chance <= chances[i])
                {
                    Instantiate(powerups[i], position, Quaternion.Euler(90.0f, 90.0f, -90.0f));
                    break;
                }
            }

            canSpawnPowerup = true;
        }
    }
}
