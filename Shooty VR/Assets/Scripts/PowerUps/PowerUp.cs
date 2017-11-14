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

namespace Hive.Armada.PowerUps
{
    public class PowerUp : MonoBehaviour
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
        private PowerUpStatus status;

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

            status = FindObjectOfType<PowerUpStatus>();
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
        /// Give player powerup upon collision, then self-destruct.
        /// </summary>
        /// <param name="other">Collider of object with which this collided.</param>
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player" && status.HasRoom())
            {
                //switch (powerupPrefab.name)
                //{
                //case "Ally":
                //    if (!status.powerupTypeStored[0])
                //    {
                //        status.StorePowerup(powerupPrefab, powerupIconPrefab);
                //        status.powerupTypeStored[0] = true;
                //        Destroy(gameObject);
                //    }
                //    break;

                //case "Area Bomb":
                //    if (!status.powerupTypeStored[1])
                //    {
                //        status.StorePowerup(powerupPrefab, powerupIconPrefab);
                //        status.powerupTypeStored[1] = true;
                //        Destroy(gameObject);

                //    }
                //    break;

                //case "Clear":
                //    if (!status.powerupTypeStored[2])
                //    {
                //        status.StorePowerup(powerupPrefab, powerupIconPrefab);
                //        status.powerupTypeStored[2] = true;
                //        Destroy(gameObject);
                //    }
                //    break;

                //case "Damage Boost":
                //    if (!status.powerupTypeStored[3])
                //    {
                //        status.StorePowerup(powerupPrefab, powerupIconPrefab);
                //        status.powerupTypeStored[3] = true;
                //        Destroy(gameObject);
                //    }
                //    break;

                //case "Shield":
                //    if (!status.powerupTypeStored[4])
                //    {
                //        status.StorePowerup(powerupPrefab, powerupIconPrefab);
                //        status.powerupTypeStored[4] = true;
                //        Destroy(gameObject);
                //    }
                //    break;
                //}
                status.StorePowerup(powerupPrefab, powerupIconPrefab);
                Destroy(gameObject);
            }
        }
    }
}