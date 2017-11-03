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

using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using Hive.Armada;
using Hive.Armada.Enemies;
using System.Collections.Generic;
using System.Linq;

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

        //private LineRenderer[] leftTracers;
        //private LineRenderer[] rightTracers;
        //public float thickness = 0.002f;
        //public Material tracerMaterial;

        private List<GameObject> tracers;
        private List<GameObject> activeTracers;
        public int tracerPoolCount;
        public int tracerFrequency = 7;

        private bool leftSpark = true;
        private bool rightSpark;
        private int leftTracer = 7;
        private int rightTracer = 1;
        private float tracerSpeed = 100.0f;

        /// <summary>
        /// Initializes variables
        /// </summary>
        void Start()
        {
            //leftTracers = new LineRenderer[left.Length];
            //rightTracers = new LineRenderer[right.Length];

            //for (int i = 0; i < left.Length; ++i)
            //{
            //    leftTracers[i] = left[i].AddComponent<LineRenderer>();
            //    leftTracers[i].material = tracerMaterial;
            //    leftTracers[i].shadowCastingMode = ShadowCastingMode.Off;
            //    leftTracers[i].receiveShadows = false;
            //    leftTracers[i].alignment = LineAlignment.View;
            //    leftTracers[i].startWidth = thickness;
            //    leftTracers[i].endWidth = thickness;
            //    leftTracers[i].enabled = false;

            //    rightTracers[i] = right[i].AddComponent<LineRenderer>();
            //    rightTracers[i].material = tracerMaterial;
            //    rightTracers[i].shadowCastingMode = ShadowCastingMode.Off;
            //    rightTracers[i].receiveShadows = false;
            //    rightTracers[i].alignment = LineAlignment.View;
            //    rightTracers[i].startWidth = thickness;
            //    rightTracers[i].endWidth = thickness;
            //    rightTracers[i].enabled = false;
            //}

            damage = shipController.minigunDamage;
            fireRate = shipController.minigunFireRate;
        }

        void Awake()
        {
            tracers = new List<GameObject>();
            activeTracers = new List<GameObject>();

            for (int i = 0; i < tracerPoolCount; ++i)
            {
                tracers.Add(Instantiate(tracerPrefab, gameObject.transform));
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
                tracers.First().GetComponent<ParticleSystem>().Clear();
                tracers.First().GetComponent<ParticleSystem>().Play();
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
                    hit.collider.gameObject.GetComponent<Enemy>().Hit(damage);
                }
            }

            //else if (Physics.SphereCast(transform.position, radius, transform.forward, out hit, 200.0F, Utility.uiMask))
            //{
            //    StartCoroutine(Shoot(hit.collider.gameObject.transform.position, hit.collider.gameObject));
            //    if (hit.collider.gameObject.GetComponent<ShootableUI>() != null)
            //    {
            //        hit.collider.gameObject.GetComponent<ShootableUI>().shot();
            //    }

            //}


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
                GameObject barrel = left[Random.Range(0, left.Length)];

                // do muzzle flash
                //Instantiate(muzzleFlashPrefab, barrel.transform);

                // do tracer
                if (--leftTracer <= 0)
                {
                    StartCoroutine(Tracer(barrel, position));

                    if (leftTracer < 0)
                        leftTracer = tracerFrequency;
                }

                // do hit spark
                if (leftSpark && target.CompareTag("Enemy"))
                {
                    StartCoroutine(HitSpark(target, position, CalculateDelay(position)));
                }

                //leftSpark = !leftSpark;
            }
            else
            {
                // right[Random.Range(0, right.Length)].transform.position;
                GameObject barrel = right[Random.Range(0, left.Length)];

                // do muzzle flash
                //Instantiate(muzzleFlashPrefab, barrel.transform);

                // do tracer
                if (--rightTracer <= 0)
                {
                    StartCoroutine(Tracer(barrel, position));

                    if (rightTracer < 0)
                        rightTracer = tracerFrequency;
                }

                // do hit spark
                if (rightSpark && target.CompareTag("Enemy"))
                {
                    StartCoroutine(HitSpark(target, position, CalculateDelay(position)));
                }

                //rightSpark = !rightSpark;
            }

            yield return new WaitForSeconds(1.0f / fireRate);

            isLeftFire = !isLeftFire;
            canShoot = true;
        }

        /// <summary>
        /// Calculates the delay for a "bullet" to "travel"
        /// from the minigun to the target enemy.
        /// </summary>
        /// <param name="position"> The position of the shot </param>
        /// <returns> How long it will take for the "bullet" to hit the target </returns>
        private float CalculateDelay(Vector3 position)
        {
            float distance = Vector3.Distance(shipController.gameObject.transform.position, position);

            return distance/tracerSpeed;
        }

        /// <summary>
        /// Spawns a hit spark on target after a delay to emulate bullet travel.
        /// </summary>
        /// <param name="target"> The GameObject that is being hit </param>
        /// <param name="position"> The position of the hit </param>
        /// <param name="delay"> How long to wait before spawning the hit spark </param>
        private IEnumerator HitSpark(GameObject target, Vector3 position, float delay)
        {
            yield return new WaitForSeconds(delay);

            GameObject spark = Instantiate(hitSparkPrefab, position, Quaternion.identity, target.transform);
            spark.transform.Rotate(0.0f, 180.0f, 0.0f);

            //if (target.CompareTag("Enemy"))
            //{
            //    Instantiate(hitSparkPrefab, position, Quaternion.identity, target.transform);
            //}
            //else if (target.CompareTag("Room"))
            //{
            //    Instantiate(hitSparkPrefab, position, Quaternion.identity, target.transform);
            //}
        }

        /// <summary>
        /// Shoots a tracer at the target position
        /// </summary>
        /// <param name="barrel"> Which barrel to spawn the tracer from </param>
        /// <param name="target"> Where to aim the tracer </param>
        private IEnumerator Tracer(GameObject barrel, Vector3 target)
        {
            // Enable tracer

            // wait however long the tracer takes
            yield return new WaitForSeconds(0.1f);

            // Disable tracer
        }
    }
}
