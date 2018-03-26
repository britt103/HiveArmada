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
        /// The minigun game object.
        /// </summary>
        public GameObject minigun;

        /// <summary>
        /// The rocket pods game object.
        /// </summary>
        public GameObject rocketPods;

        /// <summary>
        /// Enables the minigun or rocket pods if chosen.
        /// </summary>
        private void Awake()
        {
            reference = FindObjectOfType<ReferenceManager>();

            if (reference != null)
            {
                switch (reference.gameSettings.weapon)
                {
                    case 1:
                        minigun.SetActive(true);
                        break;
                    case 2:
                        rocketPods.SetActive(true);
                        break;
                }
            }
        }
    }
}