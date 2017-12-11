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

namespace Hive.Armada.PowerUps
{
    /// <summary>
    /// Ally powerup with slerp movement. 
    /// </summary>
    public class AllySlerp : MonoBehaviour
    {
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
        public GameObject currentTarget = null;

        /// <summary>
        /// Represents progress of current slerp movement in terms of time.
        /// </summary>
        private float slerpTimer = 0.0F;

        /// <summary>
        /// Ratio of slerpFraction / slerpTimer.
        /// </summary>
        private float slerpFraction;

        //private bool targetAcquired;

        /// <summary>
        /// Projectile prefab.
        /// </summary>
        public GameObject bulletPrefab;

        /// <summary>
        /// FX instanted in Start().
        /// </summary>
        public GameObject spawnEmitter;

        /// <summary>
        /// Speed of fired projectiles.
        /// </summary>
        public float bulletSpeed;

        /// <summary>
        /// Number of projectiles fired per second.
        /// </summary>
        public float firerate;

        /// <summary>
        /// State controllering when ally can fire.
        /// </summary>
        private bool canFire = true;

		public AudioSource source;
        public AudioClip[] clips;

        // Instantiate FX and set ship at distance from player ship.
        void Start()
        {
            Instantiate(spawnEmitter, transform);
            transform.localPosition = new Vector3(0, distance, 0);
        }

        // Subtract from and check timeLimit. Call Move(). Start Fire coroutine.
        void Update()
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
                StartCoroutine(Fire(currentTarget.transform.position));
            }
        }

        /// <summary>
        /// Determine transform of enemy nearest to player ship.
        /// </summary>
        /// <returns>Transform of nearest enemy ship.</returns>
        private Transform GetNearestEnemy()
        {
            Vector3 positionDifference;
            float distance;
            float shortestDistance = Mathf.Infinity;
            GameObject nearestEnemy = null;
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
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
            else
            {
                return nearestEnemy.transform;
            }
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
                else
                {
                    currentTarget = GetNearestEnemy().gameObject;
                }
            }

            Quaternion rotation = Quaternion.LookRotation((currentTarget.transform.position - 
                    transform.position).normalized);

            Vector3 enemyLocalPosition = transform.parent.transform
                    .InverseTransformPoint(currentTarget.transform.position);

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
        /// <param name="target">Enemy bullet is "aimed" at</param>
        private IEnumerator Fire(Vector3 target)
        {
            canFire = false;
            var bullet = Instantiate(bulletPrefab, transform.Find("BulletPoint")
                    .transform.position, transform.rotation);

            source.PlayOneShot(clips[0]);

            bullet.transform.LookAt(target);
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletSpeed;

            yield return new WaitForSeconds(1.0f / firerate);
            canFire = true;
        }
    }
}
