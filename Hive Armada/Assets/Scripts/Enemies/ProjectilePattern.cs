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
    public class ProjectilePattern : Poolable
    {
        private EnemyManager enemyManager;

        private Rigidbody pRigidbody;
        
        public ParticleSystems trailEmitter;

        private int hitProjectiles;

        public GameObject[] projectiles;

        private ProjectileInPattern[] projectileScripts;

        /// <summary>
        /// Initializes the reference to the Reference Manager
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            enemyManager = reference.enemyAttributes;

            pRigidbody = GetComponent<Rigidbody>();
            projectileScripts = new ProjectileInPattern[projectiles.Length];

            for (int i = 0; i < projectiles.Length; ++i)
                projectileScripts[i] = projectiles[i].GetComponent<ProjectileInPattern>();
            
            if (trailEmitter != null)
            {
                trailEmitter.stop();
                trailEmitter.clear();
            }
            else
            {
                Debug.LogError(gameObject.name + " - Does not have time warp distortion emitter.");
            }
        }

        private void OnEnable()
        {
            if (enemyManager.IsTimeWarped)
                TimeWarpToggle();
            
            EnemyManager.OnTimeWarpToggle += TimeWarpToggle;
            EnemyManager.OnTimeWarp += TimeWarpStep;
        }

        private void OnDisable()
        {
            EnemyManager.OnTimeWarpToggle -= TimeWarpToggle;
            EnemyManager.OnTimeWarp -= TimeWarpStep;
        }

        /// <summary>
        /// Sets this projectile's speed ID number.
        /// </summary>
        /// <param name="id"> The ID to use </param>
        public void Launch()
        {
            pRigidbody.velocity = transform.forward * enemyManager.projectileSpeed;
        }
        
        private void TimeWarpToggle()
        {
            if (trailEmitter == null)
                return;
            
            if (enemyManager.IsTimeWarped)
            {
                trailEmitter.play();
            }
            else
            {
                trailEmitter.stop();
                trailEmitter.clear();
            }
        }

        private void TimeWarpStep()
        {
            pRigidbody.velocity = transform.forward * enemyManager.projectileSpeed;
        }

        public void ProjectileHit()
        {
            hitProjectiles++;

            if (++hitProjectiles >= projectiles.Length)
            {
                reference.objectPoolManager.Despawn(gameObject);
            }
        }

        /// <summary>
        /// Initializes the damage for the projectile.
        /// </summary>
        protected override void Reset()
        {
            hitProjectiles = 0;

            for (int i = 0; i < projectiles.Length; ++i)
            {
                projectiles[i].SetActive(true);
                projectileScripts[i].Reset();
            }
        }
    }
}