//Name: Chad Johnson
//Student ID: 1763718
//Email: johns428@mail.chapman.edu
//Course: CPSC 340-01, CPSC-344-01
//Assignment: Group Project
//Purpose: Script shield powerup behavior

using System.Collections;
using UnityEngine;

namespace Hive.Armada
{
    public class Shield : MonoBehaviour
    {
        public float timeLimit;
        public float warningTime;
        public float warningFlashIntervalTime;
        private float flashTimer = 0.0F;
        private bool flashState = false;
        public Vector3 rotation = new Vector3(0.0F, 0.0F, 0.0F);
        private GameObject powerupAudio;

        private AudioSource source;
        public AudioClip[] clips;
        //private PowerUpStatus status;

        private void Start()
        {
            //status = GameObject.Find("Player").GetComponent<PowerUpStatus>();
            powerupAudio = GameObject.Find("Powerup Audio");
            source = powerupAudio.GetComponent<AudioSource>();
            StartCoroutine(shieldActivateSound());
        }

        // Update is called once per frame
        void Update()
        {
            timeLimit -= Time.deltaTime;
            if (timeLimit <= 0.0F)
            {
                //status.SetShield(false);
                StartCoroutine(shieldDeactivateSound());
                FindObjectOfType<PowerUpStatus>().shieldActive = false;
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
            if (other.CompareTag("bullet"))
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
        IEnumerator shieldDeactivateSound()
        {
            source.PlayOneShot(clips[1]);
            yield return new WaitForSeconds(1);
        }

        IEnumerator shieldActivateSound()
        {
            source.PlayOneShot(clips[0]);
            yield return new WaitForSeconds(1);
        }
    }
}