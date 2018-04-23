using UnityEngine;

namespace Hive.Armada.Data
{
    [CreateAssetMenu(fileName = "New Shooting Enemy Data",
        menuName = "Enemy Attributes/Shooting Enemy Data", order = 2)]
    public class ShootingEnemyData : EnemyData
    {
        [Header("Shooting Attributes")]
        [Tooltip("First attack pattern.")]
        public AttackPatternData pattern1;

        [Tooltip("Second attack pattern. Patterns can repeat if there are not 4.")]
        public AttackPatternData pattern2;

        [Tooltip("Third attack pattern. Patterns can repeat if there are not 4.")]
        public AttackPatternData pattern3;

        [Tooltip("Fourth attack pattern. Patterns can repeat if there are not 4.")]
        public AttackPatternData pattern4;

        [Header("Hovering Attributes")]
        [Tooltip("If this enemy can hover (also move).")]
        public bool canHover;

        [Tooltip("If the hover/movement is vertical. False means horizontal.")]
        public bool isVertical;

        [Tooltip("The distance between the endpoints for the hover/movement.")]
        public float hoverDistance;

        [Tooltip("How long the hover/movement takes, in seconds.")]
        public float hoverTime;
    }
}