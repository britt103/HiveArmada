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
using Valve.VR.InteractionSystem;

namespace Hive.Armada.Powerup
{
    /// <summary>
    /// Tracks stored powerups and activations.
    /// </summary>
    public class PowerupStatus : MonoBehaviour
    {
        /// <summary>
        /// State of whether Powerup 1 is queued.
        /// </summary>
        public bool p1Stored = false;

        /// <summary>
        /// State of whether Powerup 2 is queued.
        /// </summary>
        public bool p2Stored = false;

        /// <summary>
        /// State of whether Powerup 3 is queued.
        /// </summary>
        public bool p3Stored = false;

        /// <summary>
        /// State of whether Powerup 4 is queued.
        /// </summary>
        public bool p4Stored = false;

        /// <summary>
        /// State of whether Powerup 5 is queued.
        /// </summary>
        public bool p5Stored = false;

        /// <summary>
        /// State of whether Powerup 1 is active.
        /// </summary>
        public bool p1Active = false;

        /// <summary>
        /// State of whether Powerup 1 is active.
        /// </summary>
        public bool p2Active = false;

        /// <summary>
        /// State of whether Powerup 1 is active.
        /// </summary>
        public bool p3Active = false;

        /// <summary>
        /// State of whether Powerup 1 is active.
        /// </summary>
        public bool p4Active = false;

        /// <summary>
        /// State of whether Powerup 1 is active.
        /// </summary>
        public bool p5Active = false;

        /// <summary>
        /// Queue containing powerup prefabs.
        /// </summary>
        private Queue<GameObject> powerups = new Queue<GameObject>();

        /// <summary>
        /// Queue containing powerup icons.
        /// </summary>
        private Queue<GameObject> powerupIcons = new Queue<GameObject>();

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
        /// References to ControlsHighlighter on active hand.
        /// </summary>
        private ControlsHighlighter ch;

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
                    switch (powerups.Peek().name)
                    {
                        case "Ally":
                            ch.AllyOn();
                            break;

                        case "Area Bomb":
                            ch.AreaBombOn();
                            break;

                        case "Clear":
                            ch.ClearOn();
                            break;

                        case "Damage Boost":
                            ch.DamageBoostOn();
                            break;

                        case "Shield":
                            ch.ShieldOn();
                            break;
                    }
                }

                if (hand.controller.GetTouchUp(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad))
                {
                    ch.AllOff();
                }

                if (hand.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad))
                {
                    bool canActivate = true;

                    switch (powerups.Peek().name)
                    {
                        case "Ally":
                            if (p1Active)
                            {
                                canActivate = false;
                            }
                            else
                            {
                                p1Stored = false;
                                p1Active = true;
                                stats.AllyCount();
                            }

                            break;

                        case "Area Bomb":
                            if (p2Active)
                            {
                                canActivate = false;
                            }
                            else
                            {
                                p2Stored = false;
                                p2Active = true;
                                stats.AreaBombCount();
                            }

                            break;

                        case "Clear":
                            if (p3Active)
                            {
                                canActivate = false;
                            }
                            else
                            {
                                p3Stored = false;
                                p3Active = true;
                                stats.ClearCount();
                            }

                            break;

                        case "Damage Boost":
                            if (p4Active)
                            {
                                canActivate = false;
                            }
                            else
                            {
                                p4Stored = false;
                                p4Active = true;
                                stats.DamageBoostCount();
                            }

                            break;

                        case "Shield":
                            if (p5Active)
                            {
                                canActivate = false;
                            }
                            else
                            {
                                p5Stored = false;
                                p5Active = true;
                                stats.ShieldCount();
                            }

                            break;
                    }

                    if (canActivate)
                    {
                        Instantiate(powerups.Dequeue(), powerupPoint);

                        RemoveDisplayIcon();
                    }

                    ch.AllOff();
                }
            }
        }

        //Getters and setters for powerup states

        //public bool GetShield()
        //{
        //    return shieldState;
        //}

        //public void SetShield(bool newState)
        //{
        //    if (newState)
        //    {
        //        stats.ShieldCount();
        //    }
        //    shieldState = newState;
        //}

        //public bool GetAreaBomb()
        //{
        //    return areaBombState;
        //}

        //public void SetAreaBomb(bool newState)
        //{
        //    if (newState)
        //    {
        //        stats.AreaBombCount();
        //    }
        //    areaBombState = newState;
        //}

        //public bool GetClear()
        //{
        //    return clearState;
        //}

        //public void SetClear(bool newState)
        //{
        //    if (newState)
        //    {
        //        stats.ClearCount();
        //    }
        //    clearState = newState;
        //}

        //public bool GetAlly()
        //{
        //    return allyState;
        //}

        //public void SetAlly(bool newState)
        //{
        //    if (newState)
        //    {
        //        stats.AllyCount();
        //    }
        //    allyState = newState;
        //}
        //public bool GetDamageBoost()
        //{
        //    return damageBoostState;
        //}

        //public void SetDamageBoost(bool newState)
        //{
        //    if (newState)
        //    {
        //        stats.DamageBoostCount();
        //    }
        //    damageBoostState = newState;
        //}

        /// <summary>
        /// Trigger PowerupStatus to start tracking and find references.
        /// </summary>
        public void BeginTracking()
        {
            tracking = true;
            shipGO = gameObject.GetComponentInChildren<Player.ShipController>().gameObject;
            hand = shipGO.GetComponentInParent<Hand>();
            ch = hand.GetComponentInChildren<ControlsHighlighter>();
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
            newIcon.transform.localPosition = new Vector3(iconSpacing * (powerupIcons.Count - 1), 0, 0);

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
        /// Return status of queue capacity.
        /// </summary>
        /// <returns>State of whether there is room left in queue.</returns>
        public bool HasRoom()
        {
            return (powerups.Count < maxStoredPowerups);
        }
    }
}
