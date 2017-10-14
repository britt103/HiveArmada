using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Miguel Gotao
/// gotao100@mail.chapman.edu
/// #2264941
/// CPSC-340-01, CPSC-344-01
/// Group Project
/// 
/// This script handles basic enemy bullet behavior
/// </summary>
public class EnemyBullet : MonoBehaviour {

	GameObject player;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //When a bullet hits a player, it deducts the player health
	void OnTriggerEnter(Collider other){
        if (other.tag == "Player")
        {
            //other.GetComponent<CalcHealth>().healthVal -= 20;
            //other.GetComponent<CalcHealth>().isHit = true;
            other.GetComponent<CalcHealth>().Hit(1);
            Debug.Log("Hi!");
        }
	}
}
