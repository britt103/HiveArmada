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
        public GameObject turret; /// set reference PREFAB in inspector
        public Vector3 pos;
        public Vector3 thisPos;
        public float fireRate, fireSpeed, fireCone;
        private float fireNext, randX, randY, randZ;
        bool canFire;
        public Vector2 splitDir1; ///Set value in inspector
        public Vector2 splitDir2; ///Set value in inspector
        public Vector2 splitDir3; ///Set value in inspector
        public Vector2 splitDir4; ///Set value in inspector


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
            if (turret != null)
            {
                //Instantiate("Explosion.name", transform.position, transform.rotation); Placeholder for destroy effect
                GameObject smlSplt1 = Instantiate(turret, transform.position, transform.rotation) as GameObject; //Creates 4 instances of the Turret prefab set in Inspector and creates 4 copies
                GameObject smlSplt2 = Instantiate(turret, transform.position, transform.rotation) as GameObject;
                GameObject smlSplt3 = Instantiate(turret, transform.position, transform.rotation) as GameObject;
                GameObject smlSplt4 = Instantiate(turret, transform.position, transform.rotation) as GameObject;

                smlSplt1.transform.position = new Vector3(transform.position.x + splitDir1.x, transform.position.y + splitDir1.y, transform.position.z); //Places each new turret at distances relative to the destroyed Splitter Turret
                smlSplt2.transform.position = new Vector3(transform.position.x + splitDir2.x, transform.position.y + splitDir2.y, transform.position.z); //Values for each are set in Inspector
                smlSplt3.transform.position = new Vector3(transform.position.x + splitDir3.x, transform.position.y + splitDir3.y, transform.position.z); 
                smlSplt4.transform.position = new Vector3(transform.position.x + splitDir4.x, transform.position.y + splitDir4.y,transform.position.z); 
            }

            Destroy(gameObject);

        }

    }
}
