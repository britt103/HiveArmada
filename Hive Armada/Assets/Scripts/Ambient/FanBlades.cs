//=============================================================================
//
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// This file handles the basic spinning of fan blades in the bottom of the
// room. It has a public float that tells it how much to rotate every second.
//
//=============================================================================

using UnityEngine;

namespace Hive.Armada.Ambient
{
    public class FanBlades : MonoBehaviour
    {
        /// <summary>
        /// How many degrees to rotate the fan every second
        /// </summary>
        public float fanSpeed;

        /// <summary>
        /// Rotates the fan every fixed frame
        /// </summary>
        private void FixedUpdate()
        {
            transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f), fanSpeed * Time.fixedDeltaTime);
        }
    }
}
