// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
// 
// This class is for the player's laser guns.
// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using GameName;
using GameName.Enemies;
using UnityEngine.Networking;

namespace GameName.Player
{
    public class PlayerLaser : MonoBehaviour
    {
        public GameObject ship;
        private ShipController controller;
        public GameObject left;
        public GameObject right;
        public Material laserMaterial;
        private LineRenderer leftLaser;
        private LineRenderer rightLaser;
        public float firerate = 10.0f;
        public int damage = 10;
        public float radius = 0.3f;
        [Tooltip("View makes line face camera. Local makes the line face the direction of the transform component")]
        public LineAlignment alignment;
        public float thickness = 0.002f;
        public ShadowCastingMode castShadows;
        public bool receiveShadows = false;
        private bool canShoot = true;
        private bool isLeftFire = true;

        /// <summary>
        /// Initializes the lasers' linerenderers.
        /// </summary>
        void Start()
        {
            controller = ship.GetComponent<ShipController>();

            leftLaser = left.gameObject.AddComponent<LineRenderer>();
            leftLaser.material = laserMaterial;
            leftLaser.shadowCastingMode = castShadows;
            leftLaser.receiveShadows = receiveShadows;
            leftLaser.alignment = alignment;
            leftLaser.startWidth = thickness;
            leftLaser.endWidth = thickness;
            leftLaser.enabled = false;

            rightLaser = right.gameObject.AddComponent<LineRenderer>();
            rightLaser.material = laserMaterial;
            rightLaser.shadowCastingMode = castShadows;
            rightLaser.receiveShadows = receiveShadows;
            rightLaser.alignment = alignment;
            rightLaser.startWidth = thickness;
            rightLaser.endWidth = thickness;
            rightLaser.enabled = false;
        }

        /// <summary>
        /// Shoots when trigger is pressed and the ship can shoot.
        /// </summary>
        void Update()
        {
            if (controller.isTriggerPressed && canShoot)
            {
                Clicked();
            }
        }

        /// <summary>
        /// Gets enemy or wall aimpoint and shoots at it. Will damage enemies.
        /// </summary>
        private void Clicked()
        {
            RaycastHit hit;
            if (Physics.SphereCast(transform.position, radius, transform.forward, out hit, 200.0f, Utility.enemyMask))
            {
                StartCoroutine(Shoot(hit.collider.gameObject.transform.position, hit.collider.gameObject));
                if (hit.collider.gameObject.GetComponent<EnemyBasic>() != null)
                {
                    hit.collider.gameObject.GetComponent<EnemyBasic>().Hit(damage);
                }
            }
            else if (Physics.Raycast(transform.position, transform.forward, out hit, 200.0f, Utility.roomMask))
            {
                StartCoroutine(Shoot(hit.point, hit.collider.gameObject));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target">  </param>
        /// <param name="enemy">  </param>
        /// <returns>  </returns>
        private IEnumerator Shoot(Vector3 target, GameObject enemy)
        {
            canShoot = false;

            leftLaser.SetPosition(0, left.transform.position);
            leftLaser.SetPosition(1, target);
            rightLaser.SetPosition(0, right.transform.position);
            rightLaser.SetPosition(1, target);
            StartCoroutine(FlashLaser(isLeftFire));

            yield return new WaitForSeconds(1.0f / firerate);

            isLeftFire = !isLeftFire;
            canShoot = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isLeft">  </param>
        /// <returns>  </returns>
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

            yield return new WaitForSeconds(0.006f);

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
