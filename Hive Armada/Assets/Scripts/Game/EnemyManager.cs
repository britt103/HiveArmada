//=============================================================================
// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
// 
// This class generates enemy ID's for spawned enemies and handles the time
// warp projectile speed functionality.
// 
//=============================================================================

using System.Collections;
using Hive.Armada.PowerUps;
using SubjectNerd.Utilities;
using UnityEngine;

namespace Hive.Armada.Game
{
    /// <summary>
    /// Holds the attributes for all enemy types and projectiles.
    /// </summary>
    public class EnemyManager : MonoBehaviour
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

        public delegate void TimeWarpToggle();

        public static event TimeWarpToggle OnTimeWarpToggle;
        
        public delegate void TimeWarpStep();

        public static event TimeWarpStep OnTimeWarp;

        /// <summary>
        /// Reference manager that holds all needed references
        /// (e.g. spawner, game manager, etc.)
        /// </summary>
        private ReferenceManager reference;

        /// <summary>
        /// Speed at which the projectile moves.
        /// </summary>
        [Reorderable("Projectile", false)]
        public float projectileSpeed;

        public GameObject timeWarpPrefab;

        private float warpTransitionLength;

        private float warpStrength;

        public bool IsTimeWarped { get; private set; }

        private const int STEPS = 30;

        private float stepTime;

        public Speed projectileSpeedBound;

        private Coroutine timeWarpCoroutine;

        private int enemyId;

        private WaitForSeconds waitStep;

        /// <summary>
        /// Gets the type identifiers for each enemy's projectile prefabs
        /// </summary>
        public void Initialize(ReferenceManager referenceManager)
        {
            reference = referenceManager;

            GameObject timeWarp = Instantiate(timeWarpPrefab,
                                              reference.objectPoolManager.transform.position,
                                              Quaternion.identity);
            TimeWarp warpScript = timeWarp.GetComponent<TimeWarp>();
            warpTransitionLength = warpScript.transitionLength;
            warpStrength = warpScript.strength;
            Destroy(timeWarp);

            stepTime = warpTransitionLength / STEPS;

            float min = projectileSpeed * (1.0f - warpStrength);
            projectileSpeedBound = new Speed(projectileSpeed, min);

            enemyId = 0;

            waitStep = new WaitForSeconds(stepTime);
        }

        /// <summary>
        /// </summary>
        public void StartTimeWarp()
        {
            if (!IsTimeWarped)
            {
                IsTimeWarped = true;

                if (OnTimeWarpToggle != null)
                    OnTimeWarpToggle();
                
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
            float start = Time.time;
            float t = 0.0f;

            while (t < 1.0f)
            {
                t = (Time.time - start) / warpTransitionLength;

                if (isIn)
                {
                    projectileSpeed = Mathf.SmoothStep(projectileSpeedBound.maxSpeed,
                                                       projectileSpeedBound.minSpeed,
                                                       t);
                }
                else
                {
                    projectileSpeed = Mathf.SmoothStep(projectileSpeedBound.minSpeed,
                                                       projectileSpeedBound.maxSpeed,
                                                       t);
                }

                if (OnTimeWarp != null)
                    OnTimeWarp();

                yield return waitStep;
            }

            if (isIn)
                yield break;

            IsTimeWarped = false;
                
            if (OnTimeWarpToggle != null)
                OnTimeWarpToggle();
        }

        public int GetNextEnemyId()
        {
            if (enemyId == int.MaxValue)
            {
                enemyId = 0;
            }

            return enemyId++;
        }
    }
}