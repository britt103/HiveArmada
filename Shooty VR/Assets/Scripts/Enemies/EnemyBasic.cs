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
            health = maxHealth;
            material = gameObject.GetComponent<Renderer>().material;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void Hit(int damage)
        {
            health -= damage;
            StartCoroutine(HitFlash());
        }

        protected override IEnumerator HitFlash()
        {
            gameObject.GetComponent<Renderer>().material = flashColor;
            yield return new WaitForSeconds(.01f);

            if (health <= 0)
            {
                GameObject.Find("GameManager").GetComponent<GameManager>().kills++;

                if (GameObject.Find("GameManager").GetComponent<GameManager>().kills >= 10)
                {
                    GameObject.Find("GameManager").GetComponent<GameManager>().gameOver();
                }
                health = maxHealth;
                gameObject.SetActive(false);
            }
            gameObject.GetComponent<Renderer>().material = material;
        }
    }
}
