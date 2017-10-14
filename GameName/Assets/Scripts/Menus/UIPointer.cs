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

namespace GameName
{
    [RequireComponent(typeof(LineRenderer))]
    public class UIPointer : MonoBehaviour
    {
        private Hand hand;
        private LineRenderer pointer;

        // Use this for initialization
        void Start()
        {
            hand = gameObject.GetComponentInParent<Hand>();
            pointer = gameObject.GetComponent<LineRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            if(hand.GetComponentInChildren<Player.ShipControllerNew>() == null)
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
            }
            else
            {
                pointer.SetPosition(0, transform.position);
                pointer.SetPosition(1, transform.position);
            }
        }
    }

}