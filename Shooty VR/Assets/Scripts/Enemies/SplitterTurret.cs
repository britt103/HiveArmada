/// <summary>
/// Ryan Britton
/// britt103
/// #1849351
/// CPSC-340-01, CPSC-344-01
/// Group Project
/// 
/// This script handles the splitter turret that spawns 4 regular turret enemies when it is destroyed
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShootyVR.Enemies;

public class SplitterTurret : Enemy
{
    public GameObject bullet;
    public Transform spawn;
    GameObject player;
    GameObject Turret; /// set reference in inspector
    public Vector3 pos;
    public float fireRate, fireSpeed;
    private float fireNext;
    bool canFire;
    public Vector2 splitVel1; ///Set value in inspector
    public Vector2 splitVel2; ///Set value in inspector
    public Vector2 splitVel3; ///Set value in inspector
    public Vector2 splitVel4; ///Set value in inspector


    /// Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        pos = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
    }

    ///Constantly tracks the player position
    ///While shooting bullets using the formula below
	void Update()
    {
        pos = player.transform.position;
        transform.LookAt(pos);

        if (Time.time > fireNext)
        {
            fireNext = Time.time + fireRate;
            var shoot = Instantiate(bullet, spawn.position, spawn.rotation);
            shoot.GetComponent<Rigidbody>().velocity = shoot.transform.forward * fireSpeed;
        }
    }

    protected override IEnumerator HitFlash()
    {
        gameObject.GetComponent<Renderer>().material = flashColor;
        yield return new WaitForSeconds(.01f);

        if (health <= 0)
        {
            //Instantiate("Explosion.name", transform.position, transform.rotation);
            GameObject smlSplt1 = Instantiate(Turret, transform.position, transform.rotation);
            GameObject smlSplt2 = Instantiate(Turret, transform.position, transform.rotation);
            GameObject smlSplt3 = Instantiate(Turret, transform.position, transform.rotation);
            GameObject smlSplt4 = Instantiate(Turret, transform.position, transform.rotation);

            smlSplt1.GetComponent<Rigidbody>().velocity = splitVel1;
            smlSplt2.GetComponent<Rigidbody>().velocity = splitVel2;
            smlSplt3.GetComponent<Rigidbody>().velocity = splitVel3;
            smlSplt4.GetComponent<Rigidbody>().velocity = splitVel4;

            Destroy(gameObject);
        }
        gameObject.GetComponent<Renderer>().material = material;
    }

}