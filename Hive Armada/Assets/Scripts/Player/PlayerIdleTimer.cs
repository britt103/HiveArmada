//=============================================================================
//
// Chad Johnson
// 1763718
// johns428@mail.chapman.edu
// CPSC-340-01 & CPSC-344-01
// Group Project
//
// PlayerIdleTimer tracks time during which the player is not firing. If the
// player is idle for a long enough continuous amount of time, the game is
// automically quit.
//
//=============================================================================

using Hive.Armada.Game;
using UnityEngine;

namespace Hive.Armada.Player
{
    /// <summary>
    /// Tracks time while player is idle.
    /// </summary>
    public class PlayerIdleTimer : MonoBehaviour
    {
        /// <summary>
        /// State of whether player is firing.
        /// </summary>
        private bool isIdle = true;

        /// <summary>
        /// State of whether player is currently being tracked.
        /// </summary>
        private bool isTracking = false;

        private bool timerStarted;

        /// <summary>
        /// Time until game quits.
        /// </summary>
        public float allowedIdleTime;

        /// <summary>
        /// Time since player was firing.
        /// </summary>
        private float currentIdleTime;

        /// <summary>
        /// Set initial value for currentIdleTimer using ResetTimer().
        /// </summary>
        void Start()
        {
            ResetTimer();
        }

        /// <summary>
        /// Decrease currentIdleTime while isIdle. Quit application when timer runs out.
        /// </summary>
        void Update()
        {
            if (timerStarted)
            {
                currentIdleTime -= Time.deltaTime;
                if (currentIdleTime <= 0)
                {
                    SetIsTracking(false);
                    SetIsIdle(false);
                    timerStarted = false;
                    FindObjectOfType<ReferenceManager>().sceneTransitionManager.TransitionOut("Menu Room");
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    timerStarted = true;
                    allowedIdleTime = 500.0f;
                }
            }
            
        }

        /// <summary>
        /// Set value of isIdle. ResetTimer() is new state is false.
        /// </summary>
        /// <param name="state">Boolean value to be assigned to isIdle.</param>
        public void SetIsIdle(bool state)
        {
            isIdle = state;
            if (!isIdle)
            {
                ResetTimer();
            }
        }

        /// <summary>
        /// Set currentIdleTime equal to allowedIdleTime.
        /// </summary>
        private void ResetTimer()
        {
            currentIdleTime = allowedIdleTime;
        }

        /// <summary>
        /// Set value of isTracking.
        /// </summary>
        /// <param name="state">Boolean value to be assigned to isTracking.</param>
        public void SetIsTracking(bool state)
        {
            isTracking = state;
        }
    }
}
