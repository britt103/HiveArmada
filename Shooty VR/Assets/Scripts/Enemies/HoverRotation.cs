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
        /// <summary>
        /// Direction body starts facing
        /// </summary>
        private Vector3 startAngle;

        /// <summary>
        /// Angle to move body towards
        /// </summary>
        private Quaternion newAngle;
        

        /// <summary>
        /// Sets the rotation angle and begins movement
        /// </summary>
        void Start()
        {
            startAngle = transform.eulerAngles;
            StartCoroutine(SetPoints());
        }

        /// <summary>
        /// Rotates the body of this enemy randomly. Called every frame.
        /// </summary>
        private void Update()
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, newAngle, 3.0f * Time.deltaTime);

        }

        /// <summary>
        /// Selects an angle to rotate to.
        /// </summary>
        public IEnumerator SetPoints()
        {
            
            newAngle = Quaternion.Euler(startAngle.x + Random.Range(-10f, 10f), startAngle.y, startAngle.z + Random.Range(-10f, 10f));
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(Cooldown());           
        }
        /// <summary>
        /// 3 second wait timer for selecting a new angle.
        /// </summary>
        public IEnumerator Cooldown()
        {
            yield return new WaitForSeconds(3.0f);
            StartCoroutine(SetPoints());
        }

    }

   

}


