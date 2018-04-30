//=============================================================================
// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-440-01
// Group Project
// 
//=============================================================================

using UnityEngine;

namespace Hive.Armada.Data
{
    [CreateAssetMenu(fileName = "New Cheats", menuName = "Cheats")]
    public class Cheats : ScriptableObject
    {
        [Header("Cheats")]
        public bool godMode;

        public bool doubleDamage;
    }
}