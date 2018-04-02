//=============================================================================
// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-440-01
// Group Project
// 
// 
// 
//=============================================================================

using System;
using Hive.Armada.Game;
using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Hive.Armada.Enemies
{
    public class NewBoss : Enemy
    {
        public enum BossStates
        {
            Patrol,

            Idle,

            Combat,

            TransitionToCombat,

            TransitionFromCombat,

            Intro
        }

        private enum BossPosition
        {
            PatrolCenter,

            PatrolLeft,

            PatrolRight,

            Combat,

            Intro
        }

        [Header("Combat")]
        public GameObject projectilePrefab;

        /// <summary>
        /// Type identifier for object pooling purposes
        /// </summary>
        private short projectileTypeIdentifier;

        public Color projectileAlbedoColor;

        public Color projectileEmissionColor;

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

        [Header("Health")]
        public int[] health;

        public GameObject eyes;

        public Material eyeIntactMaterial;

        public Material eyeDestroyedMaterial;

        /// <summary>
        /// The strength of the boss looking at the player.
        /// </summary>
        [Range(0, 5)]
        public int lookStrength;

        public GameObject lookTarget;

        /// <summary>
        /// Audio source to play boss sounds from
        /// </summary>
        [Header("Audio")]
        public AudioSource source;

        public AudioClip introClip;

        [Header("Hover")]
        public bool hoverEnabled;

        public float hoverDistance;

        public float hoverTime;

        private float hoverPercent;

        private Vector3 hoverStart;

        private Vector3 hoverEnd;

        private Coroutine hoverCoroutine;

        private BossPosition currentPosition;

        public bool IsSpeaking { get; private set; }

        public bool LookAtTarget { get; private set; }

        public BossStates State { get; private set; }

        public GameObject testPlayer;

        public bool PatrolIsLeft { get; private set; }

        public bool PatrolIsInward { get; private set; }

        private BossManager bossManager;

        /// <summary>
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            bossManager = FindObjectOfType<BossManager>();
            testPlayer = GameObject.Find("SphereASDF");
            source = GameObject.Find("Boss Audio Source").GetComponent<AudioSource>();
            Random.InitState((int) DateTime.Now.Ticks);
            SetLookTarget(testPlayer);
            LookAtTarget = true;
            currentPosition = BossPosition.Intro;
        }

        /// <summary>
        /// tracks player and shoots projectiles in that direction, while being slightly
        /// swayed via the spread value set by user. If player is not found automatically
        /// finds player, otherwise do nothing.
        /// </summary>
        private void Update()
        {
            if (LookAtTarget)
            {
                if (lookTarget == null)
                {
                    lookTarget = testPlayer;
                }

                Quaternion to =
                    Quaternion.LookRotation(lookTarget.transform.position - transform.position);

                transform.rotation =
                    Quaternion.Slerp(transform.rotation, to, lookStrength * Time.deltaTime);
            }

            if (State == BossStates.Combat)
            {
                if (canShoot)
                {
                    StartCoroutine(Shoot());
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="newState"> </param>
        public void TransitionState(BossStates newState)
        {
            State = newState;
            switch (newState)
            {
                case BossStates.Patrol:
                    StartCoroutine(PatrolWait());
                    break;
                case BossStates.Idle:
                    break;
                case BossStates.Combat:
                    break;
                case BossStates.TransitionToCombat:
                    break;
                case BossStates.TransitionFromCombat:
                    break;
                case BossStates.Intro:

                    // play intro audio
                    if (source != null)
                    {
                        if (introClip != null)
                        {
                            source.PlayOneShot(introClip);
                        }
                        else
                        {
                            Debug.LogError(GetType().Name + " - \"introClip\" is not set.");
                        }
                    }
                    else
                    {
                        Debug.LogError(GetType().Name + " - \"source\" is not set.");
                    }

                    StartCoroutine(IntroWait());
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// </summary>
        private void StartPatrol()
        {
            if (currentPosition == BossPosition.PatrolCenter)
            {
                PickPath();
            }
            else
            {
                Hashtable moveHash = new Hashtable
                                     {
                                         {"easetype", iTween.EaseType.easeInOutSine},
                                         {"time", 7.0f},
                                         {"onComplete", "OnPatrolComplete"},
                                         {"onCompleteTarget", gameObject}
                                     };

                if (currentPosition == BossPosition.PatrolLeft)
                {
                    moveHash.Add(
                        "path",
                        iTweenPath.GetPath(bossManager
                                               .patrolLCPaths[
                                               Random.Range(0, bossManager.patrolLCPaths.Length)]
                                               .pathName));
                    PatrolIsLeft = false;
                    PatrolIsInward = false;
                    currentPosition = BossPosition.PatrolCenter;
                }
                else if (currentPosition == BossPosition.PatrolRight)
                {
                    moveHash.Add(
                        "path",
                        iTweenPath.GetPath(bossManager
                                               .patrolRCPaths[
                                               Random.Range(0, bossManager.patrolRCPaths.Length)]
                                               .pathName));
                    PatrolIsLeft = true;
                    PatrolIsInward = false;
                    currentPosition = BossPosition.PatrolCenter;
                }
                else
                {
                    Debug.LogError(GetType().Name +
                                   " - boss is not in center, left, or right patrol positions.");
                }
            }
        }

        /// <summary>
        /// </summary>
        private void OnPatrolComplete()
        {
            PathingComplete = true;

            if (State != BossStates.Patrol)
            {
                return;
            }

            PickPath();
        }

        private void PickPath()
        {
            PathingComplete = false;

            Hashtable moveHash = new Hashtable
                                 {
                                     {"easetype", iTween.EaseType.easeInOutSine},
                                     {"time", 7.0f},
                                     {"onComplete", "OnPatrolComplete"},
                                     {"onCompleteTarget", gameObject}
                                 };

            if (PatrolIsLeft)
            {
                if (PatrolIsInward)
                {
                    moveHash.Add(
                        "path",
                        iTweenPath.GetPath(bossManager
                                               .patrolLCPaths[
                                               Random.Range(0, bossManager.patrolLCPaths.Length)]
                                               .pathName));
                    PatrolIsLeft = false;
                    currentPosition = BossPosition.PatrolCenter;
                }
                else
                {
                    moveHash.Add(
                        "path",
                        iTweenPath.GetPath(bossManager
                                               .patrolCLPaths[
                                               Random.Range(0, bossManager.patrolCLPaths.Length)]
                                               .pathName));
                    currentPosition = BossPosition.PatrolLeft;
                }
            }
            else
            {
                if (PatrolIsInward)
                {
                    moveHash.Add(
                        "path",
                        iTweenPath.GetPath(bossManager
                                               .patrolRCPaths[
                                               Random.Range(0, bossManager.patrolRCPaths.Length)]
                                               .pathName));
                    PatrolIsLeft = true;
                    currentPosition = BossPosition.PatrolCenter;
                }
                else
                {
                    moveHash.Add(
                        "path",
                        iTweenPath.GetPath(bossManager
                                               .patrolCRPaths[
                                               Random.Range(0, bossManager.patrolCRPaths.Length)]
                                               .pathName));
                    currentPosition = BossPosition.PatrolRight;
                }
            }

            PatrolIsInward = !PatrolIsInward;

            iTween.MoveTo(gameObject, moveHash);
        }

        /// <summary>
        /// This is run after the enemy has completed its path.
        /// Calls hover function to set hover endpoints
        /// </summary>
        protected override void OnPathingComplete()
        {
            //SetHover();
            //SetLookTarget(reference.playerShip);
            SetLookTarget(testPlayer);
            LookAtTarget = true;

            //if (hoverEnabled)
            //{
            //    StartCoroutine(Hover(hoverStart, hoverEnd));
            //}

            base.OnPathingComplete();
        }

        /// <summary>
        /// Sets the start and end points for the hover effect.
        /// </summary>
        private void SetHover()
        {
            hoverStart = transform.position;
            hoverEnd = new Vector3(transform.position.x,
                                   transform.position.y + hoverDistance,
                                   transform.position.z);
        }

        /// <summary>
        /// </summary>
        /// <param name="start"> The start point </param>
        /// <param name="end"> The end point </param>
        private IEnumerator Hover(Vector3 start, Vector3 end)
        {
            hoverPercent = 0.0f;

            while (hoverPercent <= 1.0f)
            {
                hoverPercent += Time.deltaTime / hoverTime;
                transform.position =
                    Vector3.Lerp(start, end, Mathf.SmoothStep(0.0f, 1.0f, hoverPercent));
                yield return null;
            }

            if (hoverEnabled)
            {
                hoverCoroutine = StartCoroutine(Hover(end, start));
            }
            else
            {
                hoverCoroutine = null;
            }
        }

        /// <summary>
        /// Sets the look target for the boss.
        /// </summary>
        /// <param name="target"> The new look target </param>
        public void SetLookTarget(GameObject target)
        {
            lookTarget = target;
        }

        /// <summary>
        /// Waits for the intro audio to finish, then tells the boss to patrol.
        /// </summary>
        private IEnumerator IntroWait()
        {
            yield return new WaitWhile(() => source.isPlaying);

            Hashtable moveHash = new Hashtable
                                 {
                                     {"easetype", iTween.EaseType.easeInOutSine},
                                     {"time", 4.0f},
                                     {"onComplete", "OnPathingComplete"},
                                     {"onCompleteTarget", gameObject},
                                     {
                                         "path",
                                         iTweenPath.GetPath(bossManager.spawnToPatrolPath.pathName)
                                     }
                                 };
            iTween.MoveTo(gameObject, moveHash);
            currentPosition = BossPosition.PatrolCenter;
            TransitionState(BossStates.Patrol);
        }

        /// <summary>
        /// Wait until the previous pathing is complete.
        /// </summary>
        private IEnumerator PatrolWait()
        {
            yield return new WaitWhile(() => !PathingComplete);

            StartPatrol();
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
                }
            }

            yield return new WaitForSeconds(fireRate);

            canShoot = true;
        }

        /// <summary>
        /// </summary>
        /// <param name="pivot"> </param>
        private IEnumerator RotateProjectile(Transform pivot)
        {
            while (true)
            {
                pivot.Rotate(0, 0, 1.5f);
                yield return new WaitForSeconds(0.01f);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="wave"> </param>
        /// <returns> </returns>
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

        /// <summary>
        /// </summary>
        /// <param name="behavior"> </param>
        /// <returns> </returns>
        private IEnumerator SelectBehavior(int behavior)
        {
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
                    for (int i = 0; i < 10; ++i)
                    {
                        for (int j = 0; j < 3; ++j)
                        {
                            for (int k = 0; k < 2; ++k)
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
                    for (int i = 0; i < 15; ++i)
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
        /// <param name="attackPattern"> The new attack pattern </param>
        public override void SetAttackPattern(AttackPattern attackPattern)
        {
            int[] myPoints;
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
        }

        /// <summary>
        /// </summary>
        private void ResetAttackPattern()
        {
            StopCoroutine(RotateProjectile(shootPivot));
            shootPivot.rotation = transform.rotation;
            for (int i = 0; i < projectileArray.Length; ++i)
            {
                projectileArray[i] = false;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="points"> </param>
        /// <param name="length"> </param>
        private void ActivateShootPoints(int[] points, int length)
        {
            ResetAttackPattern();
            for (int i = 0; i < length; ++i)
            {
                projectileArray[points[i]] = true;
            }
        }

        /// <summary>
        /// </summary>
        protected override void Kill()
        {
            // Do a thing so Boss Manager knows I am defeated.
            // add score, popup on eye that is shutting off
        }

        /// <summary>
        /// </summary>
        protected override void Reset()
        {
            // reset materials
            for (int i = 0; i < renderers.Count; ++i)
            {
                renderers.ElementAt(i).material = materials.ElementAt(i);
            }

            canShoot = false;
            projectileTypeIdentifier = objectPoolManager.GetTypeIdentifier(projectilePrefab);
            maxHealth = health[wave];
            Health = maxHealth;
            PathingComplete = false;
        }
    }
}