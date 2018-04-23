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

using System;
using Hive.Armada.Data;
using UnityEngine;
using Random = UnityEngine.Random;

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
        /// The speed of the projectiles.
        /// </summary>
        private float projectileSpeed;

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

        private int hoverScale = 50;

        /// <summary>
        /// How long the hover movement will take, in seconds.
        /// </summary>
        private float hoverTime;

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
        }

        /// <summary>
        /// Runs the enemy logic every frame.
        /// </summary>
        protected void Update()
        {
            // Enemies only do this functionality after they are done pathing.
            if (!PathingComplete)
            {
                return;
            }

            // First move the enemy, if it is allowed to.
            if (canHover)
            {
                Hover(Time.deltaTime);
            }

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
                                     Time.deltaTime);
            }
        }

        /// <summary>
        /// Moves the enemy between the endpoints of the hover/movement.
        /// </summary>
        /// <param name="deltaTime"></param>
        private void Hover(float deltaTime)
        {
            hoverPercent += deltaTime / hoverTime;

            transform.position = Vector3.Lerp(startPosition, endPosition,
                                              Mathf.SmoothStep(0.0f, 1.0f, hoverPercent));

            // Hover/movement is complete.
            if (!(hoverPercent >= 1.0f))
            {
                return;
            }

            hoverPercent = 0.0f;

            // Switch the positions and reverse direction.
            Vector3 temp = endPosition;
            endPosition = startPosition;
            startPosition = temp;
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
            {
                shootPoint.Rotate(0.0f, 0.0f, 10.0f * fireRate);
            }

            GameObject projectile = objectPoolManager.Spawn(gameObject, currentProjectileTypeId,
                                                            shootPoint.position,
                                                            shootPoint.rotation);

            if (isProjectilePattern)
            {
                ProjectilePattern projectileScript = projectile.GetComponent<ProjectilePattern>();
                projectileScript.Launch(0);
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
                projectileScript.Launch(0);
            }
        }

        /// <summary>
        /// Checks if the fireRate time has passed.
        /// </summary>
        protected virtual void CheckShoot()
        {
            if (Time.time >= nextShootTime)
            {
                canShoot = true;
            }
        }

        /// <summary>
        /// Toggles post-pathing behavior, including hovering/movement.
        /// </summary>
        protected override void OnPathingComplete()
        {
            if (canHover)
            {
                SetHover();
            }

            base.OnPathingComplete();
        }

        /// <summary>
        /// Sets the endpoints for the hover/movement.
        /// </summary>
        private void SetHover()
        {
            if (isVertical)
            {
                // 
                hoverPercent = 0.5f;
                startPosition = transform.position -
                                new Vector3(0.0f, hoverDistance / hoverScale, 0.0f);
                endPosition = transform.position +
                              new Vector3(0.0f, hoverDistance / hoverScale, 0.0f);
            }
            else
            {
                hoverScale = 2;

                // Need to find what % gives us the spawn position.
                // SmoothStep makes it a bit harder than just inverting a sine function.
                float startOffset = 0.0f;
                float endOffset = 1.0f;
                float current = 0.0f;
                float spawn = Random.Range(0.375f, 0.625f);

                hoverPercent = 0.0f;

                // Move along unit movement until we find the spawn point.
                while (hoverPercent < 1.0f)
                {
                    hoverPercent += 0.0015f;

                    current = Mathf.SmoothStep(startOffset, endOffset, hoverPercent);

                    // This is or is close enough to the spawn point.
                    // Anything less than 0.015 results in overshooting.
                    if (Math.Abs(current - spawn) < 0.015f)
                    {
                        break;
                    }
                }

                // Overshot the spawn, resetting to the center point. This shouldn't happen.
                if (hoverPercent >= 1.0f)
                {
                    current = 0.5f;
                }

                startOffset -= current;
                endOffset -= current;

                // Multiply the calculated positions by the rotation to make them
                // relative to the direction the enemy is looking.
                // Multiply the offsets by the distance to convert from unit distance.
                startPosition = transform.position +
                                (transform.rotation *
                                 new Vector3(startOffset * hoverDistance / hoverScale, 0.0f, 0.0f));
                endPosition = transform.position +
                              (transform.rotation *
                               new Vector3(endOffset * hoverDistance / hoverScale, 0.0f, 0.0f));

                int roll = Random.Range(1, 100);

                if (roll <= 50)
                {
                    return;
                }

                // 50% of the time the enemy will start moving in the opposite direction.
                Vector3 temp = endPosition;
                endPosition = startPosition;
                startPosition = temp;

                // Debug.Log("PERCENT: " + hoverPercent + "\t SPAWN: " + spawn);
                //
                // GameObject mid = GameObject.CreatePrimitive(PrimitiveType.Cube);
                // mid.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                //
                // GameObject start = GameObject.CreatePrimitive(PrimitiveType.Cube);
                // start.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                // start.transform.position = startPosition;
                //
                // GameObject end = GameObject.CreatePrimitive(PrimitiveType.Cube);
                // end.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                // end.transform.position = endPosition;
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
            projectileSpeed = currentAttackPattern.projectileSpeed;
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