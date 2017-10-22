using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : Hive.Armada.Enemies.Enemy {

    private Transform target;
    public float speed;
    public int damage;

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

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //target = other.transform;
            other.gameObject.GetComponent<Hive.Armada.Player.PlayerHealth>().Hit(damage);
            Kill();
            Destroy(gameObject);
        }
    }
}
