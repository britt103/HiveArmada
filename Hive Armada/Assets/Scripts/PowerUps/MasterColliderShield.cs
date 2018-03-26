//=============================================================================
//
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
// 
// 
// 
//=============================================================================

using System.Collections;
using UnityEngine;
using Hive.Armada.Game;
using Hive.Armada.Player;

namespace Hive.Armada.PowerUps
{
    /// <summary>
    /// Damage boost powerup
    /// </summary>
    public class MasterColliderShield : MonoBehaviour
    {
        /// <summary>
        /// Reference manager that holds all needed references
        /// (e.g. spawner, game manager, etc.)
        /// </summary>
        private ReferenceManager reference;

        AudioSource source;

        AudioSource bossSource;

        public AudioClip clip;

        /// <summary>
        /// Runs the master collider shield
        /// </summary>
        private void Awake()
        {
            reference = GameObject.Find("Reference Manager").GetComponent<ReferenceManager>();
            source = GameObject.Find("Powerup Audio Source").GetComponent<AudioSource>();
            bossSource = GameObject.Find("Boss Audio Source").GetComponent<AudioSource>();
            StartCoroutine(pauseForBoss());

            reference.playerShip.GetComponent<ShipController>().masterCollider.ActivateShield();

            Destroy(gameObject);
        }

        IEnumerator pauseForBoss()
        {
            if (bossSource.isPlaying)
            {
                yield return new WaitWhile(() => bossSource.isPlaying);

                if (source.isPlaying)
                {
                    yield return new WaitWhile(() => source.isPlaying);
                }

                if (!source.isPlaying)
                {
                    source.PlayOneShot(clip);
                }
            }
            else if (!bossSource.isPlaying)
            {
                source.PlayOneShot(clip);
            }
        }
    }
}