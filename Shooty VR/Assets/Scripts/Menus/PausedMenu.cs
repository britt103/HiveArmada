//Name: Chad Johnson
//Student ID: 1763718
//Email: johns428@mail.chapman.edu
//Course: CPSC 340-01, CPSC-344-01
//Assignment: Group Project
//Purpose: Control interactions and navigation with paused menu

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausedMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnResumeButton()
    {
        gameObject.SetActive(false);
    }

    public void OnRestartButton()
    {

    }

    public void OnQuitMainMenuButton()
    {

    }

    public void OnQuitDesktopButton()
    {

    }
}
