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
        /// If a pool runs out, should it Instantiate() more objects?
        /// </summary>
        [Tooltip("If a pool runs out, should it Instantiate() more objects?")]
        public bool canExpand;

        /// <summary>
        /// Prefab of empty game object that is the parent of each object type in the pool.
        /// </summary>
        [Tooltip(
            "Prefab of empty game object to use as the parent for each object type in the pool.")]
        public GameObject poolParentPrefab;

        /// <summary>
        /// Parents for each of the object types in the pool.
        /// </summary>
        private GameObject[] poolParents;

        /// <summary>
        /// The names of the pool parents. Used to easily update the number of pooled enemies withoutregenerating the name.
        /// E.g. "Enemy_Standard Parent: "
        /// </summary>
        private string[] parentNames;

        /// <summary>
        /// Array of all objects that should have pools created for them.
        /// </summary>
        [Tooltip("Array of all objects that should have pools created for them.")]
        [Reorderable("Object", false)]
        public GameObject[] objectsToPool;

        /// <summary>
        /// Array of pool sizes for objectsToPool.
        /// </summary>
        [Tooltip(
            "Array of how big the pool for each object should be initialized at. Also represents the current amount of objects in the pool.")]
        [Reorderable("Object", false)]
        public int[] amountsToPool;

        /// <summary>
        /// Array of queues for each object to pool.
        /// These queues will hold the objects that are currently deactivated in the scene.
        /// </summary>
        private LinkedList<GameObject>[] inactivePools;

        /// <summary>
        /// Array of queues for each object to pool.
        /// These queues will hold the objects that are currently activated in the scene.
        /// </summary>
        private LinkedList<GameObject>[] activePools;

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
                if (objectsToPool.Length != amountsToPool.Length)
                {
                    Debug.LogError(GetType().Name +
                                   " - objectsToPool and amountsToPool are not the same length!");
                }
                else
                {
                    poolParents = new GameObject[objectsToPool.Length];
                    parentNames = new string[objectsToPool.Length];
                    inactivePools = new LinkedList<GameObject>[objectsToPool.Length];
                    activePools = new LinkedList<GameObject>[objectsToPool.Length];

                    for (int i = 0; i < objectsToPool.Length; ++i)
                    {
                        if (objectsToPool[i].GetComponent<Poolable>() == null)
                        {
                            Debug.LogError(GetType().Name + " - " + objectsToPool[i].name +
                                           " does not inherit Poolable!");
                            continue;
                        }

                        inactivePools[i] = new LinkedList<GameObject>();
                        activePools[i] = new LinkedList<GameObject>();

                        poolParents[i] = Instantiate(poolParentPrefab, gameObject.transform);
                        parentNames[i] = objectsToPool[i].name + " Parent: ";
                        poolParents[i].name = parentNames[i] + amountsToPool[i];

                        for (int n = 0; n < amountsToPool[i]; ++n)
                        {
                            AddObject(i);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Spawns a pooled object.
        /// </summary>
        /// <param name="objectType"> The type of object to spawn </param>
        /// <param name="position"> The position to spawn the object at </param>
        /// <returns> The spawned object </returns>
        public GameObject Spawn(GameObject objectType, Vector3 position)
        {
            int typeIdentifier = GetTypeIdentifier(objectType);

            if (typeIdentifier == -1)
            {
                Debug.LogError(GetType().Name + " - Unable to find object \"" + objectType.name +
                               "\" in pool!");
                return null;
            }

            if (inactivePools[typeIdentifier].Count == 0)
            {
                ExpandPool(typeIdentifier);
            }

            LinkedListNode<GameObject> spawnedNode = inactivePools[typeIdentifier].Last;
            GameObject spawned = spawnedNode.Value;
            inactivePools[typeIdentifier].RemoveLast();
            activePools[typeIdentifier].AddLast(spawnedNode);

            spawned.GetComponent<Poolable>().Activate();
            spawned.transform.position = position;

            return spawned;
        }

        /// <summary>
        /// Spawns a pooled object.
        /// </summary>
        /// <param name="objectType"> the type of object to spawn </param>
        /// <param name="parent"> New parent for the spawned object </param>
        /// <param name="position"> The position to spawn the object at </param>
        /// <returns> The spawned object </returns>
        public GameObject Spawn(GameObject objectType, Transform parent, Vector3 position)
        {
            int typeIdentifier = GetTypeIdentifier(objectType);

            if (typeIdentifier == -1)
            {
                Debug.LogError(GetType().Name + " - Unable to find object \"" + objectType.name +
                               "\" in pool!");
                return null;
            }

            if (inactivePools[typeIdentifier].Count == 0)
            {
                ExpandPool(typeIdentifier);
            }

            LinkedListNode<GameObject> spawnedNode = inactivePools[typeIdentifier].Last;
            GameObject spawned = spawnedNode.Value;
            inactivePools[typeIdentifier].RemoveLast();
            activePools[typeIdentifier].AddLast(spawnedNode);

            spawned.GetComponent<Poolable>().Activate();
            spawned.transform.position = position;
            spawned.transform.parent = parent;

            return spawned;
        }

        /// <summary>
        /// Spawns a pooled object.
        /// </summary>
        /// <param name="objectType"> the type of object to spawn </param>
        /// <param name="parent"> New parent for the spawned object </param>
        /// <param name="position"> The position to spawn the object at </param>
        /// <param name="rotation"> The rotation to spawn the object with </param>
        /// <returns> The spawned object </returns>
        public GameObject Spawn(GameObject objectType, Transform parent, Vector3 position,
            Quaternion rotation)
        {
            int typeIdentifier = GetTypeIdentifier(objectType);

            if (typeIdentifier == -1)
            {
                Debug.LogError(GetType().Name + " - Unable to find object \"" + objectType.name +
                               "\" in pool!");
                return null;
            }

            if (inactivePools[typeIdentifier].Count == 0)
            {
                ExpandPool(typeIdentifier);
            }

            LinkedListNode<GameObject> spawnedNode = inactivePools[typeIdentifier].Last;
            GameObject spawned = spawnedNode.Value;
            inactivePools[typeIdentifier].RemoveLast();
            activePools[typeIdentifier].AddLast(spawnedNode);

            spawned.GetComponent<Poolable>().Activate();
            spawned.transform.parent = parent;
            spawned.transform.position = position;
            spawned.transform.rotation = rotation;

            return spawned;
        }

        /// <summary>
        /// Spawns a pooled object.
        /// </summary>
        /// <param name="typeIdentifier"> The identifier (index) of the object to spawn </param>
        /// <param name="position"> The position to spawn the object at </param>
        /// <returns> The spawned object </returns>
        public GameObject Spawn(int typeIdentifier, Vector3 position)
        {
            if (typeIdentifier < 0 || typeIdentifier >= objectsToPool.Length)
            {
                Debug.LogError(GetType().Name + " - Invalid type identifier \"" + typeIdentifier +
                               "\"!");
                return null;
            }

            if (inactivePools[typeIdentifier].Count == 0)
            {
                ExpandPool(typeIdentifier);
            }

            LinkedListNode<GameObject> spawnedNode = inactivePools[typeIdentifier].Last;
            GameObject spawned = spawnedNode.Value;
            inactivePools[typeIdentifier].RemoveLast();
            activePools[typeIdentifier].AddLast(spawnedNode);

            spawned.GetComponent<Poolable>().Activate();
            spawned.transform.position = position;

            return spawned;
        }

        /// <summary>
        /// Spawns a pooled object.
        /// </summary>
        /// <param name="typeIdentifier"> The identifier (index) of the object to spawn </param>
        /// <param name="parent"> New parent for the spawned object </param>
        /// <param name="position"> The position to spawn the object at </param>
        /// <returns> The spawned object </returns>
        public GameObject Spawn(int typeIdentifier, Transform parent, Vector3 position)
        {
            if (typeIdentifier < 0 || typeIdentifier >= objectsToPool.Length)
            {
                Debug.LogError(GetType().Name + " - Invalid type identifier \"" + typeIdentifier +
                               "\"!");
                return null;
            }

            if (inactivePools[typeIdentifier].Count == 0)
            {
                ExpandPool(typeIdentifier);
            }

            LinkedListNode<GameObject> spawnedNode = inactivePools[typeIdentifier].Last;
            GameObject spawned = spawnedNode.Value;
            inactivePools[typeIdentifier].RemoveLast();
            activePools[typeIdentifier].AddLast(spawnedNode);

            spawned.GetComponent<Poolable>().Activate();
            spawned.transform.position = position;
            spawned.transform.parent = parent;

            return spawned;
        }

        /// <summary>
        /// Spawns a pooled object.
        /// </summary>
        /// <param name="typeIdentifier"> The identifier (index) of the object to spawn </param>
        /// <param name="parent"> New parent for the spawned object </param>
        /// <param name="position"> The position to spawn the object at </param>
        /// <param name="rotation"> The rotation to spawn the object with </param>
        /// <returns> The spawned object </returns>
        public GameObject Spawn(int typeIdentifier, Transform parent, Vector3 position,
            Quaternion rotation)
        {
            if (typeIdentifier < 0 || typeIdentifier >= objectsToPool.Length)
            {
                Debug.LogError(GetType().Name + " - Invalid type identifier \"" + typeIdentifier +
                               "\"!");
                return null;
            }

            if (inactivePools[typeIdentifier].Count == 0)
            {
                ExpandPool(typeIdentifier);
            }

            LinkedListNode<GameObject> spawnedNode = inactivePools[typeIdentifier].Last;
            GameObject spawned = spawnedNode.Value;
            inactivePools[typeIdentifier].RemoveLast();
            activePools[typeIdentifier].AddLast(spawnedNode);

            spawned.GetComponent<Poolable>().Activate();
            spawned.transform.parent = parent;
            spawned.transform.position = position;
            spawned.transform.rotation = rotation;

            return spawned;
        }

        /// <summary>
        /// Resets a gameobject and puts it back in the inactive pool.
        /// </summary>
        /// <param name="objectToDespawn"> The object to despawn </param>
        public void Despawn(GameObject objectToDespawn)
        {
            Poolable objectPoolable = objectToDespawn.GetComponent<Poolable>();
            if (objectPoolable)
            {
                int typeIdentifier = objectPoolable.TypeIdentifier;

                objectPoolable.Deactivate();

                // Find objectToDespawn in the activePools
                if (activePools[objectPoolable.TypeIdentifier].First.Value == objectToDespawn)
                {
                    // objectToDespawn is first in the list
                    LinkedListNode<GameObject> despawnNode =
                        activePools[typeIdentifier].First;
                    activePools[typeIdentifier].RemoveFirst();
                    inactivePools[typeIdentifier].AddLast(despawnNode);
                }
                else if (activePools[objectPoolable.TypeIdentifier].Last.Value == objectToDespawn)
                {
                    // objectToDespawn is last in the list
                    LinkedListNode<GameObject> despawnNode =
                        activePools[typeIdentifier].Last;
                    activePools[typeIdentifier].RemoveLast();
                    inactivePools[typeIdentifier].AddLast(despawnNode);
                }
                else
                {
                    // objectToDespawn is somewhere in the middle of the list
                    LinkedListNode<GameObject> despawnNode = activePools[typeIdentifier].First.Next;

                    while (despawnNode != null)
                    {
                        if (despawnNode.Value == objectToDespawn)
                        {
                            break;
                        }

                        despawnNode = despawnNode.Next;
                    }

                    if (despawnNode != null)
                    {
                        activePools[typeIdentifier].Remove(despawnNode);
                        inactivePools[typeIdentifier].AddLast(despawnNode);
                    }
                    else
                    {
                        Debug.LogError(GetType().Name +
                                       " - Cannot Despawn() because object is not in the activePool! \"" +
                                       objectToDespawn.name + "\" InstanceID = " + objectToDespawn.GetInstanceID());
                    }
                }
            }
            else
            {
                Debug.LogError(GetType().Name +
                               " - Cannot Despawn() because object is not poolable! \"" +
                               objectToDespawn.name + "\"");
            }
        }

        /// <summary>
        /// Finds the typeIdentifier for a given GameObject.
        /// </summary>
        /// <param name="objectType"> The object to identify </param>
        /// <returns> </returns>
        private int GetTypeIdentifier(GameObject objectType)
        {
            for (int i = 0; i < objectsToPool.Length; ++i)
            {
                if (objectType.name.Equals(objectsToPool[i].name))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Expands a pool that has run out of objects by 5 or 5%, whichever is larger.
        /// </summary>
        /// <param name="typeIdentifier"> The identifier (index) of the object whose pool we need to expand </param>
        private void ExpandPool(int typeIdentifier)
        {
            if (typeIdentifier < 0 || typeIdentifier >= amountsToPool.Length)
            {
                Debug.LogError(GetType().Name + " - Invalid type identifier! " + typeIdentifier);
                return;
            }

            int expansionAmount = Mathf.CeilToInt(amountsToPool[typeIdentifier] * 0.05f);

            if (expansionAmount < 5)
            {
                expansionAmount = 5;
            }

            amountsToPool[typeIdentifier] += expansionAmount;

            poolParents[typeIdentifier].name =
                parentNames[typeIdentifier] + amountsToPool[typeIdentifier];

            for (int i = 0; i < expansionAmount; ++i)
            {
                AddObject(typeIdentifier);
            }
        }

        /// <summary>
        /// Instantiates a GameObject and adds it to the appropriate inactive pool.
        /// </summary>
        /// <param name="typeIdentifier"> The identifier (index) of the object to instantiate </param>
        private void AddObject(int typeIdentifier)
        {
            GameObject pooled = Instantiate(objectsToPool[typeIdentifier],
                poolParents[typeIdentifier].transform);
            pooled.GetComponent<Poolable>()
                .Initialize(typeIdentifier);
            inactivePools[typeIdentifier].AddLast(pooled);
        }
    }
}