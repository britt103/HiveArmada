//Name: Chad Johnson
//Student ID: 1763718
//Email: johns428@mail.chapman.edu
//Course: CPSC 340-01, CPSC-344-01
//Assignment: Group Project
//Purpose: Script ally powerup movement, behavior

using UnityEngine;
using System;
using System.Collections;

public class AllySlerp : MonoBehaviour
{
    //distance between ally and player ship
    public float distance;
    public float timeLimit;
    public float slerpTime = 1.0F;
    private float slerpTimer = 0.0F;
    private float slerpFraction;
    //public float slerpAngleTolerance = 45.0F;

    public GameObject currentTarget = null;

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

        move();

        if (canFire && slerpFraction >= 1.0F)
        {
            StartCoroutine(Fire(currentTarget.transform.position));
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

    //http://answers.unity3d.com/questions/389713/detaliled-explanation-about-given-vector3slerp-exa.html
    private void move()
    {
        //no current target
        if (currentTarget == null || currentTarget.activeSelf == false)
        {
            slerpTimer = 0.0F;
            currentTarget = nearestEnemy().gameObject;
        }

        Quaternion rotation = Quaternion.LookRotation((currentTarget.transform.position - transform.position).normalized);
        Vector3 enemyLocalPosition = transform.parent.transform.InverseTransformPoint(currentTarget.transform.position);
        Vector3 localTranslation = new Vector3(enemyLocalPosition.x, enemyLocalPosition.y, 0).normalized * distance;

        slerpFraction = slerpTimer / slerpTime;

        //is slerping
        if (slerpFraction < 1.0F)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, slerpFraction);
            transform.localPosition = Vector3.Slerp(transform.localPosition, localTranslation, slerpFraction);

            slerpTimer += Time.deltaTime;
        }
        //not slerping
        else
        {
            transform.rotation = rotation;
            //transform.localPosition = localTranslation;

            //float currentTargetTheta = (float)(Math.Acos(enemyLocalPosition.normalized.x) * 180/Math.PI);
            //if(enemyLocalPosition.y < 0)
            //{
            //    currentTargetTheta = -currentTargetTheta;
            //}
            //float theta = (float)(Math.Acos(transform.localPosition.normalized.x) * 180/Math.PI);
            //if(transform.localPosition.y < 0)
            //{
            //    theta = -theta;
            //}
            //Debug.Log(currentTargetTheta - theta);

            //if(Math.Abs(currentTargetTheta - theta) > slerpAngleTolerance)
            //{
            //    slerpTimer = 0.0F;
            //}

            //http://answers.unity3d.com/questions/532062/raycast-to-determine-certain-game-objects.html
            RaycastHit hit;
            if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 1))
            {
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    slerpTimer = 0.0F;
                }
            }
        }
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
