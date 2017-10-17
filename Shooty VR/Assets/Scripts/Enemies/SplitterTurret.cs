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

namespace GameName.Enemies
{
    public class SplitterTurret : Enemy
    {
        public GameObject bullet;
        public Transform spawn;
        GameObject player;
        GameObject turret; /// set reference in inspector
        public Vector3 pos;
        public float fireRate, fireSpeed, fireCone;
        private float fireNext, randX, randY, randZ;
        bool canFire;
        public Vector2 splitVel1; ///Set value in inspector
        public Vector2 splitVel2; ///Set value in inspector
        public Vector2 splitVel3; ///Set value in inspector
        public Vector2 splitVel4; ///Set value in inspector


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
                randX = Random.Range(-fireCone, fireCone);
                randY = Random.Range(-fireCone, fireCone);
                randZ = Random.Range(-fireCone, fireCone);

                pos = player.transform.position;        //tracks the player position
                transform.LookAt(pos);                  //and makes the transform look at said position

                if (Time.time > fireNext)
                {                                                                                       //Basic firerate calculation that determines
                    fireNext = Time.time + fireRate;                                                    //how many projectiles shoot out of the turret
                    var shoot = Instantiate(bullet, spawn.position, spawn.rotation);                    //within a certain duration. The turret then
                    shoot.GetComponent<Transform>().Rotate(randX, randY, randZ);
                    shoot.GetComponent<Rigidbody>().velocity = shoot.transform.forward * fireSpeed;     //instantiates a bullet and shoots it forward
                }                                                                                       //in the direction of the player.
            }
        }
        protected override void Kill()
        {
            if (turret != null)
            {
                //Instantiate("Explosion.name", transform.position, transform.rotation);
                GameObject smlSplt1 = Instantiate(turret, transform.position, transform.rotation);
                GameObject smlSplt2 = Instantiate(turret, transform.position, transform.rotation);
                GameObject smlSplt3 = Instantiate(turret, transform.position, transform.rotation);
                GameObject smlSplt4 = Instantiate(turret, transform.position, transform.rotation);

                smlSplt1.GetComponent<Rigidbody>().velocity = splitVel1;
                smlSplt2.GetComponent<Rigidbody>().velocity = splitVel2;
                smlSplt3.GetComponent<Rigidbody>().velocity = splitVel3;
                smlSplt4.GetComponent<Rigidbody>().velocity = splitVel4;
            }

            Destroy(gameObject);

        }

    }
}
