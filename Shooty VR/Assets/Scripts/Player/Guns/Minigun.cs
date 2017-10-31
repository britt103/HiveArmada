//=============================================================================
// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
// 
// [DESCRIPTION]
// 
//=============================================================================

using System.Collections;
using UnityEngine;
using Hive.Armada;
using Hive.Armada.Enemies;
using UnityEngine.Rendering;

namespace Hive.Armada.Player.Guns
{
    /// <summary>
    /// 
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

        public LineRenderer[] leftTracers;
        public LineRenderer[] rightTracers;
        [Tooltip("View makes line face camera. Local makes the line face the direction of the transform component")]
        public LineAlignment alignment;
        public float thickness = 0.002f;
        public ShadowCastingMode castShadows;
        public bool receiveShadows = false;
        public Material tracerMaterial;

        void Start()
        {
            leftTracers = new LineRenderer[left.Length];
            rightTracers = new LineRenderer[right.Length];

            for (int i = 0; i < left.Length; ++i)
            {
                leftTracers[i] = left[i].AddComponent<LineRenderer>();
                leftTracers[i].material = tracerMaterial;
                leftTracers[i].shadowCastingMode = castShadows;
                leftTracers[i].receiveShadows = receiveShadows;
                leftTracers[i].alignment = alignment;
                leftTracers[i].startWidth = thickness;
                leftTracers[i].endWidth = thickness;
                leftTracers[i].enabled = false;

                rightTracers[i] = right[i].AddComponent<LineRenderer>();
                rightTracers[i].material = tracerMaterial;
                rightTracers[i].shadowCastingMode = castShadows;
                rightTracers[i].receiveShadows = receiveShadows;
                rightTracers[i].alignment = alignment;
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
                StartCoroutine(Shoot(hit.collider.gameObject.transform.position));

                //if (hit.collider.gameObject.GetComponent<EnemyBasic>() != null)
                //{
                //    hit.collider.gameObject.GetComponent<EnemyBasic>().Hit(damage);
                //}
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
                StartCoroutine(Shoot(hit.point));
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target">  </param>
        private IEnumerator Shoot(Vector3 target)
        {
            canShoot = false;

            GameObject bullet = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

            if (isLeftFire)
            {
                bullet.transform.position = left[Random.Range(0, left.Length)].transform.position;
            }
            else
            {
                bullet.transform.position = right[Random.Range(0, left.Length)].transform.position;
            }

            if (bullet.GetComponentInChildren<MinigunBullet>() != null)
                bullet.GetComponentInChildren<MinigunBullet>().hand = shipController.hand;

            bullet.GetComponent<MinigunBullet>().damage = shipController.minigunDamage;
            bullet.transform.LookAt(target);

            if (bullet.GetComponentInChildren<Rigidbody>() != null)
            {
                bullet.GetComponentInChildren<Rigidbody>().velocity = bullet.transform.forward * projectileSpeed;
            }

            yield return new WaitForSeconds(1.0f / fireRate);

            isLeftFire = !isLeftFire;
            canShoot = true;
        }
    }
}
