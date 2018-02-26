//=============================================================================
// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-440-01, CPSC-340-01 & CPSC-344-01
// Group Project
// 
// 
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
        /// </summary>
        public enum RocketType
        {
            /// <summary>
            /// </summary>
            RocketPod,

            /// <summary>
            /// </summary>
            Ally,

            /// <summary>
            /// </summary>
            AreaBomb
        }

        /// <summary>
        /// </summary>
        [Serializable]
        public struct RocketSetup
        {
            /// <summary>
            /// </summary>
            public RocketType rocketType;

            /// <summary>
            /// </summary>
            [EnumFlags]
            public Rocket.RocketFlags behaviorFlags;

            /// <summary>
            /// </summary>
            [Space]
            public int damage;

            /// <summary>
            /// </summary>
            public float speed;

            /// <summary>
            /// </summary>
            public float homingSensitivity;

            /// <summary>
            /// </summary>
            public float explodeTime;

            /// <summary>
            /// </summary>
            [Header("AoE Explosive Damage")]
            public float explosiveRadius;

            /// <summary>
            /// </summary>
            public float explosiveDamage;

            /// <summary>
            /// </summary>
            [Header("Random Movement")]
            public float randomTime;

            /// <summary>
            /// </summary>
            public float randomX;

            /// <summary>
            /// </summary>
            public float randomY;

            /// <summary>
            /// </summary>
            public float randomZ;

            /// <summary>
            /// </summary>
            [Header("Trail")]
            public Color trailColor;

            /// <summary>
            /// </summary>
            public float trailWidth;

            /// <summary>
            /// </summary>
            [Header("Emitters")]
            public GameObject explosionEmitterPrefab;

            public int ExplosionEmitterId { get; private set; }
        }

        /// <summary>
        /// Reference manager that holds all needed references
        /// (e.g. spawner, game manager, etc.)
        /// </summary>
        public ReferenceManager reference;

        /// <summary>
        /// </summary>
        [Reorderable("Rocket", false)]
        public RocketSetup[] rockets;

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