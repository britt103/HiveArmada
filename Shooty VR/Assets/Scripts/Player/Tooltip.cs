//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.champan.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// Tooltip controls textual instruction and tip display to the player.
// Information is shown using canvases that are children of the Hand game 
// objects.
//
//=============================================================================

using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;

namespace Hive.Armada.Player
{
    /// <summary>
    /// Display tooltips.
    /// </summary>
    public class Tooltip : MonoBehaviour
    {
        /// <summary>
        /// Reference to Text in canvas.
        /// </summary>
        public Text tooltipText;

        /// <summary>
        /// Tooltip for holding the fire button.
        /// </summary>
        public string fireButtonTooltip = "Hold to Fire";

        /// <summary>
        /// Tooltip for pressing the powerup button
        /// </summary>
        public string powerupButtonTooltip = "Press to Activate Powerup";

        //public string powerup1Tooltip = "Helper Ally Ship";
        //public string powerup2Tooltip = "Launch Timed Explosive";
        //public string powerup3Tooltip = "Destroy Enemy Projectiles";
        //public string powerup4Tooltip = "Increase Weapon Damage";
        //public string powerup5Tooltip = "Protective Barrier";

        /// <summary>
        /// Names of powerups.
        /// </summary>
        public string[] powerupNames;

        /// <summary>
        /// Tooltips for powerups, in order.
        /// </summary>
        public string[] powerupTooltips;

        /// <summary>
        /// ButtonID for the trigger.
        /// </summary>
        EVRButtonId trigger = EVRButtonId.k_EButton_SteamVR_Trigger;

        /// <summary>
        /// ButtonID for the touchpad.
        /// </summary>
        EVRButtonId touchpad = EVRButtonId.k_EButton_SteamVR_Touchpad;

        /// <summary>
        /// Reference to hand.
        /// </summary>
        Hand hand;

        // Find references.
        void Start()
        {
            hand = GetComponentInParent<Hand>();
        }

        /// <summary>
        /// Display firing tooltip.
        /// </summary>
        public void ShowFireButton()
        {
            ControllerButtonHints.ShowTextHint(hand, trigger, fireButtonTooltip);
        }

        /// <summary>
        /// Display powerup button tooltip.
        /// </summary>
        public void ShowPowerupButton()
        {
            ControllerButtonHints.ShowTextHint(hand, touchpad, powerupButtonTooltip);
        }

        /// <summary>
        /// Display powerup tooltip for corresponding powerup.
        /// </summary>
        /// <param name="name">Name of powerup.</param>
        public void ShowPowerup(string name)
        {
            for(int i = 0; i < powerupNames.Length; ++i)
            {
                if(name == powerupNames[i])
                {
                    tooltipText.text = powerupTooltips[i];
                }
            }
        }

        /// <summary>
        /// Hide all tooltips.
        /// </summary>
        public void HideAll()
        {
            ControllerButtonHints.HideAllTextHints(hand);
            tooltipText.text = "";
        }
    }
}
