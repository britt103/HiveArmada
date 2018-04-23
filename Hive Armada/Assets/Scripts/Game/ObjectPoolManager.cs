//=============================================================================
// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-440-01, CPSC-340-01 & CPSC-344-01
// Group Project
// 
// This class contains the object pool manager. It allows us to define a list
// of prefabs to generate pools for and how large each pool should be. Then
// instead of using Instantiate(), we use Spawn() (with almost the same list of
// overloads) to "spawn" an object from the pool. Pooled objects are referred
// to by an integer that they are assigned when the pools are generated that is
// based on their index in the list of objects to pool. This allows for easy
// access to pools without having to always convert a prefab into an index.
// 
//=============================================================================

using System;
using System.Collections.Generic;
using SubjectNerd.Utilities;
using UnityEngine;

namespace Hive.Armada.Game
{
    /// <summary>
    /// This object pool manager. Contains all methods needed to pool any number of
    /// dynamically-sized  object pools to reduce lag and overhead.
    /// </summary>
    public class ObjectPoolManager : MonoBehaviour
    {
        /// <summary>
        /// Structure of a prefab to pool and how many to create in the pool.
        /// </summary>
        [Serializable]
        public struct ObjectToPool
        {
            /// <summary>
            /// The game object to create in the pool.
            /// </summary>
            public GameObject objectPrefab;

            /// <summary>
            /// How many to create in the pool.
            /// </summary>
            public int amountToPool;
        }

        /// <summary>
        /// Reference manager that holds all needed references
        /// (e.g. spawner, game manager, etc.)
        /// </summary>
        private ReferenceManager reference;

        /// <summary>
        /// If a pool runs out, should it Instantiate() more objects?
        /// </summary>
        [Tooltip("If a pool runs out, should it Instantiate() more objects?")]
        public bool canExpand = true;

        /// <summary>
        /// Parents for each of the object types in the pool.
        /// </summary>
        private GameObject[] poolParents;

        /// <summary>
        /// The names of the pool parents. Used to easily update the number of pooled enemies
        /// without regenerating the name. E.g. "Enemy_Standard Parent: "
        /// </summary>
        private string[] parentNames;

        /// <summary>
        /// Array of structures for the object to pool. Includes the GameObject
        /// and how many instances to create in the pool.
        /// </summary>
        [Tooltip("Array of all objects that should have pools created for " +
                 "them and how many of each to create in the pool.")]
        [Reorderable("Pooled Object", false)]
        public ObjectToPool[] objects;

        /// <summary>
        /// Array of stacks for each object in the pool.
        /// These stacks will hold the objects that are not currently active.
        /// </summary>
        private Stack<GameObject>[] inactivePools;

        /// <summary>
        /// Dictionary of all objects in the pool that are active.
        /// </summary>
        private Dictionary<uint, GameObject> activePool;

        /// <summary>
        /// Dictionary of all pooled prefabs' type identifiers.
        /// Key is the prefab InstanceID for fast lookups.
        /// </summary>
        private Dictionary<int, short> typeIdentifierLookup;

        /// <summary>
        /// The last pool identifier used.
        /// </summary>
        private uint lastPoolIdentifier;

        /// <summary>
        /// If Initialize() has been run yet.
        /// </summary>
        private bool isInitialized;

        /// <summary>
        /// Generates all pools
        /// </summary>
        public void Initialize(ReferenceManager referenceManager)
        {
            if (isInitialized)
            {
                Debug.LogWarning(GetType().Name + " - This has already initialized!");
                return;
            }

            isInitialized = true;
            reference = referenceManager;

            typeIdentifierLookup = new Dictionary<int, short>();

            for (int i = 0; i < objects.Length; ++i)
            {
                typeIdentifierLookup.Add(objects[i].objectPrefab.GetInstanceID(), (short) i);
            }

            reference.enemyAttributes.Initialize(referenceManager);

            poolParents = new GameObject[objects.Length];
            parentNames = new string[objects.Length];

            inactivePools = new Stack<GameObject>[objects.Length];
            activePool = new Dictionary<uint, GameObject>();

            for (int i = 0; i < objects.Length; ++i)
            {
                if (objects[i].objectPrefab.GetComponent<Poolable>() == null)
                {
                    Debug.LogError(GetType().Name + " - " + objects[i].objectPrefab.name +
                                   " does not inherit Poolable!");
                    continue;
                }

                // Debug.LogWarning("CREATING POOL => PREFAB NAME: \"" + objects[i].objectPrefab.name +
                //                  "\" IID: " + objects[i].objectPrefab.GetInstanceID());

                inactivePools[i] = new Stack<GameObject>();

                poolParents[i] = new GameObject();
                poolParents[i].transform.parent = transform;
                poolParents[i].transform.localPosition = Vector3.zero;
                parentNames[i] = " - " + objects[i].objectPrefab.name;
                poolParents[i].name = objects[i].amountToPool + parentNames[i];

                for (int n = 0; n < objects[i].amountToPool; ++n)
                {
                    AddObject((short) i);
                }
            }
        }

