//=============================================================================
//
// Ryan Britton
// 1849351
// britt103@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// Hover effect for enemies that rotates their bodies randomly.
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Hive.Armada.Enemies
{
    /// <summary>
    /// Hovering rotation
    /// </summary>
    public class HoverRotation : MonoBehaviour
    {
        private Vector3 targetAngle;
        private Vector3 startAngle;
        private Quaternion newAngle;
        private float randX;
        private float randZ;
        /// <summary>
        /// Sets the rotation angle and begins movement
        /// </summary>
        void Start()
        {
            startAngle = transform.localEulerAngles;
            StartCoroutine(SetPoints());
        }
        private void Update()
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, newAngle, 3.0f * Time.deltaTime);

        }
        public IEnumerator SetPoints()
        {
            
            newAngle = Quaternion.Euler(startAngle.x + Random.Range(-10f, 10f), 0, startAngle.z + Random.Range(-10f, 10f));
            Debug.LogError("set the points");
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(Cooldown());           
        }
        public IEnumerator Cooldown()
        {
            yield return new WaitForSeconds(3.0f);
            Debug.LogError("cooldown");
            StartCoroutine(SetPoints());
        }

    }

   

}


