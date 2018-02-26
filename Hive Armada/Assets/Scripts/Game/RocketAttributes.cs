//=============================================================================
// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-440-01, CPSC-340-01 & CPSC-344-01
// Group Project
// 
// This class holds the attributes for the different rocket types used in the
// game. Currently there are 3 different rockets: rocket for the player weapon
// rocket pods, rocket for the ally power-up, and the rocket for the area bomb.
// 
//=============================================================================

using System;
using UnityEngine;
using Hive.Armada.Player.Weapons;
using SubjectNerd.Utilities;
using Valve.VR.InteractionSystem;

namespace Hive.Armada.Game
{
    /// <summary>
    /// Holds the attributes for the rocket pod rockets, ally rockets, and area bomb powerup.
    /// </summary>
    public class RocketAttributes : MonoBehaviour
    {
        /// <summary>
        /// The different rocket types.
        /// </summary>
        public enum RocketType
        {
            /// <summary>
            /// Player weapon rocket pod rockets.
            /// </summary>
            RocketPod,

            /// <summary>
            /// Ally power-up rockets.
            /// </summary>
            Ally,

            /// <summary>
            /// Area bomb power-up rocket.
            /// </summary>
            AreaBomb
        }

        /// <summary>
        /// Holds all the attributes for a rocket type.
        /// </summary>
        [Serializable]
        public struct RocketSetup
        {
            /// <summary>
            /// The type of rocket for this setup.
            /// </summary>
            public RocketType rocketType;

            /// <summary>
            /// The behavior flags.
            /// </summary>
            [EnumFlags]
            public Rocket.RocketFlags behaviorFlags;

            /// <summary>
            /// The damage dealt on impact.
            /// </summary>
            [Space]
            public int damage;

            /// <summary>
            /// The speed the rocket flies.
            /// </summary>
            public float speed;

            /// <summary>
            /// The strength of the homing.
            /// 0 - no homing
            /// 1 - perfect homing
            /// </summary>
            public float homingSensitivity;

            /// <summary>
            /// How long until the rocket self-destructs.
            /// </summary>
            public float explodeTime;

            /// <summary>
            /// The radius for the AoE explosive damage.
            /// </summary>
            [Header("AoE Explosive Damage")]
            public float explosiveRadius;

            /// <summary>
            /// How often to generate random movement.
            /// </summary>
            [Header("Random Movement")]
            public float randomTime;

            /// <summary>
            /// The range of random X-movement. [-randomX, randomX]
            /// </summary>
            public float randomX;

            /// <summary>
            /// The range of random Y-zmovement. [-randomY, randomY]
            /// </summary>
            public float randomY;

            /// <summary>
            /// The range of random Z-movement. [0, randomZ]
            /// </summary>
            public float randomZ;

            /// <summary>
            /// The color for the rocket trail.
            /// </summary>
            [Header("Trail")]
            public Color trailColor;

            /// <summary>
            /// The width of the rocket trail.
            /// </summary>
            public float trailWidth;

            /// <summary>
            /// The explosion emitter prefab.
            /// </summary>
            [Header("Emitters")]
            public GameObject explosionEmitterPrefab;
        }

        /// <summary>
        /// Reference manager that holds all needed references
        /// (e.g. spawner, game manager, etc.)
        /// </summary>
        public ReferenceManager reference;

        /// <summary>
        /// Array of rocket attribute setups.
        /// </summary>
        [Reorderable("Rocket", false)]
        public RocketSetup[] rockets;

        /// <summary>
        /// Array of Type Ids for the explosion emitters.
        /// </summary>
        public int[] RocketExplosionEmitterIds { get; private set; }

        /// <summary>
        /// Gets the type identifiers for the rocket emitters.
        /// </summary>
        public void Initialize()
        {
            RocketExplosionEmitterIds = new int[rockets.Length];

            for (int i = 0; i < rockets.Length; ++i)
            {
                if (rockets[i].explosionEmitterPrefab != null)
                {
                    RocketExplosionEmitterIds[i] =
                        reference.objectPoolManager.GetTypeIdentifier(
                            rockets[i].explosionEmitterPrefab);
                }
                else
                {
                    RocketExplosionEmitterIds[i] = -1;
                }
            }
        }
    }
}