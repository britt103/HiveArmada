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
        public bool CheckMultipleWeaponsUnlocked()
        {
            int count = 0;
            foreach (bool locked in iridiumData.weaponsLocked)
            {
                if (!locked)
                {
                    count++;
                }
            }
            
            if (count > 1)
            {
                return true;
            }
            else
            {
                return false;
            }
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
        /// Returns a list of all item names in specified iridiumData category.
        /// </summary>
        /// <returns>List of item name strings.</returns>
        public List<string> GetLockedItemNames(string category)
        {
            List<string> itemNames = new List<string>();
            string[] iridiumDataNames;
            bool[] iridiumDataLocked;

            switch (category)
            {
                case "Weapons":
                    iridiumDataNames = iridiumData.weaponNames;
                    iridiumDataLocked = iridiumData.weaponsLocked;
                    break;
                default:
                    Debug.Log("ERROR: Item category could not be identified.");
                    iridiumDataNames = new string[0];
                    iridiumDataLocked = new bool[0];
                    break;
            }

            for (int i = 0; i < iridiumDataNames.Length; i++)
            {
                if (!iridiumDataLocked[i])
                {
                    itemNames.Add(iridiumDataNames[i]);
                }
            }

            return itemNames;
        }
        /// <summary>
        /// Returns a list of all item texts in specified iridiumData category.
        /// </summary>
        /// <returns>List of item name strings.</returns>
        public List<string> GetLockedItemTexts(string category)
        {
            List<string> itemTexts = new List<string>();
            string[] iridiumDataTexts;
            bool[] iridiumDataLocked;

            switch (category)
            {
                case "Weapons":
                    iridiumDataTexts = iridiumData.weaponTexts;
                    iridiumDataLocked = iridiumData.weaponsLocked;
                    break;
                default:
                    Debug.Log("ERROR: Item category could not be identified.");
                    iridiumDataTexts = new string[0];
                    iridiumDataLocked = new bool[0];
                    break;
            }

            for (int i = 0; i < iridiumDataTexts.Length; i++)
            {
                if (!iridiumDataLocked[i])
                {
                    itemTexts.Add(iridiumDataTexts[i]);
                }
            }

            return itemTexts;
        }


        /// <summary>
        /// Returns a list of all item costs in specified iridiumData category.
        /// </summary>
        /// <returns>List of item cost integers.</returns>
        public List<int> GetLockedItemCosts(string category)
        {
            List<int> itemCosts = new List<int>();
            int[] iridiumDataCosts;
            bool[] iridiumDataLocked;

            switch (category)
            {
                case "Weapons":
                    iridiumDataCosts = iridiumData.weaponCosts;
                    iridiumDataLocked = iridiumData.weaponsLocked;
                    break;
                default:
                    Debug.Log("ERROR: Item category could not be identified.");
                    iridiumDataCosts = new int[0];
                    iridiumDataLocked = new bool[0];
                    break;
            }

            for (int i = 0; i < iridiumDataCosts.Length; i++)
            {
                if (!iridiumDataLocked[i])
                {
                    itemCosts.Add(iridiumDataCosts[i]);
                }
            }

            return itemCosts;
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
