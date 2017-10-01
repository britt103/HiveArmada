using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShootyVR.Enemies;
/// <summary>
/// /// Ryan Britton
/// britt103
/// #1849351
/// CPSC-340-01, CPSC-344-01
/// Group Project
/// 
/// This script handles the logic of a turret moving between two points
/// </summary>

public class MovingTurret : Enemy
{
    public float movingSpeed;
    public float xMax;
    public float xMin;
    public float yMax;
    public float yMin;


    private Vector3 posA;
    private Vector3 posB;

    private float xStart;
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
        SetPositions();
 
        player = GameObject.FindGameObjectWithTag("Player");
        pos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
    }
    void SetPositions()
    {
        xStart = transform.position.x + xMin;
        xEnd = transform.position.x +  xMax;

        posA = new Vector3(xStart, yMin, 0);
        posB = new Vector3(xEnd, yMax, 0);
    }
    private void Update()
    {
        transform.position = Vector3.Lerp(posA, posB, Mathf.PingPong(Time.time * movingSpeed, 1.0f));
    }
}
