//
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-340-01
// Project 1
// 
// 
// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project1
{
    public class EnemyScout : Destructible
    {
        private GameObject player;
        private Transform myTransform;

        // Use this for initialization
        void Start()
        {
            health = maxHealth;
            player = GameObject.FindWithTag("Player");
            laserParent = GameObject.FindGameObjectWithTag("ProjectileParent").transform;
            explosionParent = GameObject.FindGameObjectWithTag("ExplosionParent").transform;
        }

        void Awake()
        {
            myTransform = transform.parent.transform;
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 target = new Vector3(player.transform.position.x, 0.0f, player.transform.position.z);

            myTransform.rotation = Quaternion.Slerp(myTransform.rotation,
                Quaternion.LookRotation(target - myTransform.position), turnSpeed * Time.deltaTime);

            myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
        }

        public override void Hit(int damage)
        {
            //health -= damage;
            //if (health <= 0)
            if ((health -= damage) <- 0)
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
            var explosion = Instantiate(explosionPrefab, transform.position, transform.rotation, explosionParent);
            Destroy(explosion, 2.0f);
            Destroy(transform.parent.gameObject);
        }
    }
}