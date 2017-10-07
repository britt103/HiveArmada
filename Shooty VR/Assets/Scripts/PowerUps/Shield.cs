//Name: Chad Johnson
//Student ID: 1763718
//Email: johns428@mail.chapman.edu
//Course: CPSC 340-01, CPSC-344-01
//Assignment: Group Project
//Purpose: Script shield powerup behavior

using UnityEngine;

namespace ShootyVR
{
    public class Shield : MonoBehaviour
    {
        public float timeLimit;
        public float warningTime;
        public float warningFlashIntervalTime;
        private float flashTimer = 0.0F;
        private bool flashState = false;
        public Vector3 rotation = new Vector3(0.0F, 0.0F, 0.0F);

        // Update is called once per frame
        void Update()
        {
            timeLimit -= Time.deltaTime;
            if (timeLimit <= 0.0F)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<PowerUpStatus>().SetShield(false);
                Destroy(gameObject);
            }

            if (timeLimit <= warningTime)
            {
                Flash();
            }

            transform.Rotate(rotation);
        }

        /// <summary>
        /// Handles collision with other
        /// </summary>
        /// <param name="other">Collider of other object</param>
        void OnTriggerEnter(Collider other)
        {
            if (other.tag == "bullet")
            {
                Destroy(other.gameObject);
            }
        }

        /// <summary>
        /// Alternates mesh renderer status for Warning effect
        /// </summary>
        private void Flash()
        {
            flashTimer += Time.deltaTime;
            if (flashTimer >= warningFlashIntervalTime)
            {
                gameObject.GetComponent<MeshRenderer>().enabled = flashState;
                flashState = !flashState;
                flashTimer = 0.0F;
            }
        }
    }

}