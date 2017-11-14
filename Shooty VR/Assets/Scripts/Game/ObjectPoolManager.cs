//=============================================================================
//
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// [DESCRIPTION]
//
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SubjectNerd.Utilities;

namespace Hive.Armada.Game
{
    /// <summary>
    /// This object pool manager. Contains all methods needed to pool any number of
    /// dynamically-sized  object pools to reduce lag and overhead.
    /// </summary>
    public class ObjectPoolManager : MonoBehaviour
    {
        /// <summary>
        /// Prefab of empty game object that is the parent of each object type in the pool.
        /// </summary>
        [Tooltip("Prefab of empty game object to use as the parent for each object type in the pool.")]
        public GameObject poolParentPrefab;

        /// <summary>
        /// Array of all objects that should have pools created for them.
        /// </summary>
        [Tooltip("Array of all objects that should have pools created for them.")]
        [Reorderable("Object", false)]
        public GameObject[] objectsToPool;

        /// <summary>
        /// Array of pool sizes for objectsToPool.
        /// </summary>
        [Tooltip("Array of how big the pool for each object should be initialized at.")]
        [Reorderable("Object", false)]
        public int[] initialPools;

        /// <summary>
        /// Array of queues for each object to pool.
        /// These queues will hold the objects that are currently ready to be used.
        /// </summary>
        private Queue<GameObject>[] readyPools;

        /// <summary>
        /// Array of queues for each object to pool.
        /// These queues will hold the objects that are currently running in the scene.
        /// </summary>
        private Queue<GameObject>[] runningPools;

        /// <summary>
        /// Generates all pools
        /// </summary>
        private void Start()
        {
            if (objectsToPool.Length == 0)
            {
                Debug.LogError(GetType().Name + " - has no objects to pool!");
            }
            else
            {
                if (objectsToPool.Length != initialPools.Length)
                {
                    Debug.LogError(GetType().Name +
                                   " - objectsToPool and initialPools are not the same length!");
                }
                else
                {
                    readyPools = new Queue<GameObject>[objectsToPool.Length];
                    runningPools = new Queue<GameObject>[objectsToPool.Length];

                    for (int i = 0; i < objectsToPool.Length; ++i)
                    {
                        if (objectsToPool[i].GetComponent<Poolable>() == null)
                        {
                            Debug.LogError(GetType().Name + " - " + objectsToPool[i].name +
                                           " does not inherit Poolable!");
                            continue;
                        }

                        GameObject parent = Instantiate(poolParentPrefab, gameObject.transform);

                        for (int n = 0; n < initialPools[i]; ++n)
                        {
                            GameObject pooled = Instantiate(objectsToPool[i], parent.transform);
                            pooled.GetComponent<Poolable>().Initialize(i);
                            readyPools[i].Enqueue(pooled);
                        }
                    }
                }
            }
        }
    }
}