//=============================================================================
//
// Ryan Britton
// 1849351
// britt103@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// Enemy that shoots at the player and that spawns 4 more enemies
// when destroyed.
//
//=============================================================================

using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Hive.Armada.Enemies
{
    /// <summary>
    /// Splitter turret enemy
    /// </summary>
    public class SplitterTurret : Enemy
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
        /// The enemy to spawn when this enemy dies
        /// </summary>
        public GameObject childTurret; 
        
        /// <summary>
        /// Distance each child enemy will move when this enemy is destroyed
        /// </summary>
        public float splitDir;

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
        /// Spawns 4 'childTurret' enemies when this enemy is destroyed
        /// </summary>
        protected override void Kill()
        {
            reference.spawner.AddKill();
            if (childTurret != null)
            {
                int typeIdentifier = objectPoolManager.GetTypeIdentifier(childTurret);

                //Instantiate("Explosion.name", transform.position, transform.rotation); Placeholder for destroy effect
                GameObject child1 = objectPoolManager.Spawn(typeIdentifier, transform.position, transform.rotation);
                GameObject child2 = objectPoolManager.Spawn(typeIdentifier, transform.position, transform.rotation);
                GameObject child3 = objectPoolManager.Spawn(typeIdentifier, transform.position, transform.rotation);
                GameObject child4 = objectPoolManager.Spawn(typeIdentifier, transform.position, transform.rotation);
                
                iTween.MoveTo(child1, iTween.Hash("x", transform.localPosition.x + (splitDir), "y", transform.localPosition.y + (splitDir), "z", transform.localPosition.z, "islocal", true, "time", 3.0f));
                iTween.MoveTo(child2, iTween.Hash("x", transform.localPosition.x + (splitDir), "y", transform.localPosition.y - (splitDir), "z", transform.localPosition.z, "islocal", true, "time", 3.0f));
                iTween.MoveTo(child3, iTween.Hash("x", transform.localPosition.x - (splitDir), "y", transform.localPosition.y + (splitDir), "z", transform.localPosition.z, "islocal", true, "time", 3.0f));
                iTween.MoveTo(child4, iTween.Hash("x", transform.localPosition.x - (splitDir), "y", transform.localPosition.y - (splitDir), "z", transform.localPosition.z, "islocal", true, "time", 3.0f));
            }

            Destroy(gameObject);

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
