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
            2,
            4,
            2,
            3,
            4,
            3
        };
        private readonly float[] spawnTime =
        {
            3.0f,
            3.0f,
            2.0f,
            3.0f,
            3.0f,
            2.0f,
            3.0f,
            2.0f,
            2.0f,
            2.0f
        };

        public float[][] wavePowerupChances;

        void Awake()
        {
            waveSpawns = new int[10][];

            // 1-5
            waveSpawns[0] = new[] { 2, 0, 0, 0 };
            waveSpawns[1] = new[] { 4, 0, 0, 0 };
            waveSpawns[2] = new[] { 8, 0, 0, 0 };
            waveSpawns[3] = new[] { 4, 1, 0, 0 };
            waveSpawns[4] = new[] { 0, 3, 0, 0 };
            // 6-10
            waveSpawns[5] = new[] { 8, 2, 0, 0 };
            waveSpawns[6] = new[] { 0, 5, 0, 0 };
            waveSpawns[7] = new[] { 0, 0, 6, 0 };
            waveSpawns[8] = new[] { 4, 2, 4, 0 };
            waveSpawns[9] = new[] { 2, 0, 0, 1 };
            ;
            //=
            //{
            //    { 2, 0, 0, 0 },
            //    { 4, 0, 0, 0 },
            //    { 8, 0, 0, 0 },
            //    { 4, 1, 0, 0 },
            //    { 0, 3, 0, 0 }
            //};

            wavePowerupChances = new float[10][];

            //currently shield, ally, damage, area, clear
            //1-5
            wavePowerupChances[0] = new[] { 0.2f, 0.4f, 0.6f, 0.8f, 1.0f };
            wavePowerupChances[1] = new[] { 0.2f, 0.4f, 0.6f, 0.8f, 1.0f };
            wavePowerupChances[2] = new[] { 0.2f, 0.4f, 0.6f, 0.8f, 1.0f };
            wavePowerupChances[3] = new[] { 0.2f, 0.4f, 0.6f, 0.8f, 1.0f };
            wavePowerupChances[4] = new[] { 0.2f, 0.4f, 0.6f, 0.8f, 1.0f };
            //6-10
            wavePowerupChances[5] = new[] { 0.2f, 0.4f, 0.6f, 0.8f, 1.0f };
            wavePowerupChances[6] = new[] { 0.2f, 0.4f, 0.6f, 0.8f, 1.0f };
            wavePowerupChances[7] = new[] { 0.2f, 0.4f, 0.6f, 0.8f, 1.0f };
            wavePowerupChances[8] = new[] { 0.2f, 0.4f, 0.6f, 0.8f, 1.0f };
            wavePowerupChances[9] = new[] { 0.2f, 0.4f, 0.6f, 0.8f, 1.0f };
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

