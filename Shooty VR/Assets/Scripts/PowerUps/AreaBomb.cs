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
    [RequireComponent(typeof(Interactable))]
    public class AreaBomb : MonoBehaviour
    {
        public float radius;
        public float acceleration;

        private float currentSpeed;
        //private Hand hand;
        public GameObject fxTrail, fxBomb;

        // Use this for initialization
        void Start()
        {
            //hand = gameObject.GetComponentInParent<Hand>();
            StartCoroutine(Detonate());
            fxTrail.SetActive(true);
            gameObject.transform.parent = null;
        }

        //Update is called once per frame
        void Update()
        {
            // accelerating forward

            currentSpeed += acceleration * Time.deltaTime;
            transform.Translate(Vector3.forward * currentSpeed);

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

        private IEnumerator Detonate()
        {
            yield return new WaitForSeconds(2);
            foreach (Collider objectCollider in Physics.OverlapSphere(transform.position, radius))
            {
                if (objectCollider.gameObject.tag == "Enemy")
                {
                    objectCollider.gameObject.GetComponent<Enemies.Enemy>().Hit(100);
                }
            }
            Instantiate(fxBomb, transform.position, transform.rotation);
            Destroy(gameObject);
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                foreach (Collider objectCollider in Physics.OverlapSphere(transform.position, radius))
                {
                    if (objectCollider.gameObject.tag == "Enemy")
                    {
                        objectCollider.gameObject.GetComponent<Enemies.Enemy>().Hit(100);
                    }
                }
                Instantiate(fxBomb, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }
}
