//=============================================================================
// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-440-01
// Group Project
// 
// 
// 
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Deployment.Internal;
using UnityEngine;
using Hive.Armada.Game;
using SubjectNerd.Utilities;
using Random = UnityEngine.Random;

namespace Hive.Armada.Ambient
{
    public class ParticleFollow : MonoBehaviour
    {
        public ReferenceManager reference;

        private Transform player;

        [Header("Hovering")]
        public float hoverDistance;

        public float hoverTime;

        private float hoverPercent;

        private Vector3 hoverStart;

        private Vector3 hoverEnd;

        private Vector3 initialPosition;

        [Header("Talking")]
        [Range(0.1f, 1.0f)]
        public float minSize;

        [Range(1.0f, 2.0f)]
        public float maxSize;

        private Coroutine talkingCoroutine;

        public bool IsTalking;

        private void Awake()
        {
            if (reference == null)
            {
                Debug.LogError(GetType().Name + " - Reference is null.");
            }

            player = reference.playerLookTarget.transform;
            initialPosition = transform.position;
        }

        public void DoTalking(float time)
        {
            StartCoroutine(TalkTest(time));
        }

        private IEnumerator TalkTest(float time)
        {
            yield return new WaitForSeconds(0.6f);

            Talk();

            yield return new WaitForSeconds(time);

            IsTalking = false;
        }

        public void Talk()
        {
            if (!IsTalking)
            {
                IsTalking = true;
                if (talkingCoroutine == null)
                {
                    talkingCoroutine = StartCoroutine(Talking());
                }
            }
        }

        private IEnumerator Talking()
        {
            Random.InitState((int)DateTime.Now.Ticks);
            bool increasing = true;
            bool needTarget = true;
            float scale = transform.localScale.x;
            float initial = scale;
            float target = scale;
            float percent = 0.0f;
            const float speed = 20.0f;

            while (IsTalking)
            {
                if (needTarget)
                {
                    needTarget = false;

                    initial = target;
                    target = increasing ? Random.Range(1.001f, maxSize) : Random.Range(minSize, 1.001f);
                }

                percent += 0.02f * speed;

                scale = Mathf.SmoothStep(initial, target, percent);
                //scale = Mathf.Lerp(scale, target, percent);

                transform.localScale = new Vector3(scale, scale, scale);

                if (percent >= 1.0f)
                {
                    percent = 0.0f;
                    increasing = !increasing;
                    needTarget = true;
                    yield return new WaitForSeconds(Random.Range(0.001f, 0.2f));
                }

                yield return new WaitForSeconds(0.02f);
            }

            StartCoroutine(ResetSize());
            talkingCoroutine = null;
        }

        private IEnumerator ResetSize()
        {
            float scale = transform.localScale.x;
            float initial = scale;
            const float target = 1.0f;
            float percent = 0.0f;
            const float speed = 20.0f;

            while (true)
            {
                percent += 0.02f * speed;

                scale = Mathf.SmoothStep(initial, target, percent);

                transform.localScale = new Vector3(scale, scale, scale);

                if (percent >= 1.0f)
                {
                    yield break;
                }

                yield return new WaitForSeconds(0.02f);
            }
        }

        private void OnDialogueComplete()
        {
            IsTalking = false;
        }

        private void OnEnable()
        {
            SetHover();
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            transform.position = initialPosition;
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
                transform.LookAt(player);
                yield return null;
            }

            StartCoroutine(Hover(end, start));
        }
    }
}