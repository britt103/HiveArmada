//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// MainMenu controls interactions with the Main Menu.
//
//=============================================================================

using UnityEngine;

namespace Hive.Armada.Menu
{
    /// <summary>
    /// Contains navigation functions for the Start and Options menu and prompting of applcation 
    /// exit on Main Menu.
    /// </summary>
    public class MainMenu : MonoBehaviour
    {
        /// <summary>
        /// Start button pressed. Navigate to Start Menu.
        /// </summary>
        public void PressStart()
        {
            GameObject.Find("Main Canvas").transform.Find("Start Menu").gameObject
                    .SetActive(true);
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Options button pressed. Navigate to Options Menu.
        /// </summary>
        public void PressOptions()
        {
            GameObject.Find("Main Canvas").transform.Find("Options Menu").gameObject
                    .SetActive(true);
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Quit button pressed. Exit application.
        /// </summary>
        public void PressQuit()
        {

        }
    }
}