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

        public float[][] wavePowerupChances;

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

            wavePowerupChances = new float[5][];

            //currently shield, ally, area, clear
            wavePowerupChances[0] = new[] { 1.0f, 0.3f, 0.0f, 0.0f };
            wavePowerupChances[1] = new[] { 0.3f, 0.7f, 0.0f, 0.0f };
            wavePowerupChances[2] = new[] { 0.0f, 0.3f, 0.7f, 0.0f };
            wavePowerupChances[3] = new[] { 0.0f, 0.0f, 0.3f, 0.7f };
            wavePowerupChances[4] = new[] { 0.25f, 0.25f, 0.25f, 0.25f };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wave"></param>
        /// <returns></returns>
        public int[] GetSpawns(int wave)
        {
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

        public float[] GetPowerupChances(int wave)
        {
            return wavePowerupChances[wave];
        }
    }
}

