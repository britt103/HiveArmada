//=============================================================================
// 
// Ryan Britton
// 1849351
// britt103@mail.chapman.edu
// CPSC-440-01
// Group Project
// 
// 
// 
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hive.Armada.Game;

namespace Hive.Armada.Ambient
{
    public class AllyPosition : MonoBehaviour
    {
        /// <summary>
        /// Reference manager that holds all needed references
        /// (e.g. spawner, game manager, etc.)
        /// </summary>
        private ReferenceManager reference;

        [Range(0, 5)]
        public int lookStrength;

        public GameObject lookTarget;

        public AudioSource allySource;

        public AudioClip[] allyClips;

        public GameObject[] allyPosition;

        [Header("Hovering")]
        public float hoverDistance;

        public float hoverTime;

        private float hoverPercent;

        private Vector3 hoverStart;

        private Vector3 hoverEnd;

        /// <summary>
        /// Whether the ally can begin hovering
        /// </summary>
        private bool hoverReady;

        private GameObject player;

        /// <summary>
        /// Selects random position to place ally and play audio.
        /// </summary>
        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("MainCamera");
            int pos = Random.Range(0, allyPosition.Length);
            gameObject.transform.position = allyPosition[pos].transform.position;
            SetHover();
            StartCoroutine(PlayAllyAudio(pos));
        }

        /// <summary>
        /// Moves ship up and down over time.
        /// </summary>
        private void Update()
        {
            if (lookTarget == null)
            {
                lookTarget = reference.playerLookTarget;
            }

            Quaternion to =
                Quaternion.LookRotation(lookTarget.transform.position - transform.position);

            transform.rotation =
                Quaternion.Slerp(transform.rotation, to, lookStrength * Time.deltaTime);
        }

        /// <summary>
        /// Sets the start and end points for the hover effect.
        /// </summary>
        private void SetHover()
        {
            hoverStart = transform.position;
            hoverEnd = new Vector3(transform.position.x,
                                   transform.position.y + hoverDistance,
                                   transform.position.z);
            hoverReady = true;

            StartCoroutine(Hover(hoverStart, hoverEnd));
        }

        /// <summary>
        /// Hovers the boss from 'start' to 'end'
        /// </summary>
        /// <param name="start"> The start point </param>
        /// <param name="end"> The end point </param>
        private IEnumerator Hover(Vector3 start, Vector3 end)
        {
            hoverPercent = 0.0f;

            while (hoverPercent <= 1.0f)
            {
                hoverPercent += Time.deltaTime / hoverTime;
                transform.position =
                    Vector3.Lerp(start, end, Mathf.SmoothStep(0.0f, 1.0f, hoverPercent));
                yield return null;
            }

            if (hoverReady)
            {
                StartCoroutine(Hover(end, start));
            }
        }

        /// <summary>
        /// Plays ally audio.
        /// </summary>
        /// <param name="clip">which clip to play</param>
        private IEnumerator PlayAllyAudio(int clip)
        {
            allySource.PlayOneShot(allyClips[clip]);

            yield return new WaitForSeconds(allyClips[clip].length);
        }
    }
}
