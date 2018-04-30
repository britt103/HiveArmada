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
    [CreateAssetMenu(fileName = "New Minigun Data", menuName = "Weapon Attributes/Minigun", order = 51)]
    public class MinigunData : WeaponData
    {
        [Header("Tracers")]
        public Material tracerMaterial;

        public float thickness;

        [Header("Overheat")]
        public Color overheatColor;

        public float overheatMax;

        public float overheatPerShot;

        public float overheatDecreaseAmount;

        public float overheatDecreaseTickLength;

        public float overheatDecreaseDelay;

        public float overheatCoolDown;

        [Header("Emitters")]
        public GameObject hitSpark;

        public GameObject muzzleFlash;

        [Header("Minigun Audio")]
        public AudioClip overheatSound;
    }
}