//=============================================================================
// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
// 
// This class handles all pooled particle emitters such as hit sparks and enemy
// death effects. It stops all particle systems and clears all active particles
// in Reset(), OnDisable(), and on the onParticleSystemsDeadEvent. The particle
// systems run automatically when the object is enabled.
// 
//=============================================================================

using System.Collections;
using UnityEngine;
using MirzaBeig.ParticleSystems;

namespace Hive.Armada.Game
{
    /// <summary>
    /// Pooled particle systems emitters class.
    /// </summary>
    [RequireComponent(typeof(ParticleSystems))]
    public class Emitter : Poolable
    {
        /// <summary>
        /// The particle systems script on this emitter.
        /// </summary>
        private ParticleSystems system;

        /// <summary>
        /// If true, plays a sound from clips.
        /// </summary>
        public bool playSound;

        /// <summary>
        /// Audio source to play the audio clips with.
        /// </summary>
        public AudioSource source;

        /// <summary>
        /// Sound clips to play from if playSound is true.
        /// </summary>
        public AudioClip[] clips;

        /// <summary>
        /// Initialize reference manager reference and system
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            system = GetComponent<ParticleSystems>();

            if (system != null)
            {
                system.onParticleSystemsDeadEvent += OnParticleSystemsDead;
            }
        }

        /// <summary>
        /// Plays a random sound from clips when this emitter is enabled if playSound is true.
        /// </summary>
        private void OnEnable()
        {
            if (!playSound)
            {
                return;
            }

            int sound = Random.Range(0, clips.Length);
            source.PlayOneShot(clips[sound]);
        }

        /// <summary>
        /// Stops and clears particles when they all finish. Despawns this object with the pool.
        /// </summary>
        private void OnParticleSystemsDead()
        {
            if (system != null)
            {
                system.stop();
                system.clear();
            }

            StartCoroutine(WaitForAudio());
        }

        private IEnumerator WaitForAudio()
        {
            if (source != null)
            {
                yield return new WaitWhile(() => source.isPlaying);
            }
            else
            {
                yield return null;
            }

            reference.objectPoolManager.Despawn(gameObject);
        }

        public void PlaySound(AudioClip clip)
        {
            source.PlayOneShot(clip);
        }

        /// <summary>
        /// Stops the particle systems and clears all existing particles.
        /// </summary>
        protected override void Reset()
        {
            if (system != null)
            {
                system.stop();
                system.clear();
            }
        }
    }
}

