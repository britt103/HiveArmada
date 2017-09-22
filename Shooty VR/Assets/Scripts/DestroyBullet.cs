using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// /// Miguel Gotao
/// gotao100@mail.chapman.edu
/// #2264941
/// CPSC-340-01, CPSC-344-01
/// Group Project
/// 
/// This script destroys the lifetime of bullets
/// </summary>
public class DestroyBullet : MonoBehaviour {

    public float lifetime;

	// Use this for initialization
	void Start () {
		
	}
	
	//On spawn, destroys the bullet after a set public lifetime value
	void Update () {
        Destroy(gameObject, lifetime);
	}
}
