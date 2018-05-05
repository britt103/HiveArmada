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

using Hive.Armada.Game;
using SubjectNerd.Utilities;
using UnityEngine;

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

        public bool isSmall;

        private void Awake()
        {
            if (reference == null)
            {
                Debug.LogError(GetType().Name + " - Reference is null.");
            }

            if (enable)
            {
                isSmall = true;
                smallParticle.SetActive(true);
            }
        }

        private void OnDialogueComplete()
        {
            IsSpeaking = false;
            small.StopTalking();
            large.StopTalking();
        }

        public void Speak(AudioClip clip, bool smallTalk)
        {
            float talkTime = clip.length * 0.9f;
            
            if (smallTalk)
                small.Talk(talkTime);
            else
                large.Talk(talkTime);
            
            reference.dialoguePlayer.EnqueueDialogue(gameObject, clip);
            IsSpeaking = true;
        }

        public void StopSpeaking()
        {
            reference.dialoguePlayer.StopDialogue();
            small.StopTalking();
            large.StopTalking();
        }

        public void MovePosition(string nextMenu)
        {
            if (!enable)
                return;
            
            if (nextMenu.Contains("Extras"))
            {
                isSmall = false;
                // Debug.Log(nextMenu);
                smallParticle.SetActive(true);
                smallParticle.transform.position = positions[0].position;
                largeParticle.SetActive(false);
            }
            else if (nextMenu.Contains("Bestiary"))
            {
                isSmall = false;
                // Debug.Log(nextMenu);
                smallParticle.SetActive(false);
                largeParticle.transform.position = positions[1].position;
                largeParticle.SetActive(true);
            }
            else if (nextMenu.Contains("Shop"))
            {
                isSmall = true;
                // Debug.Log(nextMenu);
                smallParticle.SetActive(false);
                largeParticle.transform.position = positions[2].position;
                largeParticle.SetActive(true);
            }
            else
            {
                Debug.LogError(GetType().Name +
                               " - Menu isn't \"Main\", \"Bestiary\" or \"Shop\". Menu = \"" +
                               nextMenu + "\"");
            }
        }
    }
}