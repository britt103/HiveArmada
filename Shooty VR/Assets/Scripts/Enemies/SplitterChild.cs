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

using System;
using System.Collections;
using UnityEngine;
using Hive.Armada.Game;
using Random = UnityEngine.Random;
using System.Collections.Generic;

namespace Hive.Armada.Enemies
{
    /// <summary>
    /// Splitter child enemy
    /// </summary>
    public class SplitterChild : Enemy
    {
        private int projectileTypeIdentifier;

        public Transform shootPoint;

        private float fireRate;

        private float projectileSpeed;

        private float spread;

        private GameObject player;

        private bool canShoot = true;

        public override void Awake()
        {
            reference = GameObject.Find("Reference Manager").GetComponent<ReferenceManager>();

            if (reference == null)
            {
                Debug.LogError(GetType().Name + " - Could not find Reference Manager!");
            }

            materials = new List<Material>();
            health = maxHealth;
        }

        private void Update()
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

            GameObject projectile =
                reference.objectPoolManager.Spawn(projectileTypeIdentifier, shootPoint.position, shootPoint.rotation);
            projectile.GetComponent<Transform>().Rotate(Random.Range(-spread, spread),
                Random.Range(-spread, spread),
                Random.Range(-spread, spread));
            projectile.GetComponent<Rigidbody>().velocity =
                projectile.transform.forward * projectileSpeed;

            yield return new WaitForSeconds(fireRate);

            canShoot = true;
        }

        protected override void Reset()
        {
            projectileTypeIdentifier = reference.enemyAttributes.EnemyProjectileTypeIdentifiers[TypeIdentifier];
            maxHealth = reference.enemyAttributes.enemyHealthValues[TypeIdentifier];
            health = maxHealth;
            fireRate = reference.enemyAttributes.enemyFireRate[TypeIdentifier];
            projectileSpeed = reference.enemyAttributes.projectileSpeed;
            spread = reference.enemyAttributes.enemySpread[TypeIdentifier];
            pointValue = reference.enemyAttributes.enemyScoreValues[TypeIdentifier];
        }
    }
}