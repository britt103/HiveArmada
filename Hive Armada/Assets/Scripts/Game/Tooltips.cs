//=============================================================================
// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-440-01
// Group Project
// 
// 
// 
//=============================================================================

using System.Collections;
using SubjectNerd.Utilities;
using UnityEngine;

namespace Hive.Armada.Game
{
    /// <summary>
    /// </summary>
    public class Tooltips : MonoBehaviour
    {
        /// <summary>
        /// Reference manager that holds all needed references
        /// (e.g. spawner, game manager, etc.)
        /// </summary>
        private ReferenceManager reference;

        [Header("Tooltip Spawns")]
        public Transform grabShipSpawn;

        public Transform protectShipSpawn;

        public Transform shootEnemiesSpawn;

        [Reorderable("Point", false)]
        public Transform[] grabPowerupSpawn;

        public Transform usePowerupSpawn;

        [Header("Tooltip Prefabs")]
        public GameObject grabShipTooltipPrefab;

        public GameObject protectShipTooltipPrefab;

        public GameObject shootEnemiesTooltipPrefab;

        public GameObject grabPowerupTooltipPrefab;

        public GameObject usePowerupTooltipPrefab;

        // Spawned tooltips
        private GameObject grabShipTooltip;

        private GameObject protectShipTooltip;

        private GameObject shootEnemiesTooltip;

        private GameObject grabPowerupTooltip;

        private GameObject usePowerupTooltip;

        [Header("Tooltip Time Lengths")]
        public float grabShipWaitLength;

        public float protectShipLength;

        public float shootEnemiesLength;

        public float grabPowerupWaitLength;

        public float usePowerupWaitLength;

        // Tooltip Booleans, used to prevent duplicate tooltips
        public bool GrabShip { get; private set; }

        public bool ProtectShip { get; private set; }

        public bool ShootEnemies { get; private set; }

        public bool GrabPowerup { get; private set; }

        public bool UsePowerup { get; private set; }

        // Tooltip Coroutines
        public Coroutine grabShipCoroutine;

        public Coroutine protectShipCoroutine;

        public Coroutine shootEnemiesCoroutine;

        public Coroutine grabPowerupCoroutine;

        public Coroutine usePowerupCoroutine;

        /// <summary>
        /// Sets reference.
        /// </summary>
        public void Initialize(ReferenceManager referenceManager)
        {
            reference = referenceManager;
        }

        #region GrabShip

        /// <summary>
        /// Starts the countdown for the Grab Ship tooltip spawn.
        /// </summary>
        public void SpawnGrabShip()
        {
            if (!GrabShip)
            {
                GrabShip = true;

                if (grabShipCoroutine != null)
                {
                    StopCoroutine(grabShipCoroutine);
                }

                grabShipCoroutine = StartCoroutine(GrabShipWait());
            }
        }

        /// <summary>
        /// Spawns the Grab Ship tooltip after grabShipWaitLength seconds.
        /// </summary>
        private IEnumerator GrabShipWait()
        {
            yield return new WaitForSeconds(grabShipWaitLength);

            grabShipTooltip = Instantiate(grabShipTooltipPrefab, grabShipSpawn.position,
                                          Quaternion.identity, grabShipSpawn);

            grabShipCoroutine = null;
        }

        /// <summary>
        /// Removes the Grab Ship tooltip.
        /// </summary>
        public void ShipGrabbed()
        {
            if (grabShipCoroutine != null)
            {
                StopCoroutine(grabShipCoroutine);
                grabShipCoroutine = null;
            }

            GrabShip = true;
            Destroy(grabShipTooltip);
        }

        #endregion

        #region ProtectShip

        /// <summary>
        /// Starts the countdown for the Protect Ship tooltip spawn.
        /// </summary>
        public void SpawnProtectShip()
        {
            if (!ProtectShip)
            {
                ProtectShip = true;

                if (protectShipCoroutine != null)
                {
                    StopCoroutine(protectShipCoroutine);
                }

                protectShipCoroutine = StartCoroutine(ProtectShipWait());
            }
        }

        /// <summary>
        /// Spawns the Protect Ship tooltip and destroys it after protectShipLength seconds.
        /// </summary>
        private IEnumerator ProtectShipWait()
        {
            protectShipTooltip = Instantiate(protectShipTooltipPrefab, protectShipSpawn.position,
                                             Quaternion.identity, protectShipSpawn);

            yield return new WaitForSeconds(protectShipLength);

            Destroy(protectShipTooltip);

            protectShipCoroutine = null;
        }

