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

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        /// Particle emitter that spawns when the ship is hit.
        /// </summary>
        public GameObject hitEmitter;

        /// <summary>
        /// Particle emitter that is activated when the ship has 1 hit left.
        /// </summary>
        public GameObject hurtEmitter;

        /// <summary>
        /// Particle emitter that spawns when the player dies.
        /// </summary>
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
        /// Material that the ship flashes when it is hit.
        /// </summary>
        public Material flashColor;

        /// <summary>
        /// Audio source for playing sounds when hit.
        /// </summary>
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

            currentHealth = maxHealth;
        }


        /// <summary>
        /// Deals damage to the player ship
        /// </summary>
        /// <param name="damage"> How much damage to deal </param>
        public void Hit(int damage)
        {
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
                    GameObject.Find("Main Canvas").transform.Find("Game Over Menu").gameObject
                              .SetActive(true);
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
                renderers.ElementAt(i).material = materials.ElementAt(i);
            }

            hitFlash = null;
        }
    }
}