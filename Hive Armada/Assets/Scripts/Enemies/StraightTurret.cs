//=============================================================================
//
// Miguel Gotao
// 2264941
// gotao100@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// Standard enemy behavior for shooting projectiles
//
//=============================================================================

using System.Collections;
using System.Linq;
using UnityEngine;

namespace Hive.Armada.Enemies
{
    public class StraightTurret : Enemy
    {
        /// <summary>
        /// Type identifier for object pooling purposes
        /// </summary>
        private int projectileTypeIdentifier;

        /// <summary>
        /// Projectile that the turret shoots out
        /// </summary>
        public GameObject fireProjectile;

        /// <summary>
        /// Structure holding bullet prefabs that
        /// the enemy will shoot
        /// </summary>
        public GameObject[] projectileArray;

        /// <summary>
        /// Position from which bullets are initially shot from
        /// </summary>
        public Transform shootPoint;

        /// <summary>
        /// How fast the turret shoots at a given rate
        /// </summary>
        public float fireRate;

        /// <summary>
        /// The rate at which enemy projectiles travel
        /// </summary>
        public float projectileSpeed;

        /// <summary>
        /// Size of conical spread the bullets travel within
        /// </summary>
        public float spread;

        /// <summary>
        /// Value that calculates the next time at which the enemy is able to shoot again
        /// </summary>
        private float fireNext;

        /// <summary>
        /// Value that determines what projectile the enemy will shoot
        /// as well as its parameters
        /// </summary>
        private int fireMode;

        /// <summary>
        /// Spread values determined by spread on each axis
        /// </summary>
        private float randX;

        private float randY;

        private float randZ;

        /// <summary>
        /// Whether this enemy can shoot or not. Toggles when firing every 1/fireRate seconds.
        /// </summary>
        private bool canShoot = true;

        /// <summary>
        /// Whether or not the projectile being shot rotates.
        /// </summary>
        private bool canRotate;

        /// <summary>
        /// Variables for hovering
        /// </summary>
        private float theta;
        private Vector3 posA;
        private Vector3 posB;
        public float xMax;
        public float yMax;
        public float movingSpeed;

        /// <summary>
        /// tracks player and shoots projectiles in that direction, while being slightly
        /// swayed via the spread value set by user. If player is not found automatically
        /// finds player, otherwise do nothing.
        /// </summary>
        private void Update()
        {
            if (pathingComplete)
            {
                transform.position = Vector3.Lerp(posA, posB, (Mathf.Sin(theta) + 1.0f) / 2.0f);

                theta += movingSpeed * Time.deltaTime;

                if (theta > Mathf.PI * 3 / 2)
                {
                    theta -= Mathf.PI * 2;
                }

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
            }
        }

        /// <summary>
        /// This is run after the enemy has completed its path.
        /// Calls Hover function to set positions to hover between
        /// </summary>
        protected override void OnPathingComplete()
        {
            Hover();

            pathingComplete = true;
        }

        /// <summary>
        /// Function that creates 2 vector 3's to float up and down with a Sin()
        /// </summary>
        private void Hover()
        {
            posA = new Vector3(transform.position.x + xMax / 100,
                transform.position.y + yMax / 100,
                transform.position.z);

            posB = new Vector3(transform.position.x - xMax / 100,
                transform.position.y - yMax / 100,
                transform.position.z);

            theta = 0.0f;
        }

        /// <summary>
        /// Fires projectiles in a pattern determined by the firemode at the player.
        /// </summary>
        private IEnumerator Shoot()
        {
            canShoot = false;

            GameObject projectile = objectPoolManager.Spawn(projectileTypeIdentifier, shootPoint.position,
                                                       shootPoint.rotation);
            randX = Random.Range(-spread, spread);
            randY = Random.Range(-spread, spread);
            randZ = Random.Range(-spread, spread);

            projectile.GetComponent<Transform>().Rotate(randX, randY, randZ);
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
                    fireRate = 1.0f;
                    projectileSpeed = 1.5f;
                    spread = 2;
                    canRotate = false;
                    fireProjectile = projectileArray[0];
                    break;

                case 2:
                    fireRate = 2.0f;
                    projectileSpeed = 1.5f;
                    spread = 0;
                    canRotate = true;
                    fireProjectile = projectileArray[1];
                    break;
            }
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