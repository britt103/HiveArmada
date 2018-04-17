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
        public bool enable;

        [Space]
        public ReferenceManager reference;

        public GameObject smallParticle;

        public GameObject largeParticle;

        public ParticleFollow small;

        public ParticleFollow large;

        [Reorderable("Position", false)]
        public Transform[] positions;

        public bool IsSpeaking { get; private set; }

        private void Awake()
        {
            if (reference == null)
            {
                Debug.LogError(GetType().Name + " - Reference is null.");
            }

            if (enable)
            {
                smallParticle.SetActive(true);
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
            if (!enable)
            {
                return;
            }

            if (nextMenu.Contains("Extras"))
            {
                Debug.Log(nextMenu);
                smallParticle.SetActive(true);
                smallParticle.transform.position = positions[0].position;
                largeParticle.SetActive(false);
            }
            else if (nextMenu.Contains("Bestiary"))
            {
                Debug.Log(nextMenu);
                smallParticle.SetActive(false);
                largeParticle.transform.position = positions[1].position;
                largeParticle.SetActive(true);
            }
            else if (nextMenu.Contains("Shop"))
            {
                Debug.Log(nextMenu);
                smallParticle.SetActive(false);
                largeParticle.transform.position = positions[2].position;
                largeParticle.SetActive(true);
            }
            else
            {
                Debug.LogError(GetType().Name + " - Menu isn't \"Main\", \"Bestiary\" or \"Shop\". Menu = \"" + nextMenu + "\"");
            }
        }
    }
}
