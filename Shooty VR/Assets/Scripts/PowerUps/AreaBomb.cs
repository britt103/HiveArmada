//Name: Chad Johnson
//Student ID: 1763718
//Email: johns428@mail.chapman.edu
//Course: CPSC 340-01, CPSC-344-01
//Assignment: Group Project
//Purpose: Script area bomb powerup behavior; bomb accelerates forward until detonation

//http://answers.unity3d.com/questions/459602/transformforward-problem.html

using UnityEngine;
using Valve.VR.InteractionSystem;
using System.Collections;

namespace Hive.Armada
{
    public class AreaBomb : MonoBehaviour
    {
        public float radius;
        public float acceleration;
        public float startingZ;
        public float detonationTime = 2.0f;

        private float currentSpeed;
        //private Hand hand;
        public GameObject fxTrail, fxBomb;

        /// <summary>
        /// State of whether bomb should be moving.
        /// </summary>
        private bool isMoving = true;

        // Use this for initialization
        void Start()
        {
            StartCoroutine(Detonate());
            fxTrail.SetActive(true);

            transform.localPosition = new Vector3(0, 0, startingZ);
            gameObject.transform.parent = null;
        }

        //Update is called once per frame
        void Update()
        {
            // accelerating forward

            if (isMoving)
            {
                currentSpeed += acceleration * Time.deltaTime;
                transform.Translate(Vector3.forward * currentSpeed);
            }

            //if (hand.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad))
            //{
            //    Vector2 touchpad = hand.controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0);

            //    if (touchpad.y < -0.7)
            //    {
            //        if (released)
            //        {
            //            // button-based detonation

            //        }
            //        else
            //        {
            //            GameObject.Find("Player").GetComponent<PowerUpStatus>().SetAreaBomb(false);
            //            gameObject.transform.parent = null;
            //            released = true;
            //            fxTrail.SetActive(true);
            //        }
            //    }
            //}

        }

        /// <summary>
        /// Damage nearby enemies and destroy self after certain amount of time
        /// </summary>
        /// <returns>IEnumerator for coroutine</returns>
        private IEnumerator Detonate()
        {
            yield return new WaitForSeconds(detonationTime);
            foreach (Collider objectCollider in Physics.OverlapSphere(transform.position, radius))
            {
                if (objectCollider.gameObject.tag == "Enemy")
                {
                    objectCollider.gameObject.GetComponent<Enemies.Enemy>().Hit(100);
                }
            }
            Instantiate(fxBomb, transform.position, transform.rotation);
            FindObjectOfType<PowerUpStatus>().areaBombActive = false;
            Destroy(gameObject);
        }

        /// <summary>
        /// Trigger detonation on impact with enemy or stop movement on impact with room.
        /// </summary>
        /// <param name="other">collider of object this collided with</param>
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy") || other.CompareTag("Room"))
            {
                foreach (Collider objectCollider in Physics.OverlapSphere(transform.position, radius))
                {
                    if (objectCollider.gameObject.tag == "Enemy")
                    {
                        objectCollider.gameObject.GetComponent<Enemies.Enemy>().Hit(100);
                    }
                }
                Instantiate(fxBomb, transform.position, transform.rotation);
                FindObjectOfType<PowerUpStatus>().areaBombActive = false;
                Destroy(gameObject);
            }
            else if (other.CompareTag("Room"))
            {
                currentSpeed = 0.0f;
                acceleration = 0.0f;
            }
        }

        /// <summary>
        /// Cease movement.
        /// </summary>
        public void Stop()
        {
            isMoving = false;
        }
    }
}
