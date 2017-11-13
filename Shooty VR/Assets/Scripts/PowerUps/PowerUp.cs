//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.champan.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// Powerup controls interaction with powerup pickup objects. When the player
// interacts with the pickup, they recieve a powerup corresponding to Powerup's
// pickup prefab. The player can only receive the powerup if they do not 
// already have a powerup of that type currently stored.
//
//=============================================================================

using UnityEngine;

namespace Hive.Armada.Powerup
{
    public class Powerup : MonoBehaviour
    {
        /// <summary>
        /// Reference to pickup prefab.
        /// </summary>
        public GameObject powerupPrefab;

        /// <summary>
        /// Reference to powerup icon prefab.
        /// </summary>
        public GameObject powerupIconPrefab;

        /// <summary>
        /// FX of pickup instantiation.
        /// </summary>
        public GameObject fxAwake;

        /// <summary>
        /// Reference to PowerUpStatus.
        /// </summary>
        private PowerupStatus status;

        //Reference to player head transform.
        private Transform head;

        /// <summary>
        /// Time until self-destruct.
        /// </summary>
        public float lifeTime = 10.0f;

        /// <summary>
        /// Find references. Instantiate and rotate FX. Start self-destruct countdown.
        /// </summary>
        private void Start()
        {
            head = GameObject.Find("FollowHead").transform;
            GameObject fx = Instantiate(fxAwake, transform.position, transform.localRotation);
            fx.transform.rotation = Quaternion
                    .FromToRotation(Vector3.up, head.position - gameObject.transform.position);

            status = FindObjectOfType<PowerupStatus>();
            Destroy(gameObject, lifeTime);
        }

        /// <summary>
        /// Update rotation to face player.
        /// </summary>
        private void Update()
        {
            gameObject.transform.parent.LookAt(head);
        }

        /// <summary>
        /// Give player powerup upon collision.
        /// </summary>
        /// <param name="other">Collider of object with which this collided.</param>
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player" && status.HasRoom())
            {
                switch (powerupPrefab.name)
                {
                    case "Ally":
                        if (!status.p1Stored)
                        {
                            status.StorePowerup(powerupPrefab, powerupIconPrefab);
                            status.p1Stored = true;
                            Destroy(gameObject);
                        }
                        break;

                    case "Area Bomb":
                        if (!status.p2Stored)
                        {
                            status.StorePowerup(powerupPrefab, powerupIconPrefab);
                            status.p2Stored = true;
                            Destroy(gameObject);

                        }
                        break;

                    case "Clear":
                        if (!status.p3Stored)
                        {
                            status.StorePowerup(powerupPrefab, powerupIconPrefab);
                            status.p3Stored = true;
                            Destroy(gameObject);
                        }
                        break;

                    case "Damage Boost":
                        if (!status.p4Stored)
                        {
                            status.StorePowerup(powerupPrefab, powerupIconPrefab);
                            status.p4Stored = true;
                            Destroy(gameObject);
                        }
                        break;

                    case "Shield":
                        if (!status.p5Stored)
                        {
                            status.StorePowerup(powerupPrefab, powerupIconPrefab);
                            status.p5Stored = true;
                            Destroy(gameObject);
                        }
                        break;
                }
            }
        }
    }
}