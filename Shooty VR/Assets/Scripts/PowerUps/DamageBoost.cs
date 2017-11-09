using Hive.Armada.Player;
using Hive.Armada.Player.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
namespace Hive.Armada
{
    public class DamageBoost : MonoBehaviour
    {
        public float boostLength;
        public int boost;
        public GameObject fxAwake;

        // Use this for initialization
        void Start()
        {
            Instantiate(fxAwake, GameObject.FindGameObjectWithTag("Player").transform);
            StartCoroutine(Run());

        }

        private IEnumerator Run()
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<ShipController>().SetDamageBoost(boost);

            yield return new WaitForSeconds(boostLength);

            GameObject.FindGameObjectWithTag("Player").GetComponent<ShipController>().SetDamageBoost(1);
            FindObjectOfType<PowerUpStatus>().damageBoostActive = false;
            Destroy(gameObject);
        }

    }
}
