//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-440-1
// Group Project
//
// IridiumShootable inherits from Shootable. When the player shoots this
// object, Iridium is added to IridiumSystem. The object self destructs after
// a given time.
//
//=============================================================================

using System.Collections;
using UnityEngine;

namespace Hive.Armada.Game
{
    /// <summary>
    /// Shootable that awards Iridium.
    /// </summary>
    public class IridiumShootable : Shootable
    {
        /// <summary>
        /// Amount of Iridium earned when shot by player.
        /// </summary>
        public int iridiumValue;

        /// <summary>
        /// Reference to Iridium shootable emitter.
        /// </summary>
        public GameObject emitter;

        /// <summary>
        /// Number of shots required to destroy object.
        /// </summary>
        public int health;

        /// <summary>
        /// Time until object self destructs.
        /// </summary>
        public float selfDestructTime;

        /// <summary>
        /// Refernce to Referencemanager.
        /// </summary>
        private ReferenceManager reference;

        /// <summary>
        /// Reference to IridiumSystem.
        /// </summary>
        private IridiumSystem iridiumSystem;

        /// <summary>
        /// Start self destruct coroutine.
        /// </summary>
        void Start()
        {
            StartCoroutine(SelfDestruct());
        }

        /// <summary>
        /// Instantiate spawn effect and find iridiumSystem.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            reference = FindObjectOfType<ReferenceManager>();
            iridiumSystem = FindObjectOfType<IridiumSystem>();
        }

        /// <summary>
        /// make emitter look at player.
        /// </summary>
        private void Update()
        {
            emitter.transform.LookAt(reference.player.transform);
        }

        /// <summary>
        /// Add iridiumValue to total in IridiumSystem when shot.
        /// </summary>
        public override void Shot()
        {
            health--;
            if (health <= 0)
            {
                iridiumSystem.AddIridium(iridiumValue);
                base.Shot();
            }
        }

        /// <summary>
        /// Self destruct. No additions makes to IridiumSystem total.
        /// </summary>
        private IEnumerator SelfDestruct()
        {
            yield return new WaitForSeconds(selfDestructTime);
            base.Shot();
        }
    }
}
