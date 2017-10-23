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

using System.Collections;
using UnityEngine;

namespace Hive.Armada.Enemies
{
    public class SplitterChild : Enemy
    {
        public GameObject projectilePrefab;
        public Transform shootPoint;
        //[Tooltip("Number of seconds between shots")]
        //public float shootDelay;
        public float fireRate;
        public float projectileSpeed;
        public float spread;
        private GameObject player;
        private bool canShoot;

        void Update()
        {
            if (player != null)
            {
                transform.LookAt(player.transform);

                if (canShoot)
                {
                    StartCoroutine(Shoot());
                }
            }
            else
            {
                player = GameObject.FindGameObjectWithTag("Player");
            }
        }

        /// <summary>
        /// Shoots a projectile at the player.
        /// </summary>
        /// <returns> Waits until the enemy can fire again </returns>
        private IEnumerator Shoot()
        {
            canShoot = false;

            GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
            projectile.GetComponent<Transform>().Rotate(Random.Range(-spread, spread),
                                                        Random.Range(-spread, spread),
                                                        Random.Range(-spread, spread));
            projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * projectileSpeed;

            yield return new WaitForSeconds(1.0f / fireRate);

            canShoot = true;
        }
    }
}
