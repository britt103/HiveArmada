/// <summary>
/// Ryan Britton
/// britt103
/// #1849351
/// CPSC-340-01, CPSC-344-01
/// Group Project
/// 
/// This script handles the splitter turret that spawns 4 regular turret enemies when it is destroyed
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hive.Armada.Enemies
{
    public class SplitterTurret : Enemy
    {
        public GameObject bullet;
        public Transform spawn;
        GameObject player;
        public GameObject turret; /// set reference PREFAB in inspector
        private Vector3 pos;
        public float fireRate, fireSpeed;
        private float fireNext, randX, randY, randZ;
        bool canFire;
        public float splitDir;


        /// Use this for initialization
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            pos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
           
        }
        ///Constantly tracks the player position
        ///While shooting bullets using the formula below
        void Update()
        {
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player");

                if (player != null)
                {
                    pos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
                }
            }
            else
            {
                pos = player.transform.position;        //tracks the player position
                transform.LookAt(pos);                  //and makes the transform look at said position

                if (Time.time > fireNext)
                {                                                                                       //Basic firerate calculation that determines
                    fireNext = Time.time + fireRate;                                                    //how many projectiles shoot out of the turret
                    var shoot = Instantiate(bullet, spawn.position, spawn.rotation);                    //within a certain duration. The turret then
                    shoot.GetComponent<Rigidbody>().velocity = shoot.transform.forward * fireSpeed;     //instantiates a bullet and shoots it forward
                }
            }
        }
        protected override void Kill()
        {
            spawner.AddKill();
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

    }
}
