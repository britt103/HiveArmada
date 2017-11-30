//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.champan.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// Clear controls the Clear powerup. The Clear destroys all enemy projectiles
// currently in the scene. Currently assigned as Powerup 3.
//
//=============================================================================

using UnityEngine;

namespace Hive.Armada.PowerUps
{
    /// <summary>
    /// Clear powerup.
    /// </summary>
    public class ClearBomb : MonoBehaviour
    {
        /// <summary>
        /// FX of Clear activation.
        /// </summary>
        public GameObject awakeEmitter;

        // Instantiate activation FX. Destroy enemy projectiles. Self-destruct.
        void Start()
        {
            Instantiate(awakeEmitter, transform.position, transform.rotation);
            foreach (GameObject bullet in GameObject.FindGameObjectsWithTag("bullet"))
            {
                Destroy(bullet);
            }
            //FindObjectOfType<PowerupStatus>().powerupTypeActive[2] = false;
            Destroy(gameObject);
        }

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