//=============================================================================
// Ryan Britton
// britt103
// 1849351
// CPSC-340-01, CPSC-344-01
// Group Project
// 
// Enemy that moves towards the player and explodes once it gets too close
//
//=============================================================================

using System.Collections;
using Hive.Armada.Data;
using Hive.Armada.Player;
using UnityEngine;

namespace Hive.Armada.Enemies
{
    /// <summary>
    /// Kamikaze enemy
    /// </summary>
    public class KamikazeTurret : Enemy
    {
        public KamikazeEnemyData kamikazeEnemyData;
        
        /// <summary>
        /// How fast this enemy moves towards the player.
        /// </summary>
        private float moveSpeed;

        /// <summary>
        /// How fast this enemy rotates towards the player.
        /// </summary>
        private float rotationSpeed;

        /// <summary>
        /// How close this enemy can get to the player before exploding.
        /// </summary>
        private float range;

        /// <summary>
        /// How much damage this enemy does to the player.
        /// </summary>
        private int damage;

        /// <summary>
        /// If the Kamikaze has already gone near the player.
        /// </summary>
        private bool nearPlayer;

        /// <summary>
        /// If the Kamikaze has gone in range before.
        /// </summary>
        private bool inRange;

        private GameObject player;

        /// <summary>
        /// Audio source for this enemy.
        /// </summary>
        private AudioSource source;

        /// <summary>
        /// Audio clip attached to this enemy
        /// </summary>
        public AudioClip clip;

        /// <summary>
        /// Shield attached to player ship.
        /// Used to check player damage when exploding.
        /// </summary>
        private GameObject playerShield;

        protected override void Awake()
        {
            base.Awake();
            player = reference.playerShip;
            source = GetComponent<AudioSource>();
            
            Initialize(kamikazeEnemyData);
        }

        private void Initialize(KamikazeEnemyData enemyData)
        {
            base.Initialize(enemyData);

            moveSpeed = enemyData.moveSpeed;
            rotationSpeed = enemyData.rotationSpeed;
            range = enemyData.range;
            damage = reference.projectileData.projectileDamage;
        }

        /// <summary>
        /// Moves the enemy closer to the player and explodes if they are within 'range'. Runs every frame.
        /// </summary>
        private void Update()
        {
            if (PathingComplete)
            {
                if (player != null)
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation,
                                                         Quaternion.LookRotation(
                                                             player.transform.position
                                                             - transform.position),
                                                         rotationSpeed * Time.deltaTime);

                    transform.position += transform.forward * moveSpeed * Time.deltaTime;

                    if (shaking)
                    {
                        iTween.ShakePosition(gameObject, new Vector3(0.001f, 0.001f, 0.001f),
                                             0.01f);
                    }
                }
                else
                {
                    player = reference.playerShip;

                    if (player == null)
                    {
                        transform.LookAt(new Vector3(0.0f, 0.0f, 0.0f));
                    }
                }

                if (playerShield != null)
                {
                    return;
                }

                playerShield = FindObjectOfType<MasterCollider>().gameObject;
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
            if (other.CompareTag("Player"))
            {
                FindObjectOfType<PlayerHealth>().Hit(damage);

                //other.gameObject.GetComponent<PlayerHealth>().Hit(damage);
                Kill();
            }
        }

        /// <summary>
        /// Runs InRange if not already running.
        /// </summary>
        public void NearPlayer()
        {
            if (!nearPlayer)
            {
                return;
            }

            nearPlayer = true;
            StartCoroutine(InRange());
        }

        /// <summary>
        /// Causes a timed explosion when the enemy gets within range of the player ship
        /// </summary>
        private IEnumerator InRange()
        {
            inRange = true;
            moveSpeed = moveSpeed / 2;
            source.PlayOneShot(clip);
            yield return new WaitForSeconds(clip.length);

            //if (playerShield.GetComponent<Collider>().bounds
            //                .Contains(gameObject.transform.position))
            if (Vector3.Distance(gameObject.transform.position, player.transform.position) < range)
            {
                FindObjectOfType<PlayerHealth>().Hit(damage);

                //player.GetComponentInParent<PlayerHealth>().Hit(damage);
            }

            Kill();
        }

        /// <summary>
        /// Resets attributes to this enemy's defaults from enemyAttributes.
        /// </summary>
        protected override void Reset()
        {
            base.Reset();

            inRange = false;
        }
    }
}