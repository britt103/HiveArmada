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
//
//=============================================================================

using UnityEngine;
using UnityEngine.Rendering;

namespace Hive.Armada.Player
{
    /// <summary>
    /// Laser sight for the player. Also handles UI interaction.
    /// </summary>
    public class LaserSight : MonoBehaviour
    {
        /// <summary>
        /// Reference to the ship controller
        /// </summary>
        public ShipController shipController;

        /// <summary>
        /// The laser sight itself
        /// </summary>
        private LineRenderer laser;

        /// <summary>
        /// Material for the laser sight
        /// </summary>
        public Material laserMaterial;

        /// <summary>
        /// How thick the laser should be
        /// </summary>
        public float thickness = 0.002f;

        /// <summary>
        /// Initializes the laser sight's LineRenderer and ship controller reference.
        /// </summary>
        private void Start()
        {
            shipController = GameObject.FindGameObjectWithTag("Player")
                                       .GetComponent<ShipController>();

            laser = gameObject.AddComponent<LineRenderer>();
            laser.material = laserMaterial;
            laser.shadowCastingMode = ShadowCastingMode.Off;
            laser.receiveShadows = false;
            laser.alignment = LineAlignment.View;
            laser.startWidth = thickness;
            laser.endWidth = thickness;
        }

        /// <summary>
        /// Updates the laser sight position
        /// </summary>
        private void Update()
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity,
                                Utility.roomMask))
            {
                laser.SetPosition(0, transform.position);
                laser.SetPosition(1, hit.point);

                float mag = (transform.position - hit.point).magnitude;
                laser.endWidth = thickness * Mathf.Max(mag, 1.0f);
            }
        }
    }
}