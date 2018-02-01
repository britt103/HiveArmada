//=============================================================================
//
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// This class loads the waves for the classic game mode from a file and
// generates the corresponding wave and subwave objects.
//
//=============================================================================

using System.IO;
using System.IO.Compression;
using Hive.Armada.Enemies;
using UnityEngine;
using SimpleJSON;

namespace Hive.Armada.Game
{
    /// <summary>
    /// </summary>
    public class WaveLoader : MonoBehaviour
    {
        /// <summary>
        /// Reference manager that holds all needed references
        /// (e.g. spawner, game manager, etc.)
        /// </summary>
        public ReferenceManager reference;

        /// <summary>
        /// The filename for the waves file.
        /// </summary>
        [Tooltip("Enter the filename and extension for the waves file.")]
        public string filename = "";

        public Wave[] LoadWaves()
        {
            if (filename.Length == 0)
            {
                Debug.LogError(GetType().Name + " - Filename for waves is empty.");
                return null;
            }

            string text = "";

            byte[] wavesBytes = File.ReadAllBytes(filename);

            using (MemoryStream memoryStream = new MemoryStream(wavesBytes))
            {
                using (DeflateStream deflateStream =
                    new DeflateStream(memoryStream, CompressionMode.Decompress))
                {
                    using (StreamReader reader =
                        new StreamReader(deflateStream, System.Text.Encoding.UTF8))
                    {
                        text = reader.ReadToEnd();
                    }
                }
            }

            if (text.Length == 0)
            {
                Debug.LogError(GetType().Name + " - File is empty.");
                return null;
            }

            JSONNode jsonText = JSON.Parse(text);
            int waveCount = jsonText["waves"].Count;

            if (waveCount < 1)
            {
                Debug.LogError(GetType().Name + " - No waves to load.");
                return null;
            }

            GameObject waveParent = new GameObject("Waves");
            waveParent.transform.parent = reference.waveManager.transform;

            Wave[] waves = new Wave[waveCount];
            GameObject[] waveObjects = new GameObject[waveCount];

            Debug.Log("Loading " + waveCount + " waves...");

            // Iterate over the waves
            for (int wave = 0; wave < waveCount; ++wave)
            {
                JSONNode thisWave = jsonText["waves"][wave];
                int subwaveCount = thisWave["subwaves"].Count;

                waveObjects[wave] = new GameObject(thisWave["name"]);
                waveObjects[wave].transform.parent = waveParent.transform;
                waves[wave] = waveObjects[wave].AddComponent<Wave>();
                waves[wave].subwaves = new Subwave[subwaveCount];

                Subwave[] subwaves = new Subwave[subwaveCount];
                GameObject[] subwaveObjects = new GameObject[subwaveCount];

                // Iterate over this wave's subwaves
                for (int subwave = 0; subwave < subwaveCount; ++subwave)
                {
                    JSONNode thisSubwave = thisWave["subwaves"][subwave];
                    int spawnGroupCount = thisSubwave["spawn-groups"].Count;

                    subwaveObjects[subwave] = new GameObject(thisSubwave["name"]);
                    subwaveObjects[subwave].transform.parent = waveObjects[wave].transform;
                    subwaves[subwave] = subwaveObjects[subwave].AddComponent<Subwave>();
                    subwaves[subwave].enemyCap = thisSubwave["enemy-cap"].AsInt;

                    SetupSpawnGroup[] spawnGroups = new SetupSpawnGroup[spawnGroupCount];

                    // Iterate over this subwave's spawn groups
                    for (int spawnGroup = 0; spawnGroup < spawnGroupCount; ++spawnGroup)
                    {
                        JSONNode thisSpawnGroup = thisSubwave["spawn-groups"][spawnGroup];
                        int enemyCount = thisSpawnGroup["enemies"].Count;
                        SetupEnemySpawn[] enemySpawns = new SetupEnemySpawn[enemyCount];

                        // Iterate over this subwave's enemy spawns
                        for (int enemy = 0; enemy < enemyCount; ++enemy)
                        {
                            JSONNode thisEnemy = thisSpawnGroup["enemies"][enemy];

                            AttackPattern attackPattern;

                            switch (thisEnemy["attack-pattern"].AsInt)
                            {
                                case 0:
                                    attackPattern = AttackPattern.One;
                                    break;
                                case 1:
                                    attackPattern = AttackPattern.Two;
                                    break;
                                case 2:
                                    attackPattern = AttackPattern.Three;
                                    break;
                                case 3:
                                    attackPattern = AttackPattern.Four;
                                    break;
                                default:
                                    attackPattern = AttackPattern.One;
                                    break;
                            }

                            enemySpawns[enemy] = new SetupEnemySpawn
                                                 {
                                                     enemyPrefab = reference
                                                         .objectPoolManager
                                                         .objects[
                                                         thisEnemy["type"]
                                                             .AsInt]
                                                         .objectPrefab,
                                                     spawnCount = thisEnemy["count"].AsInt,
                                                     attackPattern = attackPattern
                                                 };
                        }

                        SpawnZone spawnZone;

                        switch (thisSpawnGroup["spawn-zone"].AsInt)
                        {
                            case 0:
                                spawnZone = SpawnZone.Introduction;
                                break;
                            case 1:
                                spawnZone = SpawnZone.Center;
                                break;
                            case 2:
                                spawnZone = SpawnZone.FrontLeft;
                                break;
                            case 3:
                                spawnZone = SpawnZone.FrontRight;
                                break;
                            case 4:
                                spawnZone = SpawnZone.BackLeft;
                                break;
                            case 5:
                                spawnZone = SpawnZone.BackRight;
                                break;
                            default:
                                spawnZone = SpawnZone.Center;
                                break;
                        }

                        spawnGroups[spawnGroup] = new SetupSpawnGroup
                                                  {
                                                      spawnZone = spawnZone,
                                                      spawnWithPrevious =
                                                          thisSpawnGroup[
                                                              "with-previous"]
                                                              .AsBool,
                                                      spawnGroupDelay =
                                                          thisSpawnGroup["group-delay"].AsFloat,
                                                      spawnDelay = thisSpawnGroup["spawn-delay"]
                                                          .AsFloat,
                                                      setupEnemySpawns = enemySpawns
                                                  };

                        // Add power-up spawns if there are any
                        if (thisSpawnGroup["powerups"])
                        {
                            int powerupCount = thisSpawnGroup["powerups"].Count;
                            SetupPowerupSpawn[] powerupSpawns = new SetupPowerupSpawn[powerupCount];

                            // Iterate over this spawn group's powerup spawns
                            for (int powerup = 0; powerup < powerupCount; ++powerup)
                            {
                                JSONNode thisPowerup = thisSpawnGroup["powerups"][powerup];

                                powerupSpawns[powerup] = new SetupPowerupSpawn
                                                         {
                                                             powerupPrefab =
                                                                 reference
                                                                     .waveManager
                                                                     .powerupPrefabs
                                                                     [
                                                                     thisPowerup
                                                                         ["powerup-prefab"]
                                                                         .AsInt],
                                                             spawnTimeDelayBase = 1.0f,
                                                             spawnTimeDelayRange = 1.0f
                                                         };
                            }

                            spawnGroups[spawnGroup].setupPowerupSpawns = powerupSpawns;
                        }
                    }

                    subwaves[subwave].setupSpawnGroups = spawnGroups;
                }

                waves[wave].subwaves = subwaves;
            }

            Debug.Log("Loading waves complete.");

            return waves;
        }
    }
}