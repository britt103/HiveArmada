﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///using ShootyVR.Enemies;
/// <summary>
/// /// Ryan Britton
/// britt103
/// #1849351
/// CPSC-340-01, CPSC-344-01
/// Group Project
/// 
/// This script handles the logic of a turret moving between two points
/// </summary>

public class MovingTurret : MonoBehaviour
{
    public float movingSpeed;
    public float xMax;    
    public float yMax;
    private Vector3 posA;
    private Vector3 posB;
    private bool perry = false;
    private float xEnd;
    public GameObject bullet;
    public Transform spawn;
    GameObject player;
    public Vector3 pos;
    public float fireRate, fireSpeed;
    private float fireNext;
    bool canFire;
    private float distance;


    // Use this for initialization
    void Start()
    {
        SetPosition(); 
        player = GameObject.FindGameObjectWithTag("Player");
        
    }
    void SetPosition()
    {
        pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        posA = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        posB = new Vector3(transform.position.x + xMax,transform.position.y + yMax, 0);
    }
    private void Update()
    {
        pos = transform.position;
        float step = movingSpeed * Time.deltaTime;
        if (perry == false) {
            Debug.Log("I moved, senpai!");
            transform.position = Vector3.MoveTowards(pos, posB, step);
        }
        else
        {
            Debug.Log("Die senpai!");
            transform.position = Vector3.MoveTowards(pos, posA, step);
        }
        
        if (pos.Equals(posA))
        {
            perry = false;
        }
        else if (pos.Equals(posB))
        {
            perry = true;
        }

        transform.LookAt(pos);

        /*if (Time.time > fireNext)
        {
            fireNext = Time.time + fireRate;
            var shoot = Instantiate(bullet, spawn.position, spawn.rotation);
            shoot.GetComponent<Rigidbody>().velocity = shoot.transform.forward * fireSpeed;
        }*/
    }

}
