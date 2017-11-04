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
        //public GameObject fxAlly;
        private PowerUpStatus status;
        private float lifeTime = 10.0f;

        private void Start()
        {
            status = GameObject.Find("Player").GetComponent<PowerUpStatus>();
            Destroy(gameObject, lifeTime);
        }

        /// <summary>
        /// handles collision with player
        /// </summary>
        /// <param name="other">object powerup collided with</param>
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                switch (powerUpPrefab.name)
                {
                    case "Shield":
                        if (!status.GetShield())
                        {
                            Instantiate(powerUpPrefab, other.gameObject.transform.Find("Model").transform);
                            status.SetShield(true);
                            Destroy(gameObject);
                        }
                        break;

                    case "Area Bomb":
                        if (!status.GetAreaBomb() && !status.GetClear())
                        {
                            Instantiate(powerUpPrefab, other.gameObject.transform.Find("BombPoint").transform);
                            status.SetAreaBomb(true);
                            Destroy(gameObject);

                        }
                        break;

                    case "Clear":
                        if (!status.GetClear() && !status.GetAreaBomb())
                        {
                            Instantiate(powerUpPrefab, other.gameObject.transform.Find("BombPoint").transform);
                            status.SetClear(true);
                            Destroy(gameObject);
                        }
                        break;

                    case "Ally":
                        if (!status.GetAlly())
                        {
                            
                            Instantiate(powerUpPrefab, other.gameObject.transform.Find("Model").transform);
                            //Instantiate(fxAlly, other.gameObject.transform.Find("Ally").transform);
                            status.SetAlly(true);
                            Destroy(gameObject);
                        }
                        break;
                    case "Damage Boost":
                        if (!status.GetDamageBoost())
                        {
                            Instantiate(powerUpPrefab, other.gameObject.transform.Find("Model").transform);
                            status.SetDamageBoost(true);
                            Destroy(gameObject);
                        }
                        break;
                }
            }
        }
    }

}