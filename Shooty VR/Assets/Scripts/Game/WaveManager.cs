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
using System.Collections;
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
        /// Reference manager that holds all needed references
        /// (e.g. spawner, game manager, etc.)
        /// </summary>
        public ReferenceManager reference;

        /// <summary>
        /// Array of all available spawn zones in the scene.
        /// </summary>
        [Header("Bounds")]
        [Reorderable("Spawn Zone", false)]
        public SpawnZoneBounds[] spawnZonesBounds;

        /// <summary>
        /// The lower and upper bounds of the powerup spawn zone.
        /// </summary>
        public SpawnZoneBounds powerupSpawnZone;

        /// <summary>
        /// Which wave to start at?
        /// </summary>
        [Header("Waves")]
        public int startingWave;

        /// <summary>
        /// Index of the subwave in startingWave to go first.
        /// </summary>
        public int startingSubwave;

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
        /// The source to play the wave count from.
        /// </summary>
        [Header("Audio")]
        public AudioSource waveCountSource;

        /// <summary>
        /// The clips for "wave" and wave numbers.
        /// </summary>
        [Tooltip("Sounds for the wave counts. 0 is \"Wave\", 1-15 is wave number.")]
        [Reorderable("Sound")]
        public AudioClip[] waveCountSounds;

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
                --startingWave;

                if (startingWave <= 0)
                {
                    currentWave = 0;
                }
                else if (startingWave >= waves.Length)
                {
                    currentWave = 0;
                }
                else
                {
                    currentWave = startingWave;
                }

                --startingSubwave;

                if (startingSubwave <= 0)
                {
                    startingSubwave = 0;
                }
                else if (startingSubwave >= waves[currentWave].subwaves.Length)
                {
                    startingSubwave = 0;
                }

                IsRunning = true;
                RunWave(currentWave, startingSubwave);
            }
        }

        /// <summary>
        /// Begins running a wave from the waves array
        /// </summary>
        /// <param name="wave"> The index of the wave to run </param>
        private void RunWave(int wave)
        {
            StartCoroutine(WaveNumberDisplay(wave, 0));
        }

        /// <summary>
        /// Begins running a wave from the waves array at the given subwave
        /// </summary>
        /// <param name="wave"> The index of the wave to run </param>
        /// <param name="subwave"> The index of the subwave to start on </param>
        private void RunWave(int wave, int subwave)
        {
            StartCoroutine(WaveNumberDisplay(wave, subwave));
        }

        /// <summary>
        /// Shows the wave number before starting the wave.
        /// </summary>
        /// <param name="wave"> The index of the wave being run </param>
        /// <param name="subwave"> The index of the starting subwave </param>
        private IEnumerator WaveNumberDisplay(int wave, int subwave)
        {
            StartCoroutine(PlayWaveCount(wave + 1));
            reference.menuWaveNumberDisplay.gameObject.SetActive(true);
            reference.menuWaveNumberDisplay.text = "Wave: " + (wave+1);

            yield return new WaitForSeconds(2.0f);

            reference.menuWaveNumberDisplay.gameObject.SetActive(false);

            waves[wave].Run(wave, subwave);
        }

        /// <summary>
        /// Informs the wave manager that a given wave has completed.
        /// </summary>
        /// <param name="wave"> The index of the wave that completed </param>
        public void WaveComplete(int wave)
        {
            if (!waves[currentWave].IsComplete || waves[currentWave].IsRunning)
            {
                Debug.LogError(GetType().Name + " - wave" + currentWave +
                               " says it is complete, but it isn't!");
            }

            if (waves.Length > ++currentWave)
            {
                RunWave(currentWave);
            }
            else
            {
                IsRunning = false;
                IsComplete = true;

                reference.menuWin.SetActive(true);
            }
        }

        /// <summary>
        /// Plays "wave" and then the wave number.
        /// </summary>
        /// <param name="wave"> The wave number to play, not index of the wave </param>
        private IEnumerator PlayWaveCount(int wave)
        {
            waveCountSource.PlayOneShot(waveCountSounds[0]);

            yield return new WaitForSeconds(0.9f);

            waveCountSource.PlayOneShot(waveCountSounds[wave]);
        }
    }
}