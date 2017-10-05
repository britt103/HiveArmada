using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

namespace VRTK.Examples
{
    using UnityEngine;

    public class Gun : VRTK_InteractableObject
    {
        private GameObject bullet;
        //private float bulletSpeed = 10000f;
        //private float bulletLife = 5f;
        public bool isTriggerPressed = false;

        public GameObject laserPrefab;
        public Transform laserSpawn;
        public float laserSpeed = 30.0f;
        [Tooltip("How many shots the ship can shoot per second.")]
        public float firerate = 10.0f;

        public override void StartUsing(VRTK_InteractUse usingObject)
        {
            base.StartUsing(usingObject);
        }

        public override void StopUsing(VRTK_InteractUse usingObject)
        {
            base.StopUsing(usingObject);
        }

        protected void Start()
        {
            bullet = transform.Find("Bullet").gameObject;
            bullet.SetActive(false);
        }
    }
}