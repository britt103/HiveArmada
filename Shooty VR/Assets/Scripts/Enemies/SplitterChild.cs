//=============================================================================
//
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// Basic enemy that is spawned when the Splitter dies. Simply looks at the
// player and shoots.
//
//=============================================================================

using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Hive.Armada.Enemies
{
    /// <summary>
    /// Splitter child enemy
    /// </summary>
    public class SplitterChild : Enemy
    {
        /// <summary>
        /// Type identifier for this enemy's projectiles in objectPoolManager
        /// </summary>
        private int projectileTypeIdentifier;

        /// <summary>
        /// The point where this enemy shoots from.
        /// </summary>
        public Transform shootPoint;

        /// <summary>
        /// How many time per second this enemy can shoot.
        /// </summary>
        private float fireRate;

        /// <summary>
        /// How fast the projectiles move.
        /// </summary>
        private float projectileSpeed;

        /// <summary>
        /// Degrees that the projectile can be randomly rotated.
        /// Randomly picks in the range of [-spread, spread] for all 3 axes.
        /// </summary>
        private float spread;

        /// <summary>
        /// The player's ship.
        /// </summary>
        private GameObject player;

        /// <summary>
        /// Whether this enemy can shoot or not. Toggles when firing every 1/fireRate seconds.
        /// </summary>
        private bool canShoot = true;

        /// <summary>
        /// Tries to look at the player and shoot at it when possible. Runs every frame.
        /// </summary>
        private void Update()
        {
            if (player != null)
            {
                transform.LookAt(player.transform);

                if (canShoot)
                {
                    StartCoroutine(Shoot());
                }
            }
            else
            {
                player = reference.playerShip;

                if (player == null)
                {
                    transform.LookAt(new Vector3(0.0f, 2.0f, 0.0f));
                }
            }
            if (shaking)
            {
                iTween.ShakePosition(gameObject, new Vector3(0.1f, 0.1f, 0.1f), 0.1f);

            }
            SelfDestructCountdown();
        }

        /// <summary>
        /// Shoots a projectile at the player.
        /// </summary>
        private IEnumerator Shoot()
        {
            canShoot = false;

            GameObject projectile =
                objectPoolManager.Spawn(projectileTypeIdentifier, shootPoint.position,
                                        shootPoint.rotation);

            projectile.GetComponent<Transform>().Rotate(Random.Range(-spread, spread),
                                                        Random.Range(-spread, spread),
                                                        Random.Range(-spread, spread));

            projectile.GetComponent<Rigidbody>().velocity =
                projectile.transform.forward * projectileSpeed;

            yield return new WaitForSeconds(fireRate);

            canShoot = true;
        }

        /// <summary>
        /// Resets attributes to this enemy's defaults from enemyAttributes.
        /// </summary>
        protected override void Reset()
        {
            projectileTypeIdentifier =
                enemyAttributes.EnemyProjectileTypeIdentifiers[TypeIdentifier];
            maxHealth = enemyAttributes.enemyHealthValues[TypeIdentifier];
            Health = maxHealth;
            fireRate = enemyAttributes.enemyFireRate[TypeIdentifier];
            projectileSpeed = enemyAttributes.projectileSpeed;
            spread = enemyAttributes.enemySpread[TypeIdentifier];
            pointValue = enemyAttributes.enemyScoreValues[TypeIdentifier];
            selfDestructTime = enemyAttributes.enemySelfDestructTimes[TypeIdentifier];
        }
        
    }
}