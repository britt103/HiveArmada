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

namespace Hive.Armada.Game
{
    public class Spawner : MonoBehaviour
    {
        public Waves waves;
        public GameObject[] bounds;
        //public GameObject[] enemyPrefabs;
        public int wave { get; private set; }
        public int kills { get; private set; }
        private int spawnCount;
        private float spawnTime;
        private int alive;
        private int enemyCap;
        private bool canSpawn = true;
        private Coroutine waveSpawn;

        void Start()
        {
            wave = -1;
        }

        public void Run()
        {
            StartCoroutine(WaveTimer());
            // Spawn wave 1

            //SetupWave();

            //while (kills < spawnCount)
            //{
            //    // wait for wave 1 to finish
            //}

            //// Spawn wave 2
            //SetupWave();

            //while (kills < spawnCount)
            //{
            //    // wait for wave 2 to finish
            //}

            //// Spawn wave 3
            //SetupWave();

            //while (kills < spawnCount)
            //{
            //    // wait for wave 3 to finish
            //}

            //// Spawn wave 4
            //SetupWave();

            //while (kills < spawnCount)
            //{
            //    // wait for wave 4 to finish
            //}

            //// Spawn wave 5
            //SetupWave();
        }

        private IEnumerator WaveTimer()
        {
            while (wave <= 4)
            {
                if (waveSpawn == null)
                {
                    List<GameObject> spawns = SetupWave();

                    Debug.Log("BEGINNING WAVE " + wave);

                    waveSpawn = StartCoroutine(SpawnWave(spawns));
                }
                else
                {
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }

        private List<GameObject> SetupWave()
        {
            ++wave;
            kills = 0;
            alive = 0;
            enemyCap = waves.GetEnemyCap(wave);
            spawnTime = waves.GetSpawnTime(wave);

            List<GameObject> spawns = GetSpawns();
            spawnCount = spawns.Count;

            //StartCoroutine(SpawnWave(spawns));
            return spawns;
        }

        private List<GameObject> GetSpawns()
        {
            List<GameObject> spawns = new List<GameObject>();
            int[] waveSpawns = waves.GetSpawns(wave);
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
                                spawns.Add(waves.enemies[0]);
                            else
                                if (Utility.isDebug)
                                Debug.Log("CRITICAL - Waves enemies[0] is null");
                            break;
                        case 1:
                            if (waves.enemies[1] != null)
                                spawns.Add(waves.enemies[1]);
                            else
                                if (Utility.isDebug)
                                Debug.Log("CRITICAL - Waves enemies[1] is null");
                            break;
                        case 2:
                            if (waves.enemies[2] != null)
                                spawns.Add(waves.enemies[2]);
                            else
                                if (Utility.isDebug)
                                Debug.Log("CRITICAL - Waves enemies[2] is null");
                            break;
                        case 3:
                            if (waves.enemies[3] != null)
                                spawns.Add(waves.enemies[3]);
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
            while (kills < spawnCount)
            {
                if (canSpawn)
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
            }

            waveSpawn = null;
        }

        public void EnemyHit()
        {
            canSpawn = true;
        }

        private IEnumerator SpawnEnemy(GameObject prefab)
        {
            canSpawn = false;
            yield return new WaitForSeconds(3.0f);

            // SPAWN THAT SHIT HERE
            Vector3 lower = bounds[0].transform.position;
            Vector3 upper = bounds[1].transform.position;
            Vector3 position = new Vector3(
                Random.Range(lower.x, upper.x),
                Random.Range(lower.y, upper.y),
                Random.Range(lower.z, upper.z));
            Instantiate(prefab, position, Quaternion.identity);

            ++alive;

            if (alive < enemyCap)
                canSpawn = true;
        }

        public void AddKill()
        {
            ++kills;
            --alive;

            if (alive < enemyCap)
                canSpawn = true;

            if (alive < 0)
                alive = 0;
        }
    }
}
