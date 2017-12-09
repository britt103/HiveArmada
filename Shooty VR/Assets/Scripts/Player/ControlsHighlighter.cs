//Name: Chad Johnson
//Student ID: 1763718
//Email: johns428@mail.chapman.edu
//Course: CPSC 340-01, CPSC-344-01
//Assignment: Group Project
//Purpose: Tutorial/tooltip test script

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class ControlsHighlighter : MonoBehaviour
{
    public string fireTooltip = "Hold to Fire";
    public string powerupTooltip = "Press to Activate Powerup";
    public string pauseTooltip = "Press to Pause";

    EVRButtonId trigger = EVRButtonId.k_EButton_SteamVR_Trigger;
    EVRButtonId touchpad = EVRButtonId.k_EButton_SteamVR_Touchpad;
    EVRButtonId menuButton = EVRButtonId.k_EButton_ApplicationMenu;

    Hand hand;

    // Use this for initialization
    void Start()
    {
        hand = GetComponentInParent<Hand>();
    }

    public void FireOn()
    {
        ControllerButtonHints.ShowTextHint(hand, trigger, fireTooltip);
    }

    public void PowerupOn()
    {
        ControllerButtonHints.ShowTextHint(hand, touchpad, powerupTooltip);
    }

    public void PauseOn()
    {
        ControllerButtonHints.ShowTextHint(hand, menuButton, pauseTooltip);
    }

    public void AllOff()
    {
        ControllerButtonHints.HideAllTextHints(hand);
    }
}