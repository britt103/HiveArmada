//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// DeathBounds penalizes the player for colliding with the Death Bounds
// GameObject. 
//
//=============================================================================

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hive.Armada
{
    /// <summary>
    /// Penalizes player for leaving play area.
    /// </summary>
    public class DeathBounds : MonoBehaviour
    {
        /// <summary>
        /// Penalize player when FollowHead collides with bounds.
        /// </summary>
        /// <param name="other">Collider of object with which this collided.</param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name == "FollowHead")
            {
                Debug.Log("Out of Bounds.");
                //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                
            }
        }
    }
}
