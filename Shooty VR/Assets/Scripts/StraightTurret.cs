using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Name: Miguel Luis Gotao
//Student ID: 2264941
//Email: gotao100@mail.chapman.edu
//Course: CPSC340-01
//Game Development Project 01

/// <summary>
/// Script enabling basic turret behavior that
/// shoots directly at the player character.
/// </summary>

public class StraightTurret : MonoBehaviour {

    public GameObject bullet;
    public Transform spawn;
    GameObject player;
    public Vector3 pos;
    public float fireRate, fireSpeed;
    private float fireNext;
    bool canFire;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");        //finds player and stores it's position
        pos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
        pos = player.transform.position;        //tracks the player position
        transform.LookAt(pos);                  //and makes the transform look at said position

        if (Time.time > fireNext)
        {                                                                                       //Basic firerate calculation that determines
            fireNext = Time.time + fireRate;                                                    //how many projectiles shoot out of the turret
            var shoot = Instantiate(bullet, spawn.position, spawn.rotation);                    //within a certain duration. The turret then
            shoot.GetComponent<Rigidbody>().velocity = shoot.transform.forward * fireSpeed;     //instantiates a bullet and shoots it forward
        }                                                                                       //in the direction of the player.
	}
}
