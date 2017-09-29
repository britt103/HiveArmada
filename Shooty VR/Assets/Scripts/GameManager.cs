using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using UnityEngine.UI;

/// <summary>
/// Chad Johnson
/// 1763718
/// johns428@mail.chapman.edu
/// CPSC 340-01, CPSC 344-01
/// 
/// General game mode/status manager: activates/deactivates relevent objects
/// </summary>

public class GameManager : MonoBehaviour
{
    public GameObject ship;
    public GameObject startMenu;
    public GameObject controller;
    public GameObject countdownCanvases;
    public GameObject enemyParent;
    public int kills = 0;

    private bool shipPickedUp;

    // Use this for initialization
    void Start()
    {
        //enemies = GameObject.FindGameObjectsWithTag("Enemy");

        //for (int i = 0; i < enemies.Length; ++i)
        //{
        //    enemies[i].SetActive(false);
        //}
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        ship.SetActive(true);
        startMenu.SetActive(false);
        controller.GetComponent<VRTK_Pointer>().enabled = false;
        controller.GetComponent<VRTK_StraightPointerRenderer>().enabled = false;
        controller.GetComponent<VRTK_UIPointer>().enabled = false;

        ship.GetComponent<ShootyVR.ShipController>().InteractableObjectGrabbed += new InteractableObjectEventHandler(ShipPickUp);

        //for (int i = 0; i < enemies.Length; ++i)
        //{
        //    enemies[i].SetActive(true);
        //}

        
    }

    public void gameOver()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Test01");


        //kills = 0;
        //startMenu.SetActive(true);
        //ship.GetComponent<ShootyVR.ShipController>().ForceStopInteracting();
        //ship.SetActive(false);
        //controller.GetComponent<VRTK_Pointer>().enabled = true;
        //controller.GetComponent<VRTK_StraightPointerRenderer>().enabled = true;
        //controller.GetComponent<VRTK_UIPointer>().enabled = true;

        //foreach (GameObject obj in enemies)
        //{
        //    obj.SetActive(false);
        //}
    }

    private void ShipPickUp(object sender, InteractableObjectEventArgs args)
    {
        if (!shipPickedUp)
        {
            StartCoroutine(Begin());
        }
        shipPickedUp = true;
    }

    private IEnumerator Begin()
    {
        for (int i = 0; i < countdownCanvases.transform.childCount; ++i)
        {
            countdownCanvases.transform.GetChild(i).gameObject.SetActive(true);
        }

        for (int i = 5; i > 0; i--)
        {
            for (int j = 0; j < countdownCanvases.transform.childCount; ++j)
            {
                countdownCanvases.transform.GetChild(j).gameObject.GetComponentInChildren<Text>().text = i.ToString();
            }
            yield return new WaitForSeconds(1.0f);
        }

        for (int i = 0; i < countdownCanvases.transform.childCount; ++i)
        {
            countdownCanvases.transform.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < enemyParent.transform.childCount; ++i)
        {
            enemyParent.transform.GetChild(i).gameObject.SetActive(true);
        }
    }
}