//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// DontDestroyOnLoad prevents gameObject from being destroyed when a new scene
// is loaded. Script should only be put on one object; all objects to be 
// preserved between scenes should be children to the gameObject this is
// attached to.
//
//=============================================================================

using UnityEngine;

namespace Hive.Armada
{
    /// <summary>
    /// Don't destroy gameObject on scene load.
    /// </summary>
    public class DontDestroyOnLoad : MonoBehaviour
    {
        /// <summary>
        /// State of whether gameObject is the original.
        /// </summary>
        private bool original = false;

        /// <summary>
        /// Prevent destruction on load and destroy any duplicates.
        /// </summary>
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            if (FindObjectsOfType(GetType()).Length > 1 && !original)
            {
                Destroy(gameObject);
            }

            original = true;
        }
    }
}