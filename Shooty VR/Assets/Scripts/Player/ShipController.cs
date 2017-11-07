﻿//=============================================================================
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

using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Hive.Armada.Player.Guns;
using Valve.VR;

namespace Hive.Armada.Player
{
    [RequireComponent(typeof(Interactable))]
    public class ShipController : MonoBehaviour
    {
        //public enum Handedness { Left, Right };
        public enum ShipMode { Menu, Game };
        public enum GunTypes { Lasers, Miniguns, Railguns, Launchers };

        //public Handedness currentHandGuess = Handedness.Left;
        //private float timeOfPossibleHandSwitch = 0f;
        //private float timeBeforeConfirmingHandSwitch = 1.5f;
        //private bool possibleHandSwitch = false;

        public ShipMode shipMode;

        public GameObject[] guns;
        private int currGun;

        public LaserSight laserSight;
        public GameObject lasers;
        private LaserGun laserGun;
        public Minigun minigun;
        public Transform pivotTransform;
        public Hand hand { get; private set; }

        public GunTypes currentGun = GunTypes.Lasers;

        //// Gun base stats
        //public const int LASER_BASE_DAMAGE = 10;
        //public const float LASER_BASE_FIRE_RATE = 10.0f;
        //public const int MINIGUN_BASE_DAMAGE = 1;
        //public const float MINIGUN_BASE_FIRE_RATE = 110.0f;
        //public const int RAILGUN_BASE_DAMAGE = 1;
        //public const float RAILGUN_BASE_FIRE_RATE = 1.0f;
        //public const int LAUNCHER_BASE_DAMAGE = 1;
        //public const float LAUNCHER_BASE_FIRE_RATE = 1.0f;

        // Gun current stats
        public int laserDamage;
        public float laserFireRate;
        public int minigunDamage;
        public float minigunFireRate;
        public int railgunDamage;
        public float railgunFireRate;
        public int launcherDamage;
        public float launcherFireRate;

        private bool deferNewPoses;
        private Vector3 lateUpdatePos;
        private Quaternion lateUpdateRot;

        private SteamVR_Events.Action newPosesAppliedAction;

        public SoundPlayOneshot engineSound;
        public GameObject deathExplosion;
        public bool canShoot;

        private void OnAttachedToHand(Hand attachedHand)
        {
            hand = attachedHand;

            GameObject pickup = GameObject.FindGameObjectWithTag("ShipPickup");

            if (pickup)
            {
                pickup.SetActive(false);
            }

            GameObject.Find("Main Canvas").transform.Find("Title").gameObject.SetActive(false);
            GameObject.Find("Main Canvas").transform.Find("Main Menu").gameObject.SetActive(true);

            FindObjectOfType<PowerUpStatus>().BeginTracking();
        }

        void Awake()
        {
            //laserDamage = LASER_BASE_DAMAGE;
            //laserFireRate = LASER_BASE_FIRE_RATE;
            //minigunDamage = MINIGUN_BASE_DAMAGE;
            //minigunFireRate = MINIGUN_BASE_FIRE_RATE;
            //railgunDamage = RAILGUN_BASE_DAMAGE;
            //railgunFireRate = RAILGUN_BASE_FIRE_RATE;
            //launcherDamage = LAUNCHER_BASE_DAMAGE;
            //launcherFireRate = LAUNCHER_BASE_FIRE_RATE;

            if (guns.Length > 0)
            {
                currGun = 0;
            }

            newPosesAppliedAction = SteamVR_Events.NewPosesAppliedAction(OnNewPosesApplied);
            laserGun = lasers.GetComponentInChildren<LaserGun>();
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

            if (shipMode.Equals(ShipMode.Game))
            {
                if (canShoot)
                {
                    if (hand.GetStandardInteractionButton())
                    {
                        guns[currGun].SendMessage("TriggerUpdate");
                        //switch (currentGun)
                        //{
                        //    case GunTypes.Lasers:
                        //        laserGun.TriggerUpdate();
                        //        break;
                        //    case GunTypes.Miniguns:
                        //        minigun.TriggerUpdate();
                        //        break;
                        //}
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

                //press menu button
                if (hand.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_ApplicationMenu))
                {
                    GameObject.Find("Main Canvas").transform.Find("Paused Menu").gameObject.SetActive(
                        !GameObject.Find("Main Canvas").transform.Find("Paused Menu").gameObject.activeSelf);
                }
            }
            else if (shipMode.Equals(ShipMode.Menu))
            {
                if (hand.GetStandardInteractionButtonDown())
                {
                    laserSight.TriggerUpdate();
                }
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
            laserGun.damageBoost = boost;
            minigun.damageBoost = boost;
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
