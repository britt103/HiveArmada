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
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using Hive.Armada;
using Hive.Armada.Enemies;

namespace Hive.Armada.Player.Guns
{
    /// <summary>
    /// The minigun controller for the player ship.
    /// </summary>
    public class Minigun : Gun
    {
        public GameObject tracerPrefab;
        public GameObject hitSparkPrefab;
        public GameObject muzzleFlashPrefab;

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
        /// The prefab to use for the projectile
        /// </summary>
        public GameObject projectilePrefab;

        /// <summary>
        /// 
        /// </summary>
        public float projectileSpeed;

        /// <summary>
        /// 
        /// </summary>
        [Tooltip("Radius for the aim assist SphereCast")]
        public float radius = 0.3f;

        /// <summary>
        /// 
        /// </summary>
        private bool isLeftFire = true;

        private List<GameObject> tracers;
        private List<GameObject> activeTracers;
        public int tracerPoolCount;
        public int tracerFrequency = 7;

        private int leftTracer = 7;
        private int rightTracer = 1;
        private float tracerSpeed = 100.0f;
        public int damageBoost = 1;

        /// <summary>
        /// Initializes variables
        /// </summary>
        void Start()
        {
            damageBoost = 1;
            damage = shipController.minigunDamage;
            fireRate = shipController.minigunFireRate;
        }

        /// <summary>
        /// Initializes the tracer pool
        /// </summary>
        void Awake()
        {
            tracers = new List<GameObject>();
            activeTracers = new List<GameObject>();

            for (int i = 0; i < tracerPoolCount; ++i)
            {
                tracers.Add(Instantiate(tracerPrefab, new Vector3(0.0f, -100.0f, 0.0f), Quaternion.identity));
                tracers.Last().SetActive(false);
            }
        }

        /// <summary>
        /// Sent every frame while the trigger is pressed
        /// </summary>
        public override void TriggerUpdate()
        {

            if (canShoot)
            {
                Clicked();
            }
        }

        /// <summary>
        /// Gets enemy or wall aimpoint and shoots at it.
        /// </summary>
        private void Clicked()
        {
            RaycastHit hit;
            if (Physics.SphereCast(transform.position, radius, transform.forward, out hit, 200.0f, Utility.enemyMask))
            {
                StartCoroutine(Shoot(hit.collider.gameObject, hit.point));

                if (hit.collider.gameObject.GetComponent<Enemy>() != null)
                {
                    hit.collider.gameObject.GetComponent<Enemy>().Hit(damage * damageBoost);
                }

                shipController.hand.controller.TriggerHapticPulse(2500);
            }

            else if(Physics.Raycast(transform.position, transform.forward, out hit, 200.0f, Utility.shootableMask))
            {
                StartCoroutine(Shoot(hit.collider.gameObject, hit.point));

                if(hit.collider.gameObject.GetComponent<Shootable>() != null 
                    && hit.collider.gameObject.GetComponent<Shootable>().isShootable)
                {
                    hit.collider.gameObject.GetComponent<Shootable>().Shot();
                }

                shipController.hand.controller.TriggerHapticPulse(2500);
            }

            else if (Physics.Raycast(transform.position, transform.forward, out hit, 200.0f, Utility.roomMask))
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

            if (isLeftFire)
            {
                // left[Random.Range(0, left.Length)].transform.position;
                GameObject barrel = left[UnityEngine.Random.Range(0, left.Length)];

                // do muzzle flash
                //Instantiate(muzzleFlashPrefab, barrel.transform);

                // do tracer
                if (--leftTracer <= 0)
                {
                    StartCoroutine(Tracer(barrel, position));

                    if (leftTracer < 0)
                        leftTracer = tracerFrequency;
                }
            }
            else
            {
                // right[Random.Range(0, right.Length)].transform.position;
                GameObject barrel = right[UnityEngine.Random.Range(0, left.Length)];

                // do muzzle flash
                //Instantiate(muzzleFlashPrefab, barrel.transform);

                // do tracer
                if (--rightTracer <= 0)
                {
                    StartCoroutine(Tracer(barrel, position));

                    if (rightTracer < 0)
                        rightTracer = tracerFrequency;
                }
            }

            // do hit spark
            if (target.CompareTag("Enemy"))
            {
                //StartCoroutine(HitSpark(position, CalculateDelay(position)));
                HitSpark(position);
            }

            yield return new WaitForSeconds(1.0f / fireRate);

            isLeftFire = !isLeftFire;
            canShoot = true;
        }

