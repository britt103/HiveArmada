//=============================================================================
//
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-440-01, CPSC-340-01 & CPSC-344-01
// Group Project
//
// [DESCRIPTION]
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using Hive.Armada.Enemies;
using Hive.Armada.Game;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Random = UnityEngine.Random;

namespace Hive.Armada.Player.Weapons
{
    /// <summary>
    /// The rockets launched by the rocket pod weapon.
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
            AutoTarget = 1 << 3

            ///// <summary>
            ///// If the rocket requires a target or can launch at a point.
            ///// </summary>
            //RequireTarget = 1 << 4
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
        /// How much damage to deal in the area of effect.
        /// </summary>
        private float explosiveDamage;

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
        /// The target object for the rocket to seek.
        /// </summary>
        private GameObject target;

        /// <summary>
        /// Reference to the poolable script on the target.
        /// </summary>
        private Poolable targetPoolableScript;

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
        /// Moves the rocket toward its target.
        /// </summary>
        private void Update()
        {
            if (isHoming)
            {
                if (target == null || !target.activeSelf ||
                    targetPoolableScript != null && targetPoolableScript.IsActive == false)
                {
                    if ((behaviorFlags & RocketFlags.AutoTarget) == 0)
                    {
                        target = null;
                        isHoming = false;
                        isTargetDead = true;
                    }
                    else
                    {
                        //todo add coroutine to find new target
                        AquireTarget();
                    }
                }
                else if (isTargetDead)
                {
                    targetPosition = target.transform.position;
                }
            }

            if (isTargetDead)
            {
                if (Vector3.Distance(transform.position, targetPosition) < 0.4f)
                {
                    Explode();
                }
            }

            transform.Translate(0.0f, 0.0f, speed * Time.deltaTime, Space.Self);
            transform.Translate(randomMovement);

            Vector3 relativePosition = targetPosition - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePosition);
            transform.rotation =
                Quaternion.Slerp(transform.rotation, rotation, homingSensitivity);
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
            explosiveDamage = rocketAttributes.rockets[rocketType].explosiveDamage;
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
            }
            else
            {
                targetPosition = position;
                isHoming = false;
            }

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

            StartCoroutine(RandomMovement());
            StartCoroutine(SelfDestruct());
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
        /// Finds the nearest enemy and targets it.
        /// </summary>
        /// <returns> If successfully acquired target </returns>
        private bool AquireTarget()
        {
            bool result = false;
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
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Spawns the explosion emitter and deals AoE damage.
        /// </summary>
        private void Explode()
        {
            if (explosionEmitterId >= 0)
            {
                reference.objectPoolManager.Spawn(explosionEmitterId, transform.position);
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
            //TODO implement aoe damage
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
        /// Explodes the rocket if it hits the room or an enemy.
        /// </summary>
        /// <param name="other"> The other collider </param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                explosionTarget = other.gameObject;
                other.GetComponent<Enemy>().Hit(damage * damageMultiplier);
                if ((behaviorFlags & RocketFlags.HapticFeedback) != 0)
                {
                    shipController.hand.controller.TriggerHapticPulse(2500);
                }
                Explode();
            }
            else if (other.CompareTag("Room"))
            {
                Explode();
            }
        }

        ///// <summary>
        ///// Damages the enemy that was hit and sends haptic feedback to the controller.
        ///// </summary>
        ///// <param name="enemy"> </param>
        //private void HitEnemy(Collider enemy)
        //{
        //    explosionTarget = enemy.gameObject;
        //    enemy.GetComponent<Enemy>().Hit(damage);
        //    shipController.hand.controller.TriggerHapticPulse(2500);
        //    Explode();
        //}

        /// <summary>
        /// Generates the random movement for the rocket.
        /// </summary>
        private IEnumerator RandomMovement()
        {
            while (true)
            {
                yield return new WaitForSeconds(randomTime);

                randomMovement = new Vector3(Random.Range(-randomX, randomX),
                                             Random.Range(-randomY, randomY),
                                             Random.Range(0, randomZ));
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
        /// Resets the shipController reference and target for the rocket.
        /// </summary>
        protected override void Reset()
        {
            shipController = null;
            target = null;
            targetPoolableScript = null;
            damageMultiplier = 1;
        }
    }
}