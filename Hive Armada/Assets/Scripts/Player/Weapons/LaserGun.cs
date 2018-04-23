//=============================================================================
// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
// 
// This is the player laser gun. It has two barrels and shoots alternating
// lasers using LineRenderers. There is a muzzle flash and hit spark particle
// emitter that spawn on the barrel and hit enemy, respectively, to give the
// player more feedback.
// 
//=============================================================================

using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using Hive.Armada.Enemies;
using MirzaBeig.ParticleSystems;

namespace Hive.Armada.Player.Weapons
{
    /// <summary>
    /// Player laser gun
    /// </summary>
    public class LaserGun : Weapon
    {
        /// <summary>
        /// Alternates between firing the left and right laser
        /// </summary>
        private bool isLeftFire = true;

        /// <summary>
        /// Material for the lasers
        /// </summary>
        [Header("Lasers")]
        public Material laserMaterial;

        /// <summary>
        /// Thickness of the lasers
        /// </summary>
        public float thickness = 0.002f;

        /// <summary>
        /// Left gun
        /// </summary>
        public GameObject left;

        /// <summary>
        /// The left laser's LineRenderer
        /// </summary>
        private LineRenderer leftLaser;

        /// <summary>
        /// Right gun
        /// </summary>
        public GameObject right;

        /// <summary>
        /// The right laser's LineRenderer
        /// </summary>
        private LineRenderer rightLaser;

        /// <summary>
        /// Particle emitter for the hit spark effect.
        /// </summary>
        [Header("Emitters")]
        public GameObject hitSparkEmitter;

        /// <summary>
        /// Particle emitter for the muzzle flash effect.
        /// </summary>
        public GameObject muzzleFlashEmitter;

        /// <summary>
        /// Pool of muzzle flash emitters.
        /// </summary>
        private ParticleSystems[] muzzleFlashEmitters;

        /// <summary>
        /// Index of the next muzzle flash emitter to use.
        /// </summary>
        private int muzzleFlashIndex;

        /// <summary>
        /// The audio source for the laser gun sounds
        /// </summary>
        [Header("Audio")]
        public AudioSource source;

        /// <summary>
        /// The sound the laser gun makes when it fires.
        /// </summary>
        public AudioClip laserShootSound;

        /// <summary>
        /// Initializes the LineRenderer's that are the lasers and
        /// the muzzle flash emitter pool.
        /// </summary>
        protected override void SetupWeapon()
        {
            waitFire = new WaitForSeconds(1.0f / fireRate);

            leftLaser = left.gameObject.AddComponent<LineRenderer>();
            leftLaser.material = laserMaterial;
            leftLaser.shadowCastingMode = ShadowCastingMode.Off;
            leftLaser.receiveShadows = false;
            leftLaser.alignment = LineAlignment.View;
            leftLaser.startWidth = thickness;
            leftLaser.endWidth = thickness;
            leftLaser.enabled = false;

            rightLaser = right.gameObject.AddComponent<LineRenderer>();
            rightLaser.material = laserMaterial;
            rightLaser.shadowCastingMode = ShadowCastingMode.Off;
            rightLaser.receiveShadows = false;
            rightLaser.alignment = LineAlignment.View;
            rightLaser.startWidth = thickness;
            rightLaser.endWidth = thickness;
            rightLaser.enabled = false;

            muzzleFlashEmitters = new ParticleSystems[Mathf.FloorToInt(fireRate * 2.5f)];
            muzzleFlashIndex = 0;

            for (int i = 0; i < muzzleFlashEmitters.Length; ++i)
            {
                GameObject muzzleFlash = Instantiate(muzzleFlashEmitter, transform);
                ParticleSystems muzzleFlashSystem = muzzleFlash.GetComponent<ParticleSystems>();

                muzzleFlashSystem.stop();
                muzzleFlashSystem.clear();
                muzzleFlashEmitters[i] = muzzleFlashSystem;
            }
        }

