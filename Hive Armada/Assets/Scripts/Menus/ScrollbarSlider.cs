//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-440-01
// Group Project
//
// ScrollbarSlider works in coordination with ScrollbarUIHover to visually
// replace a ScrollView scrollbar with a vertical slider. Value changes are 
// passed on to the slider.
//
//=============================================================================

using UnityEngine;
using UnityEngine.UI;

namespace Hive.Armada.Menu
{
    public class ScrollbarSlider : MonoBehaviour
    {
        /// <summary>
        /// Reference to slider.
        /// </summary>
        public Slider slider;

        /// <summary>
        /// Pass value on to slider when value changed.
        /// </summary>
        /// <param name="value">Value of scrollbar after change.</param>
        public void OnValueChanged(float value)
        {
            slider.value = value;
        }
    }
}
