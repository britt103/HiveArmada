//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// StartMenu controls interactions with the Start Menu.
//
//=============================================================================

using UnityEngine;
using Hive.Armada.Game;

namespace Hive.Armada.Menus
{
    /// <summary>
    /// Controls interactions with Start Menu;
    /// </summary>
    public class StartMenu : MonoBehaviour
    {
        /// <summary>
        /// Reference to Menu Transition Manager.
        /// </summary>
        public MenuTransitionManager transitionManager;

        /// <summary>
        /// Reference to menu to go to when back is pressed.
        /// </summary>
        public GameObject backMenuGO;

        /// <summary>
        /// Reference to Loadout Menu.
        /// </summary>
        public GameObject loadoutGO;

        /// <summary>
        /// Reference to Shop Menu.
        /// </summary>
        public GameObject shopGO;

        /// <summary>
        /// Reference to player transform for Shop Menu.
        /// </summary>
        public Transform shopTransform;

        /// <summary>
        /// Reference to Menu Audio source.
        /// </summary>
		public AudioSource source;

        /// <summary>
        /// Clips to use with source.
        /// </summary>
    	public AudioClip[] clips;

        /// <summary>
        /// Reference to GameModeSelection.
        /// </summary>
        private GameSettings gameSettings;

        /// <summary>
        /// Variables used as a check to make sure audio
        /// doesn't play over itself
        /// </summary>
        private int soloNormalCounter = 0;
        private int soloInfiniteCounter = 0;
        private int backCounter = 0;

        /// <summary>
        /// Find references.
        /// </summary>
        private void Awake()
        {
            gameSettings = FindObjectOfType<GameSettings>();
        }

        /// <summary>
        /// Called by start button; navigates to Loadout Menu. Set game mode to SoloNormal.
        /// </summary>
        public void PressSoloNormal()
        {
			source.PlayOneShot(clips[0]);
            soloNormalCounter += 1;
            if(soloNormalCounter > 1)
            {
                source.Stop();
                source.PlayOneShot(clips[0]);
            }
            gameSettings.selectedGameMode = GameSettings.GameMode.SoloNormal;
            transitionManager.Transition(loadoutGO);
        }

        /// <summary>
        /// Called by start button; navigates to Loadout Menu. Set game mode to SoloInfinite.
        /// </summary>
        public void PressSoloInfinite()
        {
            source.PlayOneShot(clips[0]);
            soloInfiniteCounter += 1;
            if (soloInfiniteCounter > 1)
            {
                source.Stop();
                source.PlayOneShot(clips[0]);
            }
            gameSettings.selectedGameMode = GameSettings.GameMode.SoloInfinite;
            transitionManager.Transition(loadoutGO);
        }

        /// <summary>
        /// Called by shop button; navigates to Shop Menu.
        /// </summary>
        public void PressShop()
        {
            source.PlayOneShot(clips[0]);
            FindObjectOfType<RoomTransport>().Transport(shopTransform, gameObject, shopGO);
        }

        /// <summary>
        /// Back button pressed; navigates to Main Menu.
        /// </summary>
        public void PressBack()
        {
            source.PlayOneShot(clips[1]);
            backCounter += 1;
            if(backCounter > 1)
            {
                source.Stop();
                source.PlayOneShot(clips[1]);
            }
            transitionManager.Transition(backMenuGO);
        }
    }
}
