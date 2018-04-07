//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// ColorBlindMode stores color channel values for the different color blind 
// modes.
//
//=============================================================================

using System;
using UnityEngine;
using UnityEngine.PostProcessing;

namespace Hive.Armada.Game
{
    public class ColorBlindMode : MonoBehaviour
    {

        public PostProcessingProfile postProcessingProfile;

        private ColorGradingModel.ChannelMixerSettings channelMixerSettings;

        public enum Mode
        {
            Standard,
            Protanopia,
            Deuteranopia,
            Tritanopia
        }

        [Serializable]
        public struct ModeData
        {
            public Mode mode;

            public Vector3 redChannel;

            public Vector3 greenChannel;

            public Vector3 blueChannel;
        }

        public ModeData[] modes;

        private void Awake()
        {
            channelMixerSettings = postProcessingProfile.colorGrading.settings.channelMixer;
        }

        public void SetMode(Mode mode)
        {
            foreach (ModeData modeData in modes)
            {
                if (modeData.mode == mode)
                {
                    channelMixerSettings.red = modeData.redChannel;
                    channelMixerSettings.green = modeData.greenChannel;
                    channelMixerSettings.blue = modeData.blueChannel;
                    break;
                }
            }
        }
    }
}
