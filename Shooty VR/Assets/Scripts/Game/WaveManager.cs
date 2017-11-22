//=============================================================================
//
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// This is the wave manager for the main game mode.
//
//=============================================================================

using System;
using UnityEngine;
using SubjectNerd.Utilities;

namespace Hive.Armada.Game
{
    /// <summary>
    /// All spawn zones in the scene.
    /// </summary>
    public enum SpawnZone
    {
        /// <summary>
        /// The introduction spawn point that is right in front of the player's view.
        /// </summary>
        Introduction = 0,

        /// <summary>
        /// The main spawn region in front of the player.
        /// </summary>
        Center = 1,

        /// <summary>
        /// The spawn region that is in the front left.
        /// </summary>
        FrontLeft = 2,

        /// <summary>
        /// The spawn region that is in the front right.
        /// </summary>
        FrontRight = 3,

        /// <summary>
        /// The spawn region that is up in the back left.
        /// </summary>
        BackLeft = 4,

        /// <summary>
        /// The spawn region that is up in the back right.
        /// </summary>
        BackRight = 5
    }

    /// <summary>
    /// Structure with 2 game objects that define the lower and upper bounds of a spawn zone.
    /// </summary>
    [Serializable]
    public struct SpawnZoneBounds
    {
        /// <summary>
        /// Game object representing the lower bound of the spawn zone.
        /// </summary>
        public GameObject lowerBound;

        /// <summary>
        /// Game object representing the upper bound of the spawn zone.
        /// </summary>
        public GameObject upperBound;
    }

    /// <summary>
    /// The manager for waves and spawning.
    /// </summary>
    public class WaveManager : MonoBehaviour
    {
        /// <summary>
        /// Array of all available spawn zones in the scene.
        /// </summary>
        [Reorderable("Spawn Zone", false)]
        public SpawnZoneBounds[] spawnZonesBounds;

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
        /// Runs the wave spawning for the game.
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