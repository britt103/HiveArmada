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
        public GameObject powerupIconPrefab;
        public GameObject fxAwake;
        private PowerUpStatus status;
        //private Transform head;
        public float lifeTime = 20.0f;

        private void Start()
        {
            Instantiate(fxAwake, transform.position, transform.localRotation);
            status = FindObjectOfType<PowerUpStatus>();
            //head = GameObject.Find("Player").transform.Find("SteamVRObjects").transform.Find("FollowHead").transform;
            Destroy(gameObject, lifeTime);
        }

        private void Update()
        {
            //gameObject.transform.LookAt(GameObject.Find("FollowHead").transform);
        }

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
                        if (!status.shieldStored)
                        {
                            status.StorePowerup(powerupPrefab, powerupIconPrefab);
                            status.shieldStored = true;
                            Destroy(gameObject);
                        }
                        break;

                    case "Area Bomb":
                        if (!status.areaBombStored)
                        {
                            status.StorePowerup(powerupPrefab, powerupIconPrefab);
                            status.areaBombStored = true;
                            Destroy(gameObject);

                        }
                        break;

                    case "Clear":
                        if (!status.clearStored)
                        {
                            status.StorePowerup(powerupPrefab, powerupIconPrefab);
                            status.clearStored = true;
                            Destroy(gameObject);
                        }
                        break;

                    case "Ally":
                        if (!status.allyStored)
                        {
                            status.StorePowerup(powerupPrefab, powerupIconPrefab);
                            status.allyStored = true;
                            Destroy(gameObject);
                        }
                        break;
                    case "Damage Boost":
                        if (!status.damageBoostStored)
                        {
                            status.StorePowerup(powerupPrefab, powerupIconPrefab);
                            status.damageBoostStored = true;
                            Destroy(gameObject);
                        }
                        break;
                }
            }
        }
    }
}