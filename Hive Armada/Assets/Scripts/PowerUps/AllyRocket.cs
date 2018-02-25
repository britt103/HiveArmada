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

using System.Collections;
using System.Collections.Generic;
using Hive.Armada.Enemies;
using Hive.Armada.Game;
using UnityEngine;

namespace Hive.Armada.PowerUps
{
    /// <summary>
    /// The rockets launched by the rocket pod weapon.
    /// </summary>
    public class AllyRocket : MonoBehaviour
    {
        /// <summary>
        /// Reference manager that holds all needed references
        /// (e.g. spawner, game manager, etc.)
        /// </summary>
        private ReferenceManager reference;

        /// <summary>
        /// Unique identifier for this rocket.
        /// </summary>
        public int Identifier { get; private set; }

        /// <summary>
        /// How much damage the rocket does to enemies.
        /// </summary>
        private int damage;

        /// <summary>
        /// How fast the rocket moves.
        /// </summary>
        public float speed;

        /// <summary>
        /// How strong the homing effect is.
        /// 0 = no homing
        /// 1 = perfect homing
        /// </summary>
        public float homingSensitivity;

        /// <summary>
        /// Time until the rocket auto explodes.
        /// </summary>
        public float explodeTime;

        /// <summary>
        /// Time between random rocket movements.
        /// </summary>
        [Header("Random Movement")]
        public float randomTime;

        /// <summary>
        /// Range for the random x movement.
        /// </summary>
        public float randomX;

        /// <summary>
        /// Range for the random y movement.
        /// </summary>
        public float randomY;

        /// <summary>
        /// Range for the random z movement.
        /// </summary>
        public float randomZ;

        /// <summary>
        /// Type ID for the explosion emitter.
        /// </summary>
        private int explosionEmitterID;

        /// <summary>
        /// The target object for the rocket to seek.
        /// </summary>
        private GameObject target;

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
        /// The enemy GameObject that the rocket hit.
        /// </summary>
        private GameObject explosionTarget;

        /// <summary>
        /// Coroutine for self-destruction.
        /// </summary>
        private Coroutine selfDestructCoroutine;

        /// <summary>
        /// Coroutine for random movement.
        /// </summary>
        private Coroutine randomCoroutine;

        /// <summary>
        /// The random movement to translate the rocket by.
        /// </summary>
        private Vector3 randomMovement;

        /// <summary>
        /// Moves the rocket toward its target.
        /// </summary>
        private void Update()
        {
            if (isHoming)
            {
                if (target == null || !target.activeSelf)
                {
                    target = null;

                    if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
                    {
                        Explode();
                    }
                }
                else
                {
                    targetPosition = target.transform.position;
                }
            }

            transform.Translate(0.0f, 0.0f, speed * Time.deltaTime, Space.Self);

            transform.Translate(randomMovement);

            Vector3 relativePosition = targetPosition - transform.position;

            Quaternion rotation = Quaternion.LookRotation(relativePosition);

            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, homingSensitivity);
        }

        /// <summary>
        /// Initializes the rocket and its attributes.
        /// </summary>
        /// <param name="damage"> Amount of damage rocket does </param>
        /// <param name="explosionEmitterID"> Type ID for explosion emitter </param>
        public void Initialize(int damage, int explosionEmitterID)
        {
            reference = FindObjectOfType<ReferenceManager>();
            this.damage = damage;
            this.explosionEmitterID = explosionEmitterID;
        }

        /// <summary>
        /// Enables the rocket, initializes its target,
        /// begins the random movement and self-destruct countdown.
        /// </summary>
        /// <param name="target"> The gameobject for the rocket to home to </param>
        public void Launch(GameObject target)
        {
            this.target = target;
            isHoming = true;
            randomCoroutine = StartCoroutine(RandomMovement());
            selfDestructCoroutine = StartCoroutine(SelfDestruct());
        }

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
        /// Spawns the explosion emitter and deals AoE damage.
        /// </summary>
        private void Explode()
        {
            if (explosionEmitterID >= 0)
            {
                reference.objectPoolManager.Spawn(explosionEmitterID, transform.position);
            }

            Destroy(gameObject);
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
                other.GetComponent<Enemy>().Hit(damage);
                Explode();
            }
            else if (other.CompareTag("Room"))
            {
                Explode();
            }
        }

        /// <summary>
        /// Explodes the rocket after a set amount of time.
        /// </summary>
        private IEnumerator SelfDestruct()
        {
            yield return new WaitForSeconds(explodeTime);

            Explode();
            selfDestructCoroutine = null;
        }
    }
}