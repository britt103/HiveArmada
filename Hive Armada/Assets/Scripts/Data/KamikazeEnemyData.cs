using UnityEngine;

namespace Hive.Armada.Data
{
    [CreateAssetMenu(fileName = "New Kamikaze Enemy Data",
        menuName = "Enemy Attributes/Kamikaze Enemy Data", order = 53)]
    public class KamikazeEnemyData : EnemyData
    {
        [Header("Kamikaze Attributes")]
        public float moveSpeed;

        public float rotationSpeed;

        public float range;

        public AudioClip nearPlayerSound;
    }
}