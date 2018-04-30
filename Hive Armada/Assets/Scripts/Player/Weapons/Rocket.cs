//=============================================================================
//
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-440-01, CPSC-340-01 & CPSC-344-01
// Group Project
//
// This class defines a multi-use rocket. This rocket is designed to be used
// with the player rocket pod weapon, ally power-up, and area bomb power-up.
//
//=============================================================================

using System;
using Hive.Armada.Data;
using Hive.Armada.Enemies;
using Hive.Armada.Game;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Random = UnityEngine.Random;

namespace Hive.Armada.Player.Weapons
{
    /// <summary>
    /// Multi-use rocket. Used by player rocket pod weapon, ally power-up, and area bomb power-up.
    /// </summary>
    public abstract class Rocket : Poolable
    {
        /// <summary>
        /// Bit flag for rocket behavior.
        /// </summary>
        [Flags]
        public enum RocketFlags
        {
            /// <summary>
            /// If the rocket sends haptic feedback to the player's controller on enemy hit.
            /// </summary>
            HapticFeedback = 1 << 0,

            /// <summary>
            /// If the rocket deals AoE explosive damage.
            /// </summary>
            ExplosiveDamage = 1 << 1,

            /// <summary>
            /// If the rocket should auto-aquire new targets.
            /// </summary>
            AutoTarget = 1 << 3,
        }

        public RocketData rocketData;

        /// <summary>
        /// Reference to the ship controller for controller vibrations.
        /// </summary>
        private ShipController shipController;

        /// <summary>
        /// How much damage the rocket does to enemies.
        /// </summary>
        protected int damage;

        /// <summary>
        /// Sets a list of behaviors for this rocket.
        /// </summary>
        [EnumFlags]
        protected RocketFlags behaviorFlags;

        /// <summary>
        /// How fast the rocket moves.
        /// </summary>
        private float speed;

        /// <summary>
        /// How strong the homing effect is.
        /// 0 = no homing
        /// 1 = perfect homing
        /// </summary>
        private float homingSensitivity;

        /// <summary>
        /// Time until the rocket auto explodes.
        /// </summary>
        private float explodeTime;

        private float selfDestructTime;

        /// <summary>
        /// The radius of the area of effect.
        /// </summary>
        private float explosiveRadius;

        /// <summary>
        /// Time between random rocket movements.
        /// </summary>
        private float randomTime;

        /// <summary>
        /// Range for the random x movement.
        /// </summary>
        private float randomX;

        /// <summary>
        /// Range for the random y movement.
        /// </summary>
        private float randomY;

        /// <summary>
        /// Range for the random z movement.
        /// </summary>
        private float randomZ;

        private float nextRandomTime;

        /// <summary>
        /// Type ID for the explosion emitter.
        /// </summary>
        private short explosionEmitterId;

        /// <summary>
        /// Type ID for the rocket emitter.
        /// </summary>
        private short rocketEmitterId;

        /// <summary>
        /// The rocket emitter game object.
        /// </summary>
        private GameObject rocketEmitter;

        /// <summary>
        /// Type ID for the rocket trail emitter.
        /// </summary>
        private short trailEmitterId;

        /// <summary>
        /// The rocket trail emitter game object.
        /// </summary>
        private GameObject trailEmitter;

        /// <summary>
        /// The target object for the rocket to seek.
        /// </summary>
        private GameObject target;

        private int targetEnemyId;

        /// <summary>
        /// Reference to the poolable script on the target.
        /// </summary>
        private Poolable targetPoolableScript;

        /// <summary>
        /// Reference to the enemy script on the target.
        /// </summary>
        private Enemy targetEnemyScript;

        /// <summary>
        /// The target position for the rocket to seek.
        /// </summary>
        private Vector3 targetPosition;

        /// <summary>
        /// Multiplier for damage dealt.
        /// </summary>
        private int damageMultiplier;

        /// <summary>
        /// If the rocket is homing on an object or launching to a point.
        /// </summary>
        private bool isHoming;

        /// <summary>
        /// If the rocket was homing and its target died.
        /// </summary>
        private bool isTargetDead;

        /// <summary>
        /// The enemy GameObject that the rocket hit.
        /// </summary>
        private GameObject explosionTarget;

        /// <summary>
        /// The random movement to translate the rocket by.
        /// </summary>
        private Vector3 randomMovement;

        private Vector3 initialMovement;

        /// <summary>
        /// Smoothly adjusts between current movement and randomMovement.
        /// </summary>
        private Vector3 smoothMovement;

        /// <summary>
        /// Fraction between smoothMovement and randomMovement.
        /// </summary>
        private float smoothFrac;

        public AudioSource source;

        private AudioClip trailClip;

        private AudioClip explosionClip;

        /// <summary>
        /// Deactivates the rocket when it is first created.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            Initialize();
            nextRandomTime = Time.time;
        }

