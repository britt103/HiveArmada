//=============================================================================
// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
// 
// [DESCRIPTION]
// 
//=============================================================================

using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using Hive.Armada.Game;
using Hive.Armada.Player.Guns;
using SubjectNerd.Utilities;

namespace Hive.Armada.Player
{
    /// <summary>
    /// The controller for the player ship.
    /// </summary>
    [RequireComponent(typeof(Interactable))]
    public class ShipController : MonoBehaviour
    {
        public enum ShipMode { Menu, Game }
        //public Handedness currentHandGuess = Handedness.Left;
        //private float timeOfPossibleHandSwitch = 0f;
        //private float timeBeforeConfirmingHandSwitch = 1.5f;
        //private bool possibleHandSwitch = false;
        //public enum Handedness { Left, Right };
        //public Transform pivotTransform;

        /// <summary>
        /// Manager with all references we might need.
        /// </summary>
        private ReferenceManager reference;

        /// <summary>
        /// Which mode is the ship currently in.
        /// </summary>
        public ShipMode shipMode;

        

        /// <summary>
        /// 
        /// </summary>
        private LaserSight laserSight;

        /// <summary>
        /// 
        /// </summary>
        [HideInInspector]
        public Hand hand;

        /// <summary>
        /// Array of the weapons available to the player.
        /// </summary>
        [Header("Weapon Attributes")]
        [Reorderable("Weapon", false)]
        public GameObject[] guns;

        /// <summary>
        /// Index of the currently activated weapon
        /// </summary>
        private int currGun;

        /// <summary>
        /// 
        /// </summary>
        [Reorderable("Weapon", false)]
        public int[] weaponDamage;

        /// <summary>
        /// 
        /// </summary>
        [Reorderable("Weapon", false)]
        public float[] weaponFireRate;

        [Space(10)]
        public SoundPlayOneshot engineSound;
        public GameObject deathExplosion;
        public bool canShoot;

        //--------------------
        // SteamVR
        //--------------------
        private bool deferNewPoses;
        private Vector3 lateUpdatePos;
        private Quaternion lateUpdateRot;
        private SteamVR_Events.Action newPosesAppliedAction;

        void Awake()
        {
            reference = GameObject.Find("Reference Manager").GetComponent<ReferenceManager>();

            if (reference == null)
                Debug.LogError(GetType().Name + " - Could not find Reference Manager!");

            reference.playerShip = gameObject;
            laserSight = transform.Find("Model").Find("Laser Sight").GetComponent<LaserSight>();
            newPosesAppliedAction = SteamVR_Events.NewPosesAppliedAction(OnNewPosesApplied);
        }

        private void OnAttachedToHand(Hand attachedHand)
        {
            hand = attachedHand;

            reference.shipPickup.SetActive(false);
            reference.menuTitle.SetActive(false);
            reference.menuMain.SetActive(true);
            reference.powerUpStatus.BeginTracking();
        }

        void OnEnable()
        {
            newPosesAppliedAction.enabled = true;
        }

        void OnDisable()
        {
            newPosesAppliedAction.enabled = false;
        }

