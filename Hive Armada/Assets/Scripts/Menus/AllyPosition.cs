using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyPosition : MonoBehaviour {

    public AudioSource allySource;

    public AudioClip[] allyClips;

    public GameObject[] allyPosition;

    [Header("Hovering")]
    public float xMax;

    public float yMax;

    public float movingSpeed;

    /// <summary>
    /// Variables for hovering
    /// </summary>
    private float theta;

    private Vector3 posA;

    private Vector3 posB;

    /// <summary>
    /// Whether the ally can begin hovering
    /// </summary>
    private bool hoverReady;

    private GameObject player;

    /// <summary>
    /// Selects random position to place ally and play audio.
    /// </summary>
    void Start () {
        player = GameObject.FindGameObjectWithTag("MainCamera");
        int pos = Random.Range(0, allyPosition.Length);
        gameObject.transform.position = allyPosition[pos].transform.position;
        Hover();
        //StartCoroutine(PlayAllyAudio(pos));
	}
    /// <summary>
    /// Moves ship up and down over time.
    /// </summary>
	void Update()
    {
        transform.LookAt(player.transform);
        if (hoverReady)
        {
            transform.position = Vector3.Lerp(posA, posB, (Mathf.Sin(theta) + 1.0f) / 2.0f);

            theta += movingSpeed * Time.deltaTime;

            if (theta > Mathf.PI)
            {
                theta -= Mathf.PI * 2;
            }
        }
    }

    /// <summary>
    /// Function that creates 2 vector 3's to float up and down with a Sin()
    /// </summary>
    private void Hover()
    {
        posA = new Vector3(transform.position.x + xMax / 100,
                           transform.position.y + yMax / 100,
                           transform.position.z);

        posB = new Vector3(transform.position.x - xMax / 100,
                           transform.position.y - yMax / 100,
                           transform.position.z);

        theta = 0.0f;
        hoverReady = true;
    }

    /// <summary>
    /// Plays ally audio.
    /// </summary>
    /// <param name="clip">which clip to play</param>
    private IEnumerator PlayAllyAudio(int clip)
    {
        allySource.PlayOneShot(allyClips[clip]);
        yield return new WaitForSeconds(allyClips[clip].length); 
    }
}
