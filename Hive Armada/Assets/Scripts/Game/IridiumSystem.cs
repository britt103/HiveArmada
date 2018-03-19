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
        /// Return amount of iridium gained from wave Iridium shootables.
        /// </summary>
        /// <returns></returns>
        public int GetShootablesAmount()
        {
            return iridiumAmount - iridiumData.amount;
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
        /// Check if multiple weapons have been unlocked.
        /// </summary>
        /// <returns>State of whether multiple weapons have been unlocked.</returns>
        public bool CheckAnyWeaponsUnlocked()
        {
            foreach (bool locked in iridiumData.weaponsLocked)
            {
                if (!locked)
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
        /// Check if weapon is part of iridium system (used for weapons that are not bought
        /// through shop).
        /// </summary>
        /// <param name="weaponName">Name of weapon to check for.</param>
        /// <returns>True if weapon found, otherwise false.</returns>
        public bool CheckWeaponIsPresent(string weaponName)
        {
            return iridiumData.weaponNames.Contains(weaponName);
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
                default:
                    Debug.Log("ERROR: Item category could not be identified.");
                    itemsLocked = new List<bool>();
                    break;
            }

            return itemsLocked;
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
                        if (name == iridiumData.weaponNames[i])
                        {
                            iridiumData.weaponsLocked[i] = false;
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
