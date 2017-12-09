//=============================================================================
//
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// This file contains the Wave class which has an array of subwaves that it
// will spawn. Subwaves can be reused in the both the same and different waves.
// This allows us to have more control over spawning and also simplifies the
// wave design process by letting us reuse subwaves.
// 
// The logic inside of Wave is very simple. It keeps track of what subwaves it
// has and tells the next one to start when the previous tells it that it has
// completed. Once the wave is done it tells WaveManager that it has completed.
//
//=============================================================================

using SubjectNerd.Utilities;
using UnityEngine;

namespace Hive.Armada.Game
{
    /// <summary>
    /// Contains all logic for a Wave.
    /// </summary>
    [DisallowMultipleComponent]
    public class Wave : MonoBehaviour
    {
        /// <summary>
        /// Reference manager that holds all needed references
        /// (e.g. spawner, game manager, etc.)
        /// </summary>
        private ReferenceManager reference;

        /// <summary>
        /// Index of this wave in the game.
        /// </summary>
        public int WaveNumber { get; private set; }

        /// <summary>
        /// All subwaves that will run during this wave in the order they will run.
        /// </summary>
        [Tooltip("All subwaves that will run during this wave in the order they will run.")]
        [Reorderable("Subwave", false)]
        public Subwave[] subwaves;

        /// <summary>
        /// Index of the subwave that is currently running.
        /// </summary>
        private int currentSubwave;

        /// <summary>
        /// If this wave, and any subwaves, are currently running.
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// If this wave has run and completed all of its subwaves.
        /// </summary>
        public bool IsComplete { get; private set; }

        /// <summary>
        /// Initializes the Wave and begins spawning its subwaves.
        /// </summary>
        /// <param name="wave"> The wave's number </param>
        /// <param name="subwave"> Index of the subwave to start with </param>
        public void Run(int wave, int subwave)
        {
            reference = GameObject.Find("Reference Manager").GetComponent<ReferenceManager>();

            if (reference == null)
            {
                Debug.LogError(GetType().Name + " - Could not find Reference Manager!");
            }

            if (subwaves.Length == 0)
            {
                Debug.LogError("Wave " + WaveNumber + " has no subwaves!");
            }
            else
            {
                WaveNumber = wave;
                Debug.Log("Beginning Wave #" + WaveNumber);

                currentSubwave = subwave;
                IsRunning = true;
                reference.statistics.IsAlive();
                RunSubwave(currentSubwave);
            }
        }

        /// <summary>
        /// Begins running a subwave from the subwaves array.
        /// </summary>
        /// <param name="subwave"> The index of the subwave to run </param>
        private void RunSubwave(int subwave)
        {
            Debug.Log("Beginning Wave #" + WaveNumber + "-" + subwave);
            subwaves[subwave].Run(WaveNumber, subwave);
        }

        /// <summary>
        /// Informs the wave that a given subwave has completed.
        /// </summary>
        /// <param name="subwave"> The index of the subwave that completed </param>
        public void SubwaveComplete(int subwave)
        {
            if (!subwaves[subwave].IsComplete || subwaves[subwave].IsRunning)
            {
                Debug.LogError("Wave " + WaveNumber + " - subwave " + subwave +
                               " says it is complete, but it isn't!");
            }

            if (subwaves.Length > ++currentSubwave)
            {
                RunSubwave(currentSubwave);
            }
            else
            {
                IsRunning = false;
                IsComplete = true;
                reference.waveManager.WaveComplete(WaveNumber);
                reference.statistics.IsNotAlive();
            }
        }
    }
}