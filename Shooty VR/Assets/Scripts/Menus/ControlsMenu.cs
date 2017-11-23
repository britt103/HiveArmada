//Name: Chad Johnson
//Student ID: 1763718
//Email: johns428@mail.chapman.edu
//Course: CPSC 340-01, CPSC-344-01
//Assignment: Group Project
//Purpose: Control interactions and navigation with controls menu

using UnityEngine;
using Hive.Armada.Player;

public class ControlsMenu : MonoBehaviour {
    private Tooltip tooltip;

    /// <summary>
    /// Activate controller highlighting
    /// </summary>
    private void OnEnable()
    {
        tooltip = FindObjectOfType<ShipController>().transform.parent.GetComponentInChildren<Tooltip>();
        tooltip.ShowFireButton();
        tooltip.ShowPowerupButton();
    }

    /// <summary>
    /// Back button pressed; navigates to options menu
    /// </summary>
    public void OnBackButton()
    {
        GameObject.Find("Main Canvas").transform.Find("Options Menu").gameObject.SetActive(true);

        tooltip.HideAll();
 
        gameObject.SetActive(false);
    }
}
