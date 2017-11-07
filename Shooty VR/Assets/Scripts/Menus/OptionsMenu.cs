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
    /// <summary>
    /// Controls button pressed; navigates to controls menu
    /// </summary>
    public void OnControlsButton()
    {
        GameObject.Find("Main Canvas").transform.Find("Controls Menu").gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Display button pressed; navigates to display menu
    /// </summary>
    public void OnDisplayButton()
    {
        GameObject.Find("Main Canvas").transform.Find("Display Menu").gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Sound button pressed; navigates to sound menu
    /// </summary>
    public void OnSoundButton()
    {
        GameObject.Find("Main Canvas").transform.Find("Sound Menu").gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Lexicon button pressed; navigates to lexicon menu
    /// </summary>
    public void OnLexiconButton()
    {
        GameObject.Find("Main Canvas").transform.Find("Lexicon Menu").gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Intro button pressed; plays intro cinematic
    /// </summary>
    public void OnIntroButton()
    {
        Debug.Log("Intro button pressed");
    }

    /// <summary>
    /// Credits button pressed; rolls credits
    /// </summary>
    public void OnCreditsButton()
    {
        Debug.Log("Credits button pressed");
    }

    /// <summary>
    /// Back button pressed; navigates to main menu
    /// </summary>
    public void OnBackButton()
    {
        GameObject.Find("Main Canvas").transform.Find("Main Menu").gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
