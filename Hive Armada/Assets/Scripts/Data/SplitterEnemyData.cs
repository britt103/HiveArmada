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
    [CreateAssetMenu(fileName = "New Splitter Enemy Data",
        menuName = "Enemy Attributes/Splitter Enemy Data", order = 51)]
    public class SplitterEnemyData : ShootingEnemyData
    {
        [Header("Splitter Attributes")]
        [Tooltip("The prefab of the Splitter Children.")]
        public GameObject childPrefab;

        [Tooltip("The amount of force at which the Splitter Children shoot out of the Splitter.")]
        public float childSplitForce;
    }
}