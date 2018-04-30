//=============================================================================
//
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-440-01
// Group Project
//
// This class enables the ship pickup preview to reflect
// the player's chosen weapon.
//
//=============================================================================

using System.Collections.Generic;
using Hive.Armada.Data;
using UnityEngine;
using Hive.Armada.Game;

namespace Hive.Armada.Player
{
    /// <summary>
    /// Enables the chosen weapon on the pickup preview.
    /// </summary>
    public class PickupPreview : MonoBehaviour
    {
        /// <summary>
        /// Reference manager that holds all needed references
        /// (e.g. spawner, game manager, etc.)
        /// </summary>
        private ReferenceManager reference;

        /// <summary>
        /// 
        /// </summary>
        public PlayerData playerData;

        /// <summary>
        /// The minigun game object.
        /// </summary>
        public GameObject minigun;

        /// <summary>
        /// The rocket pods game object.
        /// </summary>
        public GameObject rocketPods;

        public Renderer[] bodyRenderers;

        public Renderer[] minigunRenderers;

        public Renderer[] rocketPodRenderers;

        public Light[] lights;

        /// <summary>
        /// Enables the minigun or rocket pods if chosen.
        /// </summary>
        private void Awake()
        {
             reference = FindObjectOfType<ReferenceManager>();
            
             if (reference != null)
             {
                 reference = FindObjectOfType<ReferenceManager>();
                 
                 UpdateSkin(reference.gameSettings.selectedSkin);
            
                 switch ((int)reference.gameSettings.selectedWeapon)
                 {
                     case 1:
                         minigun.SetActive(true);
                         rocketPods.SetActive(false);
                         break;
                     case 2:
                         minigun.SetActive(false);
                         rocketPods.SetActive(true);
                         break;
                     default:
                         minigun.SetActive(false);
                         rocketPods.SetActive(false);
                         break;
                 }
             }
        }

        /// <summary>
        /// Configures the material and lights on the pickup to match the selected skin.
        /// </summary>
        /// <param name="skin"> The index of the selected skin </param>
        private void UpdateSkin(int skin)
        {
            if (skin == 0)
                return;

            foreach (Renderer r in bodyRenderers)
            {
                r.material = playerData.pickupBodyMaterials[skin];
            }
            
            foreach (Renderer r in minigunRenderers)
            {
                r.material = playerData.pickupMinigunMaterials[skin];
            }
            
            foreach (Renderer r in rocketPodRenderers)
            {
                r.material = playerData.pickupRocketPodsMaterials[skin];
            }

            foreach (Light l in lights)
            {
                l.color = playerData.pickupLightSettings[skin].color;
                l.intensity = playerData.pickupLightSettings[skin].intensity;
            }
        }
    }
}