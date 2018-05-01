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

using UnityEngine;

namespace Hive.Armada.Game
{
    /// <summary>
    /// 
    /// </summary>
    public class MenuSounds : MonoBehaviour
    {
        /// <summary>
        /// Reference manager that holds all needed references
        /// (e.g. spawner, game manager, etc.)
        /// </summary>
        protected ReferenceManager reference;

        [Header("General")]
        public AudioClip menuButtonHoverSound;

        public AudioClip menuButtonSelectSound;

        public AudioClip[] gameStart;

        [Header("Shop")]
        public AudioClip shopPurchaseButton;

        public AudioClip[] shopEnterSound;

        public AudioClip[] shopPurchaseSound;
    }
}