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
using Hive.Armada.Game;
using UnityEngine;

namespace Hive.Armada.Player
{
    /// <summary>
    /// Handles the player ship's health.
    /// </summary>
    public class PlayerHealth : MonoBehaviour
    {
        public ShipController shipController;

        public int maxHealth = 30;

        private int currentHealth;

        public GameObject fxHit;

        public GameObject fxHurt;

        public GameObject fxDead;

        protected List<Material> mats;

        public Material flashColor;

        /// <summary>
        /// Initializes variables
        /// </summary>
        private void Start()
        {
            mats = new List<Material>();
            currentHealth = maxHealth;
        }

        /// <summary>
        /// Deals damage to the player ship
        /// </summary>
        /// <param name="damage"> How much damage to deal </param>
        public void Hit(int damage)
        {
            Instantiate(fxHit, transform);
            currentHealth -= damage;

            if (Utility.isDebug)
            {
                Debug.Log("Hit for " + damage + " damage! Remaining health = " + currentHealth);
            }

            if (currentHealth <= 10)
            {
                fxHurt.SetActive(true);
            }

            if (currentHealth <= 0)
            {
                if (shipController != null)
                {
                    FindObjectOfType<ReferenceManager>().statistics.IsNotAlive();
                    Instantiate(fxDead, transform.position, transform.rotation);
                    GameObject.Find("Main Canvas").transform.Find("Game Over Menu").gameObject
                              .SetActive(true);
                    shipController.hand.DetachObject(gameObject);
                }
            }

            StartCoroutine(HitFlash());
        }

        /// <summary>
        /// Flashes the playership when hit
        /// </summary>
        private IEnumerator HitFlash()
        {
            foreach (Renderer renderer in gameObject.GetComponentsInChildren<Renderer>())
            {
                if (renderer.gameObject.CompareTag("FX"))
                {
                    continue;
                }
                if (renderer.material.name.Equals("Laser Sight") ||
                    renderer.material.name.Equals("Laser Gun"))
                {
                    continue;
                }

                mats.Add(renderer.material);

                renderer.material = flashColor;
            }

            yield return new WaitForSeconds(0.05f);

            foreach (Renderer renderer in gameObject.GetComponentsInChildren<Renderer>())
            {
                if (renderer.gameObject.CompareTag("FX"))
                {
                    continue;
                }
                if (renderer.material.name.Equals("Laser Sight") ||
                    renderer.material.name.Equals("Laser Gun"))
                {
                    continue;
                }

                renderer.material = mats.First();
                mats.RemoveAt(0);
            }
        }
    }
}