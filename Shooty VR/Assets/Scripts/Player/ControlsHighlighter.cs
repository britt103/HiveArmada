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

public class ControlsHighlighter : MonoBehaviour {
    public string triggerTooltip = "Hold to Fire";
    public string areaBombTooltip = "Press to Launch, Again to Detonate";
    public string clearTooltip = "Press to Detonate";

    EVRButtonId trigger = EVRButtonId.k_EButton_SteamVR_Trigger;
    EVRButtonId touchpad = EVRButtonId.k_EButton_SteamVR_Touchpad;

    Hand hand;

	// Use this for initialization
	void Start () {
        hand = GetComponentInParent<Hand>();
	}

    public void TriggerOn()
    {
        ControllerButtonHints.ShowTextHint(hand, trigger, triggerTooltip);
    }

    public void AreaBombOn()
    {
        ControllerButtonHints.ShowTextHint(hand, touchpad, areaBombTooltip);
    }

    public void ClearOn()
    {
        ControllerButtonHints.ShowTextHint(hand, touchpad, clearTooltip);
    }

    public void AllOff()
    {
        ControllerButtonHints.HideAllTextHints(hand);
    }
}
