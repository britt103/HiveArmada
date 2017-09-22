using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Miguel Gotao
/// gotao100@mail.chapman.edu
/// #2264941
/// CPSC-340-01, CPSC-344-01
/// Group Project
/// 
/// This script handles player health management
/// </summary>
public class CalcHealth : MonoBehaviour
{

    public int health;
    public bool isAlive;
    public bool isHit;
    //public Material[] color;

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

    //function called by enemy bullets;
    //deducts health and starts the coroutine for flashing -not in this build-
    //and destroying the gameobject
    //currently restarts the level because ALPHA
    public void Hit(int damage)
    {
        health -= damage;
        StartCoroutine(onHit());
    }

    IEnumerator onHit()
    {
        //gameObject.GetComponent<Renderer>().material = color[1];
        yield return new WaitForSeconds(.01f);

        if (health <= 0)
        {
            Destroy(gameObject);
            UnityEngine.SceneManagement.SceneManager.LoadScene("Test01");
        }

        isHit = false;
        //gameObject.GetComponent<Renderer>().material = color[0];
    }
}
