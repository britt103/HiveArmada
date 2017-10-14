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

using System.Collections;
using System.Collections.Generic;
<<<<<<< HEAD
using System.Security.Cryptography;
=======
>>>>>>> feature/powerups
using UnityEngine;
using Valve.VR.InteractionSystem;
using GameName.Player.Guns;

namespace GameName.Player
{
    [RequireComponent(typeof(Interactable))]
    public class ShipControllerNew : MonoBehaviour
    {
        //public enum Handedness { Left, Right };
        public enum GunTypes { Lasers, Miniguns, Railguns, Launchers };

        //public Handedness currentHandGuess = Handedness.Left;
        //private float timeOfPossibleHandSwitch = 0f;
        //private float timeBeforeConfirmingHandSwitch = 1.5f;
        //private bool possibleHandSwitch = false;

        public GameObject lasers;
        private LaserGun laserGun;
        public Transform pivotTransform;
<<<<<<< HEAD
        public Hand hand { get; private set; }
=======
        private Hand hand;
>>>>>>> feature/powerups

        public GunTypes currentGun = GunTypes.Lasers;

        // Gun base stats
        public const int LASER_BASE_DAMAGE = 10;
        public const float LASER_BASE_FIRE_RATE = 10.0f;
        public const int MINIGUN_BASE_DAMAGE = 1;
        public const float MINIGUN_BASE_FIRE_RATE = 110.0f;
        public const int RAILGUN_BASE_DAMAGE = 1;
        public const float RAILGUN_BASE_FIRE_RATE = 1.0f;
        public const int LAUNCHER_BASE_DAMAGE = 1;
        public const float LAUNCHER_BASE_FIRE_RATE = 1.0f;

        // Gun current stats
        public int laserDamage;
        public float laserFireRate;
        public int minigunDamage;
        public float minigunFireRate;
        public int railgunDamage;
        public float railgunFireRate;
        public int launcherDamage;
        public float launcherFireRate;
        
        private bool deferNewPoses = false;
        private Vector3 lateUpdatePos;
        private Quaternion lateUpdateRot;

        SteamVR_Events.Action newPosesAppliedAction;

        public SoundPlayOneshot engineSound;
        public GameObject deathExplosion;
        public bool canShoot = false;

<<<<<<< HEAD
        private void OnAttachedToHand(Hand attachedHand)
        {
            Debug.Log("HOLY SHIT I ATTACHED");
            hand = attachedHand;
=======
        //player stats
        private PlayerStats stats;

        private void OnAttachedToHand(Hand attachedHand)
        {
            hand = attachedHand;

            //player stats
            stats = gameObject.GetComponentInParent<PlayerStats>();
>>>>>>> feature/powerups
        }

        void Awake()
        {
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

            if (hand.GetStandardInteractionButton())
            {
                laserGun.TriggerUpdate();
<<<<<<< HEAD
=======

                //player stats
                stats.isFiring = true;
>>>>>>> feature/powerups
            }
            

            //// Update handedness guess
            //EvaluateHandedness();


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
