//=============================================================================
// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
// 
// [DESCRIPTION]
// 
//=============================================================================

using UnityEngine;
using UnityEngine.VR.WSA.Persistence;

namespace Hive.Armada.Game
{
    public class Waves : MonoBehaviour
    {
        public GameObject[] enemies;
        public int[][] waveSpawns;
        private readonly int[] enemyCap =
        {
            1,
            2,
            4,
            4,
            2
        };
        private readonly float[] spawnTime =
        {
            3.0f,
            3.0f,
            2.0f,
            3.0f,
            3.0f
        };

        void Awake()
        {
            waveSpawns = new int[5][];

            waveSpawns[0] = new[] { 2, 0, 0, 0 };
            waveSpawns[1] = new[] { 4, 0, 0, 0 };
            waveSpawns[2] = new[] { 8, 0, 0, 0 };
            waveSpawns[3] = new[] { 4, 1, 0, 0 };
            waveSpawns[4] = new[] { 0, 3, 0, 0 };
            ;
            //=
            //{
            //    { 2, 0, 0, 0 },
            //    { 4, 0, 0, 0 },
            //    { 8, 0, 0, 0 },
            //    { 4, 1, 0, 0 },
            //    { 0, 3, 0, 0 }
            //};
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wave"></param>
        /// <returns></returns>
        public int[] GetSpawns(int wave)
        {
            Debug.Log("waveSpawns.GetUpperBound(" + wave + ")");
            int[] spawns = new int[waveSpawns[wave].Length];

            for (int i = 0; i < waveSpawns[wave].Length; ++i)
            {
                spawns[i] = waveSpawns[wave][i];
            }

            return spawns;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wave"></param>
        /// <returns></returns>
        public int GetEnemyCap(int wave)
        {
            return enemyCap[wave];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wave"></param>
        /// <returns></returns>
        public float GetSpawnTime(int wave)
        {
            return spawnTime[wave];
        }
    }
}

