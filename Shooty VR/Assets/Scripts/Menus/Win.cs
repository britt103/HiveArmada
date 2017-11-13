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

namespace Hive.Armada.Menus
{
    /// <summary>
    /// Controls interactions with Win Menu.
    /// </summary>
    public class Win : MonoBehaviour
    {
        /// <summary>
        /// Start Run coroutine.
        /// </summary>
        private void OnEnable()
        {
            StartCoroutine(Run());
        }

        /// <summary>
        /// Call PrintStats in PlayerStats and reload scene.
        /// </summary>
        private IEnumerator Run()
        {
            yield return new WaitForSeconds(3);
            FindObjectOfType<PlayerStats>().PrintStats();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
