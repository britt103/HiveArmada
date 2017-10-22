using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hive.Armada.Enemies;
/// <summary>
/// /// Ryan Britton
/// britt103
/// #1849351
/// CPSC-340-01, CPSC-344-01
/// Group Project
/// 
/// This script handles the logic of a turret teleporting between two points
/// </summary>
namespace Hive.Armada.Enemies
{
    public class JumperTurret : Enemy
    {
        public float xMax;
        public float yMax;
        public float zMax;
        private Vector3 posA;
        private Vector3 posB;
        public Transform spawn;
        bool perry;
        private IEnumerator coroutine;
        public float timer;
        //public float timer;
        public GameObject bullet;
        private GameObject player;
        public Vector3 pos;
        public float fireRate, fireSpeed;
        private float fireNext;
        bool canFire;
        private float distance;


        // Use this for initialization
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            SetPosition();
            perry = true;
            coroutine = JumpFlash(timer);
            StartCoroutine(coroutine);
        }
        //void Awake()
        //{
        //    health = maxHealth;
        //    material = gameObject.GetComponent<Renderer>().material;
        //}
        void SetPosition()
        {
            //This sets the starting position and the 2 points the turrets teleports between
            pos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
            posA = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            posB = new Vector3(transform.position.x + xMax, transform.position.y + yMax, transform.position.z + zMax);

        }
        private void Update()
        {
            pos = player.transform.position;        //tracks the player position
            transform.LookAt(pos);                  //and makes the transform look at said position

            if (Time.time > fireNext)
            {                                                                                       //Basic firerate calculation that determines
                fireNext = Time.time + fireRate;                                                    //how many projectiles shoot out of the turret
                var shoot = Instantiate(bullet, spawn.position, spawn.rotation);                    //within a certain duration. The turret then
                shoot.GetComponent<Rigidbody>().velocity = shoot.transform.forward * fireSpeed;     //instantiates a bullet and shoots it forward
            }                                                                                    //in the direction of the player.
        }
        private IEnumerator JumpFlash(float waitTime)
        {
            while (true)
            {
                gameObject.GetComponent<Renderer>().material = flashColor;
                yield return new WaitForSeconds(1.0f);

                if (perry == true)
                {
                    transform.position = posB;
                    perry = false;
                }
                else
                {
                    transform.position = posA;
                    perry = true;
                }
                gameObject.GetComponent<Renderer>().material = material;
                yield return new WaitForSeconds(waitTime);
            }
        }

    }
}