        /// <summary>
        /// Gets the enemy, interactable, or position on the wall that player is aimed at.
        /// Shoots the enemy or interactable if there is one.
        /// </summary>
        protected override void Clicked()
        {
            RaycastHit hit;

            if (AimAssistActive)
            {
                if (Physics.SphereCast(transform.position, radius, transform.forward, out hit,
                                       200.0f,
                                       Utility.shootableMask))
                {
                    StartCoroutine(Shoot(hit.point));

                    Instantiate(hitSparkEmitter, hit.point,
                                Quaternion.LookRotation(hit.point - gameObject.transform.position));

                    if (hit.collider.gameObject.GetComponent<Shootable>() != null
                        && hit.collider.gameObject.GetComponent<Shootable>().isShootable)
                    {
                        hit.collider.gameObject.GetComponent<Shootable>().Hit();
                    }

                    shipController.hand.controller.TriggerHapticPulse(2500);
                }
                else if (Physics.SphereCast(transform.position, radius, transform.forward, out hit,
                                            200.0f,
                                            Utility.enemyMask))
                {
                    StartCoroutine(Shoot(hit.point));

                    Instantiate(hitSparkEmitter, hit.point,
                                Quaternion.LookRotation(hit.point - gameObject.transform.position));

                    if (hit.collider.gameObject.GetComponent<Enemy>() != null)
                    {
                        hit.collider.gameObject.GetComponent<Enemy>()
                           .Hit(damage * damageMultiplier, true);
                    }

                    shipController.hand.controller.TriggerHapticPulse(2500);
                }
                else if (Physics.Raycast(transform.position, transform.forward, out hit, 200.0f,
                                         Utility.roomPathingMask))
                {
                    StartCoroutine(Shoot(hit.point));
                }
            }
            else
            {
                if (Physics.Raycast(transform.position, transform.forward, out hit, 200.0f,
                                    Utility.shootableMask))
                {
                    StartCoroutine(Shoot(hit.point));

                    Instantiate(hitSparkEmitter, hit.point,
                                Quaternion.LookRotation(hit.point - gameObject.transform.position));

                    if (hit.collider.gameObject.GetComponent<Shootable>() != null
                        && hit.collider.gameObject.GetComponent<Shootable>().isShootable)
                    {
                        hit.collider.gameObject.GetComponent<Shootable>().Hit();
                    }

                    shipController.hand.controller.TriggerHapticPulse(2500);
                }
                else if (Physics.Raycast(transform.position, transform.forward, out hit, 200.0f,
                                         Utility.enemyMask))
                {
                    StartCoroutine(Shoot(hit.point));

                    Instantiate(hitSparkEmitter, hit.point,
                                Quaternion.LookRotation(hit.point - gameObject.transform.position));

                    if (hit.collider.gameObject.GetComponent<Enemy>() != null)
                    {
                        hit.collider.gameObject.GetComponent<Enemy>()
                           .Hit(damage * damageMultiplier, true);
                    }

                    shipController.hand.controller.TriggerHapticPulse(2500);
                }
                else if (Physics.Raycast(transform.position, transform.forward, out hit, 200.0f,
                                         Utility.roomPathingMask))
                {
                    StartCoroutine(Shoot(hit.point));
                }
            }
        }

        /// <summary>
        /// Blocks the gun from shooting based on fire rate, updates LineRenderers, plays a muzzle
        /// flash emitter, plays the gun sound and starts the FlashLaser coroutine.
        /// </summary>
        /// <param name="position"> The position of the target being shot </param>
        private IEnumerator Shoot(Vector3 position)
        {
            canShoot = false;
            GameObject laser;

            if (isLeftFire)
            {
                leftLaser.endWidth = thickness *
                                     Mathf.Max(
                                         Vector3.Magnitude(left.transform.position - position),
                                         1.0f);

                leftLaser.SetPosition(0, left.transform.position);
                leftLaser.SetPosition(1, position);

                laser = left;
            }
            else
            {
                rightLaser.endWidth = thickness *
                                      Mathf.Max(
                                          Vector3.Magnitude(right.transform.position - position),
                                          1.0f);

                rightLaser.SetPosition(0, right.transform.position);
                rightLaser.SetPosition(1, position);

                laser = right;
            }

            ParticleSystems muzzleFlash = muzzleFlashEmitters[muzzleFlashIndex++];
            muzzleFlash.gameObject.transform.position = laser.transform.position;
            muzzleFlash.gameObject.transform.rotation = laser.transform.rotation;
            muzzleFlash.play();

            if (muzzleFlashIndex >= muzzleFlashEmitters.Length)
            {
                muzzleFlashIndex = 0;
            }

            StartCoroutine(FlashLaser(isLeftFire));

            source.PlayOneShot(laserShootSound);

            reference.statistics.IsFiring();
            reference.statistics.WeaponFired("Laser Gun", 1);
            reference.playerIdleTimer.SetIsIdle(false);

            yield return waitFire;

            isLeftFire = !isLeftFire;
            canShoot = true;

            reference.statistics.IsNotFiring();
            reference.playerIdleTimer.SetIsIdle(true);
        }

        /// <summary>
        /// Flashes the appropriate laser LineRenderer.
        /// </summary>
        /// <param name="isLeft"> If the left laser is the one to flash </param>
        private IEnumerator FlashLaser(bool isLeft)
        {
            if (isLeft)
            {
                leftLaser.enabled = true;
            }
            else
            {
                rightLaser.enabled = true;
            }

            yield return Utility.waitLineRendererFlash;

            if (isLeft)
            {
                leftLaser.enabled = false;
            }
            else
            {
                rightLaser.enabled = false;
            }
        }
    }
}