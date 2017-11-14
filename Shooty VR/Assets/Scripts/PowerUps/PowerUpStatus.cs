//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.champan.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// PowerupStatus tracks the powerups currently stored and currently active. A
// powerup cannot be activated if a powerup of the same type is currently 
// active. Powerups are activated upon controller input.
//
//=============================================================================

using System.Collections.Generic;
using UnityEngine;
using Hive.Armada.Player;
using Valve.VR.InteractionSystem;

namespace Hive.Armada.Powerups
{
    /// <summary>
    /// Tracks stored powerups and activations.
    /// </summary>
    public class PowerupStatus : MonoBehaviour
    {
        /// <summary>
        /// Array of states of whether each type of powerup is currently stored.
        /// </summary>
        //public bool[] powerupTypeStored;

        /// <summary>
        /// Array of states of whether each type of powerup is currently active.
        /// </summary>
        //public bool[] powerupTypeActive;

        /// <summary>
        /// Queue containing powerup prefabs.
        /// </summary>
        private Queue<GameObject> powerups = new Queue<GameObject>();

        /// <summary>
        /// Queue containing powerup icons.
        /// </summary>
        private Queue<GameObject> powerupIcons = new Queue<GameObject>();

        /// <summary>
        /// Array containing names of powerups
        /// </summary>
        public string[] powerupNames;

        /// <summary>
        /// Maximum number of allowed stored powerups.
        /// </summary>
        public int maxStoredPowerups = 3;

        /// <summary>
        /// Distance between icons.
        /// </summary>
        public float iconSpacing = 1f;

        //public float alphaDelta = 30f;

        /// <summary>
        /// Reference to player ship.
        /// </summary>
        private GameObject shipGO;

        /// <summary>
        /// References to player ship powerup point.
        /// </summary>
        private Transform powerupPoint;

        /// <summary>
        /// References to playership icon point.
        /// </summary>
        private Transform iconPoint;

        /// <summary>
        /// References to PlayerStats,
        /// </summary>
        private PlayerStats stats;

        /// <summary>
        /// Reference to active hand.
        /// </summary>
        private Hand hand;

        /// <summary>
        /// References to Tooltip on active hand.
        /// </summary>
        private Tooltip tooltip;

        /// <summary>
        /// State of whether PowerupStatus is tracking inputs. 
        /// </summary>
        public bool tracking = false;

        /// <summary>
        /// Find references.
        /// </summary>
        private void Start()
        {
            stats = FindObjectOfType<PlayerStats>();
        }

        /// <summary>
        /// Activate powerup and tooltip if input is detected.
        /// </summary>
        private void Update()
        {
            if (tracking && powerups.Count > 0)
            {
                if (hand.controller.GetTouch(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad))
                {
                    //    //switch (powerups.Peek().name)
                    //    //{
                    //    //    case "Ally":
                    //    //        ch.ShowPowerup1();
                    //    //        break;

                    //    //    case "Area Bomb":
                    //    //        ch.ShowPowerup2();
                    //    //        break;

                    //    //    case "Clear":
                    //    //        ch.ShowPowerup3();
                    //    //        break;

                    //    //    case "Damage Boost":
                    //    //        ch.ShowPowerup4();
                    //    //        break;

                    //    //    case "Shield":
                    //    //        ch.ShowPowerup5();
                    //    //        break;
                    //    //}

                    string nextPowerupName = powerups.Peek().name;

                    for (int i = 0; i < powerupNames.Length; ++i)
                    {
                        if (nextPowerupName == powerupNames[i])
                        {
                            tooltip.Invoke("ShowPowerup" + (i + 1), 0);
                            break;
                        }
                    }
                }

                if (hand.controller.GetTouchUp(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad))
                {
                    tooltip.HideAll();
                }

                if (hand.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad))
                {
                    //bool canActivate = true;

                    //switch (powerups.Peek().name)
                    //{
                    //    case "Ally":
                    //        if (powerupTypeActive[0])
                    //        {
                    //            canActivate = false;
                    //        }
                    //        else
                    //        {
                    //            powerupTypeStored[0] = false;
                    //            powerupTypeActive[0] = true;
                    //            stats.AllyCount();
                    //        }

                    //        break;

                    //    case "Area Bomb":
                    //        if (powerupTypeActive[1])
                    //        {
                    //            canActivate = false;
                    //        }
                    //        else
                    //        {
                    //            powerupTypeStored[1] = false;
                    //            powerupTypeActive[1] = true;
                    //            stats.AreaBombCount();
                    //        }

                    //        break;

                    //    case "Clear":
                    //        if (powerupTypeActive[2])
                    //        {
                    //            canActivate = false;
                    //        }
                    //        else
                    //        {
                    //            powerupTypeStored[2] = false;
                    //            powerupTypeActive[2] = true;
                    //            stats.ClearCount();
                    //        }

                    //        break;

                    //    case "Damage Boost":
                    //        if (powerupTypeActive[3])
                    //        {
                    //            canActivate = false;
                    //        }
                    //        else
                    //        {
                    //            powerupTypeStored[3] = false;
                    //            powerupTypeActive[3] = true;
                    //            stats.DamageBoostCount();
                    //        }

                    //        break;

                    //    case "Shield":
                    //        if (powerupTypeActive[4])
                    //        {
                    //            canActivate = false;
                    //        }
                    //        else
                    //        {
                    //            powerupTypeStored[4] = false;
                    //            powerupTypeActive[4] = true;
                    //            stats.ShieldCount();
                    //        }

                    //        break;
                    //}

                    //if (canActivate)
                    //{
                    //    Instantiate(powerups.Dequeue(), powerupPoint);

                    //    RemoveDisplayIcon();
                    //}

                    Instantiate(powerups.Dequeue(), powerupPoint);
                    RemoveDisplayIcon();
                    //tooltip.HideAll();
                }
            }
        }

