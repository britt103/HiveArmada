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

using UnityEngine;
using Hive.Armada.Game;
using Hive.Armada.Player;

namespace Hive.Armada.Enemies
{
    /// <summary>
    /// Basic projectile used by all shooting enemies.
    /// </summary>
    public class ProjectileInPattern : MonoBehaviour
    {
        private ReferenceManager reference;

        private EnemyManager enemyManager;

        private ProjectileProximity projectileProximity;

        private ProjectilePattern pattern;

        private int projectileDamage;

        public bool HasHit { get; private set; }

        /// <summary>
        /// The renderer for the projectile.
        /// </summary>
        private Material material;

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

        private float t;

        private float alphaScale;

        private bool isFading;

        private void Awake()
        {
            reference = FindObjectOfType<ReferenceManager>();
            enemyManager = reference.enemyAttributes;
            projectileProximity = FindObjectOfType<ProjectileProximity>();
            pattern = gameObject.GetComponentInParent<ProjectilePattern>();
            projectileDamage = reference.projectileData.projectileDamage;
            material = GetComponent<Renderer>().material;
            originalAlbedo = material.GetColor("_Color");
            originalEmission = material.GetColor("_EmissionColor");
            currentAlpha = 1.0f;
        }

        public void Reset()
        {
            HasHit = false;
        }

        public void Hit()
        {
            HasHit = true;
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            projectileProximity.RemoveProjectile(gameObject.GetInstanceID());
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
            ProjectileProximity.OnFadeInStep += FadeInStep;
        }

        private void FadeOutStep()
        {
            float speedScale = enemyManager.projectileSpeed / enemyManager.projectileSpeedBound.maxSpeed;
            float scaledFadeTime = FADE_TIME * alphaScale / speedScale;

            t += Time.deltaTime / scaledFadeTime;

            currentAlpha = Mathf.SmoothStep(currentAlpha, MAX_ALPHA, t);

            currentAlbedo.a = currentAlpha;
            material.SetColor("_Color", currentAlbedo);
        }

        private void FadeInStep()
        {
            float speedScale = enemyManager.projectileSpeed / enemyManager.projectileSpeedBound.maxSpeed;
            float scaledFadeTime = FADE_TIME * alphaScale / speedScale;

            t += Time.deltaTime / scaledFadeTime;

            currentAlpha = Mathf.SmoothStep(currentAlpha, MIN_ALPHA, t);

            currentAlbedo.a = currentAlpha;
            material.SetColor("_Color", currentAlbedo);

            if (currentAlpha >= MAX_ALPHA)
            {
                isFading = false;
                ProjectileProximity.OnFadeInStep -= FadeInStep;
            }
        }

        /// <summary>
        /// Runs when the projectile collides with another object with a Collider.
        /// </summary>
        /// <param name="other"> The other collider </param>
        private void OnTriggerEnter(Collider other)
        {
            if (HasHit)
            {
                return;
            }

            if (other.CompareTag("Player"))
            {
                HasHit = true;

                if (Utility.isDebug)
                {
                    Debug.Log(GetType().Name + " - hit object named \"" + other.gameObject.name +
                              "\"");
                }

                if (reference.playerShip != null)
                {
                    reference.playerShip.GetComponent<PlayerHealth>().Hit(projectileDamage);
                }

                pattern.ProjectileHit();
                gameObject.SetActive(false);
            }
            else if (other.CompareTag("Room") || other.CompareTag("ProjectileBounds"))
            {
                pattern.ProjectileHit();
                gameObject.SetActive(false);
            }
        }
    }
}