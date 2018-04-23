using UnityEngine;

namespace Hive.Armada.Data
{
    [CreateAssetMenu(fileName = "New Health Data", menuName = "Health Data")]
    public class HealthData : ScriptableObject
    {
        public int playerMaxHealth;

        public int playerLowHealth;
    }
}