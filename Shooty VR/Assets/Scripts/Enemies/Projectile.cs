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

using Hive.Armada.Game;
using UnityEngine;
using Hive.Armada.Player;

namespace Hive.Armada.Enemies
{
    /// <summary>
    /// Basic projectile used by all shooting enemies.
    /// </summary>
    public class Projectile : Poolable
    {
        /// <summary>
        /// Reference manager that holds all needed references
        /// (e.g. spawner, game manager, etc.)
        /// </summary>
        private ReferenceManager reference;

        /// <summary>
        /// The amount of damage the projectile takes from the player's health 
        /// </summary>
        private int damage;

        /// <summary>
        /// Initializes the reference to the Reference Manager
        /// </summary>
        private void Awake()
        {
            reference = GameObject.Find("Reference Manager").GetComponent<ReferenceManager>();
        }

        /// <summary>
        /// Runs when the projectile collides with another object with a Collider.
        /// </summary>
        /// <param name="other"> The other collider </param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (Utility.isDebug)
                {
                    Debug.Log(GetType().Name + " - hit object named \"" + other.gameObject.name + "\"");
                }

                reference.playerShip.GetComponent<PlayerHealth>().Hit(damage);
                reference.objectPoolManager.Despawn(gameObject);
            }
            else if (other.CompareTag("Room"))
            {
                reference.objectPoolManager.Despawn(gameObject);
            }
        }

        /// <summary>
        /// Initializes the damage for the projectile.
        /// </summary>
        protected override void Reset()
        {
            damage = reference.enemyAttributes.projectileDamage;
        }
    }
}