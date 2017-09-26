//Name: Chad Johnson
//Student ID: 1763718
//Email: johns428@mail.chapman.edu
//Course: CPSC 340-01, CPSC-344-01
//Assignment: Group Project
//Purpose: Script clear bomb powerup bahavior

using UnityEngine;
using ShootyVR;

/// <summary>
/// Destroys all enemies
/// </summary>
public class ClearBomb : MonoBehaviour
{
    private GameObject playerShip;

    // Use this for initialization
    void Start()
    {
        playerShip = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (playerShip.GetComponent<ShipController>().isTriggerPressed)
        {
            playerShip.GetComponent<PowerUpStatus>().clearBomb = false;
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                Destroy(enemy);
            }
            Destroy(gameObject);
        }
    }
}
