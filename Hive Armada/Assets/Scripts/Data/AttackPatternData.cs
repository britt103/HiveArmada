using UnityEngine;

namespace Hive.Armada.Data
{
    [CreateAssetMenu(fileName = "New Attack Attributes",
        menuName = "Enemy Attributes/Attack Pattern", order = 1)]
    public class AttackPatternData : ScriptableObject
    {
        [Tooltip("The prefab for the projectile or the pattern.")]
        public GameObject prefab;

        [Tooltip("If the prefab is a projectile pattern. False is a single projectile.")]
        public bool isProjectilePattern;

        public float fireRate;

        public float projectileSpeed;

        public float spread;

        public bool canRotate;
    }
}