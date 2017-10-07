//Name: Chad Johnson
//Student ID: 1763718
//Email: johns428@mail.chapman.edu
//Course: CPSC 340-01, CPSC-344-01
//Assignment: Group Project
//Purpose: Handles collision with Player, instantiates powerups

using UnityEngine;
using ShootyVR;

public class PowerUp : MonoBehaviour {
    //prefab to use for instantiation
    public GameObject powerUpPrefab;

    /// <summary>
    /// handles collision with player
    /// </summary>
    /// <param name="other">object powerup collided with</param>
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            switch (powerUpPrefab.name)
            {
                case "Shield":
                    if (!other.gameObject.GetComponent<PowerUpStatus>().GetShield())
                    {
                        Instantiate(powerUpPrefab, other.gameObject.GetComponent<ShipController>().powerupPoint);
                        other.gameObject.GetComponent<PowerUpStatus>().SetShield(true);
                    }
                    break;

                case "Area Bomb":
                    if (!other.gameObject.GetComponent<PowerUpStatus>().GetSAreaBomb())
                    {
                        Instantiate(powerUpPrefab, other.gameObject.GetComponent<ShipController>().powerupPoint);
                        other.gameObject.GetComponent<PowerUpStatus>().SetAreaBomb(true);
                    }
                    break;

                case "Clear Bomb":
                    if (!other.gameObject.GetComponent<PowerUpStatus>().GetClear())
                    {
                        Instantiate(powerUpPrefab, other.gameObject.GetComponent<ShipController>().powerupPoint);
                        other.gameObject.GetComponent<PowerUpStatus>().SetClear(true);
                    }
                    break;

                case "Ally":
                    if (!other.gameObject.GetComponent<PowerUpStatus>().GetAlly())
                    {
                        Instantiate(powerUpPrefab, other.gameObject.GetComponent<ShipController>().powerupPoint);
                        other.gameObject.GetComponent<PowerUpStatus>().SetAlly(true);
                    }
                    break;
            }
        }
    }
}
