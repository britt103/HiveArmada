//Name: Chad Johnson
//Student ID: 1763718
//Email: johns428@mail.chapman.edu
//Course: CPSC 340-01, CPSC-344-01
//Assignment: Group Project
//Purpose: Control visuals and timing of countdown timer

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    public Text countdownText;

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
        for (int i = 5; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1.0f);
        }

        FindObjectOfType<Hive.Armada.Game.Spawner>().Run();
        gameObject.SetActive(false);
    }
}

