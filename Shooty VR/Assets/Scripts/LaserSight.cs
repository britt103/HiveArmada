//=============================================================================
//
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC 340-01 & CPSC-344-01
// Group Project
//
// This class controls the laser pointer for the player ship. It uses a
// Line Renderer to draw a laser from the front of the player ship to the wall.
// It originally used a cube stretched between the ship and a RayCastHit point,
// but it is easier for the player to see where they are aiming when the laser
// continues through enemies. If not, the laser could potential end just in
// front of the ship and could go unnoticed by the player.
// Doubles as the UI interaction pointer for menus. Can be toggled to menu mode
// where it will interact with UI objects.
//
//=============================================================================

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Valve.VR.InteractionSystem;
using UnityEngine.Rendering;

namespace Hive.Armada.Player
{
    public class LaserSight : MonoBehaviour
    {
        public ShipController shipController;
        public ShipController.ShipMode mode;
        private LineRenderer laser;
        public Material laserMaterial;
        [Tooltip("View makes line face camera. Local makes the line face the direction of the transform component")]
        public LineAlignment alignment;
        public float thickness = 0.002f;
        public ShadowCastingMode castShadows;
        public bool receiveShadows = false;
        private GameObject aimObject;
        private bool isButton;

        // Use this for initialization
        void Start()
        {
            laser = gameObject.AddComponent<LineRenderer>();
            laser.material = laserMaterial;
            laser.shadowCastingMode = castShadows;
            laser.receiveShadows = receiveShadows;
            laser.alignment = alignment;
            laser.startWidth = thickness;
            laser.endWidth = thickness;
        }

        void Awake()
        {
            shipController = GameObject.FindGameObjectWithTag("Player").GetComponent<ShipController>();
        }

        // Update is called once per frame
        void Update()
        {
            RaycastHit hit;

            switch (mode)
            {
                case ShipController.ShipMode.Game:
                    if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, Utility.roomMask))
                    {
                        aimObject = hit.collider.gameObject;
                        isButton = hit.collider.gameObject.CompareTag("Button");

                        laser.SetPosition(0, transform.position);
                        laser.SetPosition(1, hit.point);
                    }
                    else
                    {
                        aimObject = null;
                        isButton = false;
                    }
                    break;
                case ShipController.ShipMode.Menu:
                    if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, Utility.uiMask))
                    {
                        if (hit.collider.gameObject.CompareTag("Button"))
                        {
                            if (!isButton)
                            {
                                isButton = true;
                                if (shipController != null)
                                {
                                    shipController.hand.controller.TriggerHapticPulse();
                                }
                            }
                        }
                        else
                        {
                            isButton = false;
                        }

                        aimObject = hit.collider.gameObject;
                        isButton = hit.collider.gameObject.CompareTag("Button");

                        laser.SetPosition(0, transform.position);
                        laser.SetPosition(1, hit.point);
                    }
                    else if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, Utility.roomMask))
                    {
                        aimObject = null;
                        isButton = false;

                        laser.SetPosition(0, transform.position);
                        laser.SetPosition(1, hit.point);
                    }
                    else
                    {
                        aimObject = null;
                        isButton = false;
                    }
                    break;
            }
        }

        /// <summary>
        /// Sent every frame while the trigger is pressed
        /// </summary>
        public void TriggerUpdate()
        {
            if (mode.Equals(ShipController.ShipMode.Menu) && isButton)
            {
                ExecuteEvents.Execute(aimObject, new PointerEventData(EventSystem.current), ExecuteEvents.submitHandler);
            }
        }

        /// <summary>
        /// Sets the mode for the laser, whether it should interact with UI or not.
        /// </summary>
        /// <param name="mode"> The ShipMode to use </param>
        public void SetMode(ShipController.ShipMode mode)
        {
            this.mode = mode;
        }
    }
}