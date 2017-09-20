using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MLaserGun : MonoBehaviour
{

    public GameObject projectile;
    public Transform spawnPoint;
    public Vector3 pos;
    public float fireRate;
    public float fireSpeed;
    public float radius;
    private bool canShoot = true;

    private VRTK.Examples.Gun gunScript;

    // Use this for initialization
    void Start()
    {
        gunScript = gameObject.GetComponentInParent<VRTK.Examples.Gun>();
    }

    // Update is called once per frame
    void Update()
    {

        if (gunScript.isTriggerPressed && canShoot)
        {
            Clicked();
        }
    }

    public void Clicked()
    {
        RaycastHit hit;
        var enemyMask = LayerMask.GetMask("Enemy");
        var roomMask = LayerMask.GetMask("Room");
        if (Physics.SphereCast(transform.position, radius, transform.forward, out hit, 200.0f, enemyMask))
        {
            StartCoroutine(Fire(hit.point));
        }
        else if (Physics.Raycast(transform.position, transform.forward, out hit, 200.0f, roomMask))
        {
            StartCoroutine(Fire(hit.point));
        }
    }

    private IEnumerator Fire(Vector3 target)
    {
        canShoot = false;
        var laser = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);

        laser.transform.LookAt(target);
        laser.GetComponent<Rigidbody>().velocity = laser.transform.forward * fireSpeed;
        Destroy(laser, 6.0f);

        yield return new WaitForSeconds(1.0f / gunScript.firerate);
        canShoot = true;
    }
}
