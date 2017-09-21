using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour {

	GameObject player;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other){
        if (other.tag == "Player")
        {
            //other.GetComponent<CalcHealth>().healthVal -= 20;
            //other.GetComponent<CalcHealth>().isHit = true;
            other.GetComponent<CalcHealth>().Hit(20);
        }
	}
}
