//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// DisplayMenu controls interactions with the Display Menu.
//
//=============================================================================

using UnityEngine;
using UnityEngine.UI;
using Hive.Armada.Game;

namespace Hive.Armada.Menus
{
    /// <summary>
    /// Controls interactions with Display Menu.
    /// </summary>
    public class DisplayMenu : MonoBehaviour
    {
        /// <summary>
        /// Reference to bloom toggle.
        /// </summary>
        public Toggle bloomToggle;

        /// <summary>
        /// Reference to Reference Manager.
        /// </summary>
        private ReferenceManager reference;

        /// <summary>
        /// Find references. 
        /// </summary>
        private void Awake()
        {
            reference = FindObjectOfType<ReferenceManager>();
        }

        /// <summary>
        /// Back button pressed. Navigate to Options Menu.
        /// </summary>
        public void PressBack()
        {
            reference.optionsValues.SetDisplayPlayerPrefs();
            GameObject.Find("Main Canvas").transform.Find("Options Menu").gameObject
                    .SetActive(true);
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Change bloom setting based on bloomToggle value.
        /// </summary>
        public void SetBloom(bool isOn)
        {
            reference.optionsValues.SetBloom(isOn);
        }
    }
}
