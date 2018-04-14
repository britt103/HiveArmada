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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

namespace Hive.Armada
{
    public class FootageCameraRig : MonoBehaviour
    {
        [Header("Objects")]
        public GameObject footageCamera;

        public GameObject target;

        public iTweenPath cameraPath;

        public iTweenPath targetPath;

        [Header("Time Settings")]
        [Range(0.0f, 180.0f)]
        [Tooltip("How long after scene start to delay the camera and target path.")]
        public float startDelay;

        [Range(0.0f, 180.0f)]
        public float cameraPathTime;

        [Range(0.0f, 180.0f)]
        public float targetPathTime;

        [Space()]
        public bool loop;

        private void Start()
        {
            Hashtable cameraHash = new Hashtable
                                   {
                                       {"delay", startDelay},
                                       {"easetype", iTween.EaseType.easeInOutSine},
                                       {"looktarget", target},
                                       {"time", cameraPathTime},
                                       {"path", iTweenPath.GetPath(cameraPath.pathName)}
                                   };
            Hashtable targetHash = new Hashtable
                                   {
                                       {"delay", startDelay},
                                       {"easetype", iTween.EaseType.easeInOutSine},
                                       {"time", targetPathTime},
                                       {"path", iTweenPath.GetPath(targetPath.pathName)}
                                   };

            if (loop)
            {
                cameraHash.Add("looptype", iTween.LoopType.loop);
                targetHash.Add("looptype", iTween.LoopType.loop);
            }

            iTween.MoveTo(footageCamera, cameraHash);
            iTween.MoveTo(target, targetHash);
        }
    }
}