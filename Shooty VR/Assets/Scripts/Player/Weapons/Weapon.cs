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

namespace Hive.Armada.Player.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        public ShipController shipController;
        public int damageBoost;
        protected int damage;
        protected float fireRate;
        protected bool canShoot = true;

        public virtual void TriggerUpdate()
        {
            if (canShoot)
            {
                Clicked();
            }
        }

        protected abstract void Clicked();
    }
}
