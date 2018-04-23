using UnityEngine;

namespace Hive.Armada.Data
{
    [CreateAssetMenu(fileName = "New Projectile Data", menuName = "Projectile Data")]
    public class ProjectileData : ScriptableObject
    {
        public int projectileDamage;
    }
}