//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// DeathBounds triggers the Game Over condition when the player has moved
// beyond the play area.
//
//=============================================================================

using UnityEngine;
using Hive.Armada.Game;
using Hive.Armada.Player;

namespace Hive.Armada
{
    /// <summary>
    /// Penalize player upon collision.
    /// </summary>
    public class DeathBounds : MonoBehaviour
    {
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
        /// Trigger Game Over upon collision with player head.
        /// </summary>
        /// <param name="other">Collider with which this is colliding.</param>
        private void OnTriggerStay(Collider other)
        {
            if(other.gameObject.name == "FollowHead" && reference.statistics.isAlive
                && !reference.spawner.waveCountGO.activeSelf)
            {
                Debug.Log("Out of Bounds");
                reference.statistics.isAlive = false;
                PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
                playerHealth.Hit(playerHealth.maxHealth);
            }
        }
    }
}
