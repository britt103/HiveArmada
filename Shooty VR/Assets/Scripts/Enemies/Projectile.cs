//=============================================================================
//
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// A basic projectile. It will destroy itself after a set amount of time,
// after colliding with the room, or the player. It will also kill the player.
//
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using Hive.Armada.Game;
using UnityEngine;
using Hive.Armada.Player;

namespace Hive.Armada.Enemies
{
    /// <summary>
    /// </summary>
    public class Projectile : Poolable
    {
        /// <summary>
        /// Reference manager that holds all needed references
        /// (e.g. spawner, game manager, etc.)
        /// </summary>
        private ReferenceManager reference;

        /// <summary>
        /// </summary>
        public int damage;

        /// <summary>
        /// </summary>
        public float lifetime;

        /// <summary>
        /// Initializes the reference to the Reference Manager
        /// </summary>
        private void Awake()
        {
            reference = GameObject.Find("Reference Manager").GetComponent<ReferenceManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                reference.playerShip.GetComponent<PlayerHealth>().Hit(damage);

                //if (other.GetComponent<PlayerHealth>() != null)
                //{
                //    reference.playerShip.GetComponent<PlayerHealth>().Hit(damage);
                //}
                //else
                //{
                //    if (Utility.isDebug)
                //    {
                //        Debug.Log("[WARNING] GameObject tagged with \"Player\"" +
                //                  " does NOT have PlayerHealth.cs on it!");
                //    }
                //}
                reference.objectPoolManager.Despawn(gameObject);
            }
            else if (other.CompareTag("Room"))
            {
                reference.objectPoolManager.Despawn(gameObject);
            }
        }

        protected override void Reset()
        {
            damage = 10;
            lifetime = 10.0f;
        }
    }
}