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
// in Reset(). The particle systems run by default when the object is enabled.
// 
//=============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MirzaBeig.ParticleSystems;

namespace Hive.Armada.Game
{
    /// <summary>
    /// Pooled particle systems emitters class.
    /// </summary>
    public class Emitter : Poolable
    {
        /// <summary>
        /// The particle systems script on this emitter.
        /// </summary>
        private ParticleSystems system;

        /// <summary>
        /// Initialize system
        /// </summary>
        private void Awake()
        {
            system = GetComponent<ParticleSystems>();
        }

        private void OnDisable()
        {
            system.stop();
            system.clear();
        }

        /// <summary>
        /// Stops the particle systems and clears all existing particles.
        /// </summary>
        protected override void Reset()
        {
            
        }
    }
}

