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

        public bool targetDoPath;

        [Space]
        public bool loop;

        private void Start()
        {
            Hashtable cameraHash = new Hashtable
                                   {
                                       {"delay", startDelay},
                                       {"movetopath", false},
                                       {"easetype", iTween.EaseType.easeInOutSine},
                                       /*{"looktarget", target.transform},
                                        {"lookahead", 1.0f},*/
                                       {"time", cameraPathTime},
                                       {"path", iTweenPath.GetPath("FootageCameraPath-01")}
                                   };
            Hashtable targetHash = new Hashtable
                                   {
                                       {"delay", startDelay},
                                       {"movetopath", false},
                                       {"easetype", iTween.EaseType.easeInOutSine},
                                       {"time", targetPathTime},
                                       {"path", iTweenPath.GetPath("FootageTargetPath-01")}
                                   };

            if (loop)
            {
                cameraHash.Add("looptype", iTween.LoopType.loop);
                targetHash.Add("looptype", iTween.LoopType.loop);
            }
            iTween.MoveTo(footageCamera, cameraHash);
            if (targetDoPath)
            {
                iTween.MoveTo(target, targetHash);
            }

            //iTween.MoveTo(footageCamera, iTween.Hash("delay", startDelay, "movetopath", false, "easetype", iTween.EaseType.easeInOutSine, "looktarget", target.transform, "time", cameraPathTime, "path", iTweenPath.GetPath("FootageCameraPath-01")));
        }
    }
}