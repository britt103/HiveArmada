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

using System;
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

        /// <summary>
        /// The original albedo color, used to reset the color.
        /// </summary>
        private Color originalAlbedo;

        /// <summary>
        /// The original emission color, used to reset the color.
        /// </summary>
        private Color originalEmission;

        /// <summary>
        /// The current albedo color.
        /// </summary>
        private Color currentAlbedo;

        /// <summary>
        /// The current alpha for this projectile in the range [0.0f, 1.0f].
        /// </summary>
        private float currentAlpha = 1.0f;

        /// <summary>
        /// The minimum value for the alpha.
        /// </summary>
        private const float MIN_ALPHA = 0.1f;

        /// <summary>
        /// Length of the fade in seconds.
        /// </summary>
        private const float FADE_TIME = 0.08f;

        /// <summary>
        /// Number of steps the fade should be broken into.
        /// </summary>
        private const int FADE_STEPS = 60;

        /// <summary>
        /// The coroutine for fading the projectile in or out.
        /// </summary>
        private Coroutine fadeCoroutine;

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

                if (reference.playerShip != null)
                {
                    reference.playerShip.GetComponent<PlayerHealth>().Hit(damage);
                }
                
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
            currentAlbedo = albedoColor;
            material.SetColor("_Color", albedoColor);
            material.SetColor("_EmissionColor", emissionColor);
        }

        /// <summary>
        /// Begins fading the opacity of the projectile.
        /// </summary>
        /// <param name="fadeOut"> If the projectile should fade out </param>
        public void FadeOpacity(bool fadeOut)
        {
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }

            fadeCoroutine = StartCoroutine(Fade(fadeOut));
        }

        /// <summary>
        /// Fades the opacity of the projectile.
        /// </summary>
        /// <param name="fadeOut"> If the projectile should fade out </param>
        private IEnumerator Fade(bool fadeOut)
        {
            float fadeStep = (1.0f - MIN_ALPHA) / FADE_STEPS * (fadeOut ? -1.0f : 1.0f);
            float target = fadeOut ? MIN_ALPHA : 1.0f;
            WaitForSeconds stepTime = new WaitForSeconds((1.0f - MIN_ALPHA) / FADE_STEPS * FADE_TIME);

            while (Math.Abs(currentAlpha - target) > 0.001f)
            {
                currentAlpha += fadeStep;
                currentAlbedo.a = currentAlpha;
                material.SetColor("_Color", currentAlbedo);
                yield return stepTime;

                if (currentAlpha < MIN_ALPHA || currentAlpha > 1.0f)
                {
                    currentAlpha = fadeOut ? MIN_ALPHA : 1.0f;
                    currentAlbedo.a = currentAlpha;
                    material.SetColor("_Color", currentAlbedo);
                    break;
                }
            }

            fadeCoroutine = null;
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