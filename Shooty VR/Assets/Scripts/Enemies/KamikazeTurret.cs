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
        public int damage;

        /// <summary>
        /// Looks at the player and stores own position.
        /// </summary>
        void Start()
        {
            myTransform = transform;
            if (player != null) {
                player = GameObject.FindGameObjectWithTag("Player");                    
                transform.LookAt(player.transform.position);
            }
        }

        /// <summary>
        /// Moves the enemy closer to the player and explodes if they are within 'range'. Runs every frame.
        /// </summary>
        void Update()
        {
            player = GameObject.FindGameObjectWithTag("Player");

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
                StartCoroutine(InRange());
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
        /// Causes a timed explosion when the enemy gets within range of the player ship
        /// </summary>
        IEnumerator InRange()
        {
            foreach (Renderer r in renderers)
            {
                r.material = flashColor;
            }

            yield return new WaitForSeconds(1.0f);

            // reset materials
            for (int i = 0; i < renderers.Count; ++i)
            {
                renderers[i].material = materials[i];
            }

            if (Vector3.Distance(transform.position, player.transform.position) < range)
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
          
            maxHealth = enemyAttributes.enemyHealthValues[TypeIdentifier];
            Health = maxHealth;
            pointValue = enemyAttributes.enemyScoreValues[TypeIdentifier];
        }
    }
}
