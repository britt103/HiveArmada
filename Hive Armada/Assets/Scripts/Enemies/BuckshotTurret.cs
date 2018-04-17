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

using System.Collections;
using System.Linq;
using UnityEngine;
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
        private short projectileTypeIdentifier = -2;

        /// <summary>
        /// Projectile that the turret shoots out
        /// </summary>
        public bool[] projectileArray;

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
        /// Number of projectiles that the bullet shoots out at once
        /// </summary>
        public float projectileCount;

        /// <summary>
        /// Whether this enemy can shoot or not. Toggles when firing every 1/fireRate seconds.
        /// </summary>
        private bool canShoot = true;

        public Color projectileAlbedoColor;

        public Color projectileEmissionColor;

        /// <summary>
        /// Variables for hovering
        /// </summary>
        private float theta;
        private Vector3 posA;
        private Vector3 posB;
        public float xMax;
        public float yMax;
        public float movingSpeed;

        public GameObject projectilePattern;

        private short patternId = -2;

        /// <summary>
        /// Finds the player and instantiates pos for position holding
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            player = reference.playerShip;
            //pos = new Vector3(player.transform.position.x, player.transform.position.y,
            //    player.transform.position.z);
            //gameObject.SendMessage("Initialize", 1, SendMessageOptions.DontRequireReceiver);
            //gameObject.SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
            //Reset();
            SetAttackPattern(AttackPattern.One);
            patternId = objectPoolManager.GetTypeIdentifier(projectilePattern);
        }

        /// <summary>
        /// Have the enemy track the player position and
        /// calculate when it can fire projectiles
        /// </summary>
        private void Update()
        {
            if (PathingComplete)
            {
                transform.position = Vector3.Lerp(posA, posB, (Mathf.Sin(theta) + 1.0f) / 2.0f);

                theta += movingSpeed * Time.deltaTime;

                if (theta > Mathf.PI * 3 / 2)
                {
                    theta -= Mathf.PI * 2;
                }

                transform.LookAt(player.transform);

                if (canShoot)
                {
                    for (int i = 0; i < projectileCount; ++i)
                    {
                        StartCoroutine(Shoot());
                    }
                }

                if (shaking)
                {
                    iTween.ShakePosition(gameObject, new Vector3(0.1f, 0.1f, 0.1f), 0.1f);
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
            base.OnPathingComplete();
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

            //for (int point = 0; point < 9; ++point)
            //{
            //    if (projectileArray[point] == true)
            //    {
            //        GameObject projectile = objectPoolManager.Spawn(gameObject, projectileTypeIdentifier, shootPoint[point].position,
            //                                           shootPoint[point].rotation);

            //        projectile.GetComponent<Transform>().Rotate(Random.Range(-spread, spread),
            //                                                    Random.Range(-spread, spread),
            //                                                    Random.Range(-spread, spread));
            //        projectile.GetComponent<Projectile>()
            //                  .SetColors(projectileAlbedoColor, projectileEmissionColor);
            //        Projectile projectileScript = projectile.GetComponent<Projectile>();
            //        projectileScript.Launch(0);
            //        //projectile.GetComponent<Rigidbody>().velocity =
            //        //    projectile.transform.forward * projectileSpeed;

            //        //if (canRotate)
            //        //{
            //        //    StartCoroutine(rotateProjectile(projectile));
            //        //}
            //    }
            //}

            GameObject projectile = objectPoolManager.Spawn(gameObject, patternId, shootPoint.position,
                                                            shootPoint.rotation);

            ProjectilePattern projectileScript = projectile.GetComponent<ProjectilePattern>();
            projectileScript.Launch(0);

            yield return new WaitForSeconds(fireRate);
            canShoot = true;
        }

        /// <summary>
        /// Function that determines the enemy's projectile, firerate,
        /// spread, and projectile speed.
        /// </summary>
        /// <param name="mode">Current Enemy Firemode</param>
        public override void SetAttackPattern(AttackPattern attackPattern)
        {
            base.SetAttackPattern(attackPattern);

            switch ((int)this.attackPattern)
            {
                //standard pattern, single bullets
                case 0:
                    fireRate = 1.0f;
                    projectileSpeed = 1.5f;
                    spread = 0;
                    projectileCount = 1;
                    //canRotate = false;
                    //projectileArray[0] = true;
                    //projectileArray[1] = true;
                    //projectileArray[2] = true;
                    //projectileArray[3] = true;
                    //projectileArray[4] = true;
                    //projectileArray[5] = true;
                    //projectileArray[6] = true;
                    //projectileArray[7] = true;
                    //projectileArray[8] = true;
                    break;

                case 1:
                    fireRate = 1.2f;
                    projectileSpeed = 1.5f;
                    spread = 0.5f;
                    projectileCount = 3;
                    //canRotate = true;
                    //projectileArray[0] = false;
                    //projectileArray[1] = false;
                    //projectileArray[2] = true;
                    //projectileArray[3] = false;
                    //projectileArray[4] = true;
                    //projectileArray[5] = false;
                    //projectileArray[6] = true;
                    //projectileArray[7] = false;
                    //projectileArray[8] = true;
                    break;
            }
        }

        protected override void Reset()
        {
            base.Reset();

            canShoot = true;
            projectileTypeIdentifier =
                enemyAttributes.EnemyProjectileTypeIdentifiers[TypeIdentifier];
            fireRate = enemyAttributes.enemyFireRate[TypeIdentifier];
            projectileSpeed = enemyAttributes.projectileSpeed;
            spread = enemyAttributes.enemySpread[TypeIdentifier];
        }
    }
}