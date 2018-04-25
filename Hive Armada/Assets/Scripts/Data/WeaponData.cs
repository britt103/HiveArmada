//=============================================================================
// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-440-01
// Group Project
// 
//=============================================================================

using UnityEngine;

namespace Hive.Armada.Data
{
    public abstract class WeaponData : ScriptableObject
    {
        [Header("Weapon Attributes")]
        public float aimAssistRadius;
        
        public int damage;

        [Tooltip("# of shots per second.")]
        public int fireRate;

        [Space]
        public AudioClip shootSound;
    }
}