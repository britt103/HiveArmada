//Name: Chad Johnson
//Student ID: 1763718
//Email: johns428@mail.chapman.edu
//Course: CPSC 340-01, CPSC-344-01
//Assignment: Group Project
//Purpose: Script ally powerup movement, behavior

using System.Collections;
using UnityEngine;

public class AllyStatic : MonoBehaviour
{
    //distance between ally and player ship
    public Vector3 localPosition;
    public float timeLimit;

    public GameObject bullet;
    //public Transform bulletSpawn;
    public float bulletSpeed;
    public float firerate;
    private bool canFire = true;

    // Use this for initialization
    void Start()
    {
        transform.localPosition = localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        timeLimit -= Time.deltaTime;
        if (timeLimit < 0.0F)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PowerUpStatus>().ally = false;
            Destroy(gameObject);
        }

        Transform target = nearestEnemy();
        transform.LookAt(target);

        if (canFire)
        {
            StartCoroutine(Fire(target.position));
        }
    }

    //http://answers.unity3d.com/questions/496463/find-nearest-object.html
    //https://docs.unity3d.com/ScriptReference/Vector3-sqrMagnitude.html

    //determine nearest enemy to player ship, return its transform
    private Transform nearestEnemy()
    {
        Vector3 positionDifference;
        float distance;
        float enemyLocalZ;
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            positionDifference = enemy.transform.position - transform.parent.transform.position;
            //faster than non-squared magnitude
            distance = positionDifference.sqrMagnitude;
            enemyLocalZ = transform.parent.transform.InverseTransformPoint(enemy.transform.position).z;
            //for static, want enemy to be in front of ally
            if (distance < shortestDistance && enemyLocalZ > localPosition.z)
            {
                shortestDistance = distance;
                nearestEnemy = enemy;
            }
        }
        return nearestEnemy.transform;
    }

    private IEnumerator Fire(Vector3 target)
    {
        canFire = false;
        var laser = Instantiate(bullet, transform.position, transform.rotation);

        laser.transform.LookAt(target);
        laser.GetComponent<Rigidbody>().velocity = laser.transform.forward * bulletSpeed;

        yield return new WaitForSeconds(1.0f / firerate);
        canFire = true;
    }
}