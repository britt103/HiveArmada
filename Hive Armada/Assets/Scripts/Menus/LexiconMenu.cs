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
using System.Linq;
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
        /// Reference to description text.
        /// </summary>
        public GameObject menuDescription;

        /// <summary>
        /// Reference to menu ScrollView game object.
        /// </summary>
        public GameObject scrollView;

        /// <summary>
        /// Reference to scrollview vertical scrollbar.
        /// </summary>
        public Scrollbar scrollBar;

        /// <summary>
        /// Reference to vertical slider.
        /// </summary>
        public Slider verticalSlider;

        /// <summary>
        /// Reference to entry name text.
        /// </summary>
        public GameObject entryName;

        /// <summary>
        /// Reference to entry text text.
        /// </summary>
        public GameObject entryText;

        /// <summary>
        /// Reference to category buttons/tabs.
        /// </summary>
        public GameObject[] categoryButtons;

        public int numFittableButtons = 3;

        /// <summary>
        /// Reference to environment object on top of table.
        /// </summary>
        public GameObject tableDecoration;

        [Header("Content")]
        /// <summary>
        /// Prefab for Lexicon entry button.
        /// </summary>
        public GameObject entryButtonPrefab;

        public GameObject entryButtonEmptyPrefab;

        /// <summary>
        /// References to prefabs used in powerup entries. Order must match Lexicon.txt.
        /// </summary>
        public GameObject[] powerupPrefabs;

        /// <summary>
        /// References to prefabs used in powerup entries. Order must match Lexicon.txt.
        /// </summary>
        public GameObject[] enemyPrefabs;

        /// <summary>
        /// References to prefabs used in powerup entries. Order must match Lexicon.txt.
        /// </summary>
        public GameObject[] weaponPrefabs;

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
        /// State of whether a category is currently open.
        /// </summary>
        private bool categoryOpen = false;

        /// <summary>
        /// Object storing entry information.
        /// </summary>
        private LexiconEntryData entryData;

        /// <summary>
        /// Reference to Lexicon Unlock Data.
        /// </summary>
        private LexiconUnlockData unlockData;

        /// <summary>
        /// Entry names of currently open category.
        /// </summary>
        private List<string> currNames = new List<string>();

        /// <summary>
        /// Entry texts of currently open category.
        /// </summary>
        private List<string> currTexts = new List<string>();

        /// <summary>
        /// Entry prefabs of currently open category.
        /// </summary>
        private List<GameObject> currPrefabs = new List<GameObject>();

        /// <summary>
        /// Locked entry states of currently open category.
        /// </summary>
        private List<bool> currLocked = new List<bool>();

        /// <summary>
        /// Variables used to make sure audio
        /// doesn't play over itself
        /// </summary>
        private int backCounter = 0;

        private int entryCounter = 0;

        private int categoryCounter = 0;

        /// <summary>
        /// Reference to the information button used for
        /// playing dialogue about the selected item
        /// </summary>
        public GameObject informationButton;

        /// <summary>
        /// Variable used to set the current category
        /// </summary>
        private string entryCategory;

        /// <summary>
        /// Variable used to set the current entry chosen
        /// </summary>
        private int entryValue;

        /// <summary>
        /// Audio arrays for each of the categories to choose
        /// from to play when an entry is selected
        /// </summary>
        public AudioClip[] powerupsAudio;

        public AudioClip[] enemiesAudio;

        public AudioClip[] weaponsAudio;

        /// <summary>
        /// Read in Lexicon data and unlocks.
        /// </summary>
        private void Start()
        {
            ReadLexiconFile();
            unlockData = FindObjectOfType<LexiconUnlockData>();
            UpdateUnlocks();
        }

        /// <summary>
        /// Disable game object near Lexicon area.
        /// </summary>
        private void OnEnable()
        {
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
            foreach (string entryName in unlockData.GetUnlocks()[0])
            {
                UnlockPowerup(entryName);
            }
            foreach (string entryName in unlockData.GetUnlocks()[1])
            {
                UnlockEnemy(entryName);
            }
            foreach (string entryName in unlockData.GetUnlocks()[2])
            {
                UnlockWeapon(entryName);
            }

            WriteLexiconFile();
            unlockData.ClearUnlocks();
        }

        /// <summary>
        /// Back button pressed. If a category or entry is open, close it. Else, transition
        /// to back menu.
        /// </summary>
        public void PressBack()
        {
            source.PlayOneShot(clips[1]);
            backCounter += 1;
            if (backCounter > 1)
            {
                source.Stop();
                source.PlayOneShot(clips[1]);
            }

            if (entryOpen)
            {
                CloseEntry();
            }
            else if (categoryOpen)
            {
                CloseCategory();
            }
            else
            {
                tableDecoration.SetActive(true);
                FindObjectOfType<RoomTransport>().Transport(backMenuTransform, gameObject,
                    backMenuGO);
            }
        }

        /// <summary>
        /// Unlock the powerup entry associated with the provided name.
        /// </summary>
        /// <param name="name">Name of entry to unlock.</param>
        public void UnlockPowerup(string name)
        {
            for (int i = 0; i < entryData.powerupNames.Length; ++i)
            {
                if (name == entryData.powerupNames[i] && entryData.powerupsLocked[i])
                {
                    entryData.powerupsLocked[i] = false;
                }
            }
        }

        /// <summary>
        /// Unlock the enemy entry associated with the provided name.
        /// </summary>
        /// <param name="name">Name of entry to unlock.</param>
        public void UnlockEnemy(string name)
        {
            for (int i = 0; i < entryData.enemyNames.Length; ++i)
            {
                if (name == entryData.enemyNames[i] && entryData.enemiesLocked[i])
                {
                    entryData.enemiesLocked[i] = false;
                }
            }
        }

        /// <summary>
        /// Unlock the weapon entry associated with the provided name.
        /// </summary>
        /// <param name="name">Name of entry to unlock.</param>
        public void UnlockWeapon(string name)
        {
            for (int i = 0; i < entryData.weaponNames.Length; ++i)
            {
                if (name == entryData.weaponNames[i] && entryData.weaponsLocked[i])
                {
                    entryData.weaponsLocked[i] = false;
                }
            }
        }

        /// <summary>
        /// Create entry buttons as children of Content and destroy previous buttons.
        /// </summary>
        private void GenerateContent()
        {
            for (int i = 0; i < contentGO.transform.childCount; i++)
            {
                Destroy(contentGO.transform.GetChild(i).gameObject);
            }

            int entries;
            bool tooFewEntries;
            if (currNames.Count <= numFittableButtons)
            {
                entries = numFittableButtons + 1;
                tooFewEntries = true;
                scrollBar.gameObject.GetComponent<BoxCollider>().enabled = false;
                verticalSlider.gameObject.SetActive(false);
            }
            else
            {
                entries = currNames.Capacity;
                tooFewEntries = false;
                scrollBar.gameObject.GetComponent<BoxCollider>().enabled = true;
                verticalSlider.gameObject.SetActive(true);
            }

            for (int i = 0; i < entries; ++i)
            {
                if (i >= numFittableButtons && tooFewEntries)
                {
                    GameObject entryButtonEmpty = Instantiate(entryButtonEmptyPrefab, contentGO.transform);
                }
                else
                {
                    GameObject entryButton = Instantiate(entryButtonPrefab, contentGO.transform);
                    entryButton.GetComponent<LexiconEntryButton>().id = i;
                    entryButton.GetComponent<LexiconEntryButton>().lexiconMenu = this;
                    entryButton.GetComponent<UIHover>().source = source;
                    if (currLocked[i])
                    {
                        entryButton.transform.Find("Name").gameObject.GetComponent<Text>().text = entryData.lockedName;
                    }
                    else
                    {
                        entryButton.transform.Find("Name").gameObject.GetComponent<Text>().text = currNames[i];
                    }
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
            entryCounter += 1;
            if (entryCounter > 1)
            {
                source.Stop();
                source.PlayOneShot(clips[0]);
            }

            menuTitle.SetActive(false);
            scrollView.SetActive(false);
            entryName.SetActive(true);
            entryText.SetActive(true);
            informationButton.SetActive(true);
            entryValue = entryId;

            foreach(GameObject categoryButton in categoryButtons)
            {
                categoryButton.SetActive(false);
            }

            if (currLocked[entryId])
            {
                entryName.GetComponent<Text>().text = entryData.lockedName;
                entryText.GetComponent<Text>().text = entryData.lockedText;
            }
            else
            {
                entryName.GetComponent<Text>().text = currNames[entryId];
                entryText.GetComponent<Text>().text = currTexts[entryId];
                currEntryPrefab = Instantiate(currPrefabs[entryId], entryPrefabPoint);
            }

            entryOpen = true;
        }

        /// <summary>
        /// Close entry view and return to category view.
        /// </summary>
        public void CloseEntry()
        {
            menuTitle.SetActive(true);
            entryName.SetActive(false);
            entryText.SetActive(false);
            scrollView.SetActive(true);
            informationButton.SetActive(false);

            foreach (GameObject categoryButton in categoryButtons)
            {
                categoryButton.SetActive(true);
            }

            Destroy(currEntryPrefab);

            entryOpen = false;
        }

        /// <summary>
        /// Set variables tracking currently open category.
        /// </summary>
        /// <param name="category">Name of category</param>
        private void SetCurrCategory(string category)
        {
            switch (category)
            {
                case "Powerups":
                    currNames = entryData.powerupNames.ToList();
                    currTexts = entryData.powerupTexts.ToList();
                    currPrefabs = powerupPrefabs.ToList();
                    currLocked = entryData.powerupsLocked.ToList();
                    break;
                case "Enemies":
                    currNames = entryData.enemyNames.ToList();
                    currTexts = entryData.enemyTexts.ToList();
                    currPrefabs = enemyPrefabs.ToList();
                    currLocked = entryData.enemiesLocked.ToList();
                    break;
                case "Weapons":
                    currNames = entryData.weaponNames.ToList();
                    currTexts = entryData.weaponTexts.ToList();
                    currPrefabs = weaponPrefabs.ToList();
                    currLocked = entryData.weaponsLocked.ToList();
                    break;
                default:
                    Debug.Log("ERROR: Lexicon menu category could not be identified.");
                    break;
            }
        }

        /// <summary>
        /// Open the category specified by parameter string.
        /// </summary>
        /// <param name="category">Name of category to open.</param>
        public void OpenCategory(string category)
        {
            source.PlayOneShot(clips[0]);
            categoryCounter += 1;
            if (categoryCounter > 1)
            {
                source.Stop();
                source.PlayOneShot(clips[0]);
            }

            menuDescription.SetActive(false);
            menuTitle.GetComponent<Text>().text = category;
            scrollView.SetActive(true);
            SetCurrCategory(category);
            GenerateContent();
            scrollBar.value = 1;
            categoryOpen = true;
            entryCategory = category;
        }

        /// <summary>
        /// Close currently open category.
        /// </summary>
        private void CloseCategory()
        {
            menuDescription.SetActive(true);
            menuTitle.GetComponent<Text>().text = "Lexicon";
            scrollView.SetActive(false);
            categoryOpen = false;
        }

        /// <summary>
        /// Play audio source based first on which category
        /// is open and second on the entryId that is assigned
        /// </summary>
        public void Information()
        {
            switch(entryCategory)
            {
                case "Powerups":
                    source.PlayOneShot(powerupsAudio[entryValue]);
                    break;
                case "Enemies":
                    source.PlayOneShot(enemiesAudio[entryValue]);
                    break;
                case "Weapons":
                    source.PlayOneShot(weaponsAudio[entryValue]);
                    break;
                default:
                    Debug.Log("ERROR: the category is not defined");
                    break;
            }
        }
    }
}
