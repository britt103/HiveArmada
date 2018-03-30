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
        private short projectileTypeIdentifier;

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

        public bool canActivate;

        private float theta;

        private Vector3 posA;

        private Vector3 posB;

        public float yMax;

        public float movingSpeed;

        /// <summary>
        /// Audio source to play boss sounds from
        /// </summary>
        AudioSource source;

        public AudioClip clip;

        /// <summary>
        /// On start, select enemy behavior based on value fireMode
        /// </summary>
        private void Start()
        {
            //Reset();
            //ResetAttackPattern();
            //StartCoroutine(SelectBehavior(0));
            source = GameObject.Find("Boss Audio Source").GetComponent<AudioSource>();
            source.PlayOneShot(clip);
        }

        /// <summary>
        /// tracks player and shoots projectiles in that direction, while being slightly
        /// swayed via the spread value set by user. If player is not found automatically
        /// finds player, otherwise do nothing.
        /// </summary>
        private void Update()
        {
            if (canActivate)
            {
//                transform.position = Vector3.Lerp(posA, posB, (Mathf.Sin(theta) + 1.0f) / 2.0f);
//
//                theta += movingSpeed * Time.deltaTime;
//
//                if (theta > Mathf.PI * 3 / 2)
//                {
//                    theta -= Mathf.PI * 2;
//                }

                transform.LookAt(player.transform);
            }
        }

