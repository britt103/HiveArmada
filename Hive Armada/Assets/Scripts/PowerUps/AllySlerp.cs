//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.champan.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// AllySlerp controls the Ally powerup. The standard Ally fires projectiles
// at the nearest enemy until that enemy's death and self-destructs after a
// certain time. The AllySlerp version of this moves the Ally ship around the 
// player ship in order to avoid firing at the player ship and being fired
// at by the player ship. Currently assigned as Powerup 1.
//
//=============================================================================

using System.Collections;
using UnityEngine;
using Hive.Armada.Game;
using Hive.Armada.Player.Weapons;
using Hive.Armada.Enemies;

namespace Hive.Armada.PowerUps
{
    /// <summary>
    /// Ally powerup with slerp movement.
    /// </summary>
    public class AllySlerp : MonoBehaviour
    {
        /// <summary>
        /// Reference manager that holds all needed references
        /// (e.g. wave manager, game manager, etc.)
        /// </summary>
        private ReferenceManager reference;

        /// <summary>
        /// Distance between the player ship and the ally ship.
        /// </summary>
        public float distance;

        /// <summary>
        /// Time until self-destruct.
        /// </summary>
        public float timeLimit;

        /// <summary>
        /// Time to complete a single slerp movement.
        /// </summary>
        public float slerpTime = 1.0F;

        //public float sphereCastRadius = 1.0F;
        //public float sphereCastMaxDistance = 1.0F;

        //Reference to currently targetted enemy.
        private GameObject currentTarget;

        /// <summary>
        /// Represents progress of current slerp movement in terms of time.
        /// </summary>
        private float slerpTimer;

        /// <summary>
        /// Ratio of slerpFraction / slerpTimer.
        /// </summary>
        private float slerpFraction;

        //private bool targetAcquired;

        public Transform shootPoint;

        /// <summary>
        /// Rocket prefab to use.
        /// </summary>
        public GameObject rocketPrefab;

        /// <summary>
        /// The type ID needed to spawn the rocket.
        /// </summary>
        private short rocketTypeId;

        /// <summary>
        /// Rocket type for setting attributes for launched rockets.
        /// </summary>
        public RocketAttributes.RocketType rocketType;

        /// <summary>
        /// FX instanted in Start().
        /// </summary>
        public GameObject spawnEmitter;

        /// <summary>
        /// Damage dealt to enemies on collision.
        /// </summary>
        public int damage;

        /// <summary>
        /// Number of projectiles fired per second.
        /// </summary>
        public float firerate;

        /// <summary>
        /// State controllering when ally can fire.
        /// </summary>
        private bool canFire = true;

        public AudioSource source;

        AudioSource bossSource;

        public AudioClip[] clips;

        // Instantiate FX and set ship at distance from player ship.
        private void Awake()
        {
            reference = FindObjectOfType<ReferenceManager>();
            bossSource = GameObject.Find("Boss Audio Source").GetComponent<AudioSource>();
            StartCoroutine(pauseForBoss());

            if (reference != null)
            {
                rocketTypeId = reference.objectPoolManager.GetTypeIdentifier(rocketPrefab);
            }

            Instantiate(spawnEmitter, transform);
            transform.localPosition = new Vector3(0, distance, 0);
        }

        // Subtract from and check timeLimit. Call Move(). Start Fire coroutine.
        private void Update()
        {
            timeLimit -= Time.deltaTime;
            if (timeLimit < 0.0F)
            {
                Destroy(gameObject);
            }

            if (currentTarget && !currentTarget.activeSelf)
            {
                currentTarget = null;
            }

            Move();

            if (canFire && slerpFraction >= 1.0F && currentTarget != null)
            {
                StartCoroutine(Fire(currentTarget));
            }
        }

