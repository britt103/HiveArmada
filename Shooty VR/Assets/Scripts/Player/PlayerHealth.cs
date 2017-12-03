//=============================================================================
// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
// 
// This class handles the health for the player ship. If the player
// dies, it tells the hand to drop the ship (which destroys it).
// 
//=============================================================================

using SubjectNerd.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Hive.Armada.Game;
using UnityEngine;
using Hive.Armada.Game;

namespace Hive.Armada.Player
{
    /// <summary>
    /// Handles the player ship's health.
    /// </summary>
    public class PlayerHealth : MonoBehaviour
    {
        /// <summary>
        /// Reference to the ship controller script.
        /// </summary>
        public ShipController shipController;

        /// <summary>
        /// Maximum health for the ship.
        /// </summary>
        public int maxHealth = 30;

        /// <summary>
        /// Current health for the ship.
        /// </summary>
        private int currentHealth;

        /// <summary>
        /// Material that the ship flashes when it is hit.
        /// </summary>
        [Header("Health Feedback")]




        public Material flashColor;

        private ReferenceManager reference;
        /// <summary>
        /// Renderers for the game objects on the back of the ship
        /// that represent how many hits the player can take.
        /// </summary>
        [Tooltip("Health pods on the back of the ship that represent how" +
            " many hits the player can take before losing.")]
        [Reorderable("Health Pod", false)]
        public Renderer[] healthPods;

        /// <summary>
        /// Material for intact health pods.
        /// </summary>
        [Tooltip("Material for intact health pods.")]
        public Material podIntactMaterial;

        /// <summary>
        /// Material for destroyed health pods.
        /// </summary>
        [Tooltip("Material for destroyed health pods.")]
        public Material podDestroyedMaterial;

        /// <summary>
        /// Particle emitter that spawns when the ship is hit.
        /// </summary>
        [Header("Emitters")]
        [Tooltip("Particle emitter that spawns when the ship is hit.")]
        public GameObject hitEmitter;

        /// <summary>
        /// Particle emitter that spawns on a health pod when it blows up.
        /// </summary>
        [Tooltip("Particle emitter that spawns on a health pod when it blows up.")]
        public GameObject podHitEmitter;

        /// <summary>
        /// Particle emitter that is activated when the ship has 1 hit left.
        /// </summary>
        [Tooltip("Particle emitter that is activated when the ship has 1 hit left.")]
        public GameObject hurtEmitter;

        /// <summary>
        /// Particle emitter that spawns when the player dies.
        /// </summary>
        [Tooltip("Particle emitter that spawns when the player dies.")]
        public GameObject deathEmitter;

        /// <summary>
        /// Used to prevent HitFlash() from being called a
        /// second time before it is done flashing
        /// </summary>
        private Coroutine hitFlash;

        /// <summary>
        /// List of Renderers on the player ship that are not emitters.
        /// </summary>
        private List<Renderer> renderers;

        /// <summary>
        /// List of Materials of all pieces of the player ship model.
        /// Used to reset Materials after flashing.
        /// </summary>
        private List<Material> materials;

        /// <summary>
        /// Audio source for playing sounds when hit.
        /// </summary>
        [Header("Audio")]
        public AudioSource source;

        /// <summary>
        /// The sound that plays when the ship is hit.
        /// </summary>
        public AudioClip hitSound;

        /// <summary>
        /// Initializes health and renderers for hit flashing
        /// </summary>
        private void Start()
        {
            renderers = new List<Renderer>();
            materials = new List<Material>();

            for (int i = 0; i < 3; ++i)
            {
                healthPods[i].material = podIntactMaterial;
            }

            foreach (Renderer r in gameObject.GetComponentsInChildren<Renderer>())
            {
                if (r.gameObject.CompareTag("Emitter") ||
                    r.transform.parent.CompareTag("Emitter") ||
                    r.gameObject.CompareTag("FX") ||
                    r.transform.parent.CompareTag("FX"))
                {
                    continue;
                }

                if (r.gameObject.name.Contains("pod_"))
                {
                    continue;
                }

                renderers.Add(r);
                materials.Add(r.material);
            }

            currentHealth = maxHealth;

            reference = FindObjectOfType<ReferenceManager>();
        }


        /// <summary>
        /// Deals damage to the player ship.
        /// </summary>
        /// <param name="damage"> How much damage to deal </param>
        public void Hit(int damage)
        {
            int podIndex = (currentHealth - maxHealth) / 10;
            healthPods[podIndex].material = podDestroyedMaterial;

            Instantiate(hitEmitter, transform);
            currentHealth -= damage;

            if (Utility.isDebug)
            {
                Debug.Log("Hit for " + damage + " damage! Remaining health = " + currentHealth);
            }

            if (currentHealth <= 0)
            {
                if (shipController != null)
                {
                    Instantiate(deathEmitter, transform.position, transform.rotation);
                    reference.statistics.IsNotAlive();
                    reference.sceneTransitionManager.TransitionOut("Menu Room");
                    shipController.hand.DetachObject(gameObject);
                }
            }

            if (hitFlash == null)
            {
                hitFlash = StartCoroutine(HitFlash());
            }
        }

        /// <summary>
        /// Flashes the player ship when hit
        /// </summary>
        private IEnumerator HitFlash()
        {
            // "flash" materials to flashColor
            foreach (Renderer r in renderers)
            {
                r.material = flashColor;
            }

            yield return new WaitForSeconds(0.05f);

            // reset materials
            for (int i = 0; i < renderers.Count; ++i)
            {
                renderers[i].material = materials[i];
            }

            hitFlash = null;
        }
    }
}