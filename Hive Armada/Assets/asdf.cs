using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class asdf : MonoBehaviour
{
    private GameObject target;

    private Transform targetTransform;
	// Use this for initialization
	void Start () {
		target = GameObject.Find("Footage Camera Target");
	    targetTransform = target.transform;
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt(targetTransform);
	}
}
