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
using UnityEngine;
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

        /// <summary>
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
        /// What texture to use for the lasers.
        /// </summary>
        public Material laserMaterial;

        /// <summary>
        /// How much damage the player takes from lasers.
        /// </summary>
        public int damage;

        /// <summary>
        /// Position below the player.
        /// </summary>
        private Vector3 pointA;

        /// <summary>
        /// Position of the player.
        /// </summary>
        private Vector3 pointB;

        /// <summary>
        /// Location on enemy to spawn lasers
        /// </summary>
        public GameObject[] laserSpawn;

        /// <summary>
        /// Array of lasers to be fired by this enemy
        /// </summary>
        LineRenderer[] laserArray;

        /// <summary>
        /// Whether this enemy has hit the player with its lasers. Toggles when player is hit
        /// </summary>
        private bool hasHit = false;

        /// <summary>
        /// Whether this enemy is shooting its lasers or not. Toggles
        /// </summary>
        private bool shooting = false;

        /// <summary>
        /// How many seconds it takes for the lasers to finish moving.
        /// </summary>
        private float timer;

        /// <summary>
        /// How many seconds the lasers will be on.
        /// </summary>
        public float timeLaserOn;
        /// <summary>
        /// Returns what the laser hit
        /// </summary>
        RaycastHit hit;

        /// <summary>
        /// Tries to look at the player and shoot at it when possible. Runs every frame.
        /// </summary>
        private void OnEnable()
        {
            spawnComplete = false;
            moveComplete = false;
            player = GameObject.FindGameObjectWithTag("Player");
        }
        private void Update()
        {
            if (spawnComplete)
            {
                if (moveComplete)
                {
                    if (player != null)
                    {
                        if (shooting)
                        {
                            transform.eulerAngles = Vector3.Lerp(pointA, pointB, timer);
                            for (int i = 0; i < laserSpawn.Length; i++)
                            {
                                laserArray[i].SetPosition(0, laserSpawn[i].transform.position);
                                laserArray[i].SetPosition(1, laserSpawn[i].transform.forward * 200.0f);
                                if (Physics.Raycast(laserArray[i].GetPosition(0),
                                                    laserArray[i].GetPosition(1) - laserArray[i].GetPosition(0), out hit))
                                    switch (hit.transform.gameObject.tag)
                                    {
                                        case "Player":
                                            if (hasHit == false)
                                            {
                                                hit.collider.gameObject.GetComponent<Player.PlayerHealth>().Hit(damage);
                                                hasHit = true;
                                                StartCoroutine(HitCooldown());
                                            }
                                            break;
                                    }
                            }
                            timer += Time.deltaTime;
                        }
                        else
                        {
                            transform.LookAt(player.transform);

                            if (canShoot)
                            {
                                StartCoroutine(Shoot());
                            }
                        }
                        if (shaking)
                        {
                            iTween.ShakePosition(gameObject, new Vector3(0.1f, 0.1f, 0.1f), 0.1f);
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
                    SelfDestructCountdown();
                }
                else
                {
                    transform.position = Vector3.Lerp(transform.position, endPosition, Time.deltaTime * 1.0f);
                    if (Vector3.Distance(transform.position, endPosition) <= 0.1f)
                    {
                        MoveComplete();
                    }
                }
            }
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

                //child1.GetComponent<Rigidbody>().AddForce(new Vector3(transform.position.x + splitDir, transform.position.y + splitDir, transform.position.z) );
                //child2.GetComponent<Rigidbody>().AddForce(new Vector3(transform.position.x - splitDir, transform.position.y - splitDir, transform.position.z) );
                //child3.GetComponent<Rigidbody>().AddForce(new Vector3(transform.position.x + splitDir, transform.position.y - splitDir, transform.position.z) );
                //child4.GetComponent<Rigidbody>().AddForce(new Vector3(transform.position.x - splitDir, transform.position.y + splitDir, transform.position.z) );
                iTween.MoveTo(child1, iTween.Hash("x", transform.localPosition.x + (splitDir), "y", transform.localPosition.y + (splitDir), "z", transform.localPosition.z, "islocal", false, "time", 1.0f));
                iTween.MoveTo(child2, iTween.Hash("x", transform.localPosition.x + (splitDir), "y", transform.localPosition.y - (splitDir), "z", transform.localPosition.z, "islocal", false, "time", 1.0f));
                iTween.MoveTo(child3, iTween.Hash("x", transform.localPosition.x - (splitDir), "y", transform.localPosition.y + (splitDir), "z", transform.localPosition.z, "islocal", false, "time", 1.0f));
                iTween.MoveTo(child4, iTween.Hash("x", transform.localPosition.x - (splitDir), "y", transform.localPosition.y - (splitDir), "z", transform.localPosition.z, "islocal", false, "time", 1.0f));
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
        /// Runs when this enemy finishes default pathing to SpawnZone.
        /// </summary>
        void SpawnComplete()
        {
            spawnComplete = true;
        }

        /// <summary>
        /// Moves this enemy to endPos.
        /// </summary>
        /// <param name="endPos">Final position of this enemy.</param>
        public void SetEndpoint(Vector3 endPos)
        {
            endPosition = endPos;
            SpawnComplete();
        }
        /// <summary>
        /// 
        /// </summary>
        void MoveComplete()
        {
            moveComplete = true;
        }

        /// <summary>
        /// Removes the damage from lasers for 1 second after they collide with the player.
        /// </summary>
        public IEnumerator HitCooldown()
        {
            yield return new WaitForSeconds(1.0f);
            hasHit = false;
        }

        /// <summary>
        /// Resets attributes to this enemy's defaults from enemyAttributes.
        /// </summary>
        protected override void Reset()
        {
            projectileTypeIdentifier =
                            enemyAttributes.EnemyProjectileTypeIdentifiers[TypeIdentifier];
            maxHealth = enemyAttributes.enemyHealthValues[TypeIdentifier];
            Health = maxHealth;
            fireRate = enemyAttributes.enemyFireRate[TypeIdentifier];
            projectileSpeed = enemyAttributes.projectileSpeed;
            spread = enemyAttributes.enemySpread[TypeIdentifier];
            pointValue = enemyAttributes.enemyScoreValues[TypeIdentifier];
            selfDestructTime = enemyAttributes.enemySelfDestructTimes[TypeIdentifier];
        }
    }
}
