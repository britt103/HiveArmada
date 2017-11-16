using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Name: Miguel Luis Gotao
//Student ID: 2264941
//Email: gotao100@mail.chapman.edu
//Course: CPSC340-01
//Game Development Project

namespace Hive.Armada
{
    /// <summary>
    /// Script in charge of wave spawning in the scene.
    /// </summary>
    public class OldWaveManager : MonoBehaviour
    {
        /// <summary>
        /// Values in charge of keeping track of:
        /// -What round it is
        /// -How many enemies will be spawned current round
        /// -How many enemies have currently spawned
        /// -How many enemies have already been killed
        /// </summary>
        public int countSpawn;
        public int currWave;
        public int currSpawn;
        public int currDead;

        /// <summary>
        /// Enemy pool for easy access to list.
        /// </summary>
        public GameObject enemyOne;
        public GameObject enemyTwo;
        public GameObject enemyThree;
        public GameObject enemyFour;

        /// <summary>
        /// Reference to the spawn zone
        /// </summary>
        public GameObject mSpawn;

        /// <summary>
        /// Tracks the current number of spawned enemies and dead enemies.
        /// When the number of spawned enemies meets the designated countSpawn, shuts off spawning.
        /// When the number of dead enemies meets the designated countSpawn, makes preparations for the next wave.
        /// </summary>
        void Update()
        {
            if (currSpawn == countSpawn) mSpawn.SetActive(false);
            if (currDead == countSpawn) StartCoroutine(RoundWait());
        }

        IEnumerator RoundWait()
        {
            var waveSettings = mSpawn.GetComponent<Spawn>();
            waveSettings.canSpawn = false;

            if (currWave == 1)
            {
                countSpawn = 20;
                waveSettings.SpawnCount = 5;
                waveSettings.spawnWait = 0.5f;
                waveSettings.SpawnList.Add(enemyTwo);
                waveSettings.waveWait = 3;
                currWave++;
            }

            currSpawn = 0;
            currDead = 0;
            yield return new WaitForSeconds(5);
            waveSettings.canSpawn = true;
            StartCoroutine(waveSettings.SpawnWaves());
        }
    }
}
