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

using System;
using UnityEngine;
using SubjectNerd.Utilities;

namespace Hive.Armada.Game
{
    /// <summary>
    /// The manager for waves and spawning.
    /// </summary>
    public class WaveManager : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        [Serializable]
        public struct SpawnZone
        {
            public GameObject lowerBound;

            public GameObject upperBound;
        }

        /// <summary>
        /// 
        /// </summary>
        [Reorderable("Spawn Zone", false)]
        public SpawnZone[] spawnZones;

        /// <summary>
        /// Array of all waves that will be run.
        /// </summary>
        [Reorderable("Wave", false)]
        public Wave[] waves;

        /// <summary>
        /// The index for the wave that is currently running.
        /// </summary>
        private int currentWave;

        /// <summary>
        /// If there are currently waves running.
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// If all waves have been run and completed.
        /// </summary>
        public bool IsComplete { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public void Run()
        {
            if (!IsRunning)
            {
                IsRunning = true;
                RunWave(currentWave);
            }
        }

        /// <summary>
        /// Begins running a wave from the waves array
        /// </summary>
        /// <param name="wave"> The index of the wave to run </param>
        private void RunWave(int wave)
        {
            waves[wave].Run(wave);
        }

        /// <summary>
        /// Informs the wave manager that a given wave has completed.
        /// </summary>
        /// <param name="wave"> The index of the wave that completed </param>
        public void WaveComplete(int wave)
        {
            if (!waves[currentWave].IsComplete || waves[currentWave].IsRunning)
            {
                Debug.LogError(GetType().Name + " - wave" + currentWave + " says it is complete, but it isn't!");
            }

            if (waves.Length > ++currentWave)
            {
                RunWave(currentWave);
            }
            else
            {
                IsRunning = false;
                IsComplete = true;
            }
        }
    }
}
