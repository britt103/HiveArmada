//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// SceneTransitionManager handles the loading and transitions between scenes.
// Transition effects include fading, audio, and particles. Scene-specific
// actions are also done before and after transitions.
//
//=============================================================================

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hive.Armada.Menus;

namespace Hive.Armada.Game
{
    /// <summary>
    /// Handles scene loading and transitions.
    /// </summary>
    public class SceneTransitionManager : MonoBehaviour
    {
        /// <summary>
        /// Total length of transition.
        /// </summary>
        public float transitionLength;

        /// <summary>
        /// Color used in fades.
        /// </summary>
        public Color fadeColor = Color.black;

        /// <summary>
        /// When fade starts after in transition begins.
        /// </summary>
        public float fadeInStartTime = 0.0f;

        /// <summary>
        /// When fade starts after out transition begins.
        /// </summary>
        public float fadeOutStartTime = 0.0f;

        /// <summary>
        /// AudioSource to play during scene transitions.
        /// </summary>
        public AudioSource transitionAudioSource;

        /// <summary>
        /// When audio starts after in transition begins.
        /// </summary>
        public float audioInStartTime = 0.0f;

        /// <summary>
        /// When audio starts after out transition begins.
        /// </summary>
        public float audioOutStartTime = 0.0f;

        /// <summary>
        /// Partical effect during scene transitions.
        /// </summary>
        public GameObject transitionEmitter;

        /// <summary>
        /// When emitter starts after in transition begins.
        /// </summary>
        public float emitterInStartTime = 0.0f;

        /// <summary>
        /// When emitter starts after out transition begins.
        /// </summary>
        public float emitterOutStartTime = 0.0f;

        /// <summary>
        /// Transform to use for emitter instantiation.
        /// </summary>
        public Transform emitterPoint;

        /// <summary>
        /// Reference to SteamVR_LoadLevel
        /// </summary>
        public SteamVR_LoadLevel steamVRLoadLevel;

        /// <summary>
        /// Reference to instantiated emitter.
        /// </summary>
        private GameObject emitterGO;

        /// <summary>
        /// Reference to ReferenceManager.
        /// </summary>
        private ReferenceManager reference;

        /// <summary>
        /// Reference to SceneInfo.
        /// </summary>
        private SceneInfo sceneInfo;

        /// <summary>
        /// State of whether manager is currently performing transition.
        /// </summary>
        private bool isTransitioning = false;

        /// <summary>
        /// References to UIPointers.
        /// </summary>
        private UIPointer[] uiPointers;

        /// <summary>
        /// Find references. Implement in transition on scene start. 
        /// Minimize effect start times to audio clip length.
        /// Activate appropriate menu in Menu Room.
        /// </summary>
        private void Awake()
        {
            reference = FindObjectOfType<ReferenceManager>();
            sceneInfo = FindObjectOfType<SceneInfo>();

            if (SceneManager.GetActiveScene().name == "Menu Room")
            {
                if (sceneInfo.runFinished)
                {
                    reference.menuMain.transform.parent.transform.Find("Results Menu")
                        .gameObject.SetActive(true);
                    sceneInfo.runFinished = false;
                }
                else
                {
                    reference.menuMain.SetActive(true);
                }

                uiPointers = reference.player.GetComponentsInChildren<UIPointer>();

                if (!sceneInfo.firstScene)
                {
                    foreach (UIPointer pointer in uiPointers)
                    {
                        pointer.gameObject.SetActive(false);
                    }
                }
            }

            if (!sceneInfo.firstScene)
            {
                TransitionIn();
            }
        }

        /// <summary>
        /// Fade camera into scene.
        /// </summary>
        public void TransitionIn()
        {
            if (!isTransitioning)
            {
                isTransitioning = true;
                StartCoroutine(FadeIn());
                StartCoroutine(StartAudio(audioInStartTime));
                StartCoroutine(StartEmitter(emitterInStartTime));
                StartCoroutine(EndTransition());
            }
        }

        /// <summary>
        /// Fade camera out of scene and load specified scene.
        /// </summary>
        /// <param name="sceneName">Name of scene to load.</param>
        public void TransitionOut(string sceneName)
        {
            if (!isTransitioning)
            {
                isTransitioning = true;
                if (SceneManager.GetActiveScene().name == "Wave Room")
                {
                    reference.statistics.PrintStats();
                    sceneInfo.runFinished = true;
                }
                else if (SceneManager.GetActiveScene().name == "Menu Room")
                {
                    foreach (UIPointer pointer in uiPointers)
                    {
                        pointer.gameObject.SetActive(false);
                    }
                }

                StartCoroutine(FadeOut());
                StartCoroutine(StartAudio(audioOutStartTime));
                StartCoroutine(StartEmitter(emitterOutStartTime));
                sceneInfo.IncrementScenesLoaded();
                reference.optionsValues.SetPlayerPrefs();
                StartCoroutine(LoadScene(sceneName));
            }
        }

        /// <summary>
        /// Load specified scene after audio clip has finished playing.
        /// Save options player prefs before loading.
        /// </summary>
        /// <param name="sceneName">Name of scene to load.</param>
        private IEnumerator LoadScene(string sceneName)
        {
            steamVRLoadLevel.levelName = sceneName;
            yield return new WaitForSeconds(transitionLength);
            steamVRLoadLevel.Trigger();
        }

        /// <summary>
        /// Start fade in effect when fade in set to start.
        /// </summary>
        private IEnumerator FadeIn()
        {
            yield return new WaitForSeconds(fadeInStartTime);
            SteamVR_Fade.Start(fadeColor, 0.0f);
            SteamVR_Fade.Start(Color.clear, transitionLength - fadeInStartTime);
        }

        /// <summary>
        /// Start fade out effect when fade is set to start.
        /// </summary>
        private IEnumerator FadeOut()
        {
            yield return new WaitForSeconds(fadeOutStartTime);
            SteamVR_Fade.Start(Color.clear, 0.0f);
            SteamVR_Fade.Start(fadeColor, transitionLength - fadeOutStartTime);
        }

        /// <summary>
        /// Start emitter effect when in or out effect is set to start.
        /// </summary>
        /// <param name="startTime">Time to start emitter effect.</param>
        private IEnumerator StartEmitter(float startTime)
        {
            yield return new WaitForSeconds(startTime);
            emitterGO = Instantiate(transitionEmitter, emitterPoint);
        }

        /// <summary>
        /// Start audio effect when in or out effect is set to start.
        /// </summary>
        /// <param name="startTime">Time to start audio effect.</param>
        private IEnumerator StartAudio(float startTime)
        {
            yield return new WaitForSeconds(startTime);
            transitionAudioSource.Play();
        }

        /// <summary>
        /// Stop audio and emitter effects after transition ends (fade ends on it own).
        /// Activate UIPointers if in Menu Room.
        /// </summary>
        private IEnumerator EndTransition()
        {
            yield return new WaitForSeconds(transitionLength);
            transitionAudioSource.Stop();
            Destroy(emitterGO);

            string sceneName = SceneManager.GetActiveScene().name;

            if (sceneName == "Menu Room")
            {
                foreach (UIPointer pointer in uiPointers)
                {
                    pointer.gameObject.SetActive(true);
                }
            }

            else if (sceneName == "Wave Room")
            {
                reference.shipPickup.SetActive(true);
            }

            isTransitioning = false;
        }
    }
}
