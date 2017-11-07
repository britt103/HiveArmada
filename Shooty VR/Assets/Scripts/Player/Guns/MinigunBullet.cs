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
using Hive.Armada.Enemies;
using Valve.VR.InteractionSystem;

namespace Hive.Armada.Player.Guns
{
    public class MinigunBullet : MonoBehaviour
    {
        public int damage;
        public float lifetime;
        public Hand hand;

        void Start()
        {
            Destroy(gameObject, lifetime);
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                if (other.gameObject.GetComponent<Enemy>() != null)
                {
                    other.gameObject.GetComponent<Enemy>().Hit(damage);

                    if (hand != null)
                        hand.controller.TriggerHapticPulse(2500);
                }
                else
                {
                    if (Utility.isDebug)
                        Debug.Log("[WARNING] GameObject tagged with \"Enemy\" does NOT have EnemyBasic.cs on it!");
                }
                Destroy(gameObject);
            }
            else if (other.CompareTag("Room"))
            {
                Destroy(gameObject);
            }
        }
    }
}
