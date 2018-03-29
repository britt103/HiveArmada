//=============================================================================
// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-440-01
// Group Project
// 
// This class tells projectiles to fade their opacity when near the player.
// When a projectile enters the sphere collider on the player's head, it is
// told to fade out its opacity. Then when it exits the collider it is told to
// fade in its opacity. The goal was to increase player comfort because a large
// number of projectiles in their face will completely block their vision.
// 
//=============================================================================

using UnityEngine;
using Hive.Armada.Enemies;

namespace Hive.Armada.Player
{
    /// <summary>
    /// Projectile proximity checker for the player's head.
    /// </summary>
    public class ProjectileProximity : MonoBehaviour
    {
        /// <summary>
        /// Fades out the transparency of projectiles that get too close to the camera.
        /// </summary>
        /// <param name="other"> The entering object's collider </param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Projectile"))
            {
                Projectile projectile = other.GetComponent<Projectile>();

                if (projectile != null)
                {
                    projectile.FadeOpacity(true);
                }
            }
        }

        /// <summary>
        /// Fades in the transparency of projectiles that get too close to the camera.
        /// </summary>
        /// <param name="other"> The exiting object's collider </param>
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Projectile"))
            {
                Projectile projectile = other.GetComponent<Projectile>();

                if (projectile != null)
                {
                    projectile.FadeOpacity(false);
                }
            }
        }
    }
}