        private void Initialize()
        {
            behaviorFlags = rocketData.behaviorFlags;
            speed = rocketData.speed;
            homingSensitivity = rocketData.homingSensitivity;
            explodeTime = rocketData.explodeTime;
            explosiveRadius = rocketData.explosiveRadius;
            randomTime = rocketData.randomTime;
            randomX = rocketData.randomX;
            randomY = rocketData.randomY;
            randomZ = rocketData.randomZ;
            explosionEmitterId =
                reference.objectPoolManager.GetTypeIdentifier(rocketData.explosionEmitterPrefab);
            rocketEmitterId =
                reference.objectPoolManager.GetTypeIdentifier(rocketData.rocketEmitterPrefab);
            trailEmitterId =
                reference.objectPoolManager.GetTypeIdentifier(rocketData.trailEmitterPrefab);
            explosionClip = rocketData.explosionClip;
            trailClip = rocketData.trailClip;
        }

        /// <summary>
        /// Moves the rocket toward its target.
        /// </summary>
        private void Update()
        {
            if (Time.time >= selfDestructTime)
                Explode();

            if (isHoming)
            {
                if (target != null && target.CompareTag("Enemy") && targetEnemyScript != null &&
                    targetEnemyId != targetEnemyScript.EnemyId)
                {
                    // clear target because it died and has respawned
                    target = null;
                    targetPoolableScript = null;
                    targetEnemyScript = null;
                    isTargetDead = true;
                    isHoming = false;

                    if ((behaviorFlags & RocketFlags.AutoTarget) != 0)
                    {
                        AquireTarget();
                    }
                }
                else if (targetPoolableScript != null && !targetPoolableScript.IsActive ||
                         targetEnemyScript != null && targetEnemyScript.Health <= 0 ||
                         target == null || !target.activeSelf)
                {
                    // clear target because it is dead or deactivated
                    target = null;
                    targetPoolableScript = null;
                    targetEnemyScript = null;
                    isTargetDead = true;
                    isHoming = false;

                    if ((behaviorFlags & RocketFlags.AutoTarget) != 0)
                    {
                        AquireTarget();
                    }
                }

                if (targetPoolableScript != null && targetPoolableScript.IsActive ||
                    targetEnemyScript != null && targetEnemyScript.Health > 0 ||
                    !isTargetDead)
                {
                    // update the target position if it is alive and not null
                    if (target != null)
                    {
                        targetPosition = target.transform.position;
                    }
                }
            }

            if (target == null && (behaviorFlags & RocketFlags.AutoTarget) != 0)
            {
                AquireTarget();
            }

            if (isTargetDead || !isHoming)
            {
                if (Vector3.Distance(transform.position, targetPosition) < 0.4f)
                {
                    Explode();
                }
            }

            if (Time.time >= nextRandomTime)
            {
                nextRandomTime += randomTime;
                smoothFrac = 0.0f;
                initialMovement = randomMovement;
                randomMovement = new Vector3(Random.Range(-randomX, randomX),
                                             Random.Range(-randomY, randomY),
                                             Random.Range(0.0f, randomZ));
            }

            smoothFrac += Time.deltaTime / randomTime;
            smoothMovement = Vector3.Lerp(initialMovement, randomMovement, smoothFrac);

            transform.Translate(0.0f, 0.0f, speed * Time.deltaTime, Space.Self);
            transform.Translate(smoothMovement);

            Vector3 relativePosition = targetPosition - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePosition);
            transform.rotation =
                Quaternion.Slerp(transform.rotation, rotation, homingSensitivity);
        }

        public void Launch(GameObject launchTarget, Vector3 position, int launchDamageMultiplier)
        {
            damageMultiplier = launchDamageMultiplier;
            Launch(launchTarget, position);
        }

        /// <summary>
        /// Enables the rocket, initializes its target,
        /// begins the random movement and self-destruct countdown.
        /// </summary>
        /// <param name="launchTarget"> The gameobject for the rocket to home to </param>
        /// <param name="position"> The position for the rocket to launch to </param>
        public void Launch(GameObject launchTarget, Vector3 position)
        {
            if (launchTarget != null)
            {
                target = launchTarget;
                targetPosition = launchTarget.transform.position;
                isHoming = true;

                targetPoolableScript = launchTarget.GetComponent<Poolable>();
                targetEnemyScript = launchTarget.GetComponent<Enemy>();

                if (targetEnemyScript != null)
                {
                    targetEnemyId = targetEnemyScript.EnemyId;
                }
                else
                {
                    targetEnemyId = -1;
                }
            }
            else
            {
                if ((behaviorFlags & RocketFlags.AutoTarget) != 0)
                {
                    AquireTarget();
                }

                targetPosition = position;
                isHoming = false;
            }

            selfDestructTime = Time.time + explodeTime;

            smoothFrac = 1.0f;
            randomMovement = new Vector3(Random.Range(-randomX, randomX),
                                         Random.Range(-randomY, randomY),
                                         Random.Range(0.0f, randomZ));
            smoothMovement = Vector3.zero;
            initialMovement = Vector3.zero;

            if (rocketEmitterId >= 0)
            {
                rocketEmitter = reference.objectPoolManager.Spawn(
                    gameObject, rocketEmitterId, transform.position,
                    transform.rotation, transform);
            }

            if (trailEmitterId >= 0)
            {
                trailEmitter = reference.objectPoolManager.Spawn(
                    gameObject, trailEmitterId, transform.position,
                    transform.rotation, transform);
            }

            if (trailClip != null)
            {
                source.PlayOneShot(trailClip);
            }
        }

        /// <summary>
        /// Finds the nearest enemy and targets it.
        /// </summary>
        private void AquireTarget()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosiveRadius,
                                                         Utility.enemyMask,
                                                         QueryTriggerInteraction.Collide);
            float closestDistance = Mathf.Infinity;
            int closestIndex = -1;

            for (int i = 0; i < colliders.Length; ++i)
            {
                Vector3 direction = colliders[i].transform.position - transform.position;
                float distance = direction.sqrMagnitude;

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestIndex = i;
                }
            }

            if (closestIndex > -1)
            {
                target = colliders[closestIndex].gameObject;
                isHoming = true;
                isTargetDead = false;
            }
        }

        private void OnDisable()
        {
            source.Stop();
            source.loop = false;
            source.clip = null;
        }

        /// <summary>
        /// Explodes the rocket if it hits the room or an enemy.
        /// </summary>
        /// <param name="other"> The other collider </param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                explosionTarget = other.gameObject;
                other.GetComponent<Enemy>().Hit(damage * damageMultiplier,
                                                (behaviorFlags & RocketFlags.HapticFeedback) != 0);
                Explode();
            }
            else if (other.CompareTag("Shootable"))
            {
                Shootable shootable = other.GetComponent<Shootable>();
                if (shootable != null)
                {
                    shootable.Hit();
                }

                if (shipController == null)
                    shipController = FindObjectOfType<ShipController>();

                shipController.hand.controller.TriggerHapticPulse(2500);
                Explode();
            }
            else if (other.CompareTag("Room"))
            {
                Explode();
            }
        }

        /// <summary>
        /// Spawns the explosion emitter and deals AoE damage.
        /// </summary>
        private void Explode()
        {
            if (rocketEmitter != null)
            {
                reference.objectPoolManager.Despawn(rocketEmitter);
            }

            if (trailEmitter != null)
            {
                reference.objectPoolManager.Despawn(trailEmitter);
            }

            if (explosionEmitterId >= 0)
            {
                GameObject explosionEmitterObject = reference.objectPoolManager.Spawn(
                    gameObject, explosionEmitterId,
                    transform.position,
                    transform.rotation);

                if (explosionClip != null)
                {
                    Emitter explosionEmitter = explosionEmitterObject.GetComponent<Emitter>();

                    if (explosionEmitter != null)
                    {
                        explosionEmitter.PlaySound(explosionClip);
                    }
                }
            }

            if ((behaviorFlags & RocketFlags.ExplosiveDamage) != 0)
            {
                ExplosiveDamage();
            }

            reference.objectPoolManager.Despawn(gameObject);
        }

        /// <summary>
        /// Deals AoE explosive damage.
        /// </summary>
        private void ExplosiveDamage()
        {
            Collider[] colliders =
                Physics.OverlapSphere(transform.position, explosiveRadius, Utility.enemyMask);

            foreach (Collider enemy in colliders)
            {
                Enemy enemyScript = enemy.GetComponent<Enemy>();
                NewBoss bossScript = enemy.GetComponent<NewBoss>();

                if (explosionTarget != null)
                {
                    if (enemy.gameObject.GetInstanceID() != explosionTarget.GetInstanceID())
                    {
                        if (enemyScript != null)
                        {
                            enemyScript.Hit(damage);
                        }
                        else if (bossScript != null)
                        {
                            bossScript.Hit(damage, true);
                        }
                    }
                }
                else
                {
                    if (enemyScript != null)
                    {
                        enemyScript.Hit(damage);
                    }
                    else if (bossScript != null)
                    {
                        bossScript.Hit(damage, true);
                    }
                }
            }
        }

        /// <summary>
        /// Resets rocket variables and deactivates the object.
        /// </summary>
        public override void Deactivate()
        {
            isTargetDead = false;
            explosionTarget = null;

            base.Deactivate();
        }

        /// <summary>
        /// Resets the shipController reference and target for the rocket.
        /// </summary>
        protected override void Reset()
        {
            target = null;
            targetPoolableScript = null;
            targetEnemyScript = null;
            damageMultiplier = 1;
            rocketEmitter = null;
            trailEmitter = null;
        }
    }
}