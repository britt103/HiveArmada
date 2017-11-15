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
using SubjectNerd.Utilities;
using UnityEngine;

namespace Hive.Armada.Game
{
    /// <summary>
    /// 
    /// </summary>
    public class WaveManager : MonoBehaviour
    {
        /// <summary>
        /// Array of all waves that will be run.
        /// </summary>
        [Reorderable("Wave", false)]
        public Wave[] waves;

        private int currentWave;

        public bool IsRunning { get; private set; }

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
            waves[wave].Run();
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
