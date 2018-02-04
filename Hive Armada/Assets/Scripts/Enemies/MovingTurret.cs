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
using MirzaBeig.ParticleSystems;
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
        /// Angle used for moving with Mathf.Sin.
        /// </summary>
        private float theta;

        // <summary>
        /// Final position after spawning.
        /// </summary>
        private Vector3 endPosition;

        /// <summary>
        /// Bools used to move the enemy to its spawn position.
        /// </summary>
        bool spawnComplete;

        /// <summary>
        /// Bools used to move the enemy to its spawn position.
        /// </summary>
        bool moveComplete;

        /// <summary>
        /// Finds the player. Runs when this enemy spawns.
        /// </summary>
        private void Start()
        {
            player = reference.playerShip;
            if (player != null)
            {
                transform.LookAt(player.transform);
            }
            SetPosition();
        }

        private void OnEnable()
        {
            SetPosition();
        }

        /// <summary>
        /// Sets the two positions this enemy moves between.
        /// </summary>
        private void SetPosition()
        {
            posA = new Vector3(transform.position.x - xMax / 2,
                               transform.position.y - yMax / 2,
                               transform.position.z);
            posB = new Vector3(transform.position.x + xMax / 2,
                               transform.position.y + yMax / 2,
                               transform.position.z);

            theta = 0.0f;
        }

        /// <summary>
        /// Moves the enemy between posA and posB, and tries to look at
        /// the player and shoot at it when possible.
        /// </summary>
        private void Update()
        {
            if (spawnComplete)
            {
                if (moveComplete)
                {
                    transform.position = Vector3.Lerp(posA, posB, (Mathf.Sin(theta) + 1.0f) / 2.0f);

                    theta += movingSpeed * Time.deltaTime;

                    if (theta > Mathf.PI * 3 / 2)
                    {
                        theta -= Mathf.PI * 2;
                    }

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
                }
                else
                {
                    transform.position = Vector3.Lerp(transform.position, endPosition, Time.deltaTime * 1.0f);
                    if (Vector3.Distance(transform.position, endPosition) <= 0.1f)
                    {
                        moveComplete = true;
                    }

                }
            }

            //if (shaking)
            //{
            //    iTween.ShakePosition(gameObject, new Vector3(0.1f, 0.1f, 0.1f), 0.1f);

            //}
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
        /// Runs when this enemy finishes default pathing to a SpawnZone.
        /// </summary>
        /// <param name="endPos">Final position of this enemy.</param>
        public void SetEndpoint(Vector3 endPos)
        {
            endPosition = endPos;
            spawnComplete = true;
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
            shaking = false;
            canShoot = true;
            spawnComplete = false;
            moveComplete = false;

            projectileTypeIdentifier =
                enemyAttributes.EnemyProjectileTypeIdentifiers[TypeIdentifier];
            maxHealth = enemyAttributes.enemyHealthValues[TypeIdentifier];
            Health = maxHealth;
            fireRate = enemyAttributes.enemyFireRate[TypeIdentifier];
            projectileSpeed = enemyAttributes.projectileSpeed;
            spread = enemyAttributes.enemySpread[TypeIdentifier];
            pointValue = enemyAttributes.enemyScoreValues[TypeIdentifier];
            selfDestructTime = enemyAttributes.enemySelfDestructTimes[TypeIdentifier];
            spawnEmitter = enemyAttributes.enemySpawnEmitters[TypeIdentifier];
            deathEmitter = enemyAttributes.enemyDeathEmitters[TypeIdentifier];

            if (!isInitialized)
            {
                isInitialized = true;

                GameObject spawnEmitterObject = Instantiate(spawnEmitter,
                                                            transform.position,
                                                            transform.rotation, transform);
                spawnEmitterSystem = spawnEmitterObject.GetComponent<ParticleSystems>();

                deathEmitterTypeIdentifier = objectPoolManager.GetTypeIdentifier(deathEmitter);
            }
        }
    }
}