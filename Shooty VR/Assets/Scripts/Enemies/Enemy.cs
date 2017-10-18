//=============================================================================
// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
// 
// This abstract class is the base for all enemies.
// It handles all fields and methods related to
// health and taking damage.
// 
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameName.Enemies
{
    public abstract class Enemy : MonoBehaviour
    {
        public int maxHealth;
        protected int health;
        public Material flashColor;
        protected Material material;
        protected WaveManager waveManager;

        //void Start()
        //{
        //    StartCoroutine(InitEnemy());
        //}

        /// <summary>
        /// Initializes variables for the enemy when it loads.
        /// </summary>
        public virtual void Awake()
        {
            health = maxHealth;
            material = gameObject.GetComponent<Renderer>().material;
            waveManager = GameObject.FindGameObjectWithTag("Wave").GetComponent<WaveManager>();
        }

        /// <summary>
        /// The current health for the enemy.
        /// </summary>
        /// <returns> Integer health value </returns>
        public virtual int GetHealth()
        {
            return health;
        }

        /// <summary>
        /// Used to apply damage to an enemy.
        /// </summary>
        /// <param name="damage"> How much damage this enemy is taking. </param>
        public virtual void Hit(int damage)
        {
            health -= damage;
            StartCoroutine(HitFlash());
        }

        /// <summary>
        /// Currently unused. Flashes and destroys the enemy when it collides with the player.
        /// </summary>
        public virtual void Collide()
        {
            health = 0;
            StartCoroutine(HitFlash());
        }

        /// <summary>
        /// Destroys this GameObject without flashing.
        /// </summary>
        protected virtual void Kill()
        {
            waveManager.currDead++;
            Destroy(gameObject);
        }

        /// <summary>
        /// Visual feedback when the enemy is hit. Flashes the material using flashColor.
        /// Calls Kill() if the enemy is out of health. Adds to the score via GameManager.
        /// </summary>
        /// <returns>  </returns>
        protected virtual IEnumerator HitFlash()
        {
            gameObject.GetComponent<Renderer>().material = flashColor;
            yield return new WaitForSeconds(.01f);

            if (health <= 0)
            {
                Kill();
            }
            gameObject.GetComponent<Renderer>().material = material;
        }

        ///// <summary>
        ///// Initializes enemy health and base material for flashing.
        ///// Also finds the wave manager in scene and adds a spawn value.
        ///// </summary>
        ///// <returns></returns>
        //IEnumerator InitEnemy()
        //{
        //    health = maxHealth;
        //    material = gameObject.GetComponent<Renderer>().material;
        //    if (waveManager != null)
        //    {
        //        waveManager = GameObject.FindGameObjectWithTag("Wave").GetComponent<WaveManager>();
        //        waveManager.currSpawn++;
        //    }
        //    yield return new WaitForSeconds(3);
        //    Kill();   //just to test death without headset
        //}
    }
}
