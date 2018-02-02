//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// LexiconMenuScroll controls interactions with the Lexicon Menu. The player
// can move through the scroll view by moving the vertical slider with the
// UIPointer. Find a powerup, enemy, etc. for the first time unlocks the 
// corresponding entry (not yet implemented).
//
//=============================================================================

using UnityEngine;
using UnityEngine.UI;

namespace Hive.Armada.Menus
{
    /// <summary>
    /// Controls interactions with Lexicon Menu.
    /// </summary>
    public class LexiconMenuScroll : MonoBehaviour
    {
        /// <summary>
        /// Reference to Menu Audio source.
        /// </summary>
        public AudioSource source;

        /// <summary>
        /// Clips to use with source.
        /// </summary>
    	public AudioClip[] clips;

        /// <summary>
        /// Name of item on each entry.
        /// </summary>
        public string[] entryNames;

        /// <summary>
        /// Text description for item on each entry.
        /// </summary>
        public string[] entryTexts;

        /// <summary>
        /// Image for item on each entry.
        /// </summary>
        //public Sprite[] entryImages;

        /// <summary>
        /// States of whether each entry is locked.
        /// </summary>
        public bool[] entriesLocked;

        /// <summary>
        /// Name used for locked entries.
        /// </summary>
        public string lockedName;

        /// <summary>
        /// Text used for locked entries.
        /// </summary>
        public string lockedText;

        /// <summary>
        /// Image used for locked entries.
        /// </summary>
        //public Sprite lockedImage;

        /// <summary>
        /// Number of entrys in entries array.
        /// </summary>
        private int numEntries;

        /// <summary>
        /// Refernce to Content gameObject in Scroll View.
        /// </summary>
        public GameObject contentGO;

        public GameObject menuTitle;

        public GameObject scrollView;

        public GameObject entryName;

        public GameObject entryText;

        /// <summary>
        /// Prefab for Lexicon entry button.
        /// </summary>
        public GameObject entryButtonPrefab;

        private bool entryOpen = false;

        /// <summary>
        /// Load first entry.
        /// </summary>
        private void Awake()
        {
            numEntries = entryNames.Length;
            GenerateContent();
        }

        /// <summary>
        /// Back button pressed. Navigate to Options Menu. Reset current entry.
        /// </summary>
        public void PressBack()
        {
            source.PlayOneShot(clips[1]);
            if (!entryOpen)
            {
                GameObject.Find("Main Canvas").transform.Find("Options Menu").gameObject
                    .SetActive(true);
                gameObject.SetActive(false);
            }
            else
            {
                CloseEntry();
            }
        }

        /// <summary>
        /// Unlock the entry associated with the provided name.
        /// </summary>
        /// <param name="name">name of entry to unlock.</param>
        public void Unlock(string name)
        {
            for(int i = 0; i < numEntries; ++i)
            {
                if(name == entryNames[i] && entriesLocked[i])
                {
                    entriesLocked[i] = false;
                }
            }
        }

        /// <summary>
        /// Create entries as children of Content.
        /// </summary>
        private void GenerateContent()
        {
            for(int i = 0; i < numEntries; ++i)
            {
                GameObject entryButton = Instantiate(entryButtonPrefab, contentGO.transform);
                entryButton.GetComponent<LexiconEntryButton>().id = i;
                entryButton.GetComponent<LexiconEntryButton>().lexiconMenu = this;
                entryButton.GetComponent<UIHover>().source = source;
                if (entriesLocked[i])
                {
                    entryButton.transform.Find("Name").gameObject.GetComponent<Text>().text = lockedName;
                }
                else
                {
                    entryButton.transform.Find("Name").gameObject.GetComponent<Text>().text = entryNames[i];
                }
            }
        }

        public void OpenEntry(int entryId)
        {
            source.PlayOneShot(clips[0]);

            menuTitle.SetActive(false);
            scrollView.SetActive(false);
            entryName.SetActive(true);
            entryText.SetActive(true);

            if (entriesLocked[entryId])
            {
                entryName.GetComponent<Text>().text = lockedName;
                entryText.GetComponent<Text>().text = lockedText;
            }
            else
            {
                entryName.GetComponent<Text>().text = entryNames[entryId];
                entryText.GetComponent<Text>().text = entryTexts[entryId];
            }

            entryOpen = true;
        }

        public void CloseEntry()
        {
            menuTitle.SetActive(true);
            entryName.SetActive(false);
            entryText.SetActive(false);
            scrollView.SetActive(true);

            entryOpen = false;
        }
    }
}
