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

using Hive.Armada.Player;
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

        /// <summary>
        /// If the Kamikaze has already gone near the player.
        /// </summary>
        private bool nearPlayer;

        /// <summary>
        /// If the Kamikaze has gone in range before.
        /// </summary>
        private bool inRange;

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
                playerShield = player.transform.Find("Shield").gameObject;
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
                myTransform.rotation = Quaternion.Lerp(myTransform.rotation,
                                                          Quaternion.LookRotation(player.transform.position
                                                          - myTransform.position),
                                                          rotationSpeed * Time.deltaTime);

                myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
                               
                if (shaking)
                {
                    iTween.ShakePosition(gameObject, new Vector3(0.001f, 0.001f, 0.001f), 0.01f);
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
                Debug.Log("I am near the player");
                nearPlayer = true;
                StartCoroutine(InRange());
            }
        }

        /// <summary>
        /// Causes a timed explosion when the enemy gets within range of the player ship
        /// </summary>
        private IEnumerator InRange()
        {
            Debug.Log("I am in range");
            inRange = true;
            moveSpeed = (moveSpeed / 2);
            source.PlayOneShot(clip);
            yield return new WaitForSeconds(clip.length);

            if (playerShield.GetComponent<Collider>().bounds.Contains(gameObject.transform.position))
            {
                player.gameObject.GetComponent<Player.PlayerHealth>().Hit(damage);
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