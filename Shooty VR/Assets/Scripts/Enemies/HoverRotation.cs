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

namespace Hive.Armada.Enemies
{
    /// <summary>
    /// Hovering rotation
    /// </summary>
    public class HoverRotation : MonoBehaviour
    {
        /// <summary>
        /// Sets the rotation angle and begins movement
        /// </summary>
        void Start()
        {
            iTween.RotateAdd(gameObject, iTween.Hash("name", "Xplane", "amount",
                                                  new Vector3(transform.localPosition.x + 60f, transform.localPosition.y, transform.localPosition.z),
                                                  "time", 1.0f, "easetype", iTween.EaseType.linear,
                                                  "looptype", iTween.LoopType.pingPong, "oncomplete", "PauseTween", "oncompletetarget", gameObject));
        }

        /// <summary>
        /// Calls PauseTween() 
        /// </summary>
        private void PauseTween()
        {
            StartCoroutine(PauseTween(0.1f));
        }

        /// <summary>
        /// Pauses movement for 'waitTime' seconds.
        /// </summary>
        /// <param name="waitTime"> How many seconds to pause for.</param>
        private IEnumerator PauseTween(float waitTime)
        {
            iTween.Pause(gameObject);
            yield return new WaitForSeconds(waitTime);

            iTween.Resume(gameObject);
        }
    }
}
