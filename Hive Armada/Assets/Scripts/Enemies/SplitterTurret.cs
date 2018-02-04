//=============================================================================
//
// Ryan Britton
// 1849351
// britt103@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// Enemy that shoots at the player and that spawns 4 more enemies
// when destroyed.
//
//=============================================================================

using System.Collections;
using System.Linq;
using UnityEngine;
using MirzaBeig.ParticleSystems;
using Random = UnityEngine.Random;

namespace Hive.Armada.Enemies
{
    /// <summary>
    /// Splitter turret enemy
    /// </summary>
    public class SplitterTurret : Enemy
    {
        /// <summary>
        /// Type identifier for this enemy's projectiles in objectPoolManager
        /// </summary>
        private int projectileTypeIdentifier;

        /// <summary>
        /// The point where this enemy shoots from.
        /// </summary>
        public Transform shootPoint;

        /// <summary>
        /// How many time per second this enemy can shoot.
        /// </summary>
        private float fireRate;

        /// <summary>
        /// How fast the projectiles move.
        /// </summary>
        private float projectileSpeed;

        /// <summary>
        /// Degrees that the projectile can be randomly rotated.
        /// Randomly picks in the range of [-spread, spread] for all 3 axes.
        /// </summary>
        private float spread;

        /// <summary>
        /// The player's ship.
        /// </summary>
        private GameObject player;

        /// <summary>
        /// The enemy to spawn when this enemy dies
        /// </summary>
        public GameObject childTurret;

        /// <summary>
        /// Distance each child enemy will move when this enemy is destroyed
        /// </summary>
        public float splitDir;

        /// <summary>
        /// Whether this enemy can shoot or not. Toggles when firing every 1/fireRate seconds.
        /// </summary>
        private bool canShoot = true;

        // <summary>
        /// Final position after spawning.
        /// </summary>
        private Vector3 endPosition;

        /// <summary>
        /// Bools used to move the enemy to its spawn position.
        /// </summary>
        bool spawnComplete;

        /// <summary>
        /// Bools used to move the enemy to its spawn position.
        /// </summary>
        bool moveComplete;

        /// <summary>
        /// Tries to look at the player and shoot at it when possible. Runs every frame.
        /// </summary>
        private void Update()
        {
            if (spawnComplete)
            {
                if (moveComplete)
                {
                    if (player != null)
                    {
                        transform.LookAt(player.transform);

                        if (canShoot)
                        {
                            StartCoroutine(Shoot());
                        }
                    }
                    else
                    {
                        player = reference.playerShip;

                        if (player == null)
                        {
                            transform.LookAt(new Vector3(0.0f, 2.0f, 0.0f));
                        }
                    }
                }
                else
                {
                    transform.position = Vector3.Lerp(transform.position, endPosition, Time.deltaTime * 1.0f);
                    if (Vector3.Distance(transform.position, endPosition) <= 0.1f)
                    {
                        moveComplete = true;
                    }

                }
            }
            //if (shaking)
            //{
            //    iTween.ShakePosition(gameObject, new Vector3(0.1f, 0.1f, 0.1f), 0.1f);

            //}
            SelfDestructCountdown();
        }

        /// <summary>
        /// Shoots a projectile at the player.
        /// </summary>
        private IEnumerator Shoot()
        {
            canShoot = false;

            GameObject projectile =
                objectPoolManager.Spawn(projectileTypeIdentifier, shootPoint.position,
                                        shootPoint.rotation);

            projectile.GetComponent<Transform>().Rotate(Random.Range(-spread, spread),
                                                        Random.Range(-spread, spread),
                                                        Random.Range(-spread, spread));

            projectile.GetComponent<Rigidbody>().velocity =
                projectile.transform.forward * projectileSpeed;

            yield return new WaitForSeconds(fireRate);

            canShoot = true;
        }

        /// <summary>
        /// Runs when this enemy finishes default pathing to a SpawnZone.
        /// </summary>
        /// <param name="endPos">Final position of this enemy.</param>
        public void SetEndpoint(Vector3 endPos)
        {
            endPosition = endPos;
            spawnComplete = true;
        }

