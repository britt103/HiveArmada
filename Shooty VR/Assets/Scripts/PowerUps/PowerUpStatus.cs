//Name: Chad Johnson
//Student ID: 1763718
//Email: johns428@mail.chapman.edu
//Course: CPSC 340-01, CPSC-344-01
//Assignment: Group Project
//Purpose: Tracks powerups in use

using UnityEngine;

namespace ShootyVR
{
    public class PowerUpStatus : MonoBehaviour
    {
        private bool shieldState = false;
        private bool areaBombState = false;
        private bool clearState = false;
        private bool allyState = false;

        private PlayerStats stats;

        // Use this for initialization
        void Start()
        {
            stats = GameObject.Find("Player Stats").GetComponent<PlayerStats>();
        }

        // Update is called once per frame
        void Update()
        {

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

        public bool GetSAreaBomb()
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
    }
}
