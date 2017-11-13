//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.champan.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// AllyStatic controls the Ally powerup. The standard Ally fires projectiles
// at the nearest enemy until that enemy's death and self-destructs after a
// certain time. The AllyStatic version of this remains stationary and
// rotates to fire at the nearest enemy in front of the player ship.
// Currently not in use.
//
//=============================================================================

using System.Collections;
using UnityEngine;

namespace Hive.Armada.Powerups
{
    /// <summary>
    /// Ally powerup with static movement.
    /// </summary>
    public class AllyStatic : MonoBehaviour
    {
        /// <summary>
        /// Position relative to player ship.
        /// </summary>
        public Vector3 localPosition;

        /// <summary>
        /// Time until self-destruct.
        /// </summary>
        public float timeLimit;

        //Projectile prefab.
        public GameObject bullet;

        /// <summary>
        /// Projectile speed.
        /// </summary>
        public float bulletSpeed;

        //Number of projectiles fired per second.
        public float firerate;

        /// <summary>
        /// State controllering when ally can fire.
        /// </summary>
        private bool canFire = true;

        // Set transform.localPosition.
        void Start()
        {
            transform.localPosition = localPosition;
        }

        // Subtract from and check timeLimit. Rotate to face target. Start Fire coroutine.
        void Update()
        {
            timeLimit -= Time.deltaTime;
            if (timeLimit < 0.0F)
            {
                FindObjectOfType<PowerupStatus>().powerupTypeActive[0] = false;
                Destroy(gameObject);
            }

            Transform target = GetNearestEnemy();
            transform.LookAt(target);

            if (canFire)
            {
                StartCoroutine(Fire(target.position));
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
            float enemyLocalZ;
            float shortestDistance = Mathf.Infinity;
            GameObject nearestEnemy = null;
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                positionDifference = enemy.transform.position - 
                        transform.parent.transform.position;

                //faster than non-squared magnitude
                distance = positionDifference.sqrMagnitude;
                enemyLocalZ = transform.parent.transform
                        .InverseTransformPoint(enemy.transform.position).z;

                //for static, want enemy to be in front of ally
                if (distance < shortestDistance && enemyLocalZ > localPosition.z)
                {
                    shortestDistance = distance;
                    nearestEnemy = enemy;
                }
            }
            return nearestEnemy.transform;
        }

        /// <summary>
        /// Instantiate bullet according to firerate.
        /// </summary>
        /// <param name="target">Enemy bullet is "aimed" at</param>
        private IEnumerator Fire(Vector3 target)
        {
            canFire = false;
            var laser = Instantiate(bullet, transform.position, transform.rotation);

            laser.transform.LookAt(target);
            laser.GetComponent<Rigidbody>().velocity = laser.transform.forward * bulletSpeed;

            yield return new WaitForSeconds(1.0f / firerate);
            canFire = true;
        }
    }
}