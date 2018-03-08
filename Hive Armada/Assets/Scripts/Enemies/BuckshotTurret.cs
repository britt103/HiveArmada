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
        /// Variables for hovering
        /// </summary>
        private float theta;
        private Vector3 posA;
        private Vector3 posB;
        public float xMax;
        public float yMax;
        public float movingSpeed;

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
            if (PathingComplete)
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

                    transform.LookAt(lookTarget);
                }

                if (lookTarget != Vector3.negativeInfinity)
                {
                    if (canShoot)
                    {
                        for (int i = 0; i < projectileCount; ++i)
                        {
                            StartCoroutine(Shoot());
                        }
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