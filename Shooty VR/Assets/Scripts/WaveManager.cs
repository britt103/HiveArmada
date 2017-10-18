using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour {

    public int countSpawn, currWave, currSpawn, currDead;
    public GameObject mSpawn;

	// Use this for initialization
	void Start () {
        var waveSettings = mSpawn.GetComponent<Spawn>();
	}
	
	// Update is called once per frame
	void Update () {
        if (currSpawn == countSpawn) mSpawn.SetActive(false);
        if (currDead == countSpawn) StartCoroutine(roundWait());
	}

    IEnumerator roundWait()
    {
        yield return new WaitForSeconds(5);
        mSpawn.SetActive(true);
    }
}
