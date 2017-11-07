//Name: Chad Johnson
//Student ID: 1763718
//Email: johns428@mail.chapman.edu
//Course: CPSC 340-01, CPSC-344-01
//Assignment: Group Project
//Purpose: Control interactions and navigation with main menu

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {
    /// <summary>
    /// Start button pressed; navigated to start menu
    /// </summary>
    public void OnStartButton()
    {
        GameObject.Find("Main Canvas").transform.Find("Start Menu").gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Options button pressed; navigate to options menu
    /// </summary>
    public void OnOptionsButton()
    {
        GameObject.Find("Main Canvas").transform.Find("Options Menu").gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Quit button pressed; exits application
    /// </summary>
    public void OnQuitButton()
    {
        
    }
}
