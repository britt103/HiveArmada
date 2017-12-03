//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// Win controls interactions with the Win Menu.
//
//=============================================================================

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hive.Armada.Player;
using Hive.Armada.Game;

namespace Hive.Armada.Menus
{
    /// <summary>
    /// Controls interactions with Win Menu.
    /// </summary>
    public class Win : MonoBehaviour
    {
        /// <summary>
        /// Reference to ReferenceManager.
        /// </summary>
        private ReferenceManager reference;

        /// <summary>
        /// Run when activated
        /// </summary>
        private void OnEnable()
        {
            reference = FindObjectOfType<ReferenceManager>();
            StartCoroutine(Run());
        }

        /// <summary>
        /// Call PrintStats in PlayerStats and loads into the menu room.
        /// </summary>
        private IEnumerator Run()
        {
            yield return new WaitForSeconds(3.0f);
            FindObjectOfType<PlayerStats>().PrintStats();
            reference.sceneTransitionManager.TransitionOut("Menu Room");
        }
    }
}
