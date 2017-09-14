//
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-340-01
// Project 1
// 
// This class is the controller for the player's ship. It handles
// the input, movement, weapons, and life of the player ship.
// 

using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project1
{
    public class PlayerController : Destructible
    {
        public GameObject laserPrefab;
        public Transform laserSpawn;
        public float laserSpeed = 30.0f;
        [Tooltip("How many shots the ship can shoot per second.")]
        public float firerate = 10.0f;
        private bool canShoot = true;
        //private GameObject ship;

        // Use this for initialization
        void Start()
        {
            health = maxHealth;
            laserParent = GameObject.FindGameObjectWithTag("ProjectileParent").transform;
            explosionParent = GameObject.FindGameObjectWithTag("ExplosionParent").transform;
            //ship = gameObject;
        }

        // Update is called once per frame
        void Update()
        {
            var keyboardMove = Input.GetAxis("Horizontal");
            var keyboardRotate = Input.GetAxis("Vertical");

            // multiplied by -1 because ship is rotated to aim the correct direction
            transform.position += -1 * transform.up * keyboardRotate * moveSpeed * Time.deltaTime;
            transform.Rotate(0.0f, 0.0f, keyboardMove * turnSpeed * Time.deltaTime);

            if (Input.GetKey(KeyCode.Mouse0) && canShoot)
            {
                Clicked();
            }
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

            var laser = Instantiate(laserPrefab, laserSpawn.position, Quaternion.identity, laserParent);
            Vector3 fixedTarget = new Vector3(target.x, laserSpawn.transform.position.y, target.z);
            laser.transform.LookAt(fixedTarget);
            laser.GetComponent<Rigidbody>().velocity = laser.transform.forward * laserSpeed;
            Destroy(laser, 6.0f);

            yield return new WaitForSeconds(1.0f / firerate);
            canShoot = true;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("EnemyScout"))
            {
                Debug.Log("GAME OVER");
                other.GetComponent<EnemyScout>().Collide();
                Collide();
            }
            else if (other.CompareTag("DeathWall"))
            {
                Debug.Log("GAME OVER");
                Kill();
            }
        }

        public override void Hit(int damage)
        {
            //health -= damage;

            if ((health -= damage) <= 0)
            {
                Kill();
            }
        }

        public override void Collide()
        {
            Kill();
        }

        protected override void Kill()
        {
            var explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity, explosionParent);
            Destroy(explosion, 2.0f);
            Destroy(gameObject);
        }
    }
}