//=============================================================================
// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-440-01
// Group Project
// 
// This abstract class is the base for all enemies that shoot projectiles.
// 
//=============================================================================

using Hive.Armada.Data;
using Hive.Armada.Game;
using UnityEngine;
using Valve.VR;

namespace Hive.Armada.Enemies
{
    /// <summary>
    /// Base class for all enemies with shooting functionality.
    /// </summary>
    public class ShootingEnemy : Enemy
    {
        /// <summary>
        /// How strongly the enemy should look at the player ship.
        /// </summary>
        [Range(0, 5)]
        public int lookStrength;

        /// <summary>
        /// The transform that projectiles are shot from.
        /// </summary>
        public Transform shootPoint;

        /// <summary>
        /// Time between shots, in seconds.
        /// </summary>
        protected float fireRate;

        /// <summary>
        /// If the projectiles should be fired with spread.
        /// </summary>
        private float spread;

        /// <summary>
        /// If the current attack pattern rotates.
        /// </summary>
        private bool canRotate;

        /// <summary>
        /// If the current attack pattern shoots a projectile pattern.
        /// </summary>
        private bool isProjectilePattern;

        /// <summary>
        /// The type IDs for all attack pattern projectiles.
        /// </summary>
        private short[] projectileTypeIds;

        /// <summary>
        /// The type ID for the current projectile.
        /// </summary>
        private short currentProjectileTypeId;

        /// <summary>
        /// All attack patterns for this enemy.
        /// </summary>
        protected AttackPatternData[] attackPatternData;

        /// <summary>
        /// The attack pattern attributes for the current attack pattern.
        /// </summary>
        private AttackPatternData currentAttackPattern;

        /// <summary>
        /// The time the enemy will be able to shoot again.
        /// </summary>
        protected float nextShootTime;

        /// <summary>
        /// If the enemy can shoot or not.
        /// </summary>
        protected bool canShoot;

        /// <summary>
        /// If this enemy can hover or not.
        /// </summary>
        private bool canHover;

        /// <summary>
        /// If the hover movement is vertical. False means horizontal.
        /// </summary>
        private bool isVertical;

        /// <summary>
        /// The distance the hover movement will span.
        /// </summary>
        private float hoverDistance;

        /// <summary>
        /// How long the hover movement will take, in seconds.
        /// </summary>
        private float hoverTime;

        /// <summary>
        /// Scales down the hover movement to not be quite so drastic.
        /// </summary>
        private int hoverScale;

        /// <summary>
        /// The percentage of completion of the hover movement.
        /// </summary>
        private float hoverPercent;

        /// <summary>
        /// The start position of the current hover movement.
        /// </summary>
        private Vector3 startPosition;

        /// <summary>
        /// The end position of the current hover movement.
        /// </summary>
        private Vector3 endPosition;

        private int count;

        /// <summary>
        /// Initializes the attributes for this shooting enemy.
        /// </summary>
        /// <param name="enemyData"> The ScriptableObject with the attributes </param>
        protected void Initialize(ShootingEnemyData enemyData)
        {
            base.Initialize(enemyData);

            projectileTypeIds = new[]
                                {
                                    reference.objectPoolManager.GetTypeIdentifier(
                                        enemyData.pattern1.prefab),
                                    reference.objectPoolManager.GetTypeIdentifier(
                                        enemyData.pattern2.prefab),
                                    reference.objectPoolManager.GetTypeIdentifier(
                                        enemyData.pattern3.prefab),
                                    reference.objectPoolManager.GetTypeIdentifier(
                                        enemyData.pattern4.prefab)
                                };

            attackPatternData = new[]
                                {
                                    enemyData.pattern1,
                                    enemyData.pattern2,
                                    enemyData.pattern3,
                                    enemyData.pattern4
                                };

            canHover = enemyData.canHover;
            isVertical = enemyData.isVertical;
            hoverDistance = enemyData.hoverTime;
            hoverTime = enemyData.hoverTime;
            hoverScale = enemyData.hoverScale;
        }

        /// <summary>
        /// Runs the enemy logic every frame.
        /// </summary>
        protected void Update()
        {
            // Enemies only do this functionality after they are done pathing.
            if (!PathingComplete)
                return;

            // First move the enemy, if it is allowed to.
            if (canHover)
                Hover(Time.deltaTime);

            // Then make it look at the player.
            LookAtShip();

            // Then shoot at the player.
            if (canShoot)
            {
                Shoot();
            }
            else
            {
                // Check if the firerate cooldown has passed.
                CheckShoot();
            }

            // Finally shake the enemy if it is low on health.
            if (shaking)
            {
                iTween.ShakePosition(gameObject, new Vector3(0.05f, 0.05f, 0.05f),
                                     0.01f);
            }
        }

