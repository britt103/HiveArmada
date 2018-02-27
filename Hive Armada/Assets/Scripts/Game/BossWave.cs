//=============================================================================
//
// Ryan Britton
// 1849351
// britt103@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// Controls spawning of the Boss enemy throughout the game
//
//=============================================================================

using Hive.Armada.Enemies;
using System;
using System.Collections;
using UnityEngine;

namespace Hive.Armada.Game
{
    public class BossWave : MonoBehaviour
    {
        /// <summary>
        /// Clips to use with source.
        /// </summary>
        public AudioClip[] clips;

        /// <summary>
        /// Reference to Menu Audio source.
        /// </summary>
        public AudioSource source;

        /// <summary>
        /// Reference manager that holds all needed references
        /// (e.g. spawner, game manager, etc.)
        /// </summary>
        private ReferenceManager reference;

        /// <summary>
        /// The WaveManager
        /// </summary>
        private WaveManager waveManager;

        /// <summary>
        /// The index for the wave that is currently running.
        /// </summary>
        private int currentWave;

        /// <summary>
        /// The boss enemy.
        /// </summary>
        public GameObject bossPrefab;

        private GameObject boss;

        /// <summary>
        /// Reference for boss paths
        /// </summary>
        private GameObject bossSpawn;

        /// <summary>
        /// The health value of the boss.
        /// </summary>
        private int bossHealth;

        // Use this for initialization
        void Awake()
        {
            reference = GameObject.Find("Reference Manager").GetComponent<ReferenceManager>();
            waveManager = reference.waveManager;
            bossSpawn = GameObject.Find("BossSpawn");
            bossHealth = bossPrefab.GetComponent<Enemy>().Health;
            boss = Instantiate(bossPrefab, bossSpawn.transform);
            boss.SetActive(false);
        }


        /// <summary>
        /// Runs the spawning logic for the wave.
        /// </summary>
        /// <param name="wave"> This wave's index </param>
        public void Run(int wave)
        {
            //placeholder for boss audio
            //source.PlayOneShot(clips[wave]);
            //yield return new WaitForSeconds(clips[wave].length);
            currentWave = wave;            
            boss.SetActive(true);
            iTween.MoveTo(boss, iTween.Hash("path", iTweenPath.GetPath("BossIn") ,"easetype", iTween.EaseType.easeInOutSine,"time",5.0f,"looktarget",reference.playerShip.transform));
            StartCoroutine(WaitForWaveEnd());

        }

        private IEnumerator WaitForWaveEnd()
        {
            while (bossHealth > 0)
            {
                yield return new WaitForSeconds(0.1f);
            }

            Debug.Log("Wave #" + (currentWave + 1) + " is complete.");

            //play audio on defeat here?
            iTween.MoveTo(boss, iTween.Hash("path", iTweenPath.GetPath("BossOut"), "easetype", iTween.EaseType.easeInOutSine, "time", 5.0f, "looktarget", reference.playerShip.transform,
                                            "onComplete", "Reset","onCompleteTarget", boss));
            boss.SetActive(false);
            reference.waveManager.BossWaveComplete(currentWave);
        }
    }
    
}
