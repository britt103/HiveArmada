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

using UnityEngine;
using UnityEngine.Rendering;

namespace Hive.Armada.Player
{
    public class LaserSight : MonoBehaviour
    {
        public ShipController.ShipMode mode;
        private LineRenderer laser;
        public Material laserMaterial;
        [Tooltip("View makes line face camera. Local makes the line face the direction of the transform component")]
        public LineAlignment alignment;
        public float thickness = 0.002f;
        public ShadowCastingMode castShadows;
        public bool receiveShadows = false;

        // Use this for initialization
        void Start()
        {
            laser = gameObject.AddComponent<LineRenderer>();
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(laserMaterial.color, 0.0f), new GradientColorKey(laserMaterial.color, 1.0f), },
                new GradientAlphaKey[] { new GradientAlphaKey(laserMaterial.color.a, 0.0f), new GradientAlphaKey(laserMaterial.color.a, 1.0f), });
            laser.material = laserMaterial;
            laser.shadowCastingMode = castShadows;
            laser.receiveShadows = receiveShadows;
            laser.alignment = alignment;
            laser.colorGradient = gradient;
            laser.startWidth = thickness;
            laser.endWidth = thickness;
        }

        // Update is called once per frame
        void Update()
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, Utility.roomMask))
            {
                laser.SetPosition(0, transform.position);
                laser.SetPosition(1, hit.point);
            }
        }

        /// <summary>
        /// Sets the mode for the laser, whether it should interact with UI or not.
        /// </summary>
        /// <param name="mode">  </param>
        public void SetMode(ShipController.ShipMode mode)
        {

        }
    }
}