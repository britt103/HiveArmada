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
            //hand = gameObject.GetComponentInParent<Hand>();

            foreach (GameObject bullet in GameObject.FindGameObjectsWithTag("bullet"))
            {
                Destroy(bullet);
            }
            Destroy(gameObject);
        }

        // Update is called once per frame
        //void Update()
        //{
        //    if (hand.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad))
        //    {
        //        Vector2 touchpad = hand.controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0);

        //        if (touchpad.y < -0.7)
        //        {
        //            GameObject.Find("Player").GetComponent<PowerUpStatus>().SetClear(false);

        //            foreach (GameObject bullet in GameObject.FindGameObjectsWithTag("bullet"))
        //            {
        //                Destroy(bullet);
        //            }
        //            Destroy(gameObject);
        //        }
        //    }
        //}
    }

}