        /// <summary>
        /// Moves the enemy between the endpoints of the hover/movement.
        /// </summary>
        /// <param name="deltaTime"></param>
        private void Hover(float deltaTime)
        {
            hoverPercent += deltaTime / hoverTime;
            
            if (isVertical)
            {
                transform.position = Vector3.Lerp(startPosition, endPosition,
                                                  Mathf.SmoothStep(0.0f, 1.0f, hoverPercent));

                // Hover/movement is complete.
                if (!(hoverPercent >= 1.0f))
                    return;

                hoverPercent = 0.0f;

                // Switch the positions and reverse direction.
                Vector3 temp = endPosition;
                endPosition = startPosition;
                startPosition = temp;
            }
            else
            {
                transform.position = Vector3.Lerp(startPosition, endPosition,
                                                  (Mathf.Sin(hoverPercent) + 1.0f) / 2.0f);

                if (hoverPercent > Mathf.PI)
                    hoverPercent -= 2 * Mathf.PI;
            }
        }

        /// <summary>
        /// Looks at the player ship.
        /// </summary>
        private void LookAtShip()
        {
            Quaternion to =
                Quaternion.LookRotation(shipLookTarget.transform.position - transform.position);

            transform.rotation =
                Quaternion.Slerp(transform.rotation, to, lookStrength * Time.deltaTime);

            // transform.LookAt(shipLookTarget);
        }

        /// <summary>
        /// Shoots the current projectile or projectile pattern.
        /// </summary>
        private void Shoot()
        {
            canShoot = false;
            nextShootTime = Time.time + fireRate;

            if (canRotate)
                shootPoint.Rotate(0.0f, 0.0f, 10.0f * fireRate);

            GameObject projectile = objectPoolManager.Spawn(gameObject, currentProjectileTypeId,
                                                            shootPoint.position,
                                                            shootPoint.rotation);

            if (isProjectilePattern)
            {
                ProjectilePattern projectileScript = projectile.GetComponent<ProjectilePattern>();
                projectileScript.Launch();
            }
            else
            {
                // Only run expensive GetComponent if it needs to.
                if (spread > 0.0f)
                {
                    projectile.GetComponent<Transform>().Rotate(Random.Range(-spread, spread),
                                                                Random.Range(-spread, spread),
                                                                Random.Range(-spread, spread));
                }

                Projectile projectileScript = projectile.GetComponent<Projectile>();
                projectileScript.Launch();
            }
        }

        /// <summary>
        /// Checks if the fireRate time has passed.
        /// </summary>
        protected virtual void CheckShoot()
        {
            if (Time.time >= nextShootTime)
                canShoot = true;
        }

        /// <summary>
        /// Toggles post-pathing behavior, including hovering/movement.
        /// </summary>
        protected override void OnPathingComplete()
        {
            if (canHover)
                SetHover();

            base.OnPathingComplete();
        }

        /// <summary>
        /// Sets the endpoints for the hover/movement.
        /// </summary>
        private void SetHover()
        {
            if (isVertical)
            {
                hoverPercent = 0.5f;
                startPosition = transform.position -
                                new Vector3(0.0f, hoverDistance / hoverScale, 0.0f);
                endPosition = transform.position +
                              new Vector3(0.0f, hoverDistance / hoverScale, 0.0f);
            }
            else
            {
                float percent = Random.Range(0.4f, 0.6f);

                startPosition = new Vector3(transform.position.x - hoverDistance * percent,
                                            transform.position.y,
                                            transform.position.z);
                endPosition = new Vector3(transform.position.x + hoverDistance * (1.0f - percent),
                                          transform.position.y,
                                          transform.position.z);

                hoverPercent = Mathf.Asin(2 * percent - 1);

                 int roll = Random.Range(1, 100);
                
                 if (roll <= 50)
                     return;
                
                hoverPercent = Mathf.Asin(2 * (1 - percent) - 1);
                
                 // 50% of the time the enemy will start moving in the opposite direction.
                 Vector3 temp = endPosition;
                 endPosition = startPosition;
                 startPosition = temp;
            }
        }

        /// <summary>
        /// Sets the attack pattern and updates related values.
        /// </summary>
        /// <param name="newAttackPattern"> The new attack pattern to use </param>
        public override void SetAttackPattern(AttackPattern newAttackPattern)
        {
            base.SetAttackPattern(newAttackPattern);
            currentAttackPattern = attackPatternData[(int) newAttackPattern];

            fireRate = currentAttackPattern.fireRate;

            if (reference.gameSettings.selectedFireRate != GameSettings.FireRate.Normal)
            {
                fireRate /= reference.gameSettings.fireRatePercents[(int) reference.gameSettings
                                                                                   .selectedFireRate];
            }

            spread = currentAttackPattern.spread;
            canRotate = currentAttackPattern.canRotate;
            isProjectilePattern = currentAttackPattern.isProjectilePattern;
            currentProjectileTypeId = projectileTypeIds[(int) newAttackPattern];
        }

        /// <summary>
        /// Reset any values that should not persist across spawns of this enemy.
        /// </summary>
        protected override void Reset()
        {
            base.Reset();

            canShoot = true;
            SetAttackPattern(AttackPattern.One);
            shootPoint.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        }
    }
}