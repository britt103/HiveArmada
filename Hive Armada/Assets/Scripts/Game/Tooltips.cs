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

using System;
using System.Collections;
using SubjectNerd.Utilities;
using UnityEngine;
using UnityEngine.UI;

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

        private float fadeTime = 0.5f;

        private Coroutine usePowerupFadeCoroutine;

        private int[] instanceIds;

        /// <summary>
        /// Sets reference.
        /// </summary>
        public void Initialize(ReferenceManager referenceManager)
        {
            reference = referenceManager;
            instanceIds = new int[5];
        }

        private IEnumerator FadeOutTooltip(GameObject tooltip)
        {
            if (tooltip == null)
            {
                yield break;
            }

            bool found = false;
            for (int i = 0; i < instanceIds.Length; ++i)
            {
                if (tooltip.GetInstanceID() == instanceIds[i])
                {
                    instanceIds[i] = 0;
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                yield break;
            }

            tooltip.GetComponentInChildren<Image>().CrossFadeAlpha(0.0f, fadeTime, false);
            tooltip.GetComponentInChildren<Text>().CrossFadeAlpha(0.0f, fadeTime, false);

            yield return new WaitForSeconds(fadeTime);

            Destroy(tooltip);
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
            instanceIds[0] = grabShipTooltip.GetInstanceID();
            grabShipTooltip.GetComponentInChildren<Image>().CrossFadeAlpha(0.0f, 0.0f, false);
            grabShipTooltip.GetComponentInChildren<Image>().CrossFadeAlpha(1.0f, fadeTime, false);
            grabShipTooltip.GetComponentInChildren<Text>().CrossFadeAlpha(0.0f, 0.0f, false);
            grabShipTooltip.GetComponentInChildren<Text>().CrossFadeAlpha(1.0f, fadeTime, false);

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
            StartCoroutine(FadeOutTooltip(grabShipTooltip));
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
            instanceIds[1] = protectShipTooltip.GetInstanceID();
            protectShipTooltip.GetComponentInChildren<Image>().CrossFadeAlpha(0.0f, 0.0f, false);
            protectShipTooltip.GetComponentInChildren<Image>().CrossFadeAlpha(1.0f, fadeTime, false);
            protectShipTooltip.GetComponentInChildren<Text>().CrossFadeAlpha(0.0f, 0.0f, false);
            protectShipTooltip.GetComponentInChildren<Text>().CrossFadeAlpha(1.0f, fadeTime, false);

            yield return new WaitForSeconds(protectShipLength);

            StartCoroutine(FadeOutTooltip(protectShipTooltip));

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
            yield return new WaitForSeconds(3.0f);

            shootEnemiesTooltip = Instantiate(shootEnemiesTooltipPrefab, shootEnemiesSpawn.position,
                                              Quaternion.identity, shootEnemiesSpawn);
            instanceIds[2] = shootEnemiesTooltip.GetInstanceID();
            shootEnemiesTooltip.GetComponentInChildren<Image>().CrossFadeAlpha(0.0f, 0.0f, false);
            shootEnemiesTooltip.GetComponentInChildren<Image>().CrossFadeAlpha(1.0f, fadeTime, false);
            shootEnemiesTooltip.GetComponentInChildren<Text>().CrossFadeAlpha(0.0f, 0.0f, false);
            shootEnemiesTooltip.GetComponentInChildren<Text>().CrossFadeAlpha(1.0f, fadeTime, false);

            yield return new WaitForSeconds(shootEnemiesLength);

            StartCoroutine(FadeOutTooltip(shootEnemiesTooltip));

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
            instanceIds[3] = grabPowerupTooltip.GetInstanceID();
            grabPowerupTooltip.GetComponentInChildren<Image>().CrossFadeAlpha(0.0f, 0.0f, false);
            grabPowerupTooltip.GetComponentInChildren<Image>().CrossFadeAlpha(1.0f, fadeTime, false);
            grabPowerupTooltip.GetComponentInChildren<Text>().CrossFadeAlpha(0.0f, 0.0f, false);
            grabPowerupTooltip.GetComponentInChildren<Text>().CrossFadeAlpha(1.0f, fadeTime, false);

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
            StartCoroutine(FadeOutTooltip(grabPowerupTooltip));
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
            instanceIds[4] = usePowerupTooltip.GetInstanceID();
            usePowerupTooltip.GetComponentInChildren<Image>().CrossFadeAlpha(0.0f, 0.0f, false);
            usePowerupTooltip.GetComponentInChildren<Image>().CrossFadeAlpha(1.0f, fadeTime, false);
            usePowerupTooltip.GetComponentInChildren<Text>().CrossFadeAlpha(0.0f, 0.0f, false);
            usePowerupTooltip.GetComponentInChildren<Text>().CrossFadeAlpha(1.0f, fadeTime, false);

            yield return new WaitForSeconds(8.0f);

            if (usePowerupTooltip != null)
            {
                if (usePowerupFadeCoroutine == null)
                {
                    usePowerupFadeCoroutine = StartCoroutine(FadeOutTooltip(usePowerupTooltip));
                }
            }

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

            if (usePowerupFadeCoroutine == null)
            {
                usePowerupFadeCoroutine = StartCoroutine(FadeOutTooltip(usePowerupTooltip));
            }
        }

        #endregion
    }
}