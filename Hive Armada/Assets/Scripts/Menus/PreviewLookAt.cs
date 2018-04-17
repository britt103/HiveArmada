//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-440-1
// Group Project
//
// PreviewLookAt rotates the attached preview prefab to face the specified
// transform.
//
//=============================================================================

using UnityEngine;

namespace Hive.Armada.Menus
{
    public class PreviewLookAt : MonoBehaviour
    {
        /// <summary>
        /// Transform to lookAt.
        /// </summary>
        private Transform lookAtTransform;

        /// <summary>
        /// Find player head transform.
        /// </summary>
        private void Awake()
        {
            lookAtTransform = GameObject.Find("VRCamera").transform;
        }

        // Update is called once per frame
        void Update()
        {
            gameObject.transform.LookAt(lookAtTransform);
        }
    }
}