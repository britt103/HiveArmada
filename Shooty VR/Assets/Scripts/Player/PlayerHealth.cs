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
using Hive.Armada;

namespace Hive.Armada.Player
{
    public class PlayerHealth : MonoBehaviour
    {
        public ShipController shipController;
        public int maxHealth = 100;
        private int currentHealth;
        public bool isAlive { get; private set; }

        void Start()
        {
            currentHealth = maxHealth;
            isAlive = true;
        }

        public void Hit(int damage)
        {
            if (Utility.isDebug)
                Debug.Log("Hit for " + damage + " damage! Remaining health = " + currentHealth);

            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                if (shipController != null)
                {
                    shipController.hand.DetachObject(gameObject);
                }
            }
        }
    }
}
