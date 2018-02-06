﻿//=============================================================================
// Ryan Britton
// britt103
// #1849351
// CPSC-340-01, CPSC-344-01
// Group Project
// 
// Enemy that moves towards the player and explodes once it gets too close
//
//=============================================================================

using System.Collections;
using System.Linq;
using Hive.Armada.Player;
using UnityEngine;
using MirzaBeig.ParticleSystems;

namespace Hive.Armada.Enemies
{
    /// <summary>
    /// Kamikaze enemy
    /// </summary>
    public class KamikazeTurret : Enemy
    {
        /// <summary>
        /// The player ship.
        /// </summary>
        private GameObject player;

        /// <summary>
        /// Transform of this enemy.
        /// </summary>
        private Transform myTransform;

        /// <summary>
        /// How fast this enemy moves towards the player.
        /// </summary>
        public float moveSpeed;

        /// <summary>
        /// How fast this enemy rotates towards the player.
        /// </summary>
        public float rotationSpeed;

        /// <summary>
        /// How close this enemy can get to the player before exploding.
        /// </summary>
        public float range;

        /// <summary>
        /// How much damage this enemy does to the player.
        /// </summary>
        private int damage;

        // <summary>
        /// Final position after spawning.
        /// </summary>
        private Vector3 endPosition;

        /// <summary>
        /// Bools used to move the enemy to its spawn position.
        /// </summary>
        private bool spawnComplete;

        /// <summary>
        /// Bools used to move the enemy to its spawn position.
        /// </summary>
        private bool moveComplete;

        /// <summary>
        /// If the Kamikaze has already gone near the player.
        /// </summary>
        private bool nearPlayer;

        /// <summary>
        /// Looks at the player and stores own position.
        /// </summary>
        private void Start()
        {
            if (player != null)
            {
                player = reference.playerShip;
                transform.LookAt(player.transform.position);
            }
        }

        /// <summary>
        /// Moves the enemy closer to the player and explodes if they are within 'range'. Runs every frame.
        /// </summary>
        private void Update()
        {
            if (spawnComplete)
            {
                if (moveComplete)
                {
                    player = reference.playerShip;

                    if (Vector3.Distance(transform.position, player.transform.position) >= range)
                    {
                        myTransform.rotation = Quaternion.Lerp(myTransform.rotation,
                                                               Quaternion.LookRotation(
                                                                   player.transform.position
                                                                   - myTransform.position),
                                                               rotationSpeed * Time.deltaTime);

                        myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
                    }
                }
                else
                {
                    transform.position =
                        Vector3.Lerp(transform.position, endPosition, Time.deltaTime * 1.0f);
                    if (Vector3.Distance(transform.position, endPosition) <= 0.1f)
                    {
                        moveComplete = true;
                    }
                }
            }
        }

        /// <summary>
        /// Causes a timed explosion when the enemy gets too close to the player
        /// </summary>
        /// <param name="other">
        /// The object collided with.
        /// </param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                other.gameObject.GetComponent<PlayerHealth>().Hit(damage);
                Kill();
            }
        }

        /// <summary>
        /// Runs when this enemy finishes default pathing to a SpawnZone.
        /// </summary>
        /// <param name="endPos"> Final position of this enemy. </param>
        public void SetEndpoint(Vector3 endPos)
        {
            endPosition = endPos;
            spawnComplete = true;
        }

        /// <summary>
        /// Runs InRange if not already running.
        /// </summary>
        public void NearPlayer()
        {
            if (!nearPlayer)
            {
                nearPlayer = true;
                StartCoroutine(InRange());
            }
        }

        /// <summary>
        /// Causes a timed explosion when the enemy gets within range of the player ship
        /// </summary>
        private IEnumerator InRange()
        {
            moveSpeed = moveSpeed / 2.0f;
            foreach (Renderer r in renderers)
            {
                r.material = flashColor;
            }

            yield return new WaitForSeconds(1.0f);

            // reset materials
            for (int i = 0; i < renderers.Count; ++i)
            {
                renderers.ElementAt(i).material = materials.ElementAt(i);
            }

            if (Vector3.Distance(transform.position, player.transform.position) < range)
            {
                player.gameObject.GetComponent<PlayerHealth>().Hit(damage);
            }
            Kill();
        }

        /// <summary>
        /// Resets attributes to this enemy's defaults from enemyAttributes.
        /// </summary>
        protected override void Reset()
        {
            // reset materials
            for (int i = 0; i < renderers.Count; ++i)
            {
                renderers.ElementAt(i).material = materials.ElementAt(i);
            }

            hitFlash = null;
            shaking = false;
            spawnComplete = false;
            moveComplete = false;

            maxHealth = enemyAttributes.enemyHealthValues[TypeIdentifier];
            Health = maxHealth;
            pointValue = enemyAttributes.enemyScoreValues[TypeIdentifier];
            spawnEmitter = enemyAttributes.enemySpawnEmitters[TypeIdentifier];
            deathEmitter = enemyAttributes.enemyDeathEmitters[TypeIdentifier];
            damage = enemyAttributes.projectileDamage;

            if (!isInitialized)
            {
                isInitialized = true;

                GameObject spawnEmitterObject = Instantiate(spawnEmitter,
                                                            transform.position,
                                                            transform.rotation, transform);
                spawnEmitterSystem = spawnEmitterObject.GetComponent<ParticleSystems>();

                deathEmitterTypeIdentifier = objectPoolManager.GetTypeIdentifier(deathEmitter);
            }
        }
    }
}