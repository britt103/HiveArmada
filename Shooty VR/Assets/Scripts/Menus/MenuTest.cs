using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hive.Armada.Game;

namespace Hive.Armada.Menu
{
    public class MenuTest : MonoBehaviour
    {
        public Spawner spawner;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void buttonClicked()
        {
            if (spawner != null)
                spawner.Run();
            else
                Debug.Log("CRITICAL - MENU'S REFERENCE TO SPAWNER IS NULL");

            gameObject.SetActive(false);
        }
    }
}
