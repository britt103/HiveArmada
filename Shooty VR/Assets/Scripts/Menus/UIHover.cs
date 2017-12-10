//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// UIHover is a virtual parent class for buttons, sliders, toggles, etc. that
// are interacted with using the UIPointer. 
//
//=============================================================================

using UnityEngine;

namespace Hive.Armada.Menus
{
    /// <summary>
    /// Facilitate interaction with UIPointer.
    /// </summary>
    public class UIHover : MonoBehaviour
    {
        /// <summary>
        /// Reference to gameobject with image of active sprite.
        /// </summary>
        public GameObject activeSpriteImageGO;

        /// <summary>
        /// Reference to audio source.
        /// </summary>
        public AudioSource source;

        /// <summary>
        /// Reference to audio clip for source.
        /// </summary>
        public AudioClip hoverClip;

        /// <summary>
        /// State of whether UI is being hovered over.
        /// </summary>
        private bool isHovering = false;

        private void OnEnable()
        {
            EndHover();
        }

        /// <summary>
        /// Activate Active Image and play hover clip.
        /// </summary>
        public void Hover()
        {
            if (!isHovering)
            {
                isHovering = true;
                activeSpriteImageGO.SetActive(true);
                source.PlayOneShot(hoverClip);
            }
        }

        /// <summary>
        /// Deactivate Active Image.
        /// </summary>
        public void EndHover()
        {
            activeSpriteImageGO.SetActive(false);
            isHovering = false;
        }
    }
}
