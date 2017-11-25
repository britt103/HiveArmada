//=============================================================================
//
// Miguel Gotao
// 2264941
// gotao100@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// Enemy behavior that shoots a shotgun-like buckshot
//
//=============================================================================

using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Hive.Armada.Enemies
{
    /// <summary>
    /// Script in charge of a shotgun-like spread turret
    /// </summary>
    public class BuckshotTurret : Enemy
    {
        /// <summary>
        /// Type identifier for object pooling purposes
        /// </summary>
        private int projectileTypeIdentifier;

        /// <summary>
        /// Projectile that the turret shoots out
        /// </summary>
        public GameObject fireProjectile;

        /// <summary>
        /// Position from which bullets are initially shot from
        /// </summary>
        public Transform fireSpawn;

        /// <summary>
        /// Variable that finds the player GameObject
        /// </summary>
        private GameObject player;

        /// <summary>
        /// Vector3 that holds the player's position
        /// </summary>
        private Vector3 pos;

        /// <summary>
        /// How fast the turret shoots at a given rate
        /// </summary>
        public float fireRate;

        /// <summary>
        /// The rate at which enemy projectiles travel
        /// </summary>
        public float fireSpeed;

        /// <summary>
        /// Size of conical spread the bullets travel within
        /// </summary>
        public float fireCone;

        /// <summary>
        /// Number of projectiles that the bullet shoots out at once
        /// </summary>
        public float firePellet;

        /// <summary>
        /// Value that calculates the next time at which the enemy is able to shoot again
        /// </summary>
        private float fireNext;

        /// <summary>
        /// Spread values determined by fireCone on each axis
        /// </summary>
        private float randX;
        private float randY;
        private float randZ;

        /// <summary>
        /// Finds the player and instantiates pos for position holding
        /// </summary>
        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            pos = new Vector3(player.transform.position.x, player.transform.position.y,
                player.transform.position.z);
        }

        /// <summary>
        /// Have the enemy track the player position and
        /// calculate when it can fire projectiles
        /// </summary>
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
                        StartCoroutine(FireBullet());
                    }
                }
            }
            catch (Exception e)
            {
                //    //Debug.Log("Player is dead");
            }
        }

        /// <summary>
        /// Spawn bullets and shoot them in accordance to set spread value
        /// </summary>
        /// <returns></returns>
        private IEnumerator FireBullet()
        {
            GameObject shoot = objectPoolManager.Spawn(projectileTypeIdentifier, fireSpawn.position,
                                        fireSpawn.rotation);
            randX = Random.Range(-fireCone, fireCone);
            randY = Random.Range(-fireCone, fireCone);
            randZ = Random.Range(-fireCone, fireCone);

            shoot.GetComponent<Transform>().Rotate(randX, randY, randZ);
            shoot.GetComponent<Rigidbody>().velocity = shoot.transform.forward * fireSpeed;
            yield break;
        }

        protected override void Reset()
        {
            projectileTypeIdentifier =
                enemyAttributes.EnemyProjectileTypeIdentifiers[TypeIdentifier];
            maxHealth = enemyAttributes.enemyHealthValues[TypeIdentifier];
            Health = maxHealth;
            fireRate = enemyAttributes.enemyFireRate[TypeIdentifier];
            fireSpeed = enemyAttributes.projectileSpeed;
            fireCone = enemyAttributes.enemySpread[TypeIdentifier];
            pointValue = enemyAttributes.enemyScoreValues[TypeIdentifier];
        }
    }
}