        ///// <summary>
        ///// Calculates the delay for a "bullet" to "travel"
        ///// from the minigun to the target enemy.
        ///// </summary>
        ///// <param name="position"> The position of the shot </param>
        ///// <returns> How long it will take for the "bullet" to hit the target </returns>
        //private float CalculateDelay(Vector3 position)
        //{
        //    float distance = Vector3.Distance(shipController.gameObject.transform.position, position);

        //    return distance/tracerSpeed;
        //}

        ///// <summary>
        ///// Spawns a hit spark on target after a delay to emulate bullet travel.
        ///// </summary>
        ///// <param name="position"> The position of the hit </param>
        ///// <param name="delay"> How long to wait before spawning the hit spark </param>
        //private IEnumerator HitSpark(Vector3 position, float delay)
        //{
        //    yield return new WaitForSeconds(delay);

        //    GameObject spark = Instantiate(hitSparkPrefab, position, Quaternion.identity);
        //    spark.transform.Rotate(0.0f, 180.0f, 0.0f);
        //}

        /// <summary>
        /// Spawns a hit spark on target after a delay to emulate bullet travel.
        /// </summary>
        /// <param name="position"> The position of the hit </param>
        private void HitSpark(Vector3 position)
        {
            GameObject spark = Instantiate(hitSparkPrefab, position, Quaternion.identity);
            spark.transform.Rotate(0.0f, 180.0f, 0.0f);
        }

        /// <summary>
        /// Shoots a tracer at the target position
        /// </summary>
        /// <param name="barrel"> Which barrel to spawn the tracer from </param>
        /// <param name="target"> Where to aim the tracer </param>
        private IEnumerator Tracer(GameObject barrel, Vector3 target)
        {
            GameObject tracer = tracers.First();
            tracers.Remove(tracer);
            activeTracers.Add(tracer);

            // Enable tracer
            tracer.transform.position = barrel.transform.position + barrel.transform.forward * -0.3f;
            //tracer.transform.rotation = barrel.transform.rotation;
            tracer.transform.LookAt(target);
            tracer.SetActive(true);

            try
            {
                tracer.GetComponent<ParticleSystem>().Clear();
                tracer.GetComponent<ParticleSystem>().Play();
            }
            catch (Exception)
            {
                Debug.LogError("Minigun.cs - CAN'T GET TRACER PARTICLE SYSTEM");
            }

            // wait however long the tracer takes
            yield return new WaitForSeconds(0.5f);

            // Disable tracer
            tracer.SetActive(false);
            tracers.Add(tracer);
            activeTracers.Remove(tracer);
        }

        /// <summary>
        /// Deactivates and resets all tracers. Removes all from activeTracers.
        /// </summary>
        public void ResetTracers()
        {
            try
            {
                for (int i = activeTracers.Count - 1; i >= 0; i--)
                {
                    tracers.Add(activeTracers.ElementAt(i));
                    activeTracers.RemoveAt(i);
                }

                foreach (GameObject tracer in tracers)
                {
                    tracer.SetActive(false);
                    try
                    {
                        tracer.GetComponent<ParticleSystem>().Clear();
                    }
                    catch (Exception)
                    {
                        // do nothing
                    }
                }
            }
            catch (Exception)
            {
                // do nothing
            }
        }
    }
}
