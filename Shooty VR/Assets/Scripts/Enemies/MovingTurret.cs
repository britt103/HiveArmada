//=============================================================================
//
// Ryan Britton
// 1849351
// britt103@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// Enemy that shoots at the player while moving between two points
//
//=============================================================================

using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Hive.Armada.Enemies
{
    /// <summary>
    /// Moving turret Enemy
    /// </summary>
    public class MovingTurret : Enemy
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
        /// How fast this enemy can move.
        /// </summary>
        public float movingSpeed;

        /// <summary>
        /// Distance on the X axis this enemy will move.
        /// </summary>
        public float xMax;

        /// <summary>
        /// Distance on the Y axis this enemy will move.
        /// </summary>
        public float yMax;

        /// <summary>
        /// Initial position of this enemy.
        /// </summary>
        private Vector3 posA;

        /// <summary>
        /// Secondary position this enemy will move between.
        /// Based on xMax and yMax.
        /// </summary>
        private Vector3 posB;

        /// <summary>
        /// The player's ship.
        /// </summary>
        private GameObject player;

        /// <summary>
        /// Whether this enemy can shoot or not. Toggles when firing every 1/fireRate seconds.
        /// </summary>
        private bool canShoot = true;

        /// <summary>
        /// When this enemy was created.
        /// </summary>
        private float startTime;

        /// <summary>
        /// Finds the player. Runs when this enemy spawns.
        /// </summary>
        private void Start()
        {
            startTime = Time.time;
            player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                transform.LookAt(player.transform);
            }
            SetPosition();
        }

        /// <summary>
        /// Sets the two positions this enemy moves between.
        /// </summary>
        private void SetPosition()
        {
            posA = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            posB = new Vector3(transform.localPosition.x + xMax, transform.localPosition.y + yMax,
                transform.position.z);

            //posCenter = new Vector3(posA.x + posB.x, posA.y + posB.y) / 2.0f;
            //transform.position = posCenter;
        }

        /// <summary>
        /// Moves the enemy between posA and posB, and tries to look at the player and shoot at it when possible.
        /// Runs every Frame.
        /// </summary>
        private void Update()
        {
            transform.position = Vector3.Lerp(posA, posB,
                (Mathf.Sin(movingSpeed * (Time.time + startTime)) + 1.0f) / 2.0f);

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
            // reset materials
            for (int i = 0; i < renderers.Count; ++i)
            {
                renderers.ElementAt(i).material = materials.ElementAt(i);
            }

            hitFlash = null;

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