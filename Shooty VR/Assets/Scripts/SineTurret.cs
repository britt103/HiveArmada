using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Name: Miguel Luis Gotao
//Student ID: 2264941
//Email: gotao100@mail.chapman.edu
//Course: CPSC340-01
//Game Development Project 01

/// <summary>
/// Script that enables basic turret behavior to make
/// the turret shoot in a wavelike sine pattern.
/// </summary> 

public class SineTurret : MonoBehaviour
{
    public GameObject bullet;
    GameObject player;
    public Transform spawn;
    Vector3 pos;
    public float fireRate, fireSpeed;
    private float fireNext;
    bool canFire;
    public bool reverse;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");    //finds the player object and
        pos = player.transform.position;                        //its corresponding position
        transform.LookAt(pos);
    }

    // Update is called once per frame
    void Update()
    {
        //if(reverse) gameObject.transform.Rotate(Vector3.down * -Mathf.Sin(Time.time));    //reverse added as some turrets shot offscreen
        //{
            gameObject.transform.Rotate(Vector3.up * Mathf.Sin(Time.time));           //rotates the turret in a sine pattern
            gameObject.transform.Rotate(Mathf.Lerp(-1, 1, 0), 0, Mathf.Lerp(-1, 1, 0));
        //}
        if (Time.time > fireNext)                                                           //while shooting bullets at the angle
        {                                                                                   //of rotation.
            fireNext = Time.time + fireRate;
            var shoot = Instantiate(bullet, spawn.position, spawn.rotation);
            shoot.GetComponent<Rigidbody>().velocity = shoot.transform.forward * fireSpeed;
        }
    }
}