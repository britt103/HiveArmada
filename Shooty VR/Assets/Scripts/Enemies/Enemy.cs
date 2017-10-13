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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShootyVR.Enemies
{
    public abstract class Enemy : MonoBehaviour
    {
        /// <summary>
        /// The starting health for the enemy.
        /// </summary>
        public int maxHealth;

        /// <summary>
        /// The current health for the enemy.
        /// </summary>
        protected int health;

        /// <summary>
        /// The material that this
        /// </summary>
        public Material flashColor;

        /// <summary>
        /// Reference to the original material of the game object.
        /// </summary>
        protected Material material;


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
        public abstract void Hit(int damage);
        //{
        //    health -= damage;
        //    StartCoroutine(HitFlash());
        //}

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
            Destroy(gameObject);
        }

        /// <summary>
        /// Visual feedback when the enemy is hit. Flashes the material using flashColor.
        /// Calls Kill() if the enemy is out of health. Adds to the score via GameManager.
        /// </summary>
        /// <returns>  </returns>
        protected abstract IEnumerator HitFlash();
        //{
        //    gameObject.GetComponent<Renderer>().material = flashColor;
        //    yield return new WaitForSeconds(.01f);

        //    if (health <= 0)
        //    {
        //        Destroy(gameObject);
        //    }
        //    gameObject.GetComponent<Renderer>().material = material;
        //}
    }
}
