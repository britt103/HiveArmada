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
    /// <summary>
    /// Resume button pressed; resumes play
    /// </summary>
    public void OnResumeButton()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Restart button pressed; navigates to start menu
    /// </summary>
    public void OnRestartButton()
    {

    }

    /// <summary>
    /// Quit to main menu button pressed; navigates to main menu
    /// </summary>
    public void OnQuitMainMenuButton()
    {

    }

    /// <summary>
    /// Quit to desktop button; exits application
    /// </summary>
    public void OnQuitDesktopButton()
    {

    }
}
