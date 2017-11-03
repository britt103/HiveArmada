using Hive.Armada.Player;
using Hive.Armada.Player.Guns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
namespace Hive.Armada
{
    public class DamageBoost : MonoBehaviour
    {

        private Hand hand;
        public int boost;

        // Use this for initialization
        void Start()
        {
            //hand = gameObject.GetComponentInParent<Hand>();
            StartCoroutine(GameObject.FindGameObjectWithTag("Player").GetComponent<ShipController>().DamageBoost());
            //GameObject.Find("Player").GetComponent<PowerUpStatus>().SetDamageBoost(false);
            Destroy(gameObject);

        }
       
        //// Update is called once per frame
        //void Update()
        //{
            
        //}
       
    }
}
