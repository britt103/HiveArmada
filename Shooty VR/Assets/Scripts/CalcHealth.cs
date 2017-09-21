using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalcHealth : MonoBehaviour
{

    public int health;
    public bool isAlive;
    public bool isHit;
    public Material[] color;

    // Use this for initialization
    void Start()
    {
        isAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        //if (isHit) BlowUp();
        //if (health <= 0) isAlive = false;
        //if (!isAlive) Destroy(this.gameObject);
    }

    public void Hit(int damage)
    {
        health -= damage;
        StartCoroutine(onHit());
    }

    void BlowUp()
    {
        StartCoroutine(onHit());
    }

    IEnumerator onHit()
    {
        gameObject.GetComponent<Renderer>().material = color[1];
        yield return new WaitForSeconds(.01f);

        if (health <= 0)
        {
            Destroy(gameObject);
        }

        isHit = false;
        gameObject.GetComponent<Renderer>().material = color[0];
    }
}
