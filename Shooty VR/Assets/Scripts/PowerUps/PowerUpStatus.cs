//Name: Chad Johnson
//Student ID: 1763718
//Email: johns428@mail.chapman.edu
//Course: CPSC 340-01, CPSC-344-01
//Assignment: Group Project
//Purpose: Tracks powerups in use

using UnityEngine;
using System.Collections.Generic;

namespace Hive.Armada
{
    public class PowerUpStatus : MonoBehaviour
    {
        public bool shieldStored = false;
        public bool areaBombStored = false;
        public bool clearStored = false;
        public bool allyStored = false;
        public bool damageBoostStored = false;

        public bool shieldActive = false;
        public bool areaBombActive = false;
        public bool clearActive = false;
        public bool allyActive = false;
        public bool damageBoostActive = false;

        private Queue<GameObject> powerups = new Queue<GameObject>();
        private Queue<GameObject> powerupIcons = new Queue<GameObject>();
        public int maxStoredPowerups = 3;

        public float iconSpacing = 1f;
        public float alphaDelta = 30f;

        private GameObject shipGO;
        private Transform powerupPoint;
        private Transform iconPoint;

        private PlayerStats stats;

        private Valve.VR.InteractionSystem.Hand hand;

        public bool tracking = false;

        // Use this for initialization
        void Start()
        {
            stats = FindObjectOfType<PlayerStats>();
        }

        //// Update is called once per frame
        void Update()
        {
            //Debug.Log(tracking);

            if (tracking)
            {
                if (hand.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad) && powerups.Count > 0)
                {
                    GameObject powerup = powerups.Dequeue();
                    bool canActivate = true;

                    switch (powerup.name)
                    {
                        case "Ally":
                            if (allyActive)
                            {
                                canActivate = false;
                            }
                            else
                            {
                                allyStored = false;
                                allyActive = true;
                                stats.P1Used();
                            }

                            break;

                        case "Area Bomb":
                            if (areaBombActive)
                            {
                                canActivate = false;
                            }
                            else
                            {
                                areaBombStored = false;
                                areaBombActive = true;
                                stats.P2Used();
                            }

                            break;

                        case "Clear":
                            if (clearActive)
                            {
                                canActivate = false;
                            }
                            else
                            {
                                clearStored = false;
                                clearActive = true;
                                stats.P3Used();
                            }

                            break;

                        case "Damage Boost":
                            if (damageBoostActive)
                            {
                                canActivate = false;
                            }
                            else
                            {
                                damageBoostStored = false;
                                damageBoostActive = true;
                                stats.P4Used();
                            }

                            break;

                        case "Shield":
                            if (shieldActive)
                            {
                                canActivate = false;
                            }
                            else
                            {
                                shieldStored = false;
                                shieldActive = true;
                                stats.P5Used();
                            }

                            break;
                    }

                    if (canActivate)
                    {
                        Instantiate(powerup, powerupPoint);

                        RemoveDisplayIcon();
                    }
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
        /// Trigger status to start tracking and find necessary gameobjects and transforms
        /// </summary>
        public void BeginTracking()
        {
            tracking = true;

            shipGO = gameObject.GetComponentInChildren<Player.ShipController>().gameObject;
            hand = shipGO.GetComponentInParent<Valve.VR.InteractionSystem.Hand>();
            powerupPoint = shipGO.transform.Find("Powerup Point");
            iconPoint = shipGO.transform.Find("Powerup Icon Point");
        }

        /// <summary>
        /// Add powerup to queues
        /// </summary>
        /// <param name="powerupPrefab">gameobject to powerup</param>
        /// <param name="powerupIconPrefab">gameobject of powerup icon</param>
        public void StorePowerup(GameObject powerupPrefab, GameObject powerupIconPrefab)
        {
            powerups.Enqueue(powerupPrefab);

            GameObject newIcon = Instantiate(powerupIconPrefab, iconPoint);
            powerupIcons.Enqueue(newIcon);
            UpdateDisplayIcon(newIcon);
        }

        /// <summary>
        /// Adjust attributes of newly added icon based on queue count
        /// </summary>
        /// <param name="newIcon"></param>
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
        /// Removce icon, shift remaining icons
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
        /// Return status of queue capacity
        /// </summary>
        /// <returns>bool: True if there is room left in queue</returns>
        public bool HasRoom()
        {
            return (powerups.Count < maxStoredPowerups);
        }
    }
}
