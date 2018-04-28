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

        private float stepSizes;

        public Speed projectileSpeedBound;

        private Coroutine timeWarpCoroutine;

        private int enemyId;

        private WaitForSeconds waitStep;

        private Vector2[] movingEnemyLookupTable;

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

            stepTime = warpTransitionLength / steps;
            stepSizes = (1.0f - warpStrength) / steps;

            float min = projectileSpeed * (1.0f - warpStrength);
            projectileSpeedBound = new Speed(projectileSpeed, min);

            enemyId = 0;

            waitStep = new WaitForSeconds(stepTime);

            // y = "randomly-generated" percent
            // x = initial hoverPercent
            movingEnemyLookupTable = new Vector2[]
            {
                new Vector2(0.421f, 0.382486f),
                new Vector2(0.423f, 0.385413f),
                new Vector2(0.425f, 0.388344f),
                new Vector2(0.427f, 0.391278f),
                new Vector2(0.429f, 0.394216f),
                new Vector2(0.431f, 0.397157f),
                new Vector2(0.433f, 0.400102f),
                new Vector2(0.435f, 0.403049f),
                new Vector2(0.437f, 0.406000f),
                new Vector2(0.439f, 0.408954f),
                new Vector2(0.441f, 0.411911f),
                new Vector2(0.443f, 0.414870f),
                new Vector2(0.445f, 0.417833f),
                new Vector2(0.447f, 0.420798f),
                new Vector2(0.449f, 0.423765f),
                new Vector2(0.451f, 0.426735f),
                new Vector2(0.453f, 0.429708f),
                new Vector2(0.455f, 0.432682f),
                new Vector2(0.457f, 0.435659f),
                new Vector2(0.459f, 0.438638f),
                new Vector2(0.461f, 0.441619f),
                new Vector2(0.463f, 0.444601f),
                new Vector2(0.465f, 0.447586f),
                new Vector2(0.467f, 0.450572f),
                new Vector2(0.469f, 0.453560f),
                new Vector2(0.471f, 0.456549f),
                new Vector2(0.473f, 0.459539f),
                new Vector2(0.475f, 0.462531f),
                new Vector2(0.477f, 0.465524f),
                new Vector2(0.479f, 0.468519f),
                new Vector2(0.481f, 0.471514f),
                new Vector2(0.483f, 0.474510f),
                new Vector2(0.485f, 0.477507f),
                new Vector2(0.487f, 0.480504f),
                new Vector2(0.489f, 0.483503f),
                new Vector2(0.491f, 0.486501f),
                new Vector2(0.493f, 0.489501f),
                new Vector2(0.495f, 0.492500f),
                new Vector2(0.497f, 0.495500f),
                new Vector2(0.499f, 0.498500f),
                new Vector2(0.501f, 0.501500f),
                new Vector2(0.503f, 0.504500f),
                new Vector2(0.505f, 0.507500f),
                new Vector2(0.507f, 0.510499f),
                new Vector2(0.509f, 0.513499f),
                new Vector2(0.511f, 0.516497f),
                new Vector2(0.513f, 0.519496f),
                new Vector2(0.515f, 0.522493f),
                new Vector2(0.517f, 0.525490f),
                new Vector2(0.519f, 0.528486f),
                new Vector2(0.521f, 0.531481f),
                new Vector2(0.523f, 0.534476f),
                new Vector2(0.525f, 0.537469f),
                new Vector2(0.527f, 0.540461f),
                new Vector2(0.529f, 0.543451f),
                new Vector2(0.531f, 0.546440f),
                new Vector2(0.533f, 0.549428f),
                new Vector2(0.535f, 0.552414f),
                new Vector2(0.537f, 0.555399f),
                new Vector2(0.539f, 0.558381f),
                new Vector2(0.541f, 0.561362f),
                new Vector2(0.543f, 0.564341f),
                new Vector2(0.545f, 0.567318f),
                new Vector2(0.547f, 0.570292f),
                new Vector2(0.549f, 0.573265f),
                new Vector2(0.551f, 0.576235f),
                new Vector2(0.553f, 0.579202f),
                new Vector2(0.555f, 0.582167f),
                new Vector2(0.557f, 0.585130f),
                new Vector2(0.559f, 0.588089f),
                new Vector2(0.561f, 0.591046f),
                new Vector2(0.563f, 0.594000f),
                new Vector2(0.565f, 0.596951f),
                new Vector2(0.567f, 0.599898f),
                new Vector2(0.569f, 0.602843f),
                new Vector2(0.571f, 0.605784f),
                new Vector2(0.573f, 0.608722f),
                new Vector2(0.575f, 0.611656f),
                new Vector2(0.577f, 0.614587f),
                new Vector2(0.579f, 0.617514f)
            };
        }

        public int GetMovingEnemyLookupIndex()
        {
            return Random.Range(0, movingEnemyLookupTable.Length);
        }

        public Vector2 MovingEnemyLookup(int index)
        {
            return movingEnemyLookupTable[index];
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

                OnTimeWarp();

                yield return waitStep;
            }

            if (!isIn)
            {
                IsTimeWarped = false;
            }
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