        /// <summary>
        /// Spawns a pooled object.
        /// </summary>
        /// <param name="caller"> The object that called Spawn() </param>
        /// <param name="typeIdentifier"> The identifier (index) of the object to spawn </param>
        /// <param name="position"> The position to spawn the object at </param>
        /// <returns> The spawned object </returns>
        public GameObject Spawn(GameObject caller, short typeIdentifier, Vector3 position)
        {
            if (typeIdentifier == -2)
            {
                Debug.LogError(GetType().Name + " - Using uninitialized type identifier \"" +
                               typeIdentifier + "\".");
                return null;
            }

            if (typeIdentifier < 0 || typeIdentifier >= objects.Length)
            {
                Debug.LogError(GetType().Name + " - Invalid type identifier \"" + typeIdentifier +
                               "\". Called by \"" + caller.name + "\", instance ID " +
                               caller.GetInstanceID());
                return null;
            }

            if (inactivePools[typeIdentifier].Count == 0)
            {
                ExpandPool(typeIdentifier);
            }

            GameObject spawned = GetObjectToSpawn(typeIdentifier);

            spawned.transform.position = position;

            spawned.GetComponent<Poolable>().Activate();

            return spawned;
        }

        /// <summary>
        /// Spawns a pooled object.
        /// </summary>
        /// <param name="caller"> The object that called Spawn() </param>
        /// <param name="typeIdentifier"> The identifier (index) of the object to spawn </param>
        /// <param name="position"> The position to spawn the object at </param>
        /// <param name="parent"> New parent for the spawned object </param>
        /// <returns> The spawned object </returns>
        public GameObject Spawn(GameObject caller, short typeIdentifier, Vector3 position,
                                Transform parent)
        {
            if (typeIdentifier == -2)
            {
                Debug.LogError(GetType().Name + " - Using uninitialized type identifier \"" +
                               typeIdentifier + "\".");
                return null;
            }

            if (typeIdentifier < 0 || typeIdentifier >= objects.Length)
            {
                Debug.LogError(GetType().Name + " - Invalid type identifier \"" + typeIdentifier +
                               "\". Called by \"" + caller.name + "\", instance ID " +
                               caller.GetInstanceID());
                return null;
            }

            if (inactivePools[typeIdentifier].Count == 0)
            {
                ExpandPool(typeIdentifier);
            }

            GameObject spawned = GetObjectToSpawn(typeIdentifier);

            spawned.transform.parent = parent;

            spawned.transform.position = position;

            spawned.GetComponent<Poolable>().Activate();

            return spawned;
        }

        /// <summary>
        /// Spawns a pooled object.
        /// </summary>
        /// <param name="caller"> The object that called Spawn() </param>
        /// <param name="typeIdentifier"> The identifier (index) of the object to spawn </param>
        /// <param name="position"> The position to spawn the object at </param>
        /// <param name="rotation"> The rotation to spawn the object with </param>
        /// <returns> The spawned object </returns>
        public GameObject Spawn(GameObject caller, short typeIdentifier, Vector3 position,
                                Quaternion rotation)
        {
            if (typeIdentifier == -2)
            {
                Debug.LogError(GetType().Name + " - Using uninitialized type identifier \"" +
                               typeIdentifier + "\".");
                return null;
            }

            if (typeIdentifier < 0 || typeIdentifier >= objects.Length)
            {
                Debug.LogError(GetType().Name + " - Invalid type identifier \"" + typeIdentifier +
                               "\". Called by \"" + caller.name + "\", instance ID " +
                               caller.GetInstanceID());
                return null;
            }

            if (inactivePools[typeIdentifier].Count == 0)
            {
                ExpandPool(typeIdentifier);
            }

            GameObject spawned = GetObjectToSpawn(typeIdentifier);

            spawned.transform.position = position;
            spawned.transform.rotation = rotation;

            spawned.GetComponent<Poolable>().Activate();

            return spawned;
        }

        /// <summary>
        /// Spawns a pooled object.
        /// </summary>
        /// <param name="caller"> The object that called Spawn() </param>
        /// <param name="typeIdentifier"> The identifier (index) of the object to spawn </param>
        /// <param name="position"> The position to spawn the object at </param>
        /// <param name="rotation"> The rotation to spawn the object with </param>
        /// <param name="parent"> New parent for the spawned object </param>
        /// <returns> The spawned object </returns>
        public GameObject Spawn(GameObject caller, short typeIdentifier, Vector3 position,
                                Quaternion rotation,
                                Transform parent)
        {
            if (typeIdentifier == -2)
            {
                Debug.LogError(GetType().Name + " - Using uninitialized type identifier \"" +
                               typeIdentifier + "\".");
                return null;
            }

            if (typeIdentifier < 0 || typeIdentifier >= objects.Length)
            {
                Debug.LogError(GetType().Name + " - Invalid type identifier \"" + typeIdentifier +
                               "\". Called by \"" + caller.name + "\", instance ID " +
                               caller.GetInstanceID());
                return null;
            }

            if (inactivePools[typeIdentifier].Count == 0)
            {
                ExpandPool(typeIdentifier);
            }

            GameObject spawned = GetObjectToSpawn(typeIdentifier);

            spawned.transform.parent = parent;

            spawned.transform.position = position;
            spawned.transform.rotation = rotation;

            spawned.GetComponent<Poolable>().Activate();

            return spawned;
        }

