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
    /// </summary>
    public class Tooltip : MonoBehaviour
    {
        /// <summary>
        /// Reference manager that holds all needed references
        /// (e.g. spawner, game manager, etc.)
        /// </summary>
        private ReferenceManager reference;

        private GameObject playerLookTarget;

        /// <summary>
        /// </summary>
        private void Awake()
        {
            reference = FindObjectOfType<ReferenceManager>();

            playerLookTarget = reference.playerLookTarget;
        }

        private void Update()
        {
            gameObject.transform.rotation =
                Quaternion.LookRotation(gameObject.transform.position -
                                        playerLookTarget.transform.position);
        }
    }
}