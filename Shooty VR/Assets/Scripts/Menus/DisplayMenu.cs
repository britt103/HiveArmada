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
using UnityEngine.PostProcessing;

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
        /// Reference to main camera GameObject.
        /// </summary>
        private GameObject cameraGO;

        /// <summary>
        /// Find references. Set initial UI values;
        /// </summary>
        private void Awake()
        {
            cameraGO = GameObject.Find("Player").GetComponentInChildren<Camera>().gameObject;
            bloomToggle.isOn = cameraGO.GetComponent<PostProcessingBehaviour>().profile.bloom.enabled;
        }

        /// <summary>
        /// Back button pressed. Navigate to Options Menu.
        /// </summary>
        public void PressBack()
        {
            GameObject.Find("Main Canvas").transform.Find("Options Menu").gameObject
                    .SetActive(true);
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Change bloom setting based on bloomToggle value;
        /// </summary>
        public void SetBloom(bool isOn)
        {
            cameraGO.GetComponent<PostProcessingBehaviour>().profile.bloom.enabled = isOn;
        }
    }
}
