using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;


public class ShipController : VRTK_InteractableObject
{
    public Transform powerupPoint;
    public bool isTriggerPressed = false;

    public override void StartUsing(VRTK_InteractUse usingObject)
    {
        base.StartUsing(usingObject);
        StartDeath();
    }

    public override void StopUsing(VRTK_InteractUse usingObject)
    {
        base.StopUsing(usingObject);
        StopDeath();
    }

    private void StartDeath()
    {
        Debug.Log("trigger pressed");
        isTriggerPressed = true;
    }

    private void StopDeath()
    {
        Debug.Log("trigger released");
        isTriggerPressed = false;
    }
}
