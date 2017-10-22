﻿//=============================================================================
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

namespace Hive.Armada.Player.Guns
{
    public abstract class Gun : MonoBehaviour
    {
        public ShipController shipController;
        protected int damage;
        protected float fireRate;
        protected bool canShoot = true;

        public abstract void TriggerUpdate();
    }
}
