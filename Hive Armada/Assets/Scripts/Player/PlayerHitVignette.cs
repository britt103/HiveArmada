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

        private float effectDelta = 0.0f;

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
                effectDelta = 0.0f;
                StartCoroutine(PlayVignetteEffect());
            }
        }


        private IEnumerator PlayVignetteEffect()
        {
            VignetteModel.Settings vm = profile.GetComponent<PostProcessingBehaviour>().profile.vignette.settings;
            vm.intensity = Mathf.Lerp(0, maxIntensity, ((Mathf.Sin(effectDelta) + 1.0f) / 2.0f) / effectLength);
            effectDelta += Time.deltaTime;
            profile.GetComponent<PostProcessingBehaviour>().profile.vignette.settings = vm;
            yield return new WaitWhile(() => effectDelta < effectLength);
            vm.intensity = 0.0f;
            profile.GetComponent<PostProcessingBehaviour>().profile.vignette.settings = vm;
            effectDelta = 0.0f;
            hit = false;
        }
    }
}
