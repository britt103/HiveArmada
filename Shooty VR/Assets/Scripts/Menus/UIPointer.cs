//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// UIPointer displays a LineRenderer representing the direction in which
// the controller is currently pointing. A Raycast determines with which UI
// elements the player can currently interact. Pressing the trigger button
// will activate interactable UI elements. 
//
//=============================================================================

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using Valve.VR.InteractionSystem;

namespace Hive.Armada.Menus
{
    /// <summary>
    /// Allows UI interactions using controller with LineRenderer.
    /// </summary>
    public class UIPointer : MonoBehaviour
    {
        /// <summary>
        /// Reference to Hand in immediate hierarchy.
        /// </summary>
        private Hand hand;

        /// <summary>
        /// LineRenderer that represents controller direction.
        /// </summary>
        private LineRenderer pointer;

        /// <summary>
        /// LineRenderer material.
        /// </summary>
        public Material laserMaterial;

        /// <summary>
        /// Alignment type for LineRenderer.
        /// </summary>
        [Tooltip("View makes line face camera. Local makes the line face the direction of the transform component")]
        public LineAlignment alignment;

        /// <summary>
        /// Color of the LineRenderer.
        /// </summary>
        public Color color;

        /// <summary>
        /// Thickness of the LineRenderer.
        /// </summary>
        public float thickness = 0.002f;

        /// <summary>
        /// ShadowCastingMode of the LineRenderer.
        /// </summary>
        public ShadowCastingMode castShadows;

        /// <summary>
        /// Determines whether LineRenderer receives shadows.
        /// </summary>
        public bool receiveShadows = false;

        /// <summary>
        /// Enabled state of LineRenderer.
        /// </summary>
        private bool isEnabled = false;

        /// <summary>
        /// Find Hand and initialize LineRenderer and parameters. 
        /// </summary>
        void Start()
        {
            hand = gameObject.GetComponentInParent<Hand>();

            if (hand.controller != null)
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
            }
        }

        /// <summary>
        /// Update LineRenderer end positions and execute activation of interactables.
        /// </summary>
        void Update()
        {
            if (hand.controller != null)
            {
                if (isEnabled)
                {
                    if (hand.GetComponentInChildren<Player.ShipController>() == null)
                    {
                        RaycastHit hit;
                        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, Utility.uiMask))
                        {
                            pointer.SetPosition(0, transform.position);
                            pointer.SetPosition(1, hit.point);

                            if (hit.collider.gameObject.CompareTag("Button"))
                            {
                                PointerEventData eventData = new PointerEventData(EventSystem.current);

                                //hover
                                //ExecuteEvents.Execute(hit.collider.gameObject, eventData, ExecuteEvents.pointerEnterHandler);

                                //click button
                                if (hand.controller.GetHairTriggerDown())
                                {
                                    ExecuteEvents.Execute(hit.collider.gameObject, eventData, ExecuteEvents.submitHandler);
                                }

                            }
                        }

                        else if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, Utility.roomMask))
                        {
                            pointer.SetPosition(0, transform.position);
                            pointer.SetPosition(1, hit.point);
                        }

                        float mag = (transform.position - hit.point).magnitude;
                        pointer.endWidth = thickness * Mathf.Max(mag, 1.0f);
                    }
                    
                    else
                    {
                        pointer.SetPosition(0, transform.position);
                        pointer.SetPosition(1, transform.position);
                    }
                }
                else
                {
                    isEnabled = true;
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
            }
        }
    }
}