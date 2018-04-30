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
    [CreateAssetMenu(fileName = "New Plasma Data", menuName = "Weapon Attributes/Plasma", order = 53)]
    public class PlasmaData : WeaponData
    {
        [Header("Plasma")]
        public GameObject plasmaPrefab;

        public int maxAmmo;

        public float reloadTime;

        public float reloadDelay;

        public float reloadPartialDelay;

        [Header("Plasma Audio")]
        public AudioClip chargingSound;

        public AudioClip chargeCompleteSound;
    }
}