//=============================================================================
// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
// 
// [DESCRIPTION]
// 
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using Hive.Armada.Player;
using UnityEngine;

namespace  Hive.Armada.Game
{
    /// <summary>
    /// Holds references to all prefabs and important objects.
    /// </summary>
    public class ReferenceManager : MonoBehaviour
    {
        //--------------------
        // Systems & Managers
        //--------------------
        [Header("Systems & Managers")]
        public GameManager gameManager;
        public Spawner spawner;
        public ScoringSystem scoringSystem;
        public PlayerStats statistics;
        public WaveManager waveManager;

        //--------------------
        // Player
        //--------------------
        [Header("Player")]
        public GameObject playerShip;
        public GameObject shipPickup;
        public PowerUpStatus powerUpStatus;

        //--------------------
        // Menus
        //--------------------
        [Header("Menus")]
        public GameObject menuMain;
        public GameObject menuTitle;
    }
}

