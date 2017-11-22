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
using UnityEngine;
using Hive.Armada.Game;

namespace Hive.Armada.Enemies
{
    /// <summary>
    /// Varying attack patterns for this enemy.
    /// </summary>
    public enum AttackPattern
    {
        /// <summary>
        /// Attack pattern one.
        /// </summary>
        One,

        /// <summary>
        /// Attack pattern two.
        /// </summary>
        Two,

        /// <summary>
        /// Attack pattern three.
        /// </summary>
        Three,

        /// <summary>
        /// Attack pattern four.
        /// </summary>
        Four
    }

    /// <inheritdoc />
    /// <summary>
    /// The base class for all enemies.
    /// </summary>
    public abstract class Enemy : Poolable
    {
        /// <summary>
        /// Reference manager that holds all needed references
        /// (e.g. spawner, game manager, etc.)
        /// </summary>
        protected ReferenceManager reference;

        /// <summary>
        /// Reference to enemy attributes to initialize/reset this enemy's attributes.
        /// </summary>
        protected EnemyAttributes enemyAttributes;

        /// <summary>
        /// Reference to the scoring system for adding score when this enemy dies.
        /// </summary>
        protected ScoringSystem scoringSystem;

        /// <summary>
        /// Reference to the object pool manager. Used for easy access to spawning projectiles
        /// and despawning this enemy when it dies.
        /// </summary>
        protected ObjectPoolManager objectPoolManager;

        /// <summary>
        /// Reference to the subwave that spawned this enemy. Used to inform it
        /// when this enemy is hit for the first time and when it is killed.
        /// </summary>
        protected Subwave subwave;

        /// <summary>
        /// How much health the enemy spawns with.
        /// TODO: Move this to EnemyStats script
        /// </summary>
        [Tooltip("How much health the enemy spawns with.")]
        protected int maxHealth;

        /// <summary>
        /// Current health. Cannot be publicly changed.
        /// </summary>
        public int Health { get; protected set; }

        /// <summary>
        /// How many points this enemy is worth when killed.
        /// </summary>
        [Tooltip("How many points this enemy is worth when killed.")]
        protected int pointValue;

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

        /// <summary>
        /// Used to prevent HitFlash() from being called a
        /// second time before it is done flashing
        /// </summary>
        protected Coroutine hitFlash;

        protected List<Renderer> renderers;

        /// <summary>
        /// List of Materials of all pieces of the enemy model.
        /// Used to reset Materials after flashing.
        /// </summary>
        protected List<Material> materials;

        /// <summary>
        /// Initializes references to ReferenceManager and other managers.
        /// </summary>
        public virtual void Awake()
        {
            reference = GameObject.Find("Reference Manager").GetComponent<ReferenceManager>();

            if (reference == null)
            {
                Debug.LogError(GetType().Name + " - Could not find Reference Manager!");
            }

            enemyAttributes = reference.enemyAttributes;
            scoringSystem = reference.scoringSystem;
            objectPoolManager = reference.objectPoolManager;

            renderers = new List<Renderer>();
            materials = new List<Material>();

            foreach (Renderer r in gameObject.GetComponentsInChildren<Renderer>())
            {
                if (r.gameObject.CompareTag("Emitter") ||
                    r.transform.parent.CompareTag("Emitter") ||
                    r.gameObject.CompareTag("FX") ||
                    r.transform.parent.CompareTag("FX"))
                {
                    continue;
                }

                renderers.Add(r);
                materials.Add(r.material);
            }

            Instantiate(spawnEmitter, transform.position, transform.rotation, transform);
        }

        /// <summary>
        /// Used to apply damage to an enemy.
        /// </summary>
        /// <param name="damage"> How much damage this enemy is taking. </param>
        public virtual void Hit(int damage)
        {
            Health -= damage;

            if (hitFlash == null)
            {
                hitFlash = StartCoroutine(HitFlash());
            }

            if (Health <= 0)
            {
                Kill();
            }

            if (!untouched)
            {
                return;
            }

            untouched = false;

            subwave.EnemyHit();

            if (reference.spawner != null)
            {
                reference.spawner.EnemyHit();
            }
        }

        /// <summary>
        /// Destroys this GameObject without flashing.
        /// </summary>
        protected virtual void Kill()
        {
            subwave.EnemyDead();
            scoringSystem.AddScore(pointValue);
            reference.spawner.AddKill();
            reference.statistics.EnemyKilled();
            Instantiate(deathEmitter, transform.position, transform.rotation);

            Destroy(gameObject);
        }

        /// <summary>
        /// Visual feedback when the enemy is hit. Flashes the material using flashColor.
        /// </summary>
        protected virtual IEnumerator HitFlash()
        {
            // "flash" materials to flashColor
            foreach (Renderer r in renderers)
            {
                r.material = flashColor;
            }

            yield return new WaitForSeconds(0.01f);

            // reset materials
            for (int i = 0; i < renderers.Count; ++i)
            {
                renderers.ElementAt(i).material = materials.ElementAt(i);
            }

            hitFlash = null;
        }

        /// <summary>
        /// Used to set the subwave reference.
        /// </summary>
        /// <param name="subwave"> The subwave that spawned this enemy </param>
        public virtual void SetSubwave(Subwave subwave)
        {
            this.subwave = subwave;
        }
    }
}