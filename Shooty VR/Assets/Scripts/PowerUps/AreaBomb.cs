//Name: Chad Johnson
//Student ID: 1763718
//Email: johns428@mail.chapman.edu
//Course: CPSC 340-01, CPSC-344-01
//Assignment: Group Project
//Purpose: Script area bomb powerup behavior; bomb accelerates forward until detonation

using UnityEngine;
using ShootyVR;

public class AreaBomb : MonoBehaviour {
    public float timeLimit;
    public float radius;
    private bool released;
    public float acceleration;
    public float maxSpeed;

    private float currentSpeed;
    private GameObject playerShip;

    // Use this for initialization
    void Start () {
        playerShip = GameObject.FindGameObjectWithTag("Player");
        released = false;
	}

    // Update is called once per frame
    void Update () {
        //accelerating forward
        if (released)
        {
            currentSpeed += acceleration * Time.deltaTime;
            if(currentSpeed > maxSpeed)
            {
                currentSpeed = maxSpeed;
            }
            transform.Translate(transform.forward.normalized * currentSpeed);

            //time-based detonation
            timeLimit -= Time.deltaTime;
            if (timeLimit < 0.0F)
            {
                foreach (Collider objectCollider in Physics.OverlapSphere(transform.position, radius))
                {
                    if (objectCollider.gameObject.tag == "Enemy")
                    {
                        objectCollider.gameObject.GetComponent<ShootyVR.Enemies.EnemyBasic>().Hit(100);
                    }
                }
                Destroy(gameObject);
            }
        }

        //player presses "bomb"/"powerup" button
        if (!released && playerShip.GetComponent<ShipController>().isTriggerPressed)
        {
            playerShip.GetComponent<PowerUpStatus>().areaBomb = false;
            gameObject.transform.parent = null;

            released = true;
        }
    }
}
