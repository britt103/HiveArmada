//=============================================================================
//
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// A basic projectile. It will destroy itself after a set amount of time,
// after colliding with the room, or the player. It damages the player
// by a set amount.
//
//=============================================================================

using System.Collections;
using UnityEngine;
using Hive.Armada.Game;
using Hive.Armada.Player;

namespace Hive.Armada.Enemies
{
    /// <summary>
    /// Basic projectile used by all shooting enemies.
    /// </summary>
    public class Projectile : Poolable
    {
        /// <summary>
        /// Reference manager that holds all needed references
        /// (e.g. spawner, game manager, etc.)
        /// </summary>
        private ReferenceManager reference;

        /// <summary>
        /// The renderer for the projectile.
        /// </summary>
        private Material material;

        /// <summary>
        /// The amount of damage the projectile takes from the player's health 
        /// </summary>
        private int damage;

        private Color originalAlbedo;

        private Color originalEmission;

        private float minAlpha;

        /// <summary>
        /// Initializes the reference to the Reference Manager
        /// </summary>
        private void Awake()
        {
            reference = GameObject.Find("Reference Manager").GetComponent<ReferenceManager>();
            material = GetComponent<Renderer>().material;
            originalAlbedo = material.GetColor("_Color");
            originalEmission = material.GetColor("_EmissionColor");
        }

        /// <summary>
        /// Runs when the projectile collides with another object with a Collider.
        /// </summary>
        /// <param name="other"> The other collider </param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (Utility.isDebug)
                {
                    Debug.Log(GetType().Name + " - hit object named \"" + other.gameObject.name + "\"");
                }

                reference.playerShip.GetComponent<PlayerHealth>().Hit(damage);
                reference.objectPoolManager.Despawn(gameObject);
            }
            else if (other.CompareTag("Room") || other.CompareTag("ProjectileBounds"))
            {
                reference.objectPoolManager.Despawn(gameObject);
            }
        }

        /// <summary>
        /// Sets the albedo and emission color for the projectile.
        /// </summary>
        /// <param name="albedoColor"> The albedo color </param>
        /// <param name="emissionColor"> The emission color </param>
        public void SetColors(Color albedoColor, Color emissionColor)
        {
            material.SetColor("_Color", albedoColor);
            material.SetColor("_EmissionColor", emissionColor);
        }

        public void FadeOpacity(bool fadeOut)
        {

        }

        private IEnumerator Fade(bool fadeOut)
        {
            yield return null;
        }

        /// <summary>
        /// Initializes the damage for the projectile.
        /// </summary>
        protected override void Reset()
        {
            SetColors(originalAlbedo, originalEmission);
            damage = reference.enemyAttributes.projectileDamage;
        }
    }
}