//        /// <summary>
//        /// Called by Bosswave when boss finishes pathing.
//        /// </summary>
//        public void FinishedPathing()
//        {
//            pathingFinished = true;
//        }

        /// <summary>
        /// Begins boss firing logic.
        /// </summary>
        public void StartBoss(int currentWave)
        {
            ResetAttackPattern();
            wave = currentWave + 1;
            StartCoroutine(StartBehavior(wave));
            Hover();
        }

        /// <summary>
        /// Stops boss from attacking.
        /// </summary>
        public void PauseBoss()
        {
            PathingComplete = false;
            ResetAttackPattern();
            StopAllCoroutines();
        }

        /// <summary>
        /// Function that creates 2 vector 3's to float up and down with a Sin()
        /// </summary>
        private void Hover()
        {
            posA = new Vector3(transform.position.x,
                               transform.position.y + yMax / 100,
                               transform.position.z);

            posB = new Vector3(transform.position.x,
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

            for (int point = 0; point < 81; ++point)
            {
                if (projectileArray[point])
                {
                    GameObject spawnedProjectile = objectPoolManager.Spawn(
                        gameObject, projectileTypeIdentifier, shootPoint[point].position,
                        shootPoint[point].rotation);

                    randX = Random.Range(-spread, spread);
                    randY = Random.Range(-spread, spread);
                    randZ = Random.Range(-spread, spread);

                    spawnedProjectile.GetComponent<Transform>().Rotate(randX, randY, randZ);
                    Projectile projectileScript = spawnedProjectile.GetComponent<Projectile>();
                    projectileScript.Launch(0);
                    //spawnedProjectile.GetComponent<Rigidbody>().velocity =
                    //    spawnedProjectile.transform.forward * projectileSpeed;
                }
            }

            yield return new WaitForSeconds(fireRate);

            canShoot = true;
        }

        private IEnumerator RotateProjectile(Transform pivot)
        {
            while (true)
            {
                pivot.Rotate(0, 0, 1.5f);
                yield return new WaitForSeconds(0.01f);
            }
        }

        public IEnumerator StartBehavior(int wave)
        {
            switch (wave)
            {
                case 1:
                    yield return new WaitForSeconds(0.1f);
                    StartCoroutine(SelectBehavior(0));
                    break;

                case 2:
                    yield return new WaitForSeconds(0.1f);
                    StartCoroutine(SelectBehavior(3));
                    break;

                case 3:
                    yield return new WaitForSeconds(0.1f);
                    StartCoroutine(SelectBehavior(4));
                    break;

                case 4:
                    yield return new WaitForSeconds(0.1f);
                    StartCoroutine(SelectBehavior(2));
                    break;

                case 5:
                    yield return new WaitForSeconds(0.1f);
                    StartCoroutine(SelectBehavior(1));
                    break;
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
                    for (int i = 0; i < 10; ++i)
                    {
                        StartCoroutine(Shoot());
                        yield return new WaitForSeconds(fireRate);
                    }

                    StartCoroutine(SelectBehavior(1));
                    break;

                    //ball pattern
                case 1:
                    yield return new WaitForSeconds(1);

                    for (int i = 0; i < 10; ++i)
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

                    //tunnel pattern
                case 2:
                    yield return new WaitForSeconds(1);

                    ResetAttackPattern();
                    SetAttackPattern(AttackPattern.One);
                    StartCoroutine(RotateProjectile(shootPivot));
                    for (int i = 0; i < 100; ++i)
                    {
                        StartCoroutine(Shoot());
                        yield return new WaitForSeconds(0.15f);
                    }

                    StartCoroutine(SelectBehavior(0));
                    break;

                //spread pattern
                case 3:
                    yield return new WaitForSeconds(1);
                    SetAttackPattern(AttackPattern.Five);
                    for(int i = 0; i < 10; ++i)
                    {
                        for(int j = 0; j < 3; ++j)
                        {
                            for(int k = 0; k < 2; ++k)
                            {
                                StartCoroutine(Shoot());
                            }
                            yield return new WaitForSeconds(0.2f);
                        }
                        yield return new WaitForSeconds(fireRate);
                    }
                    break;
                
                //clover pattern
                case 4:
                    yield return new WaitForSeconds(1);
                    SetAttackPattern(AttackPattern.Six);
                    StartCoroutine(RotateProjectile(shootPivot));
                    for(int i = 0; i < 15; ++i)
                    {
                        StartCoroutine(Shoot());
                        yield return new WaitForSeconds(fireRate);
                    }
                    break;
            }

            yield return null;
        }

        /// <summary>
        /// Function that determines the enemy's projectile, firerate,
        /// spread, and projectile speed.
        /// </summary>
        /// <param name="mode"> Current Enemy Firemode </param>
        public override void SetAttackPattern(AttackPattern attackPattern)
        {
            int[] myPoints = { };
            switch ((int) attackPattern)
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
                        projectileArray[8 + 9 * i] = true;
                    }
                    for (int i = 73; i < 81; ++i)
                    {
                        projectileArray[i] = true;
                    }

                    return;

                case 1:
                    fireRate = 0.8f;
                    projectileSpeed = 5;
                    spread = 0;

                    myPoints = new[]
                               {
                                   30, 31, 32,
                                   40,
                                   48, 49, 50
                               };

                    ActivateShootPoints(myPoints, myPoints.Length);
                    return;

                case 2:
                    fireRate = 0.8f;
                    projectileSpeed = 5;
                    spread = 0;

                    myPoints = new[]
                               {
                                   13,
                                   20, 21, 23, 24,
                                   29, 33,
                                   37, 39, 41, 43,
                                   47, 51,
                                   56, 57, 59, 60,
                                   67
                               };

                    ActivateShootPoints(myPoints, myPoints.Length);
                    return;

                case 3:
                    fireRate = 0.6f;
                    projectileSpeed = 5;
                    spread = 0;

                    myPoints = new[]
                               {
                                   4,
                                   11, 12, 14, 15,
                                   19, 20, 22, 24, 25,
                                   28, 34,
                                   36, 44,
                                   46, 52,
                                   55, 56, 58, 60, 61,
                                   65, 66, 68, 69,
                                   76
                               };

                    ActivateShootPoints(myPoints, myPoints.Length);
                    return;

                case 4:
                    fireRate = 1f;
                    projectileSpeed = 3;
                    spread = 1;

                    myPoints = new[]
                    {
                        30, 32,
                        40,
                        48, 50
                    };

                    ActivateShootPoints(myPoints, myPoints.Length);
                    return;

                case 5:
                    fireRate = 1;
                    projectileSpeed = 4;
                    spread = 0;

                    myPoints = new[]
                    {
                        4,
                        12, 14,
                        21, 23,
                        28, 29, 31, 33, 34,
                        36, 40, 44,
                        46, 47, 49, 51, 52,
                        57, 59,
                        66, 68,
                        76
                    };

                    ActivateShootPoints(myPoints, myPoints.Length);
                    return;
            }

            //Debug.Log(myPoints.Length);
        }

        private void ResetAttackPattern()
        {
            StopCoroutine(RotateProjectile(shootPivot));
            shootPivot.rotation = transform.rotation;
            for (int i = 0; i < 81; ++i)
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

        protected override void Kill()
        {
            //Do Nothing
        }

        protected override void Reset()
        {
            //Do Nothing
        }

        /// <summary>
        /// Resets attributes to this enemy's defaults from enemyAttributes.
        /// </summary>
        public void BossReset()
        {
            // reset materials
            for (int i = 0; i < renderers.Count; ++i)
            {
                renderers.ElementAt(i).material = materials.ElementAt(i);
            }

            //hitFlash = null;
            //shaking = false;
            //canShoot = true;

            projectileTypeIdentifier = objectPoolManager.GetTypeIdentifier(projectile);
            maxHealth = 1000;
            Health = maxHealth;
            Debug.Log("Boss Health on boss is " + Health);
            PathingComplete = false;
        }
    }
}