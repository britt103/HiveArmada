//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.champan.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// AreaBomb controls the Area Bomb powerup. The Area Bomb accelerates forward
// until collision with an enemy, collision with the room, or it's time limit
// has run out. When one of these events occur, the Area Bomb detonates, 
// destroying any enemies within a given radius. Currently assigned as
// Powerup 2.
//
//=============================================================================

using System.Collections;
using UnityEngine;

namespace Hive.Armada.Powerup
{
    /// <summary>
    /// Area Bomb powerup.
    /// </summary>
    public class AreaBomb : MonoBehaviour
    {
        /// <summary>
        /// Radius of Physics Sphere used for detonation damage. 
        /// </summary>
        public float radius;

        /// <summary>
        /// Acceleration affecting the Area Bomb's speed.
        /// </summary>
        public float acceleration;

        /// <summary>
        /// Forward distance from player ship.
        /// </summary>
        public float startingZ;

        /// <summary>
        /// Area Bomb current speed.
        /// </summary>
        private float currentSpeed;

        /// <summary>
        /// FX of Area Bomb movement trail.
        /// </summary>
        public GameObject fxTrail;

        /// <summary>
        /// FX of Area Bomb detonation.
        /// </summary>
        public GameObject fxBomb;

        /// <summary>
        /// Start TimeDetonate countdown. Activate trail FX. Set transform.
        /// </summary>
        void Start()
        {
            StartCoroutine(TimeDetonate());
            fxTrail.SetActive(true);

            transform.localPosition = new Vector3(0, 0, startingZ);
            gameObject.transform.parent = null;
        }

        /// <summary>
        /// Move and adjust current speed using acceleration.
        /// </summary>
        void Update()
        {
            currentSpeed += acceleration * Time.deltaTime;
            transform.Translate(Vector3.forward * currentSpeed);

            //if (hand.controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad))
            //{
            //    Vector2 touchpad = hand.controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0);

            //    if (touchpad.y < -0.7)
            //    {
            //        if (released)
            //        {
            //            // button-based detonation

            //        }
            //        else
            //        {
            //            GameObject.Find("Player").GetComponent<PowerUpStatus>().SetAreaBomb(false);
            //            gameObject.transform.parent = null;
            //            released = true;
            //            fxTrail.SetActive(true);
            //        }
            //    }
            //}
        }

        /// <summary>
        /// Trigger detonation on impact with enemy.
        /// </summary>
        /// <param name="other">Collider of object with which this collided.</param>
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                Detonate();
            }
        }

        /// <summary>
        /// Detonate after set time.
        /// </summary>
        private IEnumerator TimeDetonate()
        {
            yield return new WaitForSeconds(2);
            Detonate();
        }

        /// <summary>
        /// Destroy enemies within certain radius. Activate detonation FX. Self-destruct.
        /// </summary>
        private void Detonate()
        {
            foreach (Collider objectCollider in Physics.OverlapSphere(transform.position, radius))
            {
                if (objectCollider.gameObject.tag == "Enemy")
                {
                    objectCollider.gameObject.GetComponent<Enemies.Enemy>().Hit(100);
                }
            }
            Instantiate(fxBomb, transform.position, transform.rotation);
            FindObjectOfType<PowerupStatus>().p2Active = false;
            Destroy(gameObject);
        }
    }
}
