//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// OptionsMenu controls interactions with the Options Menu.
//
//=============================================================================

using UnityEngine;

namespace Hive.Armada.Menu
{
    /// <summary>
    /// Controls interations with Options Menu.
    /// </summary>
    public class OptionsMenu : MonoBehaviour
    {
        /// <summary>
        /// Controls button pressed. Navigate to Controls Menu.
        /// </summary>
        public void PressControls()
        {
            GameObject.Find("Main Canvas").transform.Find("Controls Menu").gameObject.SetActive(true);
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Display button pressed. Navigate to Display Menu.
        /// </summary>
        public void PressDisplay()
        {
            GameObject.Find("Main Canvas").transform.Find("Display Menu").gameObject.SetActive(true);
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Sound button pressed. Navigate to Sound Menu.
        /// </summary>
        public void PressSound()
        {
            GameObject.Find("Main Canvas").transform.Find("Sound Menu").gameObject.SetActive(true);
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Lexicon button pressed. Navigate to Lexicon Menu.
        /// </summary>
        public void PressLexicon()
        {
            GameObject.Find("Main Canvas").transform.Find("Lexicon Menu").gameObject.SetActive(true);
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Intro button pressed. Play intro cinematic.
        /// </summary>
        public void PressIntro()
        {
            Debug.Log("Intro button pressed");
        }

        /// <summary>
        /// Credits button pressed. Roll credits.
        /// </summary>
        public void PressCredits()
        {
            Debug.Log("Credits button pressed");
        }

        /// <summary>
        /// Back button pressed. Navigate to Main Menu.
        /// </summary>
        public void PressBack()
        {
            GameObject.Find("Main Canvas").transform.Find("Main Menu").gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
