//=============================================================================
//
// Miguel Gotao
// 2264941
// gotao100@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// Behavior of the recurring boss throughout the game
//
//=============================================================================

using System.Collections;
using System.Linq;
using UnityEngine;
using MirzaBeig.ParticleSystems;

namespace Hive.Armada.Enemies
{
    public class Boss : Enemy
    {
        /// <summary>
        /// Type identifier for object pooling purposes
        /// </summary>
        private int projectileTypeIdentifier;

        /// <summary>
        /// Structure resposible for tracking the positions for which bullets
        /// are going to be spawned from, dependent on firing pattern.
        /// </summary>
        public bool[] projectileArray;

        /// <summary>
        /// Structure responsible for what behavior the boss is going to use
        /// </summary>
        public int[] behaviorArray;

        /// <summary>
        /// Positions from which bullets are initially shot from
        /// Positions are arranged in a 9x9 grid
        /// </summary>
        public Transform[] shootPoint;

        /// <summary>
        /// Transform of the shoot point, to be used for altering attack patterns
        /// </summary>
        public Transform shootPivot;

        /// <summary>
        /// Variable that finds the player GameObject
        /// </summary>
        private GameObject player;

        public GameObject projectile;

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

        /// <summary>
        /// On start, select enemy behavior based on value fireMode
        /// </summary>
        void Start()
        {
            Reset();
            ResetAttackPattern();
            StartCoroutine(SelectBehavior(0));
        }

