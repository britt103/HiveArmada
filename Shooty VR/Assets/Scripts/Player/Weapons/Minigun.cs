//=============================================================================
// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
// 
// This class handles the miniguns for the player ship. Each minigun has 6
// barrels. It alternates between which minigun is shooting and randomly picks
// a barrel to shoot from. Every other shot (barrel-dependent) will spawn a hit
// spark effect at shot target, either the enemy or the wall. The minigun will
// also periodically shoot a visible tracer effect to give the player more
// feedback so they know where they are shooting.
// 
//=============================================================================

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using Hive.Armada.Enemies;
using Random = UnityEngine.Random;

namespace Hive.Armada.Player.Weapons
{
    /// <summary>
    /// The minigun controller for the player ship.
    /// </summary>
    public class Minigun : Weapon
    {
        /// <summary>
        /// Array of points where the left minigun can shoot from.
        /// </summary>
        [Tooltip("Points where the left minigun can shoot from.")]
        public GameObject[] left;

        /// <summary>
        /// Array of points where the left minigun can shoot from.
        /// </summary>
        [Tooltip("Points where the right minigun can shoot from.")]
        public GameObject[] right;

        /// <summary>
        /// Used to alternate between left and right guns firing
        /// </summary>
        private bool isLeftFire = true;

        public float thickness;

        /// <summary>
        /// </summary>
        public Material material;

        /// <summary>
        /// Used to update number of shots and time firing
        /// </summary>
        private PlayerStats stats;

        /// <summary>
        /// Used to update state value of PlayerIdleTimer
        /// </summary>
        private PlayerIdleTimer playerIdleTimer;

        /// <summary>
        /// Initializes variables
        /// </summary>
        private void Start()
        {
            SetupLineRenderers();
            damageMultiplier = 1;
            damage = shipController.weaponDamage[1];
            fireRate = shipController.weaponFireRate[1];

            stats = FindObjectOfType<PlayerStats>();
            playerIdleTimer = FindObjectOfType<PlayerIdleTimer>();
        }

        /// <summary>
        /// Gets enemy or wall aimpoint and shoots at it.
        /// </summary>
        protected override void Clicked()
        {
            RaycastHit hit;
            if (Physics.SphereCast(transform.position, radius, transform.forward, out hit, 200.0f,
                Utility.enemyMask))
            {
                StartCoroutine(Shoot(hit.collider.gameObject, hit.point));

                if (hit.collider.gameObject.GetComponent<Enemy>() != null)
                {
                    hit.collider.gameObject.GetComponent<Enemy>().Hit(damage * damageMultiplier);
                }

                shipController.hand.controller.TriggerHapticPulse(2500);
            }
            else if (Physics.Raycast(transform.position, transform.forward, out hit, 200.0f,
                Utility.roomMask))
            {
                StartCoroutine(Shoot(hit.collider.gameObject, hit.point));
            }
        }

        /// <summary>
        /// Shoots the miniguns. Alternates between left and right, shooting from a
        /// random barrel for aesthetic purposes. Every other shot from each minigun
        /// will spawn a hit spark on the target. Periodically spawns a tracer to
        /// give the player feedback as to where they are actually shooting.
        /// </summary>
        /// <param name="target"> The GameObject being shot </param>
        /// <param name="position"> The position of the shot </param>
        private IEnumerator Shoot(GameObject target, Vector3 position)
        {
            canShoot = false;

            GameObject barrel = isLeftFire
                ? left[Random.Range(0, left.Length)]
                : right[Random.Range(0, right.Length)];

            StartCoroutine(FlashShot(barrel, position, barrel.GetComponent<LineRenderer>()));

            stats.isFiring = true;
            stats.ShotsFired(1);
            playerIdleTimer.SetIsIdle(false);

            yield return new WaitForSeconds(1.0f / fireRate);

            isLeftFire = !isLeftFire;
            canShoot = true;

            stats.isFiring = false;
            playerIdleTimer.SetIsIdle(true);
        }

        /// <summary>
        /// Flashes the LineRenderer on the given barrel
        /// </summary>
        /// <param name="barrel"> The barrel GameObject to start the LineRenderer at </param>
        /// <param name="position"> The position of the target to aim the LineRenderer at </param>
        /// <param name="tracer"> The LineRenderer itself </param>
        private static IEnumerator FlashShot(GameObject barrel, Vector3 position,
            LineRenderer tracer)
        {
            tracer.SetPosition(0, barrel.transform.position);
            tracer.SetPosition(1, position);
            tracer.enabled = true;

            yield return new WaitForSeconds(0.006f);

            tracer.enabled = false;
        }

        /// <summary>
        /// Calls the initialization of all LineRenderers
        /// </summary>
        private void SetupLineRenderers()
        {
            for (int i = 0; i < left.Length; ++i)
            {
                InitLineRenderer(left[i]);
                InitLineRenderer(right[i]);
            }
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

            r.material = material;
            r.shadowCastingMode = ShadowCastingMode.Off;
            r.receiveShadows = false;
            r.alignment = LineAlignment.View;
            r.startWidth = thickness;
            r.endWidth = thickness;
            r.enabled = false;
        }
    }
}