//=============================================================================
//
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// This class defines all standard functions for poolable objects. Every object
// that can be pooled will be able to receive messages from ObjectPool telling
// it when it needs to activate or deactivate itself. Activate also calls
// Reset() which will reset all object variables/attributes to their
// default values. This is useful for replaying the game after a win or loss
// without having to reload the scene.
//
//=============================================================================

using UnityEngine;

namespace Hive.Armada.Game
{
    /// <summary>
    /// Define functions for all Poolable objects.
    /// </summary>
    public abstract class Poolable : MonoBehaviour
    {
        /// <summary>
        /// If this object is currently activated.
        /// Note: This is not the same as gameObject.activeSelf or gameObject.activeInHierarchy
        /// </summary>
        public bool IsActive { get; private set; }

        /// <summary>
        /// Identifies this object's type to the pool.
        /// </summary>
        public short TypeIdentifier { get; private set; }

        /// <summary>
        /// Identifies this object within the active pool.
        /// </summary>
        public uint PoolIdentifier { get; private set; }

        /// <summary>
        /// Initializes all attributes to this object's defaults with Reset() and disables it.
        /// </summary>
        /// <param name="typeIdentifier"> The type identifier for this object </param>
        /// <param name="poolIdentifier"> The pool identifier for this object </param>
        public virtual void Initialize(short typeIdentifier, uint poolIdentifier)
        {
            TypeIdentifier = typeIdentifier;
            PoolIdentifier = poolIdentifier;
            Deactivate();
        }

        /// <summary>
        /// Re-initializes the object and activates it.
        /// </summary>
        public virtual void Activate()
        {
            Reset();
            IsActive = true;
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Deactivates the object
        /// </summary>
        public virtual void Deactivate()
        {
            IsActive = false;
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Resets all attributes to this object's defaults.
        /// </summary>
        protected abstract void Reset();
    }
}