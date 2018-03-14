//=============================================================================
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
using UnityEngine;

namespace Hive.Armada.Enemies
{
    /// <summary>
    /// Kamikaze enemy
    /// </summary>
    public class KamikazeTurret : Enemy
    {
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

        /// <summary>
        /// If the Kamikaze has already gone near the player.
        /// </summary>
        private bool nearPlayer;

        /// <summary>
        /// If the Kamikaze has gone in range before.
        /// </summary>
        private bool inRange;

        private AudioSource source;

        public AudioClip clip;

        /// <summary>
        /// Looks at the player and stores own position.
        /// </summary>
        void Start()
        {
            Reset();
            if (player != null)
            {
                player = GameObject.FindGameObjectWithTag("Player");
                transform.LookAt(player.transform.position);
            }
            source = GetComponent<AudioSource>();

        }

        /// <summary>
        /// Moves the enemy closer to the player and explodes if they are within 'range'. Runs every frame.
        /// </summary>
        void Update()
        {
            if (player != null)
            {
                myTransform = transform;


                if (Vector3.Distance(transform.position, player.transform.position) >= range)
                {
                    myTransform.rotation = Quaternion.Lerp(myTransform.rotation,
                                                           Quaternion.LookRotation(player.transform.position
                                                           - myTransform.position),
                                                           rotationSpeed * Time.deltaTime);

                    myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
                }

                else if (Vector3.Distance(transform.position, player.transform.position) < range)
                {
                    if (!inRange)
                    {
                        StartCoroutine(InRange());
                    }
                }
                if (shaking)
                {
                    iTween.ShakePosition(gameObject, new Vector3(0.1f, 0.1f, 0.1f), 0.1f);
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
                other.gameObject.GetComponent<Player.PlayerHealth>().Hit(damage);
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
                nearPlayer = true;
                StartCoroutine(InRange());
            }
        }

        /// <summary>
        /// Causes a timed explosion when the enemy gets within range of the player ship
        /// </summary>
        private IEnumerator InRange()
        {
            inRange = true;
            source.PlayOneShot(clip);
            yield return new WaitForSeconds(clip.length);

            if (Vector3.Distance(transform.position, player.transform.position) < range)
            {
                player.GetComponentInParent<Player.PlayerHealth>().Hit(damage);
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
            damage = enemyAttributes.projectileDamage;
        }
    }
}