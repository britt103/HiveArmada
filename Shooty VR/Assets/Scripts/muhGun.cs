using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class muhGun : MonoBehaviour {

    public GameObject bullet;
    public Transform spawn;
    GameObject player;
    public Vector3 pos;
    public float fireRate, fireSpeed;
    private float fireNext;
    bool canFire;

    private VRTK.Examples.Gun gunScript;

    // Use this for initialization
    void Start () {
        gunScript = gameObject.GetComponentInParent<VRTK.Examples.Gun>();
    }
	
	// Update is called once per frame
	void Update () {

        if (gunScript.isTriggerPressed && Time.time > fireNext)
        {
            fireNext = Time.time + fireRate;
            var shoot = Instantiate(bullet, spawn.position, spawn.rotation);
            shoot.GetComponent<Rigidbody>().velocity = shoot.transform.forward * fireSpeed;
        }
	}
}
