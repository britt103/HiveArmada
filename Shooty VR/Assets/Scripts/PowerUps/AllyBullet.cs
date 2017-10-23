//Name: Chad Johnson
//Student ID: 1763718
//Email: johns428@mail.chapman.edu
//Course: CPSC 340-01, CPSC-344-01
//Assignment: Group Project
//Purpose: Control bullet interactions with enemies

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hive.Armada
{
    public class AllyBullet : MonoBehaviour
    {
        public int damage;
        public float lifeTime;

        // Use this for initialization
        void Start()
        {
            Destroy(gameObject, lifeTime);
        }

        /// <summary>
        /// Damage enemy upon collision, destroy self after collision with enemy or room
        /// </summary>
        /// <param name="other">Collider of object this collided with</param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                if(other.gameObject.GetComponent<Enemies.Enemy>() != null)
                {
                    other.gameObject.GetComponent<Enemies.Enemy>().Hit(damage);
                }
                Destroy(gameObject);
            }
            else if (other.gameObject.CompareTag("Room"))
            {
                Destroy(gameObject);
            }
        }
    }
}
