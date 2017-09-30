using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameName;

public class LaserGun : MonoBehaviour
{
    private VRTK.Examples.Gun gunScript;
    private bool canShoot = true;

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
        if (Physics.Raycast(transform.position, transform.forward, out hit, 200.0f))
        {
            StartCoroutine(Fire(hit.point));
        }
    }

    private IEnumerator Fire(Vector3 target)
    {
        canShoot = false;

        Vector3 newPosition = new Vector3(gunScript.laserSpawn.position.x + Random.Range(-0.01f, 0.01f),
            gunScript.laserSpawn.position.y + Random.Range(-0.01f, 0.01f),
            gunScript.laserSpawn.position.z + Random.Range(-0.01f, 0.01f));

        var laser = Instantiate(gunScript.laserPrefab, newPosition, Quaternion.identity);
        laser.transform.LookAt(target);
        laser.GetComponent<Rigidbody>().velocity = laser.transform.forward * gunScript.laserSpeed;
        Destroy(laser, 6.0f);

        yield return new WaitForSeconds(1.0f / gunScript.firerate);
        canShoot = true;
    }
}
