//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// ColorBlindMode stores color channel values for the different color blind 
// modes and applies them to the post processing profile.
//
//=============================================================================

using System;
using UnityEngine;
using UnityEngine.PostProcessing;

namespace Hive.Armada.Game
{
    /// <summary>
    /// Stores and changes color channel values.
    /// </summary>
    public class ColorBlindMode : MonoBehaviour
    {
        /// <summary>
        /// Reference to player camera game object.
        /// </summary>
        public GameObject cameraGO;

        /// <summary>
        /// Enum for different named color blind modes. 
        /// </summary>
        public enum Mode
        {
            Standard,
            Protanopia,
            Deuteranopia,
            Tritanopia
        }

        /// <summary>
        /// Struct associating modes with channel values.
        /// </summary>
        [Serializable]
        public struct ModeData
        {
            public Mode mode;

            public Vector3 redChannel;

            public Vector3 greenChannel;

            public Vector3 blueChannel;
        }

        /// <summary>
        /// Collection of mode data.
        /// </summary>
        public ModeData[] modes;

        /// <summary>
        /// Apply channel mixer values based on specified mode.
        /// </summary>
        /// <param name="mode">Mode to get new values from.</param>
        public void SetMode(Mode mode)
        {
            cameraGO.GetComponent<PostProcessingBehaviour>().profile.colorGrading.enabled = true;

            foreach (ModeData modeData in modes)
            {
                if (modeData.mode == mode)
                {
                    ColorGradingModel.Settings cgms = cameraGO.GetComponent<PostProcessingBehaviour>().profile.colorGrading.settings;
                    cgms.channelMixer.red = modeData.redChannel;
                    cgms.channelMixer.green = modeData.greenChannel;
                    cgms.channelMixer.blue = modeData.blueChannel;
                    cameraGO.GetComponent<PostProcessingBehaviour>().profile.colorGrading.settings = cgms;
                    break;
                }
            }
        }
    }
}
