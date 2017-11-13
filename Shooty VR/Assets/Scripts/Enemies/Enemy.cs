//=============================================================================
// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
// 
// This abstract class is the base for all enemies. It handles all fields
// and methods related tohealth and taking damage.
// 
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Hive.Armada.Game;
using UnityEngine;

namespace Hive.Armada.Enemies
{
    /// <summary>
    /// The base class for all enemies.
    /// </summary>
    public abstract class Enemy : MonoBehaviour
    {
        /// <summary>
        /// Reference to the wave spawner.
        /// TODO: Remove this reference, make Reference Manager
        /// </summary>
        public Spawner spawner;

        /// <summary>
        /// How much health the enemy spawns with.
        /// TODO: Move this to EnemyStats script
        /// </summary>
        public int maxHealth;

        /// <summary>
        /// Current health. Nothing can access this.
        /// </summary>
        protected int health;

        /// <summary>
        /// The color the ship flashes when it is hit.
        /// </summary>
        public Material flashColor;

        /// <summary>
        /// The particle effect on spawn.
        /// </summary>
        public GameObject fxSpawn;

        /// <summary>
        /// The particle effect on death.
        /// </summary>
        public GameObject fxKill;

        /// <summary>
        /// Changes to false on first hit.
        /// Used to tell spawner that it can spawn more enemies.
        /// </summary>
        protected bool untouched = true;
        protected bool hitFlashing;
        protected bool alive = true;

        /// <summary>
        /// List of materials for all parts of the model.
        /// </summary>
        protected List<Material> mats;

        /// <summary>
        /// Reference to the PlayerStats script for tracking kills.
        /// </summary>
        protected PlayerStats stats;

        /// <summary>
        /// Initializes variables for the enemy when it loads.
        /// </summary>
        public virtual void Awake()
        {
            mats = new List<Material>();
            health = maxHealth;
            spawner = GameObject.FindGameObjectWithTag("Wave").GetComponent<Spawner>();
            Instantiate(fxSpawn, transform.position, transform.rotation, transform);

            stats = FindObjectOfType<PlayerStats>();
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
            if (!hitFlashing)
            {
                StartCoroutine(HitFlash());
            }

            health -= damage;
            if (health <= 0 && alive)
            {
                alive = false;
                Kill();
            }

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
            stats.EnemyKilled();

            Destroy(gameObject);
        }

        /// <summary>
        /// Visual feedback when the enemy is hit. Flashes the material using flashColor.
        /// Calls Kill() if the enemy is out of health. Adds to the score via GameManager.
        /// </summary>
        protected virtual IEnumerator HitFlash()
        {
            hitFlashing = true;

            //gameObject.GetComponent<Renderer>().material = flashColor;

            foreach (Renderer renderer in gameObject.GetComponentsInChildren<Renderer>())
            {
                if (renderer.gameObject.CompareTag("FX"))
                    continue;

                mats.Add(renderer.material);

                renderer.material = flashColor;
            }

            yield return new WaitForSeconds(0.01f);


            // reset materials
            foreach (Renderer renderer in gameObject.GetComponentsInChildren<Renderer>())
            {
                if (renderer.gameObject.CompareTag("FX"))
                    continue;

                renderer.material = mats.First();
                mats.RemoveAt(0);
            }

            hitFlashing = false;
        }
    }
}
