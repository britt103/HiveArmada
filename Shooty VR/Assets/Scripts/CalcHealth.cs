using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalcHealth : MonoBehaviour
{

    public int healthVal;
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
        if (isHit) StartCoroutine(onHit());
        if (healthVal <= 0) isAlive = false;
        if (!isAlive) Destroy(this.gameObject);
    }

    IEnumerator onHit()
    {
        gameObject.GetComponent<Renderer>().material = color[1];
        yield return new WaitForSeconds(.01f);
        isHit = false;
        gameObject.GetComponent<Renderer>().material = color[0];
    }
}
