//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// BombBounds stops the movement of an Area Bomb upon collision.
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hive.Armada
{
    /// <summary>
    /// Stop Area Bomb movement.
    /// </summary>
    public class BombBounds : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.GetComponent<AreaBomb>() != null)
            {
                other.gameObject.GetComponent<AreaBomb>().Stop();
            }
        }
    }
}
