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
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;
    public float lifetime;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<CalcHealth>() != null)
            {
                other.GetComponent<CalcHealth>().Hit(damage);
            }
            else
            {
                Debug.Log("[WARNING] GameObject tagged with \"Player\" does NOT have CalcHealth on it!");
            }
            Destroy(gameObject);
        }
        else if (other.CompareTag("Room"))
        {
            Destroy(gameObject);
        }
    }
}