        #endregion

        #region ShootEnemies

        /// <summary>
        /// Starts the Shoot Enemies spawn.
        /// </summary>
        public void SpawnShootEnemies()
        {
            if (!ShootEnemies)
            {
                ShootEnemies = true;

                if (shootEnemiesCoroutine != null)
                {
                    StopCoroutine(shootEnemiesCoroutine);
                }

                shootEnemiesCoroutine = StartCoroutine(ShootEnemiesWait());
            }
        }

        /// <summary>
        /// Spawns the Shoot Enemies tooltip and destroys it after shootEnemiesLength seconds.
        /// </summary>
        private IEnumerator ShootEnemiesWait()
        {
            shootEnemiesTooltip = Instantiate(shootEnemiesTooltipPrefab, shootEnemiesSpawn.position,
                                              Quaternion.identity, shootEnemiesSpawn);

            yield return new WaitForSeconds(shootEnemiesLength);

            Destroy(shootEnemiesTooltip);

            shootEnemiesCoroutine = null;
        }

        //public void EnemyShot()
        //{
        //    if (shootEnemiesCoroutine != null)
        //    {
        //        StopCoroutine(shootEnemiesCoroutine);
        //        shootEnemiesCoroutine = null;
        //    }

        //    ShootEnemies = true;
        //    Destroy(shootEnemiesTooltip);
        //}

        #endregion

        #region GrabPowerup

        /// <summary>
        /// Starts the countdown for the Grab Powerup spawn.
        /// </summary>
        /// <param name="powerupSpawnPoint"> Index of the transform to spawn the tooltip on </param>
        public void SpawnGrabPowerup(int powerupSpawnPoint)
        {
            if (!GrabPowerup)
            {
                GrabPowerup = true;

                if (grabPowerupCoroutine != null)
                {
                    StopCoroutine(grabPowerupCoroutine);
                }

                grabPowerupCoroutine = StartCoroutine(GrabPowerupWait(powerupSpawnPoint));
            }
        }

        /// <summary>
        /// Spawns the Grab Powerup tooltip after grabPowerupWaitLength seconds.
        /// </summary>
        /// <param name="powerupSpawnPoint"> Index of the transform to spawn the tooltip on </param>
        private IEnumerator GrabPowerupWait(int powerupSpawnPoint)
        {
            yield return new WaitForSeconds(grabPowerupWaitLength);

            grabPowerupTooltip = Instantiate(grabPowerupTooltipPrefab,
                                             grabPowerupSpawn[powerupSpawnPoint].position,
                                             Quaternion.identity,
                                             grabPowerupSpawn[powerupSpawnPoint]);

            grabPowerupCoroutine = null;
        }

        /// <summary>
        /// Removes the Grab Powerup tooltip
        /// </summary>
        public void PowerupGrabbed()
        {
            if (grabPowerupCoroutine != null)
            {
                StopCoroutine(grabPowerupCoroutine);
                grabPowerupCoroutine = null;
            }

            GrabPowerup = true;
            Destroy(grabPowerupTooltip);
        }

        #endregion

        #region UsePowerup

        /// <summary>
        /// Starts the countdown for the Use Powerup tooltip spawn.
        /// </summary>
        public void SpawnUsePowerup()
        {
            if (!UsePowerup)
            {
                UsePowerup = true;

                if (usePowerupCoroutine != null)
                {
                    StopCoroutine(usePowerupCoroutine);
                }

                usePowerupCoroutine = StartCoroutine(UsePowerupWait());
            }
        }

        /// <summary>
        /// Spawns the Use Powerup tooltip if the player
        /// hasn't used it for usePowerupWaitLength seconds.
        /// </summary>
        private IEnumerator UsePowerupWait()
        {
            yield return new WaitForSeconds(usePowerupWaitLength);

            usePowerupTooltip = Instantiate(usePowerupTooltipPrefab, usePowerupSpawn.position,
                                            Quaternion.identity, usePowerupSpawn);

            usePowerupCoroutine = null;
        }

        /// <summary>
        /// Removes the Use Powerup tooltip
        /// </summary>
        public void PowerupUsed()
        {
            if (usePowerupCoroutine != null)
            {
                StopCoroutine(usePowerupCoroutine);
                usePowerupCoroutine = null;
            }

            UsePowerup = true;
            Destroy(usePowerupTooltip);
        }

        #endregion
    }
}