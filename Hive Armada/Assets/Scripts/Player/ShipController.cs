//=============================================================================
// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-440-01, CPSC-340-01 & CPSC-344-01
// Group Project
// 
// This class handles the ship that the player picks up. It includes all
// functions needed for SteamVR's Player prefab to interact with it. It handles
// firing and switching weapons.
// 
//=============================================================================

using System;
using System.Collections;
using Hive.Armada.Data;
using Hive.Armada.Game;
using Hive.Armada.Player.Weapons;
using SubjectNerd.Utilities;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace Hive.Armada.Player
{
    /// <summary>
    /// The controller for the player ship.
    /// </summary>
    [RequireComponent(typeof(Interactable))]
    public class ShipController : MonoBehaviour
    {
        public PlayerData playerData;

        /// <summary>
        /// The master collider used for the shield and Kamikaze proximity detonation.
        /// </summary>
        public MasterCollider masterCollider;

        /// <summary>
        /// Index of the currently activated weapon.
        /// </summary>
        private int currentWeapon;

        /// <summary>
        /// Prevents the weapon from firing when the player
        /// grabs the ship until they release the trigger.
        /// </summary>
        private bool canShoot;

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
        /// Array of weapons available to the player.
        /// </summary>
        [Space]
        [Reorderable("Weapon", false)]
        public Weapon[] weapons;

        /// <summary>
        /// Audio source for ship sounds.
        /// </summary>
        [Header("Audio")]
        public AudioSource source;

        [Header("Renderers")]
        public Renderer[] body;

        public Renderer[] minigunNoOverheat;

        public Renderer[] minigunOverheat;

        public Renderer[] rocketPod;
        
        ///// <summary>
        ///// Helper dialogue that plays when the ship is grabbed.
        ///// </summary>
        //private AudioClip[] startClips;

        ///// <summary>
        ///// Helper dialogue that tells the player which weapon they have.
        ///// </summary>
        //private AudioClip[] weaponStartClips;

        /// <summary>
        /// Initializes references to Reference Manager and Laser Sight, sets this
        /// GameObject to the player ship reference in Reference Manager.
        /// </summary>
        private void Awake()
        {
            reference = FindObjectOfType<ReferenceManager>();
            newPosesAppliedAction = SteamVR_Events.NewPosesAppliedAction(OnNewPosesApplied);

            if (reference == null)
            {
                Debug.LogError(GetType().Name + " - Could not find Reference Manager!");
            }
            else
            {
                reference.playerShip = gameObject;
                reference.playerShipSource = source;

                reference.tooltips.ShipGrabbed();

                SetWeapon((int) reference.gameSettings.selectedWeapon);

                if (reference.shipPickup)
                {
                    reference.shipPickup.SetActive(false);
                }

                if (reference.powerUpStatus)
                {
                    reference.powerUpStatus.BeginTracking(reference, hand);
                }

                if (reference.gameSettings.selectedGameMode == GameSettings.GameMode.SoloNormal)
                {
                    if (reference.waveManager != null)
                    {
                        reference.waveManager.Run();
                    }
                    else
                    {
                        Debug.LogError(GetType().Name +
                                       " - Reference.WaveManager is null. Cannot call Run().");
                    }
                }
                else
                {
                    if (reference.countdown)
                    {
                        reference.countdown.SetActive(true);
                    }
                    else
                    {
                        Debug.LogError(
                            GetType().Name + " - Reference.Countdown is null. Cannot enable.");
                    }
                }

                UpdateSkin(reference.gameSettings.selectedSkin);

                StartCoroutine(IntroAudio());
                reference.tooltips.SpawnProtectShip();
            }
        }

        private void UpdateSkin(int skin)
        {
            // if (skin == 0)
            //     return;

            foreach (Renderer r in body)
            {
                r.material = playerData.shipBodyMaterials[skin];
            }
            
            foreach (Renderer r in minigunNoOverheat)
            {
                r.material = playerData.shipMinigunMaterials[skin];
            }
            
            foreach (Renderer r in minigunOverheat)
            {
                r.material = playerData.shipMinigunOverheatMaterials[skin];
            }
            
            foreach (Renderer r in rocketPod)
            {
                r.material = playerData.shipRocketPodsMaterials[skin];
            }
        }

        private IEnumerator IntroAudio()
        {
            yield return new WaitForSeconds(2.0f);
            yield return new WaitWhile(() => reference.bossManager.IsSpeaking);
            yield return new WaitForSeconds(2.0f);

            AudioClip[] introAudio = new AudioClip[playerData.shipIntroClips.Length + 1];

            for (int i = 0; i < playerData.shipIntroClips.Length; ++i)
            {
                introAudio[i] = playerData.shipIntroClips[i];
            }

            introAudio[introAudio.Length - 1] = playerData.shipWeaponIntroClips[currentWeapon];

            reference.dialoguePlayer.EnqueueDialogue(gameObject, introAudio);
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

            if (canShoot)
            {
                if (hand.GetStandardInteractionButton())
                {
                    weapons[currentWeapon].TriggerUpdate();
                }
            }
            else
            {
                if (hand.GetStandardInteractionButtonUp())
                {
                    canShoot = true;
                }
            }
        }

        /// <summary>
        /// Activates the corresponding weapon object.
        /// </summary>
        /// <param name="weaponNumber"> Index of the weapon to activate </param>
        private void SetWeapon(int weaponNumber)
        {
            weapons[weaponNumber].gameObject.SetActive(true);
            weapons[weaponNumber].Initialize(weaponNumber);

            currentWeapon = weaponNumber;
        }

        /// <summary>
        /// Sets the damage boost on all weapons
        /// </summary>
        /// <param name="boost"> The damage boost multiplier </param>
        public void SetDamageBoost(int boost)
        {
            weapons[currentWeapon].damageMultiplier = boost;
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