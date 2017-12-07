//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// LexiconMenuScroll controls interactions with the Lexicon Menu.
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
        public Sprite[] entryImages;

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
        public Sprite lockedImage;

        /// <summary>
        /// Number of entrys in entries array.
        /// </summary>
        private int numEntries;

        /// <summary>
        /// Refernce to Content gameObject in Scroll View.
        /// </summary>
        public GameObject contentGO;

        /// <summary>
        /// Prefab for Lexicon entry.
        /// </summary>
        public GameObject entryPrefab;

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
            GameObject.Find("Main Canvas").transform.Find("Options Menu").gameObject
                    .SetActive(true);
            gameObject.SetActive(false);
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
                GameObject entry = Instantiate(entryPrefab, contentGO.transform);
                if (entriesLocked[i])
                {
                    entry.transform.Find("Name").gameObject.GetComponent<Text>().text = lockedName;
                    entry.transform.Find("Text").gameObject.GetComponent<Text>().text = lockedText;
                    entry.transform.Find("Image").gameObject.GetComponent<Image>().sprite = lockedImage;
                }
                else
                {
                    entry.transform.Find("Name").gameObject.GetComponent<Text>().text = entryNames[i];
                    entry.transform.Find("Text").gameObject.GetComponent<Text>().text = entryTexts[i];
                    entry.transform.Find("Image Plane").gameObject.GetComponent<Image>().sprite = entryImages[i];
                }
            }
        }
    }
}
