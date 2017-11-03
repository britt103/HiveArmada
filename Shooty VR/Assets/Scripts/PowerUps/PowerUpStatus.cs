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
        private bool shieldState = false;
        private bool areaBombState = false;
        private bool clearState = false;
        private bool allyState = false;
        private bool damageBoostState = false;
        private Queue<GameObject> powerups = new Queue<GameObject>();

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

                    switch (powerup.name)
                    {
                        case "Shield":
                            shieldState = false;
                            break;

                        case "Area Bomb":
                            areaBombState = false;
                            break;

                        case "Clear":
                            clearState = false;
                            break;

                        case "Ally":
                            allyState = false;
                            break;

                        case "Damage Boost":
                            damageBoostState = false;
                            break;
                    }

                    Instantiate(powerup, gameObject.GetComponentInChildren<Player.ShipController>().transform.Find("Thrusters").transform);
                }
            }
        }

        public bool GetShield()
        {
            return shieldState;
        }

        public void SetShield(bool newState)
        {
            if (newState)
            {
                stats.ShieldCount();
            }
            shieldState = newState;
        }

        public bool GetAreaBomb()
        {
            return areaBombState;
        }

        public void SetAreaBomb(bool newState)
        {
            if (newState)
            {
                stats.AreaBombCount();
            }
            areaBombState = newState;
        }

        public bool GetClear()
        {
            return clearState;
        }

        public void SetClear(bool newState)
        {
            if (newState)
            {
                stats.ClearCount();
            }
            clearState = newState;
        }

        public bool GetAlly()
        {
            return allyState;
        }

        public void SetAlly(bool newState)
        {
            if (newState)
            {
                stats.AllyCount();
            }
            allyState = newState;
        }
        public bool GetDamageBoost()
        {
            return damageBoostState;
        }

        public void SetDamageBoost(bool newState)
        {
            if (newState)
            {
                stats.DamageBoostCount();
            }
            damageBoostState = newState;
        }

        public void BeginTracking()
        {
            tracking = true;
            hand = FindObjectOfType<Player.ShipController>().GetComponentInParent<Valve.VR.InteractionSystem.Hand>();
        }

        public void StorePowerup(GameObject powerupPrefab)
        {
            powerups.Enqueue(powerupPrefab);
        }
    }
}
