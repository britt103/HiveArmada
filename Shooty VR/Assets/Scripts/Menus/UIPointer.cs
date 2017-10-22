//Name: Chad Johnson
//Student ID: 1763718
//Email: johns428@mail.chapman.edu
//Course: CPSC 340-01, CPSC-344-01
//Assignment: Group Project
//Purpose: UI menu pointer interaction

//http://answers.unity3d.com/questions/820599/simulate-button-presses-through-code-unity-46-gui.html

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Valve.VR.InteractionSystem;
using UnityEngine.Rendering;

namespace Hive.Armada.Menu
{
    public class UIPointer : MonoBehaviour
    {
        private Hand hand;
        private LineRenderer pointer;
        public Material laserMaterial;
        [Tooltip("View makes line face camera. Local makes the line face the direction of the transform component")]
        public LineAlignment alignment;
        public Color color;
        public float thickness = 0.002f;
        public ShadowCastingMode castShadows;
        public bool receiveShadows = false;
        private bool isEnabled = false;

        // Use this for initialization
        void Start()
        {
            hand = gameObject.GetComponentInParent<Hand>();

            if (hand.controller != null)
            {
                //pointer = gameObject.GetComponent<LineRenderer>();
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

        // Update is called once per frame
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
                    //disapear line when ship being held
                    else
                    {
                        pointer.SetPosition(0, transform.position);
                        pointer.SetPosition(1, transform.position);
                    }
                }
                else
                {
                    isEnabled = true;
                    //pointer = gameObject.GetComponent<LineRenderer>();
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