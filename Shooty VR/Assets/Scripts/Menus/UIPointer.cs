//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// UIPointer allows the player to interact with UI elements in the scene.
//
//=============================================================================

using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using Valve.VR.InteractionSystem;

namespace Hive.Armada.Menu
{
    /// <summary>
    /// UI interaction for player.
    /// </summary>
    public class UIPointer : MonoBehaviour
    {
        /// <summary>
        /// Reference to hand in parent object.
        /// </summary>
        private Hand hand;

        /// <summary>
        /// Line renderer that shows where player is pointing controller.
        /// </summary>
        private LineRenderer pointer;

        /// <summary>
        /// Material used in pointer.
        /// </summary>
        public Material laserMaterial;

        /// <summary>
        /// Alignment of pointer.
        /// </summary>
        [Tooltip("View makes line face camera. Local makes the line face the direction of the transform component")]
        public LineAlignment alignment;

        /// <summary>
        /// Color of pointer.
        /// </summary>
        public Color color;

        /// <summary>
        /// Thickness of pointer.
        /// </summary>
        public float thickness = 0.002f;

        /// <summary>
        /// ShadowCastingMode of pointer.
        /// </summary>
        public ShadowCastingMode castShadows;

        /// <summary>
        /// State of whether pointer receives shadows.
        /// </summary>
        public bool receiveShadows = false;

        /// <summary>
        /// The object the pointer is hitting.
        /// </summary>
        private GameObject aimObject;

        /// <summary>
        /// If the aimObject is tagged as button
        /// </summary>
        private bool isButton;

        /// <summary>
        /// Find references, initialize pointer and pointer values.
        /// </summary>
        private void Awake()
        {
            hand = gameObject.GetComponentInParent<Hand>();

            pointer = gameObject.AddComponent<LineRenderer>();
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(color, 0.0f), new GradientColorKey(color, 1.0f), },
                new GradientAlphaKey[] { new GradientAlphaKey(color.a, 0.0f), new GradientAlphaKey(color.a, 1.0f), });
            pointer.material = laserMaterial;
            pointer.shadowCastingMode = castShadows;
            pointer.receiveShadows = receiveShadows;
            pointer.alignment = alignment;
            pointer.colorGradient = gradient;
            pointer.startWidth = thickness;
            pointer.endWidth = thickness;
        }

        /// <summary>
        /// Adjust pointer point positions and thickness. Trigger button presses.
        /// </summary>
        void Update()
        {
            if(hand.controller != null)
            {
                RaycastHit hit;

                if (Physics.Raycast(transform.position, transform.forward,
                        out hit, Mathf.Infinity, Utility.uiMask))
                {
                    if (hit.collider.gameObject.CompareTag("Button"))
                    {
                        if (!isButton)
                        {
                            isButton = true;

                            try
                            {
                                hand.controller.TriggerHapticPulse();
                            }
                            catch (Exception)
                            {
                                // Do nothing
                            }
                        }
                    }
                    else
                    {
                        isButton = false;
                    }

                    aimObject = hit.collider.gameObject;
                    isButton = hit.collider.gameObject.CompareTag("Button");

                    pointer.SetPosition(0, transform.position);
                    pointer.SetPosition(1, hit.point);

                    float mag = (transform.position - hit.point).magnitude;
                    pointer.endWidth = thickness * Mathf.Max(mag, 1.0f);
                }
                else if (Physics.Raycast(transform.position, transform.forward,
                    out hit, Mathf.Infinity, Utility.roomMask))
                {
                    aimObject = null;
                    isButton = false;

                    pointer.SetPosition(0, transform.position);
                    pointer.SetPosition(1, hit.point);

                    float mag = (transform.position - hit.point).magnitude;
                    pointer.endWidth = thickness * Mathf.Max(mag, 1.0f);
                }
                else
                {
                    aimObject = null;
                    isButton = false;
                }

                if (hand.GetStandardInteractionButtonDown())
                {
                    TriggerUpdate();
                }
            }
        }

        /// <summary>
        /// Sent every frame while the trigger is pressed
        /// </summary>
        public void TriggerUpdate()
        {
            if (isButton)
            {
                ExecuteEvents.Execute(aimObject,
                    new PointerEventData(EventSystem.current), ExecuteEvents.submitHandler);
            }
        }
    }
}