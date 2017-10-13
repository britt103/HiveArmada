// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
// 
// [DESCRIPTION]
// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float speed = 90.0f;
    private float theta = 0.0f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        theta += speed;

        if (theta >= 360.0f)
        {
            theta -= 360.0f;
        }

        transform.Rotate(transform.up, speed*Time.deltaTime);
    }
}