        void LateUpdate()
        {
            if (deferNewPoses)
            {
                lateUpdatePos = transform.position;
                lateUpdateRot = transform.rotation;
            }
        }

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
                            guns[currGun].SendMessage("TriggerUpdate");
                        }
                    }
                    else if (!canShoot && hand.GetStandardInteractionButtonUp())
                    {
                        canShoot = true;
                    }

                    // Switch guns
                    if (!hand.GetStandardInteractionButton() && hand.controller.GetPressDown(EVRButtonId.k_EButton_Grip))
                    {
                        SwitchGun(currGun);
                    }

                    break;
                case ShipMode.Menu:
                    if (hand.GetStandardInteractionButtonDown())
                    {
                        laserSight.TriggerUpdate();
                    }
                    break;
                default:
                    Debug.LogError("ShipController - ShipMode is not Menu or Game!");
                    break;
            }

            //// Update handedness guess
            //EvaluateHandedness();
        }

        private void SwitchGun(int previous)
        {
            ++currGun;
            if (currGun >= guns.Length)
            {
                currGun = 0;
            }

            if (currGun == previous)
                return;

            if (guns[currGun].GetComponent<Minigun>())
                guns[currGun].GetComponent<Minigun>().ResetTracers();

            if (guns[previous].GetComponent<Minigun>())
                guns[previous].GetComponent<Minigun>().ResetTracers();

            guns[previous].SetActive(false);
            guns[currGun].SetActive(true);

            if (hand.GetStandardInteractionButton())
                canShoot = false;
        }



        /// <summary>
        /// Sets the ship to switch to either Game or Menu mode.
        /// </summary>
        /// <param name="mode"> The mode to switch the ship to </param>
        public void SetShipMode(ShipMode mode)
        {
            shipMode = mode;
        }

        /// <summary>
        /// Sets the damage boost on all weapons
        /// </summary>
        /// <param name="boost"> The damage boost multiplier </param>
        public void SetDamageBoost(int boost)
        {
            foreach (GameObject obj in guns)
            {
                if (obj.GetComponent<Gun>())
                {
                    obj.GetComponent<Gun>().damageBoost = boost;
                }
            }
        }

        //private void EvaluateHandedness()
        //{
        //    Hand.HandType handType = hand.GuessCurrentHandType();

        //    if (handType == Hand.HandType.Left)// Bow hand is further left than arrow hand.
        //    {
        //        // We were considering a switch, but the current controller orientation matches our currently assigned handedness, so no longer consider a switch
        //        if (possibleHandSwitch && currentHandGuess == Handedness.Left)
        //        {
        //            possibleHandSwitch = false;
        //        }

        //        // If we previously thought the bow was right-handed, and were not already considering switching, start considering a switch
        //        if (!possibleHandSwitch && currentHandGuess == Handedness.Right)
        //        {
        //            possibleHandSwitch = true;
        //            timeOfPossibleHandSwitch = Time.time;
        //        }

        //        // If we are considering a handedness switch, and it's been this way long enough, switch
        //        if (possibleHandSwitch && Time.time > (timeOfPossibleHandSwitch + timeBeforeConfirmingHandSwitch))
        //        {
        //            currentHandGuess = Handedness.Left;
        //            possibleHandSwitch = false;
        //        }
        //    }
        //    else // Bow hand is further right than arrow hand
        //    {
        //        // We were considering a switch, but the current controller orientation matches our currently assigned handedness, so no longer consider a switch
        //        if (possibleHandSwitch && currentHandGuess == Handedness.Right)
        //        {
        //            possibleHandSwitch = false;
        //        }

        //        // If we previously thought the bow was right-handed, and were not already considering switching, start considering a switch
        //        if (!possibleHandSwitch && currentHandGuess == Handedness.Left)
        //        {
        //            possibleHandSwitch = true;
        //            timeOfPossibleHandSwitch = Time.time;
        //        }

        //        // If we are considering a handedness switch, and it's been this way long enough, switch
        //        if (possibleHandSwitch && Time.time > (timeOfPossibleHandSwitch + timeBeforeConfirmingHandSwitch))
        //        {
        //            currentHandGuess = Handedness.Right;
        //            possibleHandSwitch = false;
        //        }
        //    }
        //}

        //private void DoHandednessCheck()
        //{
        //    // Based on our current best guess about hand, switch bow orientation and arrow lerp direction
        //    if (currentHandGuess == Handedness.Left)
        //    {
        //        pivotTransform.localScale = new Vector3(1f, 1f, 1f);
        //    }
        //    else
        //    {
        //        pivotTransform.localScale = new Vector3(1f, -1f, 1f);
        //    }
        //}

        private void ShutDown()
        {
            //hand.DetachObject(gameObject);
            //if (hand != null && hand.otherHand.currentAttachedObject != null)
            //{
            //    if (hand.otherHand.currentAttachedObject.GetComponent<ItemPackageReference>() != null)
            //    {
            //        if (hand.otherHand.currentAttachedObject.GetComponent<ItemPackageReference>().itemPackage == arrowHandItemPackage)
            //        {
            //            hand.otherHand.DetachObject(hand.otherHand.currentAttachedObject);
            //        }
            //    }
            //}
        }

        private void OnHandFocusLost(Hand hand)
        {
            gameObject.SetActive(false);
        }

        private void OnHandFocusAcquired(Hand hand)
        {
            gameObject.SetActive(true);
            OnAttachedToHand(hand);
        }

        private void OnDetachedFromHand(Hand hand)
        {
            Destroy(gameObject);
        }

        void OnDestroy()
        {
            ShutDown();
        }
    }
}
