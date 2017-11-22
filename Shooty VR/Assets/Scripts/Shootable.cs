//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.champan.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// Shootable is an abstract class and a base for non-enemy object that can be
// hit by the player.
//
//=============================================================================

using UnityEngine;

namespace Hive.Armada
{
    public abstract class Shootable : MonoBehaviour
    {
        /// <summary>
        /// FX to be instantiated when spawned.
        /// </summary>
        public GameObject fxSpawn;

        /// <summary>
        /// FX to be instantiated when hit.
        /// </summary>
        public GameObject fxHit;

        /// <summary>
        /// Instantiate spawn fx.
        /// </summary>
        public virtual void Awake()
        {
            Instantiate(fxSpawn, transform.position, transform.rotation, transform);
        }

        /// <summary>
        /// Instantiate hit fx. Self-destruct.
        /// </summary>
        public virtual void Hit()
        {
            Instantiate(fxHit, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
