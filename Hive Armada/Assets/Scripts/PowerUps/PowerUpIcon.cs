//=============================================================================
// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-440-01
// Group Project
// 
// 
// 
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hive.Armada.Game;

namespace Hive.Armada.PowerUps
{
    public class PowerUpIcon : MonoBehaviour
    {
        private ReferenceManager reference;

        private void Awake()
        {
            reference = FindObjectOfType<ReferenceManager>();
        }

        private void Update()
        {
            transform.LookAt(reference.playerLookTarget.transform);
        }
    }
}
