//=============================================================================
//
// Miguel Gotao
// 2264941
// gotao100@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// Enemy behavior that shoots a shotgun-like buckshot
//
//=============================================================================

using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using MirzaBeig.ParticleSystems;
using Random = UnityEngine.Random;

namespace Hive.Armada.Enemies
{
    /// <summary>
    /// Script in charge of a shotgun-like spread turret
    /// </summary>
    public class BuckshotTurret : Enemy
    {
        /// <summary>
        /// Type identifier for object pooling purposes
        /// </summary>
        private int projectileTypeIdentifier;

        /// <summary>
        /// Projectile that the turret shoots out
        /// </summary>
        //public GameObject fireProjectile;

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
        //private Vector3 pos;

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
        /// Number of projectiles that the bullet shoots out at once
        /// </summary>
        public float projectileCount;

        /// <summary>
        /// Value that calculates the next time at which the enemy is able to shoot again
        /// </summary>
        //private float fireNext;

        /// <summary>
        /// Whether this enemy can shoot or not. Toggles when firing every 1/fireRate seconds.
        /// </summary>
        private bool canShoot = true;
        
        /// <summary>
        /// Spread values determined by fireCone on each axis
        /// </summary>
        //private float randX;
        //private float randY;
        //private float randZ;

        /// <summary>
        /// Finds the player and instantiates pos for position holding
        /// </summary>
        private void Start()
        {
            //    player = GameObject.FindGameObjectWithTag("Player");
            //    pos = new Vector3(player.transform.position.x, player.transform.position.y,
            //        player.transform.position.z);
            Reset();
        }

        /// <summary>
        /// Have the enemy track the player position and
        /// calculate when it can fire projectiles
        /// </summary>
        private void Update()
        {
            if (player != null)
            {
                try
                {
                    //pos = player.transform.position;
                    transform.LookAt(player.transform);
                    if (canShoot)
                    {
                        //Debug.Log(projectileCount);
                        //canShoot = false;
                        for (int i = 0; i < projectileCount; ++i)
                        {
                            //Debug.Log("I'm shooting!");
                            StartCoroutine(Shoot());
                        }
                        //canShoot = true;
                    }
                }
                catch (Exception)
                {
                }
                //if (shaking)
                //{
                //    iTween.ShakePosition(gameObject, new Vector3(0.1f, 0.1f, 0.1f), 0.1f);
                //}
                SelfDestructCountdown();
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

        /// <summary>
        /// Spawn bullets and shoot them in accordance to set spread value
        /// </summary>
        /// <returns></returns>
        private IEnumerator Shoot()
        {
            canShoot = false;
            //Debug.Log("Hi!");
            GameObject projectile = objectPoolManager.Spawn(projectileTypeIdentifier, shootPoint.position,
                                        shootPoint.rotation);
            //randX = Random.Range(-spread, spread);
            //randY = Random.Range(-spread, spread);
            //randZ = Random.Range(-spread, spread);

            projectile.GetComponent<Transform>().Rotate(Random.Range(-spread, spread),
                                            Random.Range(-spread, spread),
                                            Random.Range(-spread, spread));

            projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * projectileSpeed;

            yield return new WaitForSeconds(fireRate);
            canShoot = true;
        }

        protected override void Reset()
        {
            // reset materials
            for (int i = 0; i < renderers.Count; ++i)
            {
                renderers.ElementAt(i).material = materials.ElementAt(i);
            }

            hitFlash = null;
            shaking = false;

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