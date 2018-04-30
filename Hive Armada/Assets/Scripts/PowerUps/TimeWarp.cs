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
    /// Time warp powerup
    /// </summary>
    public class TimeWarp : MonoBehaviour
    {
        /// <summary>
        /// Reference manager that holds all needed references
        /// (e.g. spawner, game manager, etc.)
        /// </summary>
        private ReferenceManager reference;

        /// <summary>
        /// Length of the warp after the transition, in seconds.
        /// </summary>
        public float warpLength;

        /// <summary>
        /// How long it takes for the warp to take full effect, in seconds.
        /// </summary>
        public float transitionLength;

        /// <summary>
        /// How strong the warp is. [0.0, 1.0]
        /// Strength of 1.0 results in a velocity of 0 for projectiles.
        /// </summary>
        public float strength;

        /// <summary>
        /// The particle emitter to play when the boost is activated.
        /// </summary>
        public GameObject spawnEmitter;

        AudioSource source;

        AudioSource bossSource;

        public AudioClip clip;

        /// <summary>
        /// Spawns the spawn particle emitter and runs the damage boost.
        /// </summary>
        private void Start()
        {
            reference = GameObject.Find("Reference Manager").GetComponent<ReferenceManager>();
            source = GameObject.Find("Powerup Audio Source").GetComponent<AudioSource>();
            bossSource = GameObject.Find("Boss Audio Source").GetComponent<AudioSource>();
            reference.dialoguePlayer.EnqueueFeedback(clip);

            Instantiate(spawnEmitter, reference.playerShip.transform);
            StartCoroutine(PlayBossClip());
            StartCoroutine(Run());
        }

        private IEnumerator PlayBossClip()
        {
            yield return new WaitForSeconds(2.4f);
            reference.bossManager.PlayTimeWarp();
        }

        /// <summary>
        /// Applies the damage boost for boostLength seconds and then resets it back to 1.
        /// </summary>
        private IEnumerator Run()
        {
            reference.enemyAttributes.StartTimeWarp();

            yield return new WaitForSeconds(transitionLength);

            yield return new WaitForSeconds(warpLength);

            reference.enemyAttributes.StopTimeWarp();
            
            Destroy(gameObject);
        }
    }
}