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

namespace Hive.Armada.Menus
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
        /// Reference to last touched interactable.
        /// </summary>
        private GameObject lastInteractableAimObject;

        /// <summary>
        /// If the aimObject is tagged as InteractableUI
        /// </summary>
        private bool isInteractable;

        /// <summary>
        /// State of whether all UIPointer components have been initialized besides hand.
        /// </summary>
        private bool initialized = false;

        /// <summary>
        /// Reference to Menus gameobject.
        /// </summary>
        private GameObject menus;

        /// <summary>
        /// State of whether menus are toggled on.
        /// </summary>
        private bool menusOn = true;

        /// <summary>
        /// Find references, initialize pointer and pointer values.
        /// </summary>
        private void OnEnable()
        {
            hand = gameObject.GetComponentInParent<Hand>();

            if (hand.controller != null)
            {
                Initialize();
            }
        }

        /// <summary>
        /// Adjust pointer point positions and thickness. Trigger button presses.
        /// </summary>
        void Update()
        {
            if(hand.controller != null)
            {
                if (!initialized)
                {
                    Initialize();
                }

                if (hand.controller.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
                {
                    ToggleMenus();
                }

                RaycastHit hit;

                if (Physics.Raycast(transform.position, transform.forward,
                        out hit, Mathf.Infinity, Utility.uiMask))
                {
                    if (hit.collider.gameObject.CompareTag("InteractableUI"))
                    {
                        if (!isInteractable)
                        {
                            isInteractable = true;

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
                        ExitLastInteractable();
                        isInteractable = false;
                    }

                    aimObject = hit.collider.gameObject;

                    pointer.SetPosition(0, transform.position);
                    pointer.SetPosition(1, hit.point);

                    float mag = (transform.position - hit.point).magnitude;
                    pointer.endWidth = thickness * Mathf.Max(mag, 1.0f);
                }
                else if (Physics.Raycast(transform.position, transform.forward,
                    out hit, Mathf.Infinity, Utility.roomMask))
                {
                    ExitLastInteractable();
                    aimObject = null;
                    isInteractable = false;

                    pointer.SetPosition(0, transform.position);
                    pointer.SetPosition(1, hit.point);

                    float mag = (transform.position - hit.point).magnitude;
                    pointer.endWidth = thickness * Mathf.Max(mag, 1.0f);
                }
                else
                {
                    ExitLastInteractable();
                    aimObject = null;
                    isInteractable = false;
                }

                //Check for UI interaction
                if (isInteractable)
                {
                    if (aimObject != lastInteractableAimObject)
                    {
                        ExitLastInteractable();
                    }

                    lastInteractableAimObject = aimObject;

                    if (aimObject.GetComponent<UIHover>())
                    {
                        aimObject.GetComponent<UIHover>().Hover();
                    }
                }

                if (hand.GetStandardInteractionButtonDown())
                {
                    TriggerUpdate(false);
                }
                else if (hand.GetStandardInteractionButton())
                {
                    TriggerUpdate(true);
                }
            }
        }

        /// <summary>
        /// Sent every frame while the trigger is pressed
        /// </summary>
        public void TriggerUpdate(bool stay)
        {
            if (isInteractable)
            {
                if (aimObject.GetComponent<Slider>())
                {
                    float centerX = aimObject.GetComponent<BoxCollider>().center.x;
                    float maxX = centerX + aimObject.GetComponent<BoxCollider>().bounds.extents.x;
                    float minX = centerX - aimObject.GetComponent<BoxCollider>().bounds.extents.x;
                    float pointerX = pointer.GetPosition(1).x;
                    if (pointerX > minX && pointerX < maxX)
                    {
                        float value = (pointerX - minX) / (maxX - minX);
                        aimObject.GetComponent<Slider>().value = value;
                    }
                }
                else if (aimObject.GetComponent<Scrollbar>())
                {
                    Vector3 localCenter = aimObject.GetComponent<BoxCollider>().center;
                    float centerY = aimObject.gameObject.transform.TransformPoint(localCenter).y;
                    float maxY = centerY + aimObject.GetComponent<BoxCollider>().bounds.extents.y;
                    float minY = centerY - aimObject.GetComponent<BoxCollider>().bounds.extents.y;
                    float pointerY = pointer.GetPosition(1).y;
                    float value = (pointerY - minY) / (maxY - minY);

                    ScrollRect scrollRect = aimObject.GetComponentInParent<ScrollRect>();
                    scrollRect.verticalNormalizedPosition = value;
                }
                else if (!stay)
                {
                    ExecuteEvents.Execute(aimObject, new PointerEventData(EventSystem.current), 
                        ExecuteEvents.submitHandler);
                }

                lastInteractableAimObject = aimObject;
            }
        }

        /// <summary>
        /// Initialize non-hand components.
        /// </summary>
        private void Initialize()
        {
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

            menus = GameObject.Find("Menus");

            initialized = true;
        }

        /// <summary>
        /// Exit active state of InteractableUI last interacted with.
        /// </summary>
        private void ExitLastInteractable()
        {
            if (lastInteractableAimObject && lastInteractableAimObject.GetComponent<UIHover>())
            {
                lastInteractableAimObject.GetComponent<UIHover>().EndHover();
                lastInteractableAimObject = null;
            }
        }

        /// <summary>
        /// Toggle visibility of menus using Vive Menu button.
        /// </summary>
        private void ToggleMenus()
        {
            menusOn = !menusOn;
            menus.SetActive(menusOn);
        }
    }
}