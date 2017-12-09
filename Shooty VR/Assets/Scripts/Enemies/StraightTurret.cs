//=============================================================================
//
// Miguel Gotao
// 2264941
// gotao100@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// Standard enemy behavior for shooting projectiles
//
//=============================================================================

using System.Collections;
using UnityEngine;
namespace Hive.Armada.Enemies
{
    public class StraightTurret : Enemy
    {
        /// <summary>
        /// Type identifier for object pooling purposes
        /// </summary>
        private int projectileTypeIdentifier;

        /// <summary>
        /// Projectile that the turret shoots out
        /// </summary>
        public GameObject fireProjectile;

        /// <summary>
        /// Structure holding bullet prefabs that
        /// the enemy will shoot
        /// </summary>
        public GameObject[] projectileArray;

        /// <summary>
        /// Position from which bullets are initially shot from
        /// </summary>
        public Transform fireSpawn;

        /// <summary>
        /// Variable that finds the player GameObject
        /// </summary>
        private GameObject player;

        /// <summary>
        /// Vector3 that holds the player's position
        /// </summary>
        private Vector3 pos;

        /// <summary>
        /// Final position after spawning.
        /// </summary>
        private Vector3 endPosition;

        /// <summary>
        /// How fast the turret shoots at a given rate
        /// </summary>
        public float fireRate;

        /// <summary>
        /// The rate at which enemy projectiles travel
        /// </summary>
        public float fireSpeed;

        /// <summary>
        /// Size of conical spread the bullets travel within
        /// </summary>
        public float fireCone;

        /// <summary>
        /// Value that calculates the next time at which the enemy is able to shoot again
        /// </summary>
        private float fireNext;

        /// <summary>
        /// Value that determines what projectile the enemy will shoot
        /// as well as its parameters
        /// </summary>
        private int fireMode;

        /// <summary>
        /// Spread values determined by fireCone on each axis
        /// </summary>
        private float randX;
        private float randY;
        private float randZ;

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

        ///// <summary>
        ///// On start, select enemy behavior based on value fireMode
        ///// </summary>
        //void Start()
        //{
        //    //switchFireMode(fireMode);
        //}

        /// <summary>
        /// tracks player and shoots projectiles in that direction, while being slightly
        /// swayed via the spread value set by user. If player is not found automatically
        /// finds player, otherwise do nothing.
        /// </summary>
        private void OnEnable()
        {
            spawnComplete = false;
            moveComplete = false;
            switchFireMode(fireMode);
        }
        void Update()
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
                            transform.LookAt(player.transform.position);
                            if (Time.time > fireNext)
                            {
                                fireNext = Time.time + (1 / fireRate);
                                StartCoroutine(FireBullet());
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

        private IEnumerator FireBullet()
        {
            GameObject shoot = objectPoolManager.Spawn(projectileTypeIdentifier, fireSpawn.position,
                                        fireSpawn.rotation);
            randX = Random.Range(-fireCone, fireCone);
            randY = Random.Range(-fireCone, fireCone);
            randZ = Random.Range(-fireCone, fireCone);

            shoot.GetComponent<Transform>().Rotate(randX, randY, randZ);
            shoot.GetComponent<Rigidbody>().velocity = shoot.transform.forward * fireSpeed;
            yield break;
        }

        /// <summary>
        /// Function that determines the enemy's projectile, firerate,
        /// spread, and projectile speed.
        /// </summary>
        /// <param name="mode"></param>
        void switchFireMode(int mode)
        {
            switch (mode)
            {
                case 1:
                    fireRate = 0.6f;
                    fireSpeed = 1.5f;
                    fireCone = 2;
                    fireProjectile = projectileArray[0];
                    break;

                case 2:
                    fireRate = 0.3f;
                    fireSpeed = 1.5f;
                    fireCone = 0;
                    fireProjectile = projectileArray[1];
                    break;
                case 3:
                    SetLaser();
                    break;
            }
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
        /// Turns off this enemy's lasers for 5 seconds.
        /// </summary>
        public IEnumerator LaserCooldown()
        {
            for (int i = 0; i < laserSpawn.Length; i++)
            {
                laserArray[i].enabled = false;
            }
            yield return new WaitForSeconds(5.0f);
            StartCoroutine(LaserOn());
        }

        /// <summary>
        /// Turns on this enemy's lasers.
        /// </summary>
        public IEnumerator LaserOn()
        {
            if (player != null)
            {
                Vector3 lookPos = new Vector3(player.transform.localPosition.x, player.transform.localPosition.y - 5,
                                              player.transform.localPosition.z);
                transform.LookAt(lookPos);
                pointA = transform.localRotation.eulerAngles;
                pointB = transform.localRotation.eulerAngles + new Vector3(-90f, 0f, 0f);
            }

            yield return new WaitForSeconds(1.0f);
            for (int i = 0; i < laserSpawn.Length; i++)
            {
                laserArray[i].enabled = true;
            }
            timer = 0.0f;
            StartCoroutine(LaserMoving());
        }

        /// <summary>
        /// Makes this enemy fire its lasers for 'timelaserOn' seconds.
        /// </summary>
        public IEnumerator LaserMoving()
        {
            shooting = true;
            yield return new WaitForSeconds(timeLaserOn);
            shooting = false;
            StartCoroutine(LaserCooldown());
        }

        /// <summary>
        /// Initializes the array of lasers.
        /// </summary>
        public void SetLaser()
        {
            laserArray = new LineRenderer[laserSpawn.Length];
            for (int i = 0; i < laserSpawn.Length; i++)
            {
                laserArray[i] = laserSpawn[i].AddComponent<LineRenderer>();
                laserArray[i].material = laserMaterial;
                laserArray[i].startWidth = 0.05f;
                laserArray[i].endWidth = 0.05f;
                laserArray[i].SetPosition(0, laserSpawn[i].transform.position);
                laserArray[i].SetPosition(1, laserSpawn[i].transform.forward * 200.0f);
            }
            StartCoroutine(LaserCooldown());
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
            // reset materials
            for (int i = 0; i < renderers.Count; ++i)
            {
                renderers[i].material = materials[i];
            }

            projectileTypeIdentifier =
                enemyAttributes.EnemyProjectileTypeIdentifiers[TypeIdentifier];
            maxHealth = enemyAttributes.enemyHealthValues[TypeIdentifier];
            Health = maxHealth;
            fireRate = enemyAttributes.enemyFireRate[TypeIdentifier];
            fireSpeed = enemyAttributes.projectileSpeed;
            fireCone = enemyAttributes.enemySpread[TypeIdentifier];
            pointValue = enemyAttributes.enemyScoreValues[TypeIdentifier];
            selfDestructTime = enemyAttributes.enemySelfDestructTimes[TypeIdentifier];
            spawnEmitter = enemyAttributes.enemySpawnEmitters[TypeIdentifier];
            deathEmitter = enemyAttributes.enemyDeathEmitters[TypeIdentifier];

        }
    }
}