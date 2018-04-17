//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// PlayerHitVignette triggers a camera vignette effect when the player is hit
// by an enemy.
//
//=============================================================================


using System.Collections;
using UnityEngine;
using UnityEngine.PostProcessing;

namespace Hive.Armada.Player
{
    //Plays hit vignette effect when hit.
    public class PlayerHitVignette : MonoBehaviour
    {
        /// <summary>
        /// Reference to PostProcessingBehaviour.
        /// </summary>
        public PostProcessingBehaviour profile;

        /// <summary>
        /// Total duration of effect.
        /// </summary>
        public float effectLength = 0.5f;

        /// <summary>
        /// Maximum intensity of vignette effect. 
        /// </summary>
        public float maxIntensity = 0.671f;

        /// <summary>
        /// State of whether the player has just been hit and the effect is playing.
        /// </summary>
        private bool hit = false;

        /// <summary>
        /// Either starts or restarts PlayVignetteEffect when player is hit.
        /// </summary>
        public void Hit()
        {
            if (!hit)
            {
                StartCoroutine(PlayVignetteEffect());
                hit = true;
            }
            else
            {
                StopCoroutine(PlayVignetteEffect());
                StartCoroutine(PlayVignetteEffect());
            }
        }

        /// <summary>
        /// Transitions to and then from maximum intensity of vignette effect. Sets intensity to 0
        /// when effect ends. 
        /// </summary>
        private IEnumerator PlayVignetteEffect()
        {
            VignetteModel.Settings vm = profile.GetComponent<PostProcessingBehaviour>().profile.vignette.settings;

            for (float i = 0.0f; i <= effectLength / 2; i += Time.deltaTime)
            {
                vm.intensity = Mathf.Lerp(0.0f, maxIntensity, i / (effectLength / 2));
                profile.GetComponent<PostProcessingBehaviour>().profile.vignette.settings = vm;
                yield return new WaitForSeconds(0.005f);
            }

            for (float i = effectLength / 2; i >= 0.0f / 2; i -= Time.deltaTime)
            {
                vm.intensity = Mathf.Lerp(0.0f, maxIntensity, i / (effectLength / 2));
                profile.GetComponent<PostProcessingBehaviour>().profile.vignette.settings = vm;
                yield return new WaitForSeconds(0.005f);
            }

            vm.intensity = 0.0f;
            profile.GetComponent<PostProcessingBehaviour>().profile.vignette.settings = vm;
            hit = false;
            yield return null;
        }
    }
}
