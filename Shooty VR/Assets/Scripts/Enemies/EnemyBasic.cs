// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
// 
// A basic enemy that does not move. It simply exists for the player to shoot.
// 

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShootyVR.Enemies
{
    public class EnemyBasic : Enemy
    {
        // Use this for initialization
        void Start()
        {

        }

        /// <summary>
        /// Initializes variables before the game starts.
        /// </summary>
        void Awake()
        {
            material = gameObject.GetComponent<Renderer>().material;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
