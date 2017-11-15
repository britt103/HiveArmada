//=============================================================================
// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
// 
// [DESCRIPTION]
// 
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using Hive.Armada.Menu;
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
        /// Starting health for each enemy type.
        /// </summary>
        [Header("Enemy - General")]
        [Reorderable("Enemy", false)]
        public int[] enemyHealthValues;

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
        public float projectileDamage;

        /// <summary>
        /// Speed at which the projectile moves.
        /// </summary>
        public float projectileSpeed;

        /// <summary>
        /// How long projectiles should 
        /// </summary>
        public float projectileLifetime;
    }
}