using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstancingTest : MonoBehaviour
{
    private float time = 0.0f;

    private float maxTime = 3.0f;

	// Use this for initialization
	void Start ()
	{
	    time = 0.0f;
	    maxTime = 3.0f;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    time += Time.deltaTime;

	    if (time >= maxTime)
	    {
	        Destroy(gameObject);
	    }
	}
}
