/// <summary>
/// Ryan Britton
/// britt103
/// #1849351
/// CPSC-340-01, CPSC-344-01
/// Group Project
/// 
/// This script handles the splitter turret that spawns 4 regular turret enemies when it is destroyed
/// </summary>

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hive.Armada.Enemies
{
    public class SplitterTurret : Enemy
    {
        public GameObject bullet;
        public Transform spawn;
        private GameObject player;
        public GameObject turret; /// set reference PREFAB in inspector
        public float fireRate, fireSpeed;
        private float fireNext, randX, randY, randZ;
        bool canFire;
        public float splitDir;

        ///Constantly tracks the player position
        ///While shooting bullets using the formula below
        void Update()
        {
            if (player != null)
            {
                transform.LookAt(player.transform.position);                  //makes the transform look at the player's position

                if (Time.time > fireNext)
                {                                                                                       //Basic firerate calculation that determines
                    fireNext = Time.time + fireRate;                                                    //how many projectiles shoot out of the turret
                    var shoot = Instantiate(bullet, spawn.position, spawn.rotation);                    //within a certain duration. The turret then
                    shoot.GetComponent<Rigidbody>().velocity = shoot.transform.forward * fireSpeed;     //instantiates a bullet and shoots it forward
                }
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

        protected override void Kill()
        {
            reference.spawner.AddKill();
            if (turret != null)
            {
                Vector3 splitDir1 = new Vector3(transform.localPosition.x, transform.localPosition.y + splitDir, transform.localPosition.z);
                Vector3 splitDir2 = new Vector3(transform.localPosition.x, transform.localPosition.y - splitDir, transform.localPosition.z);
                Vector3 splitDir3 = new Vector3(transform.localPosition.x + splitDir, transform.localPosition.y, transform.localPosition.z);
                Vector3 splitDir4 = new Vector3(transform.localPosition.x - splitDir, transform.localPosition.y, transform.localPosition.z);

                //Instantiate("Explosion.name", transform.position, transform.rotation); Placeholder for destroy effect
                Instantiate(turret, splitDir1, transform.rotation); //Creates 4 instances of the Turret prefab set in Inspector
                Instantiate(turret, splitDir2, transform.rotation);
                Instantiate(turret, splitDir3, transform.rotation);
                Instantiate(turret, splitDir4, transform.rotation);
                
            }

            Destroy(gameObject);

        }

        protected override void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
