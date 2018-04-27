//=============================================================================
//
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-440-01, CPSC-340-01 & CPSC-344-01
// Group Project
//
// 
//
//=============================================================================

using System.Collections;
using UnityEngine;
using Hive.Armada.Game;
using Hive.Armada.Enemies;

namespace Hive.Armada.Player.Weapons
{
    /// <summary>
    /// The plasma gun controller for the player ship.
    /// </summary>
    public class PlasmaGun : Weapon
    {
        public int maxAmmo;

        public float reloadTime;

        public float reloadDelay;

        private int currentAmmo;

        private Coroutine reloadCoroutine;

        private bool isReloading;

        /// <summary>
        /// Prefab for the rocket gameobject.
        /// </summary>
        [Header("Projectiles")]
        public GameObject rocketPrefab;

        /// <summary>
        /// The type of rocket to use.
        /// </summary>
        public RocketAttributes.RocketType rocketType;

        /// <summary>
        /// The type id for the rockets.
        /// </summary>
        private short rocketTypeId;

        /// <summary>
        /// Array of transforms for the barrels of the rocket pod.
        /// </summary>
        public Transform[] barrels;

        /// <summary>
        /// Emitter for the launching of the rockets.
        /// </summary>
        [Header("Emitters")]
        public GameObject plasmaLaunchEmitter;

        /// <summary>
        /// The audio source for the rocket pod sounds
        /// </summary>
        [Header("Audio")]
        public AudioSource source;

        /// <summary>
        /// The sound the plasma gun makes when it fires.
        /// </summary>
        public AudioClip plasmaGunShootSound;

        /// <summary>
        /// The sounds used when the plasma gun is charging/complete
        /// </summary>
        public AudioClip[] plasmaGunChargingSounds;

        private WaitForSeconds waitReloadDelay;

        /// <summary>
        /// Initializes the rockets and active/inactive pools.
        /// </summary>
        protected override void SetupWeapon()
        {
            waitFire = new WaitForSeconds(1.0f / fireRate);
            waitReloadDelay = new WaitForSeconds(reloadDelay);
            currentAmmo = maxAmmo;

            rocketTypeId = reference.objectPoolManager.GetTypeIdentifier(rocketPrefab);
        }

        /// <summary>
        /// Gets the enemy, interactable, or position on the wall that player is aimed at.
        /// Shoots the enemy or interactable if there is one.
        /// </summary>
        protected override void Clicked()
        {
            if (canShoot && currentAmmo > 0)
            {
                RaycastHit hit;

                if (AimAssistActive)
                {
                    if (Physics.SphereCast(transform.position, radius, transform.forward, out hit, 200.0f,
                                        Utility.shootableMask))
                    {
                        StartCoroutine(Shoot(hit.collider.gameObject, hit.point, transform.forward));
                    }
                    else if (Physics.SphereCast(transform.position, radius, transform.forward, out hit,
                                                200.0f,
                                                Utility.enemyMask))
                    {
                        StartCoroutine(Shoot(hit.collider.gameObject, hit.point, transform.forward));
                    }
                    else if (Physics.Raycast(transform.position, transform.forward, out hit, 200.0f,
                                             Utility.roomMask))
                    {
                        StartCoroutine(Shoot(null, hit.point, transform.forward));
                    }
                }
                else
                {
                    if (Physics.Raycast(transform.position, transform.forward, out hit, 200.0f,
                                        Utility.shootableMask))
                    {
                        StartCoroutine(Shoot(hit.collider.gameObject, hit.point, transform.forward));
                    }
                    else if (Physics.Raycast(transform.position, transform.forward, out hit,
                                                200.0f,
                                                Utility.enemyMask))
                    {
                        StartCoroutine(Shoot(hit.collider.gameObject, hit.point, transform.forward));
                    }
                    else if (Physics.Raycast(transform.position, transform.forward, out hit, 200.0f,
                                             Utility.roomMask))
                    {
                        StartCoroutine(Shoot(null, hit.point, transform.forward));
                    }
                }
            }
        }

        /// <summary>
        /// Shoots plasma. Cycles through the barrels.
        /// </summary>
        /// <param name="position"> The position to launch the plasma at </param>
        private IEnumerator Shoot(GameObject target, Vector3 position, Vector3 forward)
        {
            canShoot = false;

            if (--currentAmmo == 0)
            {
                if (reloadCoroutine != null)
                {
                    StopCoroutine(reloadCoroutine);
                }

                reloadCoroutine = StartCoroutine(Reload());
            }

            reference.statistics.IsFiring();
            reference.statistics.WeaponFired("Plasma Gun", 1);
            reference.playerIdleTimer.SetIsIdle(false);

            int barrelIndex = Random.Range(0, barrels.Length);

            Transform barrel = barrels[barrelIndex];

            GameObject rocket =
                    reference.objectPoolManager.Spawn(gameObject, rocketTypeId, barrel.position,
                                                      barrel.rotation);
            Rocket rocketScript = rocket.GetComponent<Rocket>();

            bool launched = false;
            if (target != null)
            {
                if (target.GetComponent<Poolable>() != null)
                {
                    if (target.GetComponent<Poolable>().IsActive)
                    {
                        rocketScript.Launch(target, position, damageMultiplier);
                        launched = true;
                    }
                }

                if (!launched)
                {
                    if (target.GetComponent<Enemy>() != null)
                    {
                        if (target.GetComponent<Enemy>().Health > 0)
                        {
                            rocketScript.Launch(target, position, damageMultiplier);
                            launched = true;
                        }
                    }
                }

                if (!launched)
                {
                    if (target.activeInHierarchy && target.activeSelf)
                    {
                        rocketScript.Launch(target, position, damageMultiplier);
                        launched = true;
                    }
                }
            }
            else
            {
                rocketScript.Launch(null, position, damageMultiplier);
                launched = true;
            }

            //source.PlayOneShot(rocketPodLaunchSound);

            if (!launched)
            {
                RaycastHit hit;
                if (Physics.Raycast(position, forward, out hit, 200.0f,
                                    Utility.roomMask))
                {
                    rocketScript.Launch(null, hit.point, damageMultiplier);
                }
                else
                {
                    Debug.LogWarning(GetType().Name +
                                     " - Unable to redirect from invalid target to room.");
                    reference.objectPoolManager.Despawn(rocket);
                }
            }

            yield return waitFire;

            canShoot = true;

            reference.statistics.IsNotFiring();
            reference.playerIdleTimer.SetIsIdle(true);
        }

        private IEnumerator Reload()
        {
            source.PlayOneShot(plasmaGunChargingSounds[0]);

            yield return waitReloadDelay;

            yield return new WaitForSeconds(reloadTime / maxAmmo * currentAmmo);

            currentAmmo = maxAmmo;

            //float timeToWait = reloadTime / maxAmmo * currentAmmo;

            //while (currentAmmo < maxAmmo)
            //{
            //    yield return new WaitForSeconds(timeToWait);

            //    ++currentAmmo;
            //}

            source.PlayOneShot(plasmaGunChargingSounds[1]);

            reloadCoroutine = null;
        }
    }
}