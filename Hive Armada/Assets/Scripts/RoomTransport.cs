//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-440-1
// Group Project
//
// RoomTransport controls transitions between different areas of the Menu
// Room. These transitions are triggered by accessing certain menus.
//
//=============================================================================

using Hive.Armada.Ambient;
using UnityEngine;
using Hive.Armada.Game;

namespace Hive.Armada
{
    /// <summary>
    /// Transports player around room.
    /// </summary>
    public class RoomTransport : MonoBehaviour
    {
        /// <summary>
        /// Total length of transition.
        /// </summary>
        public float transitionLength;

        /// <summary>
        /// Color used in fades.
        /// </summary>
        public Color fadeColor = Color.black;

        /// <summary>
        /// Reference to player gameobject.
        /// </summary>
        private GameObject player;

        // Use this for initialization
        void Awake()
        {
            player = FindObjectOfType<ReferenceManager>().player;
        }

        /// <summary>
        /// Change Player transform between fading out and back in.
        /// </summary>
        /// <param name="newTransform">Transform of transport destination.</param>
        /// /// <param name="currMenuGO">Gameobject of current menu.</param>
        /// /// <param name="newTransform">Gameobject of next menu.</param>
        public void Transport(Transform newTransform, GameObject currMenuGO, GameObject nextMenuGO)
        {
            SteamVR_Fade.Start(Color.clear, 0.0f);
            SteamVR_Fade.Start(fadeColor, transitionLength / 2.0f);

            currMenuGO.SetActive(false);
            nextMenuGO.SetActive(true);

            FindObjectOfType<ReferenceManager>().talkingParticle.MovePosition(nextMenuGO.name);

            player.transform.position = newTransform.position;
            player.transform.rotation = newTransform.rotation;
            player.transform.localScale = newTransform.localScale;
            SteamVR_LaserPointer[]
                pointers = player.GetComponentsInChildren<SteamVR_LaserPointer>();
            foreach (SteamVR_LaserPointer pointer in pointers)
            {
                pointer.playerScale = newTransform.localScale.x;
            }

            SteamVR_Fade.Start(fadeColor, 0.0f);
            SteamVR_Fade.Start(Color.clear, transitionLength / 2.0f);
        }
    }
}
