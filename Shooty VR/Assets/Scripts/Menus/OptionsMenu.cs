//Name: Chad Johnson
//Student ID: 1763718
//Email: johns428@mail.chapman.edu
//Course: CPSC 340-01, CPSC-344-01
//Assignment: Group Project
//Purpose: Control interactions and navigation with options menu

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : MonoBehaviour {

    public AudioSource source;
    public AudioClip[] clips;
    /// <summary>
    /// Controls button pressed; navigates to controls menu
    /// </summary>
    public void OnControlsButton()
    {
        StartCoroutine(playOptionSound());
        GameObject.Find("Main Canvas").transform.Find("Controls Menu").gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Display button pressed; navigates to display menu
    /// </summary>
    public void OnDisplayButton()
    {
        StartCoroutine(playOptionSound());
        GameObject.Find("Main Canvas").transform.Find("Display Menu").gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Sound button pressed; navigates to sound menu
    /// </summary>
    public void OnSoundButton()
    {
        StartCoroutine(playOptionSound());
        GameObject.Find("Main Canvas").transform.Find("Sound Menu").gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Lexicon button pressed; navigates to lexicon menu
    /// </summary>
    public void OnLexiconButton()
    {
        StartCoroutine(playOptionSound());
        GameObject.Find("Main Canvas").transform.Find("Lexicon Menu").gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Intro button pressed; plays intro cinematic
    /// </summary>
    public void OnIntroButton()
    {
        StartCoroutine(playOptionSound());
        Debug.Log("Intro button pressed");
    }

    /// <summary>
    /// Credits button pressed; rolls credits
    /// </summary>
    public void OnCreditsButton()
    {
        StartCoroutine(playOptionSound());
        Debug.Log("Credits button pressed");
    }

    /// <summary>
    /// Back button pressed; navigates to main menu
    /// </summary>
    public void OnBackButton()
    {
        StartCoroutine(playBackSound());
        GameObject.Find("Main Canvas").transform.Find("Main Menu").gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    IEnumerator playOptionSound()
    {
        source.PlayOneShot(clips[0]);
        yield return new WaitForSeconds(1);
    }

    IEnumerator playBackSound()
    {
        source.PlayOneShot(clips[1]);
        yield return new WaitForSeconds(1);
    }
}
