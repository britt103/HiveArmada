//=============================================================================
// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
// 
// This class handles the ship that the player picks up. It includes all
// functions needed for SteamVR's Player prefab to interact with it. It handles
// firing and switching weapons.
// 
//=============================================================================

using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using Hive.Armada.Game;
using Hive.Armada.Player.Weapons;
using SubjectNerd.Utilities;
using System;

namespace Hive.Armada.Player
{
    /// <summary>
    /// The controller for the player ship.
    /// </summary>
    [RequireComponent(typeof(Interactable))]
    public class ShipController : MonoBehaviour
    {
        /// <summary>
        /// Modes the ship can be in.
        /// </summary>
        public enum ShipMode
        {
            /// <summary>
            /// Ship can shoot and switch guns in Game mode. Laser Sight is purely for aiming.
            /// </summary>
            Game,

            /// <summary>
            /// Ship can't shoot in Menu mode. The Laser Sight acts as a UI interaction pointer.
            /// </summary>
            Menu
        }

        /// <summary>
        /// Structure containing a weapon script, damage, and fire rate for a weapon.
        /// </summary>
        [Serializable]
        public struct WeaponSetup
        {
            /// <summary>
            /// The Weapon script on the weapon's game object.
            /// </summary>
            public Weapon weapon;

            /// <summary>
            /// The damage this weapon does with each hit.
            /// </summary>
            public int damage;

            /// <summary>
            /// The number of times this weapon can fire per second.
            /// </summary>
            public float fireRate;
        }

        /// <summary>
        /// Whether or not the player can shoot right now.
        /// </summary>
        [Space(10)]
        private bool canShoot;

        /// <summary>
        /// Index of the currently activated weapon.
        /// </summary>
        public int currentWeapon;

        /// <summary>
        /// If we should wait until LateUpdate to update poses
        /// </summary>
        private bool deferNewPoses;

        /// <summary>
        /// The hand the ship is attached to
        /// </summary>
        [HideInInspector]
        public Hand hand;

        /// <summary>
        /// The Laser Sight on the ship. Reference to switch it between Game and Menu mode.
        /// </summary>
        private LaserSight laserSight;

        /// <summary>
        /// The deferred update position
        /// </summary>
        private Vector3 lateUpdatePos;

        /// <summary>
        /// The deferred update rotation
        /// </summary>
        private Quaternion lateUpdateRot;

        /// <summary>
        /// SteamVR event for applying deferred update poses.
        /// </summary>
        private SteamVR_Events.Action newPosesAppliedAction;

        /// <summary>
        /// Manager with all references we might need.
        /// </summary>
        private ReferenceManager reference;

        /// <summary>
        /// Which mode is the ship currently in.
        /// </summary>
        public ShipMode shipMode = ShipMode.Game;

        /// <summary>
        /// Array of weapons available to the player.
        /// </summary>
        [Reorderable("Weapon", false)]
        public WeaponSetup[] weapons;

        ///// <summary>
        ///// Array of the weapons available to the player.
        ///// </summary>
        //[Header("Weapon Attributes")]
        //[Reorderable("Weapon", false)]
        //public GameObject[] weapons;

        ///// <summary>
        ///// Array of the damage for each weapon.
        ///// </summary>
        //[Reorderable("Weapon", false)]
        //public int[] weaponDamage;

        ///// <summary>
        ///// Array of the fire rate for each weapon.
        ///// </summary>
        //[Reorderable("Weapon", false)]
        //public float[] weaponFireRate;

        /// <summary>
        /// Initializes references to Reference Manager and Laser Sight, sets this
        /// GameObject to the player ship reference in Reference Manager.
        /// </summary>
        private void Awake()
        {
            reference = GameObject.Find("Reference Manager").GetComponent<ReferenceManager>();

            if (reference == null)
            {
                Debug.LogError(GetType().Name + " - Could not find Reference Manager!");
            }
            else
            {
                reference.playerShip = gameObject;
            }

            //laserSight = transform.Find("Laser Sight").GetComponent<LaserSight>();
            laserSight = transform.GetComponentInChildren<LaserSight>();
            laserSight.SetMode(ShipMode.Game);
            newPosesAppliedAction = SteamVR_Events.NewPosesAppliedAction(OnNewPosesApplied);

			GameObject.Find("Main Canvas").transform.Find("Title").gameObject.SetActive(false);
            GameObject.Find("Main Canvas").transform.Find("Main Menu").gameObject.SetActive(true);

            for (int i = 0; i < weapons.Length; ++i)
            {
                weapons[i].weapon.Initialize(i);
            }
        }

