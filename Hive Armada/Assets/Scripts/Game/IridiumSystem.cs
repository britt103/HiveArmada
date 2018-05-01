//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-440-1
// Group Project
//
// IridiumSystem tracks the amount of Iridium the player has accumulated. The 
// amount decreases when Iridium is spent on unlockables. NOTE: meant to 
// persist between scene transitions.
//
//=============================================================================

using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Hive.Armada.Game
{
    public class IridiumSystem : MonoBehaviour
    {
        /// <summary>
        /// Total Iridium currently possesed by player.
        /// </summary>
        private int iridiumAmount;

        /// <summary>
        /// Amount of iridium gained from spawned shootables.
        /// </summary>
        private int shootablesIridiumObtained = 0;

        /// <summary>
        /// Amount of iridium of spawned shootables.
        /// </summary>
        private int shootablesIridiumSpawned = 0;

        /// <summary>
        /// Copy of data from Iridium.txt.
        /// </summary>
        private IridiumData iridiumData;

        /// <summary>
        /// Read in data from Iridium.txt.
        /// </summary>
        void Awake()
        {
            ReadIridiumFile();
        }

        /// <summary>
        /// Assign iridiumData values using current values.
        /// </summary>
        private void SetData()
        {
            iridiumData.amount = iridiumAmount;
        }

        /// <summary>
        /// Write to Iridium.txt.
        /// </summary>
        public void WriteIridiumFile()
        {
            SetData();
            File.WriteAllText(@"Iridium.txt", JsonUtility.ToJson(iridiumData, true));
        }

        /// <summary>
        /// Read from Iridium.txt and assign iridiumData.
        /// </summary>
        public void ReadIridiumFile()
        {
            string jsonString = File.ReadAllText(@"Iridium.txt");
            iridiumData = JsonUtility.FromJson<IridiumData>(jsonString);
            iridiumAmount = iridiumData.amount;
        }

        /// <summary>
        /// Return iridiumAmount.
        /// </summary>
        /// <returns>Current amount of Iridium.</returns>
        public int GetIridiumAmount()
        {
            return iridiumAmount;
        }

        /// <summary>
        /// Add iridium to shootables spawned amonut.
        /// </summary>
        /// <param name="amount">Amount of iridium to add.</param>
        public void AddSpawnedShootablesIridium(int amount)
        {
            shootablesIridiumSpawned += amount;
        }

        /// <summary>
        /// Add iridium to shootables obtained amount.
        /// </summary>
        /// <param name="amount">Amount of iridium to add.</param>
        public void AddObtainedShootablesIridium(int amount)
        {
            shootablesIridiumObtained += amount;
        }

        /// <summary>
        /// Return amount of iridium of spawned shootables.
        /// </summary>
        /// <returns>Integer amount of iridium.</returns>
        public int GetSpawnedShootablesAmount()
        {
            return shootablesIridiumSpawned;
        }

        /// <summary>
        /// Return amount of iridium gained from spawned shootables.
        /// </summary>
        /// <returns>Integer amount of iridium.</returns>
        public int GetObtainedShootablesAmount()
        {
            return shootablesIridiumObtained;
        }

        /// <summary>
        /// Add Iridium amount to total.
        /// </summary>
        /// <param name="amount">Amount to add to iridiumAmount.</param>
        public void AddIridium(int amount)
        {
            iridiumAmount += amount;
        }

        /// <summary>
        /// Set values of shootables amounts to 0.
        /// </summary>
        public void ResetShootablesAmounts()
        {
            shootablesIridiumSpawned = 0;
            shootablesIridiumObtained = 0;
        }

        /// <summary>
        /// Attempt Iridium payment.
        /// </summary>
        /// <param name="payment">Payment cost.</param>
        /// <returns>State of whether payment was successful.</returns>
        public bool PayIridium(int payment)
        {
            if (payment > iridiumAmount)
            {
                return false;
            }
            else
            {
                iridiumAmount -= payment;
                //WriteIridiumFile();
                return true;
            }
        }

        /// <summary>
        /// Check if multiple weapons have been unlocked. Fixed weapons like Laser ignored.
        /// </summary>
        /// <returns>State of whether multiple weapons have been unlocked.</returns>
        public bool CheckAnyWeaponsUnlocked()
        {
            for (int i = 0; i < iridiumData.weaponsLocked.Length; i++)
            {
                if (!iridiumData.weaponsLocked[i] && iridiumData.weaponCosts[i] > 0)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Check if named weapon has been unlocked. If weapon name not found, return false.
        /// </summary>
        /// <param name="weaponName">Name of weapon to check status of.</param>
        /// <returns>State of whether weapon has been unlocked.</returns>
        public bool CheckWeaponUnlocked(string weaponName)
        {
            for (int i = 0; i < iridiumData.weaponNames.Length; i++)
            {
                if (weaponName == iridiumData.weaponNames[i])
                {
                    return !iridiumData.weaponsLocked[i];
                }
            }
            return false;
        }

        /// <summary>
        /// Check if named skin has been unlocked. If skin name not found, return false.
        /// </summary>
        /// <param name="skinName">Name of skin to check status of.</param>
        /// <returns>State of whether skin has been unlocked.</returns>
        public bool CheckSkinUnlocked(string skinName)
        {
            for (int i = 0; i < iridiumData.skinNames.Length; i++)
            {
                if (skinName == iridiumData.skinNames[i])
                {
                    return !iridiumData.skinsLocked[i];
                }
            }
            return false;
        }

        /// <summary>
        /// Check if named weapon is new. If weapon name not found, return false.
        /// </summary>
        /// <param name="weaponName">Name of weapon to check status of.</param>
        /// <returns>State of whether weapon is new.</returns>
        public bool CheckWeaponNew(string weaponName)
        {
            for (int i = 0; i < iridiumData.weaponNames.Length; i++)
            {
                if (weaponName == iridiumData.weaponNames[i])
                {
                    return !iridiumData.weaponsNew[i];
                }
            }
            return false;
        }

        /// <summary>
        /// Check if named skin is new. If skin name not found, return false.
        /// </summary>
        /// <param name="skinName">Name of skin to check status of.</param>
        /// <returns>State of whether skin is new.</returns>
        public bool CheckSkinNew(string skinName)
        {
            for (int i = 0; i < iridiumData.skinNames.Length; i++)
            {
                if (skinName == iridiumData.skinNames[i])
                {
                    return !iridiumData.skinsNew[i];
                }
            }
            return false;
        }

        /// <summary>
        /// Returns a list of all item names in specified iridiumData category.
        /// </summary>
        /// <returns>List of item name strings.</returns>
        public List<string> GetItemNames(string category)
        {
            List<string> itemNames = new List<string>();

            switch (category)
            {
                case "Weapons":
                    itemNames = iridiumData.weaponNames.ToList();
                    break;
                case "Skins":
                    itemNames = iridiumData.skinNames.ToList();
                    break;
                default:
                    Debug.Log("ERROR: Item category could not be identified.");
                    itemNames = new List<string>();
                    break;
            }

            return itemNames;
        }

        /// <summary>
        /// Returns a list of all item display names in specified iridiumData category.
        /// </summary>
        /// <returns>List of item display name strings.</returns>
        public List<string> GetItemDisplayNames(string category)
        {
            List<string> itemDisplayNames = new List<string>();

            switch (category)
            {
                case "Weapons":
                    itemDisplayNames = iridiumData.weaponDisplayNames.ToList();
                    break;
                case "Skins":
                    itemDisplayNames = iridiumData.skinDisplayNames.ToList();
                    break;
                default:
                    Debug.Log("ERROR: Item category could not be identified.");
                    itemDisplayNames = new List<string>();
                    break;
            }

            return itemDisplayNames;
        }

        /// <summary>
        /// Returns a list of all item texts in specified iridiumData category.
        /// </summary>
        /// <returns>List of item name strings.</returns>
        public List<string> GetItemTexts(string category)
        {
            List<string> itemTexts = new List<string>();

            switch (category)
            {
                case "Weapons":
                    itemTexts = iridiumData.weaponTexts.ToList();
                    break;
                case "Skins":
                    itemTexts = iridiumData.skinTexts.ToList();
                    break;
                default:
                    Debug.Log("ERROR: Item category could not be identified.");
                    itemTexts = new List<string>();
                    break;
            }

            return itemTexts;
        }


        /// <summary>
        /// Returns a list of all item costs in specified iridiumData category.
        /// </summary>
        /// <returns>List of item cost integers.</returns>
        public List<int> GetItemCosts(string category)
        {
            List<int> itemCosts = new List<int>();

            switch (category)
            {
                case "Weapons":
                    itemCosts = iridiumData.weaponCosts.ToList();
                    break;
                case "Skins":
                    itemCosts = iridiumData.skinCosts.ToList();
                    break;
                default:
                    Debug.Log("ERROR: Item category could not be identified.");
                    itemCosts = new List<int>();
                    break;
            }

            return itemCosts;
        }

        /// <summary>
        /// Returns a list of all items locked in the specified iridiumData category.
        /// </summary>
        /// <returns>List of item cost integers.</returns>
        public List<bool> GetItemsLocked(string category)
        {
            List<bool> itemsLocked = new List<bool>();

            switch (category)
            {
                case "Weapons":
                    itemsLocked = iridiumData.weaponsLocked.ToList();
                    break;
                case "Skins":
                    itemsLocked = iridiumData.skinsLocked.ToList();
                    break;
                default:
                    Debug.Log("ERROR: Item category could not be identified.");
                    itemsLocked = new List<bool>();
                    break;
            }

            return itemsLocked;
        }

        /// <summary>
        /// Returns a list of all items just unlocked in the specified iridiumData category.
        /// </summary>
        /// <returns>List of item cost integers.</returns>
        public List<bool> GetItemsNew(string category)
        {
            List<bool> itemsNew = new List<bool>();

            switch (category)
            {
                case "Weapons":
                    itemsNew = iridiumData.weaponsNew.ToList();
                    break;
                case "Skins":
                    itemsNew = iridiumData.skinsNew.ToList();
                    break;
                default:
                    Debug.Log("ERROR: Item category could not be identified.");
                    itemsNew = new List<bool>();
                    break;
            }

            return itemsNew;
        }

        /// <summary>
        /// Unlock iridium item.
        /// </summary>
        /// <param name="category">Name of category of item.</param>
        /// <param name="name">Name of item.</param>
        public void UnlockItem(string category, string name)
        {
            switch (category)
            {
                case "Weapons":
                    for (int i = 0; i < iridiumData.weaponNames.Length; i++)
                    {
                        if (name == iridiumData.weaponNames[i] && iridiumData.weaponsLocked[i])
                        {
                            iridiumData.weaponsLocked[i] = false;
                            iridiumData.weaponsNew[i] = true;
                            break;
                        }
                    }
                    WriteIridiumFile();
                    break;
                case "Skins":
                    for (int i = 0; i < iridiumData.skinNames.Length; i++)
                    {
                        if (name == iridiumData.skinNames[i] && iridiumData.skinsLocked[i])
                        {
                            iridiumData.skinsLocked[i] = false;
                            iridiumData.skinsNew[i] = true;
                        }
                    }
                    WriteIridiumFile();
                    break;
                default:
                    Debug.Log("ERROR: Item category could not be identified.");
                    break;
            }
        }

        /// <summary>
        /// Set item to no longer new after being seen by player.
        /// </summary>
        /// <param name="category">Category of the item.</param>
        /// <param name="name">Name of the item.</param>
        public void ViewItem(string category, string name)
        {
            switch (category)
            {
                case "Weapons":
                    for (int i = 0; i < iridiumData.weaponNames.Length; i++)
                    {
                        if (name == iridiumData.weaponNames[i] && iridiumData.weaponsNew[i])
                        {
                            iridiumData.weaponsNew[i] = false;
                        }
                    }
                    WriteIridiumFile();
                    break;
                case "Skins":
                    for (int i = 0; i < iridiumData.skinNames.Length; i++)
                    {
                        if (name == iridiumData.skinNames[i] && iridiumData.skinsLocked[i])
                        {
                            iridiumData.skinsLocked[i] = false;
                            iridiumData.skinsNew[i] = true;
                        }
                    }
                    WriteIridiumFile();
                    break;
                default:
                    Debug.Log("ERROR: Item category could not be identified.");
                    break;
            }
        }
    }
}
