//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-440-1
// Group Project
//
// PreviewRotation allows the player to use the UIPointer to rotate the object.
//
//=============================================================================

using UnityEngine;

namespace Hive.Armada.Menus
{
    /// <summary>
    /// Pointer-driven rotation.
    /// </summary>
    public class PreviewRotation : MonoBehaviour
    {
        /// <summary>
        /// Sensitivity of rotation. 
        /// </summary>
        private float sensitivity = 50.0f;

        /// <summary>
        /// Position of pointer end from which to calculate difference using current pointer
        /// end position.
        /// </summary>
        private Vector3 pointerPosition;

        /// <summary>
        /// State of whether object is currently rotating.
        /// </summary>
        private bool isRotating;

        /// <summary>
        /// Rotates object based on specified position.
        /// </summary>
        /// <param name="pointerEndPoint">Position of end of UI pointer.</param>
        public void Rotate(Vector3 pointerEndPoint)
        {
            if (!isRotating)
            {
                pointerPosition = pointerEndPoint;
                isRotating = true;
            }

            Vector3 pointerOffset = pointerEndPoint - pointerPosition;
            Vector3 rotation = Vector3.zero;
            rotation.y = -(pointerOffset.x + pointerOffset.y) * sensitivity;
            transform.Rotate(rotation);
            pointerPosition = pointerEndPoint;
        }

        /// <summary>
        /// Sets isRotating to false.
        /// </summary>
        public void StopRotating()
        {
            isRotating = false;
        }
    }
}
