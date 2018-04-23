//=============================================================================
//
// Ryan Britton
// 1849351
// britt103@mail.chapman.edu
// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// 
// CPSC-440-01, CPSC-340-01 & CPSC-344-01
// Group Project
//
// Enemy that shoots at the player while moving between two points
//
//=============================================================================

using Hive.Armada.Data;

namespace Hive.Armada.Enemies
{
    /// <summary>
    /// The Moving enemy.
    /// </summary>
    public class MovingTurret : ShootingEnemy
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