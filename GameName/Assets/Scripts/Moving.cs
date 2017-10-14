using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : MonoBehaviour {

    private Transform target;
    public float speed;

	// Use this for initialization
	void Start () {
        target = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
        //if (target != null)
        //{
            transform.LookAt(target);

            transform.Translate(Vector3.forward * (speed * Time.deltaTime));
        //}
        //else
        //{
        //    return;
        //}
	}

    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "Player")
    //    {
    //        target = other.transform;
    //    }
    //}
}
