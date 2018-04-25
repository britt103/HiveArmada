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
    [CreateAssetMenu(fileName = "New Rocket Pod Data", menuName = "Weapon Attributes/Rocket Pod", order = 52)]
    public class RocketPodData : WeaponData
    {
        [Header("Rockets")]
        public GameObject rocketPrefab;

        public int burstAmount;

        // [Header("Emitters")]
        // public GameObject rocketLaunchEmitter;
    }
}