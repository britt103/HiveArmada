using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameName
{
    public class ShootableUI : MonoBehaviour
    {
        public Button button;
        private Player.LaserSight laserSight;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void shot()
        {
            Destroy(gameObject);
        }
    }

}