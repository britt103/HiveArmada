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
using Hive.Armada;
using Hive.Armada.Menu;
using UnityEngine.SceneManagement;

namespace Hive.Armada.Player
{
    public class PlayerHealth : MonoBehaviour
    {
        public ShipController shipController;
        public int maxHealth = 100;
        private int currentHealth;
        public bool isAlive { get; private set; }
        public GameObject fxHit, fxHurt, fxDead;
        private Material material;
        public Material flashColor;

        public AudioSource sfx;
        public AudioClip clip;

        void Start()
        {
            currentHealth = maxHealth;
            isAlive = true;
            material = gameObject.GetComponentInChildren<Renderer>().material;
        }

        public void Hit(int damage)
        {
            Instantiate(fxHit, transform);
            currentHealth -= damage;

            if (Utility.isDebug)
                Debug.Log("Hit for " + damage + " damage! Remaining health = " + currentHealth);

            if (currentHealth <= 10) fxHurt.SetActive(true);

            if (currentHealth <= 0)
            {
                sfx.PlayOneShot(clip);
                if (shipController != null)
                {
                    Instantiate(fxDead, transform.position, transform.rotation);
                    GameObject.Find("Main Menu").GetComponent<StartMenu>().GameOver();
                    shipController.hand.DetachObject(gameObject);
                }
            }

            //StartCoroutine(HitFlash());
        }

        private IEnumerator HitFlash()
        {
            foreach (Renderer renderer in gameObject.GetComponentsInChildren<Renderer>())
            {
                if (renderer.gameObject.CompareTag("FX"))
                    continue;

                renderer.material = flashColor;
            }

            yield return new WaitForSeconds(0.05f);

            foreach (Renderer renderer in gameObject.GetComponentsInChildren<Renderer>())
            {
                if (renderer.gameObject.CompareTag("FX"))
                    continue;

                renderer.material = material;
            }
        }
    }
}
