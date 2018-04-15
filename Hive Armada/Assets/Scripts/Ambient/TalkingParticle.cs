//=============================================================================
// 
// Perry Sidler
// 1831784
// sidle104@mail.chapman.edu
// CPSC-440-01
// Group Project
// 
// 
// 
//=============================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hive.Armada.Game;
using SubjectNerd.Utilities;

namespace Hive.Armada.Ambient
{
    public class TalkingParticle : MonoBehaviour
    {
        public ReferenceManager reference;

        public GameObject smallParticle;

        public GameObject largeParticle;

        [Reorderable("Position", false)]
        public Transform[] positions;

        public bool IsSpeaking { get; private set; }

        private void Awake()
        {
            if (reference == null)
            {
                Debug.LogError(GetType().Name + " - Reference is null.");
            }
        }

        private void OnDialogueComplete()
        {
            IsSpeaking = false;
        }

        public void Speak(AudioClip clip)
        {
            reference.dialoguePlayer.EnqueueDialogue(gameObject, clip);
            IsSpeaking = true;
        }

        public void MovePosition(string nextMenu)
        {
            if (nextMenu.Contains("Main"))
            {
                smallParticle.SetActive(true);
                largeParticle.SetActive(false);
                transform.position = positions[0].position;
            }
            else if (nextMenu.Contains("Bestiary"))
            {
                smallParticle.SetActive(false);
                largeParticle.SetActive(true);
                transform.position = positions[1].position;
            }
            else if (nextMenu.Contains("Shop"))
            {
                smallParticle.SetActive(false);
                largeParticle.SetActive(true);
                transform.position = positions[2].position;
            }
            else
            {
                Debug.LogError(GetType().Name + " - Menu isn't \"Main\", \"Bestiary\" or \"Shop\". Menu = \"" + nextMenu + "\"");
            }
        }
    }
}
