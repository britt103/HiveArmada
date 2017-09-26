//Name: Chad Johnson
//Student ID: 1763718
//Email: johns428@mail.chapman.edu
//Course: CPSC 340-01, CPSC-344-01
//Assignment: Group Project
//Purpose: Script shield powerup behavior

using UnityEngine;

public class Shield : MonoBehaviour {
    public float localAdjustmentX;
    public float localAdjustmentY;
    public float localAdjustmentZ;
    public float timeLimit;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        timeLimit -= Time.deltaTime;
        if(timeLimit < 0.0F)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PowerUpStatus>().shield = false;
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "bullet")
        {
            //Debug.Log("KEK");
            Destroy(other.gameObject);
        }
    }
}
