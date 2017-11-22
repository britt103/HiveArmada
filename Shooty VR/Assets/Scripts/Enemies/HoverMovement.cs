//=============================================================================
//
// Ryan Britton
// 1849351
// britt103@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// Hover effect for enemies that moves them up and down
//
//=============================================================================

using System.Collections;
using UnityEngine;

namespace Hive.Armada.Enemies
{
    /// <summary>
    /// Hover movement
    /// </summary>
    public class HoverMovement : MonoBehaviour
    {

        /// <summary>
        /// Moves gameObject up and down over time.
        /// </summary>
        void Start()
        {
            iTween.MoveTo(gameObject, iTween.Hash("name", "Xplane", "position",
                                                  new Vector3(transform.localPosition.x, transform.localPosition.y + 0.5f, transform.localPosition.z),
                                                  "speed", 0.2f, "islocal", true, "easetype", iTween.EaseType.linear,
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
