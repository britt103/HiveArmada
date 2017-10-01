//
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
// 
// [DESCRIPTION]
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

namespace GameName
{
    public class ShipController : VRTK_InteractableObject
    {
        public Transform powerupPoint;
        public bool isTriggerPressed = false;

        public override void StartUsing(VRTK_InteractUse usingObject)
        {
            base.StartUsing(usingObject);
            isTriggerPressed = true;
        }

        public override void StopUsing(VRTK_InteractUse usingObject)
        {
            base.StopUsing(usingObject);
            isTriggerPressed = false;
        }
    }
}
