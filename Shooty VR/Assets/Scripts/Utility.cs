// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
// 
// This class has utility functions and constants
// that are going to be used in other scripts.
// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameName
{
    public static class Utility
    {
        public static LayerMask enemyMask = LayerMask.GetMask("Enemy");
        public static LayerMask roomMask = LayerMask.GetMask("Room");

        public static LayerMask uiMask = LayerMask.GetMask("UI");
    }
}
