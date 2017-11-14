//Name: Chad Johnson
//Student ID: 1763718
//Email: johns428@mail.chapman.edu
//Course: CPSC 340-01, CPSC-344-01
//Assignment: Group Project
//Purpose: Tutorial/tooltip test script

using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;

namespace Hive.Armada.Player
{
    public class Tooltip : MonoBehaviour
    {
        public Text tooltipText;

        public string fireTooltip = "Hold to Fire";
        public string powerupTooltip = "Press to Activate Powerup";
        public string pauseTooltip = "Press to Pause";

        //public string powerup1Tooltip = "Helper Ally Ship";
        //public string powerup2Tooltip = "Launch Timed Explosive";
        //public string powerup3Tooltip = "Destroy Enemy Projectiles";
        //public string powerup4Tooltip = "Increase Weapon Damage";
        //public string powerup5Tooltip = "Protective Barrier";

        public string[] powerupTooltips;

        EVRButtonId trigger = EVRButtonId.k_EButton_SteamVR_Trigger;
        EVRButtonId touchpad = EVRButtonId.k_EButton_SteamVR_Touchpad;
        EVRButtonId menuButton = EVRButtonId.k_EButton_ApplicationMenu;

        Hand hand;

        // Use this for initialization
        void Start()
        {
            hand = GetComponentInParent<Hand>();
        }

        public void ShowFire()
        {
            ControllerButtonHints.ShowTextHint(hand, trigger, fireTooltip);
        }

        public void ShowPowerup()
        {
            ControllerButtonHints.ShowTextHint(hand, touchpad, powerupTooltip);
        }

        public void ShowPause()
        {
            ControllerButtonHints.ShowTextHint(hand, menuButton, pauseTooltip);
        }

        public void ShowPowerup1()
        {
            //ControllerButtonHints.ShowTextHint(hand, touchpad, powerupTooltips[0]);
            tooltipText.text = powerupTooltips[0];
        }

        public void ShowPowerup2()
        {
            //ControllerButtonHints.ShowTextHint(hand, touchpad, powerupTooltips[1]);
            tooltipText.text = powerupTooltips[1];
        }

        public void ShowPowerup3()
        {
            //ControllerButtonHints.ShowTextHint(hand, touchpad, powerupTooltips[2]);
            tooltipText.text = powerupTooltips[2];
        }

        public void ShowPowerup4()
        {
            //ControllerButtonHints.ShowTextHint(hand, touchpad, powerupTooltips[3]);
            tooltipText.text = powerupTooltips[3];
        }

        public void ShowPowerup5()
        {
            //ControllerButtonHints.ShowTextHint(hand, touchpad, powerupTooltips[4]);
            tooltipText.text = powerupTooltips[4];
        }

        public void HideAll()
        {
            ControllerButtonHints.HideAllTextHints(hand);
            tooltipText.text = "";
        }
    }
}
