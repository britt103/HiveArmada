//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.champan.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// Powerup controls interaction with powerup pickup objects. When the player
// interacts with the pickup, they recieve a powerup corresponding to Powerup's
// pickup prefab. The player can only receive the powerup if they do not 
// already have a powerup of that type currently stored.
//
//=============================================================================

using UnityEngine;
using Hive.Armada.Game;

namespace Hive.Armada.PowerUps
{
    public class PowerUp : MonoBehaviour
    {
        /// <summary>
        /// Reference manager that holds all needed references
        /// (e.g. spawner, game manager, etc.)
        /// </summary>
        private ReferenceManager reference;

        /// <summary>
        /// Reference to pickup prefab.
        /// </summary>
        public GameObject powerupPrefab;

        /// <summary>
        /// Reference to powerup icon prefab.
        /// </summary>
        public GameObject powerupIconPrefab;

        /// <summary>
        /// Particle emitter for when the pickup spawns.
        /// </summary>
        public GameObject spawnEmitter;

        /// <summary>
        /// Particle emitter that persists on the pickup.
        /// </summary>
        public GameObject pickupEmitter;

        /// <summary>
        /// Reference to PowerUpStatus.
        /// </summary>
        private PowerUpStatus status;

        //Reference to player head transform.
        private Transform head;

        /// <summary>
        /// Time until self-destruct.
        /// </summary>
        public float lifeTime = 10.0f;

        /// <summary>
        /// State of whether pickup has been touched.
        /// </summary>
        private bool touched = false;

        /// <summary>
        /// Audio clip to play when player collides with icon
        /// </summary>
        public AudioClip clip;

        /// <summary>
        /// Source to play audio clip from
        /// </summary>
        AudioSource source;

        /// <summary>
        /// Find references. Instantiate and rotate FX. Start self-destruct countdown.
        /// </summary>
        private void Awake()
        {
            reference = GameObject.Find("Reference Manager").GetComponent<ReferenceManager>();

            source = GameObject.Find("Powerup Audio Source").GetComponent<AudioSource>();

            head = GameObject.Find("VRCamera").transform;

            if (spawnEmitter)
            {
                Instantiate(spawnEmitter, transform.position, transform.rotation, transform);
            }
            if (pickupEmitter)
            {
                Instantiate(pickupEmitter, transform.position, transform.rotation, transform);
            }
            
            status = reference.powerUpStatus;
            Destroy(transform.gameObject, lifeTime);
        }

        /// <summary>
        /// Update rotation to face player.
        /// </summary>
        private void Update()
        {
            gameObject.transform.LookAt(head);
        }

        /// <summary>
        /// Give player powerup upon collision, then self-destruct.
        /// </summary>
        /// <param name="other">Collider of object with which this collided.</param>
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && status.HasRoom() && !status.HasPowerup(powerupPrefab) 
                && !touched)
            {
                touched = true;
                reference.playerShipSource.PlayOneShot(reference.powerupReadySound);
                status.StorePowerup(powerupPrefab, powerupIconPrefab);
                source.PlayOneShot(clip);
                Destroy(transform.gameObject);
            }
        }
    }
}