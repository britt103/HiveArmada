//=============================================================================
//
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-440-01, CPSC-340-01 & CPSC-344-01
// Group Project
//
// This class handles the rocket pod weapon for the player ship. Each time the
// rocket pod is fired, it launches 'burstAmount' of rockets from a set of
// barrels. These rockets home in on the target and blow it up.
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Hive.Armada.Game;

namespace Hive.Armada.Player.Weapons
{
    /// <summary>
    /// The rocket pod controller for the player ship.
    /// </summary>
    public class RocketPod : Weapon
    {
        /// <summary>
        /// Prefab for the rocket gameobject.
        /// </summary>
        [Header("Rockets")]
        public GameObject rocketPrefab;

        /// <summary>
        /// The type of rocket to use.
        /// </summary>
        public RocketAttributes.RocketType rocketType;

        /// <summary>
        /// The index of the rocket type to use.
        /// </summary>
        private int rocketTypeIndex;

        /// <summary>
        /// The type id for the rockets.
        /// </summary>
        private int rocketTypeId;

        /// <summary>
        /// Array of transforms for the barrels of the rocket pod.
        /// </summary>
        public Transform[] barrels;

        /// <summary>
        /// Index of the barrel to launch the rocket from.
        /// </summary>
        private int barrelIndex;

        /// <summary>
        /// Number of rockets launched when the rocket pod fires.
        /// </summary>
        public int burstAmount;

        /// <summary>
        /// Emitter for the launching of the rockets.
        /// </summary>
        [Header("Emitters")]
        public GameObject rocketLaunchEmitter;

        /// <summary>
        /// Emitter for the rocket impact/self destruct explosion.
        /// </summary>
        public GameObject rocketExplosionEmitter;

        /// <summary>
        /// The audio source for the rocket pod sounds
        /// </summary>
        [Header("Audio")]
        public AudioSource source;

        /// <summary>
        /// The sound the rocket pod makes when it fires.
        /// </summary>
        public AudioClip rocketPodLaunchSound;

        /// <summary>
        /// Initializes the rockets and active/inactive pools.
        /// </summary>
        protected override void SetupWeapon()
        {
            rocketTypeIndex = -1;

            for (int i = 0; i < reference.rocketAttributes.rockets.Length; ++i)
            {
                if (reference.rocketAttributes.rockets[i].rocketType == rocketType)
                {
                    rocketTypeIndex = i;
                    break;
                }
            }

            rocketTypeId = reference.objectPoolManager.GetTypeIdentifier(rocketPrefab);
        }

        /// <summary>
        /// Gets the enemy, interactable, or position on the wall that player is aimed at.
        /// Shoots the enemy or interactable if there is one.
        /// </summary>
        protected override void Clicked()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 200.0f,
                                Utility.shootableMask))
            {
                StartCoroutine(Shoot(hit.collider.gameObject, hit.point));
            }
            else if (Physics.SphereCast(transform.position, radius, transform.forward, out hit,
                                        200.0f,
                                        Utility.enemyMask))
            {
                StartCoroutine(Shoot(hit.collider.gameObject, hit.point));
            }
            else if (Physics.Raycast(transform.position, transform.forward, out hit, 200.0f,
                                     Utility.roomMask))
            {
                StartCoroutine(Shoot(null, hit.point));
            }
        }

        /// <summary>
        /// Shoots rockets. Cycles through the barrels.
        /// </summary>
        /// <param name="target"> The gameobject to launch the rocket at </param>
        /// <param name="position"> The position to launch the rocket at </param>
        private IEnumerator Shoot(GameObject target, Vector3 position)
        {
            canShoot = false;

            reference.statistics.IsFiring();
            reference.statistics.WeaponFired("Rocket Pods", 1);
            reference.playerIdleTimer.SetIsIdle(false);

            barrelIndex = 0;

            for (int i = 0; i < burstAmount; ++i)
            {
                if (barrelIndex >= barrels.Length)
                {
                    barrelIndex = 0;
                }

                Transform barrel = barrels[barrelIndex++];

                yield return new WaitForSeconds(0.3f / burstAmount);

                GameObject rocket =
                    reference.objectPoolManager.Spawn(rocketTypeId, barrel.position,
                                                      barrel.rotation);
                Rocket rocketScript = rocket.GetComponent<Rocket>();
                rocketScript.SetupRocket(rocketTypeIndex, shipController);
                rocketScript.SetDamageMultiplier(damageMultiplier);
                rocketScript.Launch(target, position);
            }

            yield return new WaitForSeconds(1.0f / fireRate);

            canShoot = true;

            reference.statistics.IsNotFiring();
            reference.playerIdleTimer.SetIsIdle(true);
        }
    }
}