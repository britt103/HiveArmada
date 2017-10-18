//Name: Chad Johnson
//Student ID: 1763718
//Email: johns428@mail.chapman.edu
//Course: CPSC 340-01, CPSC-344-01
//Assignment: Group Project
//Purpose: Script area bomb powerup behavior; bomb accelerates forward until detonation

//http://answers.unity3d.com/questions/459602/transformforward-problem.html

using UnityEngine;
using Valve.VR.InteractionSystem;

namespace Hive.Armada
{
    [RequireComponent(typeof(Interactable))]
    public class AreaBomb : MonoBehaviour
    {
        public float radius;
        public float acceleration;

        private float currentSpeed;
        private bool released;
        private Hand hand;

        // Use this for initialization
        void Start()
        {
            hand = gameObject.GetComponentInParent<Hand>();
            released = false;
        }

        //Update is called once per frame
        void Update()
        {
            //accelerating forward
            if (released)
            {
                currentSpeed += acceleration * Time.deltaTime;
                transform.Translate(Vector3.forward * currentSpeed);

                //button-based detonation
                if(hand.controller.GetHairTriggerDown())
                {
                    foreach (Collider objectCollider in Physics.OverlapSphere(transform.position, radius))
                    {
                        if (objectCollider.gameObject.tag == "Enemy")
                        {
                            objectCollider.gameObject.GetComponent<Enemies.EnemyBasic>().Hit(100);
                        }
                    }
                    Destroy(gameObject);
                }
            }

            if (!released && hand.controller.GetHairTriggerDown())
            {
                GameObject.Find("Player").GetComponent<PowerUpStatus>().SetAreaBomb(false);

                gameObject.transform.parent = null;

                released = true;
            }
        }
    }
}
