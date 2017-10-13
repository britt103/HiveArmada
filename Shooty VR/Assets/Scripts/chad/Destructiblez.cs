using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructiblez : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "bullet")
        {
            Destroy(this.gameObject);
        }
    }
}




//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Destructible : MonoBehaviour
//{
//    public Color color = Color.cyan;
//    private Vector3 position;

//    // Use this for initialization
//    void Start()
//    {
//        position = transform.position;
//        ResetColor();
//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }

//    private void ResetColor()
//    {
//        GetComponent<Renderer>().material.color = color;
//    }

//    public void GetHit()
//    {
//        StartCoroutine("BlowUp");

//        StartCoroutine("Respawn");
//    }

//    IEnumerator BlowUp()
//    {
//        GetComponent<Renderer>().material.color = Color.red;

//        yield return new WaitForSeconds(0.25f);

//        transform.position = new Vector3(0.0f, 0.0f, -10.0f);
//        ResetColor();
//    }

//    IEnumerator Respawn()
//    {
//        yield return new WaitForSeconds(5.0f);

//        transform.position = position;
//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.tag == "bullet")
//        {
//            Destroy(this.gameObject);
//        }
//    }
//}