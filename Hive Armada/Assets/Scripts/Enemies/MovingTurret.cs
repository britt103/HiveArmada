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
        /// Structure holding bullet prefabs that
        /// the enemy will shoot
        /// </summary>
        public GameObject[] projectileArray;

        /// <summary>
        /// Projectile that the turret shoots out
        /// </summary>
        public GameObject fireProjectile;

        /// <summary>
        /// Value that determines what projectile the enemy will shoot
        /// as well as its parameters
        /// </summary>
        private int fireMode;

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
        /// Whether or not the projectile being shot rotates.
        /// </summary>
        private bool canRotate;

        /// <summary>
        /// Angle used for moving with Mathf.Sin.
        /// </summary>
        private float theta;

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
            if (pathingComplete)
            {
                if (reference.playerShip != null)
                {
                    lookTarget = reference.playerShip.transform.position;
                }

                if (lookTarget != Vector3.negativeInfinity)
                {
                    transform.LookAt(lookTarget);

                    if (canShoot)
                    {
                        StartCoroutine(Shoot());
                    }
                }
                else
                {
                    transform.LookAt(new Vector3(0.0f, 0.7f, 0.0f));
                }

                transform.position = Vector3.Lerp(posA, posB, (Mathf.Sin(theta) + 1.0f) / 2.0f);

                theta += movingSpeed * Time.deltaTime;

                if (theta > Mathf.PI * 3 / 2)
                {
                    theta -= Mathf.PI * 2;
                }

				if (shaking)
            	{
                	iTween.ShakePosition(gameObject, new Vector3(0.1f, 0.1f, 0.1f), 0.1f);
            	}
            }
        }

        /// <summary>
        /// Fires projectiles in a pattern determined by the firemode at the player.
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

            if (canRotate)
            {
                StartCoroutine(rotateProjectile(projectile));
            }

            yield return new WaitForSeconds(fireRate);

            canShoot = true;

        }

        private IEnumerator rotateProjectile(GameObject bullet)
        {
            while (true)
            {
                bullet.GetComponent<Transform>().Rotate(0, 0, 1);
                yield return new WaitForSeconds(0.01f);
            }
        }

        /// <summary>
        /// Function that determines the enemy's projectile, firerate,
        /// spread, and projectile speed.
        /// </summary>
        /// <param name="mode">Current Enemy Firemode</param>
        private void switchFireMode(int mode)
        {
            switch (mode)
            {
                case 1:
                    fireRate = 0.6f;
                    projectileSpeed = 1.5f;
                    spread = 2;
                    fireProjectile = projectileArray[0];
                    break;

                case 2:
                    fireRate = 0.3f;
                    projectileSpeed = 1.5f;
                    spread = 0;
                    fireProjectile = projectileArray[1];
                    break;
            }
        }

        /// <summary>
        /// This is run after the enemy has completed its path.
        /// </summary>
        protected override void OnPathingComplete()
        {
            SetPosition();

            pathingComplete = true;
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

            pathingComplete = false;
            hitFlash = null;
            shaking = false;
            canShoot = true;

            projectileTypeIdentifier =
                enemyAttributes.EnemyProjectileTypeIdentifiers[TypeIdentifier];
            maxHealth = enemyAttributes.enemyHealthValues[TypeIdentifier];
            Health = maxHealth;
            fireRate = enemyAttributes.enemyFireRate[TypeIdentifier];
            projectileSpeed = enemyAttributes.projectileSpeed;
            spread = enemyAttributes.enemySpread[TypeIdentifier];
            pointValue = enemyAttributes.enemyScoreValues[TypeIdentifier];
            selfDestructTime = enemyAttributes.enemySelfDestructTimes[TypeIdentifier];
            deathEmitter = enemyAttributes.enemyDeathEmitters[TypeIdentifier];

            if (!isInitialized)
            {
                isInitialized = true;
                deathEmitterTypeIdentifier = objectPoolManager.GetTypeIdentifier(deathEmitter);
            }
        }
    }
}