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

        public AudioClip clip;

        /// <summary>
        /// Runs the master collider shield
        /// </summary>
        private void Awake()
        {
            reference = GameObject.Find("Reference Manager").GetComponent<ReferenceManager>();
            reference.dialoguePlayer.EnqueueFeedback(clip);

            reference.playerShip.GetComponent<ShipController>().masterCollider.ActivateShield();

            StartCoroutine(PlayBossClip());
        }

        private IEnumerator PlayBossClip()
        {
            yield return new WaitForSeconds(2.1f);
            reference.bossManager.PlayShield();

            Destroy(gameObject);
        }
    }
}