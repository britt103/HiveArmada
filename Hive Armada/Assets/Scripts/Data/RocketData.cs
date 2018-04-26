//=============================================================================
// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-440-01
// Group Project
// 
//=============================================================================

using Hive.Armada.Player.Weapons;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace Hive.Armada.Data
{
    [CreateAssetMenu(fileName = "New Rocket Data", menuName = "Weapon Attributes/Rocket Data")]
    public class RocketData : ScriptableObject
    {
        [Header("Rocket Attributes")]
        [EnumFlags]
        public Rocket.RocketFlags behaviorFlags;

        public float speed;

        public float homingSensitivity;

        public float explodeTime;

        [Header("AoE Explosive Damage")]
        public float explosiveRadius;

        [Header("Random Movement")]
        public float randomTime;

        public float randomX;

        public float randomY;

        public float randomZ;

        [Header("Emitters")]
        public GameObject explosionEmitterPrefab;

        public GameObject rocketEmitterPrefab;

        public GameObject trailEmitterPrefab;

        [Header("Audio")]
        public AudioClip explosionClip;

        public AudioClip trailClip;
    }
}