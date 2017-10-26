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
using System.Collections.Generic;
using UnityEngine;
using Hive.Armada;
using Hive.Armada.Enemies;
using System;
using UnityEngine.Rendering;
using Valve.VR;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Hive.Armada.Player.Guns
{
    public class Minigun : Gun
    {
        public GameObject left;
        public GameObject right;
        public GameObject bulletPrefab;
        public float bulletSpeed;
        public float radius = 0.3f;
        private bool isLeftFire = true;


        void Start()
        {
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
        /// <returns>  </returns>
        private IEnumerator Shoot(Vector3 target)
        {
            canShoot = false;

            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            if (isLeftFire)
            {
                bullet.transform.position = left.transform.position;
            }
            else
            {
                bullet.transform.position = right.transform.position;
            }
            if (bullet.GetComponentInChildren<MinigunBullet>() != null)
                bullet.GetComponentInChildren<MinigunBullet>().hand = shipController.hand;

            bullet.transform.LookAt(target);

            if (bullet.GetComponentInChildren<Rigidbody>() != null)
            {
                bullet.GetComponentInChildren<Rigidbody>().velocity = bullet.transform.forward * bulletSpeed;
            }

            yield return new WaitForSeconds(1.0f / fireRate);

            isLeftFire = !isLeftFire;
            canShoot = true;
        }
    }
}
