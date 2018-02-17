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
        /// Structure holding bullet prefabs that
        /// the enemy will shoot
        /// </summary>
        public GameObject[] projectileArray;

        /// <summary>
        /// Projectile that the turret shoots out
        /// </summary>
        public GameObject fireProjectile;

        /// <summary>
        /// Value that determines what projectile the enemy will shoot
        /// as well as its parameters
        /// </summary>
        private int fireMode;

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

        /// <summary>
        /// Whether or not the projectile being shot rotates.
        /// </summary>
        private bool canRotate;

        /// <summary>
        /// Tries to look at the player and shoot at it when possible. Runs every frame.
        /// </summary>
        private void Update()
        {
            if (pathingComplete)
            {
                if (reference.playerShip != null)
                {
                    lookTarget = reference.playerShip.transform.position;
                }

                if (lookTarget != null)
                {
                    transform.LookAt(lookTarget);

                    if (canShoot)
                    {
                        StartCoroutine(Shoot());
                    }
                }
                else
                {
                    transform.LookAt(new Vector3(0.0f, 0.7f, 0.0f));
                }
            }
        }

        /// <summary>
        /// Fires projectiles in a pattern determined by the firemode at the player.
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

            if (canRotate)
            {
                StartCoroutine(rotateProjectile(projectile));
            }

            yield return new WaitForSeconds(fireRate);

            canShoot = true;

        }

        private IEnumerator rotateProjectile(GameObject bullet)
        {
            while (true)
            {
                bullet.GetComponent<Transform>().Rotate(0, 0, 1);
                yield return new WaitForSeconds(0.01f);
            }
        }
        /// <summary>
        /// Function that determines the enemy's projectile, firerate,
        /// spread, and projectile speed.
        /// </summary>
        /// <param name="mode">Current Enemy Firemode</param>
        private void switchFireMode(int mode)
        {
            switch (mode)
            {
                case 1:
                    fireRate = 0.6f;
                    projectileSpeed = 1.5f;
                    spread = 2;
                    fireProjectile = projectileArray[0];
                    break;

                case 2:
                    fireRate = 0.3f;
                    projectileSpeed = 1.5f;
                    spread = 0;
                    fireProjectile = projectileArray[1];
                    break;
            }
        }
        /// <summary>
        /// Spawns 4 'childTurret' enemies when this enemy is destroyed
        /// </summary>
        protected override void KillSpecial()
        {
            if (childTurret != null)
            {
                int typeIdentifier = objectPoolManager.GetTypeIdentifier(childTurret);
                //Game.EnemySpawn childEnemySpawn = new Game.EnemySpawn(typeIdentifier, enemySpawn.spawnZone, enemySpawn.attackPattern);

                //Instantiate("Explosion.name", transform.position, transform.rotation); Placeholder for destroy effect
                GameObject child1 = objectPoolManager.Spawn(typeIdentifier, new Vector3(transform.position.x + 0.1f, transform.position.y + 0.1f, transform.position.z), transform.rotation);
                Enemy child1Enemy = child1.GetComponent<Enemy>();
                child1Enemy.SetWave(wave);
                //child1Enemy.SetEnemySpawn(childEnemySpawn);
                //child1Enemy.SetAttackPattern(enemySpawn.attackPattern);

                GameObject child2 = objectPoolManager.Spawn(typeIdentifier, new Vector3(transform.position.x - 0.1f, transform.position.y - 0.1f, transform.position.z), transform.rotation);
                Enemy child2Enemy = child2.GetComponent<Enemy>();
                child2Enemy.SetWave(wave);
                //child2Enemy.SetEnemySpawn(childEnemySpawn);
                //child2Enemy.SetAttackPattern(enemySpawn.attackPattern);

                GameObject child3 = objectPoolManager.Spawn(typeIdentifier, new Vector3(transform.position.x + 0.1f, transform.position.y - 0.1f, transform.position.z), transform.rotation);
                Enemy child3Enemy = child3.GetComponent<Enemy>();
                child3Enemy.SetWave(wave);
                //child3Enemy.SetEnemySpawn(childEnemySpawn);
                //child3Enemy.SetAttackPattern(enemySpawn.attackPattern);

                GameObject child4 = objectPoolManager.Spawn(typeIdentifier, new Vector3(transform.position.x - 0.1f, transform.position.y + 0.1f, transform.position.z), transform.rotation);
                Enemy child4Enemy = child4.GetComponent<Enemy>();
                child4Enemy.SetWave(wave);
                //child4Enemy.SetEnemySpawn(childEnemySpawn);
                //child4Enemy.SetAttackPattern(enemySpawn.attackPattern);

                child1.GetComponent<Rigidbody>().AddForce(new Vector3(transform.position.x + splitDir, transform.position.y + splitDir, transform.position.z));
                child2.GetComponent<Rigidbody>().AddForce(new Vector3(transform.position.x - splitDir, transform.position.y - splitDir, transform.position.z));
                child3.GetComponent<Rigidbody>().AddForce(new Vector3(transform.position.x + splitDir, transform.position.y - splitDir, transform.position.z));
                child4.GetComponent<Rigidbody>().AddForce(new Vector3(transform.position.x - splitDir, transform.position.y + splitDir, transform.position.z));
                //iTween.MoveTo(child1, iTween.Hash("x", transform.localPosition.x + (splitDir), "y", transform.localPosition.y + (splitDir), "z", transform.localPosition.z, "islocal", false, "time", 1.0f));
                //iTween.MoveTo(child2, iTween.Hash("x", transform.localPosition.x + (splitDir), "y", transform.localPosition.y - (splitDir), "z", transform.localPosition.z, "islocal", false, "time", 1.0f));
                //iTween.MoveTo(child3, iTween.Hash("x", transform.localPosition.x - (splitDir), "y", transform.localPosition.y + (splitDir), "z", transform.localPosition.z, "islocal", false, "time", 1.0f));
                //iTween.MoveTo(child4, iTween.Hash("x", transform.localPosition.x - (splitDir), "y", transform.localPosition.y - (splitDir), "z", transform.localPosition.z, "islocal", false, "time", 1.0f));
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