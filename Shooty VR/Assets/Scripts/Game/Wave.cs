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
        private ReferenceManager reference;

        /// <summary>
        /// What wave is this?
        /// </summary>
        public int waveNumber;

        /// <summary>
        /// All subwaves that will run during this wave in the order they will run.
        /// </summary>
        [Tooltip("All subwaves that will run during this wave in the order they will run.")]
        [Reorderable("Subwave", false)]
        public Subwaves[] subwaves;

        private int currentSubwave;

        /// <summary>
        /// 
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsComplete { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        private void Start()
        {
            reference = GameObject.Find("Reference Manager").GetComponent<ReferenceManager>();

            if (reference == null)
            {
                Debug.LogError(GetType().Name + " - Could not find Reference Manager!");
            }
        }

        public void Run()
        {
            if (subwaves.Length == 0)
            {
                Debug.LogError("Wave " + waveNumber + " has no subwaves!");
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
            subwaves[subwave].RunSubwave();
        }

        /// <summary>
        /// Informs the wave that a given subwave has completed.
        /// </summary>
        /// <param name="subwave"> The index of the subwave that completed </param>
        public void SubwaveComplete(int subwave)
        {
            if (!subwaves[currentSubwave].IsComplete || subwaves[currentSubwave].IsRunning)
            {
                Debug.LogError("Wave " + waveNumber + " - subwave " + currentSubwave + " says it is complete, but it isn't!");
            }

            if (subwaves.Length > ++currentSubwave)
            {
                RunSubwave(currentSubwave);
            }
            else
            {
                IsRunning = false;
                IsComplete = true;
                reference.newWaveManager.WaveComplete(waveNumber);
            }
        }
    }
}
