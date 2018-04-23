using UnityEngine;

namespace Hive.Armada.Data
{
    [CreateAssetMenu(fileName = "New Splitter Child Enemy Data",
        menuName = "Enemy Attributes/Splitter Child Enemy Data", order = 52)]
    public class SplitterChildEnemyData : ShootingEnemyData
    {
        [Header("Splitter Child Attributes")]
        public GameObject spawnEmitter;
        
        public int pattern1Burst;

        public int pattern2Burst;
        
        public int pattern3Burst;

        public int pattern4Burst;
    }
}