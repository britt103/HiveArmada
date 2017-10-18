using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject CORNER1;
    public GameObject CORNER2;
    public List<GameObject> HazardList;
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;
    public bool canSpawn;

	// Use this for initialization
	void Start ()
    {
        canSpawn = true;
        StartCoroutine(SpawnWaves());
	}

    public IEnumerator SpawnWaves()
    {
        //yield return new WaitForSeconds(startWait);
        while (canSpawn)
        {
            for (int i = 0; i < hazardCount; i++)
            {
                Vector3 spawnPosition = new Vector3(Random.Range(CORNER1.transform.position.x, CORNER2.transform.position.x), Random.Range(CORNER1.transform.position.y, CORNER2.transform.position.y), Random.Range(CORNER1.transform.position.z, CORNER2.transform.position.z));
                Instantiate(HazardList[Random.Range(0, HazardList.Count - 1)], spawnPosition, Quaternion.identity);
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);
        }
    }
}