        /// <summary>
        /// Sets the late update pose if we are deferring new poses
        /// </summary>
        private void LateUpdate()
        {
            if (deferNewPoses)
            {
                lateUpdatePos = transform.position;
                lateUpdateRot = transform.rotation;
            }
        }

        /// <summary>
        /// Called when the ship is picked up by a hand. Enables the menus.
        /// </summary>
        /// <param name="attachedHand"> The hand that picked up the ship </param>
        public void OnAttachedToHand(Hand attachedHand)
        {
            hand = attachedHand;

            if (reference.shipPickup)
            {
                reference.shipPickup.SetActive(false);
            }

            if(reference.menuTitle && reference.menuMain)
            {
                reference.menuTitle.SetActive(false);
                reference.menuMain.SetActive(true);
            }
            if (reference.powerUpStatus)
            {
                reference.powerUpStatus.BeginTracking();
            }
            if (reference.countdown)
            {
                reference.countdown.SetActive(true);
            }
        }

        /// <summary>
        /// Enables the new poses applied action
        /// </summary>
        private void OnEnable()
        {
            newPosesAppliedAction.enabled = true;
        }

        /// <summary>
        /// Disables the new poses applied action
        /// </summary>
        private void OnDisable()
        {
            newPosesAppliedAction.enabled = false;
        }

        /// <summary>
        /// Updates to the late update pose if we are deferring new poses
        /// </summary>
        private void OnNewPosesApplied()
        {
            if (deferNewPoses)
            {
                // Set object back to previous pose position to avoid jitter
                transform.position = lateUpdatePos;
                transform.rotation = lateUpdateRot;

                deferNewPoses = false;
            }
        }

        /// <summary>
        /// Checks if the ship is shooting or interacting with UI
        /// every frame it is attached to a hand.
        /// </summary>
        /// <param name="hand"> The attached hand </param>
        private void HandAttachedUpdate(Hand hand)
        {
            // Reset transform since we cheated it right after getting poses on previous frame
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            switch (shipMode)
            {
                case ShipMode.Game:
                    if (canShoot)
                    {
                        if (hand.GetStandardInteractionButton())
                        {
                            weapons[currentWeapon].weapon.TriggerUpdate();
                        }
                    }
                    else if (!canShoot && hand.GetStandardInteractionButtonUp())
                    {
                        canShoot = true;
                    }
                    break;
                case ShipMode.Menu:
                    if (hand.GetStandardInteractionButtonDown())
                    {
                        laserSight.TriggerUpdate(false);
                    }

                    else if (hand.GetStandardInteractionButton())
                    {
                        laserSight.TriggerUpdate(true);
                    }
                    break;
                default:
                    Debug.LogError(GetType().Name + " - ShipMode is not Menu or Game!");
                    break;
            }

            //// Update handedness guess
            //EvaluateHandedness();
        }

        /// <summary>
        /// Switches the currently activated gun to the next in the array
        /// </summary>
        private void SwitchGun()
        {
            int previous = currentWeapon++;
            if (currentWeapon >= weapons.Length)
            {
                currentWeapon = 0;
            }

            if (currentWeapon == previous)
            {
                return;
            }

            //weapons[previous].weapon.gameObject.SetActive(false);
            //weapons[currentWeapon].weapon.gameObject.SetActive(true);

            if (hand.GetStandardInteractionButton())
            {
                canShoot = false;
            }
        }

        /// <summary>
        /// Sets the ship to switch to either Game or Menu mode.
        /// </summary>
        /// <param name="mode"> The mode to switch the ship to </param>
        public void SetShipMode(ShipMode mode)
        {
            shipMode = mode;
            laserSight.SetMode(mode);
        }

        /// <summary>
        /// Sets the damage boost on all weapons
        /// </summary>
        /// <param name="boost"> The damage boost multiplier </param>
        public void SetDamageBoost(int boost)
        {
            foreach (WeaponSetup weaponSetup in weapons)
            {
                weaponSetup.weapon.damageMultiplier = boost;
            }
        }

        /// <summary>
        /// Deactivates the ship when the hand grabs another object.
        /// </summary>
        /// <param name="hand"> The attached hand </param>
        private void OnHandFocusLost(Hand hand)
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Enables the ship when the ship regains focus by the attached hand.
        /// </summary>
        /// <param name="hand"> The attached hand </param>
        private void OnHandFocusAcquired(Hand hand)
        {
            gameObject.SetActive(true);
            OnAttachedToHand(hand);
        }

        /// <summary>
        /// Destroys the ship when it is dropped.
        /// </summary>
        /// <param name="hand"> The detaching hand </param>
        private void OnDetachedFromHand(Hand hand)
        {
            Destroy(gameObject);
        }
    }
}