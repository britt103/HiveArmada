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
using Hive.Armada.Enemies;
using Hive.Armada.PowerUps;
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
        /// Reference to Menu Transition Manager.
        /// </summary>
        public MenuTransitionManager transitionManager;

        /// <summary>
        /// Reference to menu to go to when back is pressed.
        /// </summary>
        public GameObject backMenuGO;

        /// <summary>
        /// Reference to player transform for Options Menu.
        /// </summary>
        public Transform backMenuTransform;

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
        /// Reference to environment object on top of table.
        /// </summary>
        public GameObject tableDecoration;

        [Header("Content")]
        /// <summary>
        /// Prefab for Lexicon entry button.
        /// </summary>
        public GameObject entryButtonPrefab;

        [Tooltip("Number of content entries that will start visible within viewport")]
        /// <summary>
        /// Number of content entries that will start visible within viewport.
        /// </summary>
        public int numStartingEntries;

        /// <summary>
        /// References to prefabs used in entries. Order must match order in Lexicon.txt.
        /// </summary>
        public GameObject[] entryPrefabs;

        /// <summary>
        /// Point at which entry prefabs are displayed.
        /// </summary>
        public Transform entryPrefabPoint;

        /// <summary>
        /// Reference to currently displayed entry prefab.
        /// </summary>
        private GameObject currEntryPrefab;

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
            tableDecoration.SetActive(false);
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
            foreach (string entryName in unlockData.GetUnlocks())
            {
                Unlock(entryName);
            }
            WriteLexiconFile();
            unlockData.ClearUnlocks();
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
                tableDecoration.SetActive(true);
                FindObjectOfType<RoomTransport>().Transport(backMenuTransform, gameObject,
                    backMenuGO);
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

                if (i > numStartingEntries - 1)
                {
                    entryButton.layer = LayerMask.NameToLayer("Default");
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
                currEntryPrefab = Instantiate(entryPrefabs[entryId], entryPrefabPoint);

                if (currEntryPrefab.GetComponentInChildren<PowerUp>())
                {
                    currEntryPrefab.GetComponentInChildren<PowerUp>().lifeTime = Mathf.Infinity;
                }
                //else if (currEntryPrefab.GetComponentInChildren<Enemy>())
                //{
                //    currEntryPrefab.GetComponentInChildren<Enemy>().
                //}
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

            Destroy(currEntryPrefab);

            entryOpen = false;
        }
    }
}
