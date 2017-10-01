//=============================================================================
// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
// 
// [DESCRIPTION]
// 
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameName.Player
{
    public abstract class Gun : MonoBehaviour
    {
        public int damage;
        public float fireRate;
        protected bool canShoot = true;

        public abstract void TriggerUpdate();
    }
}
