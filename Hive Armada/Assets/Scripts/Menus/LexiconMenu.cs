//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-440-1
// Group Project
//
// LexiconMenu controls interactions with the Lexicon Menu. The player
// can move through the scroll view by moving the vertical slider with the
// UIPointer. Find a powerup, enemy, etc. for the first time unlocks the 
// corresponding entry.
//
//=============================================================================

using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hive.Armada.Menus
{
    /// <summary>
    /// Controls interactions with Lexicon Menu.
    /// </summary>
    public class LexiconMenu : MonoBehaviour
    {
        [Header("References")]
        /// <summary>
        /// Reference to Menu Audio source.
        /// </summary>
        public AudioSource source;

        /// <summary>
        /// Clips to use with source.
        /// </summary>
    	public AudioClip[] clips;

        /// <summary>
        /// Refernce to Content gameObject in Scroll View.
        /// </summary>
        public GameObject contentGO;

        /// <summary>
        /// Reference to menu title text.
        /// </summary>
        public GameObject menuTitle;

        /// <summary>
        /// Reference to menu ScrollView.
        /// </summary>
        public GameObject scrollView;

        /// <summary>
        /// Reference to entry name text.
        /// </summary>
        public GameObject entryName;

        /// <summary>
        /// Reference to entry text text.
        /// </summary>
        public GameObject entryText;

        /// <summary>
        /// Prefab for Lexicon entry button.
        /// </summary>
        public GameObject entryButtonPrefab;

        /// <summary>
        /// State of whether an entry is currently open.
        /// </summary>
        private bool entryOpen = false;

        /// <summary>
        /// Object storing entry information.
        /// </summary>
        private LexiconEntryData entryData;

        /// <summary>
        /// Reference to Lexicon Unlock Data.
        /// </summary>
        private LexiconUnlockData unlockData;

        /// <summary>
        /// Read in Lexicon data and unlocks. Generate buttons for entries in scrollView.
        /// </summary>
        private void Start()
        {
            ReadLexiconFile();
            unlockData = FindObjectOfType<LexiconUnlockData>();
            UpdateUnlocks();
            GenerateContent();
        }

        /// <summary>
        /// Write LexiconEntryData to Json file.
        /// </summary>
        public void WriteLexiconFile()
        {
            File.WriteAllText(@"Lexicon.txt", JsonUtility.ToJson(entryData, true));
        }

        /// <summary>
        /// Read LexiconEntryData from Json file.
        /// </summary>
        public void ReadLexiconFile()
        {
            string jsonString = File.ReadAllText(@"Lexicon.txt");
            entryData = JsonUtility.FromJson<LexiconEntryData>(jsonString);
        }

        /// <summary>
        /// Unlock entries using names from LexiconUnlockData. Update 
        /// LexiconEntryData Json file.
        /// </summary>
        private void UpdateUnlocks()
        {
            List<string> unlocks = unlockData.GiveUnlocks();
            foreach (string entryName in unlocks)
            {
                Unlock(entryName);
            }
            WriteLexiconFile();
        }

        /// <summary>
        /// Back button pressed. If entry is not open, navigate to Main Menu.
        /// Else, navigate to entry selection.
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
            for(int i = 0; i < entryData.numEntries; ++i)
            {
                if(name == entryData.entryNames[i] && entryData.entriesLocked[i])
                {
                    entryData.entriesLocked[i] = false;
                }
            }
        }

        /// <summary>
        /// Create entrie buttons as children of Content.
        /// </summary>
        private void GenerateContent()
        {
            for(int i = 0; i < entryData.numEntries; ++i)
            {
                GameObject entryButton = Instantiate(entryButtonPrefab, contentGO.transform);
                entryButton.GetComponent<LexiconEntryButton>().id = i;
                entryButton.GetComponent<LexiconEntryButton>().lexiconMenu = this;
                entryButton.GetComponent<UIHover>().source = source;
                if (entryData.entriesLocked[i])
                {
                    entryButton.transform.Find("Name").gameObject.GetComponent<Text>().text = entryData.lockedName;
                }
                else
                {
                    entryButton.transform.Find("Name").gameObject.GetComponent<Text>().text = entryData.entryNames[i];
                }
            }
        }

        /// <summary>
        /// Open entry view and fill entry name and text with corresponding values.
        /// </summary>
        /// <param name="entryId">Index of selected entry.</param>
        public void OpenEntry(int entryId)
        {
            source.PlayOneShot(clips[0]);

            menuTitle.SetActive(false);
            scrollView.SetActive(false);
            entryName.SetActive(true);
            entryText.SetActive(true);

            if (entryData.entriesLocked[entryId])
            {
                entryName.GetComponent<Text>().text = entryData.lockedName;
                entryText.GetComponent<Text>().text = entryData.lockedText;
            }
            else
            {
                entryName.GetComponent<Text>().text = entryData.entryNames[entryId];
                entryText.GetComponent<Text>().text = entryData.entryTexts[entryId];
            }

            entryOpen = true;
        }

        /// <summary>
        /// Close entry view and return to entry selection.
        /// </summary>
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
