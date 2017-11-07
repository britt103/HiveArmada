////=============================================================================
//// 
//// Perry Sidler
//// 1831784
//// sidle104@mail.chapman.edu
//// CPSC-340-01 & CPSC-344-01
//// Group Project
//// 
//// [DESCRIPTION]
//// 
////=============================================================================

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace Hive.Armada.Player.Guns
//{
//    public class Switcher : MonoBehaviour
//    {
//        public ShipController shipController;
//        public ShipController.GunTypes[] guns =
//        {
//            ShipController.GunTypes.Laser
//        };

//        public int currentGun = 0;
//        public LaserGun laserGun;
//        public Minigun minigun;
//        //public PlasmaGun plasmaGun;
//        //public RocketPod rocketPod;

//        // Use this for initialization
//        void Start()
//        {

//        }

//        /// <summary>
//        /// Switches to the next gun, if possible.
//        /// </summary>
//        public void NextGun()
//        {
//            int current = currentGun;

//            if (currentGun + 1 < guns.Length)
//            {
//                ++currentGun;
//            }
//            else
//            {
//                currentGun = 0;
//            }
            
//            SwitchGun(current);
//        }

//        /// <summary>
//        /// Switches to the previous gun, if possible.
//        /// </summary>
//        public void PreviousGun()
//        {
//            int current = currentGun;

//            if (currentGun - 1 >= 0)
//            {
//                --currentGun;
//            }
//            else
//            {
//                currentGun = guns.Length - 1;
//            }

//            SwitchGun(current);
//        }

//        /// <summary>
//        /// Checks to see if we are actually switching guns or not
//        /// </summary>
//        /// <param name="current"> The index of our currently activated gun </param>
//        private void SwitchGun(int current)
//        {
//            if (current != currentGun)
//            {
//                SetGun(guns[current], false);
//                SetGun(guns[currentGun], true);
//            }
//        }

//        /// <summary>
//        /// Enables/disables the GameObject for the corresponding gun
//        /// </summary>
//        /// <param name="gunType"> The gun type to set </param>
//        /// <param name="enable"> Whether or not to enable or disable the gun </param>
//        private void SetGun(ShipController.GunTypes gunType, bool enable)
//        {
//            switch (gunType)
//            {
//                case ShipController.GunTypes.Laser:
//                    if (laserGun != null)
//                        laserGun.gameObject.SetActive(enable);
//                    else
//                        Debug.Log("ERROR - laserGun is null");

//                    if (Utility.isDebug)
//                        Debug.Log("Laser gun is now " + (enable ? "enabled" : "disabled") + "!");
//                    break;
//                case ShipController.GunTypes.Minigun:
//                    if (minigun != null)
//                        minigun.gameObject.SetActive(enable);
//                    else
//                        Debug.Log("ERROR - minigun is null");

//                    if (Utility.isDebug)
//                        Debug.Log("Minigun is now " + (enable ? "enabled" : "disabled") + "!");
//                    break;
//                case ShipController.GunTypes.Plasma:
//                    if (Utility.isDebug)
//                        Debug.Log("The plasma gun isn't implemented yet.");
//                    break;
//                case ShipController.GunTypes.RocketPod:
//                    if (Utility.isDebug)
//                        Debug.Log("The rocket pods aren't implemented yet.");
//                    break;
//                default:
//                    Debug.Log("ERROR - This gun isn't in the switch in Switcher.cs! gunType = " + gunType);
//                    break;
//            }
//        }
//    }
//}
