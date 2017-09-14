using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

namespace VRTK.Examples
{
    using UnityEngine;

    public class Gun : VRTK_InteractableObject
    {
        private GameObject bullet;
        private float bulletSpeed = 10000f;
        private float bulletLife = 5f;
        public bool isTriggerPressed = false;

        public GameObject laserPrefab;
        public Transform laserSpawn;
        public float laserSpeed = 30.0f;
        [Tooltip("How many shots the ship can shoot per second.")]
        public float firerate = 10.0f;
        public bool canShoot = true;

        public override void StartUsing(VRTK_InteractUse usingObject)
        {
            base.StartUsing(usingObject);
            StartDeath();
        }

        public override void StopUsing(VRTK_InteractUse usingObject)
        {
            base.StopUsing(usingObject);
            StopDeath();
        }

        protected void Start()
        {
            bullet = transform.Find("Bullet").gameObject;
            bullet.SetActive(false);
        }

        private void StartDeath()
        {
            isTriggerPressed = true;
        }

        private void StopDeath()
        {
            isTriggerPressed = false;
        }

        private void Clicked()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hit = new RaycastHit();
            var mask = LayerMask.GetMask("MousePlane");
            if (Physics.Raycast(ray, out hit, 1000.0f, mask))
            {
                StartCoroutine(Fire(hit.point));
            }
        }

        private IEnumerator Fire(Vector3 target)
        {
            canShoot = false;

            Vector3 newPosition = new Vector3(laserSpawn.position.x + Random.Range(-0.5f, 0.5f),
                laserSpawn.position.y + Random.Range(-0.5f, 0.5f),
                laserSpawn.position.z + Random.Range(-0.5f, 0.5f));

            var laser = Instantiate(laserPrefab, newPosition, Quaternion.identity);
            //Vector3 fixedTarget = new Vector3(target.x, laserSpawn.transform.position.y, target.z);
            laser.transform.LookAt(target);
            laser.GetComponent<Rigidbody>().velocity = laser.transform.forward * laserSpeed;
            Destroy(laser, 6.0f);

            yield return new WaitForSeconds(1.0f / firerate);
            canShoot = true;
        }

        //// Update is called once per frame
        //void Update()
        //{
        //    if (isTriggerPressed && canShoot)
        //    {
        //        Clicked();
        //    }
        //}
    }
}