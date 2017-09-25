//Name: Chad Johnson
//Student ID: 1763718
//Email: johns428@mail.chapman.edu
//Course: CPSC 340-01, CPSC-344-01
//Assignment: Group Project
//Purpose: Script ally powerup movement, behavior

using System.Collections;
using UnityEngine;

public class AllySlerp : MonoBehaviour
{
    //distance between ally and player ship
    public float distance;
    public float timeLimit;

    public GameObject bullet;
    //public Transform bulletSpawn;
    public float bulletSpeed;
    public float firerate;
    private bool canFire = true;

    // Use this for initialization
    void Start()
    {
        //in case no enemies are present on init
        transform.localPosition = new Vector3(0, distance, 0);
    }

    // Update is called once per frame
    void Update()
    {
        timeLimit -= Time.deltaTime;
        if(timeLimit < 0.0F)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PowerUpStatus>().ally = false;
            Destroy(gameObject);
        }

        Transform target = nearestEnemy();
        setLocalPosition(target);
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
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            positionDifference = enemy.transform.position - transform.parent.transform.position;
            //faster than non-squared magnitude
            distance = positionDifference.sqrMagnitude;
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestEnemy = enemy;
            }
        }
        return nearestEnemy.transform;
    }

    //set transform.localPosition based on position of nearest enemy in local space
    private void setLocalPosition(Transform enemy)
    {
        transform.localPosition = Vector3.zero;
        Vector3 enemyLocalPosition = transform.parent.transform.InverseTransformPoint(enemy.position);

        Vector3 direction = enemyLocalPosition - transform.localPosition;
        direction = new Vector3(direction.x, direction.y, 0).normalized * distance;

        transform.localPosition = direction;
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
