//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-440-1
// Group Project
//
// ScrollViewButtonBound removes buttons outside of a designated area from the
// UI layer. Designed to be used with ScrollView. Note: button must fully exit
// out-of-bounds before returning to UI layer.
//
//=============================================================================

using UnityEngine;
using UnityEngine.UI;

namespace Hive.Armada.Menus
{
    /// <summary>
    /// Button bound controller.
    /// </summary>
    public class ScrollViewButtonBound : MonoBehaviour
    {
        /// <summary>
        /// State of whether this bound is on the top or on the bottom.
        /// </summary>
        public bool top = true;

        /// <summary>
        /// Return button to UI layer after exiting out-of-bounds.
        /// </summary>
        /// <param name="other">Collider with which this collided.</param>
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("InteractableUI"))
            {
                if (top)
                {
                    if (other.gameObject.GetComponent<Button>() &&
                        other.gameObject.transform.position.y < gameObject.transform.position.y)
                    {
                        other.gameObject.layer = LayerMask.NameToLayer("UI");
                    }
                }
                else
                {
                    if (other.gameObject.GetComponent<Button>() &&
                        other.gameObject.transform.position.y > gameObject.transform.position.y)
                    {
                        other.gameObject.layer = LayerMask.NameToLayer("UI");
                    }
                }
            }

        }

        /// <summary>
        /// Remove button from UI layer when entering out-of-bounds.
        /// </summary>
        /// <param name="other">Collider with which this collided.</param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<Button>())
            {
                other.gameObject.layer = LayerMask.NameToLayer("Default");
            }
        }
    }
}
