//=============================================================================
//
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// Basic enemy that is spawned when the Splitter dies. Simply looks at the
// player and shoots.
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
    /// Splitter child enemy
    /// </summary>
    public class SplitterChild : Enemy
    {
        /// <summary>
        /// Type identifier for this enemy's projectiles in objectPoolManager
        /// </summary>
        private short projectileTypeIdentifier = -2;

        /// <summary>
        /// The point where this enemy shoots from.
        /// </summary>
        public Transform shootPoint;

        /// <summary>
        /// How many time per second this enemy can shoot.
        /// </summary>
        private float fireRate;

        /// <summary>
        /// Number of bursts the turret will shoot before going on cooldown
        /// Leave at 1 for regular fire
        /// </summary>
        public int burstFire = 1;

        /// <summary>
        /// Degrees that the projectile can be randomly rotated.
        /// Randomly picks in the range of [-spread, spread] for all 3 axes.
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

        private WaitForSeconds waitFire;

        /// <summary>
        /// Tries to look at the player and shoot at it when possible. Runs every frame.
        /// </summary>
        private void Update()
        {
            transform.LookAt(player.transform);

            if (canShoot)
            {
                StartCoroutine(Shoot());
            }

            if (shaking)
            {
                iTween.ShakePosition(gameObject, new Vector3(0.1f, 0.1f, 0.1f), 0.1f);
            }

            SelfDestructCountdown();
        }

        /// <summary>
        /// Fires projectiles in a pattern determined by the firemode at the player.
        /// </summary>
        private IEnumerator Shoot()
        {
            canShoot = false;

            for (int x = 0; x < burstFire; x++)
            {
                GameObject projectile =
                    objectPoolManager.Spawn(gameObject, projectileTypeIdentifier,
                                            shootPoint.position,
                                            shootPoint.rotation);

                projectile.GetComponent<Transform>().Rotate(Random.Range(-spread, spread),
                                                            Random.Range(-spread, spread),
                                                            Random.Range(-spread, spread));
                projectile.GetComponent<Projectile>()
                              .SetColors(projectileAlbedoColor, projectileEmissionColor);
                Projectile projectileScript = projectile.GetComponent<Projectile>();
                projectileScript.Launch(0);

                if (canRotate)
                {
                    StartCoroutine(rotateProjectile(projectile));
                }
                yield return Utility.waitOneTenth;
            }

            yield return waitFire;

            canShoot = true;
        }

        private IEnumerator rotateProjectile(GameObject bullet)
        {
            while (true)
            {
                bullet.GetComponent<Transform>().Rotate(0, 0, 10);
                yield return Utility.waitOneTenth;
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
                case 0:
                    fireRate = 0.6f;
                    spread = 2;
                    burstFire = 3;
                    break;

                case 1:
                    fireRate = 1.2f;
                    spread = 0;
                    burstFire = 5;
                    break;
            }
        }

        private void SetBurstFire(int burst)
        {
            burstFire = burst;
        }

        protected override void Kill()
        {
            try
            {
                iTween.Stop(gameObject);
            }
            catch (Exception)
            {
                //
            }

            base.Kill();
        }

        /// <summary>
        /// Resets attributes to this enemy's defaults from enemyAttributes.
        /// </summary>
        protected override void Reset()
        {
            base.Reset();

            canShoot = true;
            PathingComplete = true;
            projectileTypeIdentifier =
                enemyAttributes.EnemyProjectileTypeIdentifiers[TypeIdentifier];
            fireRate = enemyAttributes.enemyFireRate[TypeIdentifier];
            spread = enemyAttributes.enemySpread[TypeIdentifier];
            spawnEmitter = enemyAttributes.enemySpawnEmitters[TypeIdentifier];

            waitFire = new WaitForSeconds(fireRate);

            if (!isInitialized)
            {
                isInitialized = true;

                GameObject spawnEmitterObject = Instantiate(spawnEmitter,
                                                            transform.position,
                                                            transform.rotation, transform);
                spawnEmitterSystem = spawnEmitterObject.GetComponent<ParticleSystems>();
            }
        }
    }
}