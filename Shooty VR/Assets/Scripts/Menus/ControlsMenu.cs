//Name: Chad Johnson
//Student ID: 1763718
//Email: johns428@mail.chapman.edu
//Course: CPSC 340-01, CPSC-344-01
//Assignment: Group Project
//Purpose: Control interactions and navigation with controls menu

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsMenu : MonoBehaviour {
    private ControlsHighlighter[] chs;

    /// <summary>
    /// Activate controller highlighting
    /// </summary>
    private void Awake()
    {
        chs = FindObjectsOfType<ControlsHighlighter>();
        foreach(ControlsHighlighter ch in chs)
        {
            ch.FireOn();
            ch.PowerupOn();
        }
    }

    /// <summary>
    /// Back button pressed; navigates to options menu
    /// </summary>
    public void OnBackButton()
    {
        GameObject.Find("Main Canvas").transform.Find("Options Menu").gameObject.SetActive(true);

        foreach (ControlsHighlighter ch in chs)
        {
            ch.AllOff();
        }

        gameObject.SetActive(false);
    }
}
