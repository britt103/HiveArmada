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
        //public GameObject fireProjectile;

        /// <summary>
        /// Structure resposible for tracking the positions for which bullets
        /// are going to be spawned from, dependent on firing pattern.
        /// </summary>
        public bool[] projectileArray;

        /// <summary>
        /// Positions from which bullets are initially shot from
        /// Positions start from the center, then move counterclockwise from north
        /// Diagram for reference:
        /// 8   1   2
        ///  \  |  /
        /// 7 - 0 - 3
        ///  /  |  \
        /// 6   5   4
        /// </summary>
        public Transform[] shootPoint;

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
        //private float fireNext;

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

        ///// <summary>
        ///// On start, select enemy behavior based on value fireMode
        ///// </summary>
        //void Start()
        //{
            
        //}

        /// <summary>
        /// tracks player and shoots projectiles in that direction, while being slightly
        /// swayed via the spread value set by user. If player is not found automatically
        /// finds player, otherwise do nothing.
        /// </summary>
        private void Update()
        {
            /// Ghetto set firemode
            //if (!fireModeSet)
            //{
            //    fireMode = 2;
            //    switchFireMode(fireMode);
            //    fireModeSet = true;
            //}

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

            for(int point = 0; point < 9; ++point)
            {
                if(projectileArray[point] == true)
                {
                    GameObject projectile = objectPoolManager.Spawn(projectileTypeIdentifier, shootPoint[point].position,
                                                       shootPoint[point].rotation);

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
                }
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
        public override void SetAttackPattern(AttackPattern attackPattern)
        {
            switch ((int)attackPattern)
            {
                //standard pattern, single bullets
                case 0:
                    fireRate = 0.6f;
                    projectileSpeed = 1.5f;
                    spread = 2;
                    //canRotate = false;
                    projectileArray[0] = true;
                    projectileArray[1] = false;
                    projectileArray[2] = false;
                    projectileArray[3] = false;
                    projectileArray[4] = false;
                    projectileArray[5] = false;
                    projectileArray[6] = false;
                    projectileArray[7] = false;
                    projectileArray[8] = false;
                    break;

                case 1:
                    fireRate = 1.2f;
                    projectileSpeed = 1.5f;
                    spread = 0;
                    //canRotate = true;
                    projectileArray[0] = true;
                    projectileArray[1] = true;
                    projectileArray[2] = false;
                    projectileArray[3] = true;
                    projectileArray[4] = false;
                    projectileArray[5] = true;
                    projectileArray[6] = false;
                    projectileArray[7] = true;
                    projectileArray[8] = false;
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