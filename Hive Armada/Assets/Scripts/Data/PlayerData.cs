//=============================================================================
// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-440-01
// Group Project
// 
//=============================================================================

using System;
using UnityEngine;

namespace Hive.Armada.Data
{
    [Serializable]
    public struct LightSettings
    {
        public Color color;

        public float intensity;

        public LightSettings(Color _color, float _intensity)
        {
            color = _color;
            intensity = _intensity;
        }
    }

    [CreateAssetMenu(fileName = "New Player Data", menuName = "Player Data")]
    public class PlayerData : ScriptableObject
    {
        [Header("Health")]
        public int playerMaxHealth;

        public int playerLowHealth;

        public Material flashColor;

        public GameObject deathEmitter;

        public AudioClip hitSound;

        public AudioClip lowHealthSound;

        public AudioClip healSound;

        [Header("Dialogue")]
        public AudioClip[] shipIntroClips;

        public AudioClip[] shipWeaponIntroClips;

        [Header("Skins")]
        public Material[] shipBodyMaterials;

        public Material[] shipMinigunMaterials;

        public Material[] shipMinigunOverheatMaterials;

        public Material[] shipRocketPodsMaterials;

        public Material[] shipPodDestroyedMaterials;
        
        public LightSettings[] skinLightSettings;

        [Header("Pickup Skins")]
        public Material[] pickupBodyMaterials;

        public Material[] pickupMinigunMaterials;

        public Material[] pickupRocketPodsMaterials;

        public LightSettings[] pickupLightSettings;
    }
}