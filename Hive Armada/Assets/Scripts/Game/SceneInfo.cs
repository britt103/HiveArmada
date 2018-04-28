//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// SceneInfo tracks scene information. It is meant to not be destroyed on load.
//
//=============================================================================

using UnityEngine;

namespace Hive.Armada.Game
{
    /// <summary>
    /// Tracks scene info.
    /// </summary>
    public class SceneInfo : MonoBehaviour
    {
        /// <summary>
        /// Number of scenes that have been loaded.
        /// </summary>
        private int numScenesLoaded = 0;

        /// <summary>
        /// State of whether the current scene is the first.
        /// </summary>
        public bool firstScene = true;

        /// <summary>
        /// State of whether a run has just been completed.
        /// </summary>
        public bool runFinished = false;

        public bool demoFinished = false;

        /// <summary>
        /// Increment numScenesLoaded.
        /// </summary>
        private void Awake()
        {
            IncrementScenesLoaded();
        }

        /// <summary>
        /// Increment scenesLoaded and adjust firstScene value.
        /// </summary>
        public void IncrementScenesLoaded()
        {
            numScenesLoaded++;
            if (numScenesLoaded > 1)
            {
                firstScene = false;
            }
        }
    }
}