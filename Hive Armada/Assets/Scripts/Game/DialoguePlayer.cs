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

namespace Hive.Armada.Game
{
    /// <summary>
    /// 
    /// </summary>
    public class DialoguePlayer : MonoBehaviour
    {
        public struct Dialogue
        {
            public GameObject sender;
            public AudioClip[] dialogueClips;
            public int dialogueCount;

            public Dialogue(GameObject sender, AudioClip[] clips)
            {
                this.sender = sender;
                dialogueClips = clips;
                dialogueCount = dialogueClips.Length;
            }
        }

        public AudioSource source;
        public float lineDelay;
        public float clipDelay;
        private Queue<Dialogue> dialogueQueue;
        private Coroutine dialogueCoroutine;

        /// <summary>
        /// Adds an array of dialogue AudioClip's to the dialogue queue.
        /// </summary>
        /// <param name="sender"> The GameObject that is sending the dialogue </param>
        /// <param name="dialogueClips"> Array of AudioClip's to play </param>
        public void EnqueueDialogue(GameObject sender, AudioClip[] dialogueClips)
        {
            dialogueQueue.Enqueue(new Dialogue(sender, dialogueClips));

            if (dialogueCoroutine == null)
            {
                dialogueCoroutine = StartCoroutine(PlayDialogue());
            }
        }

        /// <summary>
        /// Adds a dialogue AudioClip to the dialogue queue.
        /// </summary>
        /// <param name="sender"> The GameObject that is sending the dialogue </param>
        /// <param name="dialogueClip"> The AudioClip to play </param>
        public void EnqueueDialogue(GameObject sender, AudioClip dialogueClip)
        {
            dialogueQueue.Enqueue(new Dialogue(sender, new AudioClip[]{dialogueClip}));

            if (dialogueCoroutine == null)
            {
                dialogueCoroutine = StartCoroutine(PlayDialogue());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private IEnumerator PlayDialogue()
        {
            while (dialogueQueue.Count > 0)
            {
                Dialogue dialogue = dialogueQueue.Dequeue();
                foreach (AudioClip clip in dialogue.dialogueClips)
                {
                    if (source.isPlaying)
                    {
                        yield return new WaitWhile(() => source.isPlaying);
                    }

                    source.PlayOneShot(clip);

                    yield return new WaitWhile(() => source.isPlaying);

                    yield return new WaitForSeconds(clipDelay);
                }

                yield return new WaitForSeconds(lineDelay);

                dialogue.sender.SendMessage("OnDialogueComplete", SendMessageOptions.DontRequireReceiver);
            }

            if (dialogueQueue.Count > 0)
            {
                dialogueCoroutine = StartCoroutine(PlayDialogue());
                yield break;
            }

            dialogueCoroutine = null;
        }
    }
}