        /// <summary>
        /// Spawns 4 'childTurret' enemies when this enemy is destroyed
        /// </summary>
        protected override void KillSpecial()
        {
            if (childTurret != null)
            {
                int typeIdentifier = objectPoolManager.GetTypeIdentifier(childTurret);
                Game.EnemySpawn childEnemySpawn = new Game.EnemySpawn(typeIdentifier, enemySpawn.spawnZone, enemySpawn.attackPattern);

                //Instantiate("Explosion.name", transform.position, transform.rotation); Placeholder for destroy effect
                GameObject child1 = objectPoolManager.Spawn(typeIdentifier, new Vector3(transform.position.x + 0.1f, transform.position.y + 0.1f, transform.position.z), transform.rotation);
                Enemy child1Enemy = child1.GetComponent<Enemy>();
                child1Enemy.SetSubwave(subwave);
                child1Enemy.SetEnemySpawn(childEnemySpawn);
                child1Enemy.SetAttackPattern(enemySpawn.attackPattern);

                GameObject child2 = objectPoolManager.Spawn(typeIdentifier, new Vector3(transform.position.x - 0.1f, transform.position.y - 0.1f, transform.position.z), transform.rotation);
                Enemy child2Enemy = child2.GetComponent<Enemy>();
                child2Enemy.SetSubwave(subwave);
                child2Enemy.SetEnemySpawn(childEnemySpawn);
                child2Enemy.SetAttackPattern(enemySpawn.attackPattern);

                GameObject child3 = objectPoolManager.Spawn(typeIdentifier, new Vector3(transform.position.x + 0.1f, transform.position.y - 0.1f, transform.position.z), transform.rotation);
                Enemy child3Enemy = child3.GetComponent<Enemy>();
                child3Enemy.SetSubwave(subwave);
                child3Enemy.SetEnemySpawn(childEnemySpawn);
                child3Enemy.SetAttackPattern(enemySpawn.attackPattern);

                GameObject child4 = objectPoolManager.Spawn(typeIdentifier, new Vector3(transform.position.x - 0.1f, transform.position.y + 0.1f, transform.position.z), transform.rotation);
                Enemy child4Enemy = child4.GetComponent<Enemy>();
                child4Enemy.SetSubwave(subwave);
                child4Enemy.SetEnemySpawn(childEnemySpawn);
                child4Enemy.SetAttackPattern(enemySpawn.attackPattern);

                child1.GetComponent<Rigidbody>().AddForce(new Vector3(transform.position.x + splitDir, transform.position.y + splitDir, transform.position.z));
                child2.GetComponent<Rigidbody>().AddForce(new Vector3(transform.position.x - splitDir, transform.position.y - splitDir, transform.position.z));
                child3.GetComponent<Rigidbody>().AddForce(new Vector3(transform.position.x + splitDir, transform.position.y - splitDir, transform.position.z));
                child4.GetComponent<Rigidbody>().AddForce(new Vector3(transform.position.x - splitDir, transform.position.y + splitDir, transform.position.z));

            }

            StartCoroutine(DeathDelay());
        }

        private IEnumerator DeathDelay()
        {
            foreach (Renderer r in renderers)
            {
                r.GetComponent<Renderer>().enabled = false;
            }
            yield return new WaitForSeconds(1.0f);
            //Destroy(gameObject);
        }

        /// <summary>
        /// Resets attributes to this enemy's defaults from enemyAttributes.
        /// </summary>
        protected override void Reset()
        {
            // reset materials
            for (int i = 0; i < renderers.Count; ++i)
            {
                renderers.ElementAt(i).material = materials.ElementAt(i);
            }

            hitFlash = null;
            shaking = false;
            canShoot = true;
            spawnComplete = false;
            moveComplete = false;

            projectileTypeIdentifier =
                            enemyAttributes.EnemyProjectileTypeIdentifiers[TypeIdentifier];
            maxHealth = enemyAttributes.enemyHealthValues[TypeIdentifier];
            Health = maxHealth;
            fireRate = enemyAttributes.enemyFireRate[TypeIdentifier];
            projectileSpeed = enemyAttributes.projectileSpeed;
            spread = enemyAttributes.enemySpread[TypeIdentifier];
            pointValue = enemyAttributes.enemyScoreValues[TypeIdentifier];
            selfDestructTime = enemyAttributes.enemySelfDestructTimes[TypeIdentifier];
            spawnEmitter = enemyAttributes.enemySpawnEmitters[TypeIdentifier];
            deathEmitter = enemyAttributes.enemyDeathEmitters[TypeIdentifier];

            if (!isInitialized)
            {
                isInitialized = true;

                GameObject spawnEmitterObject = Instantiate(spawnEmitter,
                                                            transform.position,
                                                            transform.rotation, transform);
                spawnEmitterSystem = spawnEmitterObject.GetComponent<ParticleSystems>();

                deathEmitterTypeIdentifier = objectPoolManager.GetTypeIdentifier(deathEmitter);
            }
        }
    }
}