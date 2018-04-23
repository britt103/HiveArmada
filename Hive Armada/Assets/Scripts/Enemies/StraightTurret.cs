//=============================================================================
//
// Miguel Gotao
// 2264941
// gotao100@mail.chapman.edu
// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// 
// CPSC-440-01, CPSC-340-01 & CPSC-344-01
// Group Project
//
// Standard enemy behavior for shooting projectiles
//
//=============================================================================

using Hive.Armada.Data;

namespace Hive.Armada.Enemies
{
    /// <summary>
    /// The Standard enemy.
    /// </summary>
    public class StraightTurret : ShootingEnemy
    {
        /// <summary>
        /// The ScriptableObject holding this enemy's attributes.
        /// </summary>
        public ShootingEnemyData enemyData;

        /// <summary>
        /// Initializes this enemy's attributes.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            Initialize(enemyData);
        }
    }
}