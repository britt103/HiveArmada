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
using Hive.Armada.Data;
using Hive.Armada.Game;
using SubjectNerd.Utilities;
using UnityEngine;

namespace Hive.Armada.Player
{
    /// <summary>
    /// Handles the player ship's health.
    /// </summary>
    public class PlayerHealth : MonoBehaviour
    {
        /// <summary>
        /// Reference manager that holds all needed references
        /// (e.g. spawner, game manager, etc.)
        /// </summary>
        private ReferenceManager reference;

        public PlayerData playerData;

        /// <summary>
        /// Reference to the ship controller script.
        /// </summary>
        public ShipController shipController;

        private ProjectileData projectileData;

        /// <summary>
        /// Maximum health for the ship.
        /// </summary>
        private int maxHealth;

        /// <summary>
        /// Current health for the ship.
        /// </summary>
        private int currentHealth;

        /// <summary>
        /// Material that the ship flashes when it is hit.
        /// </summary>
        private Material flashColor;

        /// <summary>
        /// Renderers for the game objects on the back of the ship
        /// that represent how many hits the player can take.
        /// </summary>
        [Tooltip("Health pods on the back of the ship that represent how" +
                 " many hits the player can take before losing.")]
        [Reorderable("Health Pod", false)]
        public GameObject[] healthPods;

        /// <summary>
        /// Material for intact health pods.
        /// </summary>
        private Material podIntactMaterial;

        /// <summary>
        /// Material for destroyed health pods.
        /// </summary>
        private Material podDestroyedMaterial;

        /// <summary>
        /// Particle emitter that spawns when the player dies.
        /// </summary>
        private GameObject deathEmitter;

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
        public AudioSource source;

        /// <summary>
        /// The sound that plays when the ship is hit.
        /// </summary>
        private AudioClip hitSound;

        private AudioClip lowHealthSound;

        private AudioClip healSound;

        /// <summary>
        /// The look target for the player ship.
        /// </summary>
        private GameObject lookTarget;

        /// <summary>
        /// Reference to PlayerHitVignette.
        /// </summary>
        private PlayerHitVignette playerHitVignette;

        /// <summary>
        /// Initializes health and renderers for hit flashing
        /// </summary>
        private void Awake()
        {
            reference = FindObjectOfType<ReferenceManager>();

            projectileData = reference.projectileData;

            lookTarget = reference.shipLookTarget;
            lookTarget.transform.parent = transform;
            lookTarget.transform.position = transform.position;
            lookTarget.transform.rotation = transform.rotation;

            int skin = (int) reference.gameSettings.selectedSkin;
            podIntactMaterial = playerData.shipBodyMaterials[skin];
            podDestroyedMaterial = playerData.shipPodDestroyedMaterials[skin];

            renderers = new List<Renderer>();
            materials = new List<Material>();

            for (int i = 0; i < 3; ++i)
            {
                healthPods[i].GetComponent<Renderer>().material = podIntactMaterial;
            }

            foreach (Renderer r in gameObject.GetComponentsInChildren<Renderer>())
            {
                if (r.gameObject.CompareTag("Emitter") || r.transform.parent.CompareTag("Emitter"))
                {
                    continue;
                }

                renderers.Add(r);
                materials.Add(r.material);
            }

            maxHealth = playerData.playerMaxHealth;
            currentHealth = maxHealth;
            playerHitVignette = reference.playerHitVignette;

            flashColor = playerData.flashColor;

            deathEmitter = playerData.deathEmitter;

            hitSound = playerData.hitSound;
            lowHealthSound = playerData.lowHealthSound;
            healSound = playerData.healSound;
        }

        /// <summary>
        /// Deals damage to the player ship.
        /// </summary>
        /// <param name="damage"> How much damage to deal </param>
        public void Hit(int damage)
        {
            if (currentHealth <= 0)
            {
                return;
            }

            int podIndex = (maxHealth - currentHealth) / projectileData.projectileDamage;
            
            if (!reference.cheats.godMode)
                healthPods[podIndex].GetComponent<Renderer>().material = podDestroyedMaterial;

            ParticleSystem mPodEmitter = healthPods[podIndex]
                                         .transform.GetChild(0).GetComponent<ParticleSystem>();
            mPodEmitter.Clear();
            mPodEmitter.Play();

            if (!reference.cheats.godMode)
                currentHealth -= damage;

            source.PlayOneShot(hitSound);
            playerHitVignette.Hit();

            if (currentHealth == playerData.playerLowHealth)
            {
                if (lowHealthSound != null)
                {
                    source.PlayOneShot(lowHealthSound);
                }
            }

            if (currentHealth <= 0)
            {
                if (shipController != null)
                {
                    reference.playerShip = null;
                    lookTarget.transform.parent = null;
                    Instantiate(deathEmitter, transform.position, transform.rotation);
                    reference.statistics.IsNotAlive();
                    reference.powerUpStatus.tracking = false;
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

            yield return Utility.waitHitFlash;

            // reset materials
            for (int i = 0; i < renderers.Count; ++i)
            {
                renderers[i].material = materials[i];
            }

            hitFlash = null;
        }

        /// <summary>
        /// Restores one health to the player.
        /// </summary>
        private bool HealOne()
        {
            if ((maxHealth - currentHealth) / 10 == 0)
                return false;

            currentHealth += 10;

            int podIndex = (maxHealth - currentHealth) / 10;
            healthPods[podIndex].GetComponent<Renderer>().material = podIntactMaterial;
            return true;
        }

        /// <summary>
        /// Restores all health to the player.
        /// </summary>
        public void HealFull()
        {
            if (currentHealth >= maxHealth)
            {
                return;
            }

            currentHealth = maxHealth;

            foreach (GameObject healthPod in healthPods)
            {
                healthPod.GetComponent<Renderer>().material = podIntactMaterial;
            }

            //while (currentHealth < maxHealth)
            //{
            //    currentHealth += 10;

            //    int podIndex = (maxHealth - currentHealth) / 10;
            //    healthPods[podIndex].GetComponent<Renderer>().material = podIntactMaterial;
            //    StartCoroutine(PlayHealSound());
            //}
        }

        private IEnumerator PlayHealSound()
        {
            if (source.isPlaying)
            {
                yield return new WaitWhile(() => source.isPlaying);

                source.PlayOneShot(healSound);
            }
            else
            {
                source.PlayOneShot(healSound);
            }
        }
    }
}