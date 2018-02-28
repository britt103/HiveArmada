//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.champan.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// Shootable is an abstract class and a base for non-enemy objects that can be
// hit by the player.
//
//=============================================================================

using System.Collections;
using UnityEngine;

namespace Hive.Armada
{
    /// <summary>
    /// Abstract shootable class.
    /// </summary>
    public abstract class Shootable : MonoBehaviour
    {
        /// <summary>
        /// State of whether game object can be shot by player.
        /// </summary>
        public bool isShootable;

        /// <summary>
        /// Delay before object can be shot.
        /// </summary>
        public float shootableDelay = 0.0f;

        /// <summary>
        /// Emitter on shootable spawn.
        /// </summary>
        public GameObject spawnEmitter;

        /// <summary>
        /// Emitter on shootable death.
        /// </summary>
        public GameObject deathEmitter;

        /// <summary>
        /// Instantiate spawn fx. Start delay coroutine.
        /// </summary>
        protected virtual void Awake()
        {
            if (spawnEmitter != null)
            {
                Instantiate(spawnEmitter, transform.position, transform.rotation, transform);
            }
            StartCoroutine(DelayShootable());
        }

        /// <summary>
        /// Make object shootable after delay time.
        /// </summary>
        /// <returns></returns>
        private IEnumerator DelayShootable()
        {
            yield return new WaitForSeconds(shootableDelay);
            isShootable = true;
        }

        /// <summary>
        /// Spawns the death emitter and destroys the object.
        /// </summary>
        public virtual void Hit()
        {
            if (deathEmitter != null)
            {
                Instantiate(deathEmitter, transform.position, transform.rotation);
            }
            Destroy(gameObject);
        }
    }
}
