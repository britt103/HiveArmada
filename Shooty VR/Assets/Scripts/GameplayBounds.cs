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
using UnityEngine.SceneManagement;
using Hive.Armada.Game;
using Hive.Armada.Player;

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

        ///// <summary>
        ///// Trigger Game Over upon collision with player head.
        ///// </summary>
        ///// <param name="other">Collider with which this is colliding.</param>
        //private void OnTriggerStay(Collider other)
        //{
        //    if(other.gameObject.name == "FollowHead" && reference.statistics.isAlive
        //        && !reference.spawner.waveCountGO.activeSelf)
        //    {
        //        Debug.Log("Out of Bounds");
        //        reference.statistics.isAlive = false;
        //        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        //        playerHealth.Hit(playerHealth.maxHealth);
        //    }
        //}

        /// <summary>
        /// Load Menu Room scene upon collision with player HMD.
        /// </summary>
        /// <param name="other">Collider with which this is colliding.</param>
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.name == "FollowHead")
            {
                Debug.Log("Out of Gameplay Bounds");
                //SceneManager.LoadScene(sceneName);
            }
        }
    }
}
