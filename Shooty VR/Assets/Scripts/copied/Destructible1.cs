using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project1
{
    public abstract class Destructible : MonoBehaviour
    {
        public float moveSpeed;
        public float turnSpeed;
        public int maxHealth;
        protected int health;
        public GameObject explosionPrefab;
        protected Transform laserParent;
        protected Transform explosionParent;

        public abstract void Hit(int damage);
        public abstract void Collide();
        protected abstract void Kill();
    }
}