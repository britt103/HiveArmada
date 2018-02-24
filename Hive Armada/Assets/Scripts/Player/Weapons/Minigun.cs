//=============================================================================
// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
// 
// This class handles the miniguns for the player ship. Each minigun has 8
// barrels. It alternates between left and right minigun, picking a random
// barrel on the shooting minigun to shoot from. The minigun "shoots" a tracer
// by flashing a LineRenderer. There is a muzzle flash and hit spark particle
// emitter that spawn on the barrel and hit enemy, respectively, to give the
// player more feedback.
// 
//=============================================================================

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using Hive.Armada.Enemies;
using MirzaBeig.ParticleSystems;
using Random = UnityEngine.Random;

namespace Hive.Armada.Player.Weapons
{
    /// <summary>
    /// The minigun controller for the player ship.
    /// </summary>
    public class Minigun : Weapon
    {
        /// <summary>
        /// Used to alternate between left and right guns firing
        /// </summary>
        private bool isLeftFire = true;

        /// <summary>
        /// Material for the minigun's tracers.
        /// </summary>
        [Header("Tracers")]
        public Material tracerMaterial;

        /// <summary>
        /// Thickness of the minigun tracers' LineRenderer's
        /// </summary>
        public float thickness = 0.003f;

        /// <summary>
        /// Array of points where the left minigun can shoot from.
        /// These are the barrels on the left minigun.
        /// </summary>
        [Tooltip("Array of points representing each barrel of the left minigun.")]
        public GameObject[] left;

        /// <summary>
        /// Array of points where the left minigun can shoot from.
        /// These are the barrels on the right minigun.
        /// </summary>
        [Tooltip("Array of points representing each barrel of the right minigun.")]
        public GameObject[] right;

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
        /// The audio source for the minigun sounds
        /// </summary>
        [Header("Audio")]
        public AudioSource source;

        /// <summary>
        /// The sound the minigun makes when it fires.
        /// </summary>
        public AudioClip minigunShootSound;

        private float overheat;

        public float overheatPerShot;

        public float overheatCooldown;

        private bool isOverheating;

        /// <summary>
        /// Initializes the LineRenderer's for the minigun tracers and
        /// the muzzle flash emitter pool.
        /// </summary>
        protected override void SetupWeapon()
        {
            for (int i = 0; i < left.Length; ++i)
            {
                InitLineRenderer(left[i]);
                InitLineRenderer(right[i]);
            }

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
            if (!isOverheating)
            {
                RaycastHit hit;
                if (Physics.SphereCast(transform.position, radius, transform.forward, out hit, 200.0f,
                                    Utility.shootableMask))
                {
                    StartCoroutine(Shoot(hit.point));

                    Instantiate(hitSparkEmitter, hit.point,
                                Quaternion.LookRotation(hit.point - gameObject.transform.position));

                    if (hit.collider.gameObject.GetComponent<Shootable>() != null
                        && hit.collider.gameObject.GetComponent<Shootable>().isShootable)
                    {
                        hit.collider.gameObject.GetComponent<Shootable>().Shot();
                    }

                    shipController.hand.controller.TriggerHapticPulse(2500);
                }
                else if (Physics.SphereCast(transform.position, radius, transform.forward, out hit, 200.0f,
                                            Utility.enemyMask))
                {
                    StartCoroutine(Shoot(hit.point));

                    Instantiate(hitSparkEmitter, hit.point,
                                Quaternion.LookRotation(hit.point - gameObject.transform.position));

                    if (hit.collider.gameObject.GetComponent<Enemy>() != null)
                    {
                        hit.collider.gameObject.GetComponent<Enemy>().Hit(damage * damageMultiplier);
                    }

                    shipController.hand.controller.TriggerHapticPulse(2500);
                }
                else if (Physics.Raycast(transform.position, transform.forward, out hit, 200.0f,
                                         Utility.roomMask))
                {
                    StartCoroutine(Shoot(hit.point));
                }
            }
        }

        /// <summary>
        /// Shoots the miniguns. Alternates between left and right, shooting from a random barrel
        /// for aesthetic purposes. Spawns the muzzle flash emitter and plays the firing sound.
        /// </summary>
        /// <param name="position"> The position of the shot </param>
        private IEnumerator Shoot(Vector3 position)
        {
            canShoot = false;
            AddOverheat();

            GameObject barrel = isLeftFire
                                    ? left[Random.Range(0, left.Length)]
                                    : right[Random.Range(0, right.Length)];

            ParticleSystems muzzleFlash = muzzleFlashEmitters[muzzleFlashIndex++];
            muzzleFlash.gameObject.transform.position = barrel.transform.position;
            muzzleFlash.gameObject.transform.rotation = barrel.transform.rotation;
            muzzleFlash.play();

            if (muzzleFlashIndex >= muzzleFlashEmitters.Length)
            {
                muzzleFlashIndex = 0;
            }

            StartCoroutine(FlashTracer(barrel, position, barrel.GetComponent<LineRenderer>()));

            source.PlayOneShot(minigunShootSound);

            reference.statistics.IsFiring();
            reference.statistics.WeaponFired("Minigun", 1);
            reference.playerIdleTimer.SetIsIdle(false);

            yield return new WaitForSeconds(1.0f / fireRate);

            isLeftFire = !isLeftFire;
            canShoot = true;

            reference.statistics.IsNotFiring();
            reference.playerIdleTimer.SetIsIdle(true);
        }

        /// <summary>
        /// Flashes the LineRenderer on the given barrel
        /// </summary>
        /// <param name="barrel"> The barrel GameObject to start the LineRenderer at </param>
        /// <param name="position"> The position of the target to aim the LineRenderer at </param>
        /// <param name="tracer"> The LineRenderer itself </param>
        private IEnumerator FlashTracer(GameObject barrel, Vector3 position, LineRenderer tracer)
        {
            tracer.endWidth = thickness *
                              Mathf.Max(Vector3.Magnitude(barrel.transform.position - position),
                                        1.0f);

            tracer.SetPosition(0, barrel.transform.position);
            tracer.SetPosition(1, position);
            tracer.enabled = true;

            yield return new WaitForSeconds(0.006f);

            tracer.enabled = false;
        }

        /// <summary>
        /// Initializes a LineRenderer on obj
        /// </summary>
        /// <param name="obj"> The object to put a LineRenderer on </param>
        private void InitLineRenderer(GameObject obj)
        {
            LineRenderer r;
            try
            {
                r = obj.AddComponent<LineRenderer>();
            }
            catch (Exception)
            {
                r = obj.GetComponent<LineRenderer>();
            }

            r.material = tracerMaterial;
            r.shadowCastingMode = ShadowCastingMode.Off;
            r.receiveShadows = false;
            r.alignment = LineAlignment.View;
            r.startWidth = thickness;
            r.endWidth = thickness;
            r.enabled = false;
        }

        private void AddOverheat()
        {
            overheat += overheatPerShot;

            if (overheat >= 100.0f)
            {
                isOverheating = true;
            }
        }

        /// <summary>
        /// Waits for the minigun to cool-down and then disables overheating.
        /// </summary>
        private IEnumerator OverheatCooldown()
        {
            yield return new WaitForSeconds(overheatCooldown);
            overheat = 0.0f;
            isOverheating = false;
        }
    }
}