        /// <summary>
        /// tracks player and shoots projectiles in that direction, while being slightly
        /// swayed via the spread value set by user. If player is not found automatically
        /// finds player, otherwise do nothing.
        /// </summary>
        private void Update()
        {
            if (player != null)
            {
                transform.LookAt(player.transform);

                //if (canShoot)
                //{
                //    StartCoroutine(Shoot());
                //}
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
        /// Fires projectiles in a pattern determined by the firemode at the player.
        /// </summary>
        private IEnumerator Shoot()
        {
            canShoot = false;

            for (int point = 0; point < 81; ++point)
            {
                if (projectileArray[point] == true)
                {
                    GameObject projectile = objectPoolManager.Spawn(projectileTypeIdentifier, shootPoint[point].position,
                                                       shootPoint[point].rotation);

                    randX = Random.Range(-spread, spread);
                    randY = Random.Range(-spread, spread);
                    randZ = Random.Range(-spread, spread);

                    projectile.GetComponent<Transform>().Rotate(randX, randY, randZ);
                    projectile.GetComponent<Rigidbody>().velocity =
                        projectile.transform.forward * projectileSpeed;
                }
            }
            yield return new WaitForSeconds(fireRate);
            canShoot = true;
        }

        private IEnumerator RotateProjectile(Transform pivot)
        {
            while (true)
            {
                pivot.Rotate(0, 0, 1);
                yield return new WaitForSeconds(0.01f);
            }
        }

        private IEnumerator SelectBehavior(int behavior)
        {
            //Debug.Log(behavior);
            switch (behavior)
            {
                //standard pattern
                case 0:
                    yield return new WaitForSeconds(1);
                    SetAttackPattern(AttackPattern.Four);
                    for(int i = 0; i < 20; ++i)
                    {
                        StartCoroutine(Shoot());
                        yield return new WaitForSeconds(fireRate);
                    }
                    StartCoroutine(SelectBehavior(1));
                    break;

                case 1:
                    yield return new WaitForSeconds(1);
                    for (int i = 0; i < 15; ++i)
                    {
                        SetAttackPattern(AttackPattern.Two);
                        StartCoroutine(Shoot());
                        yield return new WaitForSeconds(0.025f);
                        SetAttackPattern(AttackPattern.Three);
                        StartCoroutine(Shoot());
                        yield return new WaitForSeconds(0.025f);
                        SetAttackPattern(AttackPattern.Four);
                        StartCoroutine(Shoot());
                        yield return new WaitForSeconds(0.025f);
                        SetAttackPattern(AttackPattern.Three);
                        StartCoroutine(Shoot());
                        yield return new WaitForSeconds(0.025f);
                        SetAttackPattern(AttackPattern.Two);
                        StartCoroutine(Shoot());
                        yield return new WaitForSeconds(fireRate);
                    }
                    StartCoroutine(SelectBehavior(2));
                    break;

                case 2:
                    yield return new WaitForSeconds(1);
                    ResetAttackPattern();
                    SetAttackPattern(AttackPattern.One);
                    StartCoroutine(RotateProjectile(shootPivot));
                    for (int i = 0; i < 200; ++i)
                    {
                        StartCoroutine(Shoot());
                        yield return new WaitForSeconds(0.15f);
                    }
                    StartCoroutine(SelectBehavior(0));
                    break;

            }
            yield return null;
        }

        /// <summary>
        /// Function that determines the enemy's projectile, firerate,
        /// spread, and projectile speed.
        /// </summary>
        /// <param name="mode">Current Enemy Firemode</param>
        public override void SetAttackPattern(AttackPattern attackPattern)
        {
            int[] myPoints = new int[] { };
            switch ((int)attackPattern)
            {
                case 0:
                    fireRate = 0.1f;
                    projectileSpeed = 1.5f;
                    spread = 0;
                    StartCoroutine(RotateProjectile(shootPivot));
                    for (int i = 0; i < 9; ++i)
                    {
                        projectileArray[i] = true;
                        projectileArray[i * 9] = true;
                        projectileArray[8 + (9*i)] = true;
                    }
                    for (int i = 73; i < 81; ++i) projectileArray[i] = true;
                    return;

                case 1:
                    fireRate = 0.8f;
                    projectileSpeed = 2.5f;
                    spread = 0;

                    myPoints = new int[] {
                        30, 31, 32,
                        40,
                        48, 49, 50 };

                    ActivateShootPoints(myPoints, myPoints.Length);
                    return;

                case 2:
                    fireRate = 0.8f;
                    projectileSpeed = 2.5f;
                    spread = 0;

                    myPoints = new int[] {
                        13,
                        20, 21, 23, 24,
                        29, 33,
                        37, 39, 41, 43,
                        47, 51,
                        56, 57, 59, 60,
                        67 };

                    ActivateShootPoints(myPoints, myPoints.Length);
                    return;

                case 3:
                    fireRate = 0.6f;
                    projectileSpeed = 2.5f;
                    spread = 0;

                    myPoints = new int[] {
                        4,
                        11, 12, 14, 15,
                        19, 20, 22, 24, 25,
                        28, 34,
                        36, 38, 42, 44,
                        46, 52,
                        55, 56, 58, 60, 61,
                        65, 66, 68, 69,
                        76 };

                    ActivateShootPoints(myPoints, myPoints.Length);
                    return;
            }
            //Debug.Log(myPoints.Length);
        }

        private void ResetAttackPattern()
        {
            StopCoroutine(RotateProjectile(shootPivot));
            shootPivot.rotation = transform.rotation;
            for(int i = 0; i < 81; ++i)
            {
                projectileArray[i] = false;
            }
            //Debug.Log("Reset!");
        }

        private void ActivateShootPoints(int[] points, int length)
        {
            ResetAttackPattern();
            for (int i = 0; i < length; ++i)
            {
                projectileArray[points[i]] = true;
            }
        }
        /// <summary>
        /// Resets attributes to this enemy's defaults from enemyAttributes.
        /// </summary>
        protected override void Reset()
        {
            //// reset materials
            //for (int i = 0; i < renderers.Count; ++i)
            //{
            //    renderers.ElementAt(i).material = materials.ElementAt(i);
            //}

            //hitFlash = null;
            //shaking = false;
            //canShoot = true;

            projectileTypeIdentifier = objectPoolManager.GetTypeIdentifier(projectile);
            //    enemyAttributes.EnemyProjectileTypeIdentifiers[TypeIdentifier];
            maxHealth = 700;
            Health = maxHealth;
            //fireRate = enemyAttributes.enemyFireRate[TypeIdentifier];
            //projectileSpeed = enemyAttributes.projectileSpeed;
            //spread = enemyAttributes.enemySpread[TypeIdentifier];
            //pointValue = enemyAttributes.enemyScoreValues[TypeIdentifier];
            //selfDestructTime = enemyAttributes.enemySelfDestructTimes[TypeIdentifier];
            //spawnEmitter = enemyAttributes.enemySpawnEmitters[TypeIdentifier];
            //deathEmitter = enemyAttributes.enemyDeathEmitters[TypeIdentifier];

            //if (!isInitialized)
            //{
            //    isInitialized = true;

            //    GameObject spawnEmitterObject = Instantiate(spawnEmitter,
            //                                                transform.position,
            //                                                transform.rotation, transform);
            //    spawnEmitterSystem = spawnEmitterObject.GetComponent<ParticleSystems>();

            //    deathEmitterTypeIdentifier = objectPoolManager.GetTypeIdentifier(deathEmitter);
            //}
        }
    }
}