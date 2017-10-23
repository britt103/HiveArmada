//=============================================================================
// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
// 
// This abstract class is the base for all enemies.
// It handles all fields and methods related to
// health and taking damage.
// 
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using Hive.Armada.Game;
using UnityEngine;

namespace Hive.Armada.Enemies
{
    public abstract class Enemy : MonoBehaviour
    {
        public Spawner spawner;
        public int maxHealth;
        protected int health;
        public Material flashColor;
        public GameObject fxSpawn, fxKill;
        protected Material material;
        protected WaveManager waveManager;
        protected bool untouched = true;

        /// <summary>
        /// Initializes variables for the enemy when it loads.
        /// </summary>
        public virtual void Awake()
        {
            health = maxHealth;
            material = gameObject.GetComponentInChildren<Renderer>().material;
            waveManager = GameObject.FindGameObjectWithTag("Wave").GetComponent<WaveManager>();
            spawner = GameObject.FindGameObjectWithTag("Wave").GetComponent<Spawner>();
            Instantiate(fxSpawn, transform.position, transform.rotation);
        }

        /// <summary>
        /// The current health for the enemy.
        /// </summary>
        /// <returns> Integer health value </returns>
        public virtual int GetHealth()
        {
            return health;
        }

        /// <summary>
        /// Used to apply damage to an enemy.
        /// </summary>
        /// <param name="damage"> How much damage this enemy is taking. </param>
        public virtual void Hit(int damage)
        {
            health -= damage;
            StartCoroutine(HitFlash());

            if (untouched)
            {
                untouched = false;
                if (spawner != null)
                    spawner.EnemyHit();
            }
        }

        /// <summary>
        /// Currently unused. Flashes and destroys the enemy when it collides with the player.
        /// </summary>
        public virtual void Collide()
        {
            health = 0;
            StartCoroutine(HitFlash());
        }

        /// <summary>
        /// Destroys this GameObject without flashing.
        /// </summary>
        protected virtual void Kill()
        {
            Instantiate(fxKill, transform.position, transform.rotation);
            spawner.AddKill();
            waveManager.currDead++;
            Destroy(gameObject);
        }

        /// <summary>
        /// Visual feedback when the enemy is hit. Flashes the material using flashColor.
        /// Calls Kill() if the enemy is out of health. Adds to the score via GameManager.
        /// </summary>
        /// <returns>  </returns>
        protected virtual IEnumerator HitFlash()
        {
            //gameObject.GetComponent<Renderer>().material = flashColor;

            foreach (Renderer renderer in gameObject.GetComponentsInChildren<Renderer>())
            {
                renderer.material = flashColor;
            }

            yield return new WaitForSeconds(.01f);

            if (health <= 0)
            {
                Kill();
            }
            //gameObject.GetComponent<Renderer>().material = material;

            foreach (Renderer renderer in gameObject.GetComponentsInChildren<Renderer>())
            {
                renderer.material = material;
            }
        }
    }
}
