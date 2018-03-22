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
        private short projectileTypeIdentifier = -2;

        /// <summary>
        /// The point where this enemy shoots from.
        /// </summary>
        public Transform[] shootPoint;

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
        public bool[] projectileArray;

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
            if (PathingComplete)
            {
                transform.LookAt(player.transform);

                if (canShoot)
                {
                    StartCoroutine(Shoot());
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

            for (int point = 0; point < 9; ++point)
            {
                if (projectileArray[point])
                {
                    GameObject projectile = objectPoolManager.Spawn(gameObject, projectileTypeIdentifier, shootPoint[point].position,
                                                       shootPoint[point].rotation);
                    
                    projectile.GetComponent<Transform>().Rotate(Random.Range(-spread, spread), 
                        Random.Range(-spread, spread),
                        Random.Range(-spread, spread));
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
            base.SetAttackPattern(attackPattern);

            switch ((int)this.attackPattern)
            {
                //standard pattern, single bullets
                case 0:
                    fireRate = 0.8f;
                    projectileSpeed = 1.5f;
                    spread = 0;
                    //canRotate = false;
                    projectileArray[0] = true;
                    projectileArray[1] = true;
                    projectileArray[2] = false;
                    projectileArray[3] = false;
                    projectileArray[4] = true;
                    projectileArray[5] = false;
                    projectileArray[6] = true;
                    projectileArray[7] = false;
                    projectileArray[8] = false;
                    break;

                case 1:
                    fireRate = 0.8f;
                    projectileSpeed = 1.5f;
                    spread = 0;
                    //canRotate = true;
                    projectileArray[0] = false;
                    projectileArray[1] = true;
                    projectileArray[2] = true;
                    projectileArray[3] = true;
                    projectileArray[4] = true;
                    projectileArray[5] = true;
                    projectileArray[6] = true;
                    projectileArray[7] = true;
                    projectileArray[8] = true;
                    break;
            }
        }

        /// <summary>
        /// This is run after the enemy has completed its path.
        /// </summary>
        protected override void OnPathingComplete()
        {
            SetPosition();
            base.OnPathingComplete();
        }

        /// <summary>
        /// Resets attributes to this enemy's defaults from enemyAttributes.
        /// </summary>
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