        /// <summary>
        /// Determine transform of enemy nearest to player ship.
        /// </summary>
        /// <returns> Transform of nearest enemy ship. </returns>
        private Transform GetNearestEnemy()
        {
            Vector3 positionDifference;
            float distance;
            float shortestDistance = Mathf.Infinity;
            GameObject nearestEnemy = null;
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                NewBoss newBoss = enemy.GetComponent<NewBoss>();
                Enemy enemyScript = enemy.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    if (newBoss == null)
                    {
                        if (!enemyScript.PathingComplete || !enemyScript.IsActive)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (newBoss.CurrentState != BossStates.Combat)
                        {
                            continue;
                        }
                    }
                }
                else
                {
                    continue;
                }

                positionDifference = enemy.transform.position -
                                     transform.parent.transform.position;

                //faster than non-squared magnitude
                distance = positionDifference.sqrMagnitude;
                if (distance < shortestDistance && enemy.activeSelf)
                {
                    shortestDistance = distance;
                    nearestEnemy = enemy;
                }
            }

            //couldn't find any enemies
            if (shortestDistance == Mathf.Infinity)
            {
                return null;
            }

            return nearestEnemy.transform;
        }

        /// <summary>
        /// Controls movement and rotation. Utilizes slerp.
        /// </summary>
        private void Move()
        {
            //no current target
            if (currentTarget == null || currentTarget.activeSelf == false)
            {
                slerpTimer = 0.0F;

                //no enemies found
                if (GetNearestEnemy() == null)
                {
                    return;
                }

                currentTarget = GetNearestEnemy().gameObject;
            }

            Quaternion rotation = Quaternion.LookRotation((currentTarget.transform.position -
                                                           transform.position).normalized);

            Vector3 enemyLocalPosition = transform.parent.transform
                                                  .InverseTransformPoint(
                                                      currentTarget.transform.position);

            Vector3 localTranslation = new Vector3(enemyLocalPosition.x, enemyLocalPosition.y, 0)
                                           .normalized * distance;

            slerpFraction = slerpTimer / slerpTime;

            //is slerping
            if (slerpFraction < 1.0F)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, slerpFraction);
                transform.localPosition = Vector3
                    .Slerp(transform.localPosition, localTranslation, slerpFraction);

                slerpTimer += Time.deltaTime;
            }

            //not slerping
            else
            {
                transform.rotation = rotation;

                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, 1))

                //if(Physics.SphereCast(transform.position, sphereCastRadius, transform.forward, out hit, sphereCastMaxDistance))
                //if(Physics.SphereCast(transform.position, sphereCastRadius, transform.forward, out hit, sphereCastMaxDistance, LayerMask.GetMask("Player")))
                {
                    if (hit.collider.gameObject.CompareTag("Player") ||
                        hit.collider.gameObject.GetComponent<Shield>() != null)
                    {
                        slerpTimer = 0.0F;
                    }
                }
            }
        }

        /// <summary>
        /// Instantiate bullet according to firerate.
        /// </summary>
        /// <param name="target"> The target to launch the rocket at </param>
        private IEnumerator Fire(GameObject target)
        {
            canFire = false;
            GameObject rocket =
                reference.objectPoolManager.Spawn(gameObject, rocketTypeId, shootPoint.position,
                                                  shootPoint.rotation);
            Rocket rocketScript = rocket.GetComponent<Rocket>();
            rocketScript.SetupRocket((int)rocketType);
            rocketScript.Launch(target, target.transform.position);

            source.PlayOneShot(clips[0]);

            yield return new WaitForSeconds(1.0f / firerate);

            canFire = true;
        }

        IEnumerator pauseForBoss()
        {
            if (bossSource.isPlaying)
            {
                yield return new WaitWhile(() => bossSource.isPlaying);

                if (source.isPlaying)
                {
                    yield return new WaitWhile(() => source.isPlaying);
                }

                if (!source.isPlaying)
                {
                    source.PlayOneShot(clips[1]);
                }
            }
            else if (!bossSource.isPlaying)
            {
                source.PlayOneShot(clips[1]);
            }
        }
    }
}