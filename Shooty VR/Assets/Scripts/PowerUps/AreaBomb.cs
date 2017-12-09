//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.champan.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// AreaBomb controls the Area Bomb powerup. The Area Bomb accelerates forward
// and detonates after collision with an enemy, collision with the room, 
// being shot, or it's time limit has run out. Detonation destroys any enemies 
// within a given radius. Currently assigned as Powerup 2.
//
//=============================================================================

using System.Collections;
using UnityEngine;

namespace Hive.Armada.PowerUps
{
    /// <summary>
    /// Area Bomb powerup.
    /// </summary>
    public class AreaBomb : Shootable
    {
        /// <summary>
        /// Radius of Physics Sphere used for detonation damage. 
        /// </summary>
        public float radius;

        /// <summary>
        /// Acceleration affecting the Area Bomb's speed.
        /// </summary>
        public float acceleration;

        /// <summary>
        /// Time when game object stops accelerating and starts decelerating.
        /// </summary>
        public float accelerationSwitchTime;

        /// <summary>
        /// Forward distance from player ship.
        /// </summary>
        public float startingZ;

        /// <summary>
        /// Area Bomb current speed.
        /// </summary>
        private float currentSpeed;

        /// <summary>
        /// State of whether bomb has been stopped by a boundary.
        /// </summary>
        private bool stopped = false;

        /// <summary>
        /// Particle emitter for area bomb movement trail.
        /// </summary>
        public GameObject trailEmitter;

        /// <summary>
        /// Time until detonation without player interaction.
        /// </summary>
        public float detonationTime;

        /// <summary>
        /// Time when game object becomes shootable.
        /// </summary>
        public float isShootableTime;

        /// <summary>
        /// Reference to audio source.
        /// </summary>
		public AudioSource source;

        /// <summary>
        /// Clips to use with source.
        /// </summary>
        public AudioClip[] clips;

        /// <summary>
        /// Start TimeDetonate countdown. Activate trail FX. Set transform.
        /// </summary>
        protected override void Awake()
        {
            StartCoroutine(TimeDetonate());
            trailEmitter.SetActive(true);

			source.PlayOneShot(clips[0]);

            transform.localPosition = new Vector3(0, 0, startingZ);
            gameObject.transform.parent = null;
            isShootable = false;
            StartCoroutine(MakeShootable());
        }

        /// <summary>
        /// Move and adjust current speed using acceleration if not stopped.
        /// </summary>
        void Update()
        {
            if (!stopped)
            {
                accelerationSwitchTime -= Time.deltaTime;

                if (accelerationSwitchTime > 0)
                {
                    currentSpeed += acceleration * Time.deltaTime;
                }
                else
                {
                    currentSpeed -= acceleration * Time.deltaTime;
                }

                transform.Translate(Vector3.forward * Mathf.Max(currentSpeed, 0.0f));
            }
        }

        /// <summary>
        /// Change isShootable state after set amount of time;
        /// </summary>
        private IEnumerator MakeShootable()
        {
            yield return new WaitForSeconds(isShootableTime);
            isShootable = true;
        }

        /// <summary>
        /// Trigger detonation when shot by player. 
        /// </summary>
        public override void Shot()
        {
            Detonate();
        }

        /// <summary>
        /// Trigger detonation on impact with enemy or stop on impact with bomb bounds.
        /// </summary>
        /// <param name="other">Collider of object with which this collided.</param>
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                Detonate();
            }

            else if(other.CompareTag("Bomb Bounds"))
            {
                stopped = true;
            }
        }

        /// <summary>
        /// Detonate after set time.
        /// </summary>
        private IEnumerator TimeDetonate()
        {
            yield return new WaitForSeconds(detonationTime);
            Detonate();
        }

        /// <summary>
        /// Destroy enemies within certain radius. Activate detonation FX. Self-destruct.
        /// </summary>
        private void Detonate()
        {
            foreach (Collider objectCollider in Physics.OverlapSphere(transform.position, radius))
            {
                if (objectCollider.gameObject.tag == "Enemy")
                {
                    objectCollider.gameObject.GetComponent<Enemies.Enemy>().Hit(100);
                }
            }
            Instantiate(shotEmitter, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
