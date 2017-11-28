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
        /// Amount of transparency camera fades.
        /// </summary>
        public float fadeValue = 1.0f;

        /// <summary>
        /// Duration of camera fade in.
        /// </summary>
        public float fadeInLength = 2.0f;

        /// <summary>
        /// Duration of camera fade out.
        /// </summary>
        public float fadeOutLength = 1.0f;

        /// <summary>
        /// AudioSource to play during scene transitions.
        /// </summary>
        public AudioSource transitionAudioSource;

        /// <summary>
        /// Reference to ReferenceManager.
        /// </summary>
        private ReferenceManager reference;

        /// <summary>
        /// Implement in transition on scene start.
        /// </summary>
        private void Awake()
        {
            reference = FindObjectOfType<ReferenceManager>();

            TransitionIn();
        }

        /// <summary>
        /// Fade camera into scene.
        /// </summary>
        public void TransitionIn()
        {
            transitionAudioSource.Play();

            SteamVR_Fade.Start(Color.black, 0.0f);
            SteamVR_Fade.Start(Color.clear, fadeInLength);
        }

        /// <summary>
        /// Fade camera out of scene and load specified scene.
        /// </summary>
        /// <param name="sceneName">Name of scene to load.</param>
        public void TransitionTo(string sceneName)
        {
            transitionAudioSource.Play();

            SteamVR_Fade.Start(Color.clear, 0.0f);
            SteamVR_Fade.Start(Color.black, fadeOutLength);

            StartCoroutine(LoadScene(sceneName));
        }

        private IEnumerator LoadScene(string sceneName)
        {
            yield return new WaitForSeconds(fadeOutLength);
            SceneManager.LoadScene(sceneName);
        }
    }
}
