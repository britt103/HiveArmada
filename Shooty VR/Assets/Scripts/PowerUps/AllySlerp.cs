//Name: Chad Johnson
//Student ID: 1763718
//Email: johns428@mail.chapman.edu
//Course: CPSC 340-01, CPSC-344-01
//Assignment: Group Project
//Purpose: Script ally powerup movement, behavior

//http://answers.unity3d.com/questions/496463/find-nearest-object.html
//https://docs.unity3d.com/ScriptReference/Vector3-sqrMagnitude.html
//http://answers.unity3d.com/questions/389713/detaliled-explanation-about-given-vector3slerp-exa.html
//http://answers.unity3d.com/questions/532062/raycast-to-determine-certain-game-objects.html

using UnityEngine;
using System.Collections;

namespace Hive.Armada
{
    public class AllySlerp : MonoBehaviour
    {
        public float distance;
        public float timeLimit;
        public float slerpTime = 1.0F;
        public float sphereCastRadius = 1.0F;
        public float sphereCastMaxDistance = 1.0F;
        public GameObject currentTarget = null;

        private float slerpTimer = 0.0F;
        private float slerpFraction;
        private bool targetAcquired;

        public GameObject bulletPrefab, fxSpawn;
        public float bulletSpeed;
        public float firerate;

        private bool canFire = true;

        // Use this for initialization
        void Start()
        {
            //in case no enemies are present on init
            Instantiate(fxSpawn, transform);
            transform.localPosition = new Vector3(0, distance, 0);
        }

        // Update is called once per frame
        void Update()
        {
            timeLimit -= Time.deltaTime;
            if (timeLimit < 0.0F)
            {
                GameObject.Find("Player").GetComponent<PowerUpStatus>().SetAlly(false);
                Destroy(gameObject);
            }

            Move();

            if (canFire && slerpFraction >= 1.0F && currentTarget != null)
            {
                StartCoroutine(Fire(currentTarget.transform.position));
            }
        }

        /// <summary>
        /// Determine transform of enemy nearest to player ship
        /// </summary>
        /// <returns>Transform of nearest enemy ship</returns>
        private Transform NearestEnemy()
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

            //couldn't find any enemies
            if (shortestDistance == Mathf.Infinity)
            {
                return null;
            }
            else
            {
                return nearestEnemy.transform;
            }
        }

        /// <summary>
        /// Controls movement and rotation; utilizes slerp
        /// </summary>
        private void Move()
        {
            //no current target
            if (currentTarget == null || currentTarget.activeSelf == false)
            {
                slerpTimer = 0.0F;

                //no enemies found
                if (NearestEnemy() == null)
                {
                    return;
                }
                else
                {
                    currentTarget = NearestEnemy().gameObject;
                }
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

                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, 1))
                //if(Physics.SphereCast(transform.position, sphereCastRadius, transform.forward, out hit, sphereCastMaxDistance))
                //if(Physics.SphereCast(transform.position, sphereCastRadius, transform.forward, out hit, sphereCastMaxDistance, LayerMask.GetMask("Player")))
                {
                    if (hit.collider.gameObject.CompareTag("Player") || hit.collider.gameObject.GetComponent<Shield>() != null)
                    {
                        slerpTimer = 0.0F;
                    }
                }
            }
        }

        /// <summary>
        /// Instantiates bullet according to firerate
        /// </summary>
        /// <param name="target">Enemy bullet is "aimed" at</param>
        /// <returns>IEnumerator for StartCoroutine in Update()</returns>
        private IEnumerator Fire(Vector3 target)
        {
            canFire = false;
            var bullet = Instantiate(bulletPrefab, transform.Find("BulletPoint").transform.position, transform.rotation);

            bullet.transform.LookAt(target);
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletSpeed;

            yield return new WaitForSeconds(1.0f / firerate);
            canFire = true;
        }
    }
}
