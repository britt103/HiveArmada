//=============================================================================
//
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-440-01
// Group Project
//
// This class enables the Bestiary previews to hover.
//
//=============================================================================

using UnityEngine;

namespace Hive.Armada.Menus
{
    /// <summary>
    /// Simple hover for Bestiary previews.
    /// </summary>
    public class PreviewHover : MonoBehaviour
    {
        /// <summary>
        /// The distance the preview hovers.
        /// </summary>
        public float hoverDistance;

        /// <summary>
        /// The speed at which the preview hovers.
        /// </summary>
        public float hoverSpeed;

        /// <summary>
        /// Value used in the sine function to compute the hover position.
        /// </summary>
        private float theta;

        /// <summary>
        /// The bottom of the hover.
        /// </summary>
        private Vector3 lowerPosition;

        /// <summary>
        /// The top of the hover.
        /// </summary>
        private Vector3 upperPosition;

        /// <summary>
        /// Initializes the upper and lower positions.
        /// </summary>
        private void Start()
        {
            lowerPosition = new Vector3(transform.position.x,
                                        transform.position.y - hoverDistance / 2.0f,
                                        transform.position.z);
            upperPosition = new Vector3(transform.position.x,
                                        transform.position.y + hoverDistance / 2.0f,
                                        transform.position.z);

            theta = 0.0f;
        }

        /// <summary>
        /// Moves the game object between the 2 positions.
        /// </summary>
        private void Update()
        {
            transform.position =
                Vector3.Lerp(lowerPosition, upperPosition, (Mathf.Sin(theta) + 1.0f) / 2.0f);

            theta += hoverSpeed * Time.deltaTime;

            if (theta > Mathf.PI * 3.0f / 2.0f)
            {
                theta -= Mathf.PI * 2.0f;
            }
        }
    }
}