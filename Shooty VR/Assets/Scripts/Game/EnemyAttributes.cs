//=============================================================================
// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
// 
// This class holds all attributes for the enemies. The enemies use this to
// reset their values to properly allow them to respawn with the object pool.
// This also allows us to set all attributes in one place instead of having to
// go through each and every relevant prefab to change attributes.
// 
//=============================================================================

using SubjectNerd.Utilities;
using UnityEngine;

namespace Hive.Armada.Game
{
    /// <summary>
    /// Holds the attributes for all enemy types and projectiles.
    /// </summary>
    public class EnemyAttributes : MonoBehaviour
    {
        /// <summary>
        /// Reference manager that holds all needed references
        /// (e.g. spawner, game manager, etc.)
        /// </summary>
        private ReferenceManager reference;

        /// <summary>
        /// Starting health for each enemy type.
        /// </summary>
        [Header("Enemy - General")]
        [Reorderable("Enemy", false)]
        public int[] enemyHealthValues;

        /// <summary>
        /// Self-destruct time for each enemy type.
        /// </summary>
        [Reorderable("Enemy", false)]
        public float[] enemySelfDestructTimes;

        /// <summary>
        /// How many points each enemy is worth.
        /// </summary>
        [Reorderable("Enemy", false)]
        public int[] enemyScoreValues;

        /// <summary>
        /// Prefab of each enemy's projectile.
        /// </summary>
        [Reorderable("Enemy", false)]
        public GameObject[] enemyProjectilePrefab;

        /// <summary>
        /// Type identifiers for each enemy's projectile prefab.
        /// </summary>
        public int[] EnemyProjectileTypeIdentifiers { get; private set; }

        /// <summary>
        /// Fire rate for each enemy type.
        /// </summary>
        [Header("Enemy - Combat")]
        [Reorderable("Enemy", false)]
        public float[] enemyFireRate;

        /// <summary>
        /// Shooting spread for each enemy type.
        /// </summary>
        [Reorderable("Enemy", false)]
        public float[] enemySpread;

        /// <summary>
        /// Particle emitter on enemy spawn.
        /// </summary>
        [Header("Enemy - Emitters")]
        [Reorderable("Enemy", false)]
        public GameObject[] enemySpawnEmitters;

        /// <summary>
        /// Particles emitters on enemy death.
        /// </summary>
        [Reorderable("Enemy", false)]
        public GameObject[] enemyDeathEmitters;

        /// <summary>
        /// Damage that the enemy projectiles deal.
        /// </summary>
        [Header("Projectile - General")]
        public int projectileDamage;

        /// <summary>
        /// Speed at which the projectile moves.
        /// </summary>
        public float projectileSpeed;

        /// <summary>
        /// How long projectiles should 
        /// </summary>
        public float projectileLifetime;

        /// <summary>
        /// Gets the type identifiers for each enemy's projectile prefabs
        /// </summary>
        public void Initialize()
        {
            reference = transform.parent.GetComponent<ReferenceManager>();

            EnemyProjectileTypeIdentifiers = new int[enemyProjectilePrefab.Length];

            for (int i = 0; i < enemyProjectilePrefab.Length; ++i)
            {
                for (int j = 0; j < reference.objectPoolManager.objects.Length; ++j)
                {
                    if (reference.objectPoolManager.objects[j].objectPrefab.name.Equals(enemyProjectilePrefab[i].name))
                    {
                        EnemyProjectileTypeIdentifiers[i] = j;
                        break;
                    }
                }
            }
        }
    }
}
