using System.Collections;
using System.Collections.Generic;
using Hive.Armada.Game;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class TalkButton : MonoBehaviour
{
    public ReferenceManager reference;

    public Hand hand;

    private void Update()
    {
        if (hand != null && hand.controller != null)
        {
            if (hand.controller.GetPressDown(EVRButtonId.k_EButton_SteamVR_Touchpad))
            {
                if (reference.talkingParticle.small.isActiveAndEnabled)
                {
                    reference.talkingParticle.small.Talk();
                }
                if (reference.talkingParticle.large.isActiveAndEnabled)
                {
                    reference.talkingParticle.large.Talk();
                }
            }
            else if (hand.controller.GetPressUp(EVRButtonId.k_EButton_SteamVR_Touchpad))
            {
                reference.talkingParticle.small.IsTalking = false;
                reference.talkingParticle.large.IsTalking = false;
            }
        }
    }
}
