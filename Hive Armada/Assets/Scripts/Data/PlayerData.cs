using UnityEngine;

namespace Hive.Armada.Data
{
    [CreateAssetMenu(fileName = "New Player Data", menuName = "Player Data")]
    public class PlayerData : ScriptableObject
    {
        [Header("Health")]
        public int playerMaxHealth;

        public int playerLowHealth;

        [Header("Dialogue")]
        public AudioClip[] shipIntroClips;

        public AudioClip[] shipWeaponIntroClips;
    }
}