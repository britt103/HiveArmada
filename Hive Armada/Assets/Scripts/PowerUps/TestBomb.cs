//=============================================================================
//
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-440-01, CPSC-340-01 & CPSC-344-01
// Group Project
//
// 
//
//=============================================================================

using UnityEngine;
using Hive.Armada.Game;
using Hive.Armada.Player.Weapons;

namespace Hive.Armada.PowerUps
{
    /// <summary>
    /// Area Bomb powerup.
    /// </summary>
    public class TestBomb : MonoBehaviour
    {
        /// <summary>
        /// Reference manager that holds all needed references
        /// (e.g. spawner, game manager, etc.)
        /// </summary>
        private ReferenceManager reference;

        /// <summary>
        /// The prefab for the rocket.
        /// </summary>
        public GameObject rocketPrefab;

        /// <summary>
        /// The type of rocket to use.
        /// </summary>
        public RocketAttributes.RocketType rocketType;

        AudioSource source;

        public AudioClip clip;

        /// <summary>
        /// Shoots a bomb and destroys this object.
        /// </summary>
        protected void Awake()
        {
            reference = FindObjectOfType<ReferenceManager>();
            source = GameObject.Find("Powerup Audio Source").GetComponent<AudioSource>();
            source.PlayOneShot(clip);

            int typeId = reference.objectPoolManager.GetTypeIdentifier(rocketPrefab);

            if (typeId >= 0)
            {
                GameObject rocket =
                    reference.objectPoolManager.Spawn(gameObject, typeId, transform.position,
                                                      transform.rotation);
                Rocket rocketScript = rocket.GetComponent<Rocket>();

                if (rocketScript != null)
                {
                    rocketScript.SetupRocket((int) rocketType);

                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, transform.forward, out hit, 200.0f,
                                        Utility.roomMask))
                    {
                        rocketScript.Launch(null, hit.point);
                    }
                }
                else
                {
                    Debug.LogError(GetType().Name + " - Rocket prefab does not have script " +
                                   "\"Rocket\". Destroying spawned rocket.");
                    Destroy(rocket);
                }
            }
            else
            {
                Debug.LogError(GetType().Name + " - Rocket prefab is not in the pool.");
            }

            gameObject.transform.parent = null;

            Destroy(gameObject);
        }
    }
}