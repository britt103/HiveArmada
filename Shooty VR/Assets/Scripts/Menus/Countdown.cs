//Name: Chad Johnson
//Student ID: 1763718
//Email: johns428@mail.chapman.edu
//Course: CPSC 340-01, CPSC-344-01
//Assignment: Group Project
//Purpose: Control visuals and timing of countdown timer

using System.Collections;
using System.Collections.Generic;
using Hive.Armada.Game;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    public Text countdownText;
    public AudioSource countdownAudioSource;

    /// <summary>
    /// Run when activated
    /// </summary>
    private void OnEnable()
    {
        StartCoroutine(Run());
    }

    /// <summary>
    /// Changes countdown timer texts based on time, then starts spawner
    /// </summary>
    /// <returns></returns>
    private IEnumerator Run()
    {
        countdownAudioSource.Play();
        for (int i = 5; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1.0f);
        }

        FindObjectOfType<WaveManager>().Run();
        gameObject.SetActive(false);
    }
}

