//Name: Chad Johnson
//Student ID: 1763718
//Email: johns428@mail.chapman.edu
//Course: CPSC 340-01, CPSC-344-01
//Assignment: Group Project
//Purpose: Control interactions and navigation with lexicon menu

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LexiconMenu : MonoBehaviour {

    public AudioSource source;
    public AudioClip[] clips;
    /// <summary>
    /// Back button pressed; navigated to options menu
    /// </summary>
    public void OnBackButton()
    {
        StartCoroutine(playBackSound());
        GameObject.Find("Main Canvas").transform.Find("Options Menu").gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    IEnumerator playBackSound()
    {
        source.PlayOneShot(clips[0]);
        yield return new WaitForSeconds(1);
    }
}
