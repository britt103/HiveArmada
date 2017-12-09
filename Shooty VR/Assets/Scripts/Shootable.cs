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

using UnityEngine;

namespace Hive.Armada
{
    public abstract class Shootable : MonoBehaviour
    {
        /// <summary>
        /// State of whether game object can be shot by player.
        /// </summary>
        public bool isShootable = true;

        /// <summary>
        /// FX to be instantiated when spawned.
        /// </summary>
        public GameObject spawnEmitter;

        /// <summary>
        /// FX to be instantiated when hit.
        /// </summary>
        public GameObject shotEmitter;

        /// <summary>
        /// Instantiate spawn fx.
        /// </summary>
        protected virtual void Awake()
        {
            Instantiate(spawnEmitter, transform.position, transform.rotation, transform);
        }

        /// <summary>
        /// Instantiate hit fx. Self-destruct.
        /// </summary>
        public virtual void Shot()
        {
            Instantiate(shotEmitter, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
