//=============================================================================
//
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// A basic projectile. It will destroy itself after a set amount of time,
// after colliding with the room, or the player. It damages the player
// by a set amount.
//
//=============================================================================

using UnityEngine;
using Hive.Armada.Game;
using Hive.Armada.Player;

namespace Hive.Armada.Enemies
{
    /// <summary>
    /// Basic projectile used by all shooting enemies.
    /// </summary>
    public class ProjectileInPattern : MonoBehaviour
    {
        private ReferenceManager reference;

        private ProjectilePattern pattern;

        private int projectileDamage;

        public bool HasHit { get; private set; }

        private void Awake()
        {
            reference = FindObjectOfType<ReferenceManager>();
            pattern = gameObject.GetComponentInParent<ProjectilePattern>();
        }

        public void SetDamage(int damage)
        {
            projectileDamage = damage;
        }

        public void Reset()
        {
            HasHit = false;
        }

        public void Hit()
        {
            HasHit = true;
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Runs when the projectile collides with another object with a Collider.
        /// </summary>
        /// <param name="other"> The other collider </param>
        private void OnTriggerEnter(Collider other)
        {
            if (HasHit)
            {
                return;
            }

            if (other.CompareTag("Player"))
            {
                HasHit = true;

                if (Utility.isDebug)
                {
                    Debug.Log(GetType().Name + " - hit object named \"" + other.gameObject.name +
                              "\"");
                }

                if (reference.playerShip != null)
                {
                    reference.playerShip.GetComponent<PlayerHealth>().Hit(projectileDamage);
                }

                pattern.ProjectileHit();
                gameObject.SetActive(false);
            }
            else if (other.CompareTag("Room") || other.CompareTag("ProjectileBounds"))
            {
                pattern.ProjectileHit();
                gameObject.SetActive(false);
            }
        }
    }
}