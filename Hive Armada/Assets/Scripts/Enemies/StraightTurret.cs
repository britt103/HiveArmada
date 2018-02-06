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
using MirzaBeig.ParticleSystems;

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
        /// Variable that finds the player GameObject
        /// </summary>
        private GameObject player;

        /// <summary>
        /// Vector3 that holds the player's position
        /// </summary>
        private Vector3 pos;

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

        //private bool fireModeSet = false;

        /// <summary>
        /// Final position after spawning.
        /// </summary>
        private Vector3 endPosition;

        /// <summary>
        /// Bools used to move the enemy to its spawn position.
        /// </summary>
        private bool spawnComplete;

        /// <summary>
        /// Bools used to move the enemy to its spawn position.
        /// </summary>
        private bool moveComplete;

        /// <summary>
        /// tracks player and shoots projectiles in that direction, while being slightly
        /// swayed via the spread value set by user. If player is not found automatically
        /// finds player, otherwise do nothing.
        /// </summary>
        private void Update()
        {
            if (spawnComplete)
            {
                if (moveComplete)
                {
                    if (player != null)
                    {
                        pos = player.transform.position;
                        transform.LookAt(pos);

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
                            transform.LookAt(new Vector3(0.0f, 0.0f, 0.0f));
                        }
                    }
                }
                else
                {
                    transform.position =
                        Vector3.Lerp(transform.position, endPosition, Time.deltaTime * 1.0f);
                    if (Vector3.Distance(transform.position, endPosition) <= 0.1f)
                    {
                        moveComplete = true;
                    }
                }
            }

            //if (shaking)
            //{
            //    iTween.ShakePosition(gameObject, new Vector3(0.01f, 0.01f, 0.01f), Time.deltaTime);
            //}
            SelfDestructCountdown();
        }

        /// <summary>
        /// Fires projectiles in a pattern determined by the firemode at the player.
        /// </summary>
        private IEnumerator Shoot()
        {
            canShoot = false;

            GameObject projectile = objectPoolManager.Spawn(projectileTypeIdentifier,
                                                            shootPoint.position,
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
        /// <param name="mode"> Current Enemy Firemode </param>
        private void switchFireMode(int mode)
        {
            switch (mode)
            {
                case 1:
                    fireRate = 0.6f;
                    projectileSpeed = 1.5f;
                    spread = 2;
                    canRotate = false;
                    fireProjectile = projectileArray[0];
                    break;

                case 2:
                    fireRate = 0.3f;
                    projectileSpeed = 1.5f;
                    spread = 0;
                    canRotate = true;
                    fireProjectile = projectileArray[1];
                    break;
            }
        }

        /// <summary>
        /// Runs when this enemy finishes default pathing to a SpawnZone.
        /// </summary>
        /// <param name="endPos"> Final position of this enemy. </param>
        public void SetEndpoint(Vector3 endPos)
        {
            endPosition = endPos;
            spawnComplete = true;
        }
        ///// <summary>
        ///// Runs when this enemy is at endPos.
        ///// </summary>
        //public void MoveComplete()
        //{
        //    moveComplete = true;
        //}

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

            spawnComplete = false;
            moveComplete = false;
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