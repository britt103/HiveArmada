//Name: Chad Johnson
//Student ID: 1763718
//Email: johns428@mail.chapman.edu
//Course: CPSC 340-01, CPSC-344-01
//Assignment: Group Project
//Purpose: Script area bomb powerup behavior

//http://www.sdkboy.com/2016/11/throwing-objects-daydream-vr/

using UnityEngine;
using ShootyVR;
using System.Collections;

public class AreaBomb : MonoBehaviour {
    public float timeLimit;
    public float radius;
    private bool released;
    private Vector3 velocity;
    private Vector3 lastPosition;
    public bool isThrowable = true;
    public float velocityMultiplier = 10.0F;
    public int numAveragedVelocities = 10;
    private Queue velocities;
    public bool isSetVelocity;
    public float setVelocity;

    private GameObject playerShip;

    // Use this for initialization
    void Start () {
        playerShip = GameObject.FindGameObjectWithTag("Player");
        released = false;
        velocities = new Queue(numAveragedVelocities);
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
                        objectCollider.gameObject.GetComponent<ShootyVR.Enemies.EnemyBasic>().Hit(100);
                    }
                }
                Destroy(gameObject);
            }
        }

        velocity = transform.position - lastPosition;
        lastPosition = transform.position;

        velocities.Enqueue(velocity);
        if(velocities.Count > numAveragedVelocities)
        {
            velocities.Dequeue();
        }

        if (!released && playerShip.GetComponent<ShipController>().isTriggerPressed)
        {
            playerShip.GetComponent<PowerUpStatus>().areaBomb = false;
            gameObject.transform.parent = null;

            if (isThrowable)
            {
                Vector3 averagedVelocity = Vector3.zero;
                foreach(Vector3 velocity in velocities)
                {
                    averagedVelocity += velocity;
                }
                averagedVelocity /= velocities.Count;

                gameObject.GetComponent<Rigidbody>().isKinematic = false;

                if (!isSetVelocity)
                {
                    gameObject.GetComponent<Rigidbody>().AddForce(averagedVelocity * velocityMultiplier, ForceMode.Impulse);
                }
                else
                {
                    gameObject.GetComponent<Rigidbody>().AddForce(averagedVelocity.normalized * setVelocity, ForceMode.Impulse);
                }
                
            }

            released = true;
        }
    }
}
