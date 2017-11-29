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
        /// When fade starts after transition begins.
        /// </summary>
        public float fadeStartTime = 5.0f;

        /// <summary>
        /// Time between fadeStartTime and end of audio clip if clip length is 
        /// shorter than fadeStartTime.
        /// </summary>
        public float fadeStartTimeBuffer = 2.0f;

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
        /// When emitter starts after transition begins.
        /// </summary>
        public float emitterStartTime = 3.0f;

        /// <summary>
        /// Time between emitterStartTime and end of audio clip if clip length is 
        /// shorter than emitterStartTime.
        /// </summary>
        public float emitterStartTimeBuffer = 3.0f;

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
            fadeStartTime = Mathf.Min(fadeStartTime, audioClipLength - fadeStartTimeBuffer);
            emitterStartTime = Mathf.Min(emitterStartTime, audioClipLength - emitterStartTimeBuffer);
            TransitionIn();
        }

        /// <summary>
        /// Fade camera into scene.
        /// </summary>
        public void TransitionIn()
        {
            transitionAudioSource.Play();
            StartCoroutine(FadeIn());
        }

        /// <summary>
        /// Fade camera out of scene and load specified scene.
        /// </summary>
        /// <param name="sceneName">Name of scene to load.</param>
        public void TransitionTo(string sceneName)
        {
            transitionAudioSource.Play();
            StartCoroutine(FadeOut());
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
            yield return new WaitForSeconds(fadeStartTime);
            SteamVR_Fade.Start(fadeColor, 0.0f);
            SteamVR_Fade.Start(Color.clear, audioClipLength - fadeStartTime);
        }

        /// <summary>
        /// Start fade in effect when fade is set to start.
        /// </summary>
        private IEnumerator FadeOut()
        {
            yield return new WaitForSeconds(fadeStartTime);
            SteamVR_Fade.Start(Color.clear, 0.0f);
            SteamVR_Fade.Start(fadeColor, audioClipLength - fadeStartTime);
        }

        /// <summary>
        /// Start emitter effect when effect is set to start.
        /// </summary>
        /// <returns></returns>
        private IEnumerator StartEmitter()
        {
            yield return new WaitForSeconds(emitterStartTime);
            Instantiate(transitionEmitter, reference.player.transform);
        }
    }
}
