//=============================================================================
//
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-440-01, CPSC-340-01 & CPSC-344-01
// Group Project
//
// Basic enemy that is spawned when the Splitter dies. Simply looks at the
// player and shoots.
//
//=============================================================================

using Hive.Armada.Data;
using MirzaBeig.ParticleSystems;
using UnityEngine;

namespace Hive.Armada.Enemies
{
    /// <summary>
    /// Splitter child enemy
    /// </summary>
    public class SplitterChild : ShootingEnemy
    {
        /// <summary>
        /// The ScriptableObject holding this enemy's attributes.
        /// </summary>
        public SplitterChildEnemyData enemyData;

        private GameObject spawnEmitter;

        private ParticleSystems spawnEmitterSystem;

        /// <summary>
        /// The amount of burst shots in each attack pattern.
        /// </summary>
        private int[] patternBurstAmounts;

        /// <summary>
        /// The amount of burst shots every time this enemy fires.
        /// </summary>
        private int currentBurstAmount;

        /// <summary>
        /// How many projectiles have been shot in the current burst.
        /// </summary>
        private int burstCount;

        /// <summary>
        /// The amount of time between shots in a burst.
        /// </summary>
        private const float BURST_FIRE_RATE = 0.1f;

        /// <summary>
        /// The amount of time between each round of burst shots.
        /// </summary>
        private float burstCooldown;

        /// <summary>
        /// Initializes this enemy's attributes.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            Initialize(enemyData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="splitterChildEnemyData"></param>
        private void Initialize(SplitterChildEnemyData splitterChildEnemyData)
        {
            base.Initialize(enemyData);

            spawnEmitter = Instantiate(splitterChildEnemyData.spawnEmitter, transform.position,
                                       transform.rotation, transform);
            spawnEmitterSystem = spawnEmitter.GetComponent<ParticleSystems>();
            spawnEmitterSystem.stop();
            spawnEmitterSystem.clear();

            patternBurstAmounts = new[]
                                  {
                                      splitterChildEnemyData.pattern1Burst,
                                      splitterChildEnemyData.pattern2Burst,
                                      splitterChildEnemyData.pattern3Burst,
                                      splitterChildEnemyData.pattern4Burst
                                  };
        }

        protected override void OnEnable()
        {
            PathingComplete = true;
            base.OnEnable();
        }

        /// <summary>
        /// Checks if the fireRate time has passed. Switches between bursting and not.
        /// </summary>
        protected override void CheckShoot()
        {
            if (Time.time >= nextShootTime)
            {
                canShoot = true;
                ++burstCount;

                if (burstCount == currentBurstAmount - 1)
                {
                    fireRate = burstCooldown;
                }
                else if (burstCount == currentBurstAmount)
                {
                    fireRate = BURST_FIRE_RATE;
                    burstCount = 0;
                }
            }
        }

        /// <summary>
        /// Sets the attack pattern values specific to this enemy.
        /// </summary>
        /// <param name="newAttackPattern"> The new attack pattern to use </param>
        public override void SetAttackPattern(AttackPattern newAttackPattern)
        {
            base.SetAttackPattern(newAttackPattern);

            currentBurstAmount = patternBurstAmounts[(int) newAttackPattern];
            burstCooldown = attackPatternData[(int) newAttackPattern].fireRate;
            fireRate = BURST_FIRE_RATE;
        }
    }
}