        /// <summary>
        /// Gets the first object in the corresponding pool and adds it to the active pool.
        /// </summary>
        /// <param name="typeIdentifier"> The identifier (index) of the object to spawn </param>
        /// <returns> The object to spawn </returns>
        private GameObject GetObjectToSpawn(short typeIdentifier)
        {
            GameObject spawned = inactivePools[typeIdentifier].Pop();
            Poolable poolable = spawned.GetComponent<Poolable>();
            activePool.Add(poolable.PoolIdentifier, spawned);

            return spawned;
        }

        /// <summary>
        /// Resets a gameobject and puts it back in the inactive pool.
        /// </summary>
        /// <param name="objectToDespawn"> The object to despawn </param>
        public void Despawn(GameObject objectToDespawn)
        {
            objectToDespawn.transform.position = gameObject.transform.position;

            Poolable poolable = objectToDespawn.GetComponent<Poolable>();
            if (poolable != null)
            {
                short typeIdentifier = poolable.TypeIdentifier;

                poolable.Deactivate();

                objectToDespawn.transform.parent =
                    poolParents[typeIdentifier].transform;
                objectToDespawn.transform.localPosition = Vector3.zero;

                if (!activePool.ContainsKey(poolable.PoolIdentifier))
                {
                    Debug.LogError(GetType().Name + " - Attempting to despawn object that " +
                                   "is not in the active pool. TypeID=" + poolable.TypeIdentifier +
                                   ", PoolID=" + poolable.PoolIdentifier);
                    return;
                }

                activePool.Remove(poolable.PoolIdentifier);
                inactivePools[typeIdentifier].Push(objectToDespawn);
            }
            else
            {
                Debug.LogError(GetType().Name +
                               " - Cannot despawn object because object is not poolable! \"" +
                               objectToDespawn.name + "\"");
            }
        }

        /// <summary>
        /// Finds the typeIdentifier for a given GameObject.
        /// </summary>
        /// <param name="objectType"> The object to identify </param>
        /// <returns> </returns>
        public short GetTypeIdentifier(GameObject objectType)
        {
            if (typeIdentifierLookup.ContainsKey(objectType.GetInstanceID()))
            {
                return typeIdentifierLookup[objectType.GetInstanceID()];
            }

            Debug.LogError(GetType().Name + " - Object not in pool. Prefab Name: \"" +
                           objectType.name + "\"");
            return -1;
        }

        /// <summary>
        /// Expands a pool that has run out of objects by 5 or 5%, whichever is larger.
        /// </summary>
        /// <param name="typeIdentifier"> The identifier of the object whose pool we need to expand </param>
        private void ExpandPool(short typeIdentifier)
        {
            if (typeIdentifier == -2)
            {
                Debug.LogError(GetType().Name + " - Using uninitialized type identifier \"" +
                               typeIdentifier + "\".");
                return;
            }

            if (typeIdentifier < 0 || typeIdentifier >= objects.Length)
            {
                Debug.LogError(GetType().Name + " - Invalid type identifier! \"" + typeIdentifier +
                               "\"");
                return;
            }

            int expansionAmount = Mathf.CeilToInt(objects[typeIdentifier].amountToPool * 0.05f);

            if (expansionAmount < 5)
            {
                expansionAmount = 5;
            }

            objects[typeIdentifier].amountToPool += expansionAmount;

            poolParents[typeIdentifier].name =
                objects[typeIdentifier].amountToPool + parentNames[typeIdentifier];

            for (int i = 0; i < expansionAmount; ++i)
            {
                AddObject(typeIdentifier);
            }
        }

        /// <summary>
        /// Instantiates a GameObject and adds it to the appropriate inactive pool.
        /// </summary>
        /// <param name="typeIdentifier"> The identifier (index) of the object to instantiate </param>
        private void AddObject(short typeIdentifier)
        {
            try
            {
                GameObject pooled = Instantiate(objects[typeIdentifier].objectPrefab,
                                                poolParents[typeIdentifier].transform);
                pooled.GetComponent<Poolable>()
                      .Initialize(reference, typeIdentifier, lastPoolIdentifier++);
                inactivePools[typeIdentifier].Push(pooled);

                // if (typeIdentifier == reference.enemyAttributes.EnemyProjectileTypeIdentifiers[0])
                // {
                //     reference.enemyAttributes.AddProjectile(pooled);
                // }
            }
            catch (IndexOutOfRangeException e)
            {
                Debug.LogWarning("Out of range - " + typeIdentifier);
                Debug.LogError(e.Message);
                Debug.LogException(e);
            }
        }
    }
}