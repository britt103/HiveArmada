using System.Collections;
using System.Collections.Generic;
using SubjectNerd.Utilities;
using UnityEngine;

namespace Hive.Armada.Game
{
    /// <summary>
    /// 
    /// </summary>
    public class Wave : MonoBehaviour
    {
        /// <summary>
        /// 
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
        /// 
        /// </summary>
        private void Awake()
        {
            reference = GameObject.Find("Reference Manager").GetComponent<ReferenceManager>();

            if (reference == null)
            {
                Debug.LogError(GetType().Name + " - Could not find Reference Manager!");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wave"> The wave's number </param>
        public void Run(int wave)
        {
            if (subwaves.Length == 0)
            {
                Debug.LogError("Wave " + WaveNumber + " has no subwaves!");
            }
            else
            {
                IsRunning = true;
                RunSubwave(currentSubwave);
            }
        }

        /// <summary>
        /// Begins running a subwave from the subwaves array.
        /// </summary>
        /// <param name="subwave"> The index of the subwave to run </param>
        private void RunSubwave(int subwave)
        {
            this.subwaves[subwave].RunSubwave(WaveNumber, subwave);
        }

        /// <summary>
        /// Informs the wave that a given subwave has completed.
        /// </summary>
        /// <param name="subwave"> The index of the subwave that completed </param>
        public void SubwaveComplete(int subwave)
        {
            if (!this.subwaves[currentSubwave].IsComplete || this.subwaves[currentSubwave].IsRunning)
            {
                Debug.LogError("Wave " + WaveNumber + " - subwave " + currentSubwave + " says it is complete, but it isn't!");
            }

            if (this.subwaves.Length > ++currentSubwave)
            {
                RunSubwave(currentSubwave);
            }
            else
            {
                IsRunning = false;
                IsComplete = true;
                reference.waveManager.WaveComplete(WaveNumber);
            }
        }
    }
}
