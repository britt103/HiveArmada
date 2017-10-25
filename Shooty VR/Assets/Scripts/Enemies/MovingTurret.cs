using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// /// Ryan Britton
/// britt103
/// #1849351
/// CPSC-340-01, CPSC-344-01
/// Group Project
/// 
/// This script handles the logic of a turret moving between two points
/// </summary>
namespace Hive.Armada.Enemies
{
    public class MovingTurret : Enemy
    {
        public float movingSpeed;
        public float xMax;
        public float yMax;
        private Vector3 posA;
        private Vector3 posB;
        private Vector3 posCenter;
        public Transform spawn;
        private float xEnd;
        public GameObject bullet;
        private GameObject player;
        public Vector3 pos;
        public float fireRate, fireSpeed;
        private float fireNext;
        bool canFire;
        private float distance;
        private float startTime;

        // Use this for initialization
        void Start()
        {
            startTime = Time.time;
            player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                transform.LookAt(player.transform);
            }
            SetPosition();
        }

        void SetPosition()
        {
            //This sets the starting position and the 2 points the turrets moves between
            pos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
            posA = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            posB = new Vector3(transform.localPosition.x + xMax, transform.localPosition.y + yMax, transform.position.z);
            //posCenter = new Vector3(posA.x + posB.x, posA.y + posB.y) / 2.0f;
            //transform.position = posCenter;

        }
        private void Update()
        {
            transform.position = Vector3.Lerp(posA, posB, (Mathf.Sin(movingSpeed * (Time.time + startTime)) + 1.0f) / 2.0f);

            if (player != null)
            {
                transform.LookAt(player.transform.position); //and makes the transform look at said position
            }
            else
            {
                player = GameObject.FindGameObjectWithTag("Player");

                if (player == null)
                {
                    transform.LookAt(new Vector3(0.0f, 0.0f, 0.0f));
                }
            }

            if (Time.time > fireNext)
            {                                                                                       //Basic firerate calculation that determines
                fireNext = Time.time + fireRate;                                                    //how many projectiles shoot out of the turret
                var shoot = Instantiate(bullet, spawn.position, spawn.rotation);                    //within a certain duration. The turret then
                shoot.GetComponent<Rigidbody>().velocity = shoot.transform.forward * fireSpeed;     //instantiates a bullet and shoots it forward
            }                                                                                       //in the direction of the player.
        }

    }
}
