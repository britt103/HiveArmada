using Hive.Armada.Enemies;
using SubjectNerd.Utilities;
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

        private int enemiesRemaining;

        private int wave;

        public GameObject boss;

        public GameObject backspawn;

        // Use this for initialization
        void Awake()
        {
            reference = GameObject.Find("Reference Manager").GetComponent<ReferenceManager>();
            waveManager = reference.waveManager;
        }


        /// <summary>
        /// Runs the spawning logic for the wave.
        /// </summary>
        public void Run(int wave)
        {
            //source.PlayOneShot(clips[wave]);
            //yield return new WaitForSeconds(clips[wave].length);
            backspawn = GameObject.Find("BackSpawn");
            Instantiate(boss, backspawn.transform);
            iTween.MoveTo(boss, iTween.Hash("path", iTweenPath.GetPath("BossIn") ,"easetype", iTween.EaseType.easeInOutSine,"time",5.0f,"looktarget",reference.playerShip.transform));          
            //spawn and itween boss to path between front and back
        }

        private IEnumerator WaitForWaveEnd()
        {
            while (boss.GetComponent<Enemy>().Health >= 0)
            {
                yield return new WaitForSeconds(0.1f);
            }

            //Debug.Log("Wave #" + (wave + 1) + " is complete.");

            //play audio on defeat here?
            iTween.MoveTo(boss, iTween.Hash("path", iTweenPath.GetPath("BossOut"), "easetype", iTween.EaseType.easeInOutSine, "time", 5.0f, "looktarget", reference.playerShip.transform));

            reference.waveManager.BossWaveComplete(wave);
        }
    }
    
}
