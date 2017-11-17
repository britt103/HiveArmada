using System;
using System.Collections;
using System.Collections.Generic;
using SubjectNerd.Utilities;
using UnityEngine;

namespace Hive.Armada.Game
{
    /// <summary>
    /// All spawn zones in the scene.
    /// </summary>
    public enum SpawnZone
    {
        Introduction,
        Center,
        FrontLeft,
        FrontRight,
        BackLeft,
        BackRight
    }

    /// <summary>
    /// 
    /// </summary>
    public class Subwave : MonoBehaviour
    {
        /// <summary>
        /// 
        /// </summary>
        [Serializable]
        public struct SpawnGroup
        {
            /// <summary>
            /// The zone that this group will spawn in.
            /// </summary>
            [Tooltip("Which spawn zone will this group spawn in?")]
            public SpawnZone spawnZone;

            /// <summary>
            /// Delay before this group is spawned.
            /// </summary>
            [Tooltip("Time delay between this spawn group and the previous.")]
            public float spawnDelay;

            /// <summary>
            /// Array of enemy types that will be spawned.
            /// </summary>
            [Tooltip("The enemy types will be spawned.")]
            public GameObject[] enemyTypes;

            /// <summary>
            /// Number of each enemy type to spawn.
            /// </summary>
            [Tooltip("How many of each enemy type to spawn.")]
            public int[] enemyCounts;
        }

        /// <summary>
        /// Index position of this subwave in the current wave.
        /// </summary>
        public int SubwaveNumber { get; private set; }

        /// <summary>
        /// Array of all spawn groups for this subwave.
        /// </summary>
        [Tooltip("Array of all spawn groups for this subwave, in order.")]
        [Reorderable("Spawn Group", false)]
        public SpawnGroup[] spawnsGroup;

        /// <summary>
        /// Index of the current spawn group that needs to be spawned.
        /// </summary>
        private int currentSpawnGroup;

        /// <summary>
        /// If this subwave is currently running and still has spawn groups
        /// to spawn or enemies or there are still enemies alive.
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// If this subwave has run and all of its spawn groups have been killed.
        /// </summary>
        public bool IsComplete { get; private set; }

        /// <summary>
        /// Runs this subwave and tells it to spawn all of its spawn groups.
        /// </summary>
        /// <param name="wave"> Current wave number </param>
        /// <param name="subwave"> This subwave's index in the current wave </param>
        public void RunSubwave(int wave, int subwave)
        {

        }
    }
}
