using UnityEngine;

namespace Hive.Armada.Data
{
    public abstract class EnemyData : ScriptableObject
    {
        [Header("Base Attributes")]
        public int maxHealth;
        
        public int scoreValue;

        public Material flashColor;

        public GameObject deathEmitter;

        public float selfDestructTime;
    }
}