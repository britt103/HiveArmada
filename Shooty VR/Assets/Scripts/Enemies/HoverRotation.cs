//=============================================================================
//
// Ryan Britton
// 1849351
// britt103@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// Hover effect for enemies that rotates their bodies
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
        float randomZ;
        float timer = 3.0f;
        Vector3 Target;
        float angle;

        /// <summary>
        /// Sets the rotation angle and begins movement
        /// </summary>
        void Start()
        {
            iTween.RotateBy(gameObject, iTween.Hash("y", 1.0f,"speed", 100.0f, "easetype", "linear", "islocal",true, "looptype", iTween.LoopType.loop));
            //new Vector3(transform.localPosition.x, transform.localPosition.y + 0.5f, transform.localPosition.z)
        }
        private void Update()
        {
            //iTween.RotateUpdate(gameObject, iTween.Hash("y", 1.0f, "speed", 3.0f, "easetype", "linear", "islocal", true));
        }
        

    }

   

}


