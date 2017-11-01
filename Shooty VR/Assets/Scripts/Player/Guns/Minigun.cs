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

namespace Hive.Armada.Player.Guns
{
    /// <summary>
    /// The minigun controller for the player ship.
    /// </summary>
    public class Minigun : Gun
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

        private LineRenderer[] leftTracers;
        private LineRenderer[] rightTracers;
        public float thickness = 0.002f;
        public Material tracerMaterial;

        private bool leftSpark = true;
        private bool rightSpark = false;

        /// <summary>
        /// Initializes variables
        /// </summary>
        void Start()
        {
            leftTracers = new LineRenderer[left.Length];
            rightTracers = new LineRenderer[right.Length];

            for (int i = 0; i < left.Length; ++i)
            {
                leftTracers[i] = left[i].AddComponent<LineRenderer>();
                leftTracers[i].material = tracerMaterial;
                leftTracers[i].shadowCastingMode = ShadowCastingMode.Off;
                leftTracers[i].receiveShadows = false;
                leftTracers[i].alignment = LineAlignment.View;
                leftTracers[i].startWidth = thickness;
                leftTracers[i].endWidth = thickness;
                leftTracers[i].enabled = false;

                rightTracers[i] = right[i].AddComponent<LineRenderer>();
                rightTracers[i].material = tracerMaterial;
                rightTracers[i].shadowCastingMode = ShadowCastingMode.Off;
                rightTracers[i].receiveShadows = false;
                rightTracers[i].alignment = LineAlignment.View;
                rightTracers[i].startWidth = thickness;
                rightTracers[i].endWidth = thickness;
                rightTracers[i].enabled = false;
            }

            damage = shipController.laserDamage;
            fireRate = shipController.laserFireRate;
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
                StartCoroutine(Shoot(hit.collider.gameObject, hit.collider.gameObject.transform.position));

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

            //GameObject bullet = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

            //if (isLeftFire)
            //{
            //    bullet.transform.position = left[Random.Range(0, left.Length)].transform.position;
            //}
            //else
            //{
            //    bullet.transform.position = right[Random.Range(0, left.Length)].transform.position;
            //}

            //if (bullet.GetComponentInChildren<MinigunBullet>() != null)
            //    bullet.GetComponentInChildren<MinigunBullet>().hand = shipController.hand;

            //bullet.GetComponent<MinigunBullet>().damage = shipController.minigunDamage;
            //bullet.transform.LookAt(target);

            //if (bullet.GetComponentInChildren<Rigidbody>() != null)
            //{
            //    bullet.GetComponentInChildren<Rigidbody>().velocity = bullet.transform.forward * projectileSpeed;
            //}

            if (isLeftFire)
            {
                // left[Random.Range(0, left.Length)].transform.position;
                // do muzzle flash
                if (leftSpark)
                {
                    StartCoroutine(HitSpark(target, 0.1f));
                }

                leftSpark = !leftSpark;
            }
            else
            {
                // right[Random.Range(0, right.Length)].transform.position;
                // do muzzle flash
                if (rightSpark)
                {
                    // do hit spark
                }

                rightSpark = !rightSpark;
            }

            yield return new WaitForSeconds(1.0f / fireRate);

            isLeftFire = !isLeftFire;
            canShoot = true;
        }

        /// <summary>
        /// Calculates the delay for a "bullet" to "travel"
        /// from the minigun to the target enemy.
        /// </summary>
        /// <returns></returns>
        private float CalculateDelay()
        {
            return 0.0f;
        }

        /// <summary>
        /// Spawns a hit spark on target after a delay to emulate bullet travel.
        /// </summary>
        /// <param name="target"> The GameObject that is being hit </param>
        /// <param name="delay"> How long to wait before spawning the hit spark </param>
        private IEnumerator HitSpark(GameObject target, float delay)
        {
            yield return new WaitForSeconds(delay);

            // spawn the spark on target
        }

        /// <summary>
        /// Shoots a tracer at the target position
        /// </summary>
        /// <param name="barrel"> Which barrel to spawn the tracer from </param>
        /// <param name="target"> Where to aim the tracer </param>
        private IEnumerator Tracer(GameObject barrel, Vector3 target)
        {
            yield return new WaitForSeconds(0.1f);
        }
    }
}
