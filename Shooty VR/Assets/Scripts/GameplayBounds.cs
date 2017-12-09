//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// DeathBounds loads the Menu Room scene when the player has moved outside of
// the player area.
//
//=============================================================================

using UnityEngine;
using Hive.Armada.Game;

namespace Hive.Armada
{
    /// <summary>
    /// Penalize player upon collision.
    /// </summary>
    public class GameplayBounds : MonoBehaviour
    {
        /// <summary>
        /// Name of scene to load.
        /// </summary>
        public string sceneName = "Menu Room";

        /// <summary>
        /// ReferenceManager reference.
        /// </summary>
        private ReferenceManager reference;

        // Find ReferenceManager.
        void Start()
        {
            reference = FindObjectOfType<ReferenceManager>();
        }

        /// <summary>
        /// Load Menu Room scene upon collision with player HMD.
        /// </summary>
        /// <param name="other">Collider with which this is colliding.</param>
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.name == "HeadCollider")
            {
                Debug.Log("Out of Gameplay Bounds");
                reference.sceneTransitionManager.TransitionOut(sceneName);
            }
        }
    }
}
