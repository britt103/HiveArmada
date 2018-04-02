//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.champan.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// PowerupStatus tracks the powerups currently stored and currently active. 
// Powerups are activated upon controller input. Stored powerups are lost
// between waves.
//
//=============================================================================

using System.Collections.Generic;
using UnityEngine;
using Hive.Armada.Menus;
using Hive.Armada.Player;
using Valve.VR.InteractionSystem;

namespace Hive.Armada.PowerUps
{
    /// <summary>
    /// Tracks stored powerups and activations.
    /// </summary>
    public class PowerUpStatus : MonoBehaviour
    {
        /// <summary>
        /// Queue containing powerup prefabs.
        /// </summary>
        private Queue<GameObject> powerups = new Queue<GameObject>();

        /// <summary>
        /// Queue containing powerup icons.
        /// </summary>
        private Queue<GameObject> powerupIcons = new Queue<GameObject>();

        /// <summary>
        /// Array containing names of powerups
        /// </summary>
        public string[] powerupNames;

        /// <summary>
        /// Maximum number of allowed stored powerups.
        /// </summary>
        public int maxStoredPowerups = 3;

        /// <summary>
        /// Distance between icons.
        /// </summary>
        public float iconSpacing = 1f;

        /// <summary>
        /// Reference to player ship.
        /// </summary>
        private GameObject shipGO;

        /// <summary>
        /// References to player ship powerup point.
        /// </summary>
        private Transform powerupPoint;

        /// <summary>
        /// References to playership icon point.
        /// </summary>
        private Transform iconPoint;

        /// <summary>
        /// References to PlayerStats.
        /// </summary>
        private PlayerStats stats;

        /// <summary>
        /// Reference to BestiaryUnlockData.
        /// </summary>
        private BestiaryUnlockData unlockData;

        /// <summary>
        /// Reference to active hand.
        /// </summary>
        private Hand hand;

        /// <summary>
        /// State of whether PowerupStatus is tracking inputs. 
        /// </summary>
        public bool tracking = false;

        /// <summary>
        /// Find references.
        /// </summary>
        private void Start()
        {
            stats = FindObjectOfType<PlayerStats>();
            unlockData = FindObjectOfType<BestiaryUnlockData>();
        }

        /// <summary>
        /// Activate powerup and tooltip if input is detected.
        /// </summary>
        private void Update()
        {
            if (tracking && powerups.Count > 0)
            {
                if (hand.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad))
                {
                    stats.PowerupUsed(powerups.Peek().name);
                    Instantiate(powerups.Dequeue(), powerupPoint);
                    RemoveDisplayIcon();
                }
            }
        }

        /// <summary>
        /// Trigger PowerupStatus to start tracking and find references.
        /// </summary>
        public void BeginTracking()
        {
            tracking = true;
            shipGO = gameObject.GetComponentInChildren<ShipController>().gameObject;
            hand = shipGO.GetComponentInParent<Hand>();
            powerupPoint = shipGO.transform.Find("Powerup Point");
            iconPoint = shipGO.transform.Find("Powerup Icon Point");
        }

        /// <summary>
        /// Add powerup to queues.
        /// </summary>
        /// <param name="powerupPrefab">GameObject of powerup</param>
        /// <param name="powerupIconPrefab">GameObject of powerup icon</param>
        public void StorePowerup(GameObject powerupPrefab, GameObject powerupIconPrefab)
        {
            powerups.Enqueue(powerupPrefab);
            GameObject newIcon = Instantiate(powerupIconPrefab, iconPoint);
            powerupIcons.Enqueue(newIcon);
            UpdateDisplayIcon(newIcon);
        }

        /// <summary>
        /// Adjust attributes of newly added icon based on queue count.
        /// </summary>
        /// <param name="newIcon">Newly added icon</param>
        private void UpdateDisplayIcon(GameObject newIcon)
        {
            //position
            newIcon.transform.localPosition = new Vector3
                    (iconSpacing * (powerupIcons.Count - 1), 0, 0);

            //scale
            //newIcon.transform.localScale *= (powerupIcons.Count / maxStoredPowerups);

            //transparency
            //Color color = newIcon.GetComponent<MeshRenderer>().material.color;
            //color.a = (255 * (float)(powerupIcons.Count / maxStoredPowerups));
            //color.a -= (alphaDelta * (powerupIcons.Count - 1));
            //newIcon.GetComponent<MeshRenderer>().material.color = color;
        }

        /// <summary>
        /// Removce icon and shift remaining icons.
        /// </summary>
        private void RemoveDisplayIcon()
        {
            Destroy(powerupIcons.Dequeue());
            foreach(GameObject icon in powerupIcons)
            {
                icon.transform.localPosition -= new Vector3(iconSpacing, 0, 0);

                //icon.transform.localScale += new Vector3((1 / maxStoredPowerups), (1 / maxStoredPowerups), (1 / maxStoredPowerups));

                //Color color = icon.GetComponent<MeshRenderer>().material.color;
                //color.a += alphaDelta;
                //icon.GetComponent<MeshRenderer>().material.color = color;
            }
        }

        /// <summary>
        /// Clear powerup queues and remove all icons. Meant to be used between waves. 
        /// </summary>
        public void RemoveStoredPowerups()
        {
            powerups.Clear();
            foreach(GameObject icon in powerupIcons)
            {
                Destroy(icon);
            }
            powerupIcons.Clear();
        }

        /// <summary>
        /// Return status of queue capacity.
        /// </summary>
        /// <returns>State of whether there is room left in queue.</returns>
        public bool HasRoom()
        {
            return (powerups.Count < maxStoredPowerups);
        }
    }
}
