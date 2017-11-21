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

using System.Collections;
using System.Collections.Generic;
using Hive.Armada.Game;
using UnityEngine;
using Hive.Armada.Player;

namespace Hive.Armada.Enemies
{
    /// <summary>
    /// 
    /// </summary>
    public class Projectile : Poolable
    {
        /// <summary>
        /// 
        /// </summary>
        public int damage;

        /// <summary>
        /// 
        /// </summary>
        public float lifetime;

        private void Start()
        {
            //Destroy(gameObject, lifetime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (other.GetComponent<PlayerHealth>() != null)
                {
                    other.GetComponent<PlayerHealth>().Hit(damage);
                }
                else
                {
                    if (Utility.isDebug)
                    {
                        Debug.Log(
                            "[WARNING] GameObject tagged with \"Player\" does NOT have PlayerHealth.cs on it!");
                    }
                }
                Destroy(gameObject);
            }
            else if (other.CompareTag("Room"))
            {
                Destroy(gameObject);
            }
        }

        protected override void Reset()
        {
            damage = 10;
            lifetime = 10.0f;
        }
    }
}