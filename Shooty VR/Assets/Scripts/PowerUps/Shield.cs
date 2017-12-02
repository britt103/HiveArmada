//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.champan.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// Shield controls the Shield powerup. The Shield absorbs enemy projectiles
// that would otherwise collide with the player ship. It flahses as a warning
// when it's time limit is almost up, after which it self-destructs. Currently
// assigned as Powerup 5.
//
//=============================================================================

using UnityEngine;

namespace Hive.Armada.PowerUps
{
    /// <summary>
    /// Shield powerup.
    /// </summary>
    public class Shield : MonoBehaviour
    {
        /// <summary>
        /// Time until self-destruct.
        /// </summary>
        public float timeLimit;

        /// <summary>
        /// Time at which warning flashes start.
        /// </summary>
        public float startFlashingTime;

        /// <summary>
        /// Duration of individual flashes.
        /// </summary>
        public float flashDuration;

        /// <summary>
        /// Duration of current flash.
        /// </summary>
        private float currentFlashDuration = 0.0F;

        /// <summary>
        /// Multiplied with flashDuration to accelerate flash interval duration.
        /// </summary>
        public float flashAcceleration = 0.9f;

        /// <summary>
        /// State of whether Shield is currently flashing or not.
        /// </summary>
        private bool flashState = false;

        /// <summary>
        /// Rotation Shield uses to spin.
        /// </summary>
        public Vector3 rotationVector = new Vector3(0.0F, 0.0F, 0.0F);

        /// <summary>
        /// Emitter that plays when a bullet hits the shield.
        /// </summary>
        public GameObject bulletHitEmitter;

        /// <summary>
        /// Subtract from and check timeLimit. Start flashing when warningTime is reached. Rotate.
        /// </summary>
        void Update()
        {
            timeLimit -= Time.deltaTime;
            if (timeLimit <= 0.0F)
            {
                Destroy(gameObject);
            }

            if (timeLimit <= startFlashingTime)
            {
                Flash();
            }

            transform.Rotate(rotationVector);
        }

        /// <summary>
        /// Destroy enemy projectiles upon impact.
        /// </summary>
        /// <param name="other">Collider of object with which this collided.</param>
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("bullet"))
            {
                Instantiate(bulletHitEmitter, transform.position, transform.rotation);
                Destroy(other.gameObject);
            }
        }

        /// <summary>
        /// Alternate mesh renderer status for warning flash effect.
        /// </summary>
        private void Flash()
        {
            currentFlashDuration += Time.deltaTime;
            if (currentFlashDuration >= flashDuration)
            {
                gameObject.GetComponent<MeshRenderer>().enabled = flashState;
                flashState = !flashState;
                flashDuration *= flashAcceleration;
                currentFlashDuration = 0.0F;
            }
        }
    }
}