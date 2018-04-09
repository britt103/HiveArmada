//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-440-1
// Group Project
//
// VolumeAdjustment changes the volume level of the audio source of the same 
// game object according to settings in OptionsValues.
//
//=============================================================================

using UnityEngine;

namespace Hive.Armada.Game
{
    //Adjusts audio source volume to match OptionsValues.
    public class VolumeAdjustment : MonoBehaviour
    {
        /// <summary>
        /// Enums for audio source categories.
        /// </summary>
        public enum AudioSourceCategory
        {
            Music,
            FX,
            Dialogue
        }

        /// <summary>
        /// Category of audio source of same game object.
        /// </summary>
        public AudioSourceCategory category;

        /// <summary>
        /// Reference to audio source.
        /// </summary>
        private AudioSource audioSource;

        /// <summary>
        /// Reference to ReferenceManager.
        /// </summary>
        private ReferenceManager reference;

        /// <summary>
        /// Reference to optionsValues.
        /// </summary>
        private OptionsValues optionsValues;

        [Tooltip("The base volume for this audio source.")]
        [Range(0.0f, 1.0f)]
        public float baseVolume = 1.0f;

        /// <summary>
        /// Find references. Update volume.
        /// </summary>
        void Start()
        {
            audioSource = gameObject.GetComponent<AudioSource>();
            reference = FindObjectOfType<ReferenceManager>();
            optionsValues = reference.optionsValues;

            UpdateVolume();
        }

        /// <summary>
        /// Set audio source volume using value in optionsValues.
        /// </summary>
        public void UpdateVolume()
        {
            switch (category)
            {
                case AudioSourceCategory.Music:
                    audioSource.volume = baseVolume * optionsValues.musicVolume;
                    break;
                case AudioSourceCategory.FX:
                    audioSource.volume = baseVolume * optionsValues.fxVolume;
                    break;
                case AudioSourceCategory.Dialogue:
                    audioSource.volume = baseVolume * optionsValues.dialogueVolume;
                    break;
            }
        }
    }
}
