//=============================================================================
// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
// 
// This class holds all attributes for the enemies. The enemies use this to
// reset their values to properly allow them to respawn with the object pool.
// This also allows us to set all attributes in one place instead of having to
// go through each and every relevant prefab to change attributes.
// 
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Hive.Armada.Enemies;
using Hive.Armada.PowerUps;
using SubjectNerd.Utilities;
using UnityEngine;

namespace Hive.Armada.Game
{
    /// <summary>
    /// Holds the attributes for all enemy types and projectiles.
    /// </summary>
    public class EnemyAttributes : MonoBehaviour
    {
        public struct Speed
        {
            public readonly float maxSpeed;

            public readonly float minSpeed;

            public Speed(float max, float min)
            {
                maxSpeed = max;
                minSpeed = min;
            }
        }

        /// <summary>
        /// Reference manager that holds all needed references
        /// (e.g. spawner, game manager, etc.)
        /// </summary>
        private ReferenceManager reference;

        /// <summary>
        /// Starting health for each enemy type.
        /// </summary>
        [Header("Enemy - General")]
        [Reorderable("Enemy", false)]
        public int[] enemyHealthValues;

        /// <summary>
        /// Self-destruct time for each enemy type.
        /// </summary>
        [Reorderable("Enemy", false)]
        public float[] enemySelfDestructTimes;

        /// <summary>
        /// How many points each enemy is worth.
        /// </summary>
        [Reorderable("Enemy", false)]
        public int[] enemyScoreValues;

        /// <summary>
        /// Prefab of each enemy's projectile.
        /// </summary>
        [Reorderable("Enemy", false)]
        public GameObject[] enemyProjectilePrefab;

        /// <summary>
        /// Type identifiers for each enemy's projectile prefab.
        /// </summary>
        public short[] EnemyProjectileTypeIdentifiers { get; private set; }

        /// <summary>
        /// Fire rate for each enemy type.
        /// </summary>
        [Header("Enemy - Combat")]
        [Reorderable("Enemy", false)]
        public float[] enemyFireRate;

        /// <summary>
        /// Shooting spread for each enemy type.
        /// </summary>
        [Reorderable("Enemy", false)]
        public float[] enemySpread;

        /// <summary>
        /// Particle emitter on enemy spawn.
        /// </summary>
        [Header("Enemy - Emitters")]
        [Reorderable("Enemy", false)]
        public GameObject[] enemySpawnEmitters;

        /// <summary>
        /// Particles emitters on enemy death.
        /// </summary>
        [Reorderable("Enemy", false)]
        public GameObject[] enemyDeathEmitters;

        /// <summary>
        /// Type Ids for the enemy death particle emitters.
        /// </summary>
        public short[] EnemyDeathEmitterTypeIds { get; private set; }

        /// <summary>
        /// Damage that the enemy projectiles deal.
        /// </summary>
        [Header("Projectile - General")]
        public int projectileDamage;

        /// <summary>
        /// Speed at which the projectile moves.
        /// </summary>
        public float projectileSpeed;

        [Reorderable("Projectile", false)]
        public float[] projectileSpeeds;

        /// <summary>
        /// How long projectiles should
        /// </summary>
        public float projectileLifetime;

        public GameObject timeWarpPrefab;

        private float warpTransitionLength;

        private float warpStrength;

        public bool IsTimeWarped { get; private set; }

        private const int steps = 30;

        private float stepTime;

        private List<Projectile> projectiles;

        private float[] stepSizes;

        public Speed[] projectileSpeedBounds;

        private Coroutine timeWarpCoroutine;

        /// <summary>
        /// Gets the type identifiers for each enemy's projectile prefabs
        /// </summary>
        public void Initialize(ReferenceManager referenceManager)
        {
            reference = referenceManager;

            EnemyProjectileTypeIdentifiers = new short[enemyProjectilePrefab.Length];

            for (int i = 0; i < enemyProjectilePrefab.Length; ++i)
            {
                for (int j = 0; j < reference.objectPoolManager.objects.Length; ++j)
                {
                    if (reference.objectPoolManager.objects[j].objectPrefab.name
                                 .Equals(enemyProjectilePrefab[i].name))
                    {
                        EnemyProjectileTypeIdentifiers[i] = (short) j;
                        break;
                    }
                }
            }

            EnemyDeathEmitterTypeIds = new short[enemyDeathEmitters.Length];

            for (int i = 0; i < enemyDeathEmitters.Length; ++i)
            {
                if (enemyDeathEmitters[i] != null)
                {
                    EnemyDeathEmitterTypeIds[i] =
                        reference.objectPoolManager.GetTypeIdentifier(enemyDeathEmitters[i]);
                }
                else
                {
                    EnemyDeathEmitterTypeIds[i] = -1;
                }
            }

            projectiles = new List<Projectile>();
            stepSizes = new float[projectileSpeeds.Length];
            projectileSpeedBounds = new Speed[projectileSpeeds.Length];

            GameObject timeWarp = Instantiate(timeWarpPrefab,
                                              reference.objectPoolManager.transform.position,
                                              Quaternion.identity);
            TimeWarp warpScript = timeWarp.GetComponent<TimeWarp>();
            warpTransitionLength = warpScript.transitionLength;
            warpStrength = warpScript.strength;
            Destroy(timeWarp);

            stepTime = warpTransitionLength / steps;

            for (int i = 0; i < stepSizes.Length; ++i)
            {
                stepSizes[i] = (1.0f - warpStrength) / steps;

                float min = projectileSpeeds[i] * (1.0f - warpStrength);
                projectileSpeedBounds[i] = new Speed(projectileSpeeds[i], min);
            }
        }

        /// <summary>
        /// Adds a projectile to the list of projectiles.
        /// </summary>
        /// <param name="projectile"> The projectile to add </param>
        public void AddProjectile(GameObject projectile)
        {
            Projectile projectileScript = projectile.GetComponent<Projectile>();

            if (projectileScript != null)
            {
                projectiles.Add(projectileScript);
            }
        }

        /// <summary>
        /// </summary>
        public void StartTimeWarp()
        {
            if (!IsTimeWarped)
            {
                IsTimeWarped = true;
                StartCoroutine(TimeWarp(true));
            }
        }

        /// <summary>
        /// </summary>
        public void StopTimeWarp()
        {
            if (IsTimeWarped)
            {
                StartCoroutine(TimeWarp(false));
            }
        }

        private IEnumerator TimeWarp(bool isIn)
        {
            if (isIn)
            {
                foreach (Projectile p in projectiles)
                {
                    if (!p.IsActive)
                    {
                        continue;
                    }

                    p.StartTimeWarp();
                }
            }

            float start = Time.time;
            float t = 0.0f;

            while (t < 1.0f)
            {
                t = (Time.time - start) / warpTransitionLength;

                if (isIn)
                {
                    for (int p = 0; p < projectileSpeeds.Length; ++p)
                    {
                        projectileSpeeds[p] = Mathf.SmoothStep(projectileSpeedBounds[p].maxSpeed,
                                                               projectileSpeedBounds[p].minSpeed,
                                                               t);
                    }
                }
                else
                {
                    for (int p = 0; p < projectileSpeeds.Length; ++p)
                    {
                        projectileSpeeds[p] = Mathf.SmoothStep(projectileSpeedBounds[p].minSpeed,
                                                               projectileSpeedBounds[p].maxSpeed,
                                                               t);
                    }
                }

                yield return new WaitForSeconds(stepTime);
            }

            if (!isIn)
            {
                IsTimeWarped = false;
            }
        }
    }
}