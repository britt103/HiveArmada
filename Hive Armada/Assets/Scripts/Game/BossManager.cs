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

using System;
using System.Collections;
using UnityEngine;
using Hive.Armada.Enemies;
using Hive.Armada.Game;
using Hive.Armada.Player;
using SubjectNerd.Utilities;

namespace Hive.Armada.Game
{
    /// <summary>
    /// The manager for all boss behavior.
    /// </summary>
    public class BossManager : MonoBehaviour
    {
        /// <summary>
        /// Reference manager that holds all needed references
        /// (e.g. spawner, game manager, etc.)
        /// </summary>
        private ReferenceManager reference;

        public GameObject bossPrefab;

        private GameObject bossObject;

        private NewBoss bossScript;

        [Header("Points")]
        public Transform bossSpawn;

        public Transform combatPoint;

        public Transform patrolCenterPoint;

        public Transform idleLeftPoint;

        public Transform idleRightPoint;

        [Header("Paths")]
        [Reorderable("Path", false)]
        public iTweenPath[] patrolLPaths;

        [Reorderable("Path", false)]
        public iTweenPath[] patrolLCPaths;

        [Reorderable("Path", false)]
        public iTweenPath[] patrolCPaths;

        [Reorderable("Path", false)]
        public iTweenPath[] patrolRCPaths;

        [Reorderable("Path", false)]
        public iTweenPath[] patrolRPaths;

        [Header("Audio")]
        public AudioSource source;

        public AudioClip introClip;

        public NewBoss.BossStates CurrentState { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsInitialized { get; private set; }

        private void Awake()
        {
            source.PlayOneShot(introClip);
            Initialize(null);
        }

        /// <summary>
        /// Initializes
        /// </summary>
        /// <param name="referenceManager"> The reference manager </param>
        public void Initialize(ReferenceManager referenceManager)
        {
            //reference = referenceManager;

            if (bossPrefab != null)
            {
                bossObject = Instantiate(bossPrefab, bossSpawn.position, bossSpawn.rotation, transform);
                bossScript = bossObject.GetComponent<NewBoss>();

                //Hashtable moveHash = new Hashtable
                //                             {
                //                                 {"easetype", iTween.EaseType.easeOutSine},
                //                                 {"time", 8.0f},
                //                                 {"orienttopath", true},
                //                                 {"onComplete", "OnPathingComplete"},
                //                                 {"onCompleteTarget", bossObject},
                //                                 {"path", iTweenPath.GetPath(bossIntroPath.pathName)}
                //                             };
                //iTween.MoveTo(bossObject, moveHash);
                bossScript.SendMessage("OnPathingComplete");
                TransitionState(NewBoss.BossStates.Intro);
            }
            else
            {
                Debug.LogError(GetType().Name + " - bossPrefab is not set.");
            }
        }

        private void TransitionState(NewBoss.BossStates newState)
        {
            switch ((int)newState)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    // play intro audio
                    bossScript.TransitionState(newState);
                    break;
                default:
                    break;
            }
        }
    }
}
