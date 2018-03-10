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
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

namespace Hive.Armada.Player
{
    public class PlayerHitVignette : MonoBehaviour
    {
        public PostProcessingBehaviour profile;

        public float effectLength = 0.5f;

        public float maxIntensity = 0.671f;

        private bool hit = false;

        public void Hit()
        {
            Debug.Log("Here");
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
