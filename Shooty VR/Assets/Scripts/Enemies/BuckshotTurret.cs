using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

//Name: Miguel Luis Gotao
//Student ID: 2264941
//Email: gotao100@mail.chapman.edu
//Course: CPSC340-01
//Game Development Project 01

namespace Hive.Armada.Enemies
{
    /// <summary>
    /// Script in charge of a shotgun-like spread turret
    /// </summary>
    public class BuckshotTurret : Enemy
    {
        public GameObject bullet;

        public Transform spawn;

        private GameObject player;

        private Vector3 pos;

        public float fireRate;

        public float fireSpeed;

        public float fireCone;

        public float firePellet;

        private float fireNext;

        private float randX;

        private float randY;

        private float randZ;

        private bool canFire;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            pos = new Vector3(player.transform.position.x, player.transform.position.y,
                player.transform.position.z);
        }

        private void Update()
        {
            try
            {
                pos = player.transform.position;
                transform.LookAt(pos);

                if (Time.time > fireNext)
                {
                    fireNext = Time.time + fireRate;

                    for (int i = 0; i < firePellet; ++i)
                    {
                        StartCoroutine(fireBullet());
                    }
                }
            }
            catch (Exception e)
            {
                //    //Debug.Log("Player is dead");
            }
        }

        private IEnumerator fireBullet()
        {
            GameObject shoot = Instantiate(bullet, spawn.position, spawn.rotation);
            randX = Random.Range(-fireCone, fireCone);
            randY = Random.Range(-fireCone, fireCone);
            randZ = Random.Range(-fireCone, fireCone);

            shoot.GetComponent<Transform>().Rotate(randX, randY, randZ);
            shoot.GetComponent<Rigidbody>().velocity = shoot.transform.forward * fireSpeed;
            yield break;
        }

        protected override void Reset()
        {
            throw new NotImplementedException();
        }
    }
}