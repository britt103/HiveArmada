using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// /// Miguel Gotao
/// gotao100@mail.chapman.edu
/// #2264941
/// CPSC-340-01, CPSC-344-01
/// Group Project
/// 
/// This script handles the basic turret logic shooting straight at players
/// </summary>
public class Turret : MonoBehaviour {

    public GameObject bullet;
    public Transform spawn;
    GameObject player;
    public Vector3 pos;
    public float fireRate, fireSpeed;
    private float fireNext;
    bool canFire;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        pos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
	}
	
	//Constantly tracks the player position
    //While shooting bullets using the formula below
	void Update () {
        pos = player.transform.position;
        transform.LookAt(pos);

        if (Time.time > fireNext)
        {
            fireNext = Time.time + fireRate;
            var shoot = Instantiate(bullet, spawn.position, spawn.rotation);
            shoot.GetComponent<Rigidbody>().velocity = shoot.transform.forward * fireSpeed;
        }
	}
}
