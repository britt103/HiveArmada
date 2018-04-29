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
using MirzaBeig.ParticleSystems;

namespace Hive.Armada.Enemies
{
    /// <summary>
    /// Basic projectile used by all shooting enemies.
    /// </summary>
    public class Projectile : Poolable
    {
        private EnemyManager enemyManager;

        private ProjectileProximity projectileProximity;

        private Rigidbody pRigidbody;

        public ParticleSystems trailEmitter;

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
        private const float MIN_ALPHA = 0.15f;

        /// <summary>
        /// Length of the fade in seconds.
        /// </summary>
        private const float FADE_TIME = 0.5f;

        /// <summary>
        /// Number of steps the fade should be broken into.
        /// </summary>
        private const int FADE_STEPS = 60;

        private float t;

        private float alphaScale;

        private bool isFading;

        /// <summary>
        /// Initializes the reference to the Reference Manager
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            enemyManager = reference.enemyAttributes;
            projectileProximity = FindObjectOfType<ProjectileProximity>();
            damage = reference.projectileData.projectileDamage;
            pRigidbody = GetComponent<Rigidbody>();
            material = GetComponent<MeshRenderer>().material;
            originalAlbedo = material.GetColor("_Color");
            originalEmission = material.GetColor("_EmissionColor");
            currentAlbedo = originalAlbedo;
            currentAlpha = 1.0f;
        }

        private void OnEnable()
        {
            if (currentAlpha < MAX_ALPHA)
            {
                currentAlpha = MAX_ALPHA;
                currentAlbedo.a = currentAlpha;
                material.SetColor("_Color", currentAlbedo);
            }

            EnemyManager.OnTimeWarp += TimeWarpStep;
        }

        private void OnDisable()
        {
            projectileProximity.RemoveProjectile(gameObject.GetInstanceID());
            EnemyManager.OnTimeWarp -= TimeWarpStep;
            //try
            //{

            //}
            //catch (Exception)
            //{
            //    // do nothing
            //}
            ProjectileProximity.OnFadeOutStep -= FadeOutStep;
            ProjectileProximity.OnFadeInStep -= FadeInStep;
        }

        /// <summary>
        /// Sets this projectile's speed ID number.
        /// </summary>
        public void Launch()
        {
            pRigidbody.velocity = transform.forward * enemyManager.projectileSpeed;
        }

        private void TimeWarpStep()
        {
            pRigidbody.velocity = transform.forward * enemyManager.projectileSpeed;
        }

        public void FadeOut()
        {
            isFading = true;
            t = 0.0f;
            alphaScale = (currentAlpha - MIN_ALPHA) / (MAX_ALPHA - MIN_ALPHA);
            ProjectileProximity.OnFadeOutStep += FadeOutStep;
        }

        public void FadeIn()
        {
            t = 0.0f;
            alphaScale = (MAX_ALPHA - currentAlpha) / (MAX_ALPHA - MIN_ALPHA);
            ProjectileProximity.OnFadeOutStep -= FadeOutStep;
            //ProjectileProximity.OnFadeInStep += FadeInStep;
        }

        private void FadeOutStep()
        {
            if (t >= 1.0f)
            {
                isFading = false;
                ProjectileProximity.OnFadeOutStep -= FadeOutStep;
                return;
            }

            float speedScale = enemyManager.projectileSpeed / enemyManager.projectileSpeedBound.maxSpeed;
            float scaledFadeTime = FADE_TIME * alphaScale / speedScale;

            t += 1.0f / 30.0f / scaledFadeTime;

            currentAlpha = Mathf.SmoothStep(currentAlpha, MIN_ALPHA, t);

            currentAlbedo.a = currentAlpha;
            material.SetColor("_Color", currentAlbedo);
        }

        private void FadeInStep()
        {
            float speedScale = enemyManager.projectileSpeed / enemyManager.projectileSpeedBound.maxSpeed;
            float scaledFadeTime = FADE_TIME * alphaScale / speedScale;

            t += 1.0f / 30.0f / scaledFadeTime;

            currentAlpha = Mathf.SmoothStep(currentAlpha, MAX_ALPHA, t);

            currentAlbedo.a = currentAlpha;
            material.SetColor("_Color", currentAlbedo);

            if (t >= 1.0f)
            {
                isFading = false;
                ProjectileProximity.OnFadeInStep -= FadeInStep;
                projectileProximity.RemoveProjectile(gameObject.GetInstanceID());
                return;
            }

            if (currentAlpha >= MAX_ALPHA)
            {
                isFading = false;
                ProjectileProximity.OnFadeInStep -= FadeInStep;
                projectileProximity.RemoveProjectile(gameObject.GetInstanceID());
            }
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
        
        protected override void Reset()
        {
            StopAllCoroutines();
        }
    }
}