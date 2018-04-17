//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-440-1
// Group Project
//
// IridiumSpawner spawns an IridiumShootable at a transform randomly chosen
// from the provided array. Spawning is done based on a constant time interval.
//
//=============================================================================

using UnityEngine;

namespace Hive.Armada.Game
{
    /// <summary>
    /// Spawns IridiumShootables in waves.
    /// </summary>
    public class IridiumSpawner : MonoBehaviour
    {
        /// <summary>
        /// References to potential spawn locations.
        /// </summary>
        public Transform[] spawnPositions;

        /// <summary>
        /// Time between spawns.
        /// </summary>
        public float spawnTimeInterval;

        /// <summary>
        /// Reference to IridiumShootable prefab.
        /// </summary>
        public GameObject iridiumShootablePrefab;

        /// <summary>
        /// Tracks spawn timing.
        /// </summary>
        private float currTime;

        /// <summary>
        /// Set inital timer value.
        /// </summary>
        void Start()
        {
            Random.InitState((int)Time.time);
            currTime = spawnTimeInterval;
        }

        /// <summary>
        /// When timer ends, spawn, then reset timer.
        /// </summary>
        void Update()
        {
            currTime -= Time.deltaTime;
            if (currTime <= 0.0f)
            {
                Spawn();
                currTime = spawnTimeInterval;
            }
        }

        /// <summary>
        /// Instantiate an IridiumShootable in a randomly selected location.
        /// </summary>
        private void Spawn()
        {
            int spawnPosition = Random.Range(0, spawnPositions.Length);
            Instantiate(iridiumShootablePrefab, spawnPositions[spawnPosition]);
        }
    }
}