        /// <summary>
        /// Trigger PowerupStatus to start tracking and find references.
        /// </summary>
        public void BeginTracking()
        {
            tracking = true;
            shipGO = gameObject.GetComponentInChildren<ShipController>().gameObject;
            hand = shipGO.GetComponentInParent<Hand>();
            tooltip = hand.GetComponentInChildren<Tooltip>();
            powerupPoint = shipGO.transform.Find("Powerup Point");
            iconPoint = shipGO.transform.Find("Powerup Icon Point");
        }

        /// <summary>
        /// Add powerup to queues.
        /// </summary>
        /// <param name="powerupPrefab">GameObject of powerup</param>
        /// <param name="powerupIconPrefab">GameObject of powerup icon</param>
        public void StorePowerup(GameObject powerupPrefab, GameObject powerupIconPrefab)
        {
            powerups.Enqueue(powerupPrefab);
            GameObject newIcon = Instantiate(powerupIconPrefab, iconPoint);
            powerupIcons.Enqueue(newIcon);
            UpdateDisplayIcon(newIcon);
        }

        /// <summary>
        /// Adjust attributes of newly added icon based on queue count.
        /// </summary>
        /// <param name="newIcon">Newly added icon</param>
        private void UpdateDisplayIcon(GameObject newIcon)
        {
            //position
            newIcon.transform.localPosition = new Vector3
                    (iconSpacing * (powerupIcons.Count - 1), 0, 0);

            //scale
            //newIcon.transform.localScale *= (powerupIcons.Count / maxStoredPowerups);

            //transparency
            //Color color = newIcon.GetComponent<MeshRenderer>().material.color;
            //color.a = (255 * (float)(powerupIcons.Count / maxStoredPowerups));
            //color.a -= (alphaDelta * (powerupIcons.Count - 1));
            //newIcon.GetComponent<MeshRenderer>().material.color = color;
        }

        /// <summary>
        /// Removce icon and shift remaining icons.
        /// </summary>
        private void RemoveDisplayIcon()
        {
            Destroy(powerupIcons.Dequeue());
            foreach(GameObject icon in powerupIcons)
            {
                icon.transform.localPosition -= new Vector3(iconSpacing, 0, 0);

                //icon.transform.localScale += new Vector3((1 / maxStoredPowerups), (1 / maxStoredPowerups), (1 / maxStoredPowerups));

                //Color color = icon.GetComponent<MeshRenderer>().material.color;
                //color.a += alphaDelta;
                //icon.GetComponent<MeshRenderer>().material.color = color;
            }
        }

        /// <summary>
        /// Clear powerup queues and remove all icons. Meant to be used between waves. 
        /// </summary>
        public void RemoveStoredPowerups()
        {
            powerups.Clear();
            foreach(GameObject icon in powerupIcons)
            {
                Destroy(icon);
            }
            powerupIcons.Clear();
        }

        /// <summary>
        /// Return status of queue capacity.
        /// </summary>
        /// <returns>State of whether there is room left in queue.</returns>
        public bool HasRoom()
        {
            return (powerups.Count < maxStoredPowerups);
        }
    }
}
