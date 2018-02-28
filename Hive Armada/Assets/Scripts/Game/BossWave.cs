﻿//=============================================================================
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
        /// The boss enemy prefab.
        /// </summary>
        public GameObject bossPrefab;

        /// <summary>
        /// The boss enemy in scene.
        /// </summary>
        private GameObject boss;

        /// <summary>
        /// Reference for boss paths
        /// </summary>
        private GameObject bossSpawn;

        /// <summary>
        /// Sets up logic for spawning the boss.
        /// </summary>
        void Awake()
        {
            reference = GameObject.Find("Reference Manager").GetComponent<ReferenceManager>();
            waveManager = reference.waveManager;
            bossSpawn = GameObject.Find("Boss Spawn");
            boss = Instantiate(bossPrefab, bossSpawn.transform);
            boss.GetComponent<Boss>().BossReset();
            //bossHealth = boss.GetComponent<Enemy>().Health;
            Debug.Log("boss health on bosswave is: " + bossHealth);
            boss.SetActive(false);
        }


        /// <summary>
        /// Runs the spawning logic for the boss wave.
        /// </summary>
        /// <param name="wave"> This wave's index </param>
        public void Run(int wave)
        {           
            currentWave = wave;
            StartCoroutine(RunBoss());            
        }

        /// <summary>
        /// Paths boss into the scene.
        /// </summary>
        private IEnumerator RunBoss()
        {
            //placeholder for boss audio
            //source.PlayOneShot(clips[wave]);
            //yield return new WaitForSeconds(clips[wave].length);
            yield return new WaitForSeconds(0.1f);
            boss.SetActive(true);
            iTween.MoveTo(boss, iTween.Hash("path", iTweenPath.GetPath("BossIn"), "easetype", iTween.EaseType.easeInOutSine, "time", 5.0f, "looktarget", reference.playerShip.transform,
                                            "onComplete", "FinishedPathing","onCompleteTarget", boss));
            StartCoroutine(WaitForPathing());          
        }

        /// <summary>
        /// Waits for boss to finish pathing into the scene.
        /// </summary>
        private IEnumerator WaitForPathing()
        {
            while (!boss.GetComponent<Boss>().pathingFinished)
            {
                yield return new WaitForSeconds(0.1f);
            }

            StartCoroutine(WaitForWaveEnd());
            Debug.Log("Boss wave " + currentWave+1);
        }

        /// <summary>
        /// Waits for boss to reach 0 health, then paths it out of scene.
        /// </summary>
        private IEnumerator WaitForWaveEnd()
        {
            while (boss.GetComponent<Enemy>.Health > 0)
            {
                //Debug.Log("Current Boss Health: " + bossHealth);
                yield return new WaitForSeconds(0.1f);
            }

            Debug.Log("Boss wave #" + (currentWave + 1) + " is complete.");

            //play audio on defeat here?
            boss.GetComponent<Boss>().pathingFinished = false;
            iTween.MoveTo(boss, iTween.Hash("path", iTweenPath.GetPath("BossOut"), "easetype", iTween.EaseType.easeInOutSine, "time", 5.0f, "looktarget", reference.playerShip.transform,
                                            "onComplete", "FinishedPathing","onCompleteTarget", boss));
            StartCoroutine(WaitForBossOut());            
        }

        /// <summary>
        /// Resets boss values once it finishes pathing out of scene.
        /// Tells wavemanager to go to the next wave.
        /// </summary>
        private IEnumerator WaitForBossOut()
        {
            while (!boss.GetComponent<Boss>().pathingFinished)
            {
                yield return new WaitForSeconds(0.1f);
            }
            boss.GetComponent<Boss>().BossReset();
            Debug.Log("Boss wave " + currentWave+1 + "Finished");
            boss.SetActive(false);
            reference.waveManager.BossWaveComplete(currentWave);
        }
    }
    
}
