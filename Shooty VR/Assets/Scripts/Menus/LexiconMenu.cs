//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// LexiconMenu controls interactions with the Lexicon Menu.
//
//=============================================================================

using UnityEngine;
using UnityEngine.UI;

namespace Hive.Armada.Menus
{
    /// <summary>
    /// Controls interactions with Lexicon Menu.
    /// </summary>
    public class LexiconMenu : MonoBehaviour
    {
        /// <summary>
        /// Name of item on each page.
        /// </summary>
        public string[] pageNames;

        /// <summary>
        /// Text description for item on each page.
        /// </summary>
        public string[] pageTexts;

        /// <summary>
        /// Image for item on each page.
        /// </summary>
        public Material[] pageImages;

        /// <summary>
        /// States of whether each page is locked.
        /// </summary>
        public bool[] pagesLocked;

        /// <summary>
        /// Name used for locked pages.
        /// </summary>
        public string lockedName;

        /// <summary>
        /// Image used for locked pages.
        /// </summary>
        public Material lockedImage;

        /// <summary>
        /// Number of pages in pages array.
        /// </summary>
        private int numPages;

        /// <summary>
        /// Index position of currently loaded page.
        /// </summary>
        private int currIndex;

        /// <summary>
        /// Page name text component in Lexicon Menu.
        /// </summary>
        public Text pageName;

        /// <summary>
        /// Page text component in Lexicon Menu.
        /// </summary>
        public Text pageText;

        /// <summary>
        /// Plane mesh renderer component in Lexicon Menu.
        /// </summary>
        public MeshRenderer pageImageRenderer;

        /// <summary>
        /// Reference to Prev button in Lexicon Menu.
        /// </summary>
        public GameObject prevButtonGO;

        /// <summary>
        /// Reference to Next button in Lexicon Menu.
        /// </summary>
        public GameObject nextButtonGO;

        /// <summary>
        /// Load first page.
        /// </summary>
        private void OnEnable()
        {
            //numPages = pages.Length;
            numPages = pageNames.Length;
            currIndex = 0;
            prevButtonGO.SetActive(false);
            LoadPage();
        }

        /// <summary>
        /// Back button pressed. Navigate to Options Menu. Reset current page.
        /// </summary>
        public void PressBack()
        {
            GameObject.Find("Main Canvas").transform.Find("Options Menu").gameObject
                    .SetActive(true);
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Prev button pressed. Transition to the previous page.
        /// </summary>
        public void PressPrev()
        {
            currIndex = Mathf.Max(currIndex - 1, 0);
            if (currIndex == 0)
            {
                prevButtonGO.SetActive(false);
            }
            if (currIndex == numPages - 2)
            {
                nextButtonGO.SetActive(true);
            }
            LoadPage();
        }

        /// <summary>
        /// Next button pressed. Transition to the next page.
        /// </summary>
        public void PressNext()
        {
            currIndex = Mathf.Min(currIndex + 1, numPages - 1);
            if(currIndex == 1)
            {
                prevButtonGO.SetActive(true);
            }
            if (currIndex == numPages - 1)
            {
                nextButtonGO.SetActive(false);
            }
            LoadPage();
        }

        /// <summary>
        /// Load selected page into Lexicon Menu if unlocked.
        /// </summary>
        /// <param name="page">page to be loaded.</param>
        private void LoadPage()
        {
            if (pagesLocked[currIndex])
            {
                pageName.text = lockedName;
                pageText.text = "";
                pageImageRenderer.material = lockedImage;
            }
            else
            {
                pageName.text = pageNames[currIndex];
                pageText.text = pageTexts[currIndex];
                pageImageRenderer.material = pageImages[currIndex];
            }
        }

        /// <summary>
        /// Unlock the page associated with the provided name.
        /// </summary>
        /// <param name="name">name of page to unlock.</param>
        public void Unlock(string name)
        {
            for(int i = 0; i < numPages; ++i)
            {
                if(name == pageNames[i])
                {
                    pagesLocked[i] = false;
                }
            }
        }
    }
}
