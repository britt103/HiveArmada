//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-440-1
// Group Project
//
// ShopMenu controls interactions with the Shop Menu. The player can spend
// Iridium to permanently unlock different unlockables, such as new weapons.
//
//=============================================================================

using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hive.Armada.Game;
using Hive.Armada.Data;

namespace Hive.Armada.Menus
{
    /// <summary>
    /// Controls interactions with Shop Menu.
    /// </summary>
    public class ShopMenu : MonoBehaviour
    {
        /// <summary>
        /// Reference to ReferenceManager.
        /// </summary>
        private ReferenceManager reference;

        /// <summary>
        /// Reference to GameSettings.
        /// </summary>
        private GameSettings gameSettings;

        /// <summary>
        /// Reference to Menu Audio source.
        /// </summary>
        /// [Header("References")]
        public AudioSource source;

        public AudioSource zenaSource;

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
        /// Reference to IridiumSystem.
        /// </summary>
        private IridiumSystem iridiumSystem;

        /// <summary>
        /// Reference to BestiaryUnlockData.
        /// </summary>
        private BestiaryUnlockData BestiaryUnlockData;

        /// <summary>
        /// Reference to menu title text.
        /// </summary>
        public GameObject menuTitle;

        /// <summary>
        /// Reference to category buttons/tabs.
        /// </summary>
        public GameObject[] categoryButtons;

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
        /// Reference to UI Cover GameObjects.
        /// </summary>
        public GameObject[] uiCovers;

        public GameObject previewLighting;

        [Header("Sections")]
        public GameObject itemSection;

        public GameObject iridiumSection;

        public GameObject purchaseSection;

        /// <summary>
        /// Reference to item name text.
        /// </summary>
        [Header("Text")]
        public Text itemName;

        /// <summary>
        /// Reference to item text text.
        /// </summary>
        public Text itemDescription;

        /// <summary>
        /// Reference to item cost text.
        /// </summary>
        public Text itemCost;

        /// <summary>
        /// Reference to iridium amount text.
        /// </summary>
        public Text iridiumAmount;

        public GameObject purchased;

        public GameObject setDefault;

        /// <summary>
        /// Reference to buy button.
        /// </summary>
        public GameObject buyButton;

        /// <summary>
        /// Reference to armada preview game object on top of table.
        /// </summary>
        public GameObject armadaPreviewGO;

        [Header("Content")]

        /// <summary>
        /// Prefab for item button.
        /// </summary>
        public GameObject itemButtonPrefab;

        /// <summary>
        /// Prefab for empty item button.
        /// </summary>
        public GameObject itemButtonEmptyPrefab;

        /// <summary>
        /// Reference to transform to use for preview prefab instantiation.
        /// </summary>
        public Transform itemPrefabPoint;

        /// <summary>
        /// Number of buttons that are completely visible in view at a time.
        /// </summary>
        public int numFittableButtons = 3;

        /// <summary>
        /// Reference to scroll view content GO.
        /// </summary>
        public GameObject contentGO;

        /// <summary>
        /// References to prefabs used in weapon entries.
        /// </summary>
        public List<GameObject> weaponPrefabs;

        /// <summary>
        /// Stat descriptions for each weapon. Order must match Iridium.txt.
        /// </summary>
        [TextArea]
        public List<string> weaponStats;

        /// <summary>
        /// References to prefabs used in skin entries.
        /// </summary>
        public List<GameObject> skinPrefabs;

        /// <summary>
        /// Currently selected ship skin.
        /// </summary>
        private int selectedSkin;

        /// <summary>
        /// Enum of default skin; meant to initially unlocked skin.
        /// </summary>
        public GameSettings.Skin defaultSkin;

        /// <summary>
        /// Id of currently open item.
        /// </summary>
        private int currItemId;

        /// <summary>
        /// name of currently open category.
        /// </summary>
        private string currCategory;

        /// <summary>
        /// Item names of currently open category.
        /// </summary>
        private List<string> currNames = new List<string>();

        /// <summary>
        /// Item display names of currently open category.
        /// </summary>
        private List<string> currDisplayNames = new List<string>();

        /// <summary>
        /// Item texts of currently open category.
        /// </summary>
        private List<string> currTexts = new List<string>();

        /// <summary>
        /// Item stats of currently open category; primarily for weapons.
        /// </summary>
        private List<string> currStats = new List<string>();

