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
using System.Collections;
using UnityEngine;
using Hive.Armada.Enemies;
using Hive.Armada.Game;
using Random = UnityEngine.Random;
using Valve.VR.InteractionSystem;

namespace Hive.Armada.Player.Weapons
{
    /// <summary>
    /// Multi-use rocket. Used by player rocket pod weapon, ally power-up, and area bomb power-up.
    /// </summary>
    public class Rocket : Poolable
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
            /// If the rocket should acquire a new target if its current target dies.
            /// </summary>
            Retarget = 1 << 2,

            /// <summary>
            /// If the rocket should auto-aquire its own target.
            /// </summary>
            AutoTarget = 1 << 3,

            /// <summary>
            /// Disable homing.
            /// </summary>
            NoHoming = 1 << 4
        }

        /// <summary>
        /// Reference manager that holds all needed references
        /// (e.g. spawner, game manager, etc.)
        /// </summary>
        private ReferenceManager reference;

        /// <summary>
        /// Reference to the attributes for all rocket types.
        /// </summary>
        private RocketAttributes rocketAttributes;

        /// <summary>
        /// Reference to the ship controller for controller vibrations.
        /// </summary>
        private ShipController shipController;

        /// <summary>
        /// The trail renderer on this rocket.
        /// </summary>
        private TrailRenderer trailRenderer;

        /// <summary>
        /// Index of this rocket's type attributes in RocketAttributes.
        /// </summary>
        private int rocketType;

        /// <summary>
        /// How much damage the rocket does to enemies.
        /// </summary>
        private int damage;

        /// <summary>
        /// Sets a list of behaviors for this rocket.
        /// </summary>
        [EnumFlags]
        private RocketFlags behaviorFlags;

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

        /// <summary>
        /// Type ID for the explosion emitter.
        /// </summary>
        private int explosionEmitterId;

        /// <summary>
        /// Type ID for the rocket emitter.
        /// </summary>
        private int rocketEmitterId;

        /// <summary>
        /// The rocket emitter game object.
        /// </summary>
        private GameObject rocketEmitter;

        /// <summary>
        /// Type ID for the rocket trail emitter.
        /// </summary>
        private int trailEmitterId;

        /// <summary>
        /// The rocket trail emitter game object.
        /// </summary>
        private GameObject trailEmitter;

        /// <summary>
        /// The target object for the rocket to seek.
        /// </summary>
        private GameObject target;

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

        /// <summary>
        /// Deactivates the rocket when it is first created.
        /// </summary>
        private void Awake()
        {
            reference = FindObjectOfType<ReferenceManager>();
            rocketAttributes = reference.rocketAttributes;
            trailRenderer = GetComponent<TrailRenderer>();
        }

        /// <summary>
        /// Initializes the rocket and its attributes.
        /// </summary>
        /// <param name="rocketType"> The index for the rocket's attributes </param>
        /// <param name="shipController"> The ship controller </param>
        public void SetupRocket(int rocketType, ShipController shipController)
        {
            this.rocketType = rocketType;
            this.shipController = shipController;
            SetAttributes();
        }

        /// <summary>
        /// Initializes the rocket and its attributes.
        /// </summary>
        /// <param name="rocketType"> The index for the rocket's attributes </param>
        public void SetupRocket(int rocketType)
        {
            this.rocketType = rocketType;
            SetAttributes();
        }

        /// <summary>
        /// Sets the rocket's attributes from RocketAttributes.
        /// </summary>
        private void SetAttributes()
        {
            behaviorFlags = rocketAttributes.rockets[rocketType].behaviorFlags;
            damage = rocketAttributes.rockets[rocketType].damage;
            speed = rocketAttributes.rockets[rocketType].speed;
            homingSensitivity = rocketAttributes.rockets[rocketType].homingSensitivity;
            explodeTime = rocketAttributes.rockets[rocketType].explodeTime;
            explosiveRadius = rocketAttributes.rockets[rocketType].explosiveRadius;
            randomTime = rocketAttributes.rockets[rocketType].randomTime;
            randomX = rocketAttributes.rockets[rocketType].randomX;
            randomY = rocketAttributes.rockets[rocketType].randomY;
            randomZ = rocketAttributes.rockets[rocketType].randomZ;

            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new[]
                {
                    new GradientColorKey(rocketAttributes.rockets[rocketType].trailColor,
                                         0.0f),
                    new GradientColorKey(rocketAttributes.rockets[rocketType].trailColor,
                                         1.0f)
                }, new[] {new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f)});
            trailRenderer.colorGradient = gradient;
            trailRenderer.startWidth = rocketAttributes.rockets[rocketType].trailWidth;
            trailRenderer.endWidth = rocketAttributes.rockets[rocketType].trailWidth;

            explosionEmitterId = rocketAttributes.RocketExplosionEmitterIds[rocketType];
            rocketEmitterId = rocketAttributes.RocketEmitterIds[rocketType];
            trailEmitterId = rocketAttributes.TrailEmitterIds[rocketType];

            if (rocketEmitterId >= 0)
            {
                rocketEmitter =
                    reference.objectPoolManager.Spawn(gameObject, rocketEmitterId,
                                                      transform.position, transform.rotation,
                                                      transform);
            }

            if (trailEmitterId >= 0)
            {
                trailEmitter =
                    reference.objectPoolManager.Spawn(gameObject, trailEmitterId,
                                                      transform.position, transform.rotation,
                                                      transform);
                GetComponent<TrailRenderer>().enabled = false;
            }
            else
            {
                GetComponent<TrailRenderer>().enabled = true;
            }
        }

        /// <summary>
        /// Sets the damage multiplier for the rocket.
        /// </summary>
        /// <param name="multiplier"> The damage multiplier </param>
        public void SetDamageMultiplier(int multiplier)
        {
            damageMultiplier = multiplier;
        }

        /// <summary>
        /// Moves the rocket toward its target.
        /// </summary>
        private void Update()
        {
            if ((behaviorFlags & RocketFlags.NoHoming) == 0)
            {
                if (isHoming)
                {
                    if (targetPoolableScript != null && targetPoolableScript.IsActive == false ||
                        targetEnemyScript != null && targetEnemyScript.Health <= 0 ||
                        target == null || !target.activeSelf)
                    {
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

        /// <summary>
        /// Enables the rocket, initializes its target,
        /// begins the random movement and self-destruct countdown.
        /// </summary>
        /// <param name="target"> The gameobject for the rocket to home to </param>
        /// <param name="position"> The position for the rocket to launch to </param>
        public void Launch(GameObject target, Vector3 position)
        {
            if (target != null)
            {
                this.target = target;
                targetPosition = target.transform.position;
                isHoming = true;

                targetPoolableScript = target.GetComponent<Poolable>();
                targetEnemyScript = target.GetComponent<Enemy>();
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

            smoothFrac = 1.0f;
            randomMovement = new Vector3(Random.Range(-randomX, randomX),
                                         Random.Range(-randomY, randomY),
                                         Random.Range(0.0f, randomZ));
            smoothMovement = Vector3.zero;
            initialMovement = Vector3.zero;

            StartCoroutine(RandomMovement());
            StartCoroutine(SelfDestruct());
        }

        /// <summary>
        /// Enables the rocket, initializes its target,
        /// begins the random movement and self-destruct countdown.
        /// </summary>
        /// <param name="position"> The position for the rocket to launch to </param>
        public void Launch(Vector3 position)
        {
            targetPosition = position;
            isHoming = false;

            smoothFrac = 1.0f;
            randomMovement = new Vector3(Random.Range(-randomX, randomX),
                                         Random.Range(-randomY, randomY),
                                         Random.Range(0.0f, randomZ));
            smoothMovement = Vector3.zero;
            initialMovement = Vector3.zero;

            StartCoroutine(RandomMovement());
            StartCoroutine(SelfDestruct());
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
                shipController.hand.controller.TriggerHapticPulse(2500);
                Explode();
            }
            else if (other.CompareTag("Room"))
            {
                Explode();
            }
        }

        /// <summary>
        /// Generates the random movement for the rocket.
        /// </summary>
        private IEnumerator RandomMovement()
        {
            while (true)
            {
                smoothFrac = 0.0f;
                initialMovement = randomMovement;
                randomMovement = new Vector3(Random.Range(-randomX, randomX),
                                             Random.Range(-randomY, randomY),
                                             Random.Range(0.0f, randomZ));

                yield return new WaitForSeconds(randomTime);
            }
        }

        /// <summary>
        /// Explodes the rocket after a set amount of time.
        /// </summary>
        private IEnumerator SelfDestruct()
        {
            yield return new WaitForSeconds(explodeTime);

            Explode();
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
                reference.objectPoolManager.Spawn(gameObject, explosionEmitterId,
                                                  transform.position, transform.rotation);
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
                if (explosionTarget != null)
                {
                    if (enemy.gameObject.GetInstanceID() != explosionTarget.GetInstanceID())
                    {
                        enemy.gameObject.SendMessage("Hit", damage,
                                                     SendMessageOptions.DontRequireReceiver);
                    }
                }
                else
                {
                    enemy.gameObject.SendMessage("Hit", damage,
                                                 SendMessageOptions.DontRequireReceiver);
                }
            }
        }

        /// <summary>
        /// Resets rocket variables and deactivates the object.
        /// </summary>
        public override void Deactivate()
        {
            StopAllCoroutines();
            isTargetDead = false;
            explosionTarget = null;

            base.Deactivate();
        }

        /// <summary>
        /// Resets the shipController reference and target for the rocket.
        /// </summary>
        protected override void Reset()
        {
            behaviorFlags = 0;
            shipController = null;
            target = null;
            targetPoolableScript = null;
            targetEnemyScript = null;
            damageMultiplier = 1;
        }
    }
}