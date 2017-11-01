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

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnControlsButton()
    {
        GameObject.Find("Main Canvas").transform.Find("Controls Menu").gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnDisplayButton()
    {
        GameObject.Find("Main Canvas").transform.Find("Display Menu").gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnSoundButton()
    {
        GameObject.Find("Main Canvas").transform.Find("Sound Menu").gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnLexiconButton()
    {
        GameObject.Find("Main Canvas").transform.Find("Lexicon Menu").gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnIntroButton()
    {
        Debug.Log("Intro button pressed");
    }

    public void OnCreditsButton()
    {
        Debug.Log("Credits button pressed");
    }

    public void OnBackButton()
    {
        GameObject.Find("Main Canvas").transform.Find("Main Menu").gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