        /// <summary>
        /// Item costs of currently open category.
        /// </summary>
        private List<int> currCosts = new List<int>();

        /// <summary>
        /// Items not bought of currently open category.
        /// </summary>
        private List<bool> currNotBought = new List<bool>();

        /// <summary>
        /// Reference to currently displayed item prefab.
        /// </summary>
        private GameObject currPrefab;

        /// <summary>
        /// Item prefabs of currently open category.
        /// </summary>
        private List<GameObject> currPrefabs = new List<GameObject>();

        /// <summary>
        /// State of whether an item is currently open.
        /// </summary>
        private bool itemOpen;

        /// <summary>
        /// State of whether a category is currently open.
        /// </summary>
        private bool categoryOpen;

        [Header("Lighting")]
        public Light[] lights;

        public LightSettings[] skinLightSettings;

        private LightSettings defaultLightSettings;

        /// <summary>
        /// Disable game object near Shop area.
        /// </summary>
        private void OnEnable()
        {
            armadaPreviewGO.SetActive(false);
        }

        // Find IridiumSystem.
        private void Awake()
        {
            reference = FindObjectOfType<ReferenceManager>();
            iridiumSystem = FindObjectOfType<IridiumSystem>();
            gameSettings = reference.gameSettings;

            selectedSkin = PlayerPrefs.GetInt("defaultSkin", (int)defaultSkin);
            defaultLightSettings = new LightSettings(lights[0].color, lights[0].intensity);

            //int shopIndex = Random.Range(3, clips.Length);
            ////ASSUMES FIRST VISIT TO THE SHOP
            //if (iridiumSystem.GetIridiumAmount() < 0)
            //{
            //    zenaSource.PlayOneShot(clips[2]);
            //}

            ////ASSUMES SUBSEQUENT VISIT TO SHOP
            ////NEED TO ADD RANDOM RANGE CHOOSER
            //else if (iridiumSystem.GetIridiumAmount() > 0)
            //{
            //    zenaSource.PlayOneShot(clips[shopIndex]);
            //}
        }

        /// <summary>
        /// Create item buttons as children of Content and destroy previous buttons.
        /// </summary>
        private void GenerateContent()
        {
            for (int i = 0; i < contentGO.transform.childCount; i++)
            {
                Destroy(contentGO.transform.GetChild(i).gameObject);
            }

            List<int> removalIndices = new List<int>();
            for (int i = 0; i < currNames.Count; i++)
            {
                if (currCosts[i] == 0)
                {
                    removalIndices.Add(i);
                }
            }

            foreach (int index in removalIndices)
            {
                Debug.Log("Removed: " + currNames[index]);
                currNames.RemoveAt(index);
                currDisplayNames.RemoveAt(index);
                currTexts.RemoveAt(index);
                currCosts.RemoveAt(index);
                currNotBought.RemoveAt(index);
                currPrefabs.RemoveAt(index);

                if (currCategory == "Weapons")
                {
                    currStats.RemoveAt(index);
                }
            }

            int items;
            bool tooFewEntries;
            if (currNames.Count <= numFittableButtons)
            {
                items = numFittableButtons + 1;
                tooFewEntries = true;
                scrollBar.gameObject.GetComponent<BoxCollider>().enabled = false;
                verticalSlider.gameObject.SetActive(false);
            }
            else
            {
                items = currNames.Capacity;
                tooFewEntries = false;
                scrollBar.gameObject.GetComponent<BoxCollider>().enabled = true;
                verticalSlider.gameObject.SetActive(true);
            }

            for (int i = 0; i < items; ++i)
            {
                if (i >= currDisplayNames.Count && tooFewEntries)
                {
                    GameObject itemButtonEmpty =
                        Instantiate(itemButtonEmptyPrefab, contentGO.transform);
                    itemButtonEmpty.SetActive(false);
                }
                else
                {
                    GameObject itemButton = Instantiate(itemButtonPrefab, contentGO.transform);
                    itemButton.GetComponent<ShopItemButton>().id = i;
                    itemButton.GetComponent<ShopItemButton>().shopMenu = this;
                    itemButton.GetComponent<UIHover>().source = source;
                    itemButton.transform.Find("Name").gameObject.GetComponent<Text>().text =
                        currDisplayNames[i];
                }
            }
        }

        /// <summary>
        /// Attempt to purchase currently selected item.
        /// </summary>
        public void BuyItem()
        {
            /* NEED TO ADD SOUNDS INTO THE ARRAY THEN UNCOMMENT
             * SOUNDS: (ACCORDING TO DIALOGUE DOC)
             *      TAKE THIS
             *      WONDERFUL
             *      A NICE REWARD
             *      YOU'LL LIKE THAT ONE
             *      
             * Don't forget to actually do work...
             */
            //int purchaseSound = Random.Range(2, clips.Length);
            if (iridiumSystem.PayIridium(currCosts[currItemId]))
            {
                //zenaSource.PlayOneShot(clips[purchaseSound]);
                source.PlayOneShot(reference.menuSounds.shopPurchaseSound);
                iridiumSystem.UnlockItem(currCategory, currNames[currItemId]);
                currNotBought[currItemId] = false;
                purchaseSection.SetActive(false);

                if (currCategory == "Weapons")
                {
                    purchased.SetActive(true);
                }
                else if (currCategory == "Skins")
                {
                    setDefault.SetActive(true);
                }
                iridiumAmount.text = iridiumSystem.GetIridiumAmount().ToString();
            }
        }

        /// <summary>
        /// Set category item as the default item in that category (primarily meant for skins.)
        /// </summary>
        public void SetDefault()
        {
            switch (currCategory)
            {
                case "Skins":
                    selectedSkin = currItemId;
                    PlayerPrefs.SetInt("defaultSkin", selectedSkin);
                    gameSettings.selectedSkin = (GameSettings.Skin)selectedSkin;
                    setDefault.SetActive(false);
                    break;
                default:
                    Debug.Log("Unsupported use of set default in shop menu.");
                    break;
            }
        }

        /// <summary>
        /// Back button pressed. If a category or item is open, close it. Else, transition
        /// to back menu.
        /// </summary>
        public void PressBack()
        {
            source.PlayOneShot(reference.menuSounds.menuButtonSelectSound);

            if (itemOpen)
            {
                CloseItem();
            }
            else if (categoryOpen)
            {
                CloseCategory();
            }
            else
            {
                armadaPreviewGO.SetActive(true);
                FindObjectOfType<RoomTransport>().Transport(backMenuTransform, gameObject,
                                                            backMenuGO);
            }

            previewLighting.SetActive(false);
        }

        /// <summary>
        /// Open item view and fill item name and text with corresponding values.
        /// </summary>
        /// <param name="itemId"> Index of selected item. </param>
        public void OpenItem(int itemId)
        {
            source.PlayOneShot(reference.menuSounds.menuButtonSelectSound);

            menuTitle.SetActive(false);
            scrollView.SetActive(false);
            itemSection.SetActive(true);
            iridiumSection.SetActive(true);

            if (currNotBought[itemId])
            {
                purchaseSection.SetActive(true);

                Color tempColor = buyButton.GetComponent<Image>().color;

                if (iridiumSystem.GetIridiumAmount() < currCosts[itemId])
                {
                    buyButton.GetComponent<BoxCollider>().enabled = false;
                    tempColor.a = 0.2f;
                    buyButton.GetComponent<Image>().color = tempColor;
                    tempColor = buyButton.GetComponentInChildren<Text>().color;
                    tempColor.a = 0.2f;
                    buyButton.GetComponentInChildren<Text>().color = tempColor;
                }
                else
                {
                    buyButton.GetComponent<BoxCollider>().enabled = true;
                    tempColor.a = 1;
                    buyButton.GetComponent<Image>().color = tempColor;
                    tempColor = buyButton.GetComponentInChildren<Text>().color;
                    tempColor.a = 1;
                    buyButton.GetComponentInChildren<Text>().color = tempColor;
                }
            }
            else
            {
                purchaseSection.SetActive(false);

                if (currCategory == "Weapons")
                {
                    purchased.SetActive(true);
                }
                else if (currCategory == "Skins" && itemId != selectedSkin)
                {
                    setDefault.SetActive(true);
                }
            }

            foreach (GameObject uiCover in uiCovers)
            {
                uiCover.SetActive(false);
            }

            foreach (GameObject categoryButton in categoryButtons)
            {
                categoryButton.SetActive(false);
            }

            itemName.text = currDisplayNames[itemId];

            if (currCategory == "Weapons")
            {
                itemDescription.text = currTexts[itemId] + "\n"
                                       + currStats[itemId];
            }
            else
            {
                itemDescription.text = currTexts[itemId];
            }

            itemCost.text = currCosts[itemId].ToString();
            iridiumAmount.text = iridiumSystem.GetIridiumAmount().ToString();
            previewLighting.SetActive(true);
            currPrefab = Instantiate(currPrefabs[itemId], itemPrefabPoint);

            currItemId = itemId;
            itemOpen = true;

            if (currCategory.Equals("Skins"))
            {
                foreach (Light l in lights)
                {
                    l.color = skinLightSettings[currItemId].color;
                    l.intensity = skinLightSettings[currItemId].intensity;
                }
            }
        }

        /// <summary>
        /// Close item view and return to category view.
        /// </summary>
        private void CloseItem()
        {
            menuTitle.SetActive(true);

            itemSection.SetActive(false);
            purchaseSection.SetActive(false);
            iridiumSection.SetActive(false);
            if (currCategory.Equals("Weapons"))
            {
                purchased.SetActive(false);
            }
            else if (currCategory.Equals("Skins"))
            {
                setDefault.SetActive(false);
            }
            scrollView.SetActive(true);

            foreach (GameObject uiCover in uiCovers)
            {
                uiCover.SetActive(true);
            }

            foreach (GameObject categoryButton in categoryButtons)
            {
                categoryButton.SetActive(true);
            }

            Destroy(currPrefab);
            previewLighting.SetActive(false);

            itemOpen = false;
        }

        /// <summary>
        /// Set variables tracking currently open category.
        /// </summary>
        /// <param name="category"> Name of category </param>
        private void SetCurrCategory(string category)
        {
            switch (category)
            {
                case "Weapons":
                    currCategory = category;
                    currNames = iridiumSystem.GetItemNames(category);
                    currDisplayNames = iridiumSystem.GetItemDisplayNames(category);
                    currTexts = iridiumSystem.GetItemTexts(category);
                    currStats = new List<string>(weaponStats);
                    currCosts = iridiumSystem.GetItemCosts(category);
                    currNotBought = iridiumSystem.GetItemsLocked(category);
                    currPrefabs = new List<GameObject>(weaponPrefabs);
                    break;
                case "Skins":
                    currCategory = category;
                    currNames = iridiumSystem.GetItemNames(category);
                    currDisplayNames = iridiumSystem.GetItemDisplayNames(category);
                    currTexts = iridiumSystem.GetItemTexts(category);
                    currCosts = iridiumSystem.GetItemCosts(category);
                    currNotBought = iridiumSystem.GetItemsLocked(category);
                    currPrefabs = new List<GameObject>(skinPrefabs);
                    break;
                default:
                    Debug.Log("ERROR: Bestiary menu category could not be identified.");
                    break;
            }
        }

        /// <summary>
        /// Open the category specified by parameter string.
        /// </summary>
        /// <param name="category"> Name of category to open. </param>
        public void OpenCategory(string category)
        {
            source.PlayOneShot(reference.menuSounds.menuButtonSelectSound);

            menuTitle.GetComponent<Text>().text = category;
            scrollView.SetActive(true);
            SetCurrCategory(category);
            GenerateContent();
            scrollBar.value = 1;

            foreach (GameObject buttonGO in categoryButtons)
            {
                if (buttonGO.name.Contains(category))
                {
                    buttonGO.GetComponent<UIHover>().Select();
                }
                else
                {
                    buttonGO.GetComponent<UIHover>().EndSelect();
                }
            }

            categoryOpen = true;
        }

        /// <summary>
        /// Close currently open category.
        /// </summary>
        private void CloseCategory()
        {
            if (currCategory.Equals("Shop"))
            {
                foreach (Light l in lights)
                {
                    l.color = defaultLightSettings.color;
                    l.intensity = defaultLightSettings.intensity;
                }
            }

            menuTitle.GetComponent<Text>().text = "Shop";
            scrollView.SetActive(false);

            foreach (GameObject buttonGO in categoryButtons)
            {
                if (buttonGO.name.Contains(currCategory))
                {
                    buttonGO.GetComponent<UIHover>().EndSelect();
                    break;
                }
            }

            categoryOpen = false;
        }
    }
}