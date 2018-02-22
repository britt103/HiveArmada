//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-440-01
// Group Project
//
// ScrollbarUIHover works in coordination with ScrollbarSlider to visually
// replace a ScrollView scrollbar with a vertical slider. UIHover calls are 
// passed on to the slider's UIHover component.
//
//=============================================================================

using UnityEngine;

namespace Hive.Armada.Menus
{
    /// <summary>
    /// Passes UIHover calls from scrollbar to slider
    /// </summary>
    public class ScrollbarUIHover : MonoBehaviour
    {
        /// <summary>
        /// Reference to slider UIHover component.
        /// </summary>
        public UIHover sliderUIHover;

        /// <summary>
        /// Prevent hovering on enable.
        /// </summary>
        private void OnEnable()
        {
            PassEndHover();
        }

        /// <summary>
        /// Pass Hover call to slider.
        /// </summary>
        public void PassHover()
        {
            sliderUIHover.Hover();
        }

        /// <summary>
        /// Pass EdnHover call to slider.
        /// </summary>
        public void PassEndHover()
        {
            sliderUIHover.EndHover();
        }
    }
}
