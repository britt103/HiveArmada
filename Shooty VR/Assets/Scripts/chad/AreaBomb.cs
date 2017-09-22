//Name: Chad Johnson
//Student ID: 1763718
//Email: johns428@mail.chapman.edu
//Course: CPSC 340-01, CPSC-344-01
//Assignment: Group Project
//Purpose: Script area bomb powerup behavior

using UnityEngine;
using ShootyVR;

public class AreaBomb : MonoBehaviour {
    public float timeLimit;
    public float radius;
    private bool released;

    private GameObject playerShip;

    // Use this for initialization
    void Start () {
        playerShip = GameObject.FindGameObjectWithTag("Player");
        released = false;
	}

    // Update is called once per frame
    void Update () {
        if (released)
        {
            timeLimit -= Time.deltaTime;
            if (timeLimit < 0.0F)
            {
                foreach (Collider objectCollider in Physics.OverlapSphere(transform.position, radius))
                {
                    if (objectCollider.gameObject.tag == "Enemy")
                    {

                        //objectCollider.gameObject.GetComponent<CalcHealth>().isHit = true;
                        //objectCollider.gameObject.GetComponent<CalcHealth>().healthVal -= 100;
                        objectCollider.gameObject.GetComponent<CalcHealth>().Hit(100);
                        //Destroy(objectCollider.gameObject);
                    }
                }
                Destroy(gameObject);
            }
        }

        if (!released && playerShip.GetComponent<ShipController>().isTriggerPressed)
        {
            playerShip.GetComponent<PowerUpStatus>().clearBomb = false;
            gameObject.transform.parent = null;
            released = true;
        }
    }
}
