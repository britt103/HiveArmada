//=============================================================================
// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
// 
// This abstract class is the base for all enemies. It handles all fields
// and methods related to health and taking damage.
// 
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Hive.Armada.Game;
using UnityEngine;

namespace Hive.Armada.Enemies
{
    /// <inheritdoc />
    /// <summary>
    /// The base class for all enemies.
    /// </summary>
    public abstract class Enemy : MonoBehaviour
    {
        /// <summary>
        /// Reference manager that holds all needed references
        /// (e.g. spawner, game manager, etc.)
        /// </summary>
        protected ReferenceManager reference;

        /// <summary>
        /// How much health the enemy spawns with.
        /// TODO: Move this to EnemyStats script
        /// </summary>
        [Tooltip("How much health the enemy spawns with.")]
        public int maxHealth;

        /// <summary>
        /// Current health. Nothing can access this.
        /// </summary>
        protected int health;

        /// <summary>
        /// How many points this enemy is worth when killed.
        /// </summary>
        [Tooltip("How many points this enemy is worth when killed.")]
        public int pointValue;

        /// <summary>
        /// The color the ship flashes when it is hit.
        /// </summary>
        [Tooltip("The color the ship flashes when it is hit.")]
        public Material flashColor;

        /// <summary>
        /// The particle emitter for enemy spawn.
        /// </summary>
        [Tooltip("The particle emitter for enemy spawn.")]
        public GameObject spawnEmitter;

        /// <summary>
        /// The particle emitter for enemy death.
        /// </summary>
        [Tooltip("The particle emitter for enemy death.")]
        public GameObject deathEmitter;

        /// <summary>
        /// Changes to false on first hit.
        /// Used to tell spawner that it can spawn more enemies.
        /// </summary>
        protected bool untouched = true;
        protected bool hitFlashing;

        /// <summary>
        /// Used to prevent HitFlash() from being called a
        /// second time before it is done flashing
        /// </summary>
        protected Coroutine hitFlash;

        /// <summary>
        /// List of Materials of all pieces of the enemy model.
        /// Used to reset Materials after flashing.
        /// </summary>
        protected List<Material> mats;

        /// <summary>
        /// Initializes variables for the enemy when it loads.
        /// </summary>
        public virtual void Awake()
        {
            reference = GameObject.Find("Reference Manager").GetComponent<ReferenceManager>();

            if (reference == null)
                Debug.LogError(GetType().Name + " - Could not find Reference Manager!");

            mats = new List<Material>();
            health = maxHealth;
            Instantiate(spawnEmitter, transform.position, transform.rotation, transform);
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

            if (hitFlash == null)
            {
                hitFlash = StartCoroutine(HitFlash());
            }

            if (health <= 0)
                Kill();

            if (!untouched) return;

            untouched = false;

            if (reference.spawner != null)
                reference.spawner.EnemyHit();
        }

        /// <summary>
        /// Destroys this GameObject without flashing.
        /// </summary>
        protected virtual void Kill()
        {
            reference.scoringSystem.AddScore(pointValue);
            reference.spawner.AddKill();
            reference.statistics.EnemyKilled();
            Instantiate(deathEmitter, transform.position, transform.rotation);

            Destroy(gameObject);
        }

        /// <summary>
        /// Visual feedback when the enemy is hit. Flashes the material using flashColor.
        /// Calls Kill() if the enemy is out of health. Adds to the score via GameManager.
        /// </summary>
        protected virtual IEnumerator HitFlash()
        {
            foreach (Renderer r in gameObject.GetComponentsInChildren<Renderer>())
            {
                if (r.gameObject.CompareTag("Emitter") || r.transform.parent.CompareTag("Emitter"))
                    continue;

                mats.Add(r.material);

                r.material = flashColor;
            }

            yield return new WaitForSeconds(0.01f);

            // reset materials
            foreach (Renderer r in gameObject.GetComponentsInChildren<Renderer>())
            {
                if (r.gameObject.CompareTag("Emitter") || r.transform.parent.CompareTag("Emitter"))
                    continue;

                r.material = mats.First();
                mats.RemoveAt(0);
            }

            hitFlash = null;
        }
    }
}
