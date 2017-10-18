//Name: Chad Johnson
//Student ID: 1763718
//Email: johns428@mail.chapman.edu
//Course: CPSC 340-01, CPSC-344-01
//Assignment: Group Project
//Purpose: Script clear bomb powerup bahavior

using UnityEngine;
using Valve.VR.InteractionSystem;

namespace Hive.Armada
{
    public class ClearBomb : MonoBehaviour
    {
        private Hand hand;
        // Use this for initialization
        void Start()
        {
            hand = gameObject.GetComponentInParent<Hand>();
        }

        // Update is called once per frame
        void Update()
        {
            if (hand.controller.GetHairTriggerDown())
            {
                GameObject.Find("Player").GetComponent<PowerUpStatus>().SetClear(false);

                foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    enemy.GetComponent<Enemies.EnemyBasic>().Hit(100);
                }
                Destroy(gameObject);
            }
        }
    }

}