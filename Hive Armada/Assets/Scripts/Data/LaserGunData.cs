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
    [CreateAssetMenu(fileName = "New Laser Gun Data", menuName = "Weapon Attributes/Laser Gun", order = 50)]
    public class LaserGunData : WeaponData
    {
        [Header("Lasers")]
        public Material laserMaterial;

        public float thickness;

        [Header("Emitters")]
        public GameObject hitSpark;

        public GameObject muzzleFlash;
    }
}