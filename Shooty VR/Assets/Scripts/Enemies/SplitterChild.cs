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
        [Tooltip("The number of seconds between each shot.")]
        public float fireRate;
        [Tooltip("The speed at which the bullets are shot.")]
        public float projectileSpeed;
        public float spread;
        private GameObject player;
        private bool canShoot = true;

        public AudioSource source;
        public AudioClip[] clips;

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

                if (player == null)
                {
                    transform.LookAt(new Vector3(0.0f, 0.0f, 0.0f));
                }
            }
        }

        /// <summary>
        /// Shoots a projectile at the player.
        /// </summary>
        private IEnumerator Shoot()
        {
            canShoot = false;

            GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
            projectile.GetComponent<Transform>().Rotate(Random.Range(-spread, spread),
                                                        Random.Range(-spread, spread),
                                                        Random.Range(-spread, spread));
            projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * projectileSpeed;

            source.PlayOneShot(clips[0]);

            yield return new WaitForSeconds(fireRate);

            canShoot = true;
        }
    }
}
