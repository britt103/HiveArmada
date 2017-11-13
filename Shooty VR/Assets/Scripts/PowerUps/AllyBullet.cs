//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.champan.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// AllyBullet controls projectiles fired by the Ally powerup. 
//
//=============================================================================

using UnityEngine;

namespace Hive.Armada.Powerups
{
    /// <summary>
    /// Ally powerup projectile.
    /// </summary>
    public class AllyBullet : MonoBehaviour
    {
        /// <summary>
        /// Damage dealt to enemies upon collision.
        /// </summary>
        public int damage;

        /// <summary>
        /// Time until self-destruct without enemy collision.
        /// </summary>
        public float lifeTime;

        // Start self-destruct countdown time using lifeTime.
        void Start()
        {
            Destroy(gameObject, lifeTime);
        }

        /// <summary>
        /// Damage enemy upon collision and destroy self after collision with enemy or room
        /// </summary>
        /// <param name="other">Collider of object with which this collided</param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                if(other.gameObject.GetComponent<Enemies.Enemy>() != null)
                {
                    other.gameObject.GetComponent<Enemies.Enemy>().Hit(damage);
                }
                Destroy(gameObject);
            }
            else if (other.gameObject.CompareTag("Room"))
            {
                Destroy(gameObject);
            }
        }
    }
}
