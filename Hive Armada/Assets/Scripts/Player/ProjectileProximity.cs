//=============================================================================
// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-440-01
// Group Project
// 
// This class tells projectiles to fade their opacity when near the player.
// When a projectile enters the sphere collider on the player's head, it is
// told to fade out its opacity. Then when it exits the collider it is told to
// fade in its opacity. The goal was to increase player comfort because a large
// number of projectiles in their face will completely block their vision.
// 
//=============================================================================

using UnityEngine;
using Hive.Armada.Enemies;
using System.Collections.Generic;

namespace Hive.Armada.Player
{
    /// <summary>
    /// Projectile proximity checker for the player's head.
    /// </summary>
    public class ProjectileProximity : MonoBehaviour
    {
        public delegate void FadeOutStep();
        public static event FadeOutStep OnFadeOutStep;

        public delegate void FadeInStep();
        public static event FadeInStep OnFadeInStep;

        private Dictionary<int, bool> projectiles;

        private int projectileInCount;

        private int projectileOutCount;

        private bool fading;

        private float nextFadeTime;

        private float fadeTime;

        private void Awake()
        {
            projectiles = new Dictionary<int, bool>();

            fadeTime = 1.0f / 30.0f;
            nextFadeTime = Time.time + fadeTime;
        }

        private void Update()
        {
            if (fading && Time.time >= nextFadeTime)
            {
                nextFadeTime = Time.time + fadeTime;

                if (OnFadeOutStep != null && projectileInCount > 0)
                {
                    OnFadeOutStep();
                }

                if (OnFadeInStep != null && projectileOutCount > 0)
                {
                    OnFadeInStep();
                }
            }
        }

        public void RemoveProjectile(int instanceId)
        {
            if (projectiles.ContainsKey(instanceId))
            {
                if (projectiles[instanceId])
                    --projectileInCount;
                else
                    --projectileOutCount;

                if (projectileInCount < 0)
                    projectileInCount = 0;
                else if (projectileOutCount < 0)
                    projectileOutCount = 0;

                projectiles.Remove(instanceId);
            }

            if (projectiles.Count <= 0)
                fading = false;
        }

        /// <summary>
        /// Fades out the transparency of projectiles that get too close to the camera.
        /// </summary>
        /// <param name="other"> The entering object's collider </param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Projectile"))
            {
                Projectile projectileScript = other.GetComponent<Projectile>();
                ProjectileInPattern projectileInPatternScript = other.GetComponent<ProjectileInPattern>();

                if (projectiles.ContainsKey(other.gameObject.GetInstanceID()))
                {
                    --projectileOutCount;
                    projectiles.Remove(other.gameObject.GetInstanceID());
                }

                projectiles.Add(other.gameObject.GetInstanceID(), true);
                ++projectileInCount;

                if (projectileScript != null)
                    projectileScript.FadeOut();
                else if (projectileInPatternScript != null)
                    projectileInPatternScript.FadeOut();

                fading = true;
            }
        }

        /// <summary>
        /// Fades in the transparency of projectiles that get too close to the camera.
        /// </summary>
        /// <param name="other"> The exiting object's collider </param>
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Projectile"))
            {
                Projectile projectileScript = other.GetComponent<Projectile>();
                ProjectileInPattern projectileInPatternScript = other.GetComponent<ProjectileInPattern>();

                if (projectiles.ContainsKey(other.gameObject.GetInstanceID()))
                    projectiles[other.gameObject.GetInstanceID()] = false;

                --projectileInCount;
                ++projectileOutCount;

                if (projectileScript != null)
                    projectileScript.FadeIn();
                else if (projectileInPatternScript != null)
                    projectileInPatternScript.FadeIn();

                fading = true;
            }
        }
    }
}