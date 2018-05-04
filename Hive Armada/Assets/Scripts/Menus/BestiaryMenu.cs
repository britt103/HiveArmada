//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-440-1
// Group Project
//
// BestiaryMenu controls interactions with the Bestiary Menu. The player
// can move through the scroll view by moving the vertical slider with the
// UIPointer. Find a powerup, enemy, etc. for the first time unlocks the 
// corresponding entry.
//
//=============================================================================

using System.IO;
using System.Linq;
using System.Collections.Generic;
using Hive.Armada.Game;
using UnityEngine;
using UnityEngine.UI;

namespace Hive.Armada.Menus
{
    /// <summary>
    /// Controls interactions with Bestiary Menu.
    /// </summary>
    public class BestiaryMenu : MonoBehaviour
    {
        /// <summary>
        /// Reference to ReferenceManager.
        /// </summary>
        private ReferenceManager reference;

        /// <summary>
        /// Reference to Menu Transition Manager.
        /// </summary>
        [Header("References")]
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
        /// Reference to enemy specific audio
        /// </summary>
        public AudioSource zenaSource;

        /// <summary>
        /// Refernce to Content gameObject in Scroll View.
        /// </summary>
        public GameObject contentGO;

        /// <summary>
        /// Reference to menu title text.
        /// </summary>
        public GameObject menuTitle;

        /// <summary>
        /// Reference to menu ScrollView game object.
        /// </summary>
        public GameObject scrollView;

        /// <summary>
        /// Reference to scrollview vertical scrollbar.
        /// </summary>
        public Scrollbar scrollbar;

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
        /// Number of buttons that are completely visible in view at a time.
        /// </summary>
        public int numFittableButtons = 3;

        /// <summary>
        /// Reference to environment object on top of table.
        /// </summary>
        public GameObject armadaPreviewGO;

        /// <summary>
        /// Y rotation of armada.
        /// </summary>
        private float armadaYRotation;

        public ScrollRect scrollRect;

        public GameObject previewLighting;

        private RectTransform[] buttons;

        /// <summary>
        /// Prefab for Bestiary entry button.
        /// </summary>
        [Header("Content")]
        public GameObject entryButtonPrefab;

        /// <summary>
        /// Prefab for empty Bestiary entry button.
        /// </summary>
        public GameObject entryButtonEmptyPrefab;

        /// <summary>
        /// Names of enemies.
        /// </summary>
        private List<string> enemyNames = new List<string>();

        /// <summary>
        /// Display names of enemies.
        /// </summary>
        private List<string> enemyDisplayNames = new List<string>();

        /// <summary>
        /// Texts of enemies.
        /// </summary>
        private List<string> enemyTexts = new List<string>();

        /// <summary>
        /// References to enemy preview prefabs. Order must match Bestiary.txt.
        /// </summary>
        public GameObject[] enemyPrefabs;

        /// <summary>
        /// Locked entry states.
        /// </summary>
        private List<bool> enemiesLocked = new List<bool>();

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
        private bool entryOpen;

        /// <summary>
        /// Object storing entry information.
        /// </summary>
        private BestiaryEntryData entryData;

        /// <summary>
        /// Reference to Bestiary Unlock Data.
        /// </summary>
        private BestiaryUnlockData unlockData;

        /// <summary>
        /// Reference to the information button used for
        /// playing dialogue about the selected item
        /// </summary>
        public GameObject informationButton;

        /// <summary>
        /// Variable used to set the current entry chosen
        /// </summary>
        private int entryValue;

        /// <summary>
        /// Audio arrays for each of the categories to choose
        /// from to play when an entry is selected
        /// </summary>
        public AudioClip[] enemiesAudio;

        private void Awake()
        {
            reference = FindObjectOfType<ReferenceManager>();
        }

        /// <summary>
        /// Read in data. Disable game object near Bestiary area. Update unlocks.
        /// </summary>
        private void OnEnable()
        {
            ReadBestiaryFile();
            unlockData = FindObjectOfType<BestiaryUnlockData>();
            UpdateUnlocks();
            GetEnemyData();
            GenerateContent();
            armadaYRotation = armadaPreviewGO.transform.rotation.eulerAngles.y;
            armadaPreviewGO.SetActive(false);
            scrollbar.value = 1;
        }

        /// <summary>
        /// Write BestiaryEntryData to Json file.
        /// </summary>
        public void WriteBestiaryFile()
        {
            File.WriteAllText(@"Bestiary.txt", JsonUtility.ToJson(entryData, true));
        }

        /// <summary>
        /// Read BestiaryEntryData from Json file.
        /// </summary>
        public void ReadBestiaryFile()
        {
            string jsonString = File.ReadAllText(@"Bestiary.txt");
            entryData = JsonUtility.FromJson<BestiaryEntryData>(jsonString);
        }

        /// <summary>
        /// Unlock entries using names from BestiaryUnlockData. Update
        /// BestiaryEntryData Json file.
        /// </summary>
        private void UpdateUnlocks()
        {
            foreach (string entryName in unlockData.GetUnlocks())
            {
                UnlockEnemy(entryName);
            }

            WriteBestiaryFile();
            unlockData.ClearUnlocks();
        }

        /// <summary>
        /// Back button pressed. If a category or entry is open, close it. Else, transition
        /// to back menu.
        /// </summary>
        public void PressBack()
        {
            zenaSource.Stop();
            source.Stop();
            source.PlayOneShot(reference.menuSounds.menuButtonSelectSound);

            if (entryOpen)
            {
                CloseEntry();
            }
            else
            {
                Vector3 rotation = armadaPreviewGO.transform.eulerAngles;
                rotation.y = armadaYRotation;
                previewLighting.SetActive(false);
                armadaPreviewGO.transform.rotation = Quaternion.Euler(rotation);
                armadaPreviewGO.SetActive(true);
                FindObjectOfType<RoomTransport>().Transport(backMenuTransform, gameObject,
                                                            backMenuGO);
            }
        }

        /// <summary>
        /// Unlock the enemy entry associated with the provided name.
        /// </summary>
        /// <param name="name"> Name of entry to unlock. </param>
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
            if (enemyNames.Count <= numFittableButtons)
            {
                entries = numFittableButtons + 1;
                tooFewEntries = true;
                scrollbar.gameObject.GetComponent<BoxCollider>().enabled = false;
                verticalSlider.gameObject.SetActive(false);
            }
            else
            {
                entries = enemyNames.Capacity;
                tooFewEntries = false;
                scrollbar.gameObject.GetComponent<BoxCollider>().enabled = true;
                verticalSlider.gameObject.SetActive(true);
            }

            buttons = new RectTransform[entries];

            for (int i = 0; i < entries; ++i)
            {
                if (i >= enemyNames.Count && tooFewEntries)
                {
                    GameObject entryButtonEmpty =
                        Instantiate(entryButtonEmptyPrefab, contentGO.transform);
                    buttons[i] = entryButtonEmpty.GetComponent<RectTransform>();
                    entryButtonEmpty.SetActive(false);
                }
                else
                {
                    GameObject entryButton = Instantiate(entryButtonPrefab, contentGO.transform);
                    buttons[i] = entryButton.GetComponent<RectTransform>();
                    entryButton.GetComponent<BestiaryEntryButton>().id = i;
                    entryButton.GetComponent<BestiaryEntryButton>().BestiaryMenu = this;
                    entryButton.GetComponent<UIHover>().source = source;
                    if (enemiesLocked[i])
                    {
                        entryButton.transform.Find("Name").gameObject.GetComponent<Text>().text =
                            entryData.lockedName;
                        entryButton.GetComponent<BoxCollider>().enabled = false;
                    }
                    else
                    {
                        entryButton.transform.Find("Name").gameObject.GetComponent<Text>().text =
                            enemyDisplayNames[i];
                    }
                }
            }
        }

        /// <summary>
        /// Open entry view and fill entry name and text with corresponding values.
        /// </summary>
        /// <param name="entryId"> Index of selected entry. </param>
        public void OpenEntry(int entryId)
        {
            source.PlayOneShot(reference.menuSounds.menuButtonSelectSound);

            menuTitle.SetActive(false);
            scrollView.SetActive(false);
            entryName.SetActive(true);
            entryText.SetActive(true);
            entryValue = entryId;

            if (enemiesLocked[entryId])
            {
                entryName.GetComponent<Text>().text = entryData.lockedName;
                entryText.GetComponent<Text>().text = entryData.lockedText;
                informationButton.SetActive(false);
            }
            else
            {
                entryName.GetComponent<Text>().text = enemyDisplayNames[entryId];
                entryText.GetComponent<Text>().text = enemyTexts[entryId];
                informationButton.SetActive(true);

                if (entryId == 0)
                {
                    previewLighting.SetActive(false);
                    armadaPreviewGO.GetComponent<SphereCollider>().enabled = true;
                    armadaPreviewGO.SetActive(true);
                }
                else
                {
                    currEntryPrefab = Instantiate(enemyPrefabs[entryId - 1], entryPrefabPoint);
                    previewLighting.SetActive(true);
                }
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

            if (entryName.GetComponent<Text>().text == enemyDisplayNames[0])
            {
                Vector3 rotation = armadaPreviewGO.transform.eulerAngles;
                rotation.y = armadaYRotation;
                armadaPreviewGO.transform.rotation = Quaternion.Euler(rotation);
                armadaPreviewGO.GetComponent<SphereCollider>().enabled = false;
                armadaPreviewGO.SetActive(false);
            }
            else
            {
                Destroy(currEntryPrefab);
            }

            previewLighting.SetActive(false);
            entryOpen = false;
        }

        /// <summary>
        /// Get data for enemy entries from entryData.
        /// </summary>
        private void GetEnemyData()
        {
            enemyNames = entryData.enemyNames.ToList();
            enemyDisplayNames = entryData.enemyDisplayNames.ToList();
            enemyTexts = entryData.enemyTexts.ToList();
            enemiesLocked = entryData.enemiesLocked.ToList();
        }

        /// <summary>
        /// Play audio source based first on which category
        /// is open and second on the entryId that is assigned
        /// </summary>
        public void Information()
        {
            if (zenaSource.isPlaying)
            {
                //do nothing
            }
            else
            {
                Debug.Log("play info");
                zenaSource.PlayOneShot(enemiesAudio[entryValue]);
            }
        }

    }
}