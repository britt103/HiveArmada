using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

//Name: Miguel Luis Gotao
//Student ID: 2264941
//Email: gotao100@mail.chapman.edu
//Course: CPSC340-01
//Game Development Project

namespace Hive.Armada.Enemies
{
    /// <summary>
    /// Script enabling basic turret behavior that
    /// shoots directly at the player character.
    /// </summary>
    public class StraightTurret : Enemy
    {
        public GameObject bullet;
        public Transform spawn;
        GameObject player;
        public Vector3 pos;
        public float fireRate;
        public float fireSpeed;
        public float fireCone;
        private float fireNext;
        private float randX;
        private float randY;
        private float randZ;

        // Update is called once per frame
        void Update()
        {
            
            if (player != null)
            {
                randX = Random.Range(-fireCone, fireCone);
                randY = Random.Range(-fireCone, fireCone);
                randZ = Random.Range(-fireCone, fireCone);

                pos = player.transform.position;        //tracks the player position
                transform.LookAt(pos);                  //and makes the transform look at said position

                if (Time.time > fireNext)
                {                                                                                       //Basic firerate calculation that determines
                    fireNext = Time.time + (1 / fireRate);                                              //how many projectiles shoot out of the turret
                    var shoot = Instantiate(bullet, spawn.position, spawn.rotation);                    //within a certain duration. The turret then
                    shoot.GetComponent<Transform>().Rotate(randX, randY, randZ);
                    shoot.GetComponent<Rigidbody>().velocity = shoot.transform.forward * fireSpeed;     //instantiates a bullet and shoots it forward
                }                                                                                       //in the direction of the player.
            }
            else
            {
                player = GameObject.FindGameObjectWithTag("Player");

                if (player == null)
                {
                    transform.LookAt(new Vector3(0.0f, 0.0f, 0.0f));
                }
            }
        }

        protected override void Reset()
        {
            throw new NotImplementedException();
        }
    }
}

