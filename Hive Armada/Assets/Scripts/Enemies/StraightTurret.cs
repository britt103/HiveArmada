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
        private short projectileTypeIdentifier = -2;

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
        /// \  |  /
        /// 7 - 0 - 3
        /// /  |  \
        /// 6   5   4
        /// </summary>
        public Transform[] shootPoint;

        /// <summary>
        /// How fast the turret shoots at a given rate
        /// </summary>
        private float fireRate;

        /// <summary>
        /// The rate at which enemy projectiles travel
        /// </summary>
        private float projectileSpeed;

        /// <summary>
        /// Size of conical spread the bullets travel within
        /// </summary>
        private float spread;

        /// <summary>
        /// Whether this enemy can shoot or not. Toggles when firing every 1/fireRate seconds.
        /// </summary>
        private bool canShoot = true;

        /// <summary>
        /// Whether or not the projectile being shot rotates.
        /// </summary>
        private bool canRotate;

        public Color projectileAlbedoColor;

        public Color projectileEmissionColor;

        [Header("Hovering")]
        public float xMax;

        public float yMax;

        public float movingSpeed;

        /// <summary>
        /// Variables for hovering
        /// </summary>
        private float theta;

        private Vector3 posA;

        private Vector3 posB;

        /// <summary>
        /// tracks player and shoots projectiles in that direction, while being slightly
        /// swayed via the spread value set by user. If player is not found automatically
        /// finds player, otherwise do nothing.
        /// </summary>
        private void Update()
        {
            if (PathingComplete)
            {
                transform.position = Vector3.Lerp(posA, posB, (Mathf.Sin(theta) + 1.0f) / 2.0f);

                theta += movingSpeed * Time.deltaTime;

                if (theta > Mathf.PI)
                {
                    theta -= Mathf.PI * 2;
                }

                transform.LookAt(player.transform);

                if (canShoot)
                {
                    StartCoroutine(Shoot());
                }

                if (shaking)
                {
                    iTween.ShakePosition(gameObject, new Vector3(0.05f, 0.05f, 0.05f), 0.1f);
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

            for (int point = 0; point < 9; ++point)
            {
                if (projectileArray[point])
                {
                    GameObject projectile = objectPoolManager.Spawn(
                        gameObject, projectileTypeIdentifier, shootPoint[point].position,
                        shootPoint[point].rotation);

                    projectile.GetComponent<Projectile>()
                              .SetColors(projectileAlbedoColor, projectileEmissionColor);
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
        /// <param name="mode"> Current Enemy Firemode </param>
        public override void SetAttackPattern(AttackPattern attackPattern)
        {
            base.SetAttackPattern(attackPattern);

            switch ((int) this.attackPattern)
            {
                // pattern One and Three
                case 0:
                case 2:
                default:
                    fireRate = 1.0f;
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

                // pattern Two and Four
                case 1:
                case 3:
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