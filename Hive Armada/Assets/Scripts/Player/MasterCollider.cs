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
using Hive.Armada.Enemies;
using UnityEngine;
using Hive.Armada.Game;

namespace Hive.Armada.Player
{
    public class MasterCollider : MonoBehaviour
    {
        /// <summary>
        /// Reference manager that holds all needed references
        /// (e.g. spawner, game manager, etc.)
        /// </summary>
        private ReferenceManager reference;

        /// <summary>
        /// Renderer for the shield
        /// </summary>
        public Renderer shieldRenderer;

        public Collider shieldCollider;

        /// <summary>
        /// Coroutine for the shield timer.
        /// </summary>
        private Coroutine shieldCoroutine;

        /// <summary>
        /// How long the shield lasts.
        /// </summary>
        [Header("Shield Attributes")]
        public float shieldDuration;

        /// <summary>
        /// How long the initial shield flash and wait will be.
        /// The shield flashes 5 times before its duration is up.
        /// </summary>
        [Tooltip("The duration of the intial shield flash and wait.")]
        public float shieldFlashDuration;

        /// <summary>
        /// How much time to take off of the flash duration for each flash.
        /// </summary>
        [Tooltip("How fast the shield flash speed increases by.")]
        public float shieldFlashAcceleration;

        public float shieldMinAlpha;

        public float shieldMaxAlpha;

        private float shieldAlphaStep;

        private const int steps = 30;

        /// <summary>
        /// How long until the first shield flash.
        /// </summary>
        private float shieldFlashTimer;

        /// <summary>
        /// If the shield is active.
        /// </summary>
        public bool ShieldActive { get; private set; }

        /// <summary>
        /// Emitter for projectiles hitting the shield.
        /// </summary>
        [Header("Emitters")]
        [Tooltip("This emitter is used when projectiles hit the shield.")]
        public GameObject shieldHitEmitter;

        /// <summary>
        /// The type identifier for the shield hit emitter
        /// </summary>
        private short hitEmitterTypeIdentifier;

        /// <summary>
        /// Initializes the reference to the Reference Manager
        /// </summary>
        private void Awake()
        {
            reference = GameObject.Find("Reference Manager").GetComponent<ReferenceManager>();
            shieldRenderer.material.SetFloat("_Alpha", 0.0f);
            shieldRenderer.enabled = false;

            float totalFlashTime = 0.0f;
            float shieldFlashTime = shieldFlashDuration;

            for (int i = 0; i < 5; ++i)
            {
                totalFlashTime += shieldFlashTime * 2;
                shieldFlashTime *= shieldFlashAcceleration;
            }

            shieldAlphaStep = (shieldMaxAlpha - shieldMinAlpha) / steps;

            shieldFlashTimer = shieldDuration - totalFlashTime;

            if (shieldHitEmitter != null)
            {
                hitEmitterTypeIdentifier =
                    reference.objectPoolManager.GetTypeIdentifier(shieldHitEmitter);
            }
            else
            {
                Debug.LogError(GetType().Name + " - shieldHitEmitter is not set.");
            }
        }

        /// <summary>
        /// Runs when projectiles or the kamikaze enters the collider.
        /// </summary>
        /// <param name="other"> The other collider </param>
        private void OnTriggerEnter(Collider other)
        {
            KamikazeTurret kamikaze = other.gameObject.GetComponent<KamikazeTurret>();

            if (!ShieldActive)
            {
                if (kamikaze != null)
                {
                    Debug.Log("Kamikaze isn't null");
                    kamikaze.NearPlayer();
                }
                Debug.Log("Kamikaze was null");
                return;
            }

            if (!other.CompareTag("Projectile") && !other.CompareTag("Enemy"))
            {
                return;
            }

            if (kamikaze)
            {
                kamikaze.Hit(1000, false);

                return;
            }

            if (hitEmitterTypeIdentifier >= 0)
            {
                reference.objectPoolManager.Spawn(gameObject, hitEmitterTypeIdentifier,
                                                  other.transform.position,
                                                  other.transform.rotation);
                reference.objectPoolManager.Despawn(other.gameObject);
            }
            else
            {
                Debug.LogError(GetType().Name + " - shieldHitEmitter is not pooled.");
            }
        }

        /// <summary>
        /// Activates the shield. If it is already active, reset the timer for it.
        /// </summary>
        public void ActivateShield()
        {
            if (!ShieldActive)
            {
                ShieldActive = true;

                if (shieldCoroutine == null)
                {
                    shieldCoroutine = StartCoroutine(ShieldCountdown());
                }
                else
                {
                    StopCoroutine(shieldCoroutine);
                }
            }
        }

        /// <summary>
        /// Counts down the shield and flashes it 5 times at the end.
        /// </summary>
        private IEnumerator ShieldCountdown()
        {
            //foreach (Renderer r in barrelRenderers)
            //{
            //    r.material.SetColor("_overheatColor", overheatBarrelColor);
            //    r.material.SetFloat("_overheatPercent", 0.0f);
            //}
            shieldRenderer.enabled = true;
            shieldRenderer.material.SetFloat("_Alpha", shieldMaxAlpha);

            yield return new WaitForSeconds(shieldFlashTimer);

            float shieldFlashTime = shieldFlashDuration;
            float finalAlphaStep = shieldMaxAlpha / steps;
            float stepTime;
            float currentAlpha = shieldMaxAlpha;

            for (int i = 0; i < 5; ++i)
            {
                stepTime = shieldFlashTime / steps;

                for (int s = 0; s < steps; ++s)
                {
                    currentAlpha = Mathf.Clamp(currentAlpha - shieldAlphaStep, shieldMinAlpha, shieldMaxAlpha);
                    shieldRenderer.material.SetFloat("_Alpha", currentAlpha);
                    yield return new WaitForSeconds(stepTime);
                }

                //yield return null;

                for (int s = 0; s < steps; ++s)
                {
                    currentAlpha = Mathf.Clamp(currentAlpha + shieldAlphaStep, shieldMinAlpha, shieldMaxAlpha);
                    shieldRenderer.material.SetFloat("_Alpha", currentAlpha);
                    yield return new WaitForSeconds(stepTime);
                }

                //yield return null;

                //shieldRenderer.enabled = false;
                //yield return new WaitForSeconds(shieldFlashTime);

                //shieldRenderer.enabled = true;
                //yield return new WaitForSeconds(shieldFlashTime);

                shieldFlashTime *= shieldFlashAcceleration;
            }

            shieldFlashTime /= 2.0f;
            stepTime = shieldFlashTime / steps;

            for (int s = 0; s < steps; ++s)
            {
                currentAlpha = Mathf.Clamp(currentAlpha - finalAlphaStep, 0.0f, shieldMaxAlpha);
                shieldRenderer.material.SetFloat("_Alpha", currentAlpha);
                yield return new WaitForSeconds(stepTime);
            }

            shieldRenderer.enabled = false;
            ShieldActive = false;
            //shieldRenderer.enabled = false;
            //shieldRenderer.material.SetFloat("_Alpha", 0.0f);
            shieldCoroutine = null;

            yield return null;
        }
    }
}