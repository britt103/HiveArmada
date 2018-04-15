//=============================================================================
// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// 
// Miguel Gotao
// 2264941
// gotao100@mail.chapman.edu
// 
// CPSC-440-01
// Group Project
// 
// 
// 
//=============================================================================

using System;
using Hive.Armada.Game;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Hive.Armada.Player;
using SubjectNerd.Utilities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Hive.Armada.Enemies
{
    public enum BossStates
    {
        Patrol,

        Idle,

        Combat,

        TransitionToCombat,

        TransitionFromCombat,

        Intro,

        Death
    }

    public class NewBoss : Enemy
    {
        private enum BossPosition
        {
            PatrolCenter,

            PatrolLeft,

            PatrolRight,

            Combat,

            Intro
        }

        private BossManager bossManager;

        [Reorderable("Wave", false)]
        public int[] scoreValues;

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
        private bool[] projectileArray;

        /// <summary>
        /// Structure responsible for what behavior the boss is going to use
        /// </summary>
        private int[] behaviorArray;

        /// <summary>
        /// Positions from which bullets are initially shot from
        /// Positions are arranged in a 9x9 grid
        /// </summary>
        public Transform[] shootPoints;

        /// <summary>
        /// Transform of the shoot point, to be used for altering attack patterns
        /// </summary>
        public Transform shootPivot;

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

        [Reorderable("Eye", false)]
        public GameObject[] eyes;

        private Renderer[] eyeRenderers;

        public Material eyeIntactMaterial;

        public Material eyeDestroyedMaterial;

        public int Lives { get; private set; }

        private int currentEye;

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

        public AudioClip[] introClips;

        [Header("Hover")]
        public bool hoverEnabled;

        public float hoverDistance;

        public float hoverTime;

        private float hoverPercent;

        private Vector3 hoverStart;

        private Vector3 hoverEnd;

        private BossPosition currentPosition;

        public bool IsAlive { get; private set; }

        public bool IsHovering { get; private set; }

        public bool IsSpeaking { get; private set; }

        public bool LookAtTarget { get; private set; }

        public bool PatrolIsLeft { get; private set; }

        public bool PatrolIsInward { get; private set; }

        public BossStates CurrentState { get; private set; }

        public BossStates NextState { get; private set; }

        private Coroutine hoverCoroutine;

        private Coroutine introSpeakCoroutine;

        private Coroutine patrolWaitCoroutine;

        private Coroutine idleWaitCoroutine;

        private Coroutine combatWaitCoroutine;

        /// <summary>
        /// This is the combat coroutine. SelectBehavior()
        /// loops infinitely as long as the boss is alive.
        /// </summary>
        private Coroutine selectBehaviorCoroutine;

        private Coroutine combatRotationCoroutine;

        private Coroutine transitionToCombatCoroutine;

        private Coroutine transitionFromCombatCoroutine;

        public GameObject[] patterns;

        private short[] patternIds;

        private short patternId = -2;

        [Header("Score")]
        [Reorderable("Point", false)]
        public Transform[] rightScorePoints;

        [Reorderable("Point", false)]
        public Transform[] leftScorePoints;

        [Reorderable("Point", false)]
        public Transform[] rightCScorePoints;

        [Reorderable("Point", false)]
        public Transform[] leftCScorePoints;

        [Reorderable("Point", false)]
        public Transform[] topScorePoints;

        [Reorderable("Point", false)]
        public Transform[] mainScorePoints;

        private List<Transform> scorePoints;

        private Dictionary<int, Transform[]> scorePointsDictionary;

        private static System.Random random;

        /// <summary>
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            bossManager = reference.bossManager;
            lookTarget = GameObject.Find("Ship Look Target");
            source = GameObject.Find("Boss Audio Source").GetComponent<AudioSource>();

            Random.InitState((int) DateTime.Now.Ticks);
            SetLookTarget(lookTarget);
            LookAtTarget = true;
            currentPosition = BossPosition.Intro;

            currentEye = 0;

            Lives = eyes.Length;

            if (Lives <= 0)
            {
                Debug.LogError(GetType().Name + " - Boss 'eyes' are not setup. 'Lives' = " + Lives);
            }

            renderers.Clear();
            materials.Clear();
            patternIds = new short[patterns.Length];
            for (int i = 0; i < patterns.Length; ++i)
            {
                patternIds[i] = objectPoolManager.GetTypeIdentifier(patterns[i]);
            }

            //for (int i = 0; i < eyes.Length; ++i)
            //{
            //    eyes[i].GetComponent<Renderer>().material = eyeIntactMaterial;
            //}

            foreach (Renderer r in gameObject.GetComponentsInChildren<Renderer>())
            {
                if (r.gameObject.CompareTag("Emitter") || r.transform.parent.CompareTag("Emitter"))
                {
                    continue;
                }

                renderers.Add(r);
                materials.Add(r.material);
            }

            if (shootPoints.Length <= 0)
            {
                Debug.LogError(GetType().Name + " - ");
            }

            projectileArray = new bool[shootPoints.Length];

            scorePoints = new List<Transform>();
            scorePoints.AddRange(rightScorePoints);
            scorePoints.AddRange(leftScorePoints);
            scorePoints.AddRange(rightCScorePoints);
            scorePoints.AddRange(leftCScorePoints);
            scorePoints.AddRange(topScorePoints);
            scorePoints.AddRange(mainScorePoints);

            Shuffle(scorePoints);

            scorePointsDictionary = new Dictionary<int, Transform[]>
                                    {
                                        {0, topScorePoints}, {1, leftCScorePoints},
                                        {2, rightCScorePoints}, {3, leftScorePoints},
                                        {4, rightScorePoints}
                                    };
        }

        /// <summary>
        /// </summary>
        private void Update()
        {
            if (LookAtTarget)
            {
                if (lookTarget == null)
                {
                    lookTarget = GameObject.Find("Ship Look Target");
                }

                Quaternion to =
                    Quaternion.LookRotation(lookTarget.transform.position - transform.position);

                transform.rotation =
                    Quaternion.Slerp(transform.rotation, to, lookStrength * Time.deltaTime);
            }

            if (shaking)
            {
                iTween.ShakePosition(gameObject, new Vector3(0.05f, 0.05f, 0.05f), 0.2f);
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

        private void OnDialogueComplete()
        {
            IsSpeaking = false;
        }

        /// <summary>
        /// Sets the NextState for the boss and starts the
        /// Coroutine for the corresponding behavior.
        /// </summary>
        /// <param name="newState"> The next state the boss should transition to </param>
        public void TransitionState(BossStates newState)
        {
            if (CurrentState != BossStates.Combat)
            {
                NextState = newState;
            }
            else
            {
                if (newState != BossStates.TransitionFromCombat && newState != BossStates.Death)
                {
                    if (newState == BossStates.Combat)
                    {
                        Debug.LogWarning(GetType().Name + " - Trying to transition boss to" +
                                         " \"Combat\" state while already in \"Combat\" state.");
                    }
                    else
                    {
                        string badState;

                        switch (newState)
                        {
                            case BossStates.Patrol:
                                badState = "Patrol";
                                break;
                            case BossStates.Idle:
                                badState = "Idle";
                                break;
                            case BossStates.TransitionToCombat:
                                badState = "TransitionToCombat";
                                break;
                            case BossStates.Intro:
                                badState = "Intro";
                                break;
                            default:
                                badState = ((int) newState).ToString();
                                break;
                        }

                        Debug.LogWarning(GetType().Name + " - Trying to transition boss to \"" +
                                         badState + "\" state while in \"Combat\" state.");
                    }

                    return;
                }

                NextState = newState;
            }

            switch (newState)
            {
                case BossStates.Patrol:
                    if (patrolWaitCoroutine == null)
                    {
                        patrolWaitCoroutine = StartCoroutine(PatrolWait());
                    }
                    break;
                case BossStates.Idle:
                    if (idleWaitCoroutine == null)
                    {
                        idleWaitCoroutine = StartCoroutine(IdleWait());
                    }
                    break;
                case BossStates.Combat:
                    if (combatWaitCoroutine == null)
                    {
                        combatWaitCoroutine = StartCoroutine(CombatWait());
                    }
                    else
                    {
                        Debug.LogError(GetType().Name + " - \"combatWaitCoroutine\" is not null.");
                    }
                    break;
                case BossStates.TransitionToCombat:
                    if (transitionToCombatCoroutine == null)
                    {
                        transitionToCombatCoroutine = StartCoroutine(TransitionToCombat());
                    }
                    else
                    {
                        Debug.LogError(GetType().Name +
                                       " - \"transitionToCombatCoroutine\" is not null.");
                    }
                    break;
                case BossStates.TransitionFromCombat:
                    if (transitionFromCombatCoroutine == null)
                    {
                        transitionFromCombatCoroutine = StartCoroutine(TransitionFromCombat());
                    }
                    else
                    {
                        Debug.LogError(GetType().Name +
                                       " - \"transitionFromCombatCoroutine\" is not null.");
                    }
                    break;
                case BossStates.Intro:

                    // play intro audio
                    if (source != null)
                    {
                        if (introClips.Length > 0)
                        {
                            IsSpeaking = true;
                            if (introSpeakCoroutine == null)
                            {
                                introSpeakCoroutine = StartCoroutine(IntroSpeak());
                            }
                            else
                            {
                                Debug.LogError(GetType().Name +
                                               " - \"introSpeakCoroutine\" is not null.");
                            }
                        }
                        else
                        {
                            Debug.LogError(GetType().Name + " - \"introClips\" is empty.");
                        }
                    }
                    else
                    {
                        Debug.LogError(GetType().Name + " - \"source\" is not set.");
                    }
                    break;
                case BossStates.Death:
                    break;
            }
        }

        #region Patrol

        /// <summary>
        /// Wait until the previous pathing is complete.
        /// </summary>
        private IEnumerator PatrolWait()
        {
            if (!IsHovering)
            {
                yield return new WaitWhile(() => !PathingComplete);
            }
            else
            {
                hoverEnabled = false;
                yield return new WaitWhile(() => !IsHovering);
            }

            PathingComplete = false;

            if (NextState == BossStates.Patrol)
            {
                CurrentState = NextState;

                StartPatrol();
            }
            else
            {
                PathingComplete = true;
            }

            patrolWaitCoroutine = null;
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

                iTweenPath[] paths;
                if (currentPosition == BossPosition.PatrolLeft)
                {
                    paths = bossManager.bossPaths["patrolLC"];
                    PatrolIsLeft = false;
                    PatrolIsInward = false;
                    currentPosition = BossPosition.PatrolCenter;
                }
                else if (currentPosition == BossPosition.PatrolRight)
                {
                    paths = bossManager.bossPaths["patrolRC"];
                    PatrolIsLeft = true;
                    PatrolIsInward = false;
                    currentPosition = BossPosition.PatrolCenter;
                }
                else
                {
                    paths = bossManager.bossPaths["patrolCL"];
                    PatrolIsLeft = true;
                    PatrolIsInward = false;
                    Debug.LogError(GetType().Name +
                                   " - boss is not in center, left, or right patrol positions.");
                }

                moveHash.Add(
                    "path", iTweenPath.GetPath(paths[Random.Range(0, paths.Length)].pathName));

                iTween.MoveTo(gameObject, moveHash);
            }
        }

        /// <summary>
        /// </summary>
        private void OnPatrolComplete()
        {
            if (CurrentState != BossStates.Patrol || NextState != BossStates.Patrol)
            {
                PathingComplete = true;
                return;
            }

            PickPath();
        }

        /// <summary>
        /// </summary>
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

        #endregion

        #region Idle

        /// <summary>
        /// </summary>
        private IEnumerator IdleWait()
        {
            yield return new WaitWhile(() => !PathingComplete);

            PathingComplete = false;

            if (NextState == BossStates.Idle)
            {
                CurrentState = NextState;
                StartHover();
            }

            idleWaitCoroutine = null;
        }

        /// <summary>
        /// Sets the hover endpoints and then begins the hover coroutine.
        /// </summary>
        private void StartHover()
        {
            if (hoverCoroutine == null)
            {
                SetHover();
                hoverEnabled = true;
                IsHovering = true;
                hoverCoroutine = StartCoroutine(Hover(hoverStart, hoverEnd));
            }
            else
            {
                Debug.LogError(GetType().Name + " - \"hoverCoroutine\" is not null.");
            }
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
        /// Hovers the boss from 'start' to 'end'
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
                IsHovering = false;
                hoverCoroutine = null;
            }
        }

        #endregion

        /// <summary>
        /// The boss says its opening lines and then moves to patrol.
        /// </summary>
        private IEnumerator IntroSpeak()
        {
            CurrentState = NextState;

            eyeRenderers = new Renderer[eyes.Length];

            for (int i = 0; i < eyes.Length; ++i)
            {
                Renderer eyeRenderer = eyes[i].GetComponent<Renderer>();

                if (eyeRenderer != null)
                {
                    eyeRenderers[i] = eyeRenderer;
                    eyeRenderers[i].material = eyeIntactMaterial;
                }
                else
                {
                    Debug.LogError(i + " null");
                }
            }

            //StartHover();
            //yield return new WaitForSeconds(30.0f);
            //hoverEnabled = false;

            yield return null;

            // pass array of clips to DialoguePlayer

            //foreach (AudioClip clip in introClips)
            //{
            //    source.PlayOneShot(clip);

            //    yield return new WaitWhile(() => source.isPlaying);

            //    yield return new WaitForSeconds(0.8f);
            //}

            IsSpeaking = false;

            reference.shipPickup.SetActive(true);
            reference.tooltips.SpawnGrabShip();

            //yield return new WaitWhile(() => IsHovering);

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
            introSpeakCoroutine = null;
        }

        #region CombatTransitions

        /// <summary>
        /// Waits until the previous state is finished, then transitions the boss to combat.
        /// </summary>
        private IEnumerator TransitionToCombat()
        {
            if (!IsHovering)
            {
                yield return new WaitWhile(() => !PathingComplete);
            }
            else
            {
                hoverEnabled = false;
                yield return new WaitWhile(() => !IsHovering);
            }

            PathingComplete = false;

            if (NextState == BossStates.TransitionToCombat)
            {
                CurrentState = NextState;

                Hashtable moveHash = new Hashtable
                                     {
                                         {"easetype", iTween.EaseType.easeInOutSine},
                                         {"time", 4.0f},
                                         {"onComplete", "OnPathingComplete"},
                                         {"onCompleteTarget", gameObject}
                                     };

                iTweenPath[] paths;
                switch (currentPosition)
                {
                    case BossPosition.PatrolCenter:
                        paths = bossManager.bossPaths["combatFromC"];
                        break;
                    case BossPosition.PatrolLeft:
                        paths = bossManager.bossPaths["combatFromL"];
                        break;
                    case BossPosition.PatrolRight:
                        paths = bossManager.bossPaths["combatFromR"];
                        break;
                    default:
                        paths = bossManager.bossPaths["combatFromC"];
                        break;
                }

                moveHash.Add(
                    "path", iTweenPath.GetPath(paths[Random.Range(0, paths.Length)].pathName));

                iTween.MoveTo(gameObject, moveHash);
                currentPosition = BossPosition.Combat;

                TransitionState(BossStates.Combat);
            }

            transitionToCombatCoroutine = null;
        }

        /// <summary>
        /// Waits until the previous state is finished, then
        /// transitions the boss from the combat point.
        /// </summary>
        private IEnumerator TransitionFromCombat()
        {
            yield return new WaitWhile(() => NextState == BossStates.Combat);

            if (IsHovering)
            {
                hoverEnabled = false;
                yield return new WaitWhile(() => !IsHovering);
            }

            PathingComplete = false;

            if (NextState == BossStates.TransitionFromCombat)
            {
                CurrentState = NextState;

                Hashtable moveHash = new Hashtable
                                     {
                                         {"easetype", iTween.EaseType.easeInOutSine},
                                         {"time", 4.0f},
                                         {"onComplete", "OnPathingComplete"},
                                         {"onCompleteTarget", gameObject}
                                     };

                iTweenPath[] paths = bossManager.bossPaths["combatToC"];

                moveHash.Add(
                    "path", iTweenPath.GetPath(paths[Random.Range(0, paths.Length)].pathName));

                iTween.MoveTo(gameObject, moveHash);
                currentPosition = BossPosition.PatrolCenter;

                TransitionState(BossStates.Patrol);
            }

            transitionFromCombatCoroutine = null;
        }

        #endregion

        /// <summary>
        /// </summary>
        private IEnumerator CombatWait()
        {
            if (!IsHovering)
            {
                yield return new WaitWhile(() => !PathingComplete);
            }
            else
            {
                hoverEnabled = false;
                yield return new WaitWhile(() => !IsHovering);
            }

            PathingComplete = false;

            if (NextState == BossStates.Combat)
            {
                CurrentState = NextState;

                maxHealth = health[wave];
                Health = maxHealth;

                IsAlive = true;
                StartHover();

                StartCoroutine(StartBehavior());
            }

            combatWaitCoroutine = null;
        }

        #region Shooting

        /// <summary>
        /// Fires projectiles in a pattern determined by the firemode at the player.
        /// </summary>
        private IEnumerator Shoot()
        {
            canShoot = false;

            //for (int point = 0; point < 81; ++point)
            //{
            //    if (projectileArray[point])
            //    {
            //        GameObject projectile = objectPoolManager.Spawn(
            //            gameObject, projectileTypeIdentifier, shootPoints[point].position,
            //            shootPoints[point].rotation);

            //        projectile.GetComponent<Projectile>()
            //                  .SetColors(projectileAlbedoColor, projectileEmissionColor);
            //        projectile.GetComponent<Transform>().Rotate(Random.Range(-spread, spread),
            //                                                    Random.Range(-spread, spread),
            //                                                    Random.Range(-spread, spread));
            //        Projectile projectileScript = projectile.GetComponent<Projectile>();
            //        projectileScript.Launch(0);
            //    }
            //}

            GameObject projectile = objectPoolManager.Spawn(
                gameObject, patternId, shootPivot.position,
                shootPivot.rotation);

            ProjectilePattern projectileScript = projectile.GetComponent<ProjectilePattern>();
            projectileScript.Launch(0);

            yield return new WaitForSeconds(fireRate);

            //shootCoroutine = null;

            if (CurrentState == BossStates.Combat)
            {
                canShoot = true;
            }
        }

        private void ShootProjectiles()
        {
            GameObject projectile = objectPoolManager.Spawn(
                gameObject, patternId, shootPivot.position,
                shootPivot.rotation);

            ProjectilePattern projectileScript = projectile.GetComponent<ProjectilePattern>();
            if (projectileScript != null)
            {
                projectileScript.Launch(0);
            }
            else
            {
                Debug.LogError(projectile.name + " - " + projectile.GetInstanceID());
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="pivot"> </param>
        private IEnumerator RotateProjectile(Transform pivot)
        {
            while (canRotate)
            {
                pivot.Rotate(0, 0, 1.5f);
                yield return new WaitForSeconds(0.01f);
            }
        }

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public IEnumerator StartBehavior()
        {
            if (selectBehaviorCoroutine == null)
            {
                switch (wave)
                {
                    case 0:
                        yield return new WaitForSeconds(0.1f);

                        selectBehaviorCoroutine = StartCoroutine(SelectBehavior(0));
                        break;

                    case 1:
                        yield return new WaitForSeconds(0.1f);

                        selectBehaviorCoroutine = StartCoroutine(SelectBehavior(3));
                        break;

                    case 2:
                        yield return new WaitForSeconds(0.1f);

                        selectBehaviorCoroutine = StartCoroutine(SelectBehavior(4));
                        break;

                    case 3:
                        yield return new WaitForSeconds(0.1f);

                        selectBehaviorCoroutine = StartCoroutine(SelectBehavior(2));
                        break;

                    case 4:
                        yield return new WaitForSeconds(0.1f);

                        selectBehaviorCoroutine = StartCoroutine(SelectBehavior(1));
                        break;
                }
            }
            else
            {
                Debug.LogError(GetType().Name + " - selectBehaviorCoroutine is not null.");
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="behavior"> </param>
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

                    break;
                //ball pattern
                case 1:
                    yield return new WaitForSeconds(1);

                    for (int i = 0; i < 10; ++i)
                    {
                        SetAttackPattern(AttackPattern.Two);
                        StartCoroutine(Shoot());
                        yield return new WaitForSeconds(0.05f);

                        SetAttackPattern(AttackPattern.Three);
                        StartCoroutine(Shoot());
                        yield return new WaitForSeconds(0.05f);

                        SetAttackPattern(AttackPattern.Four);
                        StartCoroutine(Shoot());
                        yield return new WaitForSeconds(0.05f);

                        SetAttackPattern(AttackPattern.Three);
                        StartCoroutine(Shoot());
                        yield return new WaitForSeconds(0.05f);

                        SetAttackPattern(AttackPattern.Two);
                        StartCoroutine(Shoot());
                        yield return new WaitForSeconds(2);
                    }

                    break;
                //tunnel pattern
                case 2:
                    yield return new WaitForSeconds(1);

                    ResetAttackPattern();
                    canRotate = true;
                    StartCoroutine(RotateProjectile(shootPivot));
                    for (int i = 0; i < 5; ++i)
                    {
                        SetAttackPattern(AttackPattern.One);
                        for (int j = 0; j < 5; ++j)
                        {
                            StartCoroutine(Shoot());
                            yield return new WaitForSeconds(0.15f);
                        }

                        SetAttackPattern(AttackPattern.Ten);
                        yield return new WaitForSeconds(0.3f);

                        for (int j = 0; j < 5; ++j)
                        {
                            StartCoroutine(Shoot());
                            yield return new WaitForSeconds(0.15f);
                        }

                        yield return new WaitForSeconds(1);
                    }

                    break;
                //spread pattern
                case 3:
                    yield return new WaitForSeconds(1);

                    SetAttackPattern(AttackPattern.Five);
                    for (int i = 0; i < 10; ++i)
                    {
                        for (int j = 0; j < 3; ++j)
                        {
                            for (int k = 0; k < 3; ++k)
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
                    canRotate = true;
                    StartCoroutine(RotateProjectile(shootPivot));
                    for (int i = 0; i < 25; ++i)
                    {
                        StartCoroutine(Shoot());
                        yield return new WaitForSeconds(fireRate);
                    }

                    break;
                //spiral pattern
                case 5:
                    yield return new WaitForSeconds(1);

                    SetAttackPattern(AttackPattern.Seven);
                    canRotate = true;
                    StartCoroutine(RotateProjectile(shootPivot));
                    for (int i = 0; i < 75; ++i)
                    {
                        StartCoroutine(Shoot());
                        yield return new WaitForSeconds(0.15f);
                    }

                    break;
                //oscilatting halves
                case 6:
                    yield return new WaitForSeconds(1);

                    for (int i = 0; i < 10; ++i)
                    {
                        SetAttackPattern(AttackPattern.Eight);
                        canRotate = true;

                        //StartCoroutine(RotateProjectile(shootPivot, 1.5f));
                        for (int j = 0; j < 5; ++j)
                        {
                            StartCoroutine(Shoot());
                            yield return new WaitForSeconds(0.15f);
                        }

                        SetAttackPattern(AttackPattern.Nine);
                        canRotate = true;

                        //yield return new WaitForSeconds(1);
                        //StartCoroutine(RotateProjectile(shootPivot, -1.5f));
                        for (int j = 0; j < 5; ++j)
                        {
                            StartCoroutine(Shoot());
                            yield return new WaitForSeconds(0.15f);
                        }

                        yield return new WaitForSeconds(1.5f);
                    }
                    break;
            }

            int me = Random.Range(0, 7);
            selectBehaviorCoroutine = StartCoroutine(SelectBehavior(me));
            Debug.Log("Switched to " + me);
            yield return null;
        }

        /// <summary>
        /// Function that determines the enemy's projectile, firerate,
        /// spread, and projectile speed.
        /// </summary>
        /// <param name="attackPattern"> The new attack pattern </param>
        public override void SetAttackPattern(AttackPattern attackPattern)
        {
            ////int[] myPoints = { };
            //switch ((int) attackPattern)
            //{
            //    case 0:
            //        fireRate = 0.8f;
            //        projectileSpeed = 5;
            //        spread = 0;

            //        //myPoints = new[]
            //        //           {
            //        //               10, 11, 12, 13, 14, 15, 16,
            //        //               19, 25,
            //        //               28, 34,
            //        //               37, 43,
            //        //               46, 52,
            //        //               55, 61,
            //        //               64, 65, 66, 67, 68, 69, 70
            //        //           };

            //        //ActivateShootPoints(myPoints, myPoints.Length);
            //        break;
            //    case 1:
            //        fireRate = 0.8f;
            //        projectileSpeed = 5;
            //        spread = 0;

            //        //myPoints = new[]
            //        //           {
            //        //               30, 31, 32,
            //        //               40,
            //        //               48, 49, 50
            //        //           };

            //        //ActivateShootPoints(myPoints, myPoints.Length);
            //        break;
            //    case 2:
            //        fireRate = 0.8f;
            //        projectileSpeed = 5;
            //        spread = 0;

            //        //myPoints = new[]
            //        //           {
            //        //               13,
            //        //               20, 21, 23, 24,
            //        //               29, 33,
            //        //               37, 39, 41, 43,
            //        //               47, 51,
            //        //               56, 57, 59, 60,
            //        //               67
            //        //           };

            //        //ActivateShootPoints(myPoints, myPoints.Length);
            //        break;
            //    case 3:
            //        fireRate = 0.6f;
            //        projectileSpeed = 5;
            //        spread = 0;

            //        //myPoints = new[]
            //        //           {
            //        //               4,
            //        //               11, 12, 14, 15,
            //        //               19, 20, 22, 24, 25,
            //        //               28, 34,
            //        //               36, 44,
            //        //               46, 52,
            //        //               55, 56, 58, 60, 61,
            //        //               65, 66, 68, 69,
            //        //               76
            //        //           };

            //        //ActivateShootPoints(myPoints, myPoints.Length);
            //        break;
            //    case 4:
            //        fireRate = 2f;
            //        projectileSpeed = 5;
            //        spread = 1;

            //        //myPoints = new[]
            //        //{
            //        //    30, 32,
            //        //    40,
            //        //    48, 50
            //        //};

            //        //ActivateShootPoints(myPoints, myPoints.Length);
            //        break;
            //    case 5:
            //        fireRate = 1;
            //        projectileSpeed = 5;
            //        spread = 0;

            //        //myPoints = new[]
            //        //{
            //        //    4,
            //        //    12, 14,
            //        //    21, 23,
            //        //    28, 29, 31, 33, 34,
            //        //    36, 40, 44,
            //        //    46, 47, 49, 51, 52,
            //        //    57, 59,
            //        //    66, 68,
            //        //    76
            //        //};

            //        //ActivateShootPoints(myPoints, myPoints.Length);
            //        break;
            //    case 6:
            //        fireRate = 0.1f;
            //        projectileSpeed = 5;
            //        spread = 0;

            //        //myPoints = new[]
            //        //{
            //        //    4,
            //        //    22,
            //        //    36, 44,
            //        //    76
            //        //};

            //        //ActivateShootPoints(myPoints, myPoints.Length);
            //        break;
            //    case 7:
            //        fireRate = 0.1f;
            //        projectileSpeed = 5;
            //        spread = 0;

            //        //myPoints = new[]
            //        //{
            //        //    14,
            //        //    20, 21, 23, 24,
            //        //    30, 31, 32,
            //        //    38, 39, 41, 42
            //        //};

            //        //ActivateShootPoints(myPoints, myPoints.Length);
            //        break;
            //    case 8:
            //        fireRate = 0.1f;
            //        projectileSpeed = 5;
            //        spread = 0;

            //        //myPoints = new[]
            //        //{
            //        //    38, 39, 41, 42,
            //        //    48, 49, 50,
            //        //    56, 57, 59, 60,
            //        //    67
            //        //};

            //        //ActivateShootPoints(myPoints, myPoints.Length);
            //        break;
            //    case 9:
            //        fireRate = 0.8f;
            //        projectileSpeed = 5;
            //        spread = 0;

            //        //myPoints = new[]
            //        //           {
            //        //               30, 31, 32,
            //        //               39, 41,
            //        //               48, 49, 50
            //        //           };

            //        //ActivateShootPoints(myPoints, myPoints.Length);
            //        break;
            //}

            patternId = patternIds[(int) attackPattern];

            //Debug.Log(myPoints.Length);
        }

        /// <summary>
        /// </summary>
        private void ResetAttackPattern()
        {
            canRotate = false;
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

        #endregion

        #region BaseEnemyOverrides

        /// <summary>
        /// Used to apply damage to an enemy.
        /// </summary>
        /// <param name="damage"> How much damage this enemy is taking. </param>
        /// <param name="sendFeedback"> If the hit should trigger a haptic feedback pulse </param>
        public override void Hit(int damage, bool sendFeedback)
        {
            if (!CurrentState.Equals(BossStates.Combat))
            {
                return;
            }

            if (sendFeedback)
            {
                if (reference.playerShip != null)
                {
                    reference.playerShip.GetComponent<ShipController>().hand.controller
                             .TriggerHapticPulse(2500);
                }
            }

            if (!IsAlive)
            {
                return;
            }

            Health -= damage;

            if (hitFlash == null)
            {
                hitFlash = StartCoroutine(HitFlash());
            }

            if (Health <= 20)
            {
                shaking = true;
            }

            if (Health <= 0)
            {
                IsAlive = false;
                Kill();
            }
        }

        /// <summary>
        /// Stops the boss from shooting, destroys an eye, adds score, and
        /// transitions it out of combat.
        /// </summary>
        protected override void Kill()
        {
            try
            {
                StopCoroutine(selectBehaviorCoroutine);
            }
            catch (Exception)
            {
                // ignore errors because we never actually set
                // selectBehaviorCoroutine to null before now
            }

            selectBehaviorCoroutine = null;
            canRotate = false;
            shaking = false;
            hoverEnabled = false;
            canShoot = false;
            shootPivot.localRotation = Quaternion.identity;
            --Lives;

            eyeRenderers[Lives].material = eyeDestroyedMaterial;
            eyes[Lives].GetComponent<Renderer>().material = eyeDestroyedMaterial;

            //reference.scoringSystem.ComboIn(pointValue, eyes[Lives].transform);

            StartCoroutine(SpawnScore(Lives));

            //ParticleSystem eyeEmitter = eyes[Lives].transform.GetChild(0).GetComponent<ParticleSystem>();
            //if (eyeEmitter != null)
            //{
            //    eyeEmitter.Clear();
            //    eyeEmitter.Play();
            //}

            hoverEnabled = false;
            TransitionState(Lives > 0 ? BossStates.TransitionFromCombat : BossStates.Death);
            reference.waveManager.BossWaveComplete(wave);
        }

        private IEnumerator SpawnScore(int index)
        {
            if (index > 0)
            {
                Transform[] points = scorePointsDictionary[index];

                foreach (Transform point in points)
                {
                    reference.scoringSystem.ComboIn(scoreValues[0], point);

                    yield return new WaitForSeconds(Random.Range(0.2f, 0.4f));
                }
            }
            else
            {
                foreach (Transform point in scorePoints)
                {
                    reference.scoringSystem.ComboIn(scoreValues[0], point);

                    yield return new WaitForSeconds(Random.Range(0.2f, 0.4f));
                }
            }
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

        #endregion

        public static void Shuffle(List<Transform> list)
        {
            random = new System.Random((int) DateTime.Now.Ticks);

            for (int i = 0; i < 12; ++i)
            {
                int n = list.Count;
                while (n > 1)
                {
                    n--;
                    int k = random.Next(n + 1);
                    Transform value = list[k];
                    list[k] = list[n];
                    list[n] = value;
                }
            }
        }
    }
}