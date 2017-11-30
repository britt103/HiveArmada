//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// SceneTransitionManager handles the loading and transitions between scenes.
//
//=============================================================================

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hive.Armada.Menu;

namespace Hive.Armada.Game
{
    /// <summary>
    /// Handles scene loading and transitions.
    /// </summary>
    public class SceneTransitionManager : MonoBehaviour
    {
        /// <summary>
        /// Color used in fades.
        /// </summary>
        public Color fadeColor = Color.black;

        /// <summary>
        /// When fade starts after in transition begins.
        /// </summary>
        public float fadeInStartTime = 0.5f;

        /// <summary>
        /// When fade starts after out transition begins.
        /// </summary>
        public float fadeOutStartTime = 0.5f;

        /// <summary>
        /// AudioSource to play during scene transitions.
        /// </summary>
        public AudioSource transitionAudioSource;

        /// <summary>
        /// Length of transition audio clip.
        /// </summary>
        private float audioClipLength;

        /// <summary>
        /// Partical effect during scene transitions.
        /// </summary>
        public GameObject transitionEmitter;

        /// <summary>
        /// When emitter starts after in transition begins.
        /// </summary>
        public float emitterInStartTime = 0.5f;

        /// <summary>
        /// When emitter starts after out transition begins.
        /// </summary>
        public float emitterOutStartTime = 0.5f;

        /// <summary>
        /// Transform to use for emitter instantiation.
        /// </summary>
        public Transform emitterPoint;

        /// <summary>
        /// Reference to ReferenceManager.
        /// </summary>
        private ReferenceManager reference;

        /// <summary>
        /// Find references. Implement in transition on scene start. 
        /// Minimize effect start times to audio clip length.
        /// </summary>
        private void Awake()
        {
            reference = FindObjectOfType<ReferenceManager>();
            audioClipLength = transitionAudioSource.clip.length;
            TransitionIn();
        }

        /// <summary>
        /// Fade camera into scene.
        /// </summary>
        public void TransitionIn()
        {
            transitionAudioSource.Play();
            StartCoroutine(FadeIn());
            StartCoroutine(StartEmitter(emitterInStartTime));
        }

        /// <summary>
        /// Fade camera out of scene and load specified scene.
        /// </summary>
        /// <param name="sceneName">Name of scene to load.</param>
        public void TransitionTo(string sceneName)
        {
            transitionAudioSource.Play();
            StartCoroutine(FadeOut());
            StartCoroutine(StartEmitter(emitterOutStartTime));
            StartCoroutine(LoadScene(sceneName));
        }

        /// <summary>
        /// Load specified scene after audio clip has finished playing.
        /// </summary>
        /// <param name="sceneName">Name of scene to load.</param>
        private IEnumerator LoadScene(string sceneName)
        {
            yield return new WaitForSeconds(audioClipLength);
            SceneManager.LoadScene(sceneName);
        }

        /// <summary>
        /// Start fade in effect when fade in set to start.
        /// </summary>
        private IEnumerator FadeIn()
        {
            yield return new WaitForSeconds(fadeInStartTime);
            SteamVR_Fade.Start(fadeColor, 0.0f);
            SteamVR_Fade.Start(Color.clear, audioClipLength - fadeInStartTime);
        }

        /// <summary>
        /// Start fade out effect when fade is set to start.
        /// </summary>
        private IEnumerator FadeOut()
        {
            yield return new WaitForSeconds(fadeOutStartTime);
            SteamVR_Fade.Start(Color.clear, 0.0f);
            SteamVR_Fade.Start(fadeColor, audioClipLength - fadeOutStartTime);
        }

        /// <summary>
        /// Start emitter effect when in or out effect is set to start.
        /// </summary>
        /// <param name="startTime">Time to start emitter effect.</param>
        private IEnumerator StartEmitter(float startTime)
        {
            yield return new WaitForSeconds(startTime);
            Instantiate(transitionEmitter, emitterPoint);
        }
    }
}
