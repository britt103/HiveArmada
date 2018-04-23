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
// Enemy that shoots at the player and that spawns 4 more enemies
// when destroyed.
//
//=============================================================================

using Hive.Armada.Data;
using UnityEngine;

namespace Hive.Armada.Enemies
{
    /// <summary>
    /// The Splitter enemy.
    /// </summary>
    public class SplitterTurret : ShootingEnemy
    {
        /// <summary>
        /// The ScriptableObject holding this enemy's attributes.
        /// </summary>
        public SplitterEnemyData enemyData;

        /// <summary>
        /// The type ID for the children enemies.
        /// </summary>
        private short childTypeId;

        /// <summary>
        /// How much force the children split away from the Splitter.
        /// </summary>
        private float childSplitForce;

        /// <summary>
        /// Initializes this enemy's attributes.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            Initialize(enemyData);
        }

        /// <summary>
        /// Initializes the attributes for this splitter enemy.
        /// </summary>
        /// <param name="splitterEnemyData">  </param>
        private void Initialize(SplitterEnemyData splitterEnemyData)
        {
            base.Initialize(splitterEnemyData);

            childTypeId =
                reference.objectPoolManager.GetTypeIdentifier(splitterEnemyData.childPrefab);
            childSplitForce = splitterEnemyData.childSplitForce;
        }

        /// <summary>
        /// Spawns 4 children enemies and then dies.
        /// </summary>
        protected override void Kill()
        {
            if (childTypeId > 0)
            {
                for (int i = 1; i <= 4; ++i)
                {
                    float xAdj = i % 2 == 0 ? -1.0f : 1.0f;
                    float yAdj = i == 1 || i == 4 ? 1.0f : -1.0f;
                    Vector3 spawn = transform.position +
                                    transform.rotation * (new Vector3(xAdj, yAdj, 0.0f) / 10.0f);

                    GameObject child =
                        objectPoolManager.Spawn(gameObject, childTypeId, spawn,
                                                transform.rotation);
                    Enemy enemy = child.GetComponent<Enemy>();
                    enemy.SetWave(wave);
                    enemy.SetPath("child");
                    enemy.SetAttackPattern(attackPattern);
                    child.GetComponent<Rigidbody>()
                         .AddRelativeForce(new Vector3(xAdj, yAdj, 0.0f) * childSplitForce * 4.0f);
                }
            }

            base.Kill();
        }
    }
}