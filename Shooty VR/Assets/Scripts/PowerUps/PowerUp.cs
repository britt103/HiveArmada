//Name: Chad Johnson
//Student ID: 1763718
//Email: johns428@mail.chapman.edu
//Course: CPSC 340-01, CPSC-344-01
//Assignment: Group Project
//Purpose: Handles collision with Player, instantiates powerups

using UnityEngine; 
using Valve.VR.InteractionSystem;

namespace Hive.Armada
{
    public class PowerUp : MonoBehaviour
    {
        //prefab to use for instantiation
        public GameObject powerUpPrefab;
        public GameObject fxAwake;
        private PowerUpStatus status;
        public float lifeTime = 20.0f;

        private void Start()
        {
            Instantiate(fxAwake, transform.position, transform.rotation);
            status = GameObject.Find("Player").GetComponent<PowerUpStatus>();
            Destroy(gameObject, lifeTime);
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
                            status.StorePowerup(powerUpPrefab);
                            status.SetShield(true);
                            Destroy(gameObject);
                        }
                        break;

                    case "Area Bomb":
                        if (!status.GetAreaBomb())
                        {
                            status.StorePowerup(powerUpPrefab);
                            status.SetAreaBomb(true);
                            Destroy(gameObject);

                        }
                        break;

                    case "Clear":
                        if (!status.GetClear())
                        {
                            status.StorePowerup(powerUpPrefab);
                            status.SetClear(true);
                            Destroy(gameObject);
                        }
                        break;

                    case "Ally":
                        if (!status.GetAlly())
                        {
                            status.StorePowerup(powerUpPrefab);
                            status.SetAlly(true);
                            Destroy(gameObject);
                        }
                        break;
                    case "Damage Boost":
                        if (!status.GetDamageBoost())
                        {
                            status.StorePowerup(powerUpPrefab);
                            status.SetDamageBoost(true);
                            Destroy(gameObject);
                        }
                        break;
                }
            }
        }
    }
}