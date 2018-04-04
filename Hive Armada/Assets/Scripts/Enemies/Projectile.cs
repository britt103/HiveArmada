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
        private Rigidbody pRigidbody;

        /// <summary>
        /// The renderer for the projectile.
        /// </summary>
        private Material material;

        /// <summary>
        /// The amount of damage the projectile takes from the player's health
        /// </summary>
        private int damage;

        /// <summary>
        /// </summary>
        public byte ProjectileId { get; private set; }

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
        /// The maximum value for the alpha.
        /// </summary>
        private const float MAX_ALPHA = 1.0f;

        /// <summary>
        /// The minimum value for the alpha.
        /// </summary>
        private const float MIN_ALPHA = 0.1f;

        /// <summary>
        /// Length of the fade in seconds.
        /// </summary>
        private const float FADE_TIME = 0.5f;

        /// <summary>
        /// Number of steps the fade should be broken into.
        /// </summary>
        private const int FADE_STEPS = 60;

        /// <summary>
        /// The coroutine for fading the projectile in or out.
        /// </summary>
        private Coroutine fadeCoroutine;

        /// <summary>
        /// The coroutine for the time warp effect.
        /// </summary>
        private Coroutine timeWarpCoroutine;

        /// <summary>
        /// Initializes the reference to the Reference Manager
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            pRigidbody = GetComponent<Rigidbody>();
            material = GetComponent<Renderer>().material;
            originalAlbedo = material.GetColor("_Color");
            originalEmission = material.GetColor("_EmissionColor");
            currentAlpha = 1.0f;
        }

        /// <summary>
        /// Sets this projectile's speed ID number.
        /// </summary>
        /// <param name="id"> The ID to use </param>
        public void Launch(byte id)
        {
            ProjectileId = id;

            pRigidbody.velocity = transform.forward *
                                  reference.enemyAttributes.projectileSpeeds[ProjectileId];

            if (reference.enemyAttributes.IsTimeWarped)
            {
                StartTimeWarp();
            }
        }

        /// <summary>
        /// Sets the velocity of the projectile.
        /// </summary>
        /// <param name="velocity"> The new velocity </param>
        public void SetVelocity(float velocity)
        {
            pRigidbody.velocity = transform.forward * velocity;
        }

        /// <summary>
        /// Initiates the time warp functionality for the projectile.
        /// </summary>
        public void StartTimeWarp()
        {
            if (timeWarpCoroutine != null)
            {
                StopCoroutine(timeWarpCoroutine);
            }

            timeWarpCoroutine = StartCoroutine(TimeWarp());
        }

        /// <summary>
        /// Updates the velocity as long as the time warp is active.
        /// </summary>
        private IEnumerator TimeWarp()
        {
            while (reference.enemyAttributes.IsTimeWarped)
            {
                pRigidbody.velocity = transform.forward *
                                      reference.enemyAttributes.projectileSpeeds[ProjectileId];

                yield return null;
            }

            timeWarpCoroutine = null;
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
                    Debug.Log(GetType().Name + " - hit object named \"" + other.gameObject.name +
                              "\"");
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
            float t = 0.0f;
            float alphaScale;

            // Scale based on current alpha. So partially faded projectiles fade quicker.
            if (fadeOut)
            {
                alphaScale = (currentAlpha - MIN_ALPHA) / (MAX_ALPHA - MIN_ALPHA);
            }
            else
            {
                alphaScale = (MAX_ALPHA - currentAlpha) / (MAX_ALPHA - MIN_ALPHA);
            }

            // Scaled based on projectile speed.
            // Projectiles that are time -warped should fade slower.
            float speedScale = reference.enemyAttributes.projectileSpeeds[ProjectileId] /
                               reference.enemyAttributes.projectileSpeedBounds[ProjectileId]
                                        .maxSpeed;

            float scaledFadeTime = FADE_TIME * alphaScale / speedScale;

            //Debug.LogWarning(PoolIdentifier + " - (" + FADE_TIME + ", " + alphaScale + ", " +
            //                 speedScale + ", " + scaledFadeTime + ")");

            while (t < 1.0f)
            {
                t += Time.deltaTime / scaledFadeTime;

                currentAlpha = Mathf.SmoothStep(currentAlpha, fadeOut ? MIN_ALPHA : MAX_ALPHA, t);

                currentAlbedo.a = currentAlpha;
                material.SetColor("_Color", currentAlbedo);

                yield return null;
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