//=============================================================================
//
// Ryan Britton
// 1849351
// britt103@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// Enemy that teleports between 2 points while shooting at the player.
//
//=============================================================================

using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Hive.Armada.Enemies
{
    /// <summary>
    /// Teleporting turret enemy
    /// </summary>
    public class JumperTurret : Enemy
    {
        /// <summary>
        /// Type identifier for this enemy's projectiles in objectPoolManager
        /// </summary>
        private short projectileTypeIdentifier;

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
        /// Distance on the X axis this enemy will move.
        /// </summary>
        public float xMax;

        /// <summary>
        /// Distance on the Y axis this enemy will move.
        /// </summary>
        public float yMax;

        /// <summary>
        /// Distance on the Z axis this enemy will move.
        /// </summary>
        public float zMax;

        /// <summary>
        /// Initial position of this enemy.
        /// </summary>
        private Vector3 posA;

        /// <summary>
        /// Secondary position this enemy will teleport between.
        /// Based on xMax, yMax, and zMax.
        /// </summary>
        private Vector3 posB;

        /// <summary>
        /// Whether this enemy can teleport or not. Toggles when teleporting every 'timer' seconds.
        /// </summary>
        private bool canTeleport = true;

        /// <summary>
        /// Seconds to wait between teleporting.
        /// </summary>
        public float timer;

        /// <summary>
        /// The player's ship.
        /// </summary>
        private GameObject player;

        /// <summary>
        /// Whether this enemy can shoot or not. Toggles when firing every 1/fireRate seconds.
        /// </summary>
        private bool canShoot = true;

        /// <summary>
        /// Sets teleport positions. Runs when this enemy is spawned.
        /// </summary>
        private void Start()
        {
            SetPosition();           
            StartCoroutine(JumpFlash(timer));
        }

        /// <summary>
        /// Sets the 2 positions this enemy will teleport between.
        /// </summary>
        private void SetPosition()
        {
            posA = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            posB = new Vector3(transform.position.x + xMax, transform.position.y + yMax,
                transform.position.z + zMax);
        }

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
                objectPoolManager.Spawn(gameObject, projectileTypeIdentifier, shootPoint.position,
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
        /// Teleports this enemy between posA and posB every 'waittime' seconds.
        /// </summary>
        /// <param name="waitTime">
        /// How many seconds to wait between teleporting.
        /// </param>
        private IEnumerator JumpFlash(float waitTime)
        {
            while (true)
            {
                gameObject.GetComponent<Renderer>().material = flashColor;
                yield return new WaitForSeconds(1.0f);

                if (canTeleport)
                {
                    transform.position = posB;
                    canTeleport = false;
                }
                else
                {
                    transform.position = posA;
                    canTeleport = true;
                }
                yield return new WaitForSeconds(waitTime);
            }
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