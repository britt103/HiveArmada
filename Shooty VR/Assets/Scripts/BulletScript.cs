using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

	//// Use this for initialization
	//void Start () {
		
	//}
	
	//// Update is called once per frame
	//void Update () {
		
	//}

    //void OnCollisionEnter(Collision other)
    //{
    //    if (other.gameObject.tag != "bullet")
    //    {
    //        Debug.Log(other.gameObject.name);
    //        Destroy(this, 0.05f);
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Destructible>() != null)
        {
            Destroy(other);
            Destroy(this.gameObject);
        }
    }
}
