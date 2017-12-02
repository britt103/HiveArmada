//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// StartMenu controls interactions with the Start Menu.
//
//=============================================================================

using UnityEngine;
using Hive.Armada.Game;
using Hive.Armada.Player;

namespace Hive.Armada.Menus
{
    /// <summary>
    /// Controls interactions with Start Menu;
    /// </summary>
    public class StartMenu : MonoBehaviour
    {
        /// <summary>
        /// Reference to Spawner.
        /// </summary>
        public Spawner spawner;

        /// <summary>
        /// Solo Normal button pressed. Begin Solo Normal game mode.
        /// </summary>
        public void PressSoloNormal()
        {
            GameObject ship = GameObject.FindGameObjectWithTag("Player");

            if (ship != null)
            {
                if (ship.GetComponent<ShipController>() != null)
                    ship.GetComponent<ShipController>().SetShipMode(ShipController.ShipMode.Game);
            }

                GameObject ship = null;

                foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
                {
                    if (obj.GetComponent<ShipController>())
                    {
                        ship = obj;
                    }
                }

                if (ship != null)
                {
                    if (ship.GetComponent<ShipController>() != null)
                        ship.GetComponent<ShipController>().SetShipMode(ShipController.ShipMode.Game);
                }

                if (spawner != null)
                {
                    //gameObject.GetComponentInChildren<Button>().enabled = false;
                    GameObject.Find("Main Canvas").transform.Find("Countdown").gameObject.SetActive(true);
                    //gameObject.transform.parent.Find("Ambient FX").gameObject.SetActive(false);
                    gameObject.SetActive(false);
                }
                else
                {
                    Debug.Log("CRITICAL - MENU'S REFERENCE TO SPAWNER IS NULL");
                }  
            }
            else
            {
                Debug.Log("CRITICAL - MENU'S REFERENCE TO SPAWNER IS NULL");
            }  
        }

        /// <summary>
        /// Back button pressed. Navigate to Main Menu.
        /// </summary>
        public void PressBack()
        {
            GameObject.Find("Main Canvas").transform.Find("Main Menu").gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
