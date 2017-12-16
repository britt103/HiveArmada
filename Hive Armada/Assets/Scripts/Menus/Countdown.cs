//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// Countdown controls countdown timing and text.
//
//=============================================================================

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Hive.Armada.Game;

namespace Hive.Armada.Menus
{
    /// <summary>
    /// Controls visual and audio wave start countdown.
    /// </summary>
    public class Countdown : MonoBehaviour
    {
        /// <summary>
        /// References to Text component in Countdown GameObject. 
        /// </summary>
        public Text countdownText;

        /// <summary>
        /// Reference to AudioSource for countdown sfx.
        /// </summary>
        public AudioSource countdownAudioSource;

        /// <summary>
        /// Start Run coroutine.
        /// </summary>
        private void OnEnable()
        {
            StartCoroutine(Run());
        }

        /// <summary>
        /// Change countdown timer texts based on time, then start spawner.
        /// </summary>
        private IEnumerator Run()
        {
            countdownAudioSource.Play();
            for (int i = 5; i > 0; i--)
            {
                countdownText.text = i.ToString();
                yield return new WaitForSeconds(1.0f);
            }
            FindObjectOfType<WaveManager>().Run();
            gameObject.SetActive(false);
        }
    }

}
