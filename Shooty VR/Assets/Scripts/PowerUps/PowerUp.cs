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
        public GameObject powerupPrefab;
        public GameObject powerupIconPrehab;
        public GameObject fxAwake;
        private PowerUpStatus status;
        //private Transform head;
        public float lifeTime = 20.0f;

        private void Start()
        {
            Instantiate(fxAwake, transform.position, transform.rotation);
            status = FindObjectOfType<PowerUpStatus>();
            //head = GameObject.Find("Player").transform.Find("SteamVRObjects").transform.Find("FollowHead").transform;
            Destroy(gameObject, lifeTime);
        }

        //private void Update()
        //{
        //    gameObject.transform.LookAt(head);
        //}

        /// <summary>
        /// handles collision with player
        /// </summary>
        /// <param name="other">object powerup collided with</param>
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player" && status.HasRoom())
            {
                switch (powerupPrefab.name)
                {
                    case "Shield":
                        if (!status.GetShield())
                        {
                            status.StorePowerup(powerupPrefab, powerupIconPrehab);
                            status.SetShield(true);
                            Destroy(gameObject);
                        }
                        break;

                    case "Area Bomb":
                        if (!status.GetAreaBomb())
                        {
                            status.StorePowerup(powerupPrefab, powerupIconPrehab);
                            status.SetAreaBomb(true);
                            Destroy(gameObject);

                        }
                        break;

                    case "Clear":
                        if (!status.GetClear())
                        {
                            status.StorePowerup(powerupPrefab, powerupIconPrehab);
                            status.SetClear(true);
                            Destroy(gameObject);
                        }
                        break;

                    case "Ally":
                        if (!status.GetAlly())
                        {
                            status.StorePowerup(powerupPrefab, powerupIconPrehab);
                            status.SetAlly(true);
                            Destroy(gameObject);
                        }
                        break;
                    case "Damage Boost":
                        if (!status.GetDamageBoost())
                        {
                            status.StorePowerup(powerupPrefab, powerupIconPrehab);
                            status.SetDamageBoost(true);
                            Destroy(gameObject);
                        }
                        break;
                }
            }
        }
    }
}