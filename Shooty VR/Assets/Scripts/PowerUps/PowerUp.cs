//Name: Chad Johnson
//Student ID: 1763718
//Email: johns428@mail.chapman.edu
//Course: CPSC 340-01, CPSC-344-01
//Assignment: Group Project
//Purpose: Handles collision with Player, instantiates powerups

using UnityEngine; 
using Valve.VR.InteractionSystem;

namespace GameName
{
    public class PowerUp : MonoBehaviour
    {
        //prefab to use for instantiation
        public GameObject powerUpPrefab;
        private PowerUpStatus status;

        private void Start()
        {
            status = GameObject.Find("Player").GetComponent<PowerUpStatus>();
        }

        /// <summary>
        /// handles collision with player
        /// </summary>
        /// <param name="other">object powerup collided with</param>
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                switch (powerUpPrefab.name)
                {
                    case "Shield":
                        if (!status.GetShield())
                        {
                            Instantiate(powerUpPrefab, other.gameObject.transform);
                            status.SetShield(true);
                        }
                        break;

                    case "Area Bomb":
                        if (!status.GetSAreaBomb())
                        {
                            Instantiate(powerUpPrefab, other.gameObject.transform);
                            status.SetAreaBomb(true);

                        }
                        break;

                    case "Clear":
                        if (!status.GetClear())
                        {
                            Instantiate(powerUpPrefab, other.gameObject.transform);
                            status.SetClear(true);
                        }
                        break;

                    case "Ally":
                        if (!status.GetAlly())
                        {
                            Instantiate(powerUpPrefab, other.gameObject.transform);
                            status.SetAlly(true);
                        }
                        break;
                }
                Destroy(gameObject);
            }
        }
    }

}