//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-440-1
// Group Project
//
// PreviewRotation rotates the attached preview prrfab.
//
//=============================================================================

using UnityEngine;

namespace Hive.Armada.Menus
{
    /// <summary>
    /// Simple rotation.
    /// </summary>
    public class PreviewRotation : MonoBehaviour
    {
        /// <summary>
        /// Number of degrees of rotation per update.
        /// </summary>
        public float rotationSpeed = 1.0f;

        // Update is called once per frame
        void Update()
        {
            gameObject.transform.Rotate(Vector3.up, rotationSpeed);
        }
    }
}
