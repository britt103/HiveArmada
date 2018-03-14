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
using Hive.Armada.Menus;
using Hive.Armada.Player;
using MirzaBeig.ParticleSystems;

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
        /// (e.g. wave manager, game manager, etc.)
        /// </summary>
        protected ReferenceManager reference;

        protected WaveManager waveManager;

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
        /// Reference to the wave that spawned this enemy. Used to inform it
        /// when this enemy is hit for the first time and when it is killed.
        /// </summary>
        protected int wave;

        protected string path;

        /// <summary>
        /// Whether or not this enemy has already been initialized with its attributes.
        /// </summary>
        protected bool isInitialized;

        /// <summary>
        /// How much health the enemy spawns with.
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
        protected GameObject spawnEmitter;

        /// <summary>
        /// The particle system for the enemy spawn emitter.
        /// </summary>
        protected ParticleSystems spawnEmitterSystem;

        /// <summary>
        /// The particle emitter for enemy death.
        /// </summary>
        [Tooltip("The particle emitter for enemy death.")]
        protected GameObject deathEmitter;

        /// <summary>
        /// The type identifier needed to spawn the death emitter from the object pool.
        /// </summary>
        protected int deathEmitterTypeIdentifier;

        /// <summary>
        /// Changes to false on first hit.
        /// Used to tell the wave that it can spawn more enemies.
        /// </summary>
        protected bool untouched = true;

        /// <summary>
        /// Used to prevent HitFlash() from being called a
        /// second time before it is done flashing
        /// </summary>
        protected Coroutine hitFlash;

        /// <summary>
        /// List of Renderers on the enemy that will flash when hit.
        /// </summary>
        protected List<Renderer> renderers;

        /// <summary>
        /// List of Materials of all pieces of the enemy model.
        /// Used to reset Materials after flashing.
        /// </summary>
        protected List<Material> materials;

        /// <summary>
        /// The attack pattern number that this enemy should use.
        /// </summary>
        protected AttackPattern attackPattern;

        /// <summary>
        /// Time until enemy self-destructs if untouched.
        /// </summary>
        protected float selfDestructTime;

        /// <summary>
        /// The player's ship.
        /// </summary>
        protected GameObject player;

        /// <summary>
        /// Used to shake low health enemies.
        /// </summary>
        protected bool shaking;

        /// <summary>
        /// If this enemy has finished pathing.
        /// </summary>
        public bool PathingComplete { get; protected set; }

        /// <summary>
        /// Initializes references to ReferenceManager and other managers, list of renderers and
        /// their materials for HitFlash(), and spawns the spawn particle emitter.
        /// </summary>
        public virtual void Awake()
        {
            reference = GameObject.Find("Reference Manager").GetComponent<ReferenceManager>();

            if (reference == null)
            {
                Debug.LogError(GetType().Name +
                               " - Could not find Reference Manager. Enemy not initialized.");
            }
            else
            {
                player = reference.shipLookTarget;
                enemyAttributes = reference.enemyAttributes;
                waveManager = reference.waveManager;
                scoringSystem = reference.scoringSystem;
                objectPoolManager = reference.objectPoolManager;

                renderers = new List<Renderer>();
                materials = new List<Material>();

                foreach (Renderer r in gameObject.GetComponentsInChildren<Renderer>())
                {
                    if (r.gameObject.CompareTag("Emitter") ||
                        r.transform.parent.CompareTag("Emitter"))
                    {
                        continue;
                    }

                    renderers.Add(r);
                    materials.Add(r.material);
                }
            }
        }

        /// <summary>
        /// Used to apply damage to an enemy.
        /// </summary>
        /// <param name="damage"> How much damage this enemy is taking. </param>
        /// <param name="sendFeedback"> If the hit should trigger a haptic feedback pulse </param>
        public virtual void Hit(int damage, bool sendFeedback)
        {
            if (!PathingComplete)
            {
                return;
            }

            if (sendFeedback)
            {
                reference.playerShip.GetComponent<ShipController>().hand.controller.TriggerHapticPulse(2500);
            }

            Health -= damage;

            if (hitFlash == null)
            {
                hitFlash = StartCoroutine(HitFlash());
            }

            if (Health <= 20)
            {
                shaking = true;
            }

            if (Health <= 0)
            {
                Kill();
            }
        }

        /// <summary>
        /// Notifies the wave that this enemy has been killed, adds to the player's score
        /// and stats, spawns the death particle emitter, and despawns itself.
        /// </summary>
        protected virtual void Kill()
        {
            if (!reference.waveManager.IsInfinite)
            {
                waveManager.EnemyDead(wave);
            }
            else
            {
                waveManager.EnemyDead(path);
            }

            reference.scoringSystem.ComboIn(pointValue, transform);
            reference.statistics.EnemyKilled();
            FindObjectOfType<LexiconUnlockData>().AddEnemyUnlock(gameObject.name);
            objectPoolManager.Spawn(gameObject, deathEmitterTypeIdentifier, transform.position,
                                    transform.rotation);
            objectPoolManager.Despawn(gameObject);
        }

        /// <summary>
        /// Tells the wave to respawn this enemy.
        /// </summary>
        protected virtual void SelfDestruct()
        {
            objectPoolManager.Spawn(gameObject, deathEmitterTypeIdentifier, transform.position,
                                    transform.rotation);
            objectPoolManager.Despawn(gameObject);
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
        /// This is run after the enemy has completed its path.
        /// </summary>
        protected virtual void OnPathingComplete()
        {
//            gameObject.layer = Utility.enemyLayerId;
            PathingComplete = true;
        }

        /// <summary>
        /// Used to set the wave index.
        /// </summary>
        /// <param name="wave"> The index of the wave that spawned this enemy </param>
        public virtual void SetWave(int wave)
        {
            this.wave = wave;
        }

        /// <summary>
        /// Sets the name of the path this enemy used to spawn.
        /// </summary>
        /// <param name="path"> The path name </param>
        public virtual void SetPath(string path)
        {
            this.path = path;
        }

        /// <summary>
        /// Sets the attack pattern that this enemy will use against the player.
        /// </summary>
        /// <param name="attackPattern"> The attack pattern to use </param>
        public virtual void SetAttackPattern(AttackPattern attackPattern)
        {
            this.attackPattern = attackPattern;
        }

        /// <summary>
        /// Countdowns down from selfDestructTime. Calls Kill() if untouched.
        /// </summary>
        protected virtual void SelfDestructCountdown()
        {
            selfDestructTime -= Time.deltaTime;
            if (selfDestructTime <= 0 && untouched)
            {
                SelfDestruct();
            }
        }

        /// <summary>
        /// Resets attributes to this enemy's defaults from enemyAttributes.
        /// </summary>
        protected override void Reset()
        {
//            gameObject.layer = Utility.pathingEnemyLayerId;

            // reset materials
            for (int i = 0; i < renderers.Count; ++i)
            {
                renderers.ElementAt(i).material = materials.ElementAt(i);
            }

            PathingComplete = false;
            hitFlash = null;
            shaking = false;
            maxHealth = enemyAttributes.enemyHealthValues[TypeIdentifier];
            Health = maxHealth;
            pointValue = enemyAttributes.enemyScoreValues[TypeIdentifier];
            selfDestructTime = enemyAttributes.enemySelfDestructTimes[TypeIdentifier];
            deathEmitterTypeIdentifier = enemyAttributes.EnemyDeathEmitterTypeIds[TypeIdentifier